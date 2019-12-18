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
    public class SalesInvoiceController : Controller
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        MastersModel MM = new MastersModel();
        SalesInvoiceModel SM = new SalesInvoiceModel();
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
            AllInvoices = DAL.SP_GetAllSalesInvoiceByDate(Convert.ToDateTime(fdate), Convert.ToDateTime(tdate)).ToList();
                      
        //    var data = (from t in AllJobs where (t.JobDate >= Convert.ToDateTime(Session["FyearFrom"]) && t.JobDate <= Convert.ToDateTime(Session["FyearTo"])) select t).ToList();

            return View(AllInvoices);
        }

        [HttpGet]
        public ActionResult Invoice(string Command, int id)
        {
            SalesInvoice SI = new SalesInvoice();
            //PurchaseInvoiceModel vm = new PurchaseInvoiceModel();
            SI = SM.GetSalesInvoiceByID(id);
            BindAllMasters();
            return View(SI);
        }

        private bool DeleteAndInsertRecords(FormCollection formCollection, int InvoiceId)
        {

            DAL.SP_DeleteSalesInvoiceDetails(InvoiceId);
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
                Models.SalesInvoiceDetail SID = new Models.SalesInvoiceDetail();
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



                int iCharge = DAL.AddSalesInvoiceDetails(SID);
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
            SI.EmployeeID = Common.ParseInt(formCollection["EmployeeeID"]);
            SI.CurrencyID = Common.ParseInt(formCollection["SelectedCurrencyID"]);
            SI.ExchangeRate = Common.ParseDecimal(formCollection["ExchangeRate"]);
            SI.CreditDays = 0;
            SI.DueDate = Common.ParseDate(formCollection["DueDate"]);
            SI.AcJournalID = 0;
            SI.BranchID = Common.ParseInt(Session["branchid"].ToString()); ;
            SI.Discount =0;
            SI.StatusDiscountAmt =false;
            SI.OtherCharges = 0;
            SI.PaymentTerm = "";
            SI.Remarks = (formCollection["Remarks"]);
            SI.FYearID = Common.ParseInt(Session["fyearid"].ToString());
            SI.DeliveryId = Common.ParseInt(formCollection["SelectedDeliveryId"]);
            SI.QuotationNumber =(formCollection["QuotationNumber"]);
            SI.DiscountType = Common.ParseInt(formCollection["DiscountType"]);
            SI.DiscountValueLC = Common.ParseDecimal(formCollection["DiscountValueLC"]);
            SI.DiscountValueFC = Common.ParseDecimal(formCollection["DiscountValueFC"]);

            BindAllMasters();
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (Command == "Save")
            {
                SI.SalesInvoiceDate = System.DateTime.UtcNow;
                 int i = 0;
                i = SM.AddSalesInvoice(SI);
                SI.SalesInvoiceID = i;
                DeleteAndInsertRecords(formCollection, i);
                if (i > 0)
                {                                              
                    Session["SalesInvoiceID"] = i;
                    SI.SalesInvoiceID = i;
                    return RedirectToAction("Invoice", "SalesInvoice", new { ID = i });
                }
            }
            else if (Command == "Update")
            {

                SI.SalesInvoiceID = id;
                int k = SM.UpdateSalesInvoice(SI);
                DeleteAndInsertRecords(formCollection, id);
                return RedirectToAction("Invoice", "SalesInvoice", new { ID = SI.SalesInvoiceID });                                          
            }
            else if (Command == "SaveInvoice")
            {

            }
            return View(SI);
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
            DataSet ds= DAL.GetSalesInvoiceDetailsById(InvoiceId);
            if(ds != null && ds.Tables.Count > 0)
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
            var data = DAL.SP_GetAllSalesInvoiceByDate(Convert.ToDateTime(fdate), Convert.ToDateTime(tdate));
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

    }
}
