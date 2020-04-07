using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class HomeController : Controller
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
       

        public ActionResult Home()
        {
         
            ViewBag.Message = "";
            var Query = (from t in entity.Menus where t.IsAccountMenu.Value == false orderby t.MenuOrder select t).ToList();

            var jobdetails = (from t in entity.GETLOCALCostForDashboard(Convert.ToInt32(Session["fyearid"].ToString())) select t).ToList();
            if (jobdetails != null)
                ViewBag.jobcostdetails = jobdetails;


            var CostUpdation = (from t in entity.SPGetAllLocalCurrencyCostUpdation(Convert.ToInt32(Session["fyearid"].ToString()))
                                select t).ToList();

            if (CostUpdation != null)
                ViewBag.CostUpdationDetails = CostUpdation;

            var CustomerReciept = (from t in entity.SPGetAllLocalCurrencyCustRecievable(Convert.ToInt32(Session["fyearid"].ToString())) select t).ToList();

            if (CustomerReciept != null)
                ViewBag.custReciavable = CustomerReciept;

            var SupplierPayble = (from t in entity.SPGetAllLocalCurrencyPayble(Convert.ToInt32(Session["fyearid"].ToString())) select t).ToList();
            if (SupplierPayble != null)

                ViewBag.Payble = SupplierPayble;

            //Session["Menu"] = Query;

            var a = entity.AcFinancialYearSelect(Convert.ToInt32(Session["branchid"])).ToList();
            ViewBag.AcFyear = a.ToList();
            ViewBag.YTDSummaryDetails = GetYTDSummaryDetails();

            Session["FinancialyearList"] = a.ToList();
            return View();

        }

        public JsonResult GetJobTypeOnDashBoard()
        {
            var JobType = (from t in entity.GETLOCALCostForDashboard(Convert.ToInt32(Session["fyearid"].ToString()))
                           select t).ToList();


            return Json(JobType, JsonRequestBehavior.AllowGet);
        }

        public List<YTDSummaryDetails> GetYTDSummaryDetails()
        {
            List<YTDSummaryDetails> objYTDSummary = new List<YTDSummaryDetails>();

            var data = entity.YTDSummary().ToList();

           
                YTDSummaryDetails obj = new YTDSummaryDetails();
                obj.Title = "YTD Summary";
                List<double> lst = new List<double>();
                foreach (var i in data) {lst.Add(Convert.ToDouble(i.Year));  }
                obj.Values = lst;
                objYTDSummary.Add(obj);

                obj = new YTDSummaryDetails();
                obj.Title = "JOB";
               lst = new List<double>();
                foreach (var i in data) { lst.Add(Convert.ToDouble(i.JOB)); }
                obj.Values = lst;
                objYTDSummary.Add(obj);

                obj = new YTDSummaryDetails();
                obj.Title = "BALANCE B/F";
                lst = new List<double>();
                foreach (var i in data) { lst.Add(Convert.ToDouble(i.Balance_B_F)); }
                obj.Values = lst;
                objYTDSummary.Add(obj);

                obj = new YTDSummaryDetails();
                obj.Title = "INVOICED";
                lst = new List<double>();
                foreach (var i in data) { lst.Add(Convert.ToDouble(i.INVOICED)); }
                obj.Values = lst;
                objYTDSummary.Add(obj);

                obj = new YTDSummaryDetails();
                obj.Title = "RECEIPTS";
                lst = new List<double>();
                foreach (var i in data) { lst.Add(Convert.ToDouble(i.RECEIPTS)); }
                obj.Values = lst;
                objYTDSummary.Add(obj);

                obj = new YTDSummaryDetails();
                obj.Title = "RECEIVABLES";
                lst = new List<double>();
                foreach (var i in data) { lst.Add(Convert.ToDouble(i.RECEIVABLES)); }
                obj.Values = lst;
                objYTDSummary.Add(obj);

                obj = new YTDSummaryDetails();
                obj.Title = "COST UPDATED";
                lst = new List<double>();
                foreach (var i in data) { lst.Add(Convert.ToDouble(i.COST_UPDATED)); }
                obj.Values = lst;
                objYTDSummary.Add(obj);

                obj = new YTDSummaryDetails();
                obj.Title = "PAYMENTS";
                lst = new List<double>();
                foreach (var i in data) { lst.Add(Convert.ToDouble(i.PAYMENTS)); }
                obj.Values = lst;
                objYTDSummary.Add(obj);

                obj = new YTDSummaryDetails();
                obj.Title = "PAYABLES";
                lst = new List<double>();
                foreach (var i in data) { lst.Add(Convert.ToDouble(i.PAYABLES)); }
                obj.Values = lst;
                objYTDSummary.Add(obj);

                return objYTDSummary;
            //List<YTDSummaryDetails> objYTDSummary = new List<YTDSummaryDetails>();
            //YTDSummaryDetails obj = new YTDSummaryDetails();
            //obj.Title = "YTD Summary";
            //obj.Values = new List<double>() { 2012, 2013, 2014, 2015, 2016 };
            //objYTDSummary.Add(obj);

            ////JOB
            //obj = new YTDSummaryDetails();
            //obj.Title = "JOB";
            //obj.Values = new List<double>() { 10, 20, 30, 40, 50 };
            //objYTDSummary.Add(obj);

            ////BALANCE B/F
            //obj = new YTDSummaryDetails();
            //obj.Title = "BALANCE B/F";
            //obj.Values = new List<double>() { 10, 20, 30, 40, 50 };
            //objYTDSummary.Add(obj);

            ////INVOICED
            //obj = new YTDSummaryDetails();
            //obj.Title = "INVOICED";
            //obj.Values = new List<double>() { 10, 20, 30, 40, 50 };
            //objYTDSummary.Add(obj);

            ////RECEIPTS
            //obj = new YTDSummaryDetails();
            //obj.Title = "RECEIPTS";
            //obj.Values = new List<double>() { 10, 20, 30, 40, 50 };
            //objYTDSummary.Add(obj);

            ////RECEIVABLES
            //obj = new YTDSummaryDetails();
            //obj.Title = "RECEIVABLES";
            //obj.Values = new List<double>() { 10, 20, 30, 40, 50 };
            //objYTDSummary.Add(obj);

            ////BALANCE B/F
            //obj = new YTDSummaryDetails();
            //obj.Title = "BALANCE B/F";
            //obj.Values = new List<double>() { 10, 20, 30, 40, 50 };
            //objYTDSummary.Add(obj);


            ////COST UPDATED
            //obj = new YTDSummaryDetails();
            //obj.Title = "COST UPDATED";
            //obj.Values = new List<double>() { 10, 20, 30, 40, 50 };
            //objYTDSummary.Add(obj);

            ////PAYMENTS
            //obj = new YTDSummaryDetails();
            //obj.Title = "PAYMENTS";
            //obj.Values = new List<double>() { 10, 20, 30, 40, 50 };
            //objYTDSummary.Add(obj);

            ////PAYABLES
            //obj = new YTDSummaryDetails();
            //obj.Title = "PAYABLES";
            //obj.Values = new List<double>() { 10, 20, 30, 40, 50 };
            //objYTDSummary.Add(obj);

           

        }


    }
}

public class YTDSummaryDetails
{
    public string Title { get; set; }
    public List<double> Values { get; set; }

}
