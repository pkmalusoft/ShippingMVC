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
    public class JobModeController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        //
        // GET: /Designation/

        public ActionResult Index()
        {
            var jobtype = objSourceMastersModel.GetJobMode();
            return View(jobtype);
        }
        //
        // GET: /JobMode/Details/5

        public ActionResult Details(int id = 0)
        {
            JobMode jobmode = objSourceMastersModel.GetJobModeById(id);

            if (jobmode == null)
            {
                return HttpNotFound();
            }
            return View(jobmode);
        }

        //
        // GET: /JobMode/Create

        public ActionResult Create()
        {
           

            return View();
        }

        //
        // POST: /JobMode/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobMode jobmode)
        {
            if (ModelState.IsValid)
            {

                var query = (from t in db.JobModes where t.JobMode1 == jobmode.JobMode1 select t).ToList();

                if (query.Count > 0)
                {
                    ViewBag.SuccessMsg = "Job Mode is already exist";
                    return View();
                }

                objSourceMastersModel.SaveJobMode(jobmode);
                ViewBag.SuccessMsg = "You have successfully added Job Mode.";
                return View("Index", objSourceMastersModel.GetJobMode());
            }

            return View(jobmode);

            
        }

        //
        // GET: /JobMode/Edit/5

        public ActionResult Edit(int id = 0)
        {
            JobMode jobmode = objSourceMastersModel.GetJobModeById(id);
            if (jobmode == null)
            {
                return HttpNotFound();
            }
            return View(jobmode);
        }

        //
        // POST: /JobMode/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobMode jobmode)
        {
            if (ModelState.IsValid)
            {
                objSourceMastersModel.SaveJobModeById(jobmode);
                ViewBag.SuccessMsg = "You have successfully updated Job Mode.";
                return View("Index", objSourceMastersModel.GetJobMode());
            }
            return View(jobmode);
            }
        

        //
        // GET: /JobMode/Delete/5

        //public ActionResult  Delete(int id = 0)
        //{
        //    JobMode jobmode = objSourceMastersModel.GetJobModeById(id);
        //    if (jobmode == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(jobmode);
        //}

        //
        // POST: /JobMode/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            objSourceMastersModel.DeleteJobMode(id);
            ViewBag.SuccessMsg = "You have successfully deleted Job Mode.";
            return View("Index", objSourceMastersModel.GetJobMode());
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}

