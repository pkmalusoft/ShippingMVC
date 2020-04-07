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
    public class LocationController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();

        //
        // GET: /Designation/

        public ActionResult Index()
        {
            var location = objSourceMastersModel.GetLocation();
            return View(location);
        }

        //
        // GET: /Location/Details/5

        public ActionResult Details(int id = 0)
        {
            Location location = objSourceMastersModel.GetJobLocationById(id);

            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        //
        // GET: /Location/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Location/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Location location)
        {
            if (ModelState.IsValid)
            {

                var query = (from t in db.Locations where t.Location1 == location.Location1 select t).ToList();

                if (query.Count > 0)
                {
                    ViewBag.SuccessMsg = "Location name is already exist";
                    return View();
                }
                objSourceMastersModel.SaveLocation(location);
                ViewBag.SuccessMsg = "You have successfully added Location.";
                return View("Index", objSourceMastersModel.GetLocation());
            }

            return View(location);
        }

        //
        // GET: /Location/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Location location = objSourceMastersModel.GetJobLocationById(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        //
        // POST: /Location/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Location location)
        {
                if (ModelState.IsValid)
                {
                    objSourceMastersModel.SaveLocationById(location);
                    ViewBag.SuccessMsg = "You have successfully updated Location.";
                    return View("Index", objSourceMastersModel.GetLocation());
                }

            return View(location);
        }

        
        // GET: /Location/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Location location = objSourceMastersModel.GetJobLocationById(id);
        //    if (location == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(location);
        //}

        
        // POST: /Location/Delete/5

       // [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            objSourceMastersModel.DeleteLocation(id);
            ViewBag.SuccessMsg = "You have successfully deleted Location.";
            return View("Index", objSourceMastersModel.GetLocation());
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}