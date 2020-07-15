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
    public class BranchController : Controller
    {
        private SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        SourceMastersModel objectSourceMaster = new SourceMastersModel();


        //
        // GET: /Branch/

        public ActionResult Index()
        {
            var branchmasters = db.BranchMasters.Include(b => b.CountryMaster).Include(b => b.CurrencyMaster).Include(b => b.Designation).Include(b => b.Location);
            return View(branchmasters.ToList());
        }

        //
        // GET: /Branch/Details/5

        public ActionResult Details(int id = 0)
        {
            BranchMaster branchmaster = db.BranchMasters.Find(id);
            if (branchmaster == null)
            {
                return HttpNotFound();
            }
            return View(branchmaster);
        }

        //
        // GET: /Branch/Create

        public ActionResult Create()
        {
            ViewBag.Company = new SelectList(db.AcCompanies, "AcCompanyID", "AcCompany1");
            ViewBag.CountryID = new SelectList(db.CountryMasters, "CountryID", "CountryName");
            ViewBag.CurrencyID = new SelectList(db.CurrencyMasters, "CurrencyID", "CurrencyName");
            ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "Designation1");
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Location1");
            return View();
        }

        //
        // POST: /Branch/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BranchMaster branchmaster,BranchFinnancialYearVm branchVM)
        {
            if (ModelState.IsValid)
            {

                var query = (from t in db.BranchMasters where t.BranchName == branchmaster.BranchName select t).ToList();

                if (query.Count > 0)
                {
                    ViewBag.SuccessMsg = "Branch name is already exist";
                    return View();
                }
                branchmaster.BranchID = objectSourceMaster.GetMaxNumberBranch();
                branchmaster.BranchName = branchVM.BranchName;
                branchmaster.Address1 = branchVM.Address;
                branchmaster.AcCompanyID = branchVM.CompanyId;
                branchmaster.CurrencyID = branchVM.currencyId;
                branchmaster.CountryID = branchVM.CountryID;
                branchmaster.EMail = branchVM.Email;
                branchmaster.Website = branchVM.Website;
                branchmaster.City = branchVM.City;
                branchmaster.ContactPerson = branchVM.ContactPerson;
                branchmaster.Phone = branchVM.phone;
                db.BranchMasters.Add(branchmaster);
                db.SaveChanges();
                AcFinancialYear acFinnancialYear = new AcFinancialYear();
                acFinnancialYear.AcCompanyID = branchVM.CompanyId;
                acFinnancialYear.BranchID = branchmaster.BranchID;
                acFinnancialYear.AcFYearFrom = branchVM.FromDate;
                acFinnancialYear.AcFYearTo = branchVM.ToDate;
                var getfinancialyr = "";
                if(branchVM.FromDate.Year==branchVM.ToDate.Year)
                {
                    getfinancialyr = branchVM.FromDate.Year.ToString();
                }
                else
                {
                    getfinancialyr = branchVM.FromDate.Year + "-" + branchVM.ToDate.Year;
                }
                acFinnancialYear.ReferenceName = getfinancialyr;
                acFinnancialYear.StatusClose = false;
                acFinnancialYear.Lock = false;
                acFinnancialYear.AcFinancialYearID = objectSourceMaster.GetMaxNumberAcFinancialYear();
                acFinnancialYear.UserID = Convert.ToInt32(Session["UserID"]);

                db.AcFinancialYears.Add(acFinnancialYear);
                TempData["SuccessMsg"] = "You have successfully added Branch.";
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.Company = new SelectList(db.AcCompanies, "AcCompanyID", "AcCompany1", branchmaster.AcCompanyID);
            ViewBag.CountryID = new SelectList(db.CountryMasters, "CountryID", "CountryName", branchmaster.CountryID);
            ViewBag.CurrencyID = new SelectList(db.CurrencyMasters, "CurrencyID", "CurrencyName", branchmaster.CurrencyID);
            ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "Designation1", branchmaster.DesignationID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Location1", branchmaster.LocationID);
            return View(branchmaster);
        }

        //
        // GET: /Branch/Edit/5

        public ActionResult Edit(int id = 0)
        {
            //BranchFinnancialYearVm branchVM = new BranchFinnancialYearVm();
            BranchMaster branchmaster = db.BranchMasters.Find(id);
           
            if (branchmaster == null)
            {
                return HttpNotFound();
            }
            var financialyear=db.AcFinancialYears.Where(d=>d.BranchID==id).FirstOrDefault();
            if(financialyear==null)
            {
                ViewBag.Fromdate = "";
                ViewBag.Todate = "";
            }
            else
            {
                ViewBag.Fromdate = financialyear.AcFYearFrom.Value.ToString("dd/MM/yyyy");
                ViewBag.Todate = financialyear.AcFYearTo.Value.ToString("dd/MM/yyyy");
            }
            ViewBag.Company = db.AcCompanies.ToList();
            ViewBag.CountryID = db.CountryMasters.ToList();
            ViewBag.CurrencyID = db.CurrencyMasters.ToList();
            ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "Designation1", branchmaster.DesignationID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Location1", branchmaster.LocationID);
            return View(branchmaster);
        }

        //
        // POST: /Branch/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BranchMaster branchmaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(branchmaster).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMsg"] = "You have successfully updated Branch.";
                return RedirectToAction("Index");
            }
            ViewBag.Company = new SelectList(db.AcCompanies, "AcCompanyID", "AcCompany1");
            ViewBag.CountryID = new SelectList(db.CountryMasters, "CountryID", "CountryName", branchmaster.CountryID);
            ViewBag.CurrencyID = new SelectList(db.CurrencyMasters, "CurrencyID", "CurrencyName", branchmaster.CurrencyID);
            ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "Designation1", branchmaster.DesignationID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Location1", branchmaster.LocationID);
            return View(branchmaster);
        }

        //
        // GET: /Branch/Delete/5

        public ActionResult Delete(int id = 0)
        {
            BranchMaster branchmaster = db.BranchMasters.Find(id);
            if (branchmaster == null)
            {
                return HttpNotFound();
            }
            return View(branchmaster);
        }

        //
        // POST: /Branch/Delete/5

      
        public ActionResult DeleteConfirmed(int id)
        {
            BranchMaster branchmaster = db.BranchMasters.Find(id);
            db.BranchMasters.Remove(branchmaster);
            db.SaveChanges();
            TempData["SuccessMsg"] = "You have successfully deleted Branch.";
            return RedirectToAction("Index");
        }

        
    }
}