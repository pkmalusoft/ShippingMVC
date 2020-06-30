﻿using System;
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
    public class CustomerRecieptController : Controller
    {
        MastersModel MM = new MastersModel();
        RecieptPaymentModel RP = new RecieptPaymentModel();
        CustomerRcieptVM cust = new CustomerRcieptVM();
        SHIPPING_FinalEntities Context1 = new SHIPPING_FinalEntities();

        EditCommanFu editfu = new EditCommanFu();
        //
        // GET: /CustomerReciept/
        [HttpGet]
        public ActionResult CustomerReciept(int id)
        {
            CustomerRcieptVM cust = new CustomerRcieptVM();
            cust.CustomerRcieptChildVM = new List<CustomerRcieptChildVM>();
            if (Session["UserID"] != null)
            {

                if (id > 0)
                {
                    cust = RP.GetRecPayByRecpayID(id);
                    ViewBag.achead = Context1.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
                    ViewBag.acheadbank = Context1.AcHeadSelectForBank(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
                    cust.recPayDetail = Context1.RecPayDetails.Where(item => item.RecPayID == id).ToList();
                    //cust.CustomerRcieptChildVM = (from t in Context1.JInvoices
                    //                              join
                    //                                  p in Context1.RecPayDetails on t.InvoiceID equals p.InvoiceID
                    //                              join s in Context1.RecPays on p.RecPayID equals s.RecPayID

                    //                              where (s.RecPayID == id && p.InvoiceID != 0)
                    //                              select new CustomerRcieptChildVM
                    //                              {
                    //                                  InvoiceDate = s.RecPayDate,
                    //                                  InvoiceID = t.InvoiceID,
                    //                                  AmountToBeRecieved = t.SalesHome.Value,
                    //                                  Amount = -(p.Amount.Value),

                    //                                  Balance = t.SalesHome.Value,
                    //                                  RecPayDetailID = p.RecPayDetailID,
                    //                                  CurrencyId = p.CurrencyID.Value



                    //                              }).OrderBy(x => x.InvoiceDate).ToList();
                    cust.CustomerRcieptChildVM = DAL.GetCustomerReciept(id);
                    BindMasters_ForEdit(cust);
                }
                else
                {
                    BindAllMasters();
                    //ViewBag.achead = Context1.AcHeads.ToList().Where(x => x.AcGroupID == 10);
                    //ViewBag.acheadbank = Context1.AcHeads.ToList().Where(x => x.AcGroupID == 49);

                    ViewBag.achead = Context1.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
                    ViewBag.acheadbank = Context1.AcHeadSelectForBank(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();

                    cust.RecPayDate = System.DateTime.UtcNow;
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            return View(cust);

        }

        //[HttpPost]
        //public ActionResult CustomerReciept(CustomerRcieptVM RecP, string Command, string Currency)
        //{
        //    List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
        //    List<SP_GetJInvoiceDetailsByInvoiceID_Result> InvDetails = new List<SP_GetJInvoiceDetailsByInvoiceID_Result>();

        //    int RPID = 0;
        //    int i = 0;
        //    RecP.FYearID = Convert.ToInt32(Session["fyearid"]);
        //    RecP.UserID = Convert.ToInt32(Session["UserID"]);

        //    if (RecP.RecPayID > 0)
        //    {
        //        RP.EditCustomerRecPay(RecP, Session["UserID"].ToString());
        //        RP.EditCustomerRecieptDetails(RecP.recPayDetail, RecP.RecPayID);
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

        //                //Advance Amount getting for customer
        //                decimal AdvanceAmount = RP.GetAdvanceAmount(Convert.ToInt32(RecP.CustomerID));

        //                //All Pending Invoices getting
        //                AllInvoices = RP.GetCustomerInvoiceDetails(Convert.ToInt32(RecP.CustomerID));

        //                decimal InvoiceAmt = 0;

        //                foreach (var item in AllInvoices)
        //                {
        //                    DateTime dateTime = Convert.ToDateTime(item.InvoiceDate);
        //                    string newd = Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd h:mm tt");
        //                    InvDetails = RP.InvDtls(item.InvoiceID);

        //                    foreach (var item1 in InvDetails)
        //                    {
        //                        InvoiceAmt = Convert.ToDecimal(item1.SalesHome);
        //                    }

        //                    if (item.AmtPaidTillDate == 0)
        //                    {
        //                        if (AdvanceAmtToinsert >= item.AmountToBeRecieved)
        //                        {

        //                            int k = RP.InsertRecpayDetailsForCust(RPID, item.InvoiceID, Convert.ToDecimal(-item.AmountToBeRecieved), "", "C", false, "", newd, item.InvoiceNo.ToString(), Convert.ToInt32(RecP.CurrencyId), 3);

        //                            AdvanceAmtToinsert = AdvanceAmtToinsert - Convert.ToDecimal(item.AmountToBeRecieved);

        //                        }
        //                        else if (item.AmountToBeRecieved > AdvanceAmtToinsert)
        //                        {
        //                            int k = RP.InsertRecpayDetailsForCust(RPID, item.InvoiceID, Convert.ToDecimal(-AdvanceAmtToinsert), "", "C", false, "", newd, item.InvoiceNo.ToString(), Convert.ToInt32(RecP.CurrencyId), 2);

        //                            AdvanceAmtToinsert = AdvanceAmtToinsert - Convert.ToDecimal(AdvanceAmtToinsert);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (AdvanceAmtToinsert >= item.Balance)
        //                        {

        //                            int k = RP.InsertRecpayDetailsForCust(RPID, item.InvoiceID, Convert.ToDecimal(-item.Balance), "", "C", false, "", newd, item.InvoiceNo.ToString(), Convert.ToInt32(RecP.CurrencyId), 3);

        //                            AdvanceAmtToinsert = AdvanceAmtToinsert - Convert.ToDecimal(item.Balance);

        //                        }
        //                        else if (item.Balance > AdvanceAmtToinsert)
        //                        {
        //                            int k = RP.InsertRecpayDetailsForCust(RPID, item.InvoiceID, Convert.ToDecimal(-AdvanceAmtToinsert), "", "C", false, "", newd, item.InvoiceNo.ToString(), Convert.ToInt32(RecP.CurrencyId), 2);

        //                            AdvanceAmtToinsert = AdvanceAmtToinsert - Convert.ToDecimal(AdvanceAmtToinsert);
        //                        }
        //                    }

        //                }


        //                int l = RP.InsertRecpayDetailsForCust(RPID, 0, TotalAmount, null, "C", false, null, null, null, Convert.ToInt32(RecP.CurrencyId), 4);

        //                //Advance Amount entry
        //                if (AdvanceAmtToinsert > 0)
        //                {
        //                    int k = RP.InsertRecpayDetailsForCust(RPID, 0, AdvanceAmtToinsert, null, "C", true, null, null, null, Convert.ToInt32(RecP.CurrencyId), 4);
        //                }

        //                int fyaerId = Convert.ToInt32(Session["fyearid"].ToString());

        //                RP.InsertJournalOfCustomer(RPID, fyaerId);
        //            }
        //        }
        //        else
        //        {
        //            return RedirectToAction("Login", "Login");
        //        }
        //    }

        //    BindAllMasters();


        //    return RedirectToAction("CustomerRecieptDetails", "CustomerReciept", new { ID = RPID });


        //}

        [HttpPost]
        public ActionResult CustomerReciept(CustomerRcieptVM RecP, string Command, string Currency)
        {
            int RPID = 0;
            int i = 0;
            RecP.FYearID = Convert.ToInt32(Session["fyearid"]);
            RecP.UserID = Convert.ToInt32(Session["UserID"]);

            //if (RecP.RecPayID > 0)
            //{
            //    RP.EditCustomerRecPay(RecP, Session["UserID"].ToString());
            //    RP.EditCustomerRecieptDetails(RecP.recPayDetail, RecP.RecPayID);
            //}
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

            ///Insert Entry For RecPay Details 
            ///
            if (RecP.RecPayID <= 0)
            {
                decimal Fmoney = 0;
                for (int j = 0; j < RecP.CustomerRcieptChildVM.Count; j++)
                {
                    Fmoney = Fmoney + RecP.CustomerRcieptChildVM[j].Amount;
                }

                RecP.FMoney = Fmoney;
                RPID = RP.AddCustomerRecieptPayment(RecP, Session["UserID"].ToString());

                RecP.RecPayID = (from c in Context1.RecPays orderby c.RecPayID descending select c.RecPayID).FirstOrDefault();
                decimal TotalAmount = 0;
                foreach (var item in RecP.CustomerRcieptChildVM)
                {
                    decimal Advance = 0;
                    //if (item.Amount > 0 && (item.AmountToBeRecieved < item.Amount || item.AmountToBeRecieved == item.Amount))///900<1000
                    //{
                    // item.Amount = Convert.ToDecimal(RecP.EXRate * item.Amount);
                    //100=1000-900
                    Advance = item.Amount - item.AmountToBeRecieved;
                    DateTime vInvoiceDate = Convert.ToDateTime(item.InvoiceDate);
                    string vInvoiceDate1 = Convert.ToDateTime(vInvoiceDate).ToString("yyyy-MM-dd h:mm tt");
                    RP.InsertRecpayDetailsForCust(RecP.RecPayID, item.InvoiceID, item.InvoiceID, Convert.ToDecimal(-item.Amount), "", "C", false, "", vInvoiceDate1, item.InvoiceNo.ToString(), Convert.ToInt32(RecP.CurrencyId), 3, item.JobID);

                    if (Advance > 0)
                    {
                        //Advance Amount entry
                        RP.InsertRecpayDetailsForCust(RecP.RecPayID, 0, 0, Advance, null, "C", true, null, null, null, Convert.ToInt32(RecP.CurrencyId), 4, item.JobID);
                    }
                    TotalAmount = TotalAmount + item.Amount;
                }

                //To Balance Invoice AMount
                if (TotalAmount > 0)
                {
                    int l = RP.InsertRecpayDetailsForCust(RecP.RecPayID, 0, 0, TotalAmount, null, "C", false, null, null, null, Convert.ToInt32(RecP.CurrencyId), 4, 0);
                    int fyaerId = Convert.ToInt32(Session["fyearid"].ToString());
                    RP.InsertJournalOfCustomer(RecP.RecPayID, fyaerId);
                }
                var Recpaydata = (from d in Context1.RecPays where d.RecPayID == RecP.RecPayID select d).FirstOrDefault();

                Recpaydata.RecPayID = RecP.RecPayID;
                Recpaydata.IsTradingReceipt = false;
                Context1.Entry(Recpaydata).State = EntityState.Modified;
                Context1.SaveChanges();
            }
            else
            {
                decimal Fmoney = 0;
                for (int j = 0; j < RecP.CustomerRcieptChildVM.Count; j++)
                {
                    Fmoney = Fmoney + RecP.CustomerRcieptChildVM[j].Amount;
                }

                RecPay recpay = new RecPay();
                recpay.RecPayDate = RecP.RecPayDate;
                recpay.RecPayID = RecP.RecPayID;
                recpay.AcJournalID = RecP.AcJournalID;
                recpay.BankName = RecP.BankName;
                recpay.ChequeDate = RecP.ChequeDate;
                recpay.ChequeNo = RecP.ChequeNo;
                recpay.CustomerID = RecP.CustomerID;
                recpay.DocumentNo = RecP.DocumentNo;
                recpay.EXRate = RecP.EXRate;
                recpay.FYearID = RecP.FYearID;
                recpay.FMoney = Fmoney;
                recpay.StatusEntry = RecP.StatusEntry;
                recpay.IsTradingReceipt = true;

                //recpay.SupplierID = RecP.SupplierID;
                Context1.Entry(recpay).State = EntityState.Modified;
                Context1.SaveChanges();

                foreach (var item in RecP.CustomerRcieptChildVM)
                {
                    RecPayDetail recpd = new RecPayDetail();
                    recpd.RecPayDetailID = item.RecPayDetailID;
                    recpd.Amount = -(item.Amount);
                    recpd.CurrencyID = item.CurrencyId;
                    //recpd.InvDate = item.InvoiceDate.Value;
                    recpd.RecPayID = RecP.RecPayID;
                    recpd.Remarks = item.Remarks;
                    recpd.InvoiceID = item.InvoiceID;
                    recpd.StatusInvoice = "C";
                    /*  if (item.AmountToBeRecieved < item.Amount)
                      {
                          RecPayDetail recpd1 = new RecPayDetail();
                          recpd1.RecPayDetailID = (from c in Context1.RecPayDetails orderby c.RecPayDetailID descending select c.RecPayDetailID).FirstOrDefault();
                          recpd1.Amount = item.AmountToBeRecieved - item.Amount;
                          recpd1.RecPayID = RecP.RecPayID;
                          recpd1.Remarks = item.Remarks;
                          recpd1.CurrencyID = item.CurrencyId;
                          recpd1.StatusAdvance = true;
                          recpd.StatusInvoice = "C";
                          Context1.Entry(recpd1).State = EntityState.Modified;
                          Context1.SaveChanges();
                      }*/

                    Context1.Entry(recpd).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                int editrecPay = 0;
                var sumOfAmount = Context1.RecPayDetails.Where(m => m.RecPayID == RecP.RecPayID && m.InvoiceID != 0).Sum(c => c.Amount);
                editrecPay = editfu.EditRecpayDetailsCustR(RecP.RecPayID, Convert.ToInt32(sumOfAmount));
                int editAcJdetails = editfu.EditAcJDetails(RecP.AcJournalID.Value, Convert.ToInt32(sumOfAmount));
            }

            BindAllMasters();
            return RedirectToAction("CustomerRecieptDetails", "CustomerReciept", new { ID = RecP.RecPayID });
        }


        [HttpGet]
        public ActionResult CustomerRecieptDetails(int ID)
        {
            List<SP_GetAllRecieptsDetails_Result> Reciepts = new List<SP_GetAllRecieptsDetails_Result>();

            Reciepts = RP.GetAllReciepts();
            //var data = (from t in Reciepts where (t.RecPayDate >= Convert.ToDateTime(Session["FyearFrom"]) && t.RecPayDate <= Convert.ToDateTime(Session["FyearTo"])) select t).ToList();

            if (ID > 0)
            {
                ViewBag.SuccessMsg = "You have successfully added Customer Reciept.";
            }


            if (ID == 10)
            {
                ViewBag.SuccessMsg = "You have successfully deleted Customer Reciept.";
            }

            if (ID == 20)
            {
                ViewBag.SuccessMsg = "You have successfully updated Customer Reciept.";
            }


            Session["ID"] = ID;


            return View(Reciepts);
        }

        public JsonResult GetInvoiceOfCustomer(string ID)
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();

            DateTime fromdate = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime todate = Convert.ToDateTime(Session["FyearTo"].ToString());
            var AllInvoices = RP.GetCustomerInvoiceDetails(Convert.ToInt32(ID), fromdate, todate).OrderBy(x => x.InvoiceDate).ToList();

            return Json(AllInvoices, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExchangeRateByCurID(string ID)
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();

            var ER = RP.GetExchgeRateByCurID(Convert.ToInt32(ID));

            return Json(ER, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        public ActionResult DeleteCustomerDet(int id)
        {
            //int k = 0;
            if (id != 0)
            {
                RP.DeleteCustomerDetails(id);
            }

            return RedirectToAction("CustomerRecieptDetails", "CustomerReciept", new { ID = 10 });

        }
        public ActionResult DeleteCustomerDetTrade(int id)
        {
            //int k = 0;
            if (id != 0)
            {
                RP.DeleteCustomerDetails(id);
            }

            return RedirectToAction("CustomerTradeReceiptDetails", "CustomerReciept", new { ID = 10 });

        }

        public string getSuccessID()
        {
            string ID = "";

            if (Session["ID"] != null)
            {
                ID = Session["ID"].ToString();
            }

            return ID;
        }

        //public JsonResult GetInvoiceOfCustomer(string ID)
        //{
        //    List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();

        //    AllInvoices = RP.GetCustomerInvoiceDetails(199);

        //    return this.Json(AllInvoices.ToList());
        //}

        public void BindAllMasters()
        {
            List<SP_GetAllCustomers_Result> Customers = new List<SP_GetAllCustomers_Result>();
            Customers = MM.GetAllCustomer();

            List<SP_GetCurrency_Result> Currencys = new List<SP_GetCurrency_Result>();
            Currencys = MM.GetCurrency();

            string DocNo = RP.GetMaxRecieptDocumentNo();

            ViewBag.DocumentNos = DocNo;

            ViewBag.Customer = new SelectList(Customers, "CustomerID", "Customer");

            ViewBag.Currency = new SelectList(Currencys, "CurrencyID", "CurrencyName");
        }

        public void BindMasters_ForEdit(CustomerRcieptVM cust)
        {
            List<SP_GetAllCustomers_Result> Customers = new List<SP_GetAllCustomers_Result>();
            Customers = MM.GetAllCustomer();

            List<SP_GetCurrency_Result> Currencys = new List<SP_GetCurrency_Result>();
            Currencys = MM.GetCurrency();


            ViewBag.DocumentNos = cust.DocumentNo;

            ViewBag.Customer = new SelectList(Customers, "CustomerID", "Customer", cust.CustomerID);

            ViewBag.Currency = new SelectList(Currencys, "CurrencyID", "CurrencyName", cust.CurrencyId);

        }

        public JsonResult GetAllCurrencyCustReciept()
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            // var AllInvoices;


            var CostReciept = (from t in Context1.SPGetAllLocalCurrencyCustRecievable(Convert.ToInt32(Session["fyearid"].ToString()))
                               select t).ToList();
            return Json(CostReciept, JsonRequestBehavior.AllowGet);



        }

        public JsonResult GetAllCustomer()
        {
            DateTime d = DateTime.Now;
            DateTime fyear = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime mstart = new DateTime(fyear.Year, d.Month, 01);

            int maxday = DateTime.DaysInMonth(fyear.Year, d.Month);
            DateTime mend = new DateTime(fyear.Year, d.Month, maxday);

            var cust = Context1.SP_GetAllRecieptsDetails().Where(x => x.RecPayDate >= mstart && x.RecPayDate <= mend).OrderByDescending(x => x.RecPayDate).ToList();

            string view = this.RenderPartialView("_GetAllCustomer", cust);

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




        public JsonResult GetAllCustomerByDate(string fdate, string tdate, int FYearID)
        {
            DateTime d = DateTime.Now;
            DateTime fyear = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime mstart = new DateTime(fyear.Year, d.Month, 01);

            int maxday = DateTime.DaysInMonth(fyear.Year, d.Month);
            DateTime mend = new DateTime(fyear.Year, d.Month, maxday);

            var sdate = DateTime.Parse(fdate);
            var edate = DateTime.Parse(tdate);

            ViewBag.AllCustomers = Context1.CUSTOMERs.ToList();

            var data = Context1.RecPays.Where(x => x.RecPayDate >= sdate && x.RecPayDate <= edate && x.IsTradingReceipt != true && x.FYearID == FYearID).OrderByDescending(x => x.RecPayDate).ToList();

            var cust = Context1.SP_GetAllRecieptsDetailsByDate(fdate, tdate, FYearID).ToList();

            string view = this.RenderPartialView("_GetAllCustomerByDate", data);

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

        public ActionResult GetReceiptsByDate(string fdate, string tdate, int FYearID)
        {
            DateTime d = DateTime.Now;
            DateTime fyear = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime mstart = new DateTime(fyear.Year, d.Month, 01);

            int maxday = DateTime.DaysInMonth(fyear.Year, d.Month);
            DateTime mend = new DateTime(fyear.Year, d.Month, maxday);
            var sdate = DateTime.Parse(fdate);
            var edate = DateTime.Parse(tdate);

            ViewBag.AllCustomers = Context1.CUSTOMERs.ToList();

            var data = Context1.RecPays.Where(x => x.RecPayDate >= sdate && x.RecPayDate <= edate && x.IsTradingReceipt != true && x.FYearID == FYearID).OrderByDescending(x => x.RecPayDate).ToList();


            var cust = Context1.SP_GetAllRecieptsDetailsByDate(fdate, tdate, FYearID).ToList();

            return PartialView("_GetAllCustomerByDate", data);

        }

        public JsonResult GetAllTradeCustomerByDate(string fdate, string tdate, int FYearID)
        {
            DateTime d = DateTime.Now;
            DateTime fyear = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime mstart = new DateTime(fyear.Year, d.Month, 01);

            int maxday = DateTime.DaysInMonth(fyear.Year, d.Month);
            DateTime mend = new DateTime(fyear.Year, d.Month, maxday);

            var sdate = DateTime.Parse(fdate);
            var edate = DateTime.Parse(tdate);


            var data = Context1.RecPays.Where(x => x.RecPayDate >= sdate && x.RecPayDate <= edate && x.IsTradingReceipt == true && x.FYearID == FYearID).OrderByDescending(x => x.RecPayDate).ToList();
            var cust = Context1.SP_GetAllRecieptsDetailsByDate(fdate, tdate, FYearID).ToList();
            ViewBag.AllCustomers = Context1.CUSTOMERs.ToList();
            string view = this.RenderPartialView("_GetAllTradeCustomerByDate", data);

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
        public ActionResult GetTradeReceiptsByDate(string fdate, string tdate, int FYearID)
        {
            DateTime d = DateTime.Now;
            DateTime fyear = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime mstart = new DateTime(fyear.Year, d.Month, 01);

            int maxday = DateTime.DaysInMonth(fyear.Year, d.Month);
            DateTime mend = new DateTime(fyear.Year, d.Month, maxday);
            var sdate = DateTime.Parse(fdate);
            var edate = DateTime.Parse(tdate);
            ViewBag.AllCustomers = Context1.CUSTOMERs.ToList();

            var data = Context1.RecPays.Where(x => x.RecPayDate >= sdate && x.RecPayDate <= edate && x.FYearID == FYearID && x.IsTradingReceipt == true).OrderByDescending(x => x.RecPayDate).ToList();


            var cust = Context1.SP_GetAllRecieptsDetailsByDate(fdate, tdate, FYearID).ToList();

            return PartialView("_GetAllTradeCustomerByDate", data);

        }
        [HttpGet]
        public ActionResult CustomerTradeReceiptDetails(int ID)
        {
            List<SP_GetAllRecieptsDetails_Result> Reciepts = new List<SP_GetAllRecieptsDetails_Result>();

            Reciepts = RP.GetAllReciepts();
            //var data = (from t in Reciepts where (t.RecPayDate >= Convert.ToDateTime(Session["FyearFrom"]) && t.RecPayDate <= Convert.ToDateTime(Session["FyearTo"])) select t).ToList();

            if (ID > 0)
            {
                ViewBag.SuccessMsg = "You have successfully added Customer Reciept.";
            }


            if (ID == 10)
            {
                ViewBag.SuccessMsg = "You have successfully deleted Customer Reciept.";
            }

            if (ID == 20)
            {
                ViewBag.SuccessMsg = "You have successfully updated Customer Reciept.";
            }


            Session["ID"] = ID;


            return View(Reciepts);
        }
        [HttpGet]
        public ActionResult CustomerTradeReceipt(int id)
        {
            CustomerRcieptVM cust = new CustomerRcieptVM();
            cust.CustomerRcieptChildVM = new List<CustomerRcieptChildVM>();
            if (Session["UserID"] != null)
            {

                if (id > 0)
                {
                    cust = RP.GetRecPayByRecpayID(id);
                    ViewBag.achead = Context1.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
                    ViewBag.acheadbank = Context1.AcHeadSelectForBank(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
                    cust.recPayDetail = Context1.RecPayDetails.Where(item => item.RecPayID == id).ToList();
                    cust.CustomerRcieptChildVM = new List<CustomerRcieptChildVM>();
                    foreach (var item in cust.recPayDetail)
                    {
                        if (item.InvoiceID > 0)
                        {
                            var sInvoiceDetail = (from d in Context1.SalesInvoiceDetails where d.SalesInvoiceDetailID == item.InvoiceID select d).FirstOrDefault();
                            var Sinvoice = (from d in Context1.SalesInvoices where d.SalesInvoiceID == sInvoiceDetail.SalesInvoiceID select d).FirstOrDefault();
                            var allrecpay = (from d in Context1.RecPayDetails where d.InvoiceID == item.InvoiceID select d).ToList();
                            var totamtpaid = allrecpay.Sum(d => d.Amount) * -1;
                            var totadjust = allrecpay.Sum(d => d.AdjustmentAmount);
                            var totamt = totamtpaid + totadjust;
                            var customerinvoice = new CustomerRcieptChildVM();
                            customerinvoice.InvoiceID = Convert.ToInt32(item.InvoiceID);
                            customerinvoice.SInvoiceNo = Sinvoice.SalesInvoiceNo;
                            customerinvoice.strDate = Convert.ToDateTime(item.InvDate).ToString("dd/MM/yyyy");
                            customerinvoice.AmountToBePaid = Convert.ToDecimal(totamtpaid);
                            customerinvoice.Amount = Convert.ToDecimal(item.Amount) * -1;
                            customerinvoice.Balance = Convert.ToDecimal(sInvoiceDetail.NetValue - totamt);
                            customerinvoice.RecPayDetailID = item.RecPayDetailID;
                            customerinvoice.AmountToBeRecieved = Convert.ToDecimal(sInvoiceDetail.NetValue);
                            customerinvoice.RecPayID = Convert.ToInt32(item.RecPayID);
                            customerinvoice.AdjustmentAmount = Convert.ToDecimal(item.AdjustmentAmount);
                            cust.CustomerRcieptChildVM.Add(customerinvoice);
                        }
                    }
                    //var details=(from d in Context1.SalesInvoices join s in Context1.RecPayDetails on d.SalesInvoiceID )
                    BindMasters_ForEdit(cust);
                }
                else
                {
                    BindAllMasters();

                    ViewBag.achead = Context1.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
                    ViewBag.acheadbank = Context1.AcHeadSelectForBank(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();

                    cust.RecPayDate = System.DateTime.UtcNow;
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            return View(cust);

        }
        public JsonResult GetTradeInvoiceOfCustomer(int? ID)
        {

            DateTime fromdate = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime todate = Convert.ToDateTime(Session["FyearTo"].ToString());
            var AllInvoices = (from d in Context1.SalesInvoices where d.CustomerID == ID select d).ToList();
            var salesinvoice = new List<CustomerTradeReceiptVM>();
            foreach (var item in AllInvoices)
            {
                var invoicedeails = (from d in Context1.SalesInvoiceDetails where d.SalesInvoiceID == item.SalesInvoiceID where (d.RecPayStatus < 2 || d.RecPayStatus == null) select d).ToList();
                foreach (var det in invoicedeails)
                {
                    var allrecpay = (from d in Context1.RecPayDetails where d.InvoiceID == det.SalesInvoiceDetailID select d).ToList();
                    var totamtpaid = allrecpay.Sum(d => d.Amount) * -1;
                    var totadjust = allrecpay.Sum(d => d.AdjustmentAmount);
                    var totamt = totamtpaid + totadjust;
                    var Invoice = new CustomerTradeReceiptVM();
                    Invoice.JobID = det.JobID;
                    Invoice.JobCode = "";
                    Invoice.SalesInvoiceID = det.SalesInvoiceID;
                    Invoice.InvoiceNo = item.SalesInvoiceNo;
                    Invoice.SalesInvoiceDetailID = det.SalesInvoiceDetailID;
                    Invoice.InvoiceAmount = det.NetValue;
                    Invoice.date = item.SalesInvoiceDate;
                    Invoice.DateTime = item.SalesInvoiceDate.Value.ToString("dd/MM/yyyy");
                    var RecPay = (from d in Context1.RecPayDetails where d.RecPayDetailID == det.RecPayDetailId select d).FirstOrDefault();

                    Invoice.AmountReceived = totamt;
                    Invoice.Balance = Invoice.InvoiceAmount - totamtpaid;
                    Invoice.AdjustmentAmount = totadjust;


                    salesinvoice.Add(Invoice);
                }
            }

            return Json(salesinvoice, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CustomerTradeReceipt(CustomerRcieptVM RecP, string Command, string Currency)
        {
            int RPID = 0;
            int i = 0;
            RecP.FYearID = Convert.ToInt32(Session["fyearid"]);
            RecP.UserID = Convert.ToInt32(Session["UserID"]);

            //if (RecP.RecPayID > 0)
            //{
            //    RP.EditCustomerRecPay(RecP, Session["UserID"].ToString());
            //    RP.EditCustomerRecieptDetails(RecP.recPayDetail, RecP.RecPayID);
            //}
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

            ///Insert Entry For RecPay Details 
            ///
            if (RecP.RecPayID <= 0)
            {
                decimal Fmoney = 0;
                for (int j = 0; j < RecP.CustomerRcieptChildVM.Count; j++)
                {
                    Fmoney = Fmoney + RecP.CustomerRcieptChildVM[j].Amount;
                }

                RecP.FMoney = Fmoney;

                RPID = RP.AddCustomerRecieptPayment(RecP, Session["UserID"].ToString());

                RecP.RecPayID = (from c in Context1.RecPays orderby c.RecPayID descending select c.RecPayID).FirstOrDefault();
                decimal TotalAmount = 0;

                foreach (var item in RecP.CustomerRcieptChildVM)
                {
                    decimal Advance = 0;
                    //if (item.Amount > 0 && (item.AmountToBeRecieved < item.Amount || item.AmountToBeRecieved == item.Amount))///900<1000
                    //{
                    // item.Amount = Convert.ToDecimal(RecP.EXRate * item.Amount);
                    //100=1000-900
                    Advance = item.Amount - item.AmountToBeRecieved;
                    DateTime vInvoiceDate = Convert.ToDateTime(item.InvoiceDate);
                    string vInvoiceDate1 = Convert.ToDateTime(vInvoiceDate).ToString("yyyy-MM-dd h:mm tt");
                    if (item.Amount > 0 || item.AdjustmentAmount > 0)
                    {
                        var maxrecpaydetailid = (from c in Context1.RecPayDetails orderby c.RecPayDetailID descending select c.RecPayDetailID).FirstOrDefault();

                        RP.InsertRecpayDetailsForCust(RecP.RecPayID, item.InvoiceID, item.InvoiceID, Convert.ToDecimal(-item.Amount), "", "C", false, "", vInvoiceDate1, item.InvoiceNo.ToString(), Convert.ToInt32(RecP.CurrencyId), 3, item.JobID);
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
                        var salesinvoicedetails = (from d in Context1.SalesInvoiceDetails where d.SalesInvoiceDetailID == item.InvoiceID select d).FirstOrDefault();
                        var totamount = (from d in Context1.RecPayDetails where d.InvoiceID == salesinvoicedetails.SalesInvoiceDetailID select d).ToList();
                        var totsum = totamount.Sum(d => d.Amount);
                        var totAdsum = totamount.Sum(d => d.AdjustmentAmount);
                        var tamount = totsum + totAdsum;
                        if (tamount >= salesinvoicedetails.NetValue)
                        {
                            salesinvoicedetails.RecPayStatus = 2;
                        }
                        else
                        {
                            salesinvoicedetails.RecPayStatus = 1;
                        }
                        salesinvoicedetails.RecPayDetailId = maxrecpaydetailid + 1;
                        Context1.SaveChanges();
                    }
                    TotalAmount = TotalAmount + item.Amount;
                }

                //To Balance Invoice AMount
                if (TotalAmount > 0)
                {
                    int l = RP.InsertRecpayDetailsForCust(RecP.RecPayID, 0, 0, TotalAmount, null, "C", false, null, null, null, Convert.ToInt32(RecP.CurrencyId), 4, 0);
                    int fyaerId = Convert.ToInt32(Session["fyearid"].ToString());
                    RP.InsertJournalOfCustomer(RecP.RecPayID, fyaerId);

                }
                var Recpaydata = (from d in Context1.RecPays where d.RecPayID == RecP.RecPayID select d).FirstOrDefault();

                Recpaydata.RecPayID = RecP.RecPayID;
                Recpaydata.IsTradingReceipt = true;
                Context1.Entry(Recpaydata).State = EntityState.Modified;
                Context1.SaveChanges();

            }
            else
            {
                decimal Fmoney = 0;
                for (int j = 0; j < RecP.CustomerRcieptChildVM.Count; j++)
                {
                    Fmoney = Fmoney + RecP.CustomerRcieptChildVM[j].Amount;
                }

                RecPay recpay = new RecPay();
                recpay.RecPayDate = RecP.RecPayDate;
                recpay.RecPayID = RecP.RecPayID;
                recpay.AcJournalID = RecP.AcJournalID;
                recpay.BankName = RecP.BankName;
                recpay.ChequeDate = RecP.ChequeDate;
                recpay.ChequeNo = RecP.ChequeNo;
                recpay.CustomerID = RecP.CustomerID;
                recpay.DocumentNo = RecP.DocumentNo;
                recpay.EXRate = RecP.EXRate;
                recpay.FYearID = RecP.FYearID;
                recpay.FMoney = Fmoney;
                recpay.StatusEntry = RecP.StatusEntry;
                recpay.IsTradingReceipt = true;
                Context1.Entry(recpay).State = EntityState.Modified;
                Context1.SaveChanges();

                foreach (var item in RecP.CustomerRcieptChildVM)
                {
                    RecPayDetail recpd = new RecPayDetail();
                    recpd.RecPayDetailID = item.RecPayDetailID;
                    recpd.Amount = -(item.Amount);
                    recpd.CurrencyID = item.CurrencyId;
                    //recpd.InvDate = item.InvoiceDate.Value;
                    recpd.RecPayID = RecP.RecPayID;
                    recpd.Remarks = item.Remarks;
                    recpd.InvoiceID = item.InvoiceID;
                    recpd.StatusInvoice = "C";
                    Context1.Entry(recpd).State = EntityState.Modified;
                    Context1.SaveChanges();
                    var salesinvoicedetails = (from d in Context1.SalesInvoiceDetails where d.SalesInvoiceDetailID == item.InvoiceID select d).FirstOrDefault();
                    var totamount = (from d in Context1.RecPayDetails where d.InvoiceID == salesinvoicedetails.SalesInvoiceDetailID select d).ToList();
                    var totsum = totamount.Sum(d => d.Amount) *-1;
                    var totAdsum = totamount.Sum(d => d.AdjustmentAmount);
                    var tamount = totsum + totAdsum;
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
                    /*  if (item.AmountToBeRecieved < item.Amount)
                      {
                          RecPayDetail recpd1 = new RecPayDetail();
                          recpd1.RecPayDetailID = (from c in Context1.RecPayDetails orderby c.RecPayDetailID descending select c.RecPayDetailID).FirstOrDefault();
                          recpd1.Amount = item.AmountToBeRecieved - item.Amount;
                          recpd1.RecPayID = RecP.RecPayID;
                          recpd1.Remarks = item.Remarks;
                          recpd1.CurrencyID = item.CurrencyId;
                          recpd1.StatusAdvance = true;
                          recpd.StatusInvoice = "C";
                          Context1.Entry(recpd1).State = EntityState.Modified;
                          Context1.SaveChanges();
                      }*/


                }
                int editrecPay = 0;
                var sumOfAmount = Context1.RecPayDetails.Where(m => m.RecPayID == RecP.RecPayID && m.InvoiceID != 0).Sum(c => c.Amount);
                editrecPay = editfu.EditRecpayDetailsCustR(RecP.RecPayID, Convert.ToInt32(sumOfAmount));
                int editAcJdetails = editfu.EditAcJDetails(RecP.AcJournalID.Value, Convert.ToInt32(sumOfAmount));
            }


            BindAllMasters();
            return RedirectToAction("CustomerTradeReceiptDetails", "CustomerReciept", new { ID = RecP.RecPayID });
        }
        public JsonResult ExportToPDF(int recpayid)
        {
            //Report  
            try
            {
                ReportViewer reportViewer = new ReportViewer();

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/ReceiptVoucher.rdlc");

                DataTable dtcompany = new DataTable();
                dtcompany.Columns.Add("CompanyName");
                dtcompany.Columns.Add("Address1");
                dtcompany.Columns.Add("Address2");
                dtcompany.Columns.Add("Address3");
                dtcompany.Columns.Add("Phone");
                dtcompany.Columns.Add("AcHead");
                dtcompany.Columns.Add("Todate");

                var company = Context1.AcCompanies.FirstOrDefault();

                DataRow dr = dtcompany.NewRow();
                dr[0] = company.AcCompany1;
                dr[1] = company.Address1;
                dr[2] = company.Address2;
                dr[3] = company.Address3;
                dr[4] = company.Phone;
                dr[5] = "";
                dr[6] = DateTime.Now;

                dtcompany.Rows.Add(dr);

                var receipt = (from d in Context1.RecPays where d.RecPayID == recpayid select d).FirstOrDefault();

                if (receipt.IsTradingReceipt == true)
                {
                    var recpaydetails = (from d in Context1.RecPayDetails where d.RecPayID == recpayid where d.InvoiceID > 0 select d).ToList();
                    var cust = Context1.CUSTOMERs.Where(d => d.CustomerID == receipt.CustomerID).FirstOrDefault();
                    var listofdet = new List<ReportCustomerReceipt_Result>();
                    foreach (var item in recpaydetails)
                    {
                        var sinvoicedet = (from d in Context1.SalesInvoiceDetails where d.SalesInvoiceDetailID == item.InvoiceID select d).FirstOrDefault();
                        var sinvoice = (from d in Context1.SalesInvoices where d.SalesInvoiceID == sinvoicedet.SalesInvoiceID select d).FirstOrDefault();
                        var customerrecpay = new ReportCustomerReceipt_Result();
                        customerrecpay.Date = receipt.RecPayDate.Value.ToString("dd-MMM-yyyy");
                        customerrecpay.ReceivedFrom = cust.Customer1;
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
                        customerrecpay.DetailDocNo = sinvoice.SalesInvoiceNo;
                        customerrecpay.DocDate = sinvoice.SalesInvoiceDate.Value.ToString("dd-MMM-yyyy");
                        customerrecpay.DocAmount = Convert.ToDecimal(sinvoicedet.NetValue);

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
                else
                {
                    var recpaydetails = (from d in Context1.RecPayDetails where d.RecPayID == recpayid where d.InvoiceID > 0 select d).ToList();
                    var cust = Context1.CUSTOMERs.Where(d => d.CustomerID == receipt.CustomerID).FirstOrDefault();
                    var listofdet = new List<ReportCustomerReceipt_Result>();
                    foreach (var item in recpaydetails)
                    {
                        var sinvoicedet = (from d in Context1.JInvoices where d.InvoiceID == item.InvoiceID select d).FirstOrDefault();
                        var sinvoice = (from d in Context1.JobGenerations where d.JobID == sinvoicedet.JobID select d).FirstOrDefault();
                        var customerrecpay = new ReportCustomerReceipt_Result();
                        customerrecpay.Date = receipt.RecPayDate.Value.ToString("dd-MMM-yyyy");
                        customerrecpay.ReceivedFrom = cust.Customer1;
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

                int totalamt = 0;

                //foreach (var item in dd)
                //{
                //    totalamt = 5000;
                //}

                totalamt = 5000;

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




                DataRow r = dtcurrency.NewRow();
                r[0] = "";
                r[1] = "";
                r[2] = "";
                r[3] = "";
                r[4] = NumberToWords(totalamt);


                dtcurrency.Rows.Add(r);


                ReportDataSource _rsource3 = new ReportDataSource("Currency", dtcurrency);

                reportViewer.LocalReport.DataSources.Add(_rsource3);
                reportViewer.LocalReport.Refresh();
                //Byte  
                Warning[] warnings;
                string[] streamids;
                string mimeType, encoding, filenameExtension;

                byte[] bytes = reportViewer.LocalReport.Render("Pdf", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                //File  
                string FileName = "Customer_" + DateTime.Now.Ticks.ToString() + ".pdf";
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
                return Json(new {success=true, path = FilePathReturn }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(new {success=false, message = e.Message.ToString() }, JsonRequestBehavior.AllowGet);


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
