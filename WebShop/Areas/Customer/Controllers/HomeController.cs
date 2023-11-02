using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Shop.Models;
using Shop.DataAccess.Repository;

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

        public IActionResult Details(int? id)
        {
            Product product = _unitOfWork.Products.Find(u => u.Id == id, includeProperties: "Category");
            return View();
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
