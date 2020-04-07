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
    public class Report_CustomerOutstandingController : Controller
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        //
        // GET: /Report_CustomerOutstanding/

        public ActionResult CustomerOutstanding()
        {
            ViewBag.customers = new SelectList(entity.CUSTOMERs.OrderBy(x => x.Customer1).ToList(), "CustomerID", "Customer1");
                return View();
            
        }

        public LargeJsonResult GetCustomerOutstandingData(int CustomerID, DateTime frmdate, DateTime tdate)
        {

            int custid = 0;

            if (CustomerID == 0 || CustomerID == null)
            {
                custid = 0;
            }
            else
            {
                custid = Convert.ToInt32(CustomerID);
            }


            string view = "";
            ViewBag.custid = custid;
            ViewBag.fromdate = frmdate;
            ViewBag.todate = tdate;


            if (custid == 0)
            {
                var data = entity.qryCustomerStatements.Where(x => x.InvoiceDate >= frmdate && x.InvoiceDate <= tdate).ToList().Take(0);
                view = this.RenderPartialView("usCustomerOutstandingPartial", data);
            }
            else
            {
                var data = entity.qryCustomerStatements.Where(x => x.InvoiceToID == custid && (x.InvoiceDate >= frmdate && x.InvoiceDate <= tdate)).ToList().Take(0);
                view = this.RenderPartialView("usCustomerOutstandingPartial", data);
            }
            

            //var data = from t in db.AcJournalMasters select t;
            
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
