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
    public class CarrierController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();

        //
        // GET: /Carrier/

        public ActionResult Index()
        {
            var carrier = objSourceMastersModel.GetCarrier();
            return View(carrier);
        }

        //
        // GET: /Carrier/Details/5

        public ActionResult Details(int id = 0)
        {
            Carrier carrier = objSourceMastersModel.GetAcCarrierById(id);
            if (carrier == null)
            {
                return HttpNotFound();
            }
            return View(carrier);
        }

        //
        // GET: /Carrier/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Carrier/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Carrier carrier)
        {
            if (ModelState.IsValid)
            {

                var query = (from t in entity.Carriers where t.Carrier1 == carrier.Carrier1 select t).ToList();

                if (query.Count > 0)
                {
                    ViewBag.SuccessMsg = "Carrier name is already exist";
                    return View();
                }

                objSourceMastersModel.SaveCarrier(carrier);
                ViewBag.SuccessMsg = "You have successfully added Carrier.";
                return View("Index", objSourceMastersModel.GetCarrier());
            }

            return View(carrier);
        }

        //
        // GET: /Carrier/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Carrier carrier = objSourceMastersModel.GetAcCarrierById(id);
            if (carrier == null)
            {
                return HttpNotFound();
            }

            return View(carrier);
        }

        //
        // POST: /Carrier/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Carrier carrier)
        {
            if (ModelState.IsValid)
            {
                objSourceMastersModel.SaveCarrierById(carrier);
                ViewBag.SuccessMsg = "You have successfully updated Carrier.";
                return View("Index", objSourceMastersModel.GetCarrier());
            }
            return View(carrier);
        }

        //
        // GET: /Carrier/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Carrier carrier = objSourceMastersModel.GetAcCarrierById(id);
        //    if (carrier == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(carrier);
        //}

        //
        // POST: /Carrier/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            objSourceMastersModel.DeleteCarrier(id);
            ViewBag.SuccessMsg = "You have successfully deleted Carrier.";
            return View("Index", objSourceMastersModel.GetCarrier());
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}