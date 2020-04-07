using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;

namespace ShippingFinal.Controllers
{
    [SessionExpire]
    [Authorize]
    public class DepartmentController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();

        //
        // GET: /Department/

        public ActionResult Index()
        {
            var department = objSourceMastersModel.GetDepartment();
            return View(department);
        }

        //
        // GET: /Department/Details/5

        public ActionResult Details(int id = 0)
        {
            Department department = objSourceMastersModel.GetepartmentById(id);

            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        //
        // GET: /Department/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Department/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                var query = (from t in entity.Departments where t.Department1 == department.Department1 select t).ToList();
               
                if (query.Count > 0)
                {
                    ViewBag.SuccessMsg = "Department name is already exist";
                    return View();
                }
                else
                {
                    objSourceMastersModel.SaveDepartment(department);
                    ViewBag.SuccessMsg = "You have successfully added Department.";
                    return View("Index", objSourceMastersModel.GetDepartment());
                }
            }

           


            return View(department);
        }

        //
        // GET: /Department/Edit/5

        public ActionResult Edit(int id = 0)
        {

            Department department = objSourceMastersModel.GetepartmentById(id);
            if (department == null)
            {
                return HttpNotFound();
            }

            return View(department);
        }

        //
        // POST: /Department/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                objSourceMastersModel.SaveDepartmentById(department);
                ViewBag.SuccessMsg = "You have successfully updated Department.";
                return View("Index", objSourceMastersModel.GetDepartment());
            }
            return View(department);
        }

        //
        // GET: /Department/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Department department = objSourceMastersModel.GetepartmentById(id);
        //    if (department == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(department);
        //}

        //
        // POST: /Department/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            objSourceMastersModel.DeleteDepartment(id);
            ViewBag.SuccessMsg = "You have successfully deleted Department.";
            return View("Index", objSourceMastersModel.GetDepartment());
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}