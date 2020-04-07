using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;
using DAL;
using System.Dynamic;


namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class ReportsController : Controller
    {
        MastersModel MM = new MastersModel();
        ReportModel RM = new ReportModel();        
        // GET: /Reports/

        public ActionResult Reports()
        {
            List<SP_GetAllCustomers_Result> Customers = new List<SP_GetAllCustomers_Result>();

            Customers = MM.GetAllCustomer();

            return View(Customers);
        }



        public JsonResult PrintCustomerLedger(int id)
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            // var AllInvoices;

            List<SP_GetCustomerByID_Result> Cust = new List<SP_GetCustomerByID_Result>();

            Cust = MM.GetCustByID(id);

            foreach (var item in Cust)
            {
                ViewBag.Customername = item.Customer;
                ViewBag.Addr = item.Address1;
            }

                var CustomerLedger = RM.GetCustomerLedgerReportForPrint(id);

                return Json(CustomerLedger, JsonRequestBehavior.AllowGet);
          
        }


        public JsonResult GetCustomerDetailsByID(int id)
        {
            var custdetails = MM.GetCustByID(id);

            return Json(custdetails, JsonRequestBehavior.AllowGet);
          
        }

        public JsonResult GetAllCustomers()
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            // var AllInvoices;
            var AllCustomers = MM.GetAllCustomer();
            return Json(AllCustomers, JsonRequestBehavior.AllowGet);


        }


    }
}
