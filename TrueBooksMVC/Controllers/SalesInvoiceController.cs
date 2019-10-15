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


        [HttpGet]
        public ActionResult Invoice(string Command, int id)
        {
            SalesInvoice SI = new SalesInvoice();
            //PurchaseInvoiceModel vm = new PurchaseInvoiceModel();
            SI = SM.GetSalesInvoiceByID(id);
            BindAllMasters();
            return View(SI);
        }

        [HttpPost]
        public ActionResult Invoice(FormCollection formCollection, string Command, int id)
        {
            SalesInvoice SI = new SalesInvoice();
            UpdateModel<SalesInvoice>(SI);
            BindAllMasters();
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (Command == "Save")
            {
                SI.SalesInvoiceDate = System.DateTime.UtcNow;
                //  PI.PurchaseInvoiceNo = 0;
                // PurchaseInvoiceID = Convert.ToInt32(PM.MaxJobID()) - 1;
                //  PI.PurchaseInvoiceID = PurchaseInvoiceId;
                int i = 0;
                i = SM.AddSalesInvoice(SI);
                //   PurchaseInvoiceID = i;

                if (i > 0)
                {
                    // DeleteAndInsertRecords(formCollection, i);                                 

                    Session["PurchaseInvoiceID"] = i;

                    SI.SalesInvoiceID = i;
                    return RedirectToAction("Index", "Invoice", new { ID = i });
                }
            }

            else if (Command == "Update")
            {
                if (Session["SalesInvoiceID"] != null)
                {
                    if (Convert.ToInt32(Session["SalesInvoiceID"]) > 0)
                    {
                        if (Session["UserID"] != null)
                        {
                            //  PI.PurchaseInvoiceID = Convert.ToInt32(Session["PurchaseInvoiceID"]);
                            int k = SM.UpdateJob(SI);
                            if (k > 0)
                            {
                                // PurchaseInvoiceID = PI.PurchaseInvoiceId;
                                //   DeleteAndInsertRecords(formCollection, PurchaseInvoiceId);
                            }
                        }
                        else
                        {

                        }
                        return RedirectToAction("Index", "Invoice", new { ID = SI.SalesInvoiceID });
                    }
                }
            }
            else if (Command == "SaveInvoice")
            {

            }
            return View(SM);
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




                var query = from t in entity.SalesInvoices
                            where t.SalesInvoiceID == null

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

            var ExRate = SM.GetCurrencyExchange(Convert.ToInt32(ID));

            return Json(ExRate, JsonRequestBehavior.AllowGet);
        }



    }
}
