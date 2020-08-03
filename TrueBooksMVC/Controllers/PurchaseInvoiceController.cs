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
    public class PurchaseInvoiceController : Controller
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        MastersModel MM = new MastersModel();

        PurchaseInvoiceModel PM = new PurchaseInvoiceModel();
        SourceMastersModel objectSourceModel = new SourceMastersModel();


        // GET: /PurchaseInvoice/


        public string getPurchaseInvoiceID()
        {
            string ID = "";

            if (Session["PurchaseInvoiceID"] != null)
            {
                ID = Session["PurchaseInvoiceID"].ToString();
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

        [HttpGet]
        public ActionResult Invoice(string Command, int id)
        {
            PurchaseInvoice PI = new PurchaseInvoice();
            //PurchaseInvoiceModel vm = new PurchaseInvoiceModel();
            PI = PM.GetPurchaseInvoiceByID(id);
            BindAllMasters();
            return View(PI);
        }

        public ActionResult Index()
        {
            DateTime fdate = Convert.ToDateTime(Session["FyearFrom"]);
            DateTime tdate = Convert.ToDateTime(Session["FyearTo"]);
            List<PurchaseInvoice> AllInvoices = new List<PurchaseInvoice>();
            AllInvoices = DAL.SP_GetAllPurchaseInvoiceByDate(Convert.ToDateTime(fdate), Convert.ToDateTime(tdate),true).ToList();

            //    var data = (from t in AllJobs where (t.JobDate >= Convert.ToDateTime(Session["FyearFrom"]) && t.JobDate <= Convert.ToDateTime(Session["FyearTo"])) select t).ToList();

            return View(AllInvoices);
        }
        private bool DeleteorInsertAcJounalDetails(int? AcMasterId, int PurchaseInvoiceId)
        {
            var deleteolddetails = (from d in entity.AcJournalDetails where d.AcJournalID == AcMasterId select d).ToList();
            foreach (var item in deleteolddetails)
            {
                entity.AcJournalDetails.Remove(item);
                entity.SaveChanges();
            }

            var Purchaseinvoicedetails = (from d in entity.PurchaseInvoiceDetails where d.PurchaseInvoiceID == PurchaseInvoiceId select d).ToList();

            var totalamount = Purchaseinvoicedetails.Sum(d => d.NetValue) - Purchaseinvoicedetails.Sum(d => d.Tax);
            var totaltax = Purchaseinvoicedetails.Sum(d => d.Tax);
            var salesval = totalamount + totaltax;
            var Cos = totalamount * 10 / 100;
            //saleskt
            var acjournaldetails = new AcJournalDetail();
            int maxAcJDetailID = 0;
            maxAcJDetailID = (from c in entity.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();
            acjournaldetails.AcJournalDetailID = maxAcJDetailID + 1;
            acjournaldetails.AcHeadID = (from d in entity.AcHeads where d.AcHead1.ToLower() == "sales kt" select d.AcHeadID).FirstOrDefault();//SalesKt
            acjournaldetails.AcJournalID = AcMasterId;
            acjournaldetails.Amount = totalamount * -1;
            entity.AcJournalDetails.Add(acjournaldetails);
            entity.SaveChanges();
            //Vat payable
            var acjournaldetails1 = new AcJournalDetail();
            acjournaldetails1.AcJournalDetailID = maxAcJDetailID + 2;
            acjournaldetails1.AcHeadID = (from d in entity.AcHeads where d.AcHead1.ToLower() == "vat payable" select d.AcHeadID).FirstOrDefault();//vatpayable
            acjournaldetails1.AcJournalID = AcMasterId;
            acjournaldetails1.Amount = totaltax * -1;
            entity.AcJournalDetails.Add(acjournaldetails1);
            entity.SaveChanges();
            //Customercontrolamt
            var acjournaldetails2 = new AcJournalDetail();
            acjournaldetails2.AcJournalDetailID = maxAcJDetailID + 3;
            acjournaldetails2.AcHeadID = (from d in entity.AcHeads where d.AcHead1.ToLower() == "customer control accounts" select d.AcHeadID).FirstOrDefault();//customercontrolaccounts
            acjournaldetails2.AcJournalID = AcMasterId;
            acjournaldetails2.Amount = salesval;
            entity.AcJournalDetails.Add(acjournaldetails2);
            entity.SaveChanges();
            //costofsales
            var acjournaldetails3 = new AcJournalDetail();
            acjournaldetails3.AcJournalDetailID = maxAcJDetailID + 4;
            acjournaldetails3.AcHeadID = (from d in entity.AcHeads where d.AcHead1.ToLower() == "cost of sales" select d.AcHeadID).FirstOrDefault();//costofsales
            acjournaldetails3.AcJournalID = AcMasterId;
            acjournaldetails3.Amount = Cos;
            entity.AcJournalDetails.Add(acjournaldetails3);
            entity.SaveChanges();
            //inventorycontrolaccounts

            var acjournaldetails4 = new AcJournalDetail();
            acjournaldetails4.AcJournalDetailID = maxAcJDetailID + 5;
            acjournaldetails4.AcHeadID = (from d in entity.AcHeads where d.AcHead1.ToLower() == "inventory control accounts" select d.AcHeadID).FirstOrDefault();//costofsales
            acjournaldetails4.AcJournalID = AcMasterId;
            acjournaldetails4.Amount = Cos * -1;
            entity.AcJournalDetails.Add(acjournaldetails4);
            entity.SaveChanges();
            return true;
        }
        private bool DeleteAndInsertRecords(FormCollection formCollection, int InvoiceId)
        {

            //DAL.SP_DeletePurchaseInvoiceDetails(InvoiceId);
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
                PurchaseInvoiceDetail PID = new PurchaseInvoiceDetail();
                var PDetailId = 0;
                if (formCollection.GetValue("PurchaseInvoiceDetailID_" + InvoiceDetailsArray[c]) != null)
                {
                    //PDetailId=
                         strArray = (string[])formCollection.GetValue("PurchaseInvoiceDetailID_" + InvoiceDetailsArray[c]).RawValue;
                    int.TryParse(strArray[0], out PDetailId);
                }
                PID.PurchaseInvoiceDetailID = PDetailId;
                    PID.PurchaseInvoiceID = InvoiceId;

                if (formCollection.GetValue("Description_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("Description_" + InvoiceDetailsArray[c]).RawValue;
                    PID.Description = strArray[0].Trim();
                }
                int Quantity = 0;
                if (formCollection.GetValue("Quantity_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("Quantity_" + InvoiceDetailsArray[c]).RawValue;
                    int.TryParse(strArray[0], out Quantity);
                }
                PID.Quantity = Quantity;
                int UnitTypeID = 0;
                if (formCollection.GetValue("Unit_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("Unit_" + InvoiceDetailsArray[c]).RawValue;
                    int.TryParse(strArray[0], out UnitTypeID);
                }
                PID.ItemUnitID = UnitTypeID;

                decimal Rate = 0;
                if (formCollection.GetValue("Rate_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("Rate_" + InvoiceDetailsArray[c]).RawValue;
                    decimal.TryParse(strArray[0], out Rate);
                }
                PID.Rate = Rate;

                decimal RateFC = 0;
                if (formCollection.GetValue("RateFC_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("RateFC_" + InvoiceDetailsArray[c]).RawValue;
                    decimal.TryParse(strArray[0], out RateFC);
                }
                PID.RateFC = RateFC;

                decimal Value = 0;
                if (formCollection.GetValue("Value_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("Value_" + InvoiceDetailsArray[c]).RawValue;
                    decimal.TryParse(strArray[0], out Value);
                }
                PID.Value = Value;


                decimal ValueFC = 0;
                if (formCollection.GetValue("ValueFC_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("ValueFC_" + InvoiceDetailsArray[c]).RawValue;
                    decimal.TryParse(strArray[0], out ValueFC);
                }
                PID.ValueFC = ValueFC;

                decimal Taxprec = 0;
                if (formCollection.GetValue("Taxpre_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("Taxpre_" + InvoiceDetailsArray[c]).RawValue;
                    decimal.TryParse(strArray[0], out Taxprec);
                }
                PID.Taxprec = Taxprec;

                decimal Tax = 0;
                if (formCollection.GetValue("Tax_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("Tax_" + InvoiceDetailsArray[c]).RawValue;
                    decimal.TryParse(strArray[0], out Tax);
                }
                PID.Tax = Tax;

                decimal NetValue = 0;
                if (formCollection.GetValue("NetValue_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("NetValue_" + InvoiceDetailsArray[c]).RawValue;
                    decimal.TryParse(strArray[0], out NetValue);
                }
                PID.NetValue = NetValue;

                int AcHeadID = 0;
                if (formCollection.GetValue("AcHead_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("AcHead_" + InvoiceDetailsArray[c]).RawValue;
                    int.TryParse(strArray[0], out AcHeadID);
                }
                PID.AcHeadID = AcHeadID;

                int JobID = 0;
                if (formCollection.GetValue("JobID_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("JobID_" + InvoiceDetailsArray[c]).RawValue;
                    int.TryParse(strArray[0], out JobID);
                }
                PID.JobID = JobID;
                int ProductID = 0;
                if (formCollection.GetValue("Product_" + InvoiceDetailsArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("Product_" + InvoiceDetailsArray[c]).RawValue;
                    int.TryParse(strArray[0], out ProductID);
                }
                PID.ProductID = ProductID;

                if (PDetailId > 0)
                {
                    int iCharge = DAL.UpdatePurchaseInvoiceDetail(PID);

                }
                else
                {
                    int iCharge = DAL.AddPurchaseInvoiceDetail(PID);
                }
            }

            return true;
        }

        [HttpPost]
        public ActionResult Invoice(FormCollection formCollection, string Command, int id)
        {
            PurchaseInvoice PI = new PurchaseInvoice();
            // UpdateModel<PurchaseInvoice>(PI);

            PI.PurchaseInvoiceNo = (formCollection["PurchaseInvoiceNo"]);
            PI.PurchaseInvoiceDate = Common.ParseDate(formCollection["PurchaseInvoiceDate"]);
            PI.Reference = (formCollection["Reference"]);
            PI.LPOReference = (formCollection["LPOReference"]);
            PI.SupplierID = Common.ParseInt(formCollection["SelectedSupplierID"]);
            PI.EmployeeID = Common.ParseInt(formCollection["SelectedEmployeeID"]);
            PI.QuotationNumber = formCollection["QuotationNumber"];
            PI.CurrencyID = Common.ParseInt(formCollection["SelectedCurrencyID"]);
            PI.ExchangeRate = Common.ParseDecimal(formCollection["ExchangeRate"]);
            PI.CreditDays = Common.ParseInt(formCollection["CreditDays"]);
            PI.DueDate = Common.ParseDate(formCollection["DueDate"]);
            PI.AcJournalID = 0;
            PI.BranchID = Common.ParseInt(Session["branchid"].ToString());
            PI.Discount = 0;
            PI.StatusDiscountAmt = false;
            PI.OtherCharges = 0;
            PI.PaymentTerm = "";
            PI.Remarks = (formCollection["Remarks"]);
            PI.FYearID = Common.ParseInt(Session["fyearid"].ToString());
            PI.DiscountType = Common.ParseInt(formCollection["DiscountType"]);
            PI.DiscountValueFC = Common.ParseDecimal(formCollection["DiscountValueFC"]);
            PI.DiscountValueLC = Common.ParseDecimal(formCollection["DiscountValueLC"]);
            PI.IsShipping = true;
            BindAllMasters();
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (Command == "Save")
            {
                //PI.PurchaseInvoiceDate = System.DateTime.UtcNow;
                var context = new SHIPPING_FinalEntities();

                var PurchaseinvoiceId = (from c in context.PurchaseInvoices orderby c.PurchaseInvoiceID descending select c.PurchaseInvoiceID).FirstOrDefault();
                PurchaseinvoiceId = PurchaseinvoiceId + 1;
                var Gen_Pno = PurchaseinvoiceId.ToString("00000");

                var PNo = "PI-" + Gen_Pno;
                PI.PurchaseInvoiceNo = PNo;
                int i = 0;
                i = PM.AddPurchaseInvoice(PI);
                PI.PurchaseInvoiceID = i;
                DeleteAndInsertRecords(formCollection, i);
                AcJournalMaster acJournalMaster = new AcJournalMaster();
                acJournalMaster.AcFinancialYearID = Convert.ToInt32(Session["fyearid"].ToString());
                acJournalMaster.AcJournalID = objectSourceModel.GetMaxNumberAcJournalMasters();
                acJournalMaster.VoucherType = "PI";

                int max = (from c in context.AcJournalMasters select c).ToList().Count();

                acJournalMaster.VoucherNo = PNo; //(max + 1).ToString();
                acJournalMaster.UserID = Convert.ToInt32(Session["UserID"].ToString());
                acJournalMaster.TransDate = PI.PurchaseInvoiceDate;
                acJournalMaster.StatusDelete = false;
                acJournalMaster.ShiftID = null;
                acJournalMaster.Remarks = PI.Remarks;
                acJournalMaster.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
                acJournalMaster.Reference = PI.Reference;
                context.AcJournalMasters.Add(acJournalMaster);
                context.SaveChanges();

                var acmasterid = acJournalMaster.AcJournalID;
                PI.AcJournalID = acmasterid;
                PM.UpdatePurchaseInvoice(PI);
                if (acmasterid > 0)
                {
                    DeleteorInsertAcJounalDetails(acmasterid, i);
                }
                if (i > 0)
                {
                    Session["PurchaseInvoiceID"] = i;
                    PI.PurchaseInvoiceID = i;
                    return RedirectToAction("Index", "PurchaseInvoice");
                }
            }
            else if (Command == "Update")
            {
                PI.PurchaseInvoiceID = id;
                var pinvoice = (from d in entity.PurchaseInvoices where d.PurchaseInvoiceID == id select d).FirstOrDefault();
                PI.AcJournalID = pinvoice.AcJournalID;
                if (PI.AcJournalID == null)
                {
                    PI.AcJournalID = 0;
                }
                int k = PM.UpdatePurchaseInvoice(PI);
                DeleteAndInsertRecords(formCollection, id);
                if (PI.AcJournalID > 0)
                {
                    DeleteorInsertAcJounalDetails(PI.AcJournalID, id);
                }
                return RedirectToAction("Index", "PurchaseInvoice");
            }
            else if (Command == "SaveInvoice")
            {

            }
            return RedirectToAction("Index", "PurchaseInvoice");

            //return View(PI);
        }

        public void BindAllMasters()
        {
            try
            {
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
                //  List<SP_GetAllJobsDetails_Result> Job = new List<SP_GetAllJobsDetails_Result>();
                //   List<SP_GetAllProductServices_Result> Product = new List<SP_GetAllProductServices_Result>();
                //  List<AcHeadSelectAll_Result>AccountHead = new List<AcHeadSelectAll_Result>();


                //  AccountHead = MM.AcHeadSelectAll(Common.ParseInt(Session["branchid"].ToString()));
                //   Product = MM.GetAllProductServices();
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
                //     Job = MM.GetAllJobsDetails();

                //   ViewBag.AccountHead = new SelectList(AccountHead, "AcHeadID", "AcHead");
                //  ViewBag.Product = new SelectList(Product, "ProductID", "ProductName");
                //   ViewBag.Job = new SelectList(Job, "JobID", "JobCode");
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

                var query = from t in entity.PurchaseInvoices
                            where t.PurchaseInvoiceID == null

                            select t;

                ViewBag.MainJobId = query;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonResult GetExchangeRte(string ID)
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();

            var ExRate = PM.GetCurrencyExchange(Convert.ToInt32(ID));

            return Json(ExRate, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPurchaseInvoiceDetailsByInvoiceId(int InvoiceId)
        {
            DataSet ds = DAL.GetPurchaseInvoiceDetailsById(InvoiceId);
            if (ds != null && ds.Tables.Count > 0)
            {
                List<PurchaseInvoiceDetailVM> dtList = ds.Tables[0].AsEnumerable()
        .Select(row => new PurchaseInvoiceDetailVM
        {
            PurchaseInvoiceDetailID = int.Parse(row["PurchaseInvoiceDetailID"].ToString()),
            PurchaseInvoiceID = int.Parse(row["PurchaseInvoiceID"].ToString()),
            ProductID = int.Parse(row["ProductID"].ToString()),
            ProductName = row["ProductName"].ToString(),
            Quantity = int.Parse(row["Quantity"].ToString()),
            ItemUnitID = int.Parse(row["ItemUnitID"].ToString()),
            Rate = decimal.Parse(row["Rate"].ToString()),
            RateFC = decimal.Parse(row["RateFC"].ToString()),
            Value = decimal.Parse(row["Value"].ToString()),
            ValueFC = decimal.Parse(row["ValueFC"].ToString()),
            Taxprec = decimal.Parse(row["Taxprec"].ToString()),
            Tax = decimal.Parse(row["Tax"].ToString()),
            NetValue = decimal.Parse(row["NetValue"].ToString()),
            AcHeadID = int.Parse(row["AcHeadID"].ToString()),
            AcHead = row["AcHead"].ToString(),
            JobID = int.Parse(row["JobID"].ToString()),
            JobCode = row["JobCode"].ToString(),
            Description = row["Description"].ToString()
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

        public ActionResult GetPurchaseInvoice(DateTime fdate, DateTime tdate)
        {
            var data = DAL.SP_GetAllPurchaseInvoiceByDate(Convert.ToDateTime(fdate), Convert.ToDateTime(tdate),true);           
            foreach(var item in data)
            {
                item.CurrencyID = entity.PurchaseInvoices.Find(item.PurchaseInvoiceID).CurrencyID;
                item.PaymentTerm = entity.CurrencyMasters.Where(d => d.CurrencyID == item.CurrencyID).FirstOrDefault().CurrencyName;
            }
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
        public ActionResult DeleteInvoice(int id)
        {
            var purchaseinvoice = (from d in entity.PurchaseInvoices where d.PurchaseInvoiceID == id select d).FirstOrDefault();
            // var purchaseinoiceDetails = (from d in entity.PurchaseInvoiceDetails where d.PurchaseInvoiceID == purchaseinvoice.PurchaseInvoiceID select d).ToList();
            var acjournalmaster = (from d in entity.AcJournalMasters where d.AcJournalID == purchaseinvoice.AcJournalID select d).FirstOrDefault();
            var acjournalDetails = (from d in entity.AcJournalDetails where d.AcJournalID == purchaseinvoice.AcJournalID select d).ToList();
            entity.PurchaseInvoices.Remove(purchaseinvoice);
            if (acjournalmaster != null)
            {
                entity.AcJournalMasters.Remove(acjournalmaster);
            }
            DAL.SP_DeletePurchaseInvoiceDetails(id);
            //foreach (var item in purchaseinoiceDetails)
            //{
            //    entity.PurchaseInvoiceDetails.Remove(item);
            //}
            foreach (var item in acjournalDetails)
            {
                entity.AcJournalDetails.Remove(item);
            }
            entity.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult InvoiceReport(int id)
        {
            var purchaseInvoice = entity.PurchaseInvoices.Find(id);
            DataSet ds = DAL.GetPurchaseInvoiceDetailsById(id);
            decimal? totalamount = 0;
            var Employee = entity.Employees.Find(purchaseInvoice.EmployeeID);
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
                List<PurchaseInvoiceDetailVM> dtList = ds.Tables[0].AsEnumerable().Select(row => new PurchaseInvoiceDetailVM
                {
                    PurchaseInvoiceDetailID = int.Parse(row["PurchaseInvoiceDetailID"].ToString()),
                    PurchaseInvoiceID = int.Parse(row["PurchaseInvoiceID"].ToString()),
                    ProductID = int.Parse(row["ProductID"].ToString()),
                    ProductName = row["ProductName"].ToString(),
                    Quantity = int.Parse(row["Quantity"].ToString()),
                    ItemUnitID = int.Parse(row["ItemUnitID"].ToString()),
                    Rate = decimal.Parse(row["Rate"].ToString()),
                    RateFC = decimal.Parse(row["RateFC"].ToString()),
                    Value = decimal.Parse(row["Value"].ToString()),
                    ValueFC = decimal.Parse(row["ValueFC"].ToString()),
                    Taxprec = decimal.Parse(row["Taxprec"].ToString()),
                    Tax = decimal.Parse(row["Tax"].ToString()),
                    NetValue = decimal.Parse(row["NetValue"].ToString()),
                    AcHeadID = int.Parse(row["AcHeadID"].ToString()),
                    AcHead = row["AcHead"].ToString(),
                    JobID = int.Parse(row["JobID"].ToString()),
                    JobCode = row["JobCode"].ToString(),
                    Description = row["Description"].ToString()
                }).ToList();
                ViewBag.Details = dtList;
                totalamount = dtList.Sum(d => d.NetValue) - dtList.Sum(d => d.Tax);

            }

            decimal? discount = 0;
            if (purchaseInvoice.DiscountType == 1)
            {
                if (purchaseInvoice.DiscountValueFC == 0 || purchaseInvoice.DiscountValueFC == null)
                {
                    discount = totalamount * purchaseInvoice.DiscountValueLC / 100;
                }
                else
                {
                    discount = totalamount * purchaseInvoice.DiscountValueFC / 100;
                }
            }
            else
            {
                if (purchaseInvoice.DiscountValueFC == 0 || purchaseInvoice.DiscountValueFC == null)
                {
                    discount = purchaseInvoice.DiscountValueLC;

                }
                else
                {
                    discount = purchaseInvoice.DiscountValueFC;

                }

            }
            ViewBag.Discount = discount;
            var Currency = entity.CurrencyMasters.Find(purchaseInvoice.CurrencyID);
            ViewBag.Currency = Currency;

            var PurchaseInvoiceDetails = entity.PurchaseInvoiceDetails.Where(d => d.PurchaseInvoiceID == id).ToList();
            var company = entity.AcCompanies.FirstOrDefault();
            var getsupplier = entity.Suppliers.Where(d => d.SupplierID == purchaseInvoice.SupplierID).FirstOrDefault();
            int uid = Convert.ToInt32(Session["UserID"].ToString());
            var uname = (from c in entity.UserRegistrations where c.UserID == uid select c.UserName).FirstOrDefault();
            var Itemunits = entity.ItemUnits.ToList();
            ViewBag.itemunit = Itemunits;
            var Basecurrency = entity.CurrencyMasters.Find(company.CurrencyID);
            ViewBag.BaseCurrency = Basecurrency.CurrencyName;
            ViewBag.Invoice = purchaseInvoice;
            ViewBag.Company = company;
            ViewBag.Supplier = getsupplier;
            ViewBag.username = uname;

            return View();
        }

        public ActionResult Supplier(string term)
        {
            MastersModel MM = new MastersModel();
            if (!String.IsNullOrEmpty(term))
            {
                List<SP_GetAllSupplier_Result> Supplier = new List<SP_GetAllSupplier_Result>();
                Supplier = MM.GetAllSupplier(term);
                return Json(Supplier, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<SP_GetAllSupplier_Result> Supplier = new List<SP_GetAllSupplier_Result>();
                Supplier = MM.GetAllSupplier();
                return Json(Supplier, JsonRequestBehavior.AllowGet);
            }
        }
    }


}

