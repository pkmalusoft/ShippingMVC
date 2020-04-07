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
                    ViewBag.achead = Context1.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString())).OrderBy(x => x.AcHead).ToList();
                    ViewBag.acheadbank = Context1.AcHeadSelectForBank(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();

                }
                else
                {
                    BindMasters();
                    cust.RecPayDate = System.DateTime.UtcNow;
                    ViewBag.achead = Context1.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
                    ViewBag.acheadbank = Context1.AcHeadSelectForBank(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
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

            var Spayment = Context1.SP_GetAllPaymentsDetailsByDate(fdate, tdate, FYearID).ToList();


           return PartialView("_GetAllSupplierByDate", Spayment);
           
        }

    }
}

