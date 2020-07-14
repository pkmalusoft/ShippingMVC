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
    public class AcGroupController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();

        //
        // GET: /AcGroup/

        public ActionResult Index()
        {
            return View(objSourceMastersModel.GetAllAcGroup());
        }

        //
        // GET: /AcGroup/Details/5

        public ActionResult Details(int id = 0)
        {
            AcGroup acgroup = objSourceMastersModel.GetAcGroupById(id);
            if (acgroup == null)
            {
                return HttpNotFound();
            }
            return View(acgroup);
        }

        //
        // GET: /AcGroup/Create

        public ActionResult Create()
        {
            ViewBag.Company = DropDownList<AcCompany>.LoadItems(
                   objSourceMastersModel.GetAllAcCompanies(), "AcCompanyID", "AcCompany1");          
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
        // POST: /AcGroup/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AcGroup acgroup)
        {
            if (ModelState.IsValid)
            {
                objSourceMastersModel.SaveAcGroups(acgroup);
                ViewBag.SuccessMsg = "You have successfully added Job.";
                return View("Index", objSourceMastersModel.GetAllAcGroup());
            }
            ViewBag.Company = DropDownList<AcCompany>.LoadItems(
                  objSourceMastersModel.GetAllAcCompanies(), "AcCompanyID", "AcCompany1");  
            return View(acgroup);
        }

        //
        // GET: /AcGroup/Edit/5

        public ActionResult Edit(int id = 0)
        {
            AcGroup acgroup = objSourceMastersModel.GetAcGroupById(id);
            if (acgroup == null)
            {
                return HttpNotFound();
            }


            ViewBag.Company = new SelectList(objSourceMastersModel.GetBranchMasters(Convert.ToInt32(Session["AcCompanyID"].ToString())), "BranchID", "BranchName", acgroup.AcBranchID);


            return View(acgroup);
        }

        //
        // POST: /AcGroup/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AcGroup acgroup)
        {
            if (ModelState.IsValid)
            {
                objSourceMastersModel.SaveAcGroupsById(acgroup);
                ViewBag.SuccessMsg = "You have successfully updated Job.";
                return View("Index", objSourceMastersModel.GetAllAcGroup());
            }
            return View(acgroup);
        }

        //
        // GET: /AcGroup/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    AcGroup acgroup = objSourceMastersModel.GetAcGroupById(id);
        //    if (acgroup == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(acgroup);
        //}

        //
        // POST: /AcGroup/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            objSourceMastersModel.DeleteAcGroup(id);
            ViewBag.SuccessMsg = "You have successfully deleted Job.";
            return View("Index", objSourceMastersModel.GetAllAcGroup());
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}