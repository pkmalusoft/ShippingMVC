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
    public class SupplierLedgerController : Controller
    {
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();

        public ActionResult SupplierLedgerIndex()
        {
            ViewBag.supplier = new SelectList(db.Suppliers.OrderBy(x => x.SupplierName).ToList(), "SupplierID", "SupplierName");
            return View();
        }

        public LargeJsonResult GetSupplierLedger(int SupplierId, string frmdate, string tdate)
        {
          
            string view = "";
           
               
                var data = db.ProSupplierLedger(SupplierId, frmdate, tdate).ToList();
                ViewBag.supid = SupplierId;
                ViewBag.fromdate = frmdate;
                ViewBag.todate = tdate;


            view = this.RenderPartialView("ucSupplierLedgerPartial", data);

            DateTime d = Convert.ToDateTime(tdate);

            ViewBag.todate = d;

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
