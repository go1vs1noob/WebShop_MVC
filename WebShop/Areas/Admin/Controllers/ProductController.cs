using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using Shop.DataAccess.Repository;
using Shop.Models;
using Shop.Models.ViewModels;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> capturedProducts = _unitOfWork.Products.GetAll(includeProperties:"Category").ToList();
            return View(capturedProducts);
        }

        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Product product = _unitOfWork.Products.Find(u => u.Id == id);
            ProductVM productVM = new ProductVM() { Product = product, CategoryList = GetCategorySelectList() };

            if (product == null)
            {
                return NotFound();
            }
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Edit(ProductVM productVM, IFormFile? file)
        {

            if (!ModelState.IsValid)
            {
                productVM.CategoryList = GetCategorySelectList();
                TempData["failure"] = "Please, input valid field data";
                return Edit(productVM, file);
            }
            try
            {
                string imageFilePath = UploadFileToWwwRootFolder("product", file);
                string pathToOldImage = Path.Combine(_webHostEnvironment.WebRootPath, productVM.Product.ImageUrl);
                if (!string.IsNullOrEmpty(pathToOldImage) && !string.IsNullOrEmpty(imageFilePath))
                {
                    if(System.IO.File.Exists(pathToOldImage)){
                        System.IO.File.Delete(pathToOldImage);
                    }
                   productVM.Product.ImageUrl = imageFilePath;
                }
                
            } catch (Exception e) {
                Console.WriteLine(e);
            }
            _unitOfWork.Products.Update(productVM.Product);
            _unitOfWork.Complete();
            TempData["success"] = "Successfully edited product";
            return RedirectToAction("Index");
           

        }
        public IActionResult Create()
        {
            ProductVM productViewModel = new ProductVM() { Product = new Product(), CategoryList = GetCategorySelectList() };
            return View(productViewModel);
        }
        [HttpPost]
        public IActionResult Create(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid) {

                string imageFilePath = UploadFileToWwwRootFolder("product", file);

                productVM.Product.ImageUrl = imageFilePath;
                

                _unitOfWork.Products.Add(productVM.Product);
                _unitOfWork.Complete();
                TempData["success"] = "Successfully created product";
                return RedirectToAction("Index");
            }
            TempData["failure"] = "Please, input valid field data";
            productVM.CategoryList = GetCategorySelectList();
            return View(productVM);
        }
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                TempData["failure"] = "Something went wrong";
                return NotFound();
            }
            Product capturedProduct = _unitOfWork.Products.Find(u => u.Id == id);
            ProductVM productViewModel = new ProductVM() { Product = capturedProduct, CategoryList = GetCategorySelectList() };
            return View(productViewModel);
        }
        [HttpPost]
        public IActionResult Delete(ProductVM productVM)
        {
            if (productVM.Product.Id == 0 || productVM.Product.Id == null) {
                return NotFound();
            }
            _unitOfWork.Products.Delete(productVM.Product);
            _unitOfWork.Complete();
            TempData["success"] = "Successfully deleted product";
            return RedirectToAction("Index", "Product");
        }

        private IEnumerable<SelectListItem> GetCategorySelectList()
        {
            return _unitOfWork.Categories.GetAll().Select(
                category => new SelectListItem()
                { Text = category.Name.ToString(), Value = category.Id.ToString() });
        }
        
        private string UploadFileToWwwRootFolder(string subfolder, IFormFile? file)
        {
            if (file != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string productPath = Path.Combine(wwwRootPath, $@"images{Path.DirectorySeparatorChar}{subfolder}{Path.DirectorySeparatorChar}");
                string fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(file.FileName)}";
                string filePath = Path.Combine(productPath, fileName);

                

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                return $@"images{Path.DirectorySeparatorChar}{subfolder}{Path.DirectorySeparatorChar}{fileName}";
            }
            return string.Empty;
        }
        #region APICALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = _unitOfWork.Products.GetAll(includeProperties:"Category").ToList();
            return Json(new {data = products});
        }
        #endregion
    }
}
