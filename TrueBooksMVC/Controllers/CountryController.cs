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
    public class CountryController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();

        //
        // GET: /Country/

        public ActionResult Index()
        {
            var country = objSourceMastersModel.GetCountry();
            return View(country);
        }

        //
        // GET: /Country/Details/5

        public ActionResult Details(int id = 0)
        {
            CountryMaster country = objSourceMastersModel.GetCountryById(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        //
        // GET: /Country/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Country/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CountryMaster countrymaster)
        {
            if (ModelState.IsValid)
            {

                var query = (from t in db.CountryMasters where t.CountryName == countrymaster.CountryName select t).ToList();

                if (query.Count > 0)
                {
                    ViewBag.SuccessMsg = "Country name is already exist";
                    return View();
                }

                objSourceMastersModel.SaveCountry(countrymaster);
                ViewBag.SuccessMsg = "You have successfully added Country.";
                return View("Index", objSourceMastersModel.GetCountry());
            }

            return View(countrymaster);

           
        }

        //
        // GET: /Country/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CountryMaster country = objSourceMastersModel.GetCountryById(id);
            if (country == null)
            {
                return HttpNotFound();
            }

            return View(country);
        }

        //
        // POST: /Country/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CountryMaster countrymaster)
        {
            if (ModelState.IsValid)
            {
                objSourceMastersModel.SaveCountryById(countrymaster);
                ViewBag.SuccessMsg = "You have successfully updated Country.";
                return View("Index", objSourceMastersModel.GetCountry());
            }
            return View(countrymaster);
        }

        //
        // GET: /Country/Delete/5

        //public ActionResult Delete(int id = 0)
        //{

        //    CountryMaster country = objSourceMastersModel.GetCountryById(id);
        //    if (country == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(country);
        //}

        //
        // POST: /Country/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            objSourceMastersModel.DeleteCountry(id);
            ViewBag.SuccessMsg = "You have successfully deleted Country.";
            return View("Index", objSourceMastersModel.GetCountry());
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}