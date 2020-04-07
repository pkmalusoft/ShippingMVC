using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;

namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class JobAnalysisReportController : Controller
    {
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();

        public ActionResult JobAnalysisReportActualCost()
        {

            ViewBag.customer = new SelectList(db.CUSTOMERs, "CustomerID", "Customer1");
            ViewBag.JobType = new SelectList(db.JobTypes, "JobTypeID", "JobDescription");
            ViewBag.Employee = new SelectList(db.Employees, "EmployeeID", "EmployeeName");

            return View();
        }

        public JsonResult GetAllJobData(int CustomerID, int JobTypeID, int Employee, string frmdate, string tdate)
        {

           
            string view = "";
            string fyearFrom = Session["FyearFrom"].ToString();
            //fyearFrom = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(fyearFrom);
          
            var vJobAnalysisVMlist = db.proJobCostFullActualCost(frmdate, tdate, fyearFrom).ToList();
            view = this.RenderPartialView("ucJobAnalysisActualData", vJobAnalysisVMlist);
            return new LargeJsonResult
            {
                MaxJsonLength=Int32.MaxValue,
                Data = new
                {
                    success = true,
                    view = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        public JsonResult GetAllJobDataForPrint(int CustomerID, int JobTypeID, int Employee, string frmdate, string tdate)
        {


            string view = "";
            string fyearFrom = Session["FyearFrom"].ToString();
            //fyearFrom = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(fyearFrom);

            var vJobAnalysisVMlist = db.proJobCostFullActualCost(frmdate, tdate, fyearFrom).ToList();
            view = this.RenderPartialView("ucJobAnalysisActualDataPrint", vJobAnalysisVMlist);
            return new LargeJsonResult
            {
                MaxJsonLength = Int32.MaxValue,
                Data = new
                {
                    success = true,
                    view = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }



        public ActionResult JobAnalysisReport()
        {

            ViewBag.customer = new SelectList(db.CUSTOMERs.OrderBy(x => x.Customer1).ToList(), "CustomerID", "Customer1");
            ViewBag.JobType = new SelectList(db.JobTypes.OrderBy(x => x.JobDescription).ToList(), "JobTypeID", "JobDescription");
            ViewBag.Employee = new SelectList(db.Employees.OrderBy(x => x.EmployeeName).ToList(), "EmployeeID", "EmployeeName");

            return View();
        }

        public JsonResult GetAllJobReportData(int CustomerID, int JobTypeID, int Employee, string frmdate, string tdate)
        {

            string view = "";

            var vJobAnalysisVMlist = db.proJobCostFull(frmdate, tdate, Session["FyearFrom"].ToString()).ToList();

            view = this.RenderPartialView("ucJobAnalysisReport", vJobAnalysisVMlist);
            return new LargeJsonResult
            {
                MaxJsonLength = Int32.MaxValue,
                Data = new
                {
                    success = true,
                    view = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        public JsonResult GetAllJobReportDataForPrint(int CustomerID, int JobTypeID, int Employee, string frmdate, string tdate)
        {

            string view = "";

            var vJobAnalysisVMlist = db.proJobCostFull(frmdate, tdate, Session["FyearFrom"].ToString()).ToList();

            view = this.RenderPartialView("ucJobAnalysisReportPrint", vJobAnalysisVMlist);
            return new LargeJsonResult
            {
                MaxJsonLength = Int32.MaxValue,
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
