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
    public class DesignationController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        //
        // GET: /Designation/

        public ActionResult Index()
        {
           
           var designation = objSourceMastersModel.GetDesignationt();
            return View(designation);
        }

        //
        // GET: /Designation/Details/5

        public ActionResult Details(int id = 0)
        {
            Designation designation = objSourceMastersModel.GetDesignationById(id);

            if (designation == null)
            {
                return HttpNotFound();
            }
            return View(designation);
        }

        //
        // GET: /Designation/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Designation/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Designation designation)
        {


            if (ModelState.IsValid)
            {

                var query = (from t in entity.Designations where t.Designation1 == designation.Designation1 select t).ToList();

                if (query.Count > 0)
                {
                    ViewBag.SuccessMsg = "Designation name is already exist";
                    return View();
                }
                objSourceMastersModel.SaveDesignation(designation);
                ViewBag.SuccessMsg = "You have successfully added Designation.";
                return View("Index", objSourceMastersModel.GetDesignationt());
            }

            return View(designation);

           
        }

        //
        // GET: /Designation/Edit/5

        public ActionResult Edit(int id = 0)
        {

            Designation designation = objSourceMastersModel.GetDesignationById(id);
            if (designation == null)
            {
                return HttpNotFound();
            }

            return View(designation);
        }

        //
        // POST: /Designation/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Designation designation)
        {
            if (ModelState.IsValid)
            {
                objSourceMastersModel.SaveDesignationById(designation);
                ViewBag.SuccessMsg = "You have successfully updated Designation.";
                return View("Index", objSourceMastersModel.GetDesignationt());
            }
            return View(designation);
        }

        //
        // GET: /Designation/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Designation designation = objSourceMastersModel.GetDesignationById(id);
        //    if (designation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(designation);
        //}

        //
        // POST: /Designation/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            objSourceMastersModel.DeleteDesignation(id);
            ViewBag.SuccessMsg = "You have successfully deleted Designation.";
            return View("Index", objSourceMastersModel.GetDesignationt());
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}