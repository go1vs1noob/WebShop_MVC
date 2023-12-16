using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Shop.Models;
using Shop.DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Shop.Utility;
using System.Security.Claims;

namespace WebShop.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Products.GetAll(includeProperties: "Category");
            return View(products);
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = _unitOfWork.Products
                .Find(u => u.Id == productId, includeProperties: "Category"),
                ProductId = productId
            };
            return View(shoppingCart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            var foundCart = _unitOfWork.ShoppingCarts.Find(cart => cart.ApplicationUserId == userId
                                                           && cart.ProductId == shoppingCart.ProductId);
            if (foundCart == null)
            {
                _unitOfWork.ShoppingCarts.Add(shoppingCart);
            } else
            {
                foundCart.Count += shoppingCart.Count;
            }
            _unitOfWork.Complete();
            TempData["success"] = "Shopping cart updated successfuly!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
