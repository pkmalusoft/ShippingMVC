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
    public class CustomerLedgerController : Controller
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();

        public ActionResult Index()
        {
            ViewBag.customers = new SelectList(entity.CUSTOMERs.OrderBy(x => x.Customer1).ToList(), "CustomerID", "Customer1");
            return View();
        }

        public LargeJsonResult GetCustomerLedger(string custid, DateTime frmdate, DateTime tdate)
        {
            int vcusid = 0;

            if (custid == "")
            {
                vcusid = 0;
            }
            else
            {
                vcusid = Convert.ToInt32(custid);
            }

          
            string fdates = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(frmdate);
            string fdatet = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(tdate);

            var data = entity.ProCustomerLedger1(vcusid, fdates,fdatet);

            ViewBag.custid = vcusid;
            ViewBag.fromdate = fdates;
            ViewBag.todate = fdatet;


            string view = this.RenderPartialView("ucCustomerLedger", data);

            ViewBag.todate = tdate;
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
