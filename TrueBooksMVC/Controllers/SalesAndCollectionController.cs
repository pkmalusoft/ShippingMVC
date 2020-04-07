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
    public class SalesAndCollectionController : Controller
    {
       SHIPPING_FinalEntities entity=new SHIPPING_FinalEntities();
        

        public ActionResult SalesAndCollectionIndex()
        {
            ViewBag.customers = new SelectList(entity.CUSTOMERs.OrderBy(x => x.Customer1).ToList(), "CustomerID", "Customer1");
            return View();
        }


        public LargeJsonResult GetAllSales()
        {


            DateTime frmdate = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime tdate = DateTime.Now;

            var data = entity.SalesAndCollectionRpt(0, frmdate, tdate);
            string view = this.RenderPartialView("ucSalesAndCollection", data);

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

        public LargeJsonResult GetAllSalesByCustomerID(string custid, DateTime frmdate, DateTime tdate)
        {


            int vcustid = 0;

            if (custid == "")
            {
                vcustid = 0;
            }
            else
            {
                vcustid = Convert.ToInt32(custid);
            }

            string view = "";

           
            var data = entity.SalesAndCollectionRpt(vcustid, frmdate, tdate);

            view = this.RenderPartialView("ucSalesAndCollection", data);

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

        public LargeJsonResult GetSalesForPrint(string custid, DateTime frmdate, DateTime tdate)
        {


            int vcustid = 0;

            if (custid == "")
            {
                vcustid = 0;
            }
            else
            {
                vcustid = Convert.ToInt32(custid);
            }

         
            var data = entity.SalesAndCollectionRpt(vcustid, frmdate, tdate);

            return new LargeJsonResult
            {
                Data=data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }


        public ActionResult saleAndCollectionPrintReport()
        {

            return View();
        }
    }
}

