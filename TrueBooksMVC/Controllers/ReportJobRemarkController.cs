using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;
using DAL;

namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class ReportJobRemarkController : Controller
    {
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        // GET: /ReportJobRemark/

        public ActionResult JobRemark()
        {
            ViewBag.jobCode = new SelectList(db.JobGenerations, "JobID", "JobCode");
            ViewBag.JobType = new SelectList(db.JobTypes, "JobTypeID", "JobDescription");
            ViewBag.Employee = new SelectList(db.Employees.OrderBy(x => x.EmployeeName).ToList(), "EmployeeID", "EmployeeName");
            return View();
        }
        public LargeJsonResult GetAllJobRemark(int JobID, int JobTypeID, int Employee, DateTime frmdate, DateTime tdate)
        {

            string view = "";

            var data = db.Report_JobRemark(frmdate, tdate).ToList();

            view = this.RenderPartialView("ucJobRemarkPartial", data);
            return new LargeJsonResult
            {
                Data = new
                {
                    success = true,
                    view = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public LargeJsonResult GetAllJobRemarkForPrint(int JobID, int JobTypeID, int Employee, DateTime frmdate, DateTime tdate)
        {

            string view = "";

            var data = db.Report_JobRemark(frmdate, tdate).ToList();

            view = this.RenderPartialView("ucJobRemarkPrintPartial", data);
            return new LargeJsonResult
            {
                Data = new
                {
                    success = true,
                    view = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}
