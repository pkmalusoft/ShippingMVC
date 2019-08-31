using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;
using DAL;
using System.Dynamic;
using System.Data;

namespace TrueBooksMVC.Controllers
{
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

                    cust.CustomerRcieptChildVM = (from t in Context1.JobGenerations
                                                  join p in Context1.RecPayDetails on t.InvoiceNo equals p.InvoiceID
                                                  join s in Context1.JInvoices on t.JobID equals s.JobID
                                                  join r in Context1.RecPays on p.RecPayID equals r.RecPayID
                                                  where (r.RecPayID == id && p.InvoiceID != 0 && p.InvoiceID!=null)
                                                  select new CustomerRcieptChildVM
                                                  {
                                                      InvoiceDate = r.RecPayDate,
                                                      InvoiceID = p.InvoiceID.Value,
                                                      AmountToBeRecieved = s.SalesHome.Value,
                                                      Amount = -(p.Amount.Value),
                                                      Balance = s.SalesHome.Value,

                                                      RecPayDetailID = p.RecPayDetailID,
                                                      CurrencyId = p.CurrencyID.Value

                                                  }).ToList();



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
                            RP.InsertRecpayDetailsForCust(RecP.RecPayID, item.JobID,item.InvoiceID, Convert.ToDecimal(-item.Amount), "", "C", false, "", vInvoiceDate1, item.InvoiceNo.ToString(), Convert.ToInt32(RecP.CurrencyId), 3);

                            if (Advance > 0)
                            {
                                //Advance Amount entry
                                RP.InsertRecpayDetailsForCust(RecP.RecPayID, 0,0, Advance, null, "C", true, null, null, null, Convert.ToInt32(RecP.CurrencyId), 4);
                            }

                            TotalAmount = TotalAmount + item.Amount;
                        
                    }

                    //To Balance Invoice AMount
                    if (TotalAmount > 0)
                    {
                        int l = RP.InsertRecpayDetailsForCust(RecP.RecPayID, 0,0, TotalAmount, null, "C", false, null, null, null, Convert.ToInt32(RecP.CurrencyId), 4);

                        int fyaerId = Convert.ToInt32(Session["fyearid"].ToString());

                        RP.InsertJournalOfCustomer(RecP.RecPayID, fyaerId);
                    }
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
                   
                    //recpay.SupplierID = RecP.SupplierID;
                    Context1.Entry(recpay).State = EntityState.Modified;
                    Context1.SaveChanges();


                    foreach (var item in RecP.CustomerRcieptChildVM)
                    {
                      
                     

                        RecPayDetail recpd = new RecPayDetail();
                        recpd.RecPayDetailID = item.RecPayDetailID;
                        recpd.Amount =-(item.Amount);
                        recpd.CurrencyID = item.CurrencyId;
                        //recpd.InvDate = item.InvoiceDate.Value;
                        recpd.RecPayID = RecP.RecPayID;
                        recpd.Remarks = item.Remarks;
                        recpd.InvoiceID = item.InvoiceID;
                        recpd.StatusInvoice = "C";
                        if (item.AmountToBeRecieved < item.Amount)
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
                        }

                        Context1.Entry(recpd).State = EntityState.Modified;
                        Context1.SaveChanges();
                       
                       



                    }
                    int editrecPay = 0;
                    var sumOfAmount = Context1.RecPayDetails.Where(m => m.RecPayID == RecP.RecPayID && m.InvoiceID != 0).Sum(c => c.Amount);
                    editrecPay = editfu.EditRecpayDetailsCustR(RecP.RecPayID, Convert.ToInt32(sumOfAmount));
                    int editAcJdetails = editfu.EditAcJDetails(RecP.AcJournalID.Value, Convert.ToInt32(sumOfAmount));
                    
                }
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


            var cust = Context1.SP_GetAllRecieptsDetailsByDate(fdate, tdate, FYearID).ToList();

            string view = this.RenderPartialView("_GetAllCustomerByDate", cust);

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


            var cust = Context1.SP_GetAllRecieptsDetailsByDate(fdate, tdate, FYearID).ToList();

            return PartialView("_GetAllCustomerByDate", cust);

        }
    }
}
