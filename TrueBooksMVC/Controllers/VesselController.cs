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
    public class VesselController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        //
        // GET: /Designation/

        public ActionResult Index()
        {
            List<VesselList> lst = objSourceMastersModel.GetVessel();
            return View(lst);
        }
        //
        // GET: /Vessel/Details/5

        public ActionResult Details(int id = 0)
        {
            Vessel vessel = objSourceMastersModel.GetVesselById(id);
            if (vessel == null)
            {
                return HttpNotFound();
            }
            return View(vessel);
        }

        //
        // GET: /Vessel/Create


        public ActionResult Create()
        {

            ViewBag.carrier = DropDownList<Carrier>.LoadItems(objSourceMastersModel.GetCarrier(), "CarrierID", "Carrier1");
                   
            return View();
        }
        public static class DropDownList<T>
        {
            public static SelectList LoadItems(IList<T> collection, string value, string text)
            {
                return new SelectList(collection, value, text);
            }
        }

        //
        // POST: /Vessel/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vessel vessel)
        {
            if (ModelState.IsValid)
            {
                ViewBag.carrier = DropDownList<Carrier>.LoadItems(objSourceMastersModel.GetCarrier(), "CarrierID", "Carrier1");
                var query = (from t in db.Vessels where t.Vessel1 == vessel.Vessel1 select t).ToList();

                if (query.Count > 0)
                {

                    ViewBag.SuccessMsg = "vessel name is already exist";
                    return View();
                }
                objSourceMastersModel.SaveVesselById(vessel);
                ViewBag.SuccessMsg = "You have successfully added Vessel.";
                return View("Index", objSourceMastersModel.GetVessel());
            }

            return View(vessel);
        }

        //
        // GET: /Vessel/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Vessel vessel = objSourceMastersModel.GetVesselById(id);
            if (vessel == null)
            {
                return HttpNotFound();
            }
            ViewBag.carrier = new SelectList(objSourceMastersModel.GetCarrier(), "CarrierID", "Carrier1", vessel.CarrierID);

            

            return View(vessel);
        }

        //
        // POST: /Vessel/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vessel vessel)
        {
            if (ModelState.IsValid)
            {

                objSourceMastersModel.SaveVesselById(vessel);
                ViewBag.SuccessMsg = "You have successfully updated Vessel.";
                return View("Index", objSourceMastersModel.GetVessel());
            }

            return View(vessel);
        }

        //
        // GET: /Vessel/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Vessel vessel = objSourceMastersModel.GetVesselById(id);
        //    if (vessel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(vessel);
        //}

        //
        // POST: /Vessel/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            objSourceMastersModel.DeleteVessel(id);
            ViewBag.SuccessMsg = "You have successfully deleted Vessel.";
            return View("Index", objSourceMastersModel.GetVessel());
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}