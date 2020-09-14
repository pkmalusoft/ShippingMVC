using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;
using DAL;
using System.Dynamic;
using System.Data;
using System.Globalization;
using System.Collections;
namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    public class ClientInvoiceController : Controller
    {  // GET: VendorInvoice //account Receivables//salesinvoice
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        MastersModel MM = new MastersModel();
        SalesInvoiceModel SM = new SalesInvoiceModel();
        SourceMastersModel objectSourceModel = new SourceMastersModel();

        public string getSalesInvoiceID()
        {
            string ID = "";

            if (Session["SalesInvoiceID"] != null)
            {
                ID = Session["SalesInvoiceID"].ToString();
            }

            return ID;
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

        public ActionResult Index()
        {

            DateTime fdate = Convert.ToDateTime(Session["FyearFrom"]);
            DateTime tdate = Convert.ToDateTime(Session["FyearTo"]);
            List<SalesInvoice> AllInvoices = new List<SalesInvoice>();
            AllInvoices = DAL.SP_GetAllSalesInvoiceByDate(Convert.ToDateTime(fdate), Convert.ToDateTime(tdate),false).ToList();

            //    var data = (from t in AllJobs where (t.JobDate >= Convert.ToDateTime(Session["FyearFrom"]) && t.JobDate <= Convert.ToDateTime(Session["FyearTo"])) select t).ToList();

            return View(AllInvoices);
        }

        [HttpGet]
        public ActionResult Invoice(string Command, int id)
        {
            SalesInvoice SI = new SalesInvoice();
            //PurchaseInvoiceModel vm = new PurchaseInvoiceModel();
            SI = SM.GetSalesInvoiceByID(id);
            var context = new SHIPPING_FinalEntities();
            if (SI.SalesInvoiceID == 0)
            {
                var salesinvoiceId = (from c in context.SalesInvoices orderby c.SalesInvoiceID descending select c.SalesInvoiceID).FirstOrDefault();
                salesinvoiceId = salesinvoiceId + 1;
                var Gen_salesno = salesinvoiceId.ToString("00000");

                var salesNo = "SI-" + Gen_salesno;
                SI.SalesInvoiceNo = salesNo;
            }
            BindAllMasters();
            return View(SI);
        }
        private bool DeleteorInsertAcJounalDetails(int? AcMasterId, int salesinvoiceid)
        {
            var deleteolddetails = (from d in entity.AcJournalDetails where d.AcJournalID == AcMasterId select d).ToList();
            foreach (var item in deleteolddetails)
            {
                entity.AcJournalDetails.Remove(item);
                entity.SaveChanges();
            }
            var salesinvoice = (from d in entity.SalesInvoices where d.SalesInvoiceID == salesinvoiceid select d).FirstOrDefault();
            var salesinvoicedetails = (from d in entity.SalesInvoiceDetails where d.SalesInvoiceID == salesinvoiceid select d).ToList();

            var pageControl = (from d in entity.PageControlMasters where d.ControlName.ToLower() == "client invoice" select d).FirstOrDefault();
            var ControlFields = (from d in entity.PageControlFields where d.PageControlId == pageControl.Id select d).ToList();
            var accontrolheads = (from d in entity.AcHeadControls where d.Pagecontrol == pageControl.Id select d).ToList();

            var totalamount = salesinvoicedetails.Sum(d => d.NetValue) - salesinvoicedetails.Sum(d => d.Tax);
            var totaltax = salesinvoicedetails.Sum(d => d.Tax);
            var salesval = totalamount + totaltax;
            decimal? discount = 0;
            if (salesinvoice.DiscountType == 1)
            {
                if (salesinvoice.DiscountValueFC == 0 || salesinvoice.DiscountValueFC == null)
                {
                    discount = totalamount * salesinvoice.DiscountValueLC / 100;
                }
                else
                {
                    discount = totalamount * salesinvoice.DiscountValueFC / 100;
                }
            }
            else
            {
                if (salesinvoice.DiscountValueFC == 0 || salesinvoice.DiscountValueFC == null)
                {
                    discount = salesinvoice.DiscountValueLC;

                }
                else
                {
                    discount = salesinvoice.DiscountValueFC;

                }

            }
            var sumval = 0;
            foreach (var item in accontrolheads)
            {

                if (item.Remarks == 0)
                { }
                else
                {
                    var field = ControlFields.Where(d => d.Id == item.Remarks).FirstOrDefault();

                    if (field.FieldName.ToLower() == "invoice total")
                    {
                        var acjournal_details = new AcJournalDetail();
                        int maxAcJ_DetailID = 0;
                        maxAcJ_DetailID = (from c in entity.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();
                        acjournal_details.AcJournalDetailID = maxAcJ_DetailID + 1;
                        acjournal_details.AcHeadID = item.AccountHeadID;//SalesKt
                        acjournal_details.AcJournalID = AcMasterId;
                        if (item.AccountNature == true)
                        {
                            acjournal_details.Amount = totalamount;
                        }
                        else
                        {
                            acjournal_details.Amount = totalamount * -1;

                        }
                        entity.AcJournalDetails.Add(acjournal_details);
                        entity.SaveChanges();
                    }
                    else if (field.FieldName.ToLower() == "tax")
                    {
                        var acjournal_details = new AcJournalDetail();
                        int maxAcJ_DetailID = 0;
                        maxAcJ_DetailID = (from c in entity.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();
                        acjournal_details.AcJournalDetailID = maxAcJ_DetailID + 1;
                        acjournal_details.AcHeadID = item.AccountHeadID;//SalesKt
                        acjournal_details.AcJournalID = AcMasterId;
                        if (item.AccountNature == true)
                        {
                            acjournal_details.Amount = totaltax;
                        }
                        else
                        {
                            acjournal_details.Amount = totaltax * -1;

                        }
                        entity.AcJournalDetails.Add(acjournal_details);
                        entity.SaveChanges();

                    }
                    else if (field.FieldName.ToLower() == "discount")
                    {
                        var acjournal_details = new AcJournalDetail();
                        int maxAcJ_DetailID = 0;
                        maxAcJ_DetailID = (from c in entity.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();
                        acjournal_details.AcJournalDetailID = maxAcJ_DetailID + 1;
                        acjournal_details.AcHeadID = item.AccountHeadID;//SalesKt
                        acjournal_details.AcJournalID = AcMasterId;
                        if (item.AccountNature == true)
                        {
                            acjournal_details.Amount = discount;
                        }
                        else
                        {
                            acjournal_details.Amount = discount * -1;

                        }
                        entity.AcJournalDetails.Add(acjournal_details);
                        entity.SaveChanges();
                    }
                }


            }
            foreach (var item in accontrolheads)
            {
                var getallentries = (from d in entity.AcJournalDetails where d.AcJournalID == AcMasterId select d).ToList();
                var Creditamount = getallentries.Where(d => d.Amount < 0).ToList();
                var Debitamount = getallentries.Where(d => d.Amount > 0).ToList();
                var Diffval = (Creditamount.Sum(d => d.Amount) * -1) - Debitamount.Sum(d => d.Amount);
                if (item.Remarks == 0)
                {

                    var acjournal_details = new AcJournalDetail();
                    int maxAcJ_DetailID = 0;
                    maxAcJ_DetailID = (from c in entity.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();
                    acjournal_details.AcJournalDetailID = maxAcJ_DetailID + 1;
                    acjournal_details.AcHeadID = item.AccountHeadID;//SalesKt
                    acjournal_details.AcJournalID = AcMasterId;
                    if (item.AccountNature == true)
                    {
                        acjournal_details.Amount = Diffval;
                    }
                    else
                    {
                        acjournal_details.Amount = Diffval * -1;

                    }
                    entity.AcJournalDetails.Add(acjournal_details);
                    entity.SaveChanges();
                    break;

                }
            }

            return true;
        }

        private bool DeleteAndInsertRecords(FormCollection formCollection, int InvoiceId)
        {

            //DAL.SP_DeleteSalesInvoiceDetails(InvoiceId);
            int i = 0;
            int InvoiceDetailsCount = 0;
            ArrayList InvoiceDetailsArray = new ArrayList();

            for (int j = 0; j < formCollection.Keys.Count; j++)
            {
                if (formCollection.Keys[j].StartsWith("Description_"))
                {
                    InvoiceDetailsCount = InvoiceDetailsCount + 1;
                    InvoiceDetailsArray.Add(formCollection.Keys[j].Replace("Description_", "").Trim());
                }
            }

            for (int c = 0; c < InvoiceDetailsCount; c++)
            {
                string[] strArray;
                int SDetailId = 0;

                Models.SalesInvoiceDetail SID = new Models.SalesInvoiceDetail();
                if (formCollection.GetValue("SalesInvoiceDetailID_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("SalesInvoiceDetailID_" + InvoiceDetailsArray[c]).RawValue;
                    int.TryParse(strArray[0], out SDetailId);
                }
                SID.SalesInvoiceDetailID = SDetailId;
                SID.SalesInvoiceID = InvoiceId;

                int ProductID = 0;
                if (formCollection.GetValue("Product_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("Product_" + InvoiceDetailsArray[c]).RawValue;
                    int.TryParse(strArray[0], out ProductID);
                }
                SID.ProductID = ProductID;

                if (formCollection.GetValue("Description_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("Description_" + InvoiceDetailsArray[c]).RawValue;
                    SID.Description = strArray[0].Trim();
                }

                int Quantity = 0;
                if (formCollection.GetValue("Quantity_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("Quantity_" + InvoiceDetailsArray[c]).RawValue;
                    int.TryParse(strArray[0], out Quantity);
                }
                SID.Quantity = Quantity;
                int UnitTypeID = 0;
                if (formCollection.GetValue("Unit_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("Unit_" + InvoiceDetailsArray[c]).RawValue;
                    int.TryParse(strArray[0], out UnitTypeID);
                }
                SID.ItemUnitID = UnitTypeID;

                string RateType = "";
                if (formCollection.GetValue("RateType_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("RateType_" + InvoiceDetailsArray[c]).RawValue;
                    RateType = strArray[0].ToString();
                }
                SID.RateType = RateType;

                decimal RateLC = 0;
                if (formCollection.GetValue("RateLC_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("RateLC_" + InvoiceDetailsArray[c]).RawValue;
                    decimal.TryParse(strArray[0], out RateLC);
                }
                SID.RateLC = RateLC;

                decimal RateFC = 0;
                if (formCollection.GetValue("RateFC_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("RateFC_" + InvoiceDetailsArray[c]).RawValue;
                    decimal.TryParse(strArray[0], out RateFC);
                }
                SID.RateFC = RateFC;

                decimal Value = 0;
                //  if (formCollection.GetValue("Value_" + InvoiceDetailsArray[c]) != null)
                // {
                //    strArray = (string[])formCollection.GetValue("Value_" + InvoiceDetailsArray[c]).RawValue;
                //   decimal.TryParse(strArray[0], out Value);
                // }
                SID.Value = Value;


                decimal ValueLC = 0;
                if (formCollection.GetValue("ValueLC_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("ValueLC_" + InvoiceDetailsArray[c]).RawValue;
                    decimal.TryParse(strArray[0], out ValueLC);
                }
                SID.ValueLC = ValueLC;

                decimal ValueFC = 0;
                if (formCollection.GetValue("ValueFC_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("ValueFC_" + InvoiceDetailsArray[c]).RawValue;
                    decimal.TryParse(strArray[0], out ValueFC);
                }
                SID.ValueFC = ValueFC;


                decimal Tax = 0;
                if (formCollection.GetValue("Tax_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("Tax_" + InvoiceDetailsArray[c]).RawValue;
                    decimal.TryParse(strArray[0], out Tax);
                    //Tax = Convert.ToDecimal(strArray[0]);
                }
                SID.Tax = Tax;

                decimal NetValue = 0;
                if (formCollection.GetValue("NetValue_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("NetValue_" + InvoiceDetailsArray[c]).RawValue;
                    decimal.TryParse(strArray[0], out NetValue);
                }
                SID.NetValue = NetValue;


                int JobID = 0;
                if (formCollection.GetValue("JobID_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("JobID_" + InvoiceDetailsArray[c]).RawValue;
                    int.TryParse(strArray[0], out JobID);
                }
                SID.JobID = JobID;

                if (SID.ProductID == 0 && SID.NetValue == 0 && (SID.ValueFC == 0 || SID.ValueLC == 0))
                {

                }
                else
                {
                    if (SDetailId > 0)
                    {
                        //int iCharge = DAL.UpdatePurchaseInvoiceDetail(PID);
                        int iCharge = DAL.UpdateSalesInvoiceDetails(SID);
                    }
                    else
                    {
                        int iCharge = DAL.AddSalesInvoiceDetails(SID);
                    }

                }
            }
            return true;
        }


        [HttpPost]
        public ActionResult Invoice(FormCollection formCollection, string Command, int id)
        {
            SalesInvoice SI = new SalesInvoice();
            //  UpdateModel<SalesInvoice>(SI);
            SI.SalesInvoiceID = Common.ParseInt(formCollection["SalesInvoiceID"]);
            SI.SalesInvoiceNo = (formCollection["SalesInvoiceNo"]);
            SI.SalesInvoiceDate = Common.ParseDate(formCollection["SalesInvoiceDate"]);
            SI.Reference = (formCollection["Reference"]);
            SI.LPOReference = (formCollection["LPOReference"]);
            SI.CustomerID = Common.ParseInt(formCollection["SelectedCustomerID"]);
            //SI.EmployeeID = Common.ParseInt(formCollection["EmployeeeID"]);
            SI.EmployeeID = Common.ParseInt(formCollection["SelectedEmployeeID"]);
            SI.CurrencyID = Common.ParseInt(formCollection["SelectedCurrencyID"]);
            SI.ExchangeRate = Common.ParseDecimal(formCollection["ExchangeRate"]);
            SI.CreditDays = 0;
            SI.DueDate = Common.ParseDate(formCollection["DueDate"]);

            SI.BranchID = Common.ParseInt(Session["branchid"].ToString()); ;
            SI.Discount = 0;
            SI.StatusDiscountAmt = false;
            SI.OtherCharges = 0;
            SI.PaymentTerm = "";
            SI.Remarks = (formCollection["Remarks"]);
            SI.FYearID = Common.ParseInt(Session["fyearid"].ToString());
            SI.DeliveryId = Common.ParseInt(formCollection["SelectedDeliveryId"]);
            SI.QuotationNumber = (formCollection["QuotationNumber"]);
            SI.DiscountType = Common.ParseInt(formCollection["DiscountType"]);
            SI.DiscountValueLC = Common.ParseDecimal(formCollection["DiscountValueLC"]);
            SI.DiscountValueFC = Common.ParseDecimal(formCollection["DiscountValueFC"]);
            SI.IsShipping = false;
            BindAllMasters();
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (Command == "Save")
            {
                //SI.SalesInvoiceDate = System.DateTime.UtcNow;
                var context = new SHIPPING_FinalEntities();
                var salesinvoiceId = (from c in context.SalesInvoices orderby c.SalesInvoiceID descending select c.SalesInvoiceID).FirstOrDefault();
                salesinvoiceId = salesinvoiceId + 1;
                var Gen_salesno = salesinvoiceId.ToString("00000");

                var salesNo = "SI-" + Gen_salesno;
                SI.SalesInvoiceNo = salesNo;
                int i = 0;
                SI.AcJournalID = 0;
                i = SM.AddSalesInvoice(SI);
                SI.SalesInvoiceID = i;
                DeleteAndInsertRecords(formCollection, i);

                AcJournalMaster acJournalMaster = new AcJournalMaster();
                acJournalMaster.AcFinancialYearID = Convert.ToInt32(Session["fyearid"].ToString());
                acJournalMaster.AcJournalID = objectSourceModel.GetMaxNumberAcJournalMasters();
                acJournalMaster.VoucherType = "SI";

                int max = (from c in context.AcJournalMasters select c).ToList().Count();

                acJournalMaster.VoucherNo = salesNo; //(max + 1).ToString();
                acJournalMaster.UserID = Convert.ToInt32(Session["UserID"].ToString());
                acJournalMaster.TransDate = SI.SalesInvoiceDate;
                acJournalMaster.StatusDelete = false;
                acJournalMaster.ShiftID = null;
                acJournalMaster.Remarks = SI.Remarks;
                acJournalMaster.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
                acJournalMaster.Reference = SI.Reference;
                context.AcJournalMasters.Add(acJournalMaster);
                context.SaveChanges();

                var acmasterid = acJournalMaster.AcJournalID;
                SI.AcJournalID = acmasterid;
                SM.UpdateSalesInvoice(SI);
                if (acmasterid > 0)
                {
                    DeleteorInsertAcJounalDetails(acmasterid, i);
                }
                if (i > 0)
                {
                    Session["SalesInvoiceID"] = i;
                    SI.SalesInvoiceID = i;
                    //return RedirectToAction("Invoice", "SalesInvoice", new { ID = i });
                    return RedirectToAction("Index", "ClientInvoice");

                }
            }
            else if (Command == "Update")
            {

                SI.SalesInvoiceID = id;
                var salesinvoice = (from d in entity.SalesInvoices where d.SalesInvoiceID == id select d).FirstOrDefault();
                SI.AcJournalID = salesinvoice.AcJournalID;
                if (SI.AcJournalID == null)
                {
                    SI.AcJournalID = 0;
                }
                int k = SM.UpdateSalesInvoice(SI);
                DeleteAndInsertRecords(formCollection, id);
                if (SI.AcJournalID > 0)
                {
                    DeleteorInsertAcJounalDetails(SI.AcJournalID, id);
                }
                return RedirectToAction("Index", "ClientInvoice");

                //return RedirectToAction("Invoice", "SalesInvoice", new { ID = SI.SalesInvoiceID });                                          
            }
            else if (Command == "SaveInvoice")
            {

            }
            return RedirectToAction("Index", "ClientInvoice");

            //return View(SI);
        }



        public void BindAllMasters()
        {
            //   try
            //   {
            List<SP_GetAllPorts_Result> Ports = new List<SP_GetAllPorts_Result>();
            List<SP_GetAllEmployees_Result> Employees = new List<SP_GetAllEmployees_Result>();
            List<SP_GetAllJobType_Result> JobType = new List<SP_GetAllJobType_Result>();
            List<SP_GetAllCarrier_Result> Carriers = new List<SP_GetAllCarrier_Result>();
            List<SP_GetAllCustomers_Result> Customers = new List<SP_GetAllCustomers_Result>();
            List<SP_GetAllCountries_Result> Countries = new List<SP_GetAllCountries_Result>();
            List<SP_GetAllContainerTypes_Result> ContainerTypes = new List<SP_GetAllContainerTypes_Result>();
            List<SP_GetAllRevenueType_Result> RevenueType = new List<SP_GetAllRevenueType_Result>();
            List<SP_GetCurrency_Result> Currency = new List<SP_GetCurrency_Result>();
            List<SP_GetAllSupplier_Result> Supplier = new List<SP_GetAllSupplier_Result>();
            List<SP_GetShippingAgents_Result> ShippingAgent = new List<SP_GetShippingAgents_Result>();
            List<SP_GetAllItemUnit_Result> Unit = new List<SP_GetAllItemUnit_Result>();
            List<SP_GetAllJobsDetails_Result> Job = new List<SP_GetAllJobsDetails_Result>();
            List<SP_GetAllProductServices_Result> Product = new List<SP_GetAllProductServices_Result>();
            List<AcHeadSelectAll_Result> AccountHead = new List<AcHeadSelectAll_Result>();


            Ports = MM.GetAllPort();
            Employees = MM.GetAllEmployees();
            JobType = MM.GetJobTypeS();
            Carriers = MM.GetAllCarrier();
            Customers = MM.GetAllCustomer();
            Countries = MM.GetAllCountries();
            ContainerTypes = MM.GetAllContainerTypes();
            //    RevenueType = MM.GetRevenueType();
            Currency = MM.GetCurrency();
            Supplier = MM.GetAllSupplier();
            ShippingAgent = MM.GetShippingAgents();
            Unit = MM.GetItemUnit();
            Job = MM.GetAllJobsDetails();
            AccountHead = MM.AcHeadSelectAll(Common.ParseInt(Session["branchid"].ToString()));
            Product = MM.GetAllProductServices();

            var servicecustids = (from d in entity.CUSTOMERs where d.CustomerType == 2 select d.CustomerID).ToList();
            Customers = Customers.Where(d => servicecustids.Contains(d.CustomerID)).ToList();
            ViewBag.AccountHead = new SelectList(AccountHead, "AcHeadID", "AcHead");
            ViewBag.Product = new SelectList(Product, "ProductID", "ProductName");
            ViewBag.Job = new SelectList(Job, "JobID", "JobCode");
            ViewBag.Ports = new SelectList(Ports, "PortID", "Port");
            ViewBag.Customer = new SelectList(Customers, "CustomerID", "Customer");
            ViewBag.Employees = new SelectList(Employees, "EmployeeID", "EmployeeName");
            ViewBag.Countries = new SelectList(Countries, "CountryID", "CountryName");
            ViewBag.JobType = new SelectList(JobType, "JobTypeID", "JobDescription");
            ViewBag.Carriers = new SelectList(Carriers, "CarrierID", "Carrier");
            ViewBag.ContainerTypes = new SelectList(ContainerTypes, "ContainerTypeID", "ContainerType");
            ViewBag.Unit = new SelectList(Unit, "ItemUnitID", "ItemUnit");
            // ViewBag.RevenueT = new SelectList(RevenueType, "RevenueTypeID", "RevenueType");
            ViewBag.Curency = new SelectList(Currency, "CurrencyID", "CurrencyName");
            ViewBag.Suplier = new SelectList(Supplier, "SupplierID", "SupplierName");
            ViewBag.ShippingA = new SelectList(ShippingAgent.OrderBy(x => x.AgentName).ToList(), "ShippingAgentID", "AgentName");
            List<SelectListItem> objBLStatusList = new List<SelectListItem>
                    { new SelectListItem() { Text = "SURRENDERED", Selected = false, Value = "SURRENDERED"}
                     , new SelectListItem() { Text = "WAY BILL", Selected = false, Value = "WAY BILL"}
                     , new SelectListItem() { Text = "OBL REQUIRED", Selected = false, Value = "OBL REQUIRED"}};




            var query = from t in entity.SalesInvoices
                        where t.SalesInvoiceID == null

                        select t;

            ViewBag.MainJobId = query;
            //  }
            //    catch (Exception)
            //   {

            //      throw;
            //  }
        }

        public JsonResult GetExchangeRte(string ID)
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();

            var ExRate = SM.GetCurrencyExchange(Convert.ToInt32(ID));

            return Json(ExRate, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSalesInvoiceDetailsByInvoiceId(int InvoiceId)
        {
            DataSet ds = DAL.GetSalesInvoiceDetailsById(InvoiceId);
            if (ds != null && ds.Tables.Count > 0)
            {
                List<Models.SalesInvoiceDetail> dtList = ds.Tables[0].AsEnumerable()
        .Select(row => new Models.SalesInvoiceDetail
        {
            SalesInvoiceDetailID = int.Parse(row["SalesInvoiceDetailID"].ToString()),
            SalesInvoiceID = int.Parse(row["SalesInvoiceID"].ToString()),
            ProductID = int.Parse(row["ProductID"].ToString()),
            ProductName = row["ProductName"].ToString(),
            Quantity = int.Parse(row["Quantity"].ToString()),
            ItemUnitID = int.Parse(row["ItemUnitID"].ToString()),
            RateType = row["RateType"].ToString(),
            RateLC = decimal.Parse(row["RateLC"].ToString()),
            RateFC = decimal.Parse(row["RateFC"].ToString()),
            Value = decimal.Parse(row["Value"].ToString()),
            ValueLC = decimal.Parse(row["ValueLC"].ToString()),
            ValueFC = decimal.Parse(row["ValueFC"].ToString()),
            Tax = decimal.Parse(row["Tax"].ToString()),
            NetValue = decimal.Parse(row["NetValue"].ToString()),
            JobID = int.Parse(row["JobID"].ToString()),
            JobCode = row["JobCode"].ToString(),
            Description = row["Description"].ToString()
            //...
        }).ToList();
                return Json(dtList, JsonRequestBehavior.AllowGet);
                // return Json(ds.Tables[0], JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Failed = "failed";
                return Json(Failed, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetSalesInvoice(DateTime fdate, DateTime tdate)
        {
            var data = DAL.SP_GetAllSalesInvoiceByDate(Convert.ToDateTime(fdate), Convert.ToDateTime(tdate),false);
            foreach (var item in data)
            {
                var salesinvoicedetails = entity.SalesInvoiceDetails.Where(s => s.SalesInvoiceID == item.SalesInvoiceID).ToList();
                var totalamount = salesinvoicedetails.Sum(d => d.NetValue) - salesinvoicedetails.Sum(d => d.Tax);
                var totaltax = salesinvoicedetails.Sum(d => d.Tax);
                var salesval = totalamount + totaltax;
                var invoice = entity.SalesInvoices.Find(item.SalesInvoiceID);
                item.DiscountType = invoice.DiscountType;
                item.DiscountValueLC = invoice.DiscountValueLC;
                item.DiscountValueFC = invoice.DiscountValueFC;
                decimal? discount = 0;
                if (item.DiscountType == 1)
                {
                    if (item.DiscountValueFC == 0 || item.DiscountValueFC == null)
                    {
                        discount = totalamount * item.DiscountValueLC / 100;
                    }
                    else
                    {
                        discount = totalamount * item.DiscountValueFC / 100;
                    }
                }
                else
                {
                    if (item.DiscountValueFC == 0 || item.DiscountValueFC == null)
                    {
                        discount = item.DiscountValueLC;

                    }
                    else
                    {
                        discount = item.DiscountValueFC;

                    }

                }
                if (discount == null)
                {
                    discount = 0;
                }
                var det = salesinvoicedetails.Sum(d => d.NetValue);
                item.OtherCharges = salesinvoicedetails.Sum(d => d.NetValue) - discount;
                item.CurrencyID = invoice.CurrencyID;
                item.PaymentTerm = entity.CurrencyMasters.Where(d => d.CurrencyID == item.CurrencyID).FirstOrDefault().CurrencyName;
            }
            //data.ForEach(d => d.OtherCharges = entity.SalesInvoiceDetails.Where(s => s.SalesInvoiceID == d.SalesInvoiceID).ToList().Sum(a => a.NetValue));
            string view = this.RenderPartialView("_InvoiceView", data);

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


        public JsonResult GetProductInfo(int ID)
        {
            SourceMastersModel SM1 = new SourceMastersModel();

            var Product = SM1.GetProductById(ID);

            return Json(Product, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCreditDays(int ID)
        {
            SourceMastersModel SM1 = new SourceMastersModel();
            int? CreditDays = 0;
            var Product = SM1.GetCustomerById(ID);
            if (Product.MaxCreditDays != null)
            {
                CreditDays = Product.MaxCreditDays;
            }

            return Json(CreditDays, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteInvoice(int id)
        {
            var salesinvoice = (from d in entity.SalesInvoices where d.SalesInvoiceID == id select d).FirstOrDefault();
            var salesinoiceDetails = (from d in entity.SalesInvoiceDetails where d.SalesInvoiceID == salesinvoice.SalesInvoiceID select d).ToList();
            var acjournalmaster = (from d in entity.AcJournalMasters where d.AcJournalID == salesinvoice.AcJournalID select d).FirstOrDefault();
            var acjournalDetails = (from d in entity.AcJournalDetails where d.AcJournalID == salesinvoice.AcJournalID select d).ToList();
            entity.SalesInvoices.Remove(salesinvoice);
            if (acjournalmaster != null)
            {
                entity.AcJournalMasters.Remove(acjournalmaster);
            }
            foreach (var item in salesinoiceDetails)
            {
                entity.SalesInvoiceDetails.Remove(item);
            }
            foreach (var item in acjournalDetails)
            {
                entity.AcJournalDetails.Remove(item);
            }
            entity.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult InvoiceReport(int id)
        {
            var Salesinvoice = entity.SalesInvoices.Find(id);
            DataSet ds = DAL.GetSalesInvoiceDetailsById(id);
            decimal? totalamount = 0;
            var Employee = entity.Employees.Find(Salesinvoice.EmployeeID);
            if (Employee != null)
            {
                ViewBag.Employeename = Employee.EmployeeName;
            }
            else
            {
                ViewBag.Employeename = "";
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                List<Models.SalesInvoiceDetail> dtList = ds.Tables[0].AsEnumerable()
        .Select(row => new Models.SalesInvoiceDetail
        {
            SalesInvoiceDetailID = int.Parse(row["SalesInvoiceDetailID"].ToString()),
            SalesInvoiceID = int.Parse(row["SalesInvoiceID"].ToString()),
            ProductID = int.Parse(row["ProductID"].ToString()),
            ProductName = row["ProductName"].ToString(),
            Quantity = int.Parse(row["Quantity"].ToString()),
            ItemUnitID = int.Parse(row["ItemUnitID"].ToString()),
            RateType = row["RateType"].ToString(),
            RateLC = decimal.Parse(row["RateLC"].ToString()),
            RateFC = decimal.Parse(row["RateFC"].ToString()),
            Value = decimal.Parse(row["Value"].ToString()),
            ValueLC = decimal.Parse(row["ValueLC"].ToString()),
            ValueFC = decimal.Parse(row["ValueFC"].ToString()),
            Tax = decimal.Parse(row["Tax"].ToString()),
            NetValue = decimal.Parse(row["NetValue"].ToString()),
            JobID = int.Parse(row["JobID"].ToString()),
            JobCode = row["JobCode"].ToString(),
            Description = row["Description"].ToString(),
            //...
        }).ToList();
                ViewBag.Details = dtList;
                totalamount = dtList.Sum(d => d.NetValue) - dtList.Sum(d => d.Tax);

            }
            var Products = (from d in entity.ProductServices select d).ToList();
            ViewBag.Products = Products;
            decimal? discount = 0;
            if (Salesinvoice.DiscountType == 1)
            {
                if (Salesinvoice.DiscountValueFC == 0 || Salesinvoice.DiscountValueFC == null)
                {
                    discount = totalamount * Salesinvoice.DiscountValueLC / 100;
                }
                else
                {
                    discount = totalamount * Salesinvoice.DiscountValueFC / 100;
                }
            }
            else
            {
                if (Salesinvoice.DiscountValueFC == 0 || Salesinvoice.DiscountValueFC == null)
                {
                    discount = Salesinvoice.DiscountValueLC;

                }
                else
                {
                    discount = Salesinvoice.DiscountValueFC;

                }

            }
            ViewBag.Discount = discount;
            var Currency = entity.CurrencyMasters.Find(Salesinvoice.CurrencyID);
            ViewBag.Currency = Currency;
            var PurchaseInvoiceDetails = entity.PurchaseInvoiceDetails.Where(d => d.PurchaseInvoiceID == id).ToList();
            var company = entity.AcCompanies.FirstOrDefault();
            var getCustomer = entity.CUSTOMERs.Where(d => d.CustomerID == Salesinvoice.CustomerID).FirstOrDefault();
            int uid = Convert.ToInt32(Session["UserID"].ToString());
            var uname = (from c in entity.UserRegistrations where c.UserID == uid select c.UserName).FirstOrDefault();
            var Basecurrency = entity.CurrencyMasters.Find(company.CurrencyID);
            ViewBag.BaseCurrency = Basecurrency.CurrencyName;
            var Itemunits = entity.ItemUnits.ToList();
            ViewBag.itemunit = Itemunits;
            ViewBag.Invoice = Salesinvoice;
            ViewBag.Company = company;
            ViewBag.Supplier = getCustomer;
            ViewBag.username = uname;

            return View();
        }
        public bool TryGetCurrencySymbol(string ISOCurrencySymbol, out string symbol)
        {
            symbol = CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture)
                .Select(culture =>
                {
                    try
                    {
                        return new RegionInfo(culture.Name);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(ri => ri != null && ri.ISOCurrencySymbol == ISOCurrencySymbol)
                .Select(ri => ri.CurrencySymbol)
                .FirstOrDefault();
            return symbol != null;
        }
        public ActionResult Customer(string term)
        {
            MastersModel MM = new MastersModel();
            if (!String.IsNullOrEmpty(term))
            {
                List<SP_GetAllCustomers_Result> CustomerList = new List<SP_GetAllCustomers_Result>();
                CustomerList = MM.GetAllCustomer(term);
                var servicecustids = (from d in entity.CUSTOMERs where d.CustomerType == 2 select d.CustomerID).ToList();
                CustomerList = CustomerList.Where(d => servicecustids.Contains(d.CustomerID)).ToList();

                return Json(CustomerList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<SP_GetAllCustomers_Result> CustomerList = new List<SP_GetAllCustomers_Result>();
                CustomerList = MM.GetAllCustomer();
                var servicecustids = (from d in entity.CUSTOMERs where d.CustomerType == 2 select d.CustomerID).ToList();
                CustomerList = CustomerList.Where(d => servicecustids.Contains(d.CustomerID)).ToList();

                return Json(CustomerList, JsonRequestBehavior.AllowGet);
            }
        }
    }
}