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
    public class VayageController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        //
        // GET: /Designation/

        public ActionResult Index()
        {
            var voyage = objSourceMastersModel.GetVoyage();
            return View(voyage);
        }
        //
        // GET: /Vayage/Details/5

        public ActionResult Details(int id = 0)
        {
            Voyage voyage = objSourceMastersModel.GetVoyageById(id);
            if (voyage == null)
            {
                return HttpNotFound();
            }
            return View(voyage);
        }

        //
        // GET: /Vayage/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Vayage/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Voyage voyage)
        {
            if (ModelState.IsValid)
            {
                var query = (from t in db.Voyages where t.Voyage1 == voyage.Voyage1 select t).ToList();

                if (query.Count > 0)
                {

                    ViewBag.SuccessMsg = "Voyage name is already exist";
                    return View();
                }
                objSourceMastersModel.SaveVoyage(voyage);
                ViewBag.SuccessMsg = "You have successfully added Voyage.";
                return View("Index", objSourceMastersModel.GetVoyage());
            }

            return View(voyage);
        }

        //
        // GET: /Vayage/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Voyage voyage = objSourceMastersModel.GetVoyageById(id);
            if (voyage == null)
            {
                return HttpNotFound();
            }
            return View(voyage);
        }

        //
        // POST: /Vayage/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Voyage voyage)
        {
            if (ModelState.IsValid)
            {

                objSourceMastersModel.SaveVoyageById(voyage);
                ViewBag.SuccessMsg = "You have successfully updated Voyage.";
                return View("Index", objSourceMastersModel.GetVoyage());
            }

            return View(voyage);
        }

        //
        // GET: /Vayage/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Voyage voyage = objSourceMastersModel.GetVoyageById(id);
        //    if (voyage == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(voyage);
        //}

        //
        // POST: /Vayage/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            objSourceMastersModel.DeleteVoyage(id);
            ViewBag.SuccessMsg = "You have successfully deleted Voyage.";
            return View("Index", objSourceMastersModel.GetVoyage());
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}