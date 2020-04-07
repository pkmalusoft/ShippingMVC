using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;

namespace ShippingFinal.Controllers
{
    [SessionExpire]
     [Authorize]
    public class AcCompanyController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        //
        // GET: /AcCompany/

        public ActionResult Index()
        {
           
                      
            var vAcCompnies = objSourceMastersModel.GetAllAcCompanies();
            return View(vAcCompnies);
        }

        //
        // GET: /AcCompany/Details/5

        public ActionResult Details(int id = 0)
        {
            AcCompany accompany = objSourceMastersModel.GetAcCompaniesById(id);
            if (accompany == null)
            {
                return HttpNotFound();
            }
            return View(accompany);
        }

        //
        // GET: /AcCompany/Create

        public ActionResult Create()
        {
            ViewBag.Country = db.CountryMasters.OrderBy(x => x.CountryName).ToList();
            ViewBag.Currency = db.CurrencyMasters.OrderBy(x => x.CurrencyName).ToList();
            return View();
        }

        //
        // POST: /AcCompany/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AcCompany accompany, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                string fileName = "";
                var query = (from t in db.AcCompanies where t.AcCompany1 == accompany.AcCompany1 select t).ToList();

                if (query.Count > 0)
                {
                   
                    ViewBag.Country = db.CountryMasters.ToList();
                    ViewBag.Currency = db.CurrencyMasters.ToList();

                    ViewBag.SuccessMsg = "Company name is already exist";
                    return View();
                }
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    fileName = Path.GetFileName(ImageFile.FileName);
                    if (System.IO.File.Exists(Server.MapPath("~/Content/Logo/" + fileName)))
                    {
                        ViewBag.SuccessMsg = "Logo with same file name is already exist";
                        return View();
                    }
                    ImageFile.SaveAs(Server.MapPath("~/Content/Logo/" + fileName));
                    accompany.logo = fileName;
                }
                objSourceMastersModel.SaveAcCompanies(accompany);
                ViewBag.SuccessMsg = "You have successfully added Company.";
                return View("Index",objSourceMastersModel.GetAllAcCompanies());
            }

            return View(accompany);
        }

        //
        // GET: /AcCompany/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ViewBag.Country = db.CountryMasters.OrderBy(x => x.CountryName).ToList();
            ViewBag.Currency = db.CurrencyMasters.OrderBy(x => x.CurrencyName).ToList();
            AcCompany accompany = objSourceMastersModel.GetAcCompaniesById(id);
            if (accompany == null)
            {
                return HttpNotFound();
            }
            return View(accompany);
        }

        //
        // POST: /AcCompany/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AcCompany accompany, HttpPostedFileBase ImageFile)
        {
            string fileName = "";
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    fileName = Path.GetFileName(ImageFile.FileName);
                    if (System.IO.File.Exists(Server.MapPath("~/Content/Logo/" + fileName)))
                    {
                        ViewBag.SuccessMsg = "Logo with same file name is already exist";
                        return View();
                    }
                    ImageFile.SaveAs(Server.MapPath("~/Content/Logo/" + fileName));
                    accompany.logo = fileName;
                }
                objSourceMastersModel.SaveAcCompaniesById(accompany);

                ViewBag.SuccessMsg = "You have successfully updated Company.";
                return View("Index", objSourceMastersModel.GetAllAcCompanies());
                
            }

            return View(accompany);
        }

        //
        // GET: /AcCompany/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    AcCompany accompany = objSourceMastersModel.GetAcCompaniesById(id);
        //    if (accompany == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(accompany);
        //}

        //
        // POST: /AcCompany/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            objSourceMastersModel.DeleteCompany(id);
            ViewBag.SuccessMsg = "You have successfully deleted Company.";
            return View("Index", objSourceMastersModel.GetAllAcCompanies());
        }

        //protected override void Dispose(bool disposing)
        //{
        //    objSourceMastersModel.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}