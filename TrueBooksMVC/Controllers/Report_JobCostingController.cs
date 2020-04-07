using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;
using DAL;
using System.IO;

namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class Report_JobCostingController : Controller
    {
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        //
        // GET: /Report_JobCosting/

        public ActionResult JobCosting()
        {

            ViewBag.customer = new SelectList(db.CUSTOMERs.OrderBy(x => x.Customer1).ToList(), "CustomerID", "Customer1");
            ViewBag.JobType = new SelectList(db.JobTypes.OrderBy(x => x.JobDescription).ToList(), "JobTypeID", "JobDescription");
            ViewBag.Employee = new SelectList(db.Employees.OrderBy(x => x.EmployeeName).ToList(), "EmployeeID", "EmployeeName");

            return View();
        }
        public LargeJsonResult GetJobCostData(int CustomerID, int JobTypeID, int Employee, string frmdate, string tdate)
        {
           
            string view = "";
            
          var  vJobAnalysisVMlist = db.proJobCostFull(frmdate, tdate, Session["FyearFrom"].ToString()).ToList();

          view = this.RenderPartialView("ucJobcosting_partial", vJobAnalysisVMlist);
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


