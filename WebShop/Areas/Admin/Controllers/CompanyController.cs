using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.DataAccess.Repository;
using Shop.Models;
using Shop.Utility;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Company> companies = _unitOfWork.Companies.GetAll();

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Company());
        }
        [HttpPost]
        public IActionResult Create(Company company)
        {
            if (!ModelState.IsValid)
            {
                TempData["failure"] = "Please, input valid field data";
                return View(company);
            }
            _unitOfWork.Companies.Add(company);
            _unitOfWork.Complete();
            TempData["success"] = "Successfully created company";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            Company company = _unitOfWork.Companies.Find(u => u.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }
        [HttpPost]
        public IActionResult Edit(Company company)
        {
            if(!ModelState.IsValid) {
                TempData["failure"] = "Please, input valid field data";
                return View(company);
            }
            _unitOfWork.Companies.Update(company);
            _unitOfWork.Complete();
            TempData["success"] = "Successfully edited company";
            return RedirectToAction("Index");   
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            Company company = _unitOfWork.Companies.Find(u => u.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }
        [HttpPost]
        public IActionResult Delete(Company company)
        {
            _unitOfWork.Companies.Delete(company);
            _unitOfWork.Complete();
            TempData["success"] = "Successfully deleted company";
            return RedirectToAction("Index");
        }



        #region APICALLS 
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> companies = _unitOfWork.Companies.GetAll().ToList();
            return Json(new {data = companies });
        }
        #endregion

    }
}
