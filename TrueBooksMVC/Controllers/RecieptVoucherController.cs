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
    public class RecieptVoucherController : Controller
    {
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        RecieptPaymentModel RP = new RecieptPaymentModel();
        public ActionResult RecieptVoucher()
        {
            //ViewBag.customers = new SelectList(db.CUSTOMERs, "CustomerID", "Customer1");
            //List<SP_GetAllRecieptsDetails_Result> Reciepts = new List<SP_GetAllRecieptsDetails_Result>();

            //Reciepts = RP.GetAllReciepts();
            ////var data = (from t in Reciepts where (t.RecPayDate >= Convert.ToDateTime(Session["FyearFrom"]) && t.RecPayDate <= Convert.ToDateTime(Session["FyearTo"])) select t).ToList();

           


            //return View(Reciepts);

            return View();
           
          
        }

        public JsonResult GetAllReciepts()
        {
            List<SP_GetAllRecieptsDetails_Result> Reciepts = new List<SP_GetAllRecieptsDetails_Result>();

            Reciepts = RP.GetAllReciepts();

            string view = "";
            view = this.RenderPartialView("uc_GetAllCustomer", Reciepts);
            return new JsonResult
            {
                Data = new
                {
                    success = true,
                    view = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetAllRecieptVoucher(int recpayid)
        {
            AcCompany c = db.AcCompanies.FirstOrDefault();
            ViewBag.companyname = c.AcCompany1;
            ViewBag.caddress1 = c.Address1;
            ViewBag.caddress2 = c.Address2;
            ViewBag.caddress3 = c.Address3;
            ViewBag.cphone = c.Phone;

            var data = (from t in db.RecPays where t.RecPayID == recpayid select t).FirstOrDefault();

            string view = this.RenderPartialView("ucRecieptVoucher", data);
            return new JsonResult
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
