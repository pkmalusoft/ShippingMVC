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
    [Authorize]
    public class JobController : Controller
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        MastersModel MM = new MastersModel();
        JobModel J = new JobModel();

        //
        // GET: /Job/
        [HttpGet]
        public ActionResult Job(int id)
        {
            Session["Cargoid"] = null;
            Session["InvoiceID"] = null;
            Session["AddBill"] = null;
            Session["ContainerID"] = null;
            Session["AddLog"] = null;

            JobGeneration JG = new JobGeneration();


            if (Session["UserID"] != null)
            {

                BindAllMasters();


                Session["JobID"] = id;

                JG = J.JobGenerationByJobID(id);

                if (id == 0)
                {
                    JG.InvoiceDate = System.DateTime.UtcNow;
                    JG.JobDate = System.DateTime.UtcNow;
                    JG.InvoiceNo = Convert.ToInt32(J.GetMaxInvoiceNumber());
                    var ji = entity.JInvoices.Where(a => a.JobID == 0).ToList();
                    foreach (var i in ji)
                    {
                        entity.JInvoices.Remove(i);
                        entity.SaveChanges();
                    }
                    var JC = entity.JContainerDetails.Where(a => a.JobID == 0).ToList();
                    foreach (var i in JC)
                    {
                        entity.JContainerDetails.Remove(i);
                        entity.SaveChanges();
                    }
                    var Jcargo = entity.JCargoDescriptions.Where(a => a.JobID == 0).ToList();
                    foreach (var i in Jcargo)
                    {
                        entity.JCargoDescriptions.Remove(i);
                        entity.SaveChanges();
                    }
                    var JBillofEntry = entity.JBIllOfEntries.Where(a => a.JobID == 0).ToList();
                    foreach (var i in JBillofEntry)
                    {
                        entity.JBIllOfEntries.Remove(i);
                        entity.SaveChanges();
                    }
                    var JAuditlog = entity.JAuditLogs.Where(a => a.JobID == 0).ToList();
                    foreach (var i in JAuditlog)
                    {
                        entity.JAuditLogs.Remove(i);
                        entity.SaveChanges();
                    }
                }
                else
                {
                    if (JG.InvoiceDate.ToString() != "")
                    {
                        JG.InvoiceDate = JG.InvoiceDate;
                    }
                    else
                    {
                        JG.InvoiceDate = System.DateTime.UtcNow;
                    }

                    if (Convert.ToInt32(JG.InvoiceNo) > 0)
                    {
                        JG.InvoiceNo = JG.InvoiceNo;
                        ViewBag.IsInvoiveGenerated = true;
                    }
                    else
                    {

                        ViewBag.IsInvoiveGenerated = false;
                        var invoice = (from t in entity.JobGenerations where t.JobID == JG.JobID select t.InvoiceNo).FirstOrDefault();

                        JG.InvoiceNo = Convert.ToInt32(invoice.Value);
                        if (invoice == 0)
                        {
                            invoice = Convert.ToInt32(J.GetMaxInvoiceNumber());
                            JG.InvoiceNo = invoice;
                        }

                        bool x = (from c in entity.JobTypes where c.JobTypeID == JG.JobTypeID select c.StatusSea).FirstOrDefault().Value;

                        if (x == false)
                        {
                            var d = entity.JobGenerations.Find(id);

                            JG.Flight = d.Flight;
                            JG.HAWB = d.HAWB;
                            JG.MAWB = d.MAWB;
                            JG.DepartingDate = d.DepartingDate;
                        }

                    }
                }

            }
            else
            {
                return RedirectToAction("Login", "Login", new { ID = "1" });
            }

            var query = from t in entity.JobGenerations
                        where t.MainJobID == null || t.MainJobID == 0

                        select t;

            ViewBag.MainJobId = query;

            return View(JG);

        }

        public ActionResult JobDetails(int ID)
        {
            List<SP_GetAllJobsDetails_Result> AllJobs = new List<SP_GetAllJobsDetails_Result>();

            // AllJobs = J.AllJobsDetails();
            DateTime a = Convert.ToDateTime(Session["FyearFrom"]);
            DateTime b = Convert.ToDateTime(Session["FyearTo"]);
            var data = (from t in AllJobs where (t.JobDate >= Convert.ToDateTime(Session["FyearFrom"]) && t.JobDate <= Convert.ToDateTime(Session["FyearTo"])) select t).ToList();

            if (ID > 0)
            {
                ViewBag.SuccessMsg = "You have successfully added Job.";
            }

            if (ID == 10)
            {
                ViewBag.SuccessMsg = "You have successfully deleted Job.";
            }

            if (ID == 20)
            {
                ViewBag.SuccessMsg = "You have successfully updated Job.";
            }

            Session["ID"] = ID;

            return View(AllJobs);
        }

        public string getJobID()
        {
            string ID = "";

            if (Session["JobID"] != null)
            {
                ID = Session["JobID"].ToString();
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

        [HttpPost]
        public ActionResult Job(FormCollection formCollection, string Command, int id)
        {
            JobGeneration JM = new JobGeneration();
            UpdateModel<JobGeneration>(JM);
            
            int i;
            int JobId = 0;
            BindAllMasters();
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (Command == "Save")
            {
                JM.InvoiceDate = System.DateTime.UtcNow;
                JM.InvoiceNo = 0;
                JobId = Convert.ToInt32(J.MaxJobID()) - 1;
                //  JM.JobID = JobId;

                i = J.AddJob(JM);
                JobId = i;

                if (i > 0)
                {
                    DeleteAndInsertRecords(formCollection, JobId);

                    /*  int ChargesCount = 0;
                      ArrayList ChargesArray = new ArrayList();
                      for (int j=0;j<formCollection.Keys.Count; j++)
                      {
                          if(formCollection.Keys[j].StartsWith("RevenueTypeID_")){
                              ChargesCount = ChargesCount + 1;
                              ChargesArray.Add(formCollection.Keys[j].Replace("RevenueTypeID_", "").Trim());
                          }
                      }
                      for(int c = 1; c <= ChargesCount; c++)
                      {
                          string[] strArray;
                          JInvoice Charges = new JInvoice();
                          Charges.UserID = Convert.ToInt32(Session["UserID"].ToString());
                          Charges.JobID = JobId;
                          int RevenueTypeID = 0;
                          if (formCollection.GetValue("RevenueTypeID_" + ChargesArray[c]) != null) {
                              strArray = (string[]) formCollection.GetValue("RevenueTypeID_" + ChargesArray[c]).RawValue;
                              int.TryParse(strArray[0], out RevenueTypeID);
                          }
                          Charges.RevenueTypeID = RevenueTypeID;
                          int SupplierId = 0;
                          if (formCollection.GetValue("SupplierID_" + ChargesArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("SupplierID_" + ChargesArray[c]).RawValue;
                             int.TryParse(strArray[0], out SupplierId);
                          }
                          Charges.SupplierID = SupplierId;
                          if (formCollection.GetValue("ChargeDescription_" + ChargesArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("ChargeDescription_" + ChargesArray[c]).RawValue;
                              Charges.Description = strArray[0].Trim();
                          }
                          double Qty = 0;
                          if (formCollection.GetValue("Quantity_" + c.ToString()) != null)
                          {
                              strArray = (string[])formCollection.GetValue("Quantity_" + ChargesArray[c]).RawValue;
                              double.TryParse(strArray[0], out Qty);
                          }
                          Charges.Quantity = Qty;
                          decimal ProvRate = 0;
                          if (formCollection.GetValue("ProvisionRate_" + c.ToString()) != null)
                          {
                              strArray = (string[])formCollection.GetValue("ProvisionRate_" + ChargesArray[c]).RawValue;
                              decimal.TryParse(strArray[0], out ProvRate);
                          }
                          Charges.ProvisionRate = ProvRate;
                          int ProvCurrency = 0;
                          if (formCollection.GetValue("ProvisionCurrencyId_" + ChargesArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("ProvisionCurrencyId_" + ChargesArray[c]).RawValue;
                              int.TryParse(strArray[0], out ProvCurrency);
                          }
                          Charges.ProvisionCurrencyID = ProvCurrency;
                          decimal ProvExRate = 0;
                          if (formCollection.GetValue("ProvisionExchangeRate_" + ChargesArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("ProvisionExchangeRate_" + ChargesArray[c]).RawValue;
                              decimal.TryParse(strArray[0].Trim(), out ProvExRate);
                          }
                          Charges.ProvisionExchangeRate = ProvExRate;

                          decimal ProvHome = 0;
                          if (formCollection.GetValue("ProvisionHome_" + ChargesArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("ProvisionHome_" + ChargesArray[c]).RawValue;
                              decimal.TryParse(strArray[0].Trim(), out ProvHome);
                          }
                          Charges.ProvisionHome = ProvHome;

                          decimal ProvForiegn = 0;
                          if (formCollection.GetValue("ProvisionForeign_" + ChargesArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("ProvisionForeign_" + ChargesArray[c]).RawValue;
                              decimal.TryParse(strArray[0].Trim(), out ProvForiegn);
                          }
                          Charges.ProvisionForeign = ProvForiegn;

                          decimal SalesRate = 0;
                          if (formCollection.GetValue("SalesRate_" + ChargesArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("SalesRate_" + ChargesArray[c]).RawValue;
                              decimal.TryParse(strArray[0].Trim(), out SalesRate);
                          }
                          Charges.SalesRate = ProvForiegn;
                          int SalesCurrencyId = 0;
                          if (formCollection.GetValue("SalesCurrencyId_" + ChargesArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("SalesCurrencyId_" + ChargesArray[c]).RawValue;
                              int.TryParse(strArray[0].Trim(), out SalesCurrencyId);
                          }
                          Charges.SalesCurrencyID = SalesCurrencyId;
                          decimal SalesExRate = 0;
                          if (formCollection.GetValue("SalesExchangeRate_" + ChargesArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("SalesExchangeRate_" + ChargesArray[c]).RawValue;
                              decimal.TryParse(strArray[0].Trim(), out SalesExRate);
                          }
                          Charges.SalesExchangeRate = SalesExRate;

                          decimal SalesHome = 0;
                          if (formCollection.GetValue("SalesHome_" + ChargesArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("SalesHome_" + ChargesArray[c]).RawValue;
                              decimal.TryParse(strArray[0].Trim(), out SalesHome);
                          }
                          Charges.SalesHome = SalesHome;

                          decimal SalesForeign = 0;
                          if (formCollection.GetValue("SalesForeign_" + ChargesArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("SalesHome_" + ChargesArray[c]).RawValue;
                              decimal.TryParse(strArray[0].Trim(), out SalesForeign);
                          }
                          Charges.SalesForeign = SalesForeign;
                          decimal Cost = 0;
                          if (formCollection.GetValue("Cost_" + ChargesArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("SalesHome_" + ChargesArray[c]).RawValue;
                              decimal.TryParse(strArray[0].Trim(), out Cost);
                          }
                          Charges.Cost = Cost;

                          int iCharge = J.AddCharges(Charges, Session["UserID"].ToString());
                      }
                      int CargoCount = 0;
                      ArrayList CargoArray = new ArrayList();
                      for (int j = 0; j < formCollection.Keys.Count; j++)
                      {
                          if (formCollection.Keys[j].StartsWith("Mark_"))
                          {
                              CargoCount = CargoCount + 1;
                              CargoArray.Add(formCollection.Keys[j].Replace("Mark_","").Trim());
                          }
                      }
                      for (int c = 1; c <= CargoCount; c++)
                      {
                          string[] strArray;
                          JCargoDescription Cargo = new JCargoDescription();
                          Cargo.UserID = Session["UserID"].ToString();
                          Cargo.JobID = JobId;
                          string MarkId = "";
                          if (formCollection.GetValue("Mark_" + CargoArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("Mark_" + CargoArray[c]).RawValue;
                              MarkId = strArray[0];
                          }
                          string Description = "";
                          if (formCollection.GetValue("CarDescription_" + c.ToString()) != null)
                          {
                              strArray = (string[])formCollection.GetValue("CarDescription_" + CargoArray[c]).RawValue;
                              Description = strArray[0];
                          }
                          Cargo.Description = Description;
                          decimal Weight = 0;
                          if (formCollection.GetValue("weight_" + ChargesArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("weight_" + ChargesArray[c]).RawValue;
                              decimal.TryParse(strArray[0].Trim(), out Weight);
                          }
                          Cargo.weight = Weight;
                          decimal Volume = 0;
                          if (formCollection.GetValue("volume_" + ChargesArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("volume_" + ChargesArray[c]).RawValue;
                              decimal.TryParse(strArray[0].Trim(), out Volume);
                          }
                          Cargo.volume = Volume;

                          decimal Packages = 0;
                          if (formCollection.GetValue("Packages_" + ChargesArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("Packages_" + ChargesArray[c]).RawValue;
                              decimal.TryParse(strArray[0].Trim(), out Packages);
                          }
                          Cargo.Packages = Packages;

                          decimal GrossWeight = 0;
                          if (formCollection.GetValue("GrossWeight_" + ChargesArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("GrossWeight_" + ChargesArray[c]).RawValue;
                              decimal.TryParse(strArray[0].Trim(), out GrossWeight);
                          }
                          Cargo.GrossWeight = GrossWeight;
                          i = J.AddCargo(Cargo, Session["UserID"].ToString());
                      }

                      int ContainerCount = 0;
                      ArrayList ContainerArray = new ArrayList();
                      for (int j = 0; j < formCollection.Keys.Count; j++)
                      {
                          if (formCollection.Keys[j].StartsWith("ContainerType_"))
                          {
                              ContainerCount = ContainerCount + 1;
                              ContainerArray.Add(formCollection.Keys[j].Replace("ContainerType_", "").Trim());
                          }
                      }
                      for (int c = 1; c <= ContainerCount; c++)
                      {
                          string[] strArray;
                          JContainerDetail ContainerObj = new JContainerDetail();
                          ContainerObj.JobID = JobId;
                          int ContainerTypeId = 0;
                          if (formCollection.GetValue("ContainerTypeID_" + ContainerArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("ContainerTypeID_" + ContainerArray[c]).RawValue;
                              int.TryParse(strArray[0].Trim(), out ContainerTypeId);
                          }
                          ContainerObj.ContainerTypeID = ContainerTypeId;
                          string ContainerNo = "";
                          if (formCollection.GetValue("ContainerNo_" + ContainerArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("ContainerNo_" + ContainerArray[c]).RawValue;
                              ContainerNo = strArray[0].Trim();
                          }
                          ContainerObj.ContainerNo = ContainerNo;
                          string SealNo = "";
                          if (formCollection.GetValue("SealNo_" + ContainerArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("SealNo_" + ContainerArray[c]).RawValue;
                              SealNo = strArray[0].Trim();
                          }
                          ContainerObj.SealNo = SealNo;
                          string ContainerDescription = "";
                          if (formCollection.GetValue("ContainerDescription_" + ContainerArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("ContainerDescription_" + ContainerArray[c]).RawValue;
                              ContainerDescription = strArray[0].Trim();
                          }
                          ContainerObj.Description = ContainerDescription;
                          AddContainerDetails(ContainerObj);
                      }

                      int BillOfEntryCount = 0;
                      ArrayList BillOfEntryArray = new ArrayList();
                      for (int j = 0; j < formCollection.Keys.Count; j++)
                      {
                          if (formCollection.Keys[j].StartsWith("BIllOfEntry_"))
                          {
                              BillOfEntryCount = ContainerCount + 1;
                              BillOfEntryArray.Add(formCollection.Keys[j].Replace("BIllOfEntry_", "").Trim());
                          }
                      }
                      for (int c = 1; c <= BillOfEntryCount; c++)
                      {
                          string[] strArray;
                          JBIllOfEntry objBillOfEntry = new JBIllOfEntry();
                          objBillOfEntry.JobID = JobId;
                          string BIllOfEntry = "";
                          if (formCollection.GetValue("BIllOfEntry_" + BillOfEntryArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("BIllOfEntry_" + BillOfEntryArray[c]).RawValue;
                              BIllOfEntry = strArray[0].Trim();
                          }
                          objBillOfEntry.BIllOfEntry = BIllOfEntry;
                          DateTime BIllOfEntryDate;
                          if (formCollection.GetValue("BillofEntryDate_" + BillOfEntryArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("BillofEntryDate_" + BillOfEntryArray[c]).RawValue;
                              DateTime.TryParseExact(strArray[0].Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out BIllOfEntryDate);
                              objBillOfEntry.BillofEntryDate = BIllOfEntryDate;
                          }
                          int ShippingAgentId = 0;
                          if (formCollection.GetValue("ShippingAgentID_" + BillOfEntryArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("ShippingAgentID_" + BillOfEntryArray[c]).RawValue;
                              int.TryParse(strArray[0].Trim(), out ShippingAgentId);
                          }
                          objBillOfEntry.ShippingAgentID = ShippingAgentId;
                          AddBill(objBillOfEntry);
                      }
                      int NotificationCount = 0;
                      ArrayList NotificationArray = new ArrayList();
                      for (int j = 0; j < formCollection.Keys.Count; j++)
                      {
                          if (formCollection.Keys[j].StartsWith("AuditTransDate_"))
                          {
                              NotificationCount = NotificationCount + 1;
                              NotificationArray.Add(formCollection.Keys[j].Replace("AuditTransDate_", "").Trim());
                          }
                      }
                      for (int c = 1; c <= ContainerCount; c++)
                      {
                          JAuditLog objAudit = new JAuditLog();
                          objAudit.JobID = JobId;
                          string[] strArray;
                          DateTime NotificationDate;
                          if (formCollection.GetValue("AuditTransDate_" + NotificationArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("AuditTransDate_" + NotificationArray[c]).RawValue;
                              DateTime.TryParseExact(strArray[0].Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out NotificationDate);
                              objAudit.TransDate = NotificationDate;
                          }

                          if (formCollection.GetValue("AuditRemarks_" + NotificationArray[c]) != null)
                          {
                              strArray = (string[])formCollection.GetValue("AuditRemarks_" + NotificationArray[c]).RawValue;
                              objAudit.Remarks = strArray[0].Trim();
                          }
                          objAudit.JobID = JobId;
                          AddALog(objAudit);
                      }
                      -------
                            if (Session["Cargoid"] != null)
                             {
                                 List<int> list = new List<int>();
                                 list = (List<int>)Session["Cargoid"];
                                 foreach (var item in list)
                                 {
                                     int cargoid = Convert.ToInt32(item.ToString());
                                     var a = (from t in entity.JCargoDescriptions where t.CargoDescriptionID == cargoid select t).FirstOrDefault();

                                     a.JobID = JobId;
                                     entity.SaveChanges();
                                 }
                             }

                             if (Session["ContainerID"] != null)
                             {
                                 List<int> list = new List<int>();
                                 list = (List<int>)Session["ContainerID"];
                                 foreach (var item in list)
                                 {

                                     int containerid = Convert.ToInt32(item.ToString());
                                     var a = (from t in entity.JContainerDetails where t.JContainerDetailID == containerid select t).FirstOrDefault();

                                     a.JobID = JobId;
                                     entity.SaveChanges();
                                 }
                             }

                             if (Session["AddLog"] != null)
                             {
                                 List<int> list = new List<int>();
                                 list = (List<int>)Session["AddLog"];
                                 foreach (var item in list)
                                 {
                                     int addlog = Convert.ToInt32(item.ToString());
                                     var a = (from t in entity.JAuditLogs where t.JAuditLogID == addlog select t).FirstOrDefault();

                                     a.JobID = JobId;
                                     entity.SaveChanges();
                                 }
                             }

                             if (Session["AddBill"] != null)
                             {
                                 List<int> list = new List<int>();
                                 list = (List<int>)Session["AddBill"];
                                 foreach (var item in list)
                                 {
                                     int addbill = Convert.ToInt32(item.ToString());
                                     var a = (from t in entity.JBIllOfEntries where t.BIllOfEntryID == addbill select t).FirstOrDefault();

                                     a.JobID = JobId;
                                     entity.SaveChanges();
                                 }
                             }

                             if (Session["InvoiceID"] != null)
                             {
                                 List<int> list = new List<int>();
                                 list = (List<int>)Session["InvoiceID"];
                                 foreach (var item in list)
                                 {
                                     int charges = Convert.ToInt32(item.ToString());
                                     var a = (from t in entity.JInvoices where t.InvoiceID == charges select t).FirstOrDefault();

                                     a.JobID = JobId;
                                     entity.SaveChanges();
                                 }
                             }*/

                    // int j = J.UpdateJobIDinAllModules(JobId, Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["fyearid"]));

                    Session["JobID"] = JobId;

                    JM.JobID = JobId;
                    return RedirectToAction("Job", "Job", new { ID = JobId });
                }
            }
            else if (Command == "Update")
            {
                if (Session["JobID"] != null)
                {
                    if (Convert.ToInt32(Session["JobID"]) > 0)
                    {
                        if (Session["UserID"] != null)
                        {
                            JM.JobID = Convert.ToInt32(Session["JobID"]);
                            int k = J.UpdateJob(JM);
                            if (k > 0)
                            {
                                JobId = JM.JobID;
                                var data = (from c in entity.JobGenerations where c.JobID == JobId select c).FirstOrDefault();
                                int acjid = 0;
                                if(data.AcJournalID != null)
                                {
                                    acjid = data.AcJournalID.Value;
                                }
                                int acprovjid = 0;
                                if (data.AcProvisionCostJournalID != null)
                                {
                                    acprovjid = data.AcProvisionCostJournalID.Value;
                                }

                                decimal shome = entity.JInvoices.Where(x => x.JobID == JobId).Sum(x => x.SalesHome).Value;
                                decimal phome = entity.JInvoices.Where(x => x.JobID == JobId).Sum(x => x.ProvisionHome).Value;

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

                                DeleteAndInsertRecords(formCollection, JobId);

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

        /*
        [HttpPost]
        public ActionResult Job(JobGeneration JM, string Command, int id)
        {
            JobGeneration Jobgeneration = new JobGeneration();
            // JM.JobTypeID = Jobgeneration.MainJobID;

            int i;
            int JobId = 0;

            BindAllMasters();

            if (Command == "Save")
            {

                if (Session["JobID"] != null)
                {
                    if (Convert.ToInt32(Session["JobID"]) > 0)
                    {
                        if (Session["UserID"] != null)
                        {
                            //rgm.EditUser(UR);
                            //int k = J.UpdateJob(JM);

                            //if (k > 0)
                            //{
                            //    return RedirectToAction("JobDetails", "Job", new { ID = "U" });
                            //}
                        }
                    }
                    else
                    {
                        if (Session["UserID"] != null)
                        {
                            JM.InvoiceDate = System.DateTime.UtcNow;
                            JM.InvoiceNo = 0;


                            i = J.AddJob(JM);


                            if (i > 0)
                            {
                                JobId = Convert.ToInt32(J.MaxJobID()) - 1;
                                if (Session["Cargoid"] != null)
                                {
                                    List<int> list = new List<int>();
                                    list = (List<int>)Session["Cargoid"];
                                    foreach (var item in list)
                                    {
                                        int cargoid = Convert.ToInt32(item.ToString());
                                        var a = (from t in entity.JCargoDescriptions where t.CargoDescriptionID == cargoid select t).FirstOrDefault();

                                        a.JobID = JobId;
                                        entity.SaveChanges();

                                    }

                                }

                                if (Session["ContainerID"] != null)
                                {
                                    List<int> list = new List<int>();
                                    list = (List<int>)Session["ContainerID"];
                                    foreach (var item in list)
                                    {

                                        int containerid = Convert.ToInt32(item.ToString());
                                        var a = (from t in entity.JContainerDetails where t.JContainerDetailID == containerid select t).FirstOrDefault();

                                        a.JobID = JobId;
                                        entity.SaveChanges();
                                    }
                                }





                                if (Session["AddLog"] != null)
                                {
                                    List<int> list = new List<int>();
                                    list = (List<int>)Session["AddLog"];
                                    foreach (var item in list)
                                    {
                                        int addlog = Convert.ToInt32(item.ToString());
                                        var a = (from t in entity.JAuditLogs where t.JAuditLogID == addlog select t).FirstOrDefault();

                                        a.JobID = JobId;
                                        entity.SaveChanges();
                                    }
                                }

                                if (Session["AddBill"] != null)
                                {
                                    List<int> list = new List<int>();
                                    list = (List<int>)Session["AddBill"];
                                    foreach (var item in list)
                                    {
                                        int addbill = Convert.ToInt32(item.ToString());
                                        var a = (from t in entity.JBIllOfEntries where t.BIllOfEntryID == addbill select t).FirstOrDefault();

                                        a.JobID = JobId;
                                        entity.SaveChanges();
                                    }
                                }

                                if (Session["InvoiceID"] != null)
                                {
                                    List<int> list = new List<int>();
                                    list = (List<int>)Session["InvoiceID"];
                                    foreach (var item in list)
                                    {
                                        int charges = Convert.ToInt32(item.ToString());
                                        var a = (from t in entity.JInvoices where t.InvoiceID == charges select t).FirstOrDefault();

                                        a.JobID = JobId;
                                        entity.SaveChanges();
                                    }
                                }


                               // int j = J.UpdateJobIDinAllModules(JobId, Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["fyearid"]));

                                Session["JobID"] = JobId;

                                JM.JobID = JobId;
                            }
                        }
                    }
                }
            }
            else if (Command == "Update")
            {
                if (Session["JobID"] != null)
                {
                    if (Convert.ToInt32(Session["JobID"]) > 0)
                    {
                        if (Session["UserID"] != null)
                        {
                            JM.JobID = Convert.ToInt32(Session["JobID"]);
                            //rgm.EditUser(UR);
                            int k = J.UpdateJob(JM);

                            if (k > 0)
                            {
                                JobId = JM.JobID;

                                var data = (from c in entity.JobGenerations where c.JobID == JobId select c).FirstOrDefault();
                                int acjid = data.AcJournalID.Value;
                                int acprovjid = data.AcProvisionCostJournalID.Value;

                                decimal shome = entity.JInvoices.Where(x => x.JobID == JobId).Sum(x => x.SalesHome).Value;
                                decimal phome = entity.JInvoices.Where(x => x.JobID == JobId).Sum(x => x.ProvisionHome).Value;

                                int custcontrolacid = (from c in entity.AcHeadAssigns select c.CustomerControlAcID.Value).FirstOrDefault();
                                int freightacheadid = 158;
                                int provcontrolacid = (from c in entity.AcHeadAssigns select c.ProvisionCostControlAcID.Value).FirstOrDefault();
                                int accruedcontrolacid = (from c in entity.AcHeadAssigns select c.AccruedCostControlAcID.Value).FirstOrDefault();

                                int acjdetail1=(from x in entity.AcJournalDetails where x.AcJournalID==acjid && x.AcHeadID==custcontrolacid select x.AcJournalDetailID).FirstOrDefault();

                                var data1 = (from x in entity.AcJournalDetails where x.AcJournalDetailID == acjdetail1 select x).FirstOrDefault();
                                data1.Amount = shome;
                                entity.Entry(data1).State = EntityState.Modified;
                                entity.SaveChanges();

                                 acjdetail1 = (from x in entity.AcJournalDetails where x.AcJournalID == acjid && x.AcHeadID == freightacheadid select x.AcJournalDetailID).FirstOrDefault();
                                 data1 = (from x in entity.AcJournalDetails where x.AcJournalDetailID == acjdetail1 select x).FirstOrDefault();
                                data1.Amount = -shome;
                                entity.Entry(data1).State = EntityState.Modified;
                                entity.SaveChanges();


                                acjdetail1 = (from x in entity.AcJournalDetails where x.AcJournalID == acprovjid && x.AcHeadID == provcontrolacid select x.AcJournalDetailID).FirstOrDefault();
                                data1 = (from x in entity.AcJournalDetails where x.AcJournalDetailID == acjdetail1 select x).FirstOrDefault();
                                data1.Amount = phome;
                                entity.Entry(data1).State = EntityState.Modified;
                                entity.SaveChanges();


                                acjdetail1 = (from x in entity.AcJournalDetails where x.AcJournalID == acprovjid && x.AcHeadID == accruedcontrolacid select x.AcJournalDetailID).FirstOrDefault();
                                data1 = (from x in entity.AcJournalDetails where x.AcJournalDetailID == acjdetail1 select x).FirstOrDefault();
                                data1.Amount = -phome;
                                entity.Entry(data1).State = EntityState.Modified;
                                entity.SaveChanges();
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
                            int j = J.UpdateJobIDinAllModules(JobId, Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["fyearid"]));
                        }
                        return RedirectToAction("JobDetails", "Job", new { ID = 20 });
                    }
                }
            }
               
            else if (Command == "SaveInvoice")
            {
                if (Session["JobID"] != null)
                {
                    if (Convert.ToInt32(Session["JobID"]) > 0)
                    {
                        if (Session["UserID"] != null)
                        {
                            //rgm.EditUser(UR);
                            int k = J.UpdateInvoiceNumber(Convert.ToInt32(Session["JobID"]), Convert.ToInt32(JM.InvoiceNo), Convert.ToDateTime(JM.InvoiceDate),Convert.ToInt32(Session["fyearid"].ToString()));

                            if (k > 0)
                            {
                                return RedirectToAction("JobDetails", "Job", new { ID = Session["JobID"].ToString() });
                            }
                        }
                        else
                        {
                            return RedirectToAction("Login", "Login");
                        }
                    }
                    else
                    {
                        if (Session["UserID"] != null)
                        {
                            i = J.AddJob(JM);

                            if (i > 0)
                            {
                                JobId = Convert.ToInt32(J.MaxJobID()) - 1;
                                //int j = J.UpdateJobIDinAllModules(JobId, Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserID"]));

                            }
                        }
                        else
                        {
                            return RedirectToAction("Login", "Login");
                        }
                    }
                }


            }

            return View(JM);

        }
            */

        private bool DeleteAndInsertRecords(FormCollection formCollection, int JobId)
        {
            if (JobId <= 0)
            {
                return false;
            }
            int i = 0;
            int ChargesCount = 0;
            ArrayList ChargesArray = new ArrayList();

            DeleteJobDetailsByJobID(JobId);

            for (int j = 0; j < formCollection.Keys.Count; j++)
            {
                if (formCollection.Keys[j].StartsWith("RevenueTypeID_"))
                {
                    ChargesCount = ChargesCount + 1;
                    ChargesArray.Add(formCollection.Keys[j].Replace("RevenueTypeID_", "").Trim());
                }
            }

            for (int c = 0; c < ChargesCount; c++)
            {
                string[] strArray;
                JInvoice Charges = new JInvoice();
                Charges.UserID = Convert.ToInt32(Session["UserID"].ToString());
                Charges.JobID = JobId;
                
                int RevenueTypeID = 0;
                if (formCollection.GetValue("RevenueTypeID_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("RevenueTypeID_" + ChargesArray[c]).RawValue;
                    int.TryParse(strArray[0], out RevenueTypeID);
                }
                Charges.RevenueTypeID = RevenueTypeID;
                int SupplierId = 0;
                if (formCollection.GetValue("SupplierID_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("SupplierID_" + ChargesArray[c]).RawValue;
                    int.TryParse(strArray[0], out SupplierId);
                }
                Charges.SupplierID = SupplierId;
                if (formCollection.GetValue("ChargeDescription_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("ChargeDescription_" + ChargesArray[c]).RawValue;
                    Charges.Description = strArray[0].Trim();
                }
                double Qty = 0;
                if (formCollection.GetValue("Quantity_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("Quantity_" + ChargesArray[c]).RawValue;
                    double.TryParse(strArray[0], out Qty);
                }
                Charges.Quantity = Qty;
                int UnitItemId = 0;
                if (formCollection.GetValue("ItemUnitID_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("ItemUnitID_" + ChargesArray[c]).RawValue;
                    int.TryParse(strArray[0], out UnitItemId);
                }
                Charges.UnitID = UnitItemId;
                decimal ProvRate = 0;
                if (formCollection.GetValue("ProvisionRate_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("ProvisionRate_" + ChargesArray[c]).RawValue;
                    decimal.TryParse(strArray[0], out ProvRate);
                }
                Charges.ProvisionRate = ProvRate;
                int ProvCurrency = 0;
                if (formCollection.GetValue("ProvisionCurrencyId_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("ProvisionCurrencyId_" + ChargesArray[c]).RawValue;
                    int.TryParse(strArray[0], out ProvCurrency);
                }
                Charges.ProvisionCurrencyID = ProvCurrency;
                decimal ProvExRate = 0;
                if (formCollection.GetValue("ProvisionExchangeRate_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("ProvisionExchangeRate_" + ChargesArray[c]).RawValue;
                    decimal.TryParse(strArray[0].Trim(), out ProvExRate);
                }
                Charges.ProvisionExchangeRate = ProvExRate;

                decimal ProvHome = 0;
                if (formCollection.GetValue("ProvisionHome_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("ProvisionHome_" + ChargesArray[c]).RawValue;
                    decimal.TryParse(strArray[0].Trim(), out ProvHome);
                }
                Charges.ProvisionHome = ProvHome;

                decimal ProvForiegn = 0;
                if (formCollection.GetValue("ProvisionForeign_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("ProvisionForeign_" + ChargesArray[c]).RawValue;
                    decimal.TryParse(strArray[0].Trim(), out ProvForiegn);
                }
                Charges.ProvisionForeign = ProvForiegn;

                decimal SalesRate = 0;
                if (formCollection.GetValue("SalesRate_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("SalesRate_" + ChargesArray[c]).RawValue;
                    decimal.TryParse(strArray[0].Trim(), out SalesRate);
                }
                Charges.SalesRate = ProvForiegn;
                int SalesCurrencyId = 0;
                if (formCollection.GetValue("SalesCurrencyId_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("SalesCurrencyId_" + ChargesArray[c]).RawValue;
                    int.TryParse(strArray[0].Trim(), out SalesCurrencyId);
                }
                Charges.SalesCurrencyID = SalesCurrencyId;
                decimal SalesExRate = 0;
                if (formCollection.GetValue("SalesExchangeRate_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("SalesExchangeRate_" + ChargesArray[c]).RawValue;
                    decimal.TryParse(strArray[0].Trim(), out SalesExRate);
                }
                Charges.SalesExchangeRate = SalesExRate;

                decimal SalesHome = 0;
                if (formCollection.GetValue("SalesHome_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("SalesHome_" + ChargesArray[c]).RawValue;
                    decimal.TryParse(strArray[0].Trim(), out SalesHome);
                }
                Charges.SalesHome = SalesHome;

                decimal SalesForeign = 0;
                if (formCollection.GetValue("SalesForeign_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("SalesHome_" + ChargesArray[c]).RawValue;
                    decimal.TryParse(strArray[0].Trim(), out SalesForeign);
                }
                Charges.SalesForeign = SalesForeign;
                decimal Cost = 0;
                if (formCollection.GetValue("Cost_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("SalesHome_" + ChargesArray[c]).RawValue;
                    decimal.TryParse(strArray[0].Trim(), out Cost);
                }
                Charges.Cost = Cost;
                decimal Tax = 0;
                if (formCollection.GetValue("tax_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("tax_" + ChargesArray[c]).RawValue;
                    decimal.TryParse(strArray[0].Trim(), out Tax);
                }
                Charges.Tax = Tax;
                decimal TaxAmount = 0;
                if (formCollection.GetValue("taxamt_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("taxamt_" + ChargesArray[c]).RawValue;
                    decimal.TryParse(strArray[0].Trim(), out TaxAmount);
                }
                Charges.TaxAmount = TaxAmount;
                decimal Margin = 0;
                if (formCollection.GetValue("margin_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("margin_" + ChargesArray[c]).RawValue;
                    decimal.TryParse(strArray[0].Trim(), out Margin);
                }
                Charges.Margin = Margin;

                int iCharge = J.AddCharges(Charges, Session["UserID"].ToString());
            }
            int CargoCount = 0;
            ArrayList CargoArray = new ArrayList();
            for (int j = 0; j < formCollection.Keys.Count; j++)
            {
                if (formCollection.Keys[j].StartsWith("Mark_"))
                {
                    CargoCount = CargoCount + 1;
                    CargoArray.Add(formCollection.Keys[j].Replace("Mark_", "").Trim());
                }
            }
            for (int c = 0; c < CargoCount; c++)
            {
                string[] strArray;
                JCargoDescription Cargo = new JCargoDescription();
                Cargo.UserID = Session["UserID"].ToString();
                Cargo.JobID = JobId;
               // string MarkId = "";
                if (formCollection.GetValue("Mark_" + CargoArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("Mark_" + CargoArray[c]).RawValue;
                    Cargo.Mark = strArray[0];
                }
               // string Description = "";
                if (formCollection.GetValue("CarDescription_" + CargoArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("CarDescription_" + CargoArray[c]).RawValue;
                    Cargo.Description = strArray[0];
                }
               // Cargo.Description = Description;
                decimal Weight = 0;
                if (formCollection.GetValue("weight_" + CargoArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("weight_" + CargoArray[c]).RawValue;
                    decimal.TryParse(strArray[0].Trim(), out Weight);
                }
                Cargo.weight = Weight;
                decimal Volume = 0;
                if (formCollection.GetValue("volume_" + CargoArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("volume_" + CargoArray[c]).RawValue;
                    decimal.TryParse(strArray[0].Trim(), out Volume);
                }
                Cargo.volume = Volume;

                decimal Packages = 0;
                if (formCollection.GetValue("Packages_" + CargoArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("Packages_" + CargoArray[c]).RawValue;
                    decimal.TryParse(strArray[0].Trim(), out Packages);
                }
                Cargo.Packages = Packages;

                decimal GrossWeight = 0;
                if (formCollection.GetValue("GrossWeight_" + CargoArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("GrossWeight_" + CargoArray[c]).RawValue;
                    decimal.TryParse(strArray[0].Trim(), out GrossWeight);
                }
                Cargo.GrossWeight = GrossWeight;
                i = J.AddCargo(Cargo, Session["UserID"].ToString());
            }

            int ContainerCount = 0;
            ArrayList ContainerArray = new ArrayList();
            for (int j = 0; j < formCollection.Keys.Count; j++)
            {
                if (formCollection.Keys[j].StartsWith("ContainerType_"))
                {
                    ContainerCount = ContainerCount + 1;
                    ContainerArray.Add(formCollection.Keys[j].Replace("ContainerType_", "").Trim());
                }
            }
            for (int c = 0; c < ContainerCount; c++)
            {
                string[] strArray;
                JContainerDetail ContainerObj = new JContainerDetail();
                ContainerObj.JobID = JobId;
                int ContainerTypeId = 0;
                if (formCollection.GetValue("ContainerTypeID_" + ContainerArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("ContainerTypeID_" + ContainerArray[c]).RawValue;
                    int.TryParse(strArray[0].Trim(), out ContainerTypeId);
                }
                ContainerObj.ContainerTypeID = ContainerTypeId;
                string ContainerNo = "";
                if (formCollection.GetValue("ContainerNo_" + ContainerArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("ContainerNo_" + ContainerArray[c]).RawValue;
                    ContainerNo = strArray[0].Trim();
                }
                ContainerObj.ContainerNo = ContainerNo;
                string SealNo = "";
                if (formCollection.GetValue("SealNo_" + ContainerArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("SealNo_" + ContainerArray[c]).RawValue;
                    SealNo = strArray[0].Trim();
                }
                ContainerObj.SealNo = SealNo;
                string ContainerDescription = "";
                if (formCollection.GetValue("ContainerDescription_" + ContainerArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("ContainerDescription_" + ContainerArray[c]).RawValue;
                    ContainerDescription = strArray[0].Trim();
                }
                ContainerObj.Description = ContainerDescription;
                AddContainerDetails(ContainerObj);
            }

            int BillOfEntryCount = 0;
            ArrayList BillOfEntryArray = new ArrayList();
            for (int j = 0; j < formCollection.Keys.Count; j++)
            {
                if (formCollection.Keys[j].StartsWith("BIllOfEntry_"))
                {
                    BillOfEntryCount = BillOfEntryCount + 1;
                    BillOfEntryArray.Add(formCollection.Keys[j].Replace("BIllOfEntry_", "").Trim());
                }
            }
            for (int c = 0; c < BillOfEntryCount; c++)
            {
                string[] strArray;
                JBIllOfEntry objBillOfEntry = new JBIllOfEntry();
                objBillOfEntry.JobID = JobId;
                string BIllOfEntry = "";
                if (formCollection.GetValue("BIllOfEntry_" + BillOfEntryArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("BIllOfEntry_" + BillOfEntryArray[c]).RawValue;
                    BIllOfEntry = strArray[0].Trim();
                }
                objBillOfEntry.BIllOfEntry = BIllOfEntry;
                DateTime BIllOfEntryDate;
                if (formCollection.GetValue("BillofEntryDate_" + BillOfEntryArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("BillofEntryDate_" + BillOfEntryArray[c]).RawValue;
                    DateTime.TryParseExact(strArray[0].Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out BIllOfEntryDate);
                    objBillOfEntry.BillofEntryDate = BIllOfEntryDate;
                }
                int ShippingAgentId = 0;
                if (formCollection.GetValue("ShippingAgentID_" + BillOfEntryArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("ShippingAgentID_" + BillOfEntryArray[c]).RawValue;
                    int.TryParse(strArray[0].Trim(), out ShippingAgentId);
                }
                objBillOfEntry.ShippingAgentID = ShippingAgentId;
                AddBill(objBillOfEntry);
            }
            int NotificationCount = 0;
            ArrayList NotificationArray = new ArrayList();
            for (int j = 0; j < formCollection.Keys.Count; j++)
            {
                if (formCollection.Keys[j].StartsWith("AuditTransDate_"))
                {
                    NotificationCount = NotificationCount + 1;
                    NotificationArray.Add(formCollection.Keys[j].Replace("AuditTransDate_", "").Trim());
                }
            }
            for (int c = 0; c < NotificationCount; c++)
            {
                JAuditLog objAudit = new JAuditLog();
                objAudit.JobID = JobId;
                string[] strArray;
                DateTime NotificationDate;
                if (formCollection.GetValue("AuditTransDate_" + NotificationArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("AuditTransDate_" + NotificationArray[c]).RawValue;
                    DateTime.TryParseExact(strArray[0].Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out NotificationDate);
                    objAudit.TransDate = NotificationDate;
                }

                if (formCollection.GetValue("AuditRemarks_" + NotificationArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("AuditRemarks_" + NotificationArray[c]).RawValue;
                    objAudit.Remarks = strArray[0].Trim();
                }
                objAudit.JobID = JobId;
                AddALog(objAudit);

            }
            return true;
        }

        [HttpGet]
        public ActionResult DeleteJob(int id)
        {
            // int k = 0;
            if (id != 0)
            {
                J.DeleteJobDetails(id);
            }

            return RedirectToAction("JobDetails", "Job", new { ID = 10 });

        }

        public string AddCargoDescription(JCargoDescription Cargo)
        {
            int i = 0;

            if (Session["UserID"] != null)
            {
                //  Cargo.JobID = Convert.ToInt32(Session["JobID"]);

                if (Session["JobID"] != null)
                {
                    if (Convert.ToInt32(Session["JobID"]) > 0)
                    {
                        Cargo.JobID = Convert.ToInt32(Session["JobID"]);
                    }
                    else
                    {
                        Cargo.JobID = 0;
                    }
                }
                else
                {
                    Cargo.JobID = 0;
                }


                i = J.AddCargo(Cargo, Session["UserID"].ToString());
                var cargoid = (from t in entity.JCargoDescriptions orderby t.CargoDescriptionID descending select t).FirstOrDefault();

                if (Session["Cargoid"] == null)
                {
                    List<int> id = new List<int>();
                    id.Add(cargoid.CargoDescriptionID);
                    Session["Cargoid"] = id;
                }
                else
                {
                    List<int> id = (List<int>)Session["Cargoid"];
                    id.Add(cargoid.CargoDescriptionID);
                    Session["Cargoid"] = id;
                }

            }

            return i.ToString();
        }

        public string AddContainerDet(JContainerDetail Conta)
        {
            int i = 0;
            if (Session["UserID"] != null)
            {
                if (Session["JobID"] != null)
                {
                    if (Convert.ToInt32(Session["JobID"]) > 0)
                    {
                        Conta.JobID = Convert.ToInt32(Session["JobID"]);
                    }
                    else
                    {
                        Conta.JobID = 0;
                    }
                }
                else
                {
                    Conta.JobID = 0;
                }



                i = J.AddContainerDetails(Conta, Session["UserID"].ToString());
                var container = (from t in entity.JContainerDetails orderby t.JContainerDetailID descending select t).FirstOrDefault();

                if (Session["ContainerID"] == null)
                {
                    List<int> id = new List<int>();
                    id.Add(container.JContainerDetailID);
                    Session["ContainerID"] = id;
                }
                else
                {
                    List<int> id = (List<int>)Session["ContainerID"];
                    id.Add(container.JContainerDetailID);
                    Session["ContainerID"] = id;
                }


            }
            return i.ToString();
        }

        public string AddALog(JAuditLog Audit)
        {
            int i = 0;
            if (Session["UserID"] != null)
            {
                i = J.AddAuditLog(Audit, Session["UserID"].ToString());
                var AddLog = (from t in entity.JAuditLogs orderby t.JAuditLogID descending select t).FirstOrDefault();
            }

            return i.ToString();
        }

        public string AddBill(JBIllOfEntry Bill)
        {
            int i = 0;
            if (Session["UserID"] != null)
            {
              
                i = J.AddBillOfEntry(Bill, Session["UserID"].ToString());
                var billEntry = (from t in entity.JBIllOfEntries orderby t.BIllOfEntryID descending select t).FirstOrDefault();

                if (Session["AddBill"] == null)
                {
                    List<int> id = new List<int>();
                    id.Add(billEntry.BIllOfEntryID);
                    Session["AddBill"] = id;
                }
                else
                {
                    List<int> id = (List<int>)Session["AddBill"];
                    id.Add(billEntry.BIllOfEntryID);
                    Session["AddBill"] = id;
                }
            }
            return i.ToString();
        }

        public string AddChargesDetails(JInvoice Charges)
        {
            JInvoice jInvoice = new JInvoice();

            if (Charges.JobID > 0)
            {
                Charges.UserID = Convert.ToInt32(Session["UserID"].ToString());
                Charges.CostUpdationStatus = "1";
                entity.Entry(Charges).State = EntityState.Modified;
                entity.SaveChanges();
            }
            if (Session["UserID"] != null)
            {
                if (Session["JobID"] != null)
                {
                    if (Convert.ToInt32(Session["JobID"]) > 0)
                    {
                        Charges.UserID = Convert.ToInt32(Session["UserID"].ToString());
                        Charges.JobID = Convert.ToInt32(Session["JobID"]);

                    }
                    else
                    {
                        Charges.UserID = Convert.ToInt32(Session["UserID"].ToString());
                        Charges.JobID = 0;
                    }
                }
                else
                {
                    Charges.UserID = Convert.ToInt32(Session["UserID"].ToString());
                    Charges.JobID = 0;
                }

                if (Charges.InvoiceID <= 0)
                {

                    int i = J.AddCharges(Charges, Session["UserID"].ToString());
                }
                else
                {
                    entity.Entry(Charges).State = EntityState.Modified;
                    entity.SaveChanges();
                }

                var charges = (from t in entity.JInvoices orderby t.InvoiceID descending select t).FirstOrDefault();

                if (Session["InvoiceID"] == null)
                {
                    List<int> id = new List<int>();

                    id.Add(charges.InvoiceID);
                    Session["InvoiceID"] = id;
                }
                else
                {
                    List<int> id = (List<int>)Session["InvoiceID"];
                    if (id.Contains(charges.InvoiceID) == false)
                    {
                        id.Add(charges.InvoiceID);
                        Session["InvoiceID"] = id;
                    }
                }

            }

            return "Success";
        }

        public string AddContainerDetails(JContainerDetail ContainerDetail)
        {
            JContainerDetail jJContainerDetail = new JContainerDetail();

            //if (ContainerDetail.JobID > 0)
            //{
            //    ContainerDetail.UserID = Convert.ToInt32(Session["UserID"].ToString());
            //    entity.Entry(ContainerDetail).State = EntityState.Modified;
            //    entity.SaveChanges();
            //}
            if (Session["UserID"] != null)
            {
                if (ContainerDetail.JContainerDetailID <= 0)
                {
                    int i = J.AddContainerDetails(ContainerDetail, Session["UserID"].ToString());
                }
                else
                {
                    entity.Entry(ContainerDetail).State = EntityState.Modified;
                    entity.SaveChanges();
                }
                var containers = (from t in entity.JContainerDetails orderby t.JContainerDetailID descending select t).FirstOrDefault();
            }

            return "Success";
        }


        public JsonResult GetChargesByJobIdandUserID()
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            // var AllInvoices;
            int JobID;
            if (Session["UserID"] != null)
            {
                if (Session["JobID"] != null)
                {
                    JobID = Convert.ToInt32(Session["JobID"]);
                }
                else
                {
                    JobID = 0;
                }

                //  var AllInvoices = J.GetContainerByJob(JobID, Convert.ToInt32(Session["UserID"]));
                var AllInvoices = J.GetChargesByJob(JobID, Convert.ToInt32(Session["UserID"]));
                return Json(AllInvoices, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Failed = "failed";

                return Json(Failed, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GETLOCALCostForDashboard()
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            // var AllInvoices;
            int JobID;
            if (Session["UserID"] != null)
            {
                if (Session["JobID"] != null)
                {
                    JobID = Convert.ToInt32(Session["JobID"]);
                }
                else
                {
                    JobID = 0;
                }


                var JobDetails = (from t in entity.GETLOCALCostForDashboard(Convert.ToInt32(Session["fyearid"].ToString()))
                                  select t).ToList();
                return Json(JobDetails, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Failed = "failed";

                return Json(Failed, JsonRequestBehavior.AllowGet);
            }


        }



        public JsonResult GetAuditByJobIdandUserID()
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            // var AllInvoices;
            int JobID;
            if (Session["UserID"] != null)
            {
                if (Session["JobID"] != null)
                {
                    JobID = Convert.ToInt32(Session["JobID"]);
                }
                else
                {
                    JobID = 0;
                }

                var AllAudits = J.GetAuditByJob(JobID, Convert.ToInt32(Session["UserID"]));

                return Json(AllAudits, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Failed = "failed";

                return Json(Failed, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult GetBillByJobIdandUserID()
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            // var AllInvoices;
            int JobID;
            if (Session["UserID"] != null)
            {
                if (Session["JobID"] != null)
                {
                    JobID = Convert.ToInt32(Session["JobID"]);
                }
                else
                {
                    JobID = 0;
                }


                var AllBill = J.GetBillByJob(JobID, Convert.ToInt32(Session["UserID"]));
                return Json(AllBill, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Failed = "failed";

                return Json(Failed, JsonRequestBehavior.AllowGet);
            }


        }

        public bool DeleteJobDetailsByJobID(int JobID)
        {
            J.DeleteJobDetailsByJobID(JobID);
            return true;
        }

        public JsonResult GetCargoByJobIdandUserID()
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            // var AllInvoices;
            int JobID;
            if (Session["UserID"] != null)
            {
                if (Session["JobID"] != null)
                {
                    JobID = Convert.ToInt32(Session["JobID"]);
                }
                else
                {
                    JobID = 0;
                }

                var AllCargo = J.GetCargoByJob(JobID, Convert.ToInt32(Session["UserID"]));
                return Json(AllCargo, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Failed = "failed";

                return Json(Failed, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult GetContainerByJobIdandUserID()
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            // var AllInvoices;
            int JobID;
            if (Session["UserID"] != null)
            {
                if (Session["JobID"] != null)
                {
                    JobID = Convert.ToInt32(Session["JobID"]);
                }
                else
                {
                    JobID = 0;
                }

                var AllConta = J.GetContainerByJob(JobID, Convert.ToInt32(Session["UserID"]));
                return Json(AllConta, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Failed = "failed";

                return Json(Failed, JsonRequestBehavior.AllowGet);
            }


        }



        public JsonResult DeleteContainerJobIdandUserID(JContainerDetail Conta)
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            // var AllInvoices;

            if (Session["UserID"] != null)
            {
                var AllConta = J.DeleteContainers(Convert.ToInt32(Conta.JContainerDetailID));
                return Json(AllConta, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Failed = "failed";

                return Json(Failed, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult DeleteCargoDescriptionByID(JCargoDescription Cargo)
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            // var AllInvoices;

            if (Session["UserID"] != null)
            {
                var i = J.DeleteCargo(Convert.ToInt32(Cargo.CargoDescriptionID));
                return Json(i, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Failed = "failed";

                return Json(Failed, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult DeleteBillsByID(JBIllOfEntry Bill)
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            // var AllInvoices;

            if (Session["UserID"] != null)
            {
                var i = J.DeleteBill(Convert.ToInt32(Bill.BIllOfEntryID));
                return Json(i, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Failed = "failed";

                return Json(Failed, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult DeleteAuditLogsByID(JAuditLog Log)
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            // var AllInvoices;

            if (Session["UserID"] != null)
            {
                var i = J.DeleteAudit(Convert.ToInt32(Log.JAuditLogID));
                return Json(i, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Failed = "failed";

                return Json(Failed, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult DeleteInvoicesByID(JInvoice Inv)
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            // var AllInvoices;

            if (Session["UserID"] != null)
            {
                var i = J.DeleteInvoice(Convert.ToInt32(Inv.InvoiceID));
                return Json(i, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Failed = "failed";

                return Json(Failed, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult EditInvoicesByID(JInvoice Inv)
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            // var AllInvoices;

            if (Session["UserID"] != null)
            {
                var i = J.DeleteInvoice(Convert.ToInt32(Inv.InvoiceID));
                return Json(i, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Failed = "failed";

                return Json(Failed, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        public List<SP_GetAllContainerTypes_Result> GetContainers()
        {
            return MM.GetAllContainerTypes().ToList();
        }

        public void BindAllMasters()
        {
            try
            {
                List<SP_GetAllPorts_Result> Ports = new List<SP_GetAllPorts_Result>();
                List<SP_GetAllVessels_Result> Vessels = new List<SP_GetAllVessels_Result>();
                List<SP_GetAllTransporters_Result> Transporters = new List<SP_GetAllTransporters_Result>();
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

                Ports = MM.GetAllPort();
                Vessels = MM.GetAllVessels();
                Transporters = MM.GetAllTransporters();
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

                ViewBag.Ports = new SelectList(Ports, "PortID", "Port");
                ViewBag.Customer = new SelectList(Customers, "CustomerID", "Customer");
                ViewBag.Employees = new SelectList(Employees, "EmployeeID", "EmployeeName");
                ViewBag.Vessels = new SelectList(Vessels, "VesselID", "Vessel");
                ViewBag.Countries = new SelectList(Countries, "CountryID", "CountryName");
                ViewBag.JobType = new SelectList(JobType, "JobTypeID", "JobDescription");
                ViewBag.Carriers = new SelectList(Carriers, "CarrierID", "Carrier");
                ViewBag.Transporters = new SelectList(Transporters, "TransPorterID", "TransPorter");
                ViewBag.ContainerTypes = new SelectList(ContainerTypes, "ContainerTypeID", "ContainerType");
                ViewBag.Unit = new SelectList(Unit, "ItemUnitID", "ItemUnit");
                // ViewBag.RevenueT = new SelectList(RevenueType, "RevenueTypeID", "RevenueType");
                ViewBag.Curency = new SelectList(Currency, "CurrencyID", "CurrencyName");
                ViewBag.Suplier = new SelectList(Supplier, "SupplierID", "SupplierName");
                ViewBag.ShippingA = new SelectList(ShippingAgent.OrderBy(x => x.AgentName).ToList(), "ShippingAgentID", "AgentName");
                ViewBag.MaxInvoiceNumber = J.GetMaxInvoiceNumber();
                ViewBag.voyages = (from c in entity.Voyages select c).ToList();
               
                List< SelectListItem > objBLStatusList = new List<SelectListItem>
                    { new SelectListItem() { Text = "SURRENDERED", Selected = false, Value = "SURRENDERED"}
                     , new SelectListItem() { Text = "WAY BILL", Selected = false, Value = "WAY BILL"}
                     , new SelectListItem() { Text = "OBL REQUIRED", Selected = false, Value = "OBL REQUIRED"}};

         
                ViewBag.BLStatusList = new SelectList(objBLStatusList,"Value","Text");

                var query = from t in entity.JobGenerations
                            where t.MainJobID == null

                            select t;

                ViewBag.MainJobId = query;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonResult GetJobPrefix(string ID)
        {


            string vPrefix = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();

            var dta = J.JobPrefixByJobTypeID(Convert.ToInt32(ID)) + vPrefix + J.MaxJobID();

            return Json(dta, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetSupplierOfRevID(string ID)
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();

            var Suppliers = J.GetSuppliersByRevenueTypeID((ID));

            return Json(Suppliers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRevenueTypeList()
        {
            var RevenueType = MM.GetRevenueType();

            return Json(RevenueType, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUnitList()
        {
            var ItemUnit = MM.GetItemUnit();

            return Json(ItemUnit, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContainerTypeList()
        {
            var ContainerType = MM.GetAllContainerTypes();

            return Json(ContainerType, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCurrencyList()
        {
            var vCurrencylist = MM.GetCurrency();

            return Json(vCurrencylist, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetExchangeRte(string ID)
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();

            var ExRate = J.GetCurrencyExchange(Convert.ToInt32(ID));

            return Json(ExRate, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetJobCodeByMainJobCode(int mainjobid)
        {
            var query = (from t in entity.JobGenerations
                         where t.MainJobID == mainjobid
                         group t by t.JobCode into a
                         select new { JobCode = a.Key, mainjob = a.Sum(x => x.MainJobID) }).ToList();



            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClosedJob()
        {

            DateTime d = DateTime.Now;
            DateTime fyear = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime mstart = new DateTime(fyear.Year, d.Month, 01);

            int maxday = DateTime.DaysInMonth(fyear.Year, d.Month);
            DateTime mend = new DateTime(fyear.Year, d.Month, maxday);
            List<CloseJobVm> lst = new List<CloseJobVm>();
            var data = entity.SP_GetAllJobsDetailsForClosedJob().Where(x => x.JobDate >= mstart && x.JobDate <= mend).OrderByDescending(x => x.JobDate).ToList();

            foreach (var item in data)
            {
                CloseJobVm closevm = new CloseJobVm();
                closevm.Consignee = item.Consignee;
                closevm.Customer = item.Customer;
                closevm.InvoiceDate = item.InvoiceDate;
                closevm.InvoiceNo = item.InvoiceNo.Value;
                closevm.JobCode = item.JobCode;
                closevm.JobDate = item.JobDate.Value;
                closevm.JobDescription = item.JobDescription;
                closevm.Shipper = item.Shipper;
                closevm.JobID = item.JobID;

                lst.Add(closevm);
            }
            return View(lst);
        }
        [HttpPost]
        public ActionResult ClosedJob(List<TrueBooksMVC.Models.CloseJobVm> lst)
        {

            var selectedrecords = lst.Where(item => item.IsSelected == true && item.InvoiceNo > 0).ToList();
            foreach (var item in selectedrecords)
            {
                JobGeneration a = (from c in entity.JobGenerations where c.JobID == item.JobID select c).FirstOrDefault();
                a.IsClosed = true;

                entity.Entry(a).State = EntityState.Modified;
                entity.SaveChanges();
            }
            return RedirectToAction("ClosedJob");
        }
        public ActionResult ClodedJobShow()
        {
            DateTime d = DateTime.Now;
            DateTime fyear = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime mstart = new DateTime(fyear.Year, d.Month, 01);

            int maxday = DateTime.DaysInMonth(fyear.Year, d.Month);
            DateTime mend = new DateTime(fyear.Year, d.Month, maxday);
            List<CloseJobVm> lst = new List<CloseJobVm>();
            var data = entity.SP_GetAllClosedJobsDetails().Where(x => x.JobDate >= mstart && x.JobDate <= mend).OrderByDescending(x => x.JobDate).ToList();

            foreach (var item in data)
            {
                CloseJobVm closevm = new CloseJobVm();
                closevm.Consignee = item.Consignee;
                closevm.Customer = item.Customer;
                closevm.InvoiceDate = item.InvoiceDate.Value;
                closevm.InvoiceNo = item.InvoiceNo.Value;
                closevm.JobCode = item.JobCode;
                closevm.JobDate = item.JobDate.Value;
                closevm.JobDescription = item.JobDescription;
                closevm.Shipper = item.Shipper;
                closevm.JobID = item.JobID;

                lst.Add(closevm);
            }
            return View(lst);

        }



        public JsonResult GetClosedJob()
        {
            List<CloseJobVm> lst = new List<CloseJobVm>();
            var data = entity.SP_GetAllClosedJobsDetails().ToList();

            foreach (var item in data)
            {
                CloseJobVm closevm = new CloseJobVm();
                closevm.Consignee = item.Consignee;
                closevm.Customer = item.Customer;
                closevm.InvoiceDate = item.InvoiceDate;
                closevm.InvoiceNo = item.InvoiceNo.Value;
                closevm.JobCode = item.JobCode;
                closevm.JobDate = item.JobDate.Value;
                closevm.JobDescription = item.JobDescription;
                closevm.Shipper = item.Shipper;
                closevm.JobID = item.JobID;

                lst.Add(closevm);
            }
            string view = this.RenderPartialView("ucClosedJob", lst);

            return new JsonResult
            {
                Data = new
                {
                    success = true,
                    closedjoblist = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }


        //public JsonResult GetClosedJobbyDate(string fdate, string tdate)
        //{
        //    List<CloseJobVm> lst = new List<CloseJobVm>();
        //    var data = entity.SP_GetAllClosedJobsDetailsByDate(fdate, tdate, Convert.ToInt32(Session["fyearid"])).ToList();

        //    foreach (var item in data)
        //    {
        //        CloseJobVm closevm = new CloseJobVm();
        //        closevm.Consignee = item.Consignee;
        //        closevm.Customer = item.Customer;
        //        closevm.InvoiceDate = item.InvoiceDate;
        //        closevm.InvoiceNo = item.InvoiceNo.Value;
        //        closevm.JobCode = item.JobCode;
        //        closevm.JobDate = item.JobDate.Value;
        //        closevm.JobDescription = item.JobDescription;
        //        closevm.Shipper = item.Shipper;
        //        closevm.JobID = item.JobID;

        //        lst.Add(closevm);
        //    }
        //    string view = this.RenderPartialView("ucClosedJob", lst);

        //    return new JsonResult
        //    {
        //        Data = new
        //        {
        //            success = true,
        //            closedjoblist = view
        //        },
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    };

        //}

        public JsonResult GetOpenJobbyDate(DateTime fdate, DateTime tdate)
        {
            List<CloseJobVm> lst = new List<CloseJobVm>();
            var data = entity.SP_GetAllJobsDetailsByDate(fdate, tdate).ToList();

            foreach (var item in data)
            {
                CloseJobVm closevm = new CloseJobVm();
                closevm.Consignee = item.Consignee;
                closevm.Customer = item.Customer;
                closevm.InvoiceDate = item.InvoiceDate;
                closevm.InvoiceNo = item.InvoiceNo.Value;
                closevm.JobCode = item.JobCode;
                closevm.JobDate = item.JobDate.Value;
                closevm.JobDescription = item.JobDescription;
                closevm.Shipper = item.Shipper;
                closevm.JobID = item.JobID;

                lst.Add(closevm);
            }
            string view = this.RenderPartialView("ucClosedJob", lst);

            return new JsonResult
            {
                Data = new
                {
                    success = true,
                    closedjoblist = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        public JsonResult CheckForStatusSea(int jobtypeid)
        {
            //var query = (from t in entity.JobGenerations
            //             where t.MainJobID == mainjobid
            //             group t by t.JobCode into a
            //             select new { JobCode = a.Key, mainjob = a.Sum(x => x.MainJobID) }).ToList();

            bool mystatus;
            mystatus = (from c in entity.JobTypes where c.JobTypeID == jobtypeid select c.StatusSea).FirstOrDefault().Value;

            return Json(mystatus, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSeaStatus(int JobTypeID)
        {
            var query = (from t in entity.JobTypes where t.JobTypeID == JobTypeID select t.StatusSea).FirstOrDefault();
            return Json(query, JsonRequestBehavior.AllowGet);
        }




        public ActionResult GetJob(DateTime fdate, DateTime tdate)
        {

            var data = entity.SP_GetAllJobsDetailsByDate(Convert.ToDateTime(fdate), Convert.ToDateTime(tdate)).ToList();



            string view = this.RenderPartialView("_GetJob", data);

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
        [HttpGet]
        public ActionResult ClosedJobView(int id)
        {
            //id = 31154;
            Session["Cargoid"] = null;
            Session["InvoiceID"] = null;
            Session["AddBill"] = null;
            Session["ContainerID"] = null;
            Session["AddLog"] = null;

            JobGeneration JG = new JobGeneration();


            if (Session["UserID"] != null)
            {

                BindAllMasters();


                Session["JobID"] = id;

                JG = J.JobGenerationByJobID(id);

                if (id == 0)
                {
                    JG.InvoiceDate = System.DateTime.UtcNow;
                    JG.JobDate = System.DateTime.UtcNow;
                    JG.InvoiceNo = Convert.ToInt32(J.GetMaxInvoiceNumber());
                    var ji = entity.JInvoices.Where(a => a.JobID == 0).ToList();
                    foreach (var i in ji)
                    {
                        entity.JInvoices.Remove(i);
                        entity.SaveChanges();
                    }
                    var JC = entity.JContainerDetails.Where(a => a.JobID == 0).ToList();
                    foreach (var i in JC)
                    {
                        entity.JContainerDetails.Remove(i);
                        entity.SaveChanges();
                    }
                    var Jcargo = entity.JCargoDescriptions.Where(a => a.JobID == 0).ToList();
                    foreach (var i in Jcargo)
                    {
                        entity.JCargoDescriptions.Remove(i);
                        entity.SaveChanges();
                    }
                    var JBillofEntry = entity.JBIllOfEntries.Where(a => a.JobID == 0).ToList();
                    foreach (var i in JBillofEntry)
                    {
                        entity.JBIllOfEntries.Remove(i);
                        entity.SaveChanges();
                    }
                    var JAuditlog = entity.JAuditLogs.Where(a => a.JobID == 0).ToList();
                    foreach (var i in JAuditlog)
                    {
                        entity.JAuditLogs.Remove(i);
                        entity.SaveChanges();
                    }
                }
                else
                {
                    if (JG.InvoiceDate.ToString() != "")
                    {
                        JG.InvoiceDate = JG.InvoiceDate;
                    }
                    else
                    {
                        JG.InvoiceDate = System.DateTime.UtcNow;
                    }

                    if (Convert.ToInt32(JG.InvoiceNo) > 0)
                    {
                        JG.InvoiceNo = JG.InvoiceNo;
                        ViewBag.IsInvoiveGenerated = true;
                    }
                    else
                    {

                        ViewBag.IsInvoiveGenerated = false;
                        var invoice = (from t in entity.JobGenerations where t.JobID == JG.JobID select t.InvoiceNo).FirstOrDefault();

                        JG.InvoiceNo = Convert.ToInt32(invoice.Value);
                        if (invoice == 0)
                        {
                            invoice = Convert.ToInt32(J.GetMaxInvoiceNumber());
                            JG.InvoiceNo = invoice;
                        }

                        bool x = (from c in entity.JobTypes where c.JobTypeID == JG.JobTypeID select c.StatusSea).FirstOrDefault().Value;

                        if (x == false)
                        {
                            var d = entity.JobGenerations.Find(id);

                            JG.Flight = d.Flight;
                            JG.HAWB = d.HAWB;
                            JG.MAWB = d.MAWB;
                            JG.DepartingDate = d.DepartingDate;
                        }

                    }
                }

            }
            else
            {
                return RedirectToAction("Login", "Login", new { ID = "1" });
            }

            var query = from t in entity.JobGenerations
                        where t.MainJobID == null || t.MainJobID == 0

                        select t;

            ViewBag.MainJobId = query;

            return View(JG);
        }
        public JsonResult GetopenByDate(string fdate, string tdate, int FYearID)
        {

            DateTime d = DateTime.Now;
            DateTime fyear = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime mstart = new DateTime(fyear.Year, d.Month, 01);

            int maxday = DateTime.DaysInMonth(fyear.Year, d.Month);
            DateTime mend = new DateTime(fyear.Year, d.Month, maxday);


            var open = entity.SP_GetAllJobsDetailsByDate(Convert.ToDateTime(fdate), Convert.ToDateTime(tdate)).ToList();

            string view = this.RenderPartialView("ucOpenJob", open);
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

        public JsonResult GetclouseByDate(string fdate, string tdate, int FYearID)
        {

            DateTime d = DateTime.Now;
            DateTime fyear = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime mstart = new DateTime(fyear.Year, d.Month, 01);

            int maxday = DateTime.DaysInMonth(fyear.Year, d.Month);
            DateTime mend = new DateTime(fyear.Year, d.Month, maxday);


            var open = entity.SP_GetAllClosedJobsDetailsByDate(Convert.ToDateTime(fdate), Convert.ToDateTime(tdate), FYearID).ToList();

            string view = this.RenderPartialView("ucClosedJob", open);
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


        //Closed and Open Job


        public ActionResult NewOpenJob(string frmdate, string tdate)
        {
            DateTime d = DateTime.Now;
            DateTime fyear = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime mstart = new DateTime(fyear.Year, fyear.Month, 01);

            int maxday = DateTime.DaysInMonth(fyear.Year, d.Month);
            DateTime mend = new DateTime(fyear.Year, d.Month, maxday);

            List<CloseJobVm> lst = new List<CloseJobVm>();

            if (frmdate == null && tdate == null)
            {

                var open = entity.SP_GetAllJobsDetailsForClosedJob().Where(x => x.JobDate >= mstart && x.JobDate <= mend).OrderByDescending(x => x.JobDate).ToList();
                foreach (var item in open)
                {
                    CloseJobVm closevm = new CloseJobVm();
                    closevm.Consignee = item.Consignee;
                    closevm.Customer = item.Customer;
                    closevm.InvoiceDate = item.InvoiceDate;
                    closevm.InvoiceNo = item.InvoiceNo.Value;
                    closevm.JobCode = item.JobCode;
                    closevm.JobDate = item.JobDate.Value;
                    closevm.JobDescription = item.JobDescription;
                    closevm.Shipper = item.Shipper;
                    closevm.JobID = item.JobID;

                    lst.Add(closevm);
                }
                return View(lst);
            }
            else
            {

                var open = entity.SP_GetAllJobsDetailsForClosedJob().Where(x => x.JobDate >= Convert.ToDateTime(frmdate) && x.JobDate <= Convert.ToDateTime(tdate)).OrderByDescending(x => x.JobDate).ToList();
                foreach (var item in open)
                {
                    CloseJobVm closevm = new CloseJobVm();
                    closevm.Consignee = item.Consignee;
                    closevm.Customer = item.Customer;
                    closevm.InvoiceDate = item.InvoiceDate;
                    closevm.InvoiceNo = item.InvoiceNo.Value;
                    closevm.JobCode = item.JobCode;
                    closevm.JobDate = item.JobDate.Value;
                    closevm.JobDescription = item.JobDescription;
                    closevm.Shipper = item.Shipper;
                    closevm.JobID = item.JobID;

                    lst.Add(closevm);
                }
                return View(lst);

            }

        }
        [HttpPost]
        public ActionResult NewOpenJob(List<TrueBooksMVC.Models.CloseJobVm> lst)
        {

            var selectedrecords = lst.Where(item => item.IsSelected == true && item.InvoiceNo > 0).ToList();
            foreach (var item in selectedrecords)
            {
                JobGeneration a = (from c in entity.JobGenerations where c.JobID == item.JobID select c).FirstOrDefault();
                a.IsClosed = true;

                entity.Entry(a).State = EntityState.Modified;
                entity.SaveChanges();
            }
            return RedirectToAction("NewOpenJob");
        }

        public ActionResult NewClosedJob(string frmdate, string tdate)
        {

            DateTime d = DateTime.Now;
            DateTime fyear = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime mstart = new DateTime(fyear.Year, fyear.Month, 01);

            int maxday = DateTime.DaysInMonth(fyear.Year, d.Month);
            DateTime mend = new DateTime(fyear.Year, d.Month, maxday);
            List<CloseJobVm> lst = new List<CloseJobVm>();
            if (frmdate == null && tdate == null)
            {

                var data = entity.SP_GetAllClosedJobsDetails().Where(x => x.JobDate >= mstart && x.JobDate <= mend).OrderByDescending(x => x.JobDate).ToList();
                foreach (var item in data)
                {
                    CloseJobVm closevm = new CloseJobVm();
                    closevm.Consignee = item.Consignee;
                    closevm.Customer = item.Customer;
                    closevm.InvoiceDate = item.InvoiceDate.Value;
                    closevm.InvoiceNo = item.InvoiceNo.Value;
                    closevm.JobCode = item.JobCode;
                    closevm.JobDate = item.JobDate.Value;
                    closevm.JobDescription = item.JobDescription;
                    closevm.Shipper = item.Shipper;
                    closevm.JobID = item.JobID;

                    lst.Add(closevm);
                }
                return View(lst);
            }
            else
            {
                var data = entity.SP_GetAllClosedJobsDetails().Where(x => x.JobDate >= Convert.ToDateTime(frmdate) && x.JobDate <= Convert.ToDateTime(tdate)).OrderByDescending(x => x.JobDate).ToList();


                foreach (var item in data)
                {
                    CloseJobVm closevm = new CloseJobVm();
                    closevm.Consignee = item.Consignee;
                    closevm.Customer = item.Customer;
                    closevm.InvoiceDate = item.InvoiceDate.Value;
                    closevm.InvoiceNo = item.InvoiceNo.Value;
                    closevm.JobCode = item.JobCode;
                    closevm.JobDate = item.JobDate.Value;
                    closevm.JobDescription = item.JobDescription;
                    closevm.Shipper = item.Shipper;
                    closevm.JobID = item.JobID;

                    lst.Add(closevm);
                }
                return View(lst);
            }
        }



    }
}
