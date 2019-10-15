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
    public class PurchaseInvoiceController : Controller
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        MastersModel MM = new MastersModel();

        PurchaseInvoiceModel PM = new PurchaseInvoiceModel();


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

        [HttpPost]
        public ActionResult Invoice(FormCollection formCollection, string Command, int id)
        {
            PurchaseInvoice PI = new PurchaseInvoice();
            UpdateModel<PurchaseInvoice>(PI);
            BindAllMasters();
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (Command == "Save")
            {
                PI.PurchaseInvoiceDate = System.DateTime.UtcNow;
                //  PI.PurchaseInvoiceNo = 0;
                // PurchaseInvoiceID = Convert.ToInt32(PM.MaxJobID()) - 1;
                //  PI.PurchaseInvoiceID = PurchaseInvoiceId;
                int i = 0;
                i = PM.AddPurchaseInvoice(PI);
             //   PurchaseInvoiceID = i;

                if (i > 0)
                {
                   // DeleteAndInsertRecords(formCollection, i);                                 

                    Session["PurchaseInvoiceID"] = i;

                    PI.PurchaseInvoiceID = i;
                    return RedirectToAction("Index", "Invoice", new { ID = i });
                }
            }

            else if (Command == "Update")
            {
                if (Session["PurchaseInvoiceID"] != null)
                {
                    if (Convert.ToInt32(Session["PurchaseInvoiceID"]) > 0)
                    {
                        if (Session["UserID"] != null)
                        {
                          //  PI.PurchaseInvoiceID = Convert.ToInt32(Session["PurchaseInvoiceID"]);
                            int k = PM.UpdateJob(PI);
                            if (k > 0)
                            {
                               // PurchaseInvoiceID = PI.PurchaseInvoiceId;
                             //   DeleteAndInsertRecords(formCollection, PurchaseInvoiceId);
                            }
                        }
                        else
                        {
                           
                        }
                        return RedirectToAction("Index", "Invoice", new { ID = PI.PurchaseInvoiceID });
                    }
                }
            }
            else if (Command == "SaveInvoice")
            {

            }
            return View(PM);
        }

        public void BindAllMasters()
        {
            try
            {

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
    }
   
}

