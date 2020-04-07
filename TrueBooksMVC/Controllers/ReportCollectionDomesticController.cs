using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using TrueBooksMVC.Models;

namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class ReportCollectionDomesticController : Controller
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        // GET: /ReportCollectionDomestic/

        public ActionResult CollectionReport()
        {
            ViewBag.customers = new SelectList(entity.CUSTOMERs.OrderBy(x => x.Customer1).ToList(), "CustomerID", "Customer1");
            return View();
        }

        public LargeJsonResult GetAllCollectionDomestic(string custid, string frmdate, string tdate)
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

            var data = entity.proCollectionDetails(frmdate, tdate, vcusid).ToList();

            string view = this.RenderPartialView("ucCollectionReportPartial", data);
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
