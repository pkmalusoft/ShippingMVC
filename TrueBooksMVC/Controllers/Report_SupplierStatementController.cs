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
    public class Report_SupplierStatementController : Controller
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        // GET: /Report_SupplierStatement/

        public ActionResult supplierStatement()
        {
            ViewBag.supplier = new SelectList(entity.Suppliers.OrderBy(x => x.SupplierName).ToList(), "SupplierID", "SupplierName");
            return View();
        }

        public LargeJsonResult GetSupplierStatement(string SupplierID, DateTime frmdate, DateTime tdate)
        {

            string view = "";

            var data = entity.qrySupplierOutstandings.Where(x => x.SupplierID == 0 && (x.InvoiceDate >= frmdate && x.InvoiceDate <= tdate)).ToList().Take(0);
          

            ViewBag.supid = SupplierID;
            ViewBag.fromdate = frmdate;
            ViewBag.todate = tdate;
            view = this.RenderPartialView("ucSupplierStatement_Partial", data);

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
