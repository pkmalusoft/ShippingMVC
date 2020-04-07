using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    public class AirShipmentReportController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetReport(DateTime frmdate, DateTime tdate)
        {

            string fdates = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(frmdate);
            string fdatet = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(tdate);
            ViewBag.fromdate = fdates;
            ViewBag.todate = fdatet;
            return PartialView("ucReport", null);



        }

    }
}
