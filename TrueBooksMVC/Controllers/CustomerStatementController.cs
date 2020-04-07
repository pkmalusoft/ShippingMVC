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
    public class CustomerStatementController : Controller
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();

        public ActionResult Index()
        {
            ViewBag.customers = new SelectList(entity.CUSTOMERs.OrderBy(x => x.Customer1).ToList(), "CustomerID", "Customer1");
            return View();
        }

        public LargeJsonResult GetAllSalesByCustomerID(string custid, DateTime frmdate, DateTime tdate)
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

            ViewBag.todate = tdate;
            ViewBag.fromdate = frmdate;
            ViewBag.custid = vcusid;
            string fdates = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(frmdate);
            string fdatet = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(tdate);

            //var data = entity.ProCustomerStatement1(vcusid, fdates,fdatet);
            string view = "";
            if (vcusid == 0)
            {
                var data = entity.qryCustomerStatements.Where(x=>x.InvoiceDate >= frmdate && x.InvoiceDate <= tdate).ToList();

                view = this.RenderPartialView("ucCustomerStatement", data);
            }
            else
            {
                var data = entity.qryCustomerStatements.Where(x=>x.InvoiceToID==vcusid &&  (x.InvoiceDate >= frmdate && x.InvoiceDate <= tdate)).ToList();

                view = this.RenderPartialView("ucCustomerStatement", data);
            }

            return new LargeJsonResult
            {
                MaxJsonLength=Int32.MaxValue,
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
