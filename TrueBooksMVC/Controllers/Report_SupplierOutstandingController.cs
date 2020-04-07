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
    public class Report_SupplierOutstandingController : Controller
    {
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        //
        // GET: /Report_SupplierOutstanding/

        public ActionResult SupplierOutstanding()
        {
            ViewBag.supplier = new SelectList(db.Suppliers.OrderBy(x => x.SupplierName).ToList(), "SupplierID", "SupplierName");
            return View();
        }

        public LargeJsonResult GetSupplierOutstanding(int SupplierID, DateTime frmdate, DateTime tdate)
        {

            int supid = 0;
            if(SupplierID==null || SupplierID==0)
            {
                supid=0;
            }
            else{
                supid = Convert.ToInt32(SupplierID);
            }
           string view = "";

           ViewBag.supid = supid;
           ViewBag.fromdate = frmdate;
           ViewBag.todate = tdate;


            //int supplierControlAcHeadId = (from t in db.AcHeadAssigns select t.SupplierControlAcID.Value).FirstOrDefault();
            //var data = db.ProSupplierOutstanding(tdate, Convert.ToInt32(supplierControlAcHeadId), false);
           if (supid == 0)
           {
               var data = db.qrySupplierOutstandings.Where((x => x.InvoiceDate >= frmdate && x.InvoiceDate <= tdate)).ToList().Take(0);
               view = this.RenderPartialView("ucSupplierOutstanding", data);
           }
           else
           {
               var data = db.qrySupplierOutstandings.Where(x => x.SupplierID == supid && (x.InvoiceDate >= frmdate && x.InvoiceDate <= tdate)).ToList().Take(0);
               view = this.RenderPartialView("ucSupplierOutstanding", data);
           }


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
        }


    }
}
