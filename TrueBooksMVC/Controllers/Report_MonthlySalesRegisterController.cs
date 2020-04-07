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
    public class Report_MonthlySalesRegisterController : Controller
    {
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();


        public ActionResult MonthlySalesRegister()
        {
            var data = db.CUSTOMERs.ToList();


            ViewBag.customers = ViewBag.supplier = new SelectList(db.CUSTOMERs.OrderBy(x => x.Customer1).ToList(), "CustomerID", "Customer1");
            ViewBag.employee = ViewBag.supplier = new SelectList(db.Employees.OrderBy(x => x.EmployeeName).ToList(), "EmployeeID", "EmployeeName");
            return View();
        }

        public LargeJsonResult GetReport(string CustomerID,string EmployeeID, DateTime frmdate, DateTime tdate)
        {

          


            ViewBag.custid = CustomerID;
            ViewBag.empid = EmployeeID;
            ViewBag.fromdate = frmdate;
            ViewBag.todate = tdate;
            string view = this.RenderPartialView("ucMonthlyReport", null);

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

            // return new LargeJsonResult { Data = customers, JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet };

        }


    }
}
