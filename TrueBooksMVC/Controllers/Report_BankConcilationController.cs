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
    public class Report_BankConcilationController : Controller
    {
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        // GET: /Report_BankConcilation/

        public ActionResult BankConcilationIndex()
        {
            ViewBag.AcHead = new SelectList(db.AcHeads, "AcHeadID", "AcHead1");

            return View();
        }
        public LargeJsonResult GetBankConcilation()
        {

            string view = "";
           // var data = entity.ProCustomerAgingDatewise1(frmdate, tdate, CustomerID).ToList();

           // view = this.RenderPartialView("ucBankReconcilation_Partial", data);
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
