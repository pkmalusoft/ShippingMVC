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

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Invoice(FormCollection formCollection, string Command, int id)
        {
            PurchaseInvoice PI = new PurchaseInvoice();
            UpdateModel<JobGeneration>(PI);
            BindAllMasters();
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (Command == "Save")
            {
                PI.PurchaseInvoiceDate = System.DateTime.UtcNow;
                PI.PurchaseInvoiceNo = 0;
               // PurchaseInvoiceID = Convert.ToInt32(PM.MaxJobID()) - 1;
                 PI.PurchaseInvoiceID = PurchaseInvoiceId;

                i = PM.AddPurchaseInvoice(PI);
                PurchaseInvoiceID = i;

                if (i > 0)
                {
                    DeleteAndInsertRecords(formCollection, JobId);                                 

                    Session["PurchaseInvoiceID"] = PurchaseInvoiceId;

                    PM.PurchaseInvoiceID = PurchaseInvoiceId;
                    return RedirectToAction("Invoice", "Invoice", new { ID = PurchaseInvoiceId });
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
                            PI.PurchaseInvoiceID = Convert.ToInt32(Session["PurchaseInvoiceID"]);
                            int k = J.UpdateJob(PI);
                            if (k > 0)
                            {
                                JobId = JM.JobID;
                                DeleteAndInsertRecords(formCollection, JobId);
                                var data = (from c in entity.JobGenerations where c.JobID == JobId select c).FirstOrDefault();
                                int acjid = 0;
                                if (data.AcJournalID != null)
                                {
                                    acjid = data.AcJournalID.Value;
                                }
                                int acprovjid = 0;
                                if (data.AcProvisionCostJournalID != null)
                                {
                                    acprovjid = data.AcProvisionCostJournalID.Value;
                                }

                                decimal shome = 0;
                                decimal phome = 0;
                                if (entity.JInvoices.Where(x => x.JobID == JobId).Count() > 0)
                                {
                                    shome = entity.JInvoices.Where(x => x.JobID == JobId).Sum(x => x.SalesHome ?? 0);
                                    phome = entity.JInvoices.Where(x => x.JobID == JobId).Sum(x => x.ProvisionHome ?? 0);
                                }

                                int custcontrolacid = (from c in entity.AcHeadAssigns select c.CustomerControlAcID.Value).FirstOrDefault();
                                int freightacheadid = 158;
                                int provcontrolacid = (from c in entity.AcHeadAssigns select c.ProvisionCostControlAcID.Value).FirstOrDefault();
                                int accruedcontrolacid = (from c in entity.AcHeadAssigns select c.AccruedCostControlAcID.Value).FirstOrDefault();

                                int acjdetail1 = (from x in entity.AcJournalDetails where x.AcJournalID == acjid && x.AcHeadID == custcontrolacid select x.AcJournalDetailID).FirstOrDefault();

                                var data1 = (from x in entity.AcJournalDetails where x.AcJournalDetailID == acjdetail1 select x).FirstOrDefault();
                                if (data1 != null)
                                {
                                    data1.Amount = shome;
                                }
                                if (data1 != null)
                                {
                                    entity.Entry(data1).State = EntityState.Modified;
                                    entity.SaveChanges();
                                }
                                acjdetail1 = (from x in entity.AcJournalDetails where x.AcJournalID == acjid && x.AcHeadID == freightacheadid select x.AcJournalDetailID).FirstOrDefault();
                                data1 = (from x in entity.AcJournalDetails where x.AcJournalDetailID == acjdetail1 select x).FirstOrDefault();
                                if (data1 != null)
                                {
                                    data1.Amount = -shome;
                                    entity.Entry(data1).State = EntityState.Modified;
                                    entity.SaveChanges();
                                }

                                acjdetail1 = (from x in entity.AcJournalDetails where x.AcJournalID == acprovjid && x.AcHeadID == provcontrolacid select x.AcJournalDetailID).FirstOrDefault();
                                data1 = (from x in entity.AcJournalDetails where x.AcJournalDetailID == acjdetail1 select x).FirstOrDefault();
                                if (data1 != null)
                                {
                                    data1.Amount = phome;
                                    entity.Entry(data1).State = EntityState.Modified;
                                    entity.SaveChanges();
                                }

                                acjdetail1 = (from x in entity.AcJournalDetails where x.AcJournalID == acprovjid && x.AcHeadID == accruedcontrolacid select x.AcJournalDetailID).FirstOrDefault();
                                data1 = (from x in entity.AcJournalDetails where x.AcJournalDetailID == acjdetail1 select x).FirstOrDefault();
                                if (data1 != null)
                                {
                                    data1.Amount = -phome;
                                    entity.Entry(data1).State = EntityState.Modified;
                                    entity.SaveChanges();
                                }



                                //var acjournalid = (from m in entity.JobGenerations where m.JobID == JobId select m.AcJournalID).FirstOrDefault();
                                //if (acjournalid > 0)
                                //{
                                //    var data = (from t in entity.AcJournalDetails where t.AcJournalID == acjournalid select t).ToList();
                                //    var SumsaleHome = entity.JInvoices.Where(p => p.JobID == JM.JobID).Sum(c => c.SalesHome);
                                //    var accuredCostID = (from t in entity.AcHeadAssigns select t.AccruedCostControlAcID).FirstOrDefault();
                                //    var provisionCostid = (from t in entity.AcHeadAssigns select t.ProvisionCostControlAcID).FirstOrDefault();
                                //    foreach (var item in data)
                                //    {

                                //        AcJournalDetail acJouDetail = new AcJournalDetail();

                                //        if (item.AcHeadID == accuredCostID)
                                //        {
                                //            acJouDetail.Amount = -(SumsaleHome);
                                //        }
                                //        else
                                //        {
                                //            acJouDetail.Amount = +(SumsaleHome);
                                //        }
                                //        acJouDetail.AcHeadID = item.AcHeadID;
                                //        acJouDetail.AcJournalDetailID = item.AcJournalDetailID;
                                //        acJouDetail.AcJournalID = item.AcJournalID;
                                //        acJouDetail.ID = item.ID;
                                //        entity.Entry(item).CurrentValues.SetValues(acJouDetail);
                                //        //entity.Entry(acJouDetail).State = EntityState.Modified;
                                //        entity.SaveChanges();
                                //    }

                                //}


                            }

                        }
                        else
                        {
                            //  int j = J.UpdateJobIDinAllModules(JobId, Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["fyearid"]));
                        }
                        return RedirectToAction("JobDetails", "Job", new { ID = 20 });
                    }
                }
            }
            else if (Command == "SaveInvoice")
            {

            }
            return View(JM);
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
                            where t.PurchaseInvoicesID == null

                            select t;

                ViewBag.MainJobId = query;
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}

