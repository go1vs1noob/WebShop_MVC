using Microsoft.AspNetCore.Mvc;
using Shop.DataAccess.Data;
using Shop.DataAccess.Repository;
using Shop.Models;


namespace WebShop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly UnitOfWork unitOfWork;
        public CategoryController(ApplicationDbContext dbContext)
        {
                unitOfWork = new UnitOfWork(dbContext);
        }
        public IActionResult Index()
        {
            List<Category> categories = unitOfWork.Categories.GetAll().ToList();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid) { 
                unitOfWork.Categories.Add(obj);
                unitOfWork.Complete();
                TempData["failure"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            TempData["failure"] = "Wrong field data! Try again";
            return View();
        }
        public IActionResult Edit(int id)
        {
            if (id==null || id == 0){
                return NotFound();
            }
            Category? categoryFromDb = unitOfWork.Categories.Get(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Categories.Update(obj);
                unitOfWork.Complete();
                TempData["success"] = "Category edited successfully";
                return RedirectToAction("Index");
            }
            TempData["failure"] = "Wrong field data! Try again";
            return View();
        }
        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = unitOfWork.Categories.Get(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Delete(Category obj)
        {
            unitOfWork.Categories.Delete(obj);
            unitOfWork.Complete();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }


    }
}
