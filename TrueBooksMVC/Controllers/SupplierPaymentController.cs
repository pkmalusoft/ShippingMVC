using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;
using DAL;
using System.Dynamic;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.xml.simpleparser;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using Newtonsoft.Json;
namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class SupplierPaymentController : Controller
    {
        MastersModel MM = new MastersModel();
        RecieptPaymentModel RP = new RecieptPaymentModel();
        SHIPPING_FinalEntities Context1 = new SHIPPING_FinalEntities();
        EditCommanFu editfu = new EditCommanFu();

        //
        // GET: /SupplierPayment/

        public ActionResult SupplierPayment(int id)
        {
            CustomerRcieptVM cust = new CustomerRcieptVM();
            var branchid = Convert.ToInt32(Session["branchid"]);

            if (Session["UserID"] != null)
            {

                if (id > 0)
                {

                    cust = RP.GetSupplierRecPayByRecpayID(id);
                    //cust.recPayDetail = Context1.RecPayDetails.Where(item => item.RecPayID == id).ToList();
                    //cust.CustomerRcieptChildVM=
                    //                   (from t in Context1.JInvoices  join
                    //                        p in Context1.RecPayDetails on t.InvoiceID equals p.InvoiceID
                    //                     join s in Context1.RecPays on p.RecPayID equals s.RecPayID
                    //                      join l in Context1.CostUpdationDetails on t.InvoiceID equals l.JInvoiceID
                    //                      where (s.RecPayID==id && p.InvoiceID!=0)
                    //           select new CustomerRcieptChildVM
                    //{
                    //   // InvoiceDate=Convert.ToDateTime(p.InvDate),
                    //    InvoiceID=t.InvoiceID,
                    //    AmountToBePaid=t.ProvisionHome.Value,
                    //    Amount=p.Amount.Value,
                    //    AmtPaidTillDate=l.AmountPaidTillDate.Value,
                    //    Balance = (t.ProvisionHome.Value - l.AmountPaidTillDate.Value),
                    //    RecPayDetailID=p.RecPayDetailID,
                    //    CurrencyId=p.CurrencyID.Value



                    //}).ToList();

                    /*   cust.CustomerRcieptChildVM = (from t in Context1.JobGenerations
                                                     join p in Context1.RecPayDetails on t.InvoiceNo equals p.InvoiceID
                                                     join s in Context1.JInvoices on t.InvoiceNo equals s.JobID
                                                     join r in Context1.RecPays on p.RecPayID equals r.RecPayID
                                                     where (r.RecPayID == id && p.InvoiceID != 0)
                                                     select new CustomerRcieptChildVM
                                                     {
                                                         InvoiceDate = r.RecPayDate,
                                                         InvoiceID = p.InvoiceID.Value,
                                                         AmountToBePaid = s.SalesHome.Value,
                                                         AmtPaidTillDate=(p.Amount),
                                                         Amount = (p.Amount.Value),

                                                         Balance = (s.SalesHome.Value)-(p.Amount.Value),
                                                         RecPayDetailID = p.RecPayDetailID,
                                                         CurrencyId = p.CurrencyID.Value

                                                     }).Distinct().OrderBy(x => x.InvoiceDate).ToList();*/
                    cust.CustomerRcieptChildVM = DAL.GetCustomerReceipt(id);

                    BindMasters_ForEdit(cust);
                    //ViewBag.achead = Context1.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString())).OrderBy(x => x.AcHead).ToList();
                    //ViewBag.acheadbank = Context1.AcHeadSelectForBank(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
                    var acheadforcash = (from d in Context1.AcHeads
                                         join
                                        s in Context1.AcGroups on d.AcGroupID equals s.AcGroupID
                                         join
                                         t in Context1.AcTypes on s.AcTypeId equals t.Id
                                         where
                                         t.AccountType.ToLower() == "cash" && s.AcBranchID == branchid
                                         select d).ToList();
                    var acheadforbank = (from d in Context1.AcHeads
                                         join
                                        s in Context1.AcGroups on d.AcGroupID equals s.AcGroupID
                                         join
                                         t in Context1.AcTypes on s.AcTypeId equals t.Id
                                         where
                                         t.AccountType.ToLower() == "bank" && s.AcBranchID == branchid
                                         select d).ToList();
                    ViewBag.achead = acheadforcash;
                    ViewBag.acheadbank = acheadforbank;
                }
                else
                {
                    BindMasters();
                    cust.RecPayDate = System.DateTime.UtcNow;
                    //ViewBag.achead = Context1.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
                    //ViewBag.acheadbank = Context1.AcHeadSelectForBank(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
                    var acheadforcash = (from d in Context1.AcHeads
                                         join
                                        s in Context1.AcGroups on d.AcGroupID equals s.AcGroupID
                                         join
                                         t in Context1.AcTypes on s.AcTypeId equals t.Id
                                         where
                                         t.AccountType.ToLower() == "cash" && s.AcBranchID == branchid
                                         select d).ToList();
                    var acheadforbank = (from d in Context1.AcHeads
                                         join
                                        s in Context1.AcGroups on d.AcGroupID equals s.AcGroupID
                                         join
                                         t in Context1.AcTypes on s.AcTypeId equals t.Id
                                         where
                                         t.AccountType.ToLower() == "bank" && s.AcBranchID == branchid
                                         select d).ToList();
                    ViewBag.achead = acheadforcash;
                    ViewBag.acheadbank = acheadforbank;
                }

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

            return View(cust);
        }


        //Commenting Code from Vishal as on 6 Dec 2016 to change logic
        //[HttpPost]
        //public ActionResult SupplierPayment_Old(CustomerRcieptVM RecP, string Command, string Currency)
        //{
        //    List<SP_GetSupplierCostDetailsForPayment_Result> AllInvoices = new List<SP_GetSupplierCostDetailsForPayment_Result>();
        //    List<SP_GetJInvoiceDetailsByInvoiceID_Result> InvDetails=new List<SP_GetJInvoiceDetailsByInvoiceID_Result>();
        //    int RPID = 0;
        //    int i = 0;
        //    RecP.FYearID = Convert.ToInt32(Session["fyearid"]);
        //    RecP.UserID = Convert.ToInt32(Session["UserID"]);

        //    if (RecP.RecPayID > 0)
        //    {
        //        RP.EditSupplierRecPay(RecP, Session["UserID"].ToString());
        //        RP.EditSupplierRecieptDetails(RecP.recPayDetail, RecP.RecPayID);

        //    }
        //    else
        //    {


        //        if (Session["UserID"] != null)
        //        {
        //            if (RecP.CashBank != null)
        //            {
        //                RecP.BankName = RecP.CashBank;
        //            }
        //            else
        //            {
        //                RecP.BankName = RecP.ChequeBank;

        //            }
        //            i = RP.AddCustomerRecieptPayment(RecP, Session["UserID"].ToString());

        //            if (i > 0)
        //            {
        //                RPID = RP.GetMaxRecPayID();
        //            }

        //            if (RPID > 0)
        //            {
        //                //RecPayDetails Records Inserting Logic

        //                //Total Amount calculating using ExRate and enterted Amount
        //                decimal TotalAmount = Convert.ToDecimal(RecP.EXRate * RecP.FMoney);
        //                decimal AdvanceAmtToinsert = TotalAmount;

        //                //Advance Amount getting for supplier
        //                decimal AdvanceAmount = RP.GetAdvanceAmount(Convert.ToInt32(RecP.SupplierID));

        //                //All Pending Invoices getting
        //                AllInvoices = RP.GetSupplierCostDetails(Convert.ToInt32(RecP.SupplierID));

        //                decimal InvoiceAmt = 0;

        //                foreach (var item in AllInvoices)
        //                {

        //                    var vAmount=RecP.CustomerRcieptChildVM.Where(item=>item.)

        //                    DateTime dateTime = Convert.ToDateTime(item.InvoiceDate);
        //                    string newd = Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd h:mm tt"); 
        //                    InvDetails = RP.InvDtls(item.InvoiceID);

        //                    foreach (var item1 in InvDetails)
        //                    {
        //                        InvoiceAmt = Convert.ToDecimal(item1.SalesForeign);
        //                    }

        //                    if (item.AmtPaidTillDate == 0)
        //                    {
        //                        if (AdvanceAmtToinsert >= item.AmountToBePaid)
        //                        {

        //                            int k = RP.InsertRecpayDetailsForSup(RPID, item.InvoiceID, Convert.ToDecimal(item.AmountToBePaid), "", "S", false, "", newd, item.InvoiceNo.ToString(), Convert.ToInt32(Currency), 3);

        //                            AdvanceAmtToinsert = AdvanceAmtToinsert - Convert.ToDecimal(item.AmountToBePaid);
        //                        }
        //                        else if (item.AmountToBePaid > AdvanceAmtToinsert)
        //                        {
        //                            int k = RP.InsertRecpayDetailsForSup(RPID, item.InvoiceID, Convert.ToDecimal(AdvanceAmtToinsert), "", "S", false, "", newd, item.InvoiceNo.ToString(), Convert.ToInt32(Currency), 2);

        //                            AdvanceAmtToinsert = AdvanceAmtToinsert - Convert.ToDecimal(AdvanceAmtToinsert);

        //                        }

        //                    }
        //                    else
        //                    {
        //                        if (AdvanceAmtToinsert >= item.Balance)
        //                        {
        //                            int k = RP.InsertRecpayDetailsForSup(RPID, item.InvoiceID, Convert.ToDecimal(AdvanceAmtToinsert), "", "S", false, "", newd, item.InvoiceNo.ToString(), Convert.ToInt32(Currency), 3);

        //                            AdvanceAmtToinsert = AdvanceAmtToinsert - Convert.ToDecimal(item.AmtPaidTillDate);
        //                        }
        //                        else if (item.Balance > AdvanceAmtToinsert)
        //                        {
        //                            int k = RP.InsertRecpayDetailsForSup(RPID, item.InvoiceID, Convert.ToDecimal(AdvanceAmtToinsert), "", "S", false, "", newd, item.InvoiceNo.ToString(), Convert.ToInt32(Currency), 2);

        //                            AdvanceAmtToinsert = AdvanceAmtToinsert - Convert.ToDecimal(AdvanceAmtToinsert);
        //                        }
        //                    }

        //                }

        //                int l = RP.InsertRecpayDetailsForSup(RPID, 0, -TotalAmount, "", "S", false, "", "", "", Convert.ToInt32(Currency), 4);

        //                //Advance Amount entry
        //                if (AdvanceAmtToinsert > 0)
        //                {
        //                    int k = RP.InsertRecpayDetailsForSup(RPID, 0, -AdvanceAmtToinsert, "", "S", true, "", "", "", Convert.ToInt32(Currency), 4);
        //                }
        //                int fyaerId = Convert.ToInt32(Session["fyearid"].ToString());
        //                RP.InsertJournalOfSupplier(RPID, fyaerId);
        //            }

        //        }
        //        else
        //        {
        //            return RedirectToAction("Login", "Login");
        //        }
        //    }

        //    return RedirectToAction("SupplierPaymentDetails", "SupplierPayment", new { ID = RPID });

        //}



        [HttpPost]
        public ActionResult SupplierPayment(CustomerRcieptVM RecP, string Command, string Currency)
        {
            int RPID = 0;
            int i = 0;
            RecP.FYearID = Convert.ToInt32(Session["fyearid"]);
            RecP.UserID = Convert.ToInt32(Session["UserID"]);

            //Edit Record
            //if (RecP.RecPayID > 0)
            //{
              // RP.EditSupplierRecPay(RecP, Session["UserID"].ToString());
            //    RP.EditSupplierRecieptDetails(RecP.recPayDetail, RecP.RecPayID);
            //}
            //add Record

            {
                 if (RecP.CashBank != null)
                {
                   
                    RecP.StatusEntry = "CS";
                    int acheadid=Convert.ToInt32(RecP.CashBank);
                     var achead=(from t in Context1.AcHeads where t.AcHeadID==acheadid select t.AcHead1).FirstOrDefault();
                    RecP.BankName = achead;
                }
                else
                {
                   
                    RecP.StatusEntry = "BK";
                     int acheadid=Convert.ToInt32(RecP.ChequeBank);
                     var achead=(from t in Context1.AcHeads where t.AcHeadID==acheadid select t.AcHead1).FirstOrDefault();
                    RecP.BankName = achead;
                }
               

                    //Adding Entry in Rec PAY
                   // RPID = RP.AddCustomerRecieptPayment(RecP, Session["UserID"].ToString());
                   // RecP.RecPayID = (from c in Context1.RecPays orderby c.RecPayID descending select c.RecPayID).FirstOrDefault();
                    ///Insert Entry For RecPay Details 
                    ///
                    if (RecP.RecPayID <= 0)
                    {

                        decimal Fmoney = 0;
                        for (int j = 0; j < RecP.CustomerRcieptChildVM.Count; j++)
                        {
                            if (RecP.CustomerRcieptChildVM[j].Amount != null)
                            {
                                Fmoney = Fmoney + RecP.CustomerRcieptChildVM[j].Amount;
                            }
                        }

                        RecP.FMoney = Fmoney;

                        RPID = RP.AddCustomerRecieptPayment(RecP, Session["UserID"].ToString());
                        RecP.RecPayID = (from c in Context1.RecPays orderby c.RecPayID descending select c.RecPayID).FirstOrDefault();
                        decimal TotalAmount = 0;
                        foreach (var item in RecP.CustomerRcieptChildVM)
                        {
                            decimal Advance = 0;
                            //if (item.Amount > 0 && (item.AmountToBePaid < item.Amount || item.AmountToBePaid == item.Amount))///900<1000
                            //{
                            //item.Amount = Convert.ToDecimal(RecP.EXRate * item.Amount);
                            //100=1000-900
                            Advance = item.Amount - item.AmountToBePaid;
                           // DateTime vInvoiceDate = Convert.ToDateTime();
                            string vInvoiceDate1 = (item.InvoiceDate).ToString();
                            RP.InsertRecpayDetailsForSup(RecP.RecPayID, item.InvoiceID, item.InvoiceID, Convert.ToDecimal(item.Amount), "", "S", false, "", vInvoiceDate1, "", Convert.ToInt32(Currency), 3, item.JobID);

                            if (Advance > 0)
                            {
                                //Advance Amount entry
                                int k = RP.InsertRecpayDetailsForSup(RecP.RecPayID,0,0, Advance, "", "S", true, "", "", "", Convert.ToInt32(Currency), 4, item.JobID);
                            }

                            TotalAmount = TotalAmount + item.Amount;
                            //}

                        }

                        //To Balance Invoice AMount
                        if (TotalAmount > 0)
                        {
                            RP.InsertRecpayDetailsForSup(RecP.RecPayID, 0,0, -TotalAmount, "", "S", false, "", "", "", Convert.ToInt32(Currency), 4,0);

                            int fyaerId = Convert.ToInt32(Session["fyearid"].ToString());
                            RP.InsertJournalOfSupplier(RecP.RecPayID, fyaerId);
                        }
                    var Recpaydata = (from d in Context1.RecPays where d.RecPayID == RecP.RecPayID select d).FirstOrDefault();

                    Recpaydata.RecPayID = RecP.RecPayID;
                    Recpaydata.IsTradingReceipt = false;
                    Context1.Entry(Recpaydata).State = EntityState.Modified;
                    Context1.SaveChanges();

                }
                        //Edit code
                    else
                    {
                        
                        RecPay recpay = new RecPay();
                        recpay.RecPayDate = RecP.RecPayDate;
                        recpay.RecPayID = RecP.RecPayID;
                       
                        recpay.AcJournalID = RecP.AcJournalID;
                        recpay.BankName = RecP.BankName;
                        recpay.ChequeDate = RecP.ChequeDate;
                        recpay.ChequeNo = RecP.ChequeNo;
                        //recpay.CustomerID = RecP.CustomerID;
                        recpay.DocumentNo = RecP.DocumentNo;
                        recpay.EXRate = RecP.EXRate;
                        recpay.FYearID = RecP.FYearID;
                        recpay.SupplierID = RecP.SupplierID;
                        recpay.IsTradingReceipt = false;

                        decimal Fmoney = 0;
                        for (int j = 0; j < RecP.CustomerRcieptChildVM.Count; j++)
                        {
                            if (RecP.CustomerRcieptChildVM[j].Amount != null)
                            {
                                Fmoney = Fmoney + RecP.CustomerRcieptChildVM[j].Amount;
                            }
                        }

                        recpay.FMoney = Fmoney;
                        recpay.StatusEntry = RecP.StatusEntry;
                        Context1.Entry(recpay).State = EntityState.Modified;
                        Context1.SaveChanges();

                       

                       
                        foreach (var item in RecP.CustomerRcieptChildVM)
                        {


                            RecPayDetail recpd = new RecPayDetail();
                            recpd.RecPayDetailID = item.RecPayDetailID;
                            recpd.Amount = item.Amount;
                            recpd.CurrencyID = item.CurrencyId;
                            recpd.InvDate = (item.InvoiceDate).ToString();
                            recpd.RecPayID = RecP.RecPayID;
                            recpd.Remarks = item.Remarks;
                            recpd.InvoiceID = item.InvoiceID;
                            recpd.StatusInvoice = "S";
                          /*  if (item.AmountToBePaid < item.Amount)
                            {
                                RecPayDetail recpd1 = new RecPayDetail();
                                recpd1.RecPayDetailID = (from c in Context1.RecPayDetails orderby c.RecPayDetailID descending select c.RecPayDetailID).FirstOrDefault();
                                recpd1.Amount = item.AmountToBePaid - item.Amount;
                                recpd1.RecPayID = RecP.RecPayID;
                                recpd1.Remarks = item.Remarks;
                                recpd1.CurrencyID = item.CurrencyId;
                                recpd1.StatusAdvance = true;
                                recpd.StatusInvoice = "S";
                                Context1.Entry(recpd1).State = EntityState.Modified;
                                Context1.SaveChanges();
                            }*/
                            Context1.Entry(recpd).State = EntityState.Modified;
                            Context1.SaveChanges();

                            //int editAcJdetails = editfu.EditAcJDetails(RecP.AcJournalID.Value, item.Amount);
                        }

                        int editrecPay = 0;
                        var sumOfAmount = Context1.RecPayDetails.Where(m => m.RecPayID == RecP.RecPayID && m.InvoiceID != 0).Sum(c => c.Amount);
                        editrecPay = editfu.EditRecpayDetailsCustR(RecP.RecPayID, Convert.ToInt32(sumOfAmount));
                        int editAcJdetails = editfu.EditAcJDetails(RecP.AcJournalID.Value, Convert.ToInt32(sumOfAmount));
                    
                            
                    }
                }
                
            return RedirectToAction("SupplierPaymentDetails", "SupplierPayment", new { ID = RecP.RecPayID });

        }


        public JsonResult GetExchangeRateByCurID(string ID)
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();

            var ER = RP.GetExchgeRateByCurID(Convert.ToInt32(ID));

            return Json(ER, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeleteSupplierDet(int id)
        {
            // int k = 0;
            if (id != 0)
            {
                RP.DeleteSupplierDetails(id);
            }

            return RedirectToAction("SupplierPaymentDetails", "SupplierPayment", new { ID = 10 });

        }
        [HttpGet]
        public ActionResult DeleteTradeSupplierDet(int id)
        {
            // int k = 0;
            if (id != 0)
            {
                RP.DeleteSupplierDetails(id);
            }

            return RedirectToAction("SupplierTradePaymentDetails", "SupplierPayment", new { ID = 10 });

        }

        public string getSuccessID()
        {
            string ID = "";

            if (Session["SPID"] != null)
            {
                ID = Session["SPID"].ToString();
            }

            return ID;
        }


        public JsonResult GetCostOfSupplier(string ID)
        {
            //List<SP_GetSupplierCostDetailsForPayment_Result> Costs = new List<SP_GetSupplierCostDetailsForPayment_Result>();

            var Costs = RP.GetSupplierCostDetails(Convert.ToInt32(ID)).Distinct().OrderBy(x => x.InvoiceDate);

            return Json(Costs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SupplierPaymentDetails(int ID)
        {
            List<SP_GetAllPaymentsDetails_Result> Payments = new List<SP_GetAllPaymentsDetails_Result>();

            Payments = RP.GetAllPayments();
            var data = (from t in Payments where (t.RecPayDate >= Convert.ToDateTime(Session["FyearFrom"]) && t.RecPayDate <= Convert.ToDateTime(Session["FyearTo"])) select t).ToList();

            if (ID > 0)
            {
                ViewBag.SuccessMsg = "You have successfully added Supplier payment.";
            }

            if (ID == 10)
            {
                ViewBag.SuccessMsg = "You have successfully deleted Supplier payment.";
            }

            if (ID == 20)
            {
                ViewBag.SuccessMsg = "You have successfully updated Supplier payment.";
            }


            Session["SPID"] = ID;

            return View(Payments);
        }

        public void BindMasters()
        {
            List<SP_GetAllSupplier_Result> Suppliers = new List<SP_GetAllSupplier_Result>();
            Suppliers = MM.GetAllSupplier();

            List<SP_GetCurrency_Result> Currencys = new List<SP_GetCurrency_Result>();
            Currencys = MM.GetCurrency();

            string DocNo = RP.GetMaxPaymentDocumentNo();

            ViewBag.DocumentNos = DocNo;

            ViewBag.Supplier = new SelectList(Suppliers.OrderBy(x => x.SupplierName).ToList(), "SupplierID", "SupplierName");

            ViewBag.Currency = new SelectList(Currencys.OrderBy(x => x.CurrencyName).ToList(), "CurrencyID", "CurrencyName");
        }
        public void BindMasters_ForEdit(CustomerRcieptVM cust)
        {
            List<SP_GetAllSupplier_Result> Suppliers = new List<SP_GetAllSupplier_Result>();
            Suppliers = MM.GetAllSupplier();

            List<SP_GetCurrency_Result> Currencys = new List<SP_GetCurrency_Result>();
            Currencys = MM.GetCurrency();
            ViewBag.DocumentNos = cust.DocumentNo;

            ViewBag.Supplier = new SelectList(Suppliers.OrderBy(x => x.SupplierName).ToList(), "SupplierID", "SupplierName", cust.SupplierID);

            ViewBag.Currency = new SelectList(Currencys.OrderBy(x => x.CurrencyName).ToList(), "CurrencyID", "CurrencyName", cust.CurrencyId);

        }

        public JsonResult GetAllCurrencySupplierPayble()
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            // var AllInvoices;


            var CostReciept = (from t in Context1.SPGetAllLocalCurrencyPayble(Convert.ToInt32(Session["fyearid"].ToString()))
                               select t).ToList();
            return Json(CostReciept, JsonRequestBehavior.AllowGet);



        }

        public JsonResult GetSupplier()
        {

            DateTime d = DateTime.Now;
            DateTime fyear = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime mstart = new DateTime(fyear.Year, d.Month, 01);

            int maxday = DateTime.DaysInMonth(fyear.Year, d.Month);
            DateTime mend = new DateTime(fyear.Year, d.Month, maxday);

            var Spayment = Context1.SP_GetAllPaymentsDetails().Where(x => x.RecPayDate >= mstart && x.RecPayDate <= mend).OrderByDescending(x => x.RecPayDate).ToList();


            string view = this.RenderPartialView("_GetSupplier", Spayment);
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

        //public JsonResult GetSupplierByDate(string fdate, string tdate, int FYearID)
        //{

        //    DateTime d = DateTime.Now;
        //    DateTime fyear = Convert.ToDateTime(Session["FyearFrom"].ToString());
        //    DateTime mstart = new DateTime(fyear.Year, d.Month, 01);

        //    int maxday = DateTime.DaysInMonth(fyear.Year, d.Month);
        //    DateTime mend = new DateTime(fyear.Year, d.Month, maxday);

        //    var Spayment = Context1.SP_GetAllPaymentsDetailsByDate(fdate, tdate, FYearID).ToList();


        //    string view = this.RenderPartialView("_GetAllSupplierByDate", Spayment);
        //    return new JsonResult
        //    {
        //        Data = new
        //        {
        //            success = true,
        //            view = view
        //        },
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    };
        //}

        public ActionResult GetSupplierByDate(string fdate, string tdate, int FYearID)
        {

            DateTime d = DateTime.Now;
            DateTime fyear = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime mstart = new DateTime(fyear.Year, d.Month, 01);

            int maxday = DateTime.DaysInMonth(fyear.Year, d.Month);
            DateTime mend = new DateTime(fyear.Year, d.Month, maxday);
            var sdate = DateTime.Parse(fdate);
            var edate = DateTime.Parse(tdate);
            //var Spayment = Context1.SP_GetAllPaymentsDetailsByDate(fdate, tdate, FYearID).ToList();
            var data = Context1.RecPays.Where(x => x.RecPayDate >= sdate && x.RecPayDate <= edate && x.SupplierID != null && x.IsTradingReceipt == false).OrderByDescending(x => x.RecPayDate).ToList();
            ViewBag.Suppliers = Context1.Suppliers.ToList();
            data.ForEach(s => s.Remarks = (from x in Context1.RecPayDetails where x.RecPayID == s.RecPayID && (x.CurrencyID != null || x.CurrencyID > 0) select x).FirstOrDefault() != null ? (from x in Context1.RecPayDetails join C in Context1.CurrencyMasters on x.CurrencyID equals C.CurrencyID where x.RecPayID == s.RecPayID && (x.CurrencyID != null || x.CurrencyID > 0) select C.CurrencyName).FirstOrDefault() : "");

            return PartialView("_GetAllSupplierByDate", data);
           
        }
        public JsonResult GetCostOfTradeSupplier(int? ID)
        {
            //List<SP_GetSupplierCostDetailsForPayment_Result> Costs = new List<SP_GetSupplierCostDetailsForPayment_Result>();

            var Costs = RP.GetSupplierCostDetails(Convert.ToInt32(ID)).Distinct().OrderBy(x => x.InvoiceDate);
            DateTime fromdate = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime todate = Convert.ToDateTime(Session["FyearTo"].ToString());
            var AllInvoices = (from d in Context1.PurchaseInvoices where d.SupplierID == ID select d).ToList();
            var salesinvoice = new List<CostUpdationTradeDetailVM>();
            foreach (var item in AllInvoices)
            {
                var invoicedeails = (from d in Context1.PurchaseInvoiceDetails where d.PurchaseInvoiceID == item.PurchaseInvoiceID where (d.RecPayStatus < 2 || d.RecPayStatus == null) select d).ToList();
                foreach (var det in invoicedeails)
                {
                    var allrecpay = (from d in Context1.RecPayDetails where d.InvoiceID == det.PurchaseInvoiceDetailID select d).ToList();
                    var totamtpaid = allrecpay.Sum(d => d.Amount);
                    var totadjust = allrecpay.Sum(d => d.AdjustmentAmount);
                    var Debitnote = (from d in Context1.DebitNotes where d.InvoiceID == det.PurchaseInvoiceDetailID && d.SupplierID == item.SupplierID select d).ToList();
                    decimal? DebitAmount = 0;
                    if (Debitnote.Count > 0)
                    {
                        DebitAmount = Debitnote.Sum(d => d.Amount);
                    }
                    var totamt = totamtpaid + totadjust+DebitAmount;
                    var Invoice = new CostUpdationTradeDetailVM();
                    Invoice.JobID = det.JobID;
                    Invoice.JobCode = "";
                    Invoice.InvoiceID =Convert.ToInt32(det.PurchaseInvoiceID);
                    Invoice.InvoiceNo = item.PurchaseInvoiceNo;
                    Invoice.PurchaseInvoiceDetailId = det.PurchaseInvoiceDetailID;
                    Invoice.InvoiceAmount = det.NetValue;
                    Invoice.DateTime = item.PurchaseInvoiceDate.Value.ToString("dd/MM/yyyy");
                    var RecPay = (from d in Context1.RecPayDetails where d.RecPayDetailID == det.RecPayDetailId select d).FirstOrDefault();

                    Invoice.AmountPaidTillDate = totamt;
                    Invoice.Balance = Invoice.InvoiceAmount - totamtpaid;
                    Invoice.AdjustmentAmount = totadjust;


                    salesinvoice.Add(Invoice);
                }
            }
            return Json(salesinvoice, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SupplierTradePayment(int id)
        {
            CustomerRcieptVM cust = new CustomerRcieptVM();
            var branchid = Convert.ToInt32(Session["branchid"]);

            if (Session["UserID"] != null)
            {

                if (id > 0)
                {


                    cust = RP.GetSupplierRecPayByRecpayID(id);
                    cust.recPayDetail = Context1.RecPayDetails.Where(item => item.RecPayID == id).ToList();
                    cust.CustomerRcieptChildVM = new List<CustomerRcieptChildVM>();
                    foreach (var item in cust.recPayDetail)
                    {
                        if (item.InvoiceID > 0)
                        {
                            var sInvoiceDetail = (from d in Context1.PurchaseInvoiceDetails where d.PurchaseInvoiceDetailID == item.InvoiceID select d).FirstOrDefault();
                            var Sinvoice = (from d in Context1.PurchaseInvoices where d.PurchaseInvoiceID == sInvoiceDetail.PurchaseInvoiceID select d).FirstOrDefault();
                            var allrecpay = (from d in Context1.RecPayDetails where d.InvoiceID == item.InvoiceID select d).ToList();
                            var Debitnote = (from d in Context1.DebitNotes where d.InvoiceID == item.InvoiceID && d.SupplierID == cust.SupplierID select d).ToList();
                            decimal? DebitAmount = 0;
                            if(Debitnote.Count>0)
                            {
                                DebitAmount = Debitnote.Sum(d => d.Amount);
                            }
                            var totamtpaid = allrecpay.Sum(d => d.Amount);
                            var totadjust = allrecpay.Sum(d => d.AdjustmentAmount);
                            var totamt = totamtpaid + totadjust + DebitAmount;
                            var customerinvoice = new CustomerRcieptChildVM();
                            customerinvoice.InvoiceID = Convert.ToInt32(item.InvoiceID);
                            customerinvoice.SInvoiceNo = Sinvoice.PurchaseInvoiceNo;
                            customerinvoice.strDate = Convert.ToDateTime(item.InvDate).ToString("dd/MM/yyyy");
                            customerinvoice.AmountToBePaid = Convert.ToDecimal(sInvoiceDetail.NetValue);
                            customerinvoice.Amount = Convert.ToDecimal(item.Amount);
                            customerinvoice.Balance = Convert.ToDecimal(sInvoiceDetail.NetValue - totamt);
                            customerinvoice.RecPayDetailID = item.RecPayDetailID;
                            customerinvoice.AmountToBeRecieved = Convert.ToDecimal(totamtpaid);
                            customerinvoice.RecPayID = Convert.ToInt32(item.RecPayID);
                            customerinvoice.AdjustmentAmount = Convert.ToDecimal(item.AdjustmentAmount);
                            customerinvoice.InvoiceDate = Sinvoice.PurchaseInvoiceDate;
                            cust.CustomerRcieptChildVM.Add(customerinvoice);
                        }
                    }
                    BindMasters_ForEdit(cust);



                   
                    //cust.CustomerRcieptChildVM = DAL.GetCustomerReceipt(id);

                    BindMasters_ForEdit(cust);
                    //ViewBag.achead = Context1.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString())).OrderBy(x => x.AcHead).ToList();
                    //ViewBag.acheadbank = Context1.AcHeadSelectForBank(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
                    var acheadforcash = (from d in Context1.AcHeads
                                         join
                                        s in Context1.AcGroups on d.AcGroupID equals s.AcGroupID
                                         join
                                         t in Context1.AcTypes on s.AcTypeId equals t.Id
                                         where
                                         t.AccountType.ToLower() == "cash" && s.AcBranchID == branchid
                                         select d).ToList();
                    var acheadforbank = (from d in Context1.AcHeads
                                         join
                                        s in Context1.AcGroups on d.AcGroupID equals s.AcGroupID
                                         join
                                         t in Context1.AcTypes on s.AcTypeId equals t.Id
                                         where
                                         t.AccountType.ToLower() == "bank" && s.AcBranchID == branchid
                                         select d).ToList();
                    ViewBag.achead = acheadforcash;
                    ViewBag.acheadbank = acheadforbank;
                }
                else
                {
                    BindMasters();
                    cust.RecPayDate = System.DateTime.UtcNow;
                    //ViewBag.achead = Context1.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
                    //ViewBag.acheadbank = Context1.AcHeadSelectForBank(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
                    var acheadforcash = (from d in Context1.AcHeads
                                         join
                                        s in Context1.AcGroups on d.AcGroupID equals s.AcGroupID
                                         join
                                         t in Context1.AcTypes on s.AcTypeId equals t.Id
                                         where
                                         t.AccountType.ToLower() == "cash" && s.AcBranchID == branchid
                                         select d).ToList();
                    var acheadforbank = (from d in Context1.AcHeads
                                         join
                                        s in Context1.AcGroups on d.AcGroupID equals s.AcGroupID
                                         join
                                         t in Context1.AcTypes on s.AcTypeId equals t.Id
                                         where
                                         t.AccountType.ToLower() == "bank" && s.AcBranchID == branchid
                                         select d).ToList();
                    ViewBag.achead = acheadforcash;
                    ViewBag.acheadbank = acheadforbank;
                }

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

            return View(cust);
        }
        public ActionResult SupplierTradePaymentDetails(int ID)
        {
            List<SP_GetAllPaymentsDetails_Result> Payments = new List<SP_GetAllPaymentsDetails_Result>();

            Payments = RP.GetAllPayments();
            //var data = (from t in Payments where (t.RecPayDate >= Convert.ToDateTime(Session["FyearFrom"]) && t.RecPayDate <= Convert.ToDateTime(Session["FyearTo"])) 
            //            select t).ToList();

            if (ID > 0)
            {
                ViewBag.SuccessMsg = "You have successfully added Supplier payment.";
            }

            if (ID == 10)
            {
                ViewBag.SuccessMsg = "You have successfully deleted Supplier payment.";
            }

            if (ID == 20)
            {
                ViewBag.SuccessMsg = "You have successfully updated Supplier payment.";
            }


            Session["SPID"] = ID;

            return View(Payments);
        }



        public ActionResult GetTradeSupplierByDate(string fdate, string tdate, int FYearID)
        {

            DateTime d = DateTime.Now;
            DateTime fyear = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime mstart = new DateTime(fyear.Year, d.Month, 01);

            int maxday = DateTime.DaysInMonth(fyear.Year, d.Month);
            DateTime mend = new DateTime(fyear.Year, d.Month, maxday);
            var sdate = DateTime.Parse(fdate);
            var edate = DateTime.Parse(tdate);
            //var Spayment = Context1.SP_GetAllPaymentsDetailsByDate(fdate, tdate, FYearID).ToList();
            var data = Context1.RecPays.Where(x => x.RecPayDate >= sdate && x.RecPayDate <= edate && x.SupplierID !=null && x.IsTradingReceipt == true).OrderByDescending(x => x.RecPayDate).ToList();
            ViewBag.Suppliers = Context1.Suppliers.ToList();

            data.ForEach(s => s.Remarks = (from x in Context1.RecPayDetails where x.RecPayID == s.RecPayID && (x.CurrencyID != null || x.CurrencyID > 0) select x).FirstOrDefault() != null ? (from x in Context1.RecPayDetails join C in Context1.CurrencyMasters on x.CurrencyID equals C.CurrencyID where x.RecPayID == s.RecPayID && (x.CurrencyID != null || x.CurrencyID > 0) select C.CurrencyName).FirstOrDefault() : "");

            return PartialView("_GetAllTradeSupplierByDate", data);

        }
        [HttpPost]
        public ActionResult SupplierTradePayment(CustomerRcieptVM RecP, string Command, string Currency)
        {
            int RPID = 0;
            int i = 0;
            RecP.FYearID = Convert.ToInt32(Session["fyearid"]);
            RecP.UserID = Convert.ToInt32(Session["UserID"]);
            //Edit Record
            //if (RecP.RecPayID > 0)
            //{
            // RP.EditSupplierRecPay(RecP, Session["UserID"].ToString());
            //    RP.EditSupplierRecieptDetails(RecP.recPayDetail, RecP.RecPayID);
            //}
            //add Record

            {
                if (RecP.CashBank != null)
                {

                    RecP.StatusEntry = "CS";
                    int acheadid = Convert.ToInt32(RecP.CashBank);
                    var achead = (from t in Context1.AcHeads where t.AcHeadID == acheadid select t.AcHead1).FirstOrDefault();
                    RecP.BankName = achead;
                }
                else
                {

                    RecP.StatusEntry = "BK";
                    int acheadid = Convert.ToInt32(RecP.ChequeBank);
                    var achead = (from t in Context1.AcHeads where t.AcHeadID == acheadid select t.AcHead1).FirstOrDefault();
                    RecP.BankName = achead;
                }


                //Adding Entry in Rec PAY
                // RPID = RP.AddCustomerRecieptPayment(RecP, Session["UserID"].ToString());
                // RecP.RecPayID = (from c in Context1.RecPays orderby c.RecPayID descending select c.RecPayID).FirstOrDefault();
                ///Insert Entry For RecPay Details 
                ///
                if (RecP.RecPayID <= 0)
                {

                    decimal Fmoney = 0;
                    for (int j = 0; j < RecP.CustomerRcieptChildVM.Count; j++)
                    {
                        if (RecP.CustomerRcieptChildVM[j].Amount != null)
                        {
                            Fmoney = Fmoney + RecP.CustomerRcieptChildVM[j].Amount;
                        }
                    }

                    RecP.FMoney = Fmoney;

                    RPID = RP.AddCustomerRecieptPayment(RecP, Session["UserID"].ToString());
                    RecP.RecPayID = (from c in Context1.RecPays orderby c.RecPayID descending select c.RecPayID).FirstOrDefault();
                    decimal TotalAmount = 0;
                    foreach (var item in RecP.CustomerRcieptChildVM)
                    {
                        decimal Advance = 0;
                        //if (item.Amount > 0 && (item.AmountToBePaid < item.Amount || item.AmountToBePaid == item.Amount))///900<1000
                        //{
                        //item.Amount = Convert.ToDecimal(RecP.EXRate * item.Amount);
                        //100=1000-900
                        Advance = item.Amount - item.AmountToBePaid;
                        // DateTime vInvoiceDate = Convert.ToDateTime();
                        string vInvoiceDate1 = (item.InvoiceDate).ToString();


                        if (item.Amount > 0 || item.AdjustmentAmount > 0)
                        {
                            var maxrecpaydetailid = (from c in Context1.RecPayDetails orderby c.RecPayDetailID descending select c.RecPayDetailID).FirstOrDefault();

                            RP.InsertRecpayDetailsForSup(RecP.RecPayID, item.InvoiceID, item.InvoiceID, Convert.ToDecimal(item.Amount), "", "S", false, "", vInvoiceDate1, "", Convert.ToInt32(Currency), 3, item.JobID);
                            var recpaydetail = (from d in Context1.RecPayDetails where d.RecPayDetailID == maxrecpaydetailid + 1 select d).FirstOrDefault();
                            var recpd = recpaydetail;
                            recpaydetail.AdjustmentAmount = item.AdjustmentAmount;
                            Context1.Entry(recpd).State = EntityState.Modified;
                            Context1.SaveChanges();
                            if (Advance > 0)
                            {
                                //Advance Amount entry
                                RP.InsertRecpayDetailsForCust(RecP.RecPayID, 0, 0, Advance, null, "C", true, null, null, null, Convert.ToInt32(RecP.CurrencyId), 4, item.JobID);
                            }
                            var salesinvoicedetails = (from d in Context1.PurchaseInvoiceDetails where d.PurchaseInvoiceDetailID == item.InvoiceID select d).FirstOrDefault();
                            var Debitnote = (from d in Context1.DebitNotes where d.InvoiceID == item.InvoiceID && d.SupplierID == RecP.SupplierID select d).ToList();
                            decimal? DebitAmount = 0;
                            if (Debitnote.Count > 0)
                            {
                                DebitAmount = Debitnote.Sum(d => d.Amount);
                            }
                            var totamount = (from d in Context1.RecPayDetails where d.InvoiceID == salesinvoicedetails.PurchaseInvoiceDetailID select d).ToList();
                            var totsum = totamount.Sum(d => d.Amount);
                            var totAdsum = totamount.Sum(d => d.AdjustmentAmount);
                            var tamount = totsum + totAdsum+DebitAmount;
                            if (tamount >= salesinvoicedetails.NetValue)
                            {
                                salesinvoicedetails.RecPayStatus = 2;
                            }
                            else
                            {
                                salesinvoicedetails.RecPayStatus = 1;
                            }
                            salesinvoicedetails.RecPayDetailId = maxrecpaydetailid + 1;
                            var data = salesinvoicedetails;
                            Context1.Entry(salesinvoicedetails).State = EntityState.Modified;
                            Context1.SaveChanges();

                        }


                        if (Advance > 0)
                        {
                            //Advance Amount entry
                            int k = RP.InsertRecpayDetailsForSup(RecP.RecPayID, 0, 0, Advance, "", "S", true, "", "", "", Convert.ToInt32(Currency), 4, item.JobID);
                        }

                        TotalAmount = TotalAmount + item.Amount;
                        //}

                    }

                    //To Balance Invoice AMount
                    if (TotalAmount > 0)
                    {
                        RP.InsertRecpayDetailsForSup(RecP.RecPayID, 0, 0, -TotalAmount, "", "S", false, "", "", "", Convert.ToInt32(Currency), 4, 0);

                        int fyaerId = Convert.ToInt32(Session["fyearid"].ToString());
                        RP.InsertJournalOfSupplier(RecP.RecPayID, fyaerId);
                    }
                    var Recpaydata = (from d in Context1.RecPays where d.RecPayID == RecP.RecPayID select d).FirstOrDefault();

                    Recpaydata.RecPayID = RecP.RecPayID;
                    Recpaydata.IsTradingReceipt = true;
                    Context1.Entry(Recpaydata).State = EntityState.Modified;
                    Context1.SaveChanges();

                }
                //Edit code
                else
                {
                    
                    RecPay recpay = new RecPay();
                    recpay.RecPayDate = RecP.RecPayDate;
                    recpay.RecPayID = RecP.RecPayID;

                    recpay.AcJournalID = RecP.AcJournalID;
                    recpay.BankName = RecP.BankName;
                    recpay.ChequeDate = RecP.ChequeDate;
                    recpay.ChequeNo = RecP.ChequeNo;
                    //recpay.CustomerID = RecP.CustomerID;
                    recpay.DocumentNo = RecP.DocumentNo;
                    recpay.EXRate = RecP.EXRate;
                    recpay.FYearID = RecP.FYearID;
                    recpay.SupplierID = RecP.SupplierID;

                    decimal Fmoney = 0;
                    for (int j = 0; j < RecP.CustomerRcieptChildVM.Count; j++)
                    {
                        if (RecP.CustomerRcieptChildVM[j].Amount != null)
                        {
                            Fmoney = Fmoney + RecP.CustomerRcieptChildVM[j].Amount;
                        }
                    }

                    recpay.FMoney = Fmoney;
                    recpay.StatusEntry = RecP.StatusEntry;
                    recpay.IsTradingReceipt = true;

                    Context1.Entry(recpay).State = EntityState.Modified;
                    Context1.SaveChanges();




                    foreach (var item in RecP.CustomerRcieptChildVM)
                    {


                        RecPayDetail recpd = new RecPayDetail();
                        recpd.RecPayDetailID = item.RecPayDetailID;
                        recpd.Amount = (item.Amount);
                        recpd.CurrencyID = item.CurrencyId;
                        recpd.InvDate = (item.InvoiceDate).ToString();
                        recpd.RecPayID = RecP.RecPayID;
                        recpd.Remarks = item.Remarks;
                        recpd.InvoiceID = item.InvoiceID;
                        recpd.StatusInvoice = "S";
                        Context1.Entry(recpd).State = EntityState.Modified;
                        Context1.SaveChanges();
                        var salesinvoicedetails = (from d in Context1.PurchaseInvoiceDetails where d.PurchaseInvoiceID == item.InvoiceID select d).FirstOrDefault();
                        var totamount = (from d in Context1.RecPayDetails where d.InvoiceID == salesinvoicedetails.PurchaseInvoiceDetailID select d).ToList();
                        var totsum = totamount.Sum(d => d.Amount) * -1;
                        var totAdsum = totamount.Sum(d => d.AdjustmentAmount);
                        var Debitnote = (from d in Context1.DebitNotes where d.InvoiceID == item.InvoiceID && d.SupplierID == RecP.SupplierID select d).ToList();
                        decimal? DebitAmount = 0;
                        if (Debitnote.Count > 0)
                        {
                            DebitAmount = Debitnote.Sum(d => d.Amount);
                        }
                        var tamount = totsum + totAdsum +DebitAmount;
                        if (tamount >= salesinvoicedetails.NetValue)
                        {
                            salesinvoicedetails.RecPayStatus = 2;
                        }
                        else
                        {
                            salesinvoicedetails.RecPayStatus = 1;
                        }
                        var maxrecpaydetailid = (from c in Context1.RecPayDetails orderby c.RecPayDetailID descending select c.RecPayDetailID).FirstOrDefault();

                        var recpaydet = (from d in Context1.RecPayDetails where d.InvoiceID == item.InvoiceID select d).FirstOrDefault();
                        if (recpaydet != null)
                        {
                            salesinvoicedetails.RecPayDetailId = recpaydet.RecPayDetailID;
                        }
                        else
                        {
                            salesinvoicedetails.RecPayDetailId = maxrecpaydetailid + 1;
                        }
                        Context1.SaveChanges();                      

                    }

                    int editrecPay = 0;
                    var sumOfAmount = Context1.RecPayDetails.Where(m => m.RecPayID == RecP.RecPayID && m.InvoiceID != 0).Sum(c => c.Amount);
                    editrecPay = editfu.EditRecpayDetailsCustR(RecP.RecPayID, Convert.ToInt32(sumOfAmount));
                    int editAcJdetails = editfu.EditAcJDetails(RecP.AcJournalID.Value, Convert.ToInt32(sumOfAmount));


                }
            }

            return RedirectToAction("SupplierTradePaymentDetails", "SupplierPayment", new { ID = RecP.RecPayID });

        }

        public JsonResult ExportToPDF(int recpayid)
        {
            //Report  
            try
            {
                decimal? totalamt = 0;
                int? currencyId = 0;

                ReportViewer reportViewer = new ReportViewer();

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/SupplierPayment.rdlc");

                DataTable dtcompany = new DataTable();
                dtcompany.Columns.Add("CompanyName");
                dtcompany.Columns.Add("Address1");
                dtcompany.Columns.Add("Address2");
                dtcompany.Columns.Add("Address3");
                dtcompany.Columns.Add("Phone");
                dtcompany.Columns.Add("AcHead");
                dtcompany.Columns.Add("Todate");

                var company = Context1.AcCompanies.FirstOrDefault();
                string imagePath = new Uri(Server.MapPath("~/Content/Logo/" + company.logo)).AbsoluteUri;

                DataRow dr = dtcompany.NewRow();
                dr[0] = company.AcCompany1;
                dr[1] = company.Address1;
                dr[2] = company.Address2;
                dr[3] = company.Address3;
                dr[4] = company.Phone;
                dr[5] = imagePath;
                dr[6] = DateTime.Now;

                dtcompany.Rows.Add(dr);

                var receipt = (from d in Context1.RecPays where d.RecPayID == recpayid select d).FirstOrDefault();
                totalamt = receipt.FMoney;
                if (receipt.IsTradingReceipt == true)
                {
                    var recpaydetails = (from d in Context1.RecPayDetails where d.RecPayID == recpayid where d.InvoiceID > 0 select d).ToList();
                    var currency = recpaydetails.Where(d => d.CurrencyID > 0).FirstOrDefault();
                    if (currency != null)
                    {
                        currencyId = currency.CurrencyID;
                    }
                    var cust = Context1.Suppliers.Where(d => d.SupplierID == receipt.SupplierID).FirstOrDefault();
                    var listofdet = new List<ReportCustomerReceipt_Result>();
                    foreach (var item in recpaydetails)
                    {
                        var sinvoicedet = (from d in Context1.PurchaseInvoiceDetails where d.PurchaseInvoiceDetailID == item.InvoiceID select d).FirstOrDefault();
                        var sinvoice = (from d in Context1.PurchaseInvoices where d.PurchaseInvoiceID == sinvoicedet.PurchaseInvoiceID select d).FirstOrDefault();
                        var customerrecpay = new ReportCustomerReceipt_Result();
                        customerrecpay.Date = receipt.RecPayDate.Value.ToString("dd-MMM-yyyy");
                        customerrecpay.ReceivedFrom = cust.SupplierName;
                        customerrecpay.DocumentNo = receipt.DocumentNo;
                        customerrecpay.Amount = Convert.ToDecimal(receipt.FMoney);
                        customerrecpay.Remarks = receipt.Remarks;
                        customerrecpay.Account = receipt.BankName;
                        if (receipt.ChequeDate != null)
                        {
                            customerrecpay.ChequeDate = receipt.ChequeDate.Value.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            customerrecpay.ChequeDate = "";
                        }
                        if (!string.IsNullOrEmpty(receipt.ChequeNo))
                        {
                            customerrecpay.ChequeNo = Convert.ToDecimal(receipt.ChequeNo);
                        }
                        customerrecpay.CustomerBank = "";
                        customerrecpay.DetailDocNo = sinvoice.PurchaseInvoiceNo;
                        customerrecpay.DocDate = sinvoice.PurchaseInvoiceDate.Value.ToString("dd-MMM-yyyy");
                        customerrecpay.DocAmount = Convert.ToDecimal(sinvoicedet.NetValue);

                        if (item.Amount > 0)
                        {
                            customerrecpay.SettledAmount = Convert.ToDecimal(item.Amount);
                            customerrecpay.AdjustmentAmount = Convert.ToInt32(item.AdjustmentAmount);
                        }
                        else
                        {
                            customerrecpay.SettledAmount = Convert.ToDecimal(item.Amount);
                            customerrecpay.AdjustmentAmount = Convert.ToInt32(item.AdjustmentAmount);
                        }
                        listofdet.Add(customerrecpay);
                    }

                    ReportDataSource _rsource;

                    //var dd = entity.ReportCustomerReceipt(recpayid).ToList();
                    _rsource = new ReportDataSource("ReceiptVoucher", listofdet);
                    reportViewer.LocalReport.DataSources.Add(_rsource);

                }
                else
                {
                    var recpaydetails = (from d in Context1.RecPayDetails where d.RecPayID == recpayid where d.InvoiceID > 0 select d).ToList();
                    var currency = recpaydetails.Where(d => d.CurrencyID > 0).FirstOrDefault();
                    if (currency != null)
                    {
                        currencyId = currency.CurrencyID;
                    }
                    var cust = Context1.Suppliers.Where(d => d.SupplierID == receipt.SupplierID).FirstOrDefault();
                    var listofdet = new List<ReportCustomerReceipt_Result>();
                    foreach (var item in recpaydetails)
                    {
                        var sinvoicedet = (from d in Context1.JInvoices where d.InvoiceID == item.InvoiceID select d).FirstOrDefault();
                        var sinvoice = (from d in Context1.JobGenerations where d.JobID == sinvoicedet.JobID select d).FirstOrDefault();
                        var customerrecpay = new ReportCustomerReceipt_Result();
                        customerrecpay.Date = receipt.RecPayDate.Value.ToString("dd-MMM-yyyy");
                        customerrecpay.ReceivedFrom = cust.SupplierName;
                        customerrecpay.DocumentNo = receipt.DocumentNo;
                        customerrecpay.Amount = Convert.ToDecimal(receipt.FMoney);
                        customerrecpay.Remarks = receipt.Remarks;
                        customerrecpay.Account = receipt.BankName;
                        if (receipt.ChequeDate != null)
                        {
                            customerrecpay.ChequeDate = receipt.ChequeDate.Value.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            customerrecpay.ChequeDate = "";
                        }
                        if (!string.IsNullOrEmpty(receipt.ChequeNo))
                        {
                            customerrecpay.ChequeNo = Convert.ToDecimal(receipt.ChequeNo);
                        }
                        customerrecpay.CustomerBank = "";
                        customerrecpay.DetailDocNo = sinvoice.InvoiceNo.ToString();
                        customerrecpay.DocDate = sinvoice.InvoiceDate.Value.ToString("dd-MMM-yyyy");
                        customerrecpay.DocAmount = Convert.ToDecimal(sinvoicedet.SalesHome);

                        if (item.Amount > 0)
                        {
                            customerrecpay.SettledAmount = Convert.ToDecimal(item.Amount);
                            customerrecpay.AdjustmentAmount = Convert.ToInt32(item.AdjustmentAmount);
                        }
                        else
                        {
                            customerrecpay.SettledAmount = Convert.ToDecimal(item.Amount) * -1;
                            customerrecpay.AdjustmentAmount = Convert.ToInt32(item.AdjustmentAmount);
                        }
                        listofdet.Add(customerrecpay);
                    }

                    ReportDataSource _rsource;

                    //var dd = entity.ReportCustomerReceipt(recpayid).ToList();
                    _rsource = new ReportDataSource("ReceiptVoucher", listofdet);
                    reportViewer.LocalReport.DataSources.Add(_rsource);

                }
                ReportDataSource _rsource1 = new ReportDataSource("Company", dtcompany);


                reportViewer.LocalReport.DataSources.Add(_rsource1);



                //foreach (var item in dd)
                //{
                //    totalamt = 5000;
                //}

                //totalamt = 5000;

                //DataTable dtuser = new DataTable();
                //dtuser.Columns.Add("UserName");

                //DataRow dr1 = dtuser.NewRow();
                //int uid = Convert.ToInt32(Session["UserID"].ToString());
                //dr1[0] = (from c in entity.UserRegistrations where c.UserID == uid select c.UserName).FirstOrDefault();
                //dtuser.Rows.Add(dr1);

                //ReportDataSource _rsource2 = new ReportDataSource("User", dtuser);

                //ReportViewer1.LocalReport.DataSources.Add(_rsource2);


                DataTable dtcurrency = new DataTable();
                dtcurrency.Columns.Add("SalesCurrency");
                dtcurrency.Columns.Add("ForeignCurrency");
                dtcurrency.Columns.Add("SalesCurrencySymbol");
                dtcurrency.Columns.Add("ForeignCurrencySymbol");
                dtcurrency.Columns.Add("InWords");

                var currencyName = (from d in Context1.CurrencyMasters where d.CurrencyID == currencyId select d).FirstOrDefault();
                if (currencyName == null)
                {
                    currencyName = new CurrencyMaster();
                }



                DataRow r = dtcurrency.NewRow();
                r[0] = currencyName.CurrencyName;
                r[1] = "";
                r[2] = "";
                r[3] = "";
                r[4] = currencyName.CurrencyName + ",  " + NumberToWords(Convert.ToInt32(totalamt)) + " /00 baisa.";


                dtcurrency.Rows.Add(r);


                ReportDataSource _rsource3 = new ReportDataSource("Currency", dtcurrency);

                reportViewer.LocalReport.DataSources.Add(_rsource3);
                reportViewer.LocalReport.EnableExternalImages = true;
                reportViewer.LocalReport.Refresh();
                //Byte  
                Warning[] warnings;
                string[] streamids;
                string mimeType, encoding, filenameExtension;

                byte[] bytes = reportViewer.LocalReport.Render("Pdf", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                //File  
                string FileName = "Supplier_" + DateTime.Now.Ticks.ToString() + ".pdf";
                string FilePath = Server.MapPath(@"~\TempFile\") + FileName;
                string path = Server.MapPath(@"~\TempFile\");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string[] files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.LastAccessTime < DateTime.Now.AddMinutes(-5))
                        try
                        {
                            fi.Delete();
                        }
                        catch
                        {

                        }
                }
                //create and set PdfReader  
                PdfReader reader = new PdfReader(bytes);
                FileStream output = new FileStream(FilePath, FileMode.Create);

                string Agent = Request.Headers["User-Agent"].ToString();

                //create and set PdfStamper  
                PdfStamper pdfStamper = new PdfStamper(reader, output, '0', true);

                if (Agent.Contains("Firefox"))
                    pdfStamper.JavaScript = "var res = app.loaded('var pp = this.getPrintParams();pp.interactive = pp.constants.interactionLevel.full;this.print(pp);');";
                else
                    pdfStamper.JavaScript = "var res = app.setTimeOut('var pp = this.getPrintParams();pp.interactive = pp.constants.interactionLevel.full;this.print(pp);', 200);";

                pdfStamper.FormFlattening = false;
                pdfStamper.Close();
                reader.Close();

                //return file path  
                string FilePathReturn = @"TempFile/" + FileName;
                return Json(new { success = true, path = FilePathReturn }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message.ToString() }, JsonRequestBehavior.AllowGet);


            }
        }
        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }
            return words;
        }
    }
}

