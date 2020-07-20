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
    public class JobStatusController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        //
        // GET: /Designation/

        public ActionResult Index()
        {
            var jobtype = objSourceMastersModel.GetJobStatus();
            return View(jobtype);
        }
        //
        // GET: /JobMode/Details/5

        public ActionResult Details(int id = 0)
        {
            JobStatu jobmode = objSourceMastersModel.GetJobStatusById(id);

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
        public ActionResult Create(JobStatu jobstatus)
        {
            if (ModelState.IsValid)
            {

                var query = (from t in db.JobStatus where t.StatusName == jobstatus.StatusName select t).ToList();

                if (query.Count > 0)
                {
                    ViewBag.SuccessMsg = "Job Status is already exist";
                    return View();
                }

                objSourceMastersModel.SaveJobStaus(jobstatus);
                ViewBag.SuccessMsg = "You have successfully added Job Status.";
                return View("Index", objSourceMastersModel.GetJobStatus());
            }

            return View(jobstatus);


        }

        //
        // GET: /JobMode/Edit/5

        public ActionResult Edit(int id = 0)
        {
            JobStatu jobmode = objSourceMastersModel.GetJobStatusById(id);
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
        public ActionResult Edit(JobStatu jobstatus)
        {
            if (ModelState.IsValid)
            {
                objSourceMastersModel.SaveJobStatusById(jobstatus);
                ViewBag.SuccessMsg = "You have successfully updated Job Status.";
                return View("Index", objSourceMastersModel.GetJobStatus());
            }
            return View(jobstatus);
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
            var isinuse = (from d in db.JobGenerations where d.JobStatusId == id select d).ToList();
            if (isinuse.Count == 0)
            {
                objSourceMastersModel.DeleteJobStatus(id);
                ViewBag.SuccessMsg = "You have successfully deleted Job Status.";
                return View("Index", objSourceMastersModel.GetJobStatus());
            }
            else
            {
                ViewBag.ErrorMsg = "Can not delete, Job Status in use.";
                return View("Index", objSourceMastersModel.GetJobStatus());
            }
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}

