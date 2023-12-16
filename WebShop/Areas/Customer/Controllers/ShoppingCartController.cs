using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Shop.DataAccess.Repository;
using Shop.Models;
using Shop.Models.ViewModels;
using Shop.Utility;
using Stripe.BillingPortal;
using Stripe.Checkout;
using System.Security.Claims;

namespace WebShop.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	public class ShoppingCartController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		[BindProperty]
		public ShoppingCartVM shoppingCartVM { get; set; }
		public ShoppingCartController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
		{

			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			var items = _unitOfWork.ShoppingCarts.GetAll(includeProperties: "Product").Where(cart => cart.ApplicationUserId == userId).ToList();
			decimal total = (decimal)items?.Sum(item => item.Count * item.Product.Price);

			shoppingCartVM = new ShoppingCartVM()
			{
				Items = items,
				OrderHeader = new OrderHeader() { OrderTotal = total }
			};

			return View(shoppingCartVM);
		}
		public IActionResult IncrementProduct(int productId)
		{
			ShoppingCart cart = _unitOfWork.ShoppingCarts.Find(cart => cart.ProductId == productId);
			cart.Count++;
			_unitOfWork.ShoppingCarts.Update(cart);
			_unitOfWork.Complete();
			return RedirectToAction(nameof(Index));
		}
		public IActionResult DecrementProduct(int productId)
		{
			ShoppingCart cart = _unitOfWork.ShoppingCarts.Find(cart => cart.ProductId == productId);
			cart.Count--;
			if (cart.Count == 0)
			{
				_unitOfWork.ShoppingCarts.Delete(cart);
			} else
			{
				_unitOfWork.ShoppingCarts.Update(cart);
			}
			_unitOfWork.Complete();
			return RedirectToAction(nameof(Index));
		}
		public IActionResult RemoveProduct(int productId)
		{
			ShoppingCart cart = _unitOfWork.ShoppingCarts.Find(cart => cart.ProductId == productId);
			_unitOfWork.ShoppingCarts.Delete(cart);
			_unitOfWork.Complete();
			return RedirectToAction(nameof(Index));
		}
		public IActionResult Summary()
		{

			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			var items = _unitOfWork.ShoppingCarts.GetAll(includeProperties: "Product").Where(cart => cart.ApplicationUserId == userId).ToList();
			decimal total = (decimal)items?.Sum(item => item.Count * item.Product.Price);

			var user = _unitOfWork.ApplicationUsers.Find(u => u.Id == userId);

			shoppingCartVM = new ShoppingCartVM()
			{
				Items = items,
				OrderHeader = new OrderHeader()
				{
					OrderTotal = total,
					ApplicationUser = user,
					PhoneNumber = user.PhoneNumber,
					PostalCode = user.PostalCode,
					State = user.State,
					City = user.City,
					StreetAddress = user.StreetAddress,
					Name = user.Name
				}
			};

			return View(shoppingCartVM);
		}

		[HttpPost]
		[ActionName("Summary")]
		public IActionResult SummaryPOST()
		{

			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			var items = _unitOfWork.ShoppingCarts.GetAll(includeProperties: "Product").Where(cart => cart.ApplicationUserId == userId).ToList();
			decimal total = (decimal)items?.Sum(item => item.Count * item.Product.Price);

			var user = _unitOfWork.ApplicationUsers.Find(u => u.Id == userId);

			shoppingCartVM.OrderHeader.ShippingDate = DateTime.Now;

			shoppingCartVM.OrderHeader.ApplicationUserId = userId;
			shoppingCartVM.Items = items;

			bool customerIsRegular = user.CompanyId.GetValueOrDefault() == 0;


			if (customerIsRegular)
			{
				shoppingCartVM.OrderHeader.PaymentStatus = "Pending";
				shoppingCartVM.OrderHeader.OrderStatus = "Pending";
			} else
			{
				shoppingCartVM.OrderHeader.PaymentStatus = "DelayedPayment";
				shoppingCartVM.OrderHeader.OrderStatus = "Approved";
			}
			_unitOfWork.OrderHeaders.Add(shoppingCartVM.OrderHeader);
			_unitOfWork.Complete();
			foreach (var item in shoppingCartVM.Items)
			{
				OrderDetail orderDetail = new OrderDetail
				{
					ProductId = item.ProductId,
					Price = item.Product.Price,
					Count = item.Count,
					OrderHeaderId = shoppingCartVM.OrderHeader.Id
				};
				_unitOfWork.OrderDetails.Add(orderDetail);
				_unitOfWork.Complete();
			}


			if (customerIsRegular)
			{
				var domain = "https://localhost:7232/";
				var options = new Stripe.Checkout.SessionCreateOptions
				{

					SuccessUrl = $"{domain}customer/shoppingcart/OrderConfirmation?id={shoppingCartVM.OrderHeader.Id}",
					CancelUrl = $"{domain}customer/shoppingcart/index",
					LineItems = new List<SessionLineItemOptions>(),
					Mode="payment"
					
				}; 

				foreach(var item in shoppingCartVM.Items)
				{
					SessionLineItemOptions sessionLineItem = new()
					{
						PriceData = new SessionLineItemPriceDataOptions()
						{
							UnitAmount = (long)(item.Product.Price * 100),
							Currency = "rub",
							ProductData = new SessionLineItemPriceDataProductDataOptions()
							{
								Name = item.Product.Name
							}

						},
						Quantity = item.Count
					};
					options.LineItems.Add(sessionLineItem);
				}

				var service = new Stripe.Checkout.SessionService();
				Stripe.Checkout.Session session= service.Create(options);
				_unitOfWork.OrderHeaders.UpdateStripePaymentId(shoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
				_unitOfWork.Complete();
				Response.Headers.Add("Location", session.Url);
				return new StatusCodeResult(303);
			}

			return RedirectToAction(nameof(OrderConfirmation), new { id = shoppingCartVM.OrderHeader.Id });
		}
		public IActionResult OrderConfirmation(int id)
		{
			return View();
		}

	}
}
