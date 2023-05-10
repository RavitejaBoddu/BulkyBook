using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess.Repository;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using BulkyBook.Utility;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        //GET
        public IActionResult Upsert(int? id)
        {

            Company company = new();
            if (id == null || id == 0)
            {
                return View(company);
            }
            else
            {
               company = _unitOfWork.Company.GetFirstOrDefault(i => i.Id == id);
                //update product
                return View(company);
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company Obj)
        {
            if (ModelState.IsValid)
            {
                if (Obj.Id == 0)
                {
                    _unitOfWork.Company.Add(Obj);
                    TempData["success"] = "Company Added Successfully.";
                }
                else
                {
                    _unitOfWork.Company.Update(Obj);
                    TempData["success"] = "Company Updated Successfully.";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(Obj);

        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error While deleting" });
            }
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully!" });
        }

        #endregion

    }

}
