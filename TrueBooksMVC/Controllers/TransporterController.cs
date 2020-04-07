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
    public class TransporterController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        //
        // GET: /Designation/

        public ActionResult Index()
        {
            var transporter = objSourceMastersModel.GetTransporter();
            return View(transporter);
        }
        //
        // GET: /Transporter/Details/5

        public ActionResult Details(int id = 0)
        {
            Transporter transporter = objSourceMastersModel.GetTransporterById(id);
            if (transporter == null)
            {
                return HttpNotFound();
            }
            return View(transporter);
        }

        //
        // GET: /Transporter/Create


        public ActionResult Create()
        {

            ViewBag.country = DropDownList<CountryMaster>.LoadItems(
                   objSourceMastersModel.GetCountry(), "CountryID", "CountryName");

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
        // POST: /Transporter/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Transporter transporter)
        {
            if (ModelState.IsValid)
            {
                ViewBag.country = DropDownList<CountryMaster>.LoadItems(
                     objSourceMastersModel.GetCountry(), "CountryID", "CountryName");
                var query = (from t in db.Transporters where t.TransPorter1 == transporter.TransPorter1 select t).ToList();

                if (query.Count > 0)
                {

                    ViewBag.SuccessMsg = "Transporter name is already exist";
                    return View();
                }

                objSourceMastersModel.SaveTransporter(transporter);
                ViewBag.SuccessMsg = "You have successfully added Transporter.";
                return View("Index", objSourceMastersModel.GetTransporter());
            }

            return View(transporter);
        }

        //
        // GET: /Transporter/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Transporter transporter = objSourceMastersModel.GetTransporterById(id);
            if (transporter == null)
            {
                return HttpNotFound();
            }
            ViewBag.country = new SelectList(objSourceMastersModel.GetCountry(), "CountryID", "CountryName", transporter.CountryID);
            return View(transporter);
        }

        //
        // POST: /Transporter/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Transporter transporter)
        {
            if (ModelState.IsValid)
            {

                objSourceMastersModel.SaveTransporterById(transporter);
                ViewBag.SuccessMsg = "You have successfully updated Transporter.";
                return View("Index", objSourceMastersModel.GetTransporter());
            }

            return View(transporter);
        }

        //
        // GET: /Transporter/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Transporter transporter = objSourceMastersModel.GetTransporterById(id);
        //    if (transporter == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(transporter);
        //}

        //
        // POST: /Transporter/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            objSourceMastersModel.DeleteTransportert(id);
            ViewBag.SuccessMsg = "You have successfully deleted Transporter.";
            return View("Index", objSourceMastersModel.GetTransporter());
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }




}