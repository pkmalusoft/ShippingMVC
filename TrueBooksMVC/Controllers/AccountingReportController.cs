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
    public class AccountingReportController : Controller
    {

        SHIPPING_FinalEntities context = new SHIPPING_FinalEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CashBookReport()
        {
            //ViewBag.cashhead = context.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();

            //ViewBag.cashhead = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
            ViewBag.cashhead = context.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString())).OrderBy(x => x.AcHead).ToList();
         
            return View();
        }

        public JsonResult GetCashBookReport(string fdate, string tdate, int acheadid)
        {
            //var data = context.Report_CashBook(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToDateTime(fdate), Convert.ToDateTime(tdate), acheadid, Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList().Take(0);
            var data = "";
            TempData["tdate"] = tdate;
            ViewBag.accounthead = (from a in context.AcHeads where a.AcHeadID == acheadid select a.AcHead1).FirstOrDefault();
         

            ViewBag.acheadid = acheadid;
            ViewBag.fromdate = fdate;
            ViewBag.todate = tdate;

            string view = this.RenderPartialView("_CashbookReport", data);


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



        public ActionResult BankBookReport()
        {
            //ViewBag.bankhead = context.AcHeadSelectForBank(Convert.ToInt32(Session["AcCompanyID"].ToString()));

            ViewBag.bankhead = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));

           
            return View();
        }


        public JsonResult GetBankBookReport(string fdate, string tdate, int acheadid)
        {
            var data = context.Report_CashAndBankBook(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToDateTime(fdate), Convert.ToDateTime(tdate), acheadid, Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList().Take(0);

            TempData["tdate"] = tdate;
            ViewBag.bankbookhead = (from a in context.AcHeads where a.AcHeadID == acheadid select a.AcHead1).FirstOrDefault();

            ViewBag.acheadid = acheadid;
            ViewBag.fromdate = fdate;
            ViewBag.todate = tdate;

            string view = this.RenderPartialView("_BankBookReport", data);

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

        public ActionResult Ledgerreport()
        {

            ViewBag.legherhead = context.AcHeads.ToList();
            return View();
        }


        public JsonResult GetLedgerreport(string fdate, string tdate, int acheadid)
        {
            var data = context.Report_Ledger(acheadid, Convert.ToInt32(Session["AcCompanyID"].ToString()), Convert.ToDateTime(fdate), Convert.ToDateTime(tdate), Convert.ToInt32(Session["fyearid"].ToString()));
            TempData["tdate"] = tdate;
            ViewBag.ledgerhead = (from a in context.AcHeads where a.AcHeadID == acheadid select a.AcHead1).FirstOrDefault();

            ViewBag.acheadid = acheadid;
            ViewBag.fromdate = fdate;
            ViewBag.todate = tdate;
            string view = this.RenderPartialView("_GetLedgerreport", data);

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

        public ActionResult ChartOfAccountReport()
        {


            return View(context.AcHeadSelectAll(1).ToList());
        }


        public ActionResult ProfitandLossreport()
        {
            //ViewBag.cashhead = context.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
            ViewBag.cashhead = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
            return View();
        }


        public JsonResult GetProfitAndLossReport(string fdate, string tdate)
        {
            var data = context.Report_ProfitAndLossAccount(Convert.ToDateTime(fdate), Convert.ToDateTime(tdate), 1, 1).ToList();
            TempData["tdate"] = tdate;
            string view = this.RenderPartialView("_GetProfitAndLossReport", data);

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

        public ActionResult AnalysisheadReport()
        {
            ViewBag.anlaysishead = context.AnalysisHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
            return View();
        }

        public JsonResult GetAnalysisreport(DateTime fdate, DateTime tdate, int AnalysisHeadID)
        {



            var data = context.Report_ProfitAndLossAccount(fdate, tdate, 1, 1).ToList();
            TempData["tdate"] = tdate;
            string view = this.RenderPartialView("_GetAnalysisreport", data);

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

        public ActionResult dayBookReport()
        {
            ViewBag.daybook = context.AcHeads.ToList().OrderBy(x => x.AcHead1).ToList();
         
            var opreation = new SelectList(new[] 
                                        {
                                            new { ID = "0", trans = "All" },
                                            new { ID = "1", trans = "Greater Then" },
                                             new { ID = "2", trans = "Less Then" },
                                              new { ID = "3", trans = "between" },
                                           
                                        },
                                   "ID", "trans", 1);

            


              return View();
        }
        public ActionResult GetDayBookreport(string fdate, string tdate, int acheadid, int? rentflag, int? montlyrent1, int? montlyrent2)
        {
            ViewBag.accounthead = (from a in context.AcHeads where a.AcHeadID == acheadid select a.AcHead1).FirstOrDefault();
            TempData["tdate"] = tdate;
            var data = context.Report_DayBook(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString()), Convert.ToDateTime(fdate), Convert.ToDateTime(tdate), Convert.ToInt32(acheadid), rentflag, montlyrent1, montlyrent2).Take(0);

            var voucher = data.Select(d => d.VoucherNo).Distinct();

            ViewBag.acheadid = acheadid;
            ViewBag.fromdate = fdate;
            ViewBag.todate = tdate;
            ViewBag.rentflag = rentflag;
            if (montlyrent1 == null)
                ViewBag.m1 = 0;
            else
                ViewBag.m1 = montlyrent1;

            if (montlyrent2 == null)
                ViewBag.m2 = 0;
            else
                ViewBag.m2 = montlyrent2;


         return PartialView("_GetDayBookreport", data);

            //return new JsonResult
            //{
            //    Data = new
            //    {
            //        success = true,
            //        view = view
            //    },
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //};

        }



        public ActionResult BalanceSheet()
        {
            return View();
        }

        public JsonResult GetBalanceSheet(string fdate, string tdate)
        {
            var data = context.Report_BalanceSheet(Convert.ToDateTime(fdate), Convert.ToDateTime(tdate), Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString()));

            TempData["tdate"] = tdate;

            ViewBag.fromdate = fdate;
            ViewBag.todate = tdate;
            string view = this.RenderPartialView("_GetBalanceSheet", data);

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


        public ActionResult GetTrialBalance()
        {
            
            return View();
        }

        public JsonResult TrialBalance(string fdate, string tdate)
        {
            var data = context.Report_TrialBalance(Convert.ToDateTime(fdate), Convert.ToDateTime(tdate), Convert.ToInt32(Session["AcCompanyID"].ToString()), Convert.ToInt32(Session["fyearid"].ToString()));
            TempData["tdate"] = tdate;

            ViewBag.fromdate = fdate;
            ViewBag.todate = tdate;

            string view = this.RenderPartialView("_GetTrialBalannce", data);
            
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

        public ActionResult ProfitLoss()
        {
            return View();
        }

        public JsonResult GetProfitAndLoss(string fdate, string tdate)
        {
            var data = context.Report_ProfitAndLossAccount(Convert.ToDateTime(fdate), Convert.ToDateTime(tdate), Convert.ToInt32(Session["AcCompanyID"].ToString()), Convert.ToInt32(Session["fyearid"].ToString()));
            TempData["tdate"] = tdate;
            string view = this.RenderPartialView("_ShowProfitAndLoss", data);

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


        public ActionResult xyztemp()
        {
            var data = context.Report_TrialBalance(Convert.ToDateTime("01 Jan 2016"), Convert.ToDateTime("31 Dec 2016"), Convert.ToInt32(Session["AcCompanyID"].ToString()), Convert.ToInt32(Session["fyearid"].ToString()));

            return View(data);

        }
      
    }
}
