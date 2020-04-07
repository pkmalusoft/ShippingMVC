using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using TrueBooksMVC.Models;

namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class CurrencyMasterController : Controller
    {
        private SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        SourceMastersModel obj = new SourceMastersModel();
        //
        // GET: /CurrencyMaster/

        public ActionResult Index()
        {
            return View(db.CurrencyMasters.ToList());
        }

        //
        // GET: /CurrencyMaster/Details/5

        public ActionResult Details(int id = 0)
        {
            CurrencyMaster currencymaster = db.CurrencyMasters.Find(id);
            if (currencymaster == null)
            {
                return HttpNotFound();
            }
            return View(currencymaster);
        }

        //
        // GET: /CurrencyMaster/Create

        public ActionResult Create()
        {

            

            var data=db.CountryMasters.ToList();
            ViewBag.country = data;
            return View();
        }

        //
        // POST: /CurrencyMaster/Create

        [HttpPost]
        public ActionResult Create(CurrencyMaster currencymaster)
        {

            var data = db.CountryMasters.OrderBy(x => x.CountryName).ToList();
            ViewBag.country = data;

                var query = (from t in db.CurrencyMasters where t.CurrencyName == currencymaster.CurrencyName select t).ToList();

                if (query.Count > 0)
                {
                    
                    ViewBag.SuccessMsg = "Currency name is already exist";
                    return View();
                }
                currencymaster.CurrencyID = obj.GetMaxNumberCurrency();
                db.CurrencyMasters.Add(currencymaster);
                db.SaveChanges();
                ViewBag.SuccessMsg = "You have successfully added Currency.";
                return View("Index",db.CurrencyMasters.ToList());
            

          
        }

        //
        // GET: /CurrencyMaster/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CurrencyMaster currencymaster = db.CurrencyMasters.Find(id);
            if (currencymaster == null)
            {
                return HttpNotFound();
            }

            var data = db.CountryMasters.OrderBy(x => x.CountryName).ToList();
            ViewBag.country = data;
            return View(currencymaster);
        }

        //
        // POST: /CurrencyMaster/Edit/5

        [HttpPost]
        public ActionResult Edit(CurrencyMaster currencymaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(currencymaster).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.SuccessMsg = "You have successfully updated Currency.";
                return View("Index", db.CurrencyMasters.ToList());
            }
            return View(currencymaster);
        }

        //
        // GET: /CurrencyMaster/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    CurrencyMaster currencymaster = db.CurrencyMasters.Find(id);
        //    if (currencymaster == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(currencymaster);
        //}

        //
        // POST: /CurrencyMaster/Delete/5

        //[HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            CurrencyMaster currencymaster = db.CurrencyMasters.Find(id);
            db.CurrencyMasters.Remove(currencymaster);
            db.SaveChanges();
            ViewBag.SuccessMsg = "You have successfully deleted Currency.";
            return View("Index", db.CurrencyMasters.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}