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
    public class SupplierAgingDateWiseController : Controller
    {
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();

        public ActionResult SupplierAgingIndex()
        {
            ViewBag.supplier = new SelectList(db.Suppliers.OrderBy(x => x.SupplierName).ToList(), "SupplierID", "SupplierName");
           
            return View();
        }

        public LargeJsonResult GetSupplierAgingdata(int SupplierID, string frmdate, string tdate)
        {


            string view = "";

            var data = db.ProSupplierAgingDatewise( frmdate, tdate,SupplierID).ToList();
            ViewBag.supid = SupplierID;
            ViewBag.fromdate = frmdate;
            ViewBag.todate = tdate;



            view = this.RenderPartialView("ucSupplierAging_Partial", data);

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
