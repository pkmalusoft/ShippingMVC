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
    public class JobTypeController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        //
        // GET: /Designation/

        public ActionResult Index()
        {
            List<JobModeJobTypeVM> model = new List<JobModeJobTypeVM>();
            var query = (from t in entity.JobTypes
                         join t1 in entity.JobModes on t.JobModeID equals t1.JobModeID
                         select new JobModeJobTypeVM

                         {
                             JobDescription=t.JobDescription,
                             JobMode=t1.JobMode1,
                             JobTypeID=t.JobTypeID,
                             Remarks=t.Remarks,
                             JobCode=t.JobCode,
                             JobPrefix=t.JobPrefix,
                             StatusSea=t.StatusSea.HasValue?t.StatusSea.Value:false
                             
                         }).ToList();
           // var jobmode = objSourceMastersModel.GetJobType();
            return View(query);
        }

        //
        // GET: /JobType/Details/5

        public ActionResult Details(int id = 0)
        {
            JobType jobtype = objSourceMastersModel.GetJobTypeById(id);

            if (jobtype == null)
            {
                return HttpNotFound();
            }
            return View(jobtype);
        }

        //
        // GET: /JobType/Create

        public ActionResult Create()
        {
            ViewBag.JobMode = DropDownList<JobMode>.LoadItems(
                objSourceMastersModel.GetJobMode(), "JobModeID", "JobMode1");

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
        // POST: /JobType/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobType jobtype)
        {
            if (ModelState.IsValid)
            {
                ViewBag.JobMode = DropDownList<JobMode>.LoadItems(
               objSourceMastersModel.GetJobMode(), "JobModeID", "JobMode1");


                var query = (from t in entity.JobTypes where t.JobDescription == jobtype.JobDescription select t).ToList();

                if (query.Count > 0)
                {
                    ViewBag.SuccessMsg = "Job type is already exist";
                    return View();
                }
                objSourceMastersModel.SaveJobType(jobtype);
                TempData["SuccessMSG"] = "You have successfully added Job Type.";
                return RedirectToAction("Index");
            }

            return View(jobtype);
        }

        //
        // GET: /JobType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ViewBag.JobMode = DropDownList<JobMode>.LoadItems(
               objSourceMastersModel.GetJobMode(), "JobModeID", "JobMode1");
            JobType jobtype = objSourceMastersModel.GetJobTypeById(id);
            if (jobtype == null)
            {
                return HttpNotFound();
            }

            ViewBag.JobModes = new SelectList(objSourceMastersModel.GetJobMode(), "JobModeID", "JobMode1", jobtype.JobModeID);

            return View(jobtype);
        }

        //
        // POST: /JobType/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobType jobtype)
        {
            if (ModelState.IsValid)
            {
                ViewBag.JobModes = new SelectList(objSourceMastersModel.GetJobMode(), "JobModeID", "JobMode1", jobtype.JobModeID);
                if (ModelState.IsValid)
                {
                    objSourceMastersModel.SaveJobTypeById(jobtype);
                    TempData["SuccessMSG"] = "You have successfully updated Job Type.";
                    return RedirectToAction("Index");
                }


            }
            return View(jobtype);
        }

        //
        // GET: /JobType/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    JobType jobtype = objSourceMastersModel.GetJobTypeById(id);
        //    if (jobtype == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(jobtype);
        //}

        //
        // POST: /JobType/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            objSourceMastersModel.DeleteJobType(id);
            TempData["SuccessMSG"] = "You have successfully deleted Job Type.";
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}