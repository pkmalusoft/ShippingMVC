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
    public class Report_CustomerAgieingController : Controller
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        //
        // GET: /Report_CustomerAgieing/

        public ActionResult CustomerAging()
        {
            ViewBag.customers = new SelectList(entity.CUSTOMERs.OrderBy(x => x.Customer1).ToList(), "CustomerID", "Customer1");
            return View();
        }

        public LargeJsonResult GetCustomerAgingData(int CustomerID, string frmdate, string tdate)
        {


            string view = "";
            var data = entity.ProCustomerAgingDatewise(frmdate, tdate, CustomerID).ToList().Take(0);
            ViewBag.supid = CustomerID;
            ViewBag.fromdate = frmdate;
            ViewBag.todate = tdate;

            view = this.RenderPartialView("ucCustomerAging_partial", data);


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
