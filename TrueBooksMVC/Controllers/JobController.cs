﻿using System;
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
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Configuration;
namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
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
            var Jobstatus = (from d in entity.JobStatus select d).ToList();
            ViewBag.Jobstatus = Jobstatus;

            if (Session["UserID"] != null)
            {

                BindAllMasters();


                if (id > 0)
                {


                    var InvoiceNo = 10000;
                    var invoiceNumber = entity.JInvoices.Select(d => d.InvoiceNumber).ToList().LastOrDefault();


                    var invoicenum = "";
                    if (invoiceNumber != null)
                    {
                        var strInvoice = invoiceNumber.Split('-');
                        var strinvoicenum = strInvoice[1].Split('/');
                        if (strinvoicenum.Count() > 1)
                        {
                            var invnum = Convert.ToInt32(strinvoicenum[1]) + 1;
                            invoicenum = "/" + invnum;
                            InvoiceNo = Convert.ToInt32(strinvoicenum[0]);
                        }
                        else
                        {
                            invoicenum = "/1";
                            InvoiceNo = Convert.ToInt32(strinvoicenum[0]);
                        }
                    }
                    ViewBag.InvoiceNumber = "JI-" + InvoiceNo + invoicenum;



                }
                else
                {
                    var InvoiceNo = 10000;
                    ViewBag.InvoiceNumber = "JI-" + InvoiceNo;

                }








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
                    var jinvoices = entity.JInvoices.Where(d => d.JobID == JG.JobID && d.InvoiceStatus == "1").ToList();
                    if (jinvoices.Count > 0)
                    {
                        ViewBag.IsInvoiveGenerated = true;
                    }
                    else
                    {
                        ViewBag.IsInvoiveGenerated = false;

                    }
                    if (Convert.ToInt32(JG.InvoiceNo) > 0)
                    {
                        JG.InvoiceNo = JG.InvoiceNo;

                    }
                    else
                    {

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
            var job = (from d in entity.JobGenerations where d.JobID == id select d).FirstOrDefault();
            if (job != null)
            {
                if (job.JobStatusId != null)
                {
                    ViewBag.JobStatusId = job.JobStatusId;
                }
                else
                {
                    ViewBag.JobStatusId = 0;
                }
            }
            var query = from t in entity.JobGenerations
                        where t.MainJobID == null || t.MainJobID == 0

                        select t;

            ViewBag.MainJobId = query;
            var StaffNotes = (from d in entity.StaffNotes where d.JobId == id && d.PageTypeId == 1 orderby d.Id descending select d).ToList();
            var branchid = Convert.ToInt32(Session["branchid"]);
            var users = (from d in entity.UserRegistrations select d).ToList();
            var staffnotemodel = new List<StaffNoteModel>();
            foreach (var item in StaffNotes)
            {
                var model = new StaffNoteModel();
                model.id = item.Id;
                model.employeeid = item.EmployeeId;
                model.jobid = item.JobId;
                model.TaskDetails = item.TaskDetails;
                model.Datetime = item.Datetime;
                model.EmpName = users.Where(d => d.UserID == item.EmployeeId).FirstOrDefault().UserName;
                staffnotemodel.Add(model);
            }
            ViewBag.StaffNoteModel = staffnotemodel;
            var JobStatusModel = new List<JobStatusModel>();
            var Jstatus = (from d in entity.JStatus where d.JobId == id orderby d.Id descending select d).ToList();
            foreach (var item in Jstatus)
            {
                var model = new JobStatusModel();
                model.id = item.Id;
                model.employeeid = item.EmployeeId;
                model.jobid = item.JobId;
                model.StatusId = item.JobStatusId;
                model.StatusName = Jobstatus.Where(d => d.JobStatusId == item.JobStatusId).FirstOrDefault().StatusName;
                model.Datetime = item.DateTime;
                model.EmpName = users.Where(d => d.UserID == item.EmployeeId).FirstOrDefault().UserName;
                JobStatusModel.Add(model);
            }
            ViewBag.JobStatusModel = JobStatusModel;
            var customerdetails = (from d in entity.CUSTOMERs where d.CustomerID == JG.ConsigneeID select d).FirstOrDefault();
            if (customerdetails == null)
            {
                customerdetails = new CUSTOMER();
            }
            ViewBag.CustomerDetail = customerdetails;
            var CustomerNotification = (from d in entity.CustomerNotifications where d.JobId == id && d.PageTypeId == 1 orderby d.Id descending select d).ToList();

            var customernotification = new List<CustomerNotificationModel>();
            foreach (var item in CustomerNotification)
            {
                var model = new CustomerNotificationModel();
                model.id = item.Id;
                model.employeeid = item.StaffId;
                model.jobid = item.JobId;
                model.Message = item.Message;
                model.Datetime = item.Datetime;
                model.IsEmail = item.IsEmail;
                model.IsSms = item.IsSms;
                model.IsWhatsapp = item.IsWhatsapp;
                model.EmpName = users.Where(d => d.UserID == item.StaffId).FirstOrDefault().UserName;
                customernotification.Add(model);
            }
            ViewBag.CustomerNotification = customernotification;
            List<JTimeLine> TimeLine = entity.JTimeLines.Where(d => d.JobId == id).OrderByDescending(d => d.DateTime).ToList();
            ViewBag.TimeLines = TimeLine;
            return View(JG);

        }

        public ActionResult JobDetails(int ID)
        {
            List<SP_GetAllJobsDetails_Result> AllJobs = new List<SP_GetAllJobsDetails_Result>();

            // AllJobs = J.AllJobsDetails();
            DateTime a = Convert.ToDateTime(Session["FyearFrom"]);
            DateTime b = Convert.ToDateTime(Session["FyearTo"]);
            var JobStatus = (from d in entity.JobStatus select d).ToList();
            ViewBag.Jobstatus = JobStatus;
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
            if (ID == 21)
            {
                ViewBag.SuccessMsg = "You have successfully Generated Invoice.";
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
            var Jobstatus = (from d in entity.JobStatus select d).ToList();
            ViewBag.Jobstatus = Jobstatus;

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
                var maxjobid = J.MaxJobID();
                if (String.IsNullOrEmpty(maxjobid))
                {
                    JobId = 1;
                }
                else
                {
                    JobId = Convert.ToInt32(J.MaxJobID()) - 1;
                }
                //  JM.JobID = JobId;
                //  JM.EmployeeID = Convert.ToInt32(Session["UserID"]);
                i = J.AddJob(JM);
                JobId = i;

                if (i > 0)
                {
                    DeleteAndInsertRecords(formCollection, JobId, 1);

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
                    var job = (from d in entity.JobGenerations where d.JobID == JobId select d).FirstOrDefault();
                    job.JobStatusId = 1;
                    entity.Entry(job).State = EntityState.Modified;
                    entity.SaveChanges();
                    var jobstatus = new JStatu();
                    jobstatus.JobId = JobId;
                    jobstatus.EmployeeId = Convert.ToInt32(Session["UserID"]);
                    jobstatus.DateTime = DateTime.Now;
                    jobstatus.JobStatusId = 1;
                    entity.JStatus.Add(jobstatus);
                    entity.SaveChanges();
                    Session["JobID"] = JobId;
                    JTimeLine Timeline = new JTimeLine();
                    Timeline.JobId = JobId;
                    Timeline.TabName = "";
                    Timeline.ActionType = "Created";
                    Timeline.DateTime = DateTime.Now;
                    Timeline.UserId = Convert.ToInt32(Session["UserID"]);
                    Timeline.UserName = Session["UserName"].ToString();
                    entity.JTimeLines.Add(Timeline);
                    entity.SaveChanges();


                    JM.JobID = JobId;
                    return RedirectToAction("JobDetails", "Job", new { ID = JobId });
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
                                DeleteAndInsertRecords(formCollection, JobId, 2);

                                var data = (from c in entity.JobGenerations where c.JobID == JobId select c).FirstOrDefault();
                                data.JobStatusId = 2;
                                entity.Entry(data).State = EntityState.Modified;
                                entity.SaveChanges();
                                //int acjid = 0;
                                //if (data.AcJournalID != null)
                                //{
                                //    acjid = data.AcJournalID.Value;
                                //}
                                //else
                                //{
                                //    var LatestJournal = entity.AcJournalMasters.ToList().LastOrDefault();
                                //    var JournalId = 1;
                                //    if(LatestJournal !=null)
                                //    {
                                //        JournalId = LatestJournal.AcJournalID + 1;
                                //    }
                                //    var acjournalmaster = new AcJournalMaster();
                                //    acjournalmaster.AcFinancialYearID = Convert.ToInt32(Session["fyearid"]);
                                //    acjournalmaster.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"]);
                                //    acjournalmaster.VoucherType = "J";
                                //    acjournalmaster.TransType = 1;
                                //    acjournalmaster.UserID = Convert.ToInt32(Session["UserID"]);
                                //    acjournalmaster.AcJournalID = JournalId;
                                //    acjournalmaster.VoucherNo = JM.JobCode;
                                //    entity.AcJournalMasters.Add(acjournalmaster);
                                //    entity.SaveChanges();
                                //    var job = (from d in entity.JobGenerations where d.JobID == JobId select d).FirstOrDefault();
                                //    job.AcJournalID = JournalId;
                                //    entity.Entry(job).State = EntityState.Modified;
                                //    entity.SaveChanges();
                                //    acjid = JournalId;
                                //}
                                //int acprovjid = 0;
                                //if (data.AcProvisionCostJournalID != null)
                                //{
                                //    acprovjid = data.AcProvisionCostJournalID.Value;
                                //}

                                //decimal shome = 0;
                                //decimal phome = 0;
                                //if (entity.JInvoices.Where(x => x.JobID == JobId).Count() > 0)
                                //{
                                //    shome = entity.JInvoices.Where(x => x.JobID == JobId).Sum(x => x.SalesHome ?? 0);
                                //    phome = entity.JInvoices.Where(x => x.JobID == JobId).Sum(x => x.ProvisionHome ?? 0);
                                //}
                                //var jinvoices = entity.JInvoices.Where(x => x.JobID == JobId).ToList();
                                //foreach (var item in jinvoices)
                                //{
                                //    var Rev_achead = entity.RevenueTypes.Where(d => d.RevenueTypeID ==item.RevenueTypeID).FirstOrDefault();
                                //    int acdet = (from x in entity.AcJournalDetails where x.AcJournalID == acjid && x.AcHeadID == Rev_achead.AcHeadID  select x.AcJournalDetailID).FirstOrDefault();
                                //    var acdata= (from x in entity.AcJournalDetails where x.AcJournalDetailID == acdet select x).FirstOrDefault();
                                //    if(acdata !=null)
                                //    {
                                //        acdata.Amount = item.SalesHome;
                                //        entity.Entry(acdata).State = EntityState.Modified;
                                //        entity.SaveChanges();
                                //    }
                                //    else
                                //    {
                                //        var acJournalDetailid = entity.AcJournalDetails.ToList().LastOrDefault();
                                //        var acjournalDet_id = 1;
                                //        if(acJournalDetailid !=null)
                                //        {
                                //            acjournalDet_id = acJournalDetailid.AcJournalDetailID + 1;
                                //        }
                                //        acdata = new AcJournalDetail();
                                //        acdata.AcHeadID = Rev_achead.AcHeadID;
                                //        acdata.AcJournalID = acjid;
                                //        acdata.Amount = item.SalesHome;
                                //        acdata.AcJournalDetailID = acjournalDet_id;
                                //        entity.AcJournalDetails.Add(acdata);
                                //        entity.SaveChanges();


                                //    }
                                //}


                                //var pagecontrol = (from d in entity.PageControlMasters where d.ControlName.ToLower() == "generate invoice" select d).FirstOrDefault();
                                //var accountsetup = (from d in entity.AcHeadControls where d.Pagecontrol == pagecontrol.Id select d).FirstOrDefault();

                                ////int custcontrolacid = (from c in entity.AcHeadAssigns select c.CustomerControlAcID.Value).FirstOrDefault();
                                //////int freightacheadid = 158;
                                //////int provcontrolacid = (from c in entity.AcHeadAssigns select c.ProvisionCostControlAcID.Value).FirstOrDefault();
                                //////int accruedcontrolacid = (from c in entity.AcHeadAssigns select c.AccruedCostControlAcID.Value).FirstOrDefault();

                                //var acjdetail1 = (from x in entity.AcJournalDetails where x.AcJournalID == acjid && x.AcHeadID == accountsetup.AccountHeadID select x).FirstOrDefault();
                                //int acjdetail2 = 0;
                                //if (acjdetail1 !=null)
                                //{
                                //    acjdetail2 = acjdetail1.AcJournalDetailID;
                                //}                               
                                //var data1 = (from x in entity.AcJournalDetails where x.AcJournalDetailID == acjdetail2 select x).FirstOrDefault();
                                //if (data1 != null)
                                //{
                                //    data1.Amount = -shome;
                                //    entity.Entry(data1).State = EntityState.Modified;
                                //    entity.SaveChanges();
                                //}
                                //else
                                //{
                                //    var acJournalDetailid = entity.AcJournalDetails.ToList().LastOrDefault();
                                //    var acjournalDet_id = 1;
                                //    if (acJournalDetailid != null)
                                //    {
                                //        acjournalDet_id = acJournalDetailid.AcJournalDetailID + 1;
                                //    }

                                //    data1 = new AcJournalDetail();
                                //    data1.AcHeadID = accountsetup.AccountHeadID;
                                //    data1.AcJournalID = acjid;
                                //    data1.Amount = -shome;
                                //    data1.AcJournalDetailID = acjournalDet_id;
                                //    entity.AcJournalDetails.Add(data1);
                                //    entity.SaveChanges();

                                //}


                                JTimeLine Timeline = new JTimeLine();
                                Timeline.JobId = JobId;
                                Timeline.TabName = "";
                                Timeline.ActionType = "Modified";
                                Timeline.DateTime = DateTime.Now;
                                Timeline.UserId = Convert.ToInt32(Session["UserID"]);
                                Timeline.UserName = Session["UserName"].ToString();
                                entity.JTimeLines.Add(Timeline);
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
                            //  int j = J.UpdateJobIDinAllModules(JobId, Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["fyearid"]));
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
                            //int k = J.UpdateInvoiceNumber(Convert.ToInt32(Session["JobID"]), Convert.ToInt32(JM.InvoiceNo), Convert.ToDateTime(JM.InvoiceDate), Convert.ToInt32(Session["fyearid"].ToString()));
                            JobId = Convert.ToInt32(Session["JobID"]);
                            var jobinvoice = (from d in entity.JobGenerations where d.JobID == JobId select d).FirstOrDefault();
                            jobinvoice.InvoiceNo = 1;
                            entity.Entry(jobinvoice).State = EntityState.Modified;
                            entity.SaveChanges();

                            var data = (from c in entity.JobGenerations where c.JobID == JobId select c).FirstOrDefault();
                            int acjid = 0;
                            if (data.AcJournalID != null)
                            {
                                acjid = data.AcJournalID.Value;
                            }
                            else
                            {
                                var LatestJournal = entity.AcJournalMasters.ToList().LastOrDefault();
                                var JournalId = 1;
                                if (LatestJournal != null)
                                {
                                    JournalId = LatestJournal.AcJournalID + 1;
                                }
                                var acjournalmaster = new AcJournalMaster();
                                acjournalmaster.AcFinancialYearID = Convert.ToInt32(Session["fyearid"]);
                                acjournalmaster.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"]);
                                acjournalmaster.VoucherType = "CI";
                                acjournalmaster.TransType = null;
                                acjournalmaster.TransDate = DateTime.Now;
                                acjournalmaster.UserID = Convert.ToInt32(Session["UserID"]);
                                acjournalmaster.AcJournalID = JournalId;
                                acjournalmaster.VoucherNo = JM.JobCode;
                                entity.AcJournalMasters.Add(acjournalmaster);
                                entity.SaveChanges();
                                var job = (from d in entity.JobGenerations where d.JobID == JobId select d).FirstOrDefault();
                                job.AcJournalID = JournalId;
                                job.JobStatusId = 3;
                                entity.Entry(job).State = EntityState.Modified;
                                entity.SaveChanges();
                                acjid = JournalId;
                            }
                            int acprovjid = 0;
                            if (data.AcProvisionCostJournalID != null)
                            {
                                acprovjid = data.AcProvisionCostJournalID.Value;
                            }

                            decimal shome = 0;
                            decimal Tax = 0;
                            if (entity.JInvoices.Where(x => x.JobID == JobId && x.CancelledInvoice == false).Count() > 0)
                            {
                                shome = entity.JInvoices.Where(x => x.JobID == JobId && x.CancelledInvoice == false).Sum(x => x.SalesHome ?? 0);
                                Tax = entity.JInvoices.Where(x => x.JobID == JobId && x.CancelledInvoice == false).Sum(x => x.TaxAmount ?? 0);
                            }
                            var jinvoices = entity.JInvoices.Where(x => x.JobID == JobId && x.CancelledInvoice == false).ToList();
                            foreach (var item in jinvoices)
                            {
                                var Rev_achead = entity.RevenueTypes.Where(d => d.RevenueTypeID == item.RevenueTypeID).FirstOrDefault();
                                int acdet = (from x in entity.AcJournalDetails where x.AcJournalID == acjid && x.AcHeadID == Rev_achead.AcHeadID select x.AcJournalDetailID).FirstOrDefault();
                                var acdata = (from x in entity.AcJournalDetails where x.AcJournalDetailID == acdet select x).FirstOrDefault();
                                if (acdata != null)
                                {
                                    acdata.Amount = item.SalesHome;
                                    entity.Entry(acdata).State = EntityState.Modified;
                                    entity.SaveChanges();
                                }
                                else
                                {
                                    var acJournalDetailid = entity.AcJournalDetails.ToList().LastOrDefault();
                                    var acjournalDet_id = 1;
                                    if (acJournalDetailid != null)
                                    {
                                        acjournalDet_id = acJournalDetailid.AcJournalDetailID + 1;
                                    }
                                    acdata = new AcJournalDetail();
                                    acdata.AcHeadID = Rev_achead.AcHeadID;
                                    acdata.AcJournalID = acjid;
                                    acdata.Amount = item.SalesHome;
                                    acdata.AcJournalDetailID = acjournalDet_id;
                                    entity.AcJournalDetails.Add(acdata);
                                    entity.SaveChanges();


                                }
                            }


                            var pagecontrol = (from d in entity.PageControlMasters where d.ControlName.ToLower() == "generate invoice" select d).FirstOrDefault();
                            var accountsetup = (from d in entity.AcHeadControls where d.Pagecontrol == pagecontrol.Id select d).ToList();

                            //int custcontrolacid = (from c in entity.AcHeadAssigns select c.CustomerControlAcID.Value).FirstOrDefault();
                            ////int freightacheadid = 158;
                            ////int provcontrolacid = (from c in entity.AcHeadAssigns select c.ProvisionCostControlAcID.Value).FirstOrDefault();
                            ////int accruedcontrolacid = (from c in entity.AcHeadAssigns select c.AccruedCostControlAcID.Value).FirstOrDefault();
                            foreach (var acsetup in accountsetup)
                            {
                                var acjdetail1 = (from x in entity.AcJournalDetails where x.AcJournalID == acjid && x.AcHeadID == acsetup.AccountHeadID select x).FirstOrDefault();
                                int acjdetail2 = 0;
                                decimal amount = 0;
                                if (acsetup.AccountName.ToLower() == "sales total")
                                {
                                    if (acsetup.AccountNature == true)
                                    {
                                        amount = shome * -1;
                                    }
                                    else
                                    {
                                        amount = shome;
                                    }
                                }
                                else if (acsetup.AccountName.ToLower() == "tax total")
                                {
                                    if (acsetup.AccountNature == true)
                                    {
                                        amount = Tax * -1;
                                    }
                                    else
                                    {
                                        amount = Tax;
                                    }
                                }
                                else if (acsetup.AccountName.ToLower() == "total sales & tax")
                                {
                                    if (acsetup.AccountNature == true)
                                    {
                                        amount = (shome + Tax) * -1;
                                    }
                                    else
                                    {
                                        amount = (shome + Tax);
                                    }
                                }
                                if (acjdetail1 != null)
                                {
                                    acjdetail2 = acjdetail1.AcJournalDetailID;
                                }
                                var data1 = (from x in entity.AcJournalDetails where x.AcJournalDetailID == acjdetail2 select x).FirstOrDefault();
                                if (data1 != null)
                                {
                                    data1.Amount = amount;
                                    entity.Entry(data1).State = EntityState.Modified;
                                    entity.SaveChanges();
                                }
                                else
                                {
                                    var acJournalDetailid = entity.AcJournalDetails.ToList().LastOrDefault();
                                    var acjournalDet_id = 1;
                                    if (acJournalDetailid != null)
                                    {
                                        acjournalDet_id = acJournalDetailid.AcJournalDetailID + 1;
                                    }

                                    data1 = new AcJournalDetail();
                                    data1.AcHeadID = acsetup.AccountHeadID;
                                    data1.AcJournalID = acjid;
                                    data1.Amount = amount;
                                    data1.AcJournalDetailID = acjournalDet_id;
                                    entity.AcJournalDetails.Add(data1);
                                    entity.SaveChanges();

                                }
                            }
                            var alljinvoices = (from d in entity.JInvoices where d.JobID == JobId && d.CancelledInvoice == false select d).ToList();
                            var InvoiceNo = 10000;
                            var invoiceNumber = entity.JInvoices.Select(d => d.InvoiceNumber).ToList().LastOrDefault();

                            foreach (var item in alljinvoices)
                            {
                                var j_invoice = entity.JInvoices.Where(d => d.InvoiceID == item.InvoiceID).FirstOrDefault();
                                j_invoice.CancelledInvoice = false;
                                j_invoice.InvoiceDate = DateTime.Now;
                                j_invoice.CancelReason = "";

                                var invoicenum = "";
                                if (invoiceNumber != null)
                                {
                                    var strInvoice = invoiceNumber.Split('-');
                                    var strinvoicenum = strInvoice[1].Split('/');
                                    if (strinvoicenum.Count() > 1)
                                    {
                                        var invnum = Convert.ToInt32(strinvoicenum[1]) + 1;
                                        invoicenum = "/" + invnum;
                                        InvoiceNo = Convert.ToInt32(strinvoicenum[0]);
                                    }
                                    else
                                    {
                                        invoicenum = "/1";
                                        InvoiceNo = Convert.ToInt32(strinvoicenum[0]);
                                    }
                                    //InvoiceNo = Convert.ToInt32(strInvoice[1]) + 1;
                                }
                                //InvoiceNo = InvoiceNo + 1;
                                j_invoice.InvoiceNumber = "JI-" + InvoiceNo + invoicenum;
                                j_invoice.InvoiceStatus = "1";
                                entity.Entry(j_invoice).State = EntityState.Modified;
                                entity.SaveChanges();
                            }
                            JTimeLine Timeline1 = new JTimeLine();
                            Timeline1.JobId = Convert.ToInt32(Session["JobID"]);
                            Timeline1.TabName = "";
                            Timeline1.ActionType = "Invoice Generated";
                            Timeline1.DateTime = DateTime.Now;
                            Timeline1.UserId = Convert.ToInt32(Session["UserID"]);
                            Timeline1.UserName = Session["UserName"].ToString();
                            entity.JTimeLines.Add(Timeline1);
                            entity.SaveChanges();
                            var acJournalmaster = new AcJournalMaster();



                            ////if (k > 0)
                            ////{
                            return RedirectToAction("JobDetails", "Job", new { ID = 21 });
                            //}
                        }
                        else
                        {
                            return RedirectToAction("Login", "Login");
                        }
                    }
                }
            }
            var StaffNotes = (from d in entity.StaffNotes where d.JobId == JobId && d.PageTypeId == 1 orderby d.Id descending select d).ToList();
            var branchid = Convert.ToInt32(Session["branchid"]);
            var users = (from d in entity.UserRegistrations select d).ToList();
            var staffnotemodel = new List<StaffNoteModel>();
            foreach (var item in StaffNotes)
            {
                var model = new StaffNoteModel();
                model.id = item.Id;
                model.employeeid = item.EmployeeId;
                model.jobid = item.JobId;
                model.TaskDetails = item.TaskDetails;
                model.Datetime = item.Datetime;
                model.EmpName = users.Where(d => d.UserID == item.EmployeeId).FirstOrDefault().UserName;

                staffnotemodel.Add(model);
            }
            ViewBag.StaffNoteModel = staffnotemodel;
            var JobStatusModel = new List<JobStatusModel>();
            var Jstatus = (from d in entity.JStatus where d.JobId == id orderby d.Id descending select d).ToList();
            foreach (var item in Jstatus)
            {
                var model = new JobStatusModel();
                model.id = item.Id;
                model.employeeid = item.EmployeeId;
                model.jobid = item.JobId;
                model.StatusId = item.JobStatusId;
                model.StatusName = Jobstatus.Where(d => d.JobStatusId == item.JobStatusId).FirstOrDefault().StatusName;
                model.Datetime = item.DateTime;
                model.EmpName = users.Where(d => d.UserID == item.EmployeeId).FirstOrDefault().UserName;
                JobStatusModel.Add(model);
            }
            ViewBag.JobStatusModel = JobStatusModel;
            var customerdetails = (from d in entity.CUSTOMERs where d.CustomerID == JM.ConsigneeID select d).FirstOrDefault();
            if (customerdetails == null)
            {
                customerdetails = new CUSTOMER();
            }
            ViewBag.CustomerDetail = customerdetails;
            var CustomerNotification = (from d in entity.CustomerNotifications where d.JobId == id && d.PageTypeId == 1 orderby d.Id descending select d).ToList();

            var customernotification = new List<CustomerNotificationModel>();
            foreach (var item in CustomerNotification)
            {
                var model = new CustomerNotificationModel();
                model.id = item.Id;
                model.employeeid = item.StaffId;
                model.jobid = item.JobId;
                model.Message = item.Message;
                model.Datetime = item.Datetime;
                model.IsEmail = item.IsEmail;
                model.IsSms = item.IsSms;
                model.IsWhatsapp = item.IsWhatsapp;
                model.EmpName = users.Where(d => d.UserID == item.StaffId).FirstOrDefault().UserName;
                customernotification.Add(model);
            }
            ViewBag.CustomerNotification = customernotification;
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

        private bool DeleteAndInsertRecords(FormCollection formCollection, int JobId, int Type)
        {
            if (JobId <= 0)
            {
                return false;
            }
            int i = 0;
            int ChargesCount = 0;
            ArrayList ChargesArray = new ArrayList();
            string DeletedInvoiceIds = ((string[])formCollection.GetValue("DeletedInvoiceIds").RawValue)[0].ToString();
            string DeletedCargoIds = ((string[])formCollection.GetValue("DeletedCargoIds").RawValue)[0].ToString();
            string DeletedContainerIds = ((string[])formCollection.GetValue("DeletedContainerIds").RawValue)[0].ToString();
            string DeletedBillOfEntryIds = ((string[])formCollection.GetValue("DeletedBillOfEntryIds").RawValue)[0].ToString();
            string DeletedAuditLogIDs = ((string[])formCollection.GetValue("DeletedAuditLogIDs").RawValue)[0].ToString();

            DeleteJobDetailsByJobID(JobId, DeletedInvoiceIds, DeletedCargoIds, DeletedContainerIds, DeletedBillOfEntryIds, DeletedAuditLogIDs);

            for (int j = 0; j < formCollection.Keys.Count; j++)
            {
                if (formCollection.Keys[j].StartsWith("RevenueTypeID_"))
                {
                    ChargesCount = ChargesCount + 1;
                    ChargesArray.Add(formCollection.Keys[j].Replace("RevenueTypeID_", "").Trim());
                }
            }
            var Jobgeneration = (from d in entity.JobGenerations where d.JobID == JobId select d).FirstOrDefault();

            for (int c = 0; c < ChargesCount; c++)
            {
                var InvoiceNo = 10000;
                var invoiceNumber = entity.JInvoices.Select(d => d.InvoiceNumber).ToList().LastOrDefault();
                string[] strArray;
                JInvoice Charges = new JInvoice();
                Charges.UserID = Convert.ToInt32(Session["UserID"].ToString());
                Charges.JobID = JobId;
                Charges.CostUpdationStatus = "0";

                int InvoiceID = 0;
                if (formCollection.GetValue("InvoiceID_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("InvoiceID_" + ChargesArray[c]).RawValue;
                    int.TryParse(strArray[0], out InvoiceID);
                }
                Charges.InvoiceID = InvoiceID;

                int RevenueTypeID = 0;
                if (formCollection.GetValue("RevenueTypeID_" + ChargesArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("RevenueTypeID_" + ChargesArray[c]).RawValue;
                    int.TryParse(strArray[0], out RevenueTypeID);
                }
                Charges.RevenueTypeID = RevenueTypeID;
                var RevenueType = entity.RevenueTypes.Where(d => d.RevenueTypeID == RevenueTypeID).FirstOrDefault();
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
                Charges.SalesRate = SalesRate;
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
                Charges.RevenueCode = RevenueType.RevenueCode;
                if (Jobgeneration.InvoiceNo > 0)
                {
                    Charges.CancelledInvoice = false;
                    Charges.InvoiceDate = DateTime.Now;
                    Charges.CancelReason = "";


                    var invoicenum = "";
                    if (invoiceNumber != null)
                    {
                        var strInvoice = invoiceNumber.Split('-');
                        var strinvoicenum = strInvoice[1].Split('/');
                        if (strinvoicenum.Count() > 1)
                        {
                            var invnum = Convert.ToInt32(strinvoicenum[1]) + 1;
                            invoicenum = "/" + invnum;
                            InvoiceNo = Convert.ToInt32(strinvoicenum[0]);
                        }
                        else
                        {
                            invoicenum = "/1";
                            InvoiceNo = Convert.ToInt32(strinvoicenum[0]);
                        }
                        //InvoiceNo = Convert.ToInt32(strInvoice[1]) + 1;
                    }
                    //InvoiceNo = InvoiceNo + 1;
                    Charges.InvoiceNumber = "JI-" + InvoiceNo;
                    Charges.InvoiceStatus = "1";
                }
                else
                {
                    Charges.CancelledInvoice = false;
                    Charges.InvoiceDate = null;
                    Charges.CancelReason = "";
                    Charges.InvoiceNumber = null;
                    Charges.InvoiceStatus = "0";
                }

                int iCharge = J.AddOrUpdateCharges(Charges, Session["UserID"].ToString());

            }
            if (Type == 2 && ChargesCount > 0)
            {
                JTimeLine Timeline = new JTimeLine();
                Timeline.JobId = JobId;
                Timeline.TabName = "Revenue Details";
                Timeline.ActionType = "Modified";
                Timeline.DateTime = DateTime.Now;
                Timeline.UserId = Convert.ToInt32(Session["UserID"]);
                Timeline.UserName = Session["UserName"].ToString();
                entity.JTimeLines.Add(Timeline);
                entity.SaveChanges();
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

                int CargoDescriptionID = 0;
                if (formCollection.GetValue("CargoDescriptionID_" + CargoArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("CargoDescriptionID_" + CargoArray[c]).RawValue;
                    int.TryParse(strArray[0].Trim(), out CargoDescriptionID);
                }
                Cargo.CargoDescriptionID = CargoDescriptionID;
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
                i = J.AddOrUpdateCargo(Cargo, Session["UserID"].ToString());
                if (Type == 2 && CargoCount > 0)
                {
                    JTimeLine Timeline = new JTimeLine();
                    Timeline.JobId = JobId;
                    Timeline.TabName = "Cargo";
                    Timeline.ActionType = "Modified";
                    Timeline.DateTime = DateTime.Now;
                    Timeline.UserId = Convert.ToInt32(Session["UserID"]);
                    Timeline.UserName = Session["UserName"].ToString();
                    entity.JTimeLines.Add(Timeline);
                    entity.SaveChanges();
                }
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
                int JContainerDetailID = 0;
                if (formCollection.GetValue("JContainerDetailID_" + ContainerArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("JContainerDetailID_" + ContainerArray[c]).RawValue;
                    int.TryParse(strArray[0].Trim(), out JContainerDetailID);
                }
                ContainerObj.JContainerDetailID = JContainerDetailID;

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

                AddOrUpdateContainerDetails(ContainerObj);
            }
            if (Type == 2 && ContainerCount > 0)
            {
                JTimeLine Timeline = new JTimeLine();
                Timeline.JobId = JobId;
                Timeline.TabName = "Container";
                Timeline.ActionType = "Modified";
                Timeline.DateTime = DateTime.Now;
                Timeline.UserId = Convert.ToInt32(Session["UserID"]);
                Timeline.UserName = Session["UserName"].ToString();
                entity.JTimeLines.Add(Timeline);
                entity.SaveChanges();
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

                int BIllOfEntryID = 0;
                if (formCollection.GetValue("BIllOfEntryID_" + BillOfEntryArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("BIllOfEntryID_" + BillOfEntryArray[c]).RawValue;
                    int.TryParse(strArray[0].Trim(), out BIllOfEntryID);
                }
                objBillOfEntry.BIllOfEntryID = BIllOfEntryID;

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
                    DateTime.TryParseExact(strArray[0].Trim(), new string[] { "dd-MMM-yyyy", "d-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out BIllOfEntryDate);
                    objBillOfEntry.BillofEntryDate = BIllOfEntryDate;
                }
                int ShippingAgentId = 0;
                if (formCollection.GetValue("ShippingAgentID_" + BillOfEntryArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("ShippingAgentID_" + BillOfEntryArray[c]).RawValue;
                    int.TryParse(strArray[0].Trim(), out ShippingAgentId);
                }
                objBillOfEntry.ShippingAgentID = ShippingAgentId;
                AddOrUpdateBill(objBillOfEntry);

            }
            if (Type == 2 && BillOfEntryCount > 0)
            {
                JTimeLine Timeline = new JTimeLine();
                Timeline.JobId = JobId;
                Timeline.TabName = "Bill of Entry";
                Timeline.ActionType = "Modified";
                Timeline.DateTime = DateTime.Now;
                Timeline.UserId = Convert.ToInt32(Session["UserID"]);
                Timeline.UserName = Session["UserName"].ToString();
                entity.JTimeLines.Add(Timeline);
                entity.SaveChanges();
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
                int JAuditLogID = 0;
                if (formCollection.GetValue("JAuditLogID_" + NotificationArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("JAuditLogID_" + NotificationArray[c]).RawValue;
                    int.TryParse(strArray[0].Trim(), out JAuditLogID);
                }
                objAudit.JAuditLogID = JAuditLogID;

                DateTime NotificationDate;
                if (formCollection.GetValue("AuditTransDate_" + NotificationArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("AuditTransDate_" + NotificationArray[c]).RawValue;
                    DateTime.TryParseExact(strArray[0].Trim(), new string[] { "dd-MMM-yyyy", "d-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out NotificationDate);
                    objAudit.TransDate = NotificationDate;
                }

                if (formCollection.GetValue("AuditRemarks_" + NotificationArray[c]) != null)
                {
                    strArray = (string[])formCollection.GetValue("AuditRemarks_" + NotificationArray[c]).RawValue;
                    objAudit.Remarks = strArray[0].Trim();
                }
                objAudit.JobID = JobId;
                AddOrUpdateALog(objAudit);

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
                var JTimeline = entity.JTimeLines.Where(d => d.JobId == id).ToList();
                foreach (var TL in JTimeline)
                {
                    entity.JTimeLines.Remove(TL);
                }
                entity.SaveChanges();
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


                i = J.AddOrUpdateCargo(Cargo, Session["UserID"].ToString());
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



                i = J.AddOrUpdateContainerDetails(Conta, Session["UserID"].ToString());
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

        public string AddOrUpdateALog(JAuditLog Audit)
        {
            int i = 0;
            if (Session["UserID"] != null)
            {
                i = J.AddOrUpdateAuditLog(Audit, Session["UserID"].ToString());
                var AddLog = (from t in entity.JAuditLogs orderby t.JAuditLogID descending select t).FirstOrDefault();
            }

            return i.ToString();
        }

        public string AddOrUpdateBill(JBIllOfEntry Bill)
        {
            int i = 0;
            if (Session["UserID"] != null)
            {

                i = J.AddOrUpdateBillOfEntry(Bill, Session["UserID"].ToString());
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

                //   if (Charges.InvoiceID <= 0)
                // {

                int i = J.AddOrUpdateCharges(Charges, Session["UserID"].ToString());
                //  }
                //   else
                //  {
                //      entity.Entry(Charges).State = EntityState.Modified;
                //    entity.SaveChanges();
                //  }

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

        public string AddOrUpdateContainerDetails(JContainerDetail ContainerDetail)
        {
            JContainerDetail jJContainerDetail = new JContainerDetail();
            jJContainerDetail = (from d in entity.JContainerDetails where d.JContainerDetailID == ContainerDetail.JContainerDetailID select d).FirstOrDefault();
            if (jJContainerDetail == null)
            {
                jJContainerDetail = new JContainerDetail();
            }
            if (jJContainerDetail.JContainerDetailID > 0)
            {
                jJContainerDetail.UserID = Convert.ToInt32(Session["UserID"].ToString());
                entity.Entry(jJContainerDetail).State = EntityState.Modified;
                entity.SaveChanges();
            }
            if (Session["UserID"] != null)
            {
                if (jJContainerDetail.JContainerDetailID <= 0)
                {
                    try
                    {
                        //int i = J.AddOrUpdateContainerDetails(ContainerDetail, Session["UserID"].ToString());
                        jJContainerDetail.ContainerNo = ContainerDetail.ContainerNo;
                        jJContainerDetail.ContainerTypeID = ContainerDetail.ContainerTypeID;
                        jJContainerDetail.Description = ContainerDetail.Description;
                        jJContainerDetail.JobID = ContainerDetail.JobID;
                        jJContainerDetail.SealNo = ContainerDetail.SealNo;
                        jJContainerDetail.UserID = Convert.ToInt32(Session["UserID"].ToString());
                        entity.JContainerDetails.Add(jJContainerDetail);
                        entity.SaveChanges();
                    }
                    catch
                    {

                    }
                }
                else
                {
                    entity.Entry(jJContainerDetail).State = EntityState.Modified;
                    entity.SaveChanges();
                    entity.Entry(jJContainerDetail).State = EntityState.Detached;
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
                var All_Invoices = new List<SP_GetChargesbyJobIDandUser_Result>();

                foreach (var item in AllInvoices)
                {
                    //var jinvoice = entity.JInvoices.Where(d => d.InvoiceID == item.InvoiceID).FirstOrDefault();
                    var invoices = (from d in entity.JInvoices where d.InvoiceID == item.InvoiceID select d).FirstOrDefault();
                    if (invoices.CancelledInvoice == false)
                    {
                        item.SalesRate = item.SalesHome / Convert.ToDecimal(item.Quantity);
                        item.InvoiceStatus = item.InvoiceStatus.Trim();
                        All_Invoices.Add(item);
                    }
                }
                return Json(All_Invoices, JsonRequestBehavior.AllowGet);
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
                var userid = Convert.ToInt32(Session["UserID"]);
                var allbill = entity.JBIllOfEntries.Where(d => d.JobID == JobID && d.UserID == userid).ToList();
                var Bill_list = new List<SP_GetBillsbyJobIDandUser_Result>();
                foreach (var item in allbill)
                {
                    var Billresult = new SP_GetBillsbyJobIDandUser_Result();
                    var agent = entity.ShippingAgents.Where(d => d.ShippingAgentID == item.ShippingAgentID).FirstOrDefault();
                    if (agent != null)
                    {
                        Billresult.AgentName = agent.AgentName;

                    }
                    Billresult.BIllOfEntry = item.BIllOfEntry;
                    Billresult.BIllOfEntryID = item.BIllOfEntryID;
                    Billresult.BillofEntryDate = item.BillofEntryDate;
                    Billresult.JobID = item.JobID;
                    Billresult.UserID = item.UserID;
                    Billresult.ShippingAgentID = item.ShippingAgentID;
                    Bill_list.Add(Billresult);

                }
                //var AllBill = J.GetBillByJob(JobID, Convert.ToInt32(Session["UserID"]));
                return Json(Bill_list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Failed = "failed";

                return Json(Failed, JsonRequestBehavior.AllowGet);
            }


        }

        public bool DeleteJobDetailsByJobID(int JobID, string InvoiceIds, string DeletedCargoIds, string DeletedContainerIds, string DeletedBillOfEntryIds, string DeletedAuditLogIDs)
        {
            J.DeleteJobDetailsByJobID(JobID, InvoiceIds, DeletedCargoIds, DeletedContainerIds, DeletedBillOfEntryIds, DeletedAuditLogIDs);
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

                //var AllConta = J.GetContainerByJob(JobID, Convert.ToInt32(Session["UserID"]));
                var Containers = (from d in entity.JContainerDetails where d.JobID == JobID select d).ToList();
                var Container_list = new List<SP_GetContainerDecbyJobIDandUser_Result>();
                foreach (var item in Containers)
                {
                    var Container = new SP_GetContainerDecbyJobIDandUser_Result();
                    Container.ContainerNo = item.ContainerNo;
                    var ContainerType = entity.ContainerTypes.Where(d => d.ContainerTypeID == item.ContainerTypeID).FirstOrDefault();
                    Container.ContainerType = ContainerType == null ? "Select" : ContainerType.ContainerType1;
                    Container.ContainerTypeID = item.ContainerTypeID;
                    Container.Description = item.Description;
                    Container.JContainerDetailID = item.JContainerDetailID;
                    Container.JobID = item.JobID;
                    Container.SealNo = item.SealNo;
                    Container.UserID = item.UserID;
                    Container_list.Add(Container);

                }
                return Json(Container_list, JsonRequestBehavior.AllowGet);
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

                List<SelectListItem> objBLStatusList = new List<SelectListItem>
                    { new SelectListItem() { Text = "SURRENDERED", Selected = false, Value = "SURRENDERED"}
                     , new SelectListItem() { Text = "WAY BILL", Selected = false, Value = "WAY BILL"}
                     , new SelectListItem() { Text = "OBL REQUIRED", Selected = false, Value = "OBL REQUIRED"}};
                List<SelectListItem> FreightList = new List<SelectListItem>
                    { new SelectListItem() { Text = "Prepaid", Selected = false, Value = "Prepaid"}
                     , new SelectListItem() { Text = "Destination", Selected = false, Value = "Destination"}
                     , new SelectListItem() { Text = "Collect", Selected = false, Value = "Collect"}};
                ViewBag.Freightlist = new SelectList(FreightList, "Value", "Text");

                ViewBag.BLStatusList = new SelectList(objBLStatusList, "Value", "Text");

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
        public JsonResult GetJobModeByJobType(int JobTypeID)
        {
            var data = (from d in entity.JobTypes where d.JobTypeID == JobTypeID select d).FirstOrDefault();
            var jobmode = (from d in entity.JobModes where d.JobModeID == data.JobModeID select d).FirstOrDefault();
            var isimportmode = false;
            var loadport = new List<Port>();
            var DestinationPort = new List<Port>();
            if (jobmode.JobMode1.ToLower() == "import")
            {
                isimportmode = true;
                var branchid = Convert.ToInt32(Session["branchid"]);
                var branchmaster = (from d in entity.BranchMasters where d.BranchID == branchid select d).FirstOrDefault();
                loadport = (from d in entity.Ports where d.CountryID != branchmaster.CountryID select d).ToList();
                DestinationPort = (from d in entity.Ports where d.CountryID == branchmaster.CountryID select d).ToList();
            }
            else
            {
                var branchid = Convert.ToInt32(Session["branchid"]);
                var branchmaster = (from d in entity.BranchMasters where d.BranchID == branchid select d).FirstOrDefault();
                loadport = (from d in entity.Ports where d.CountryID == branchmaster.CountryID select d).ToList();
                DestinationPort = (from d in entity.Ports where d.CountryID != branchmaster.CountryID select d).ToList();

                isimportmode = false;
            }
            return Json(new { isImport = isimportmode, loadport = loadport, DestinationPort = DestinationPort }, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetBaseCurrency()
        {
            var BaseCurrency = Session["BaseCurrencyId"].ToString();

            return Json(BaseCurrency, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetExchangeRte(string ID)
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            var branchid = Convert.ToInt32(Session["branchid"]);

            var branch = (from d in entity.BranchMasters where d.BranchID == branchid select d).FirstOrDefault();
            var ExRate = "";
            if (branch.CurrencyID == Convert.ToInt32(ID))
            {
                ExRate = "1.00";
            }
            else
            {
                ExRate = J.GetCurrencyExchange(Convert.ToInt32(ID));
                ExRate = Convert.ToDecimal(ExRate).ToString("0.00");
            }

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
        public JsonResult CloseJobsById(string selectedVal)
        {
            try
            {
                var Ids = selectedVal.Split(',').Select(Int32.Parse).ToList();
                foreach (var item in Ids)
                {
                    JobGeneration a = (from c in entity.JobGenerations where c.JobID == item select c).FirstOrDefault();
                    a.IsClosed = true;
                    a.StatusClose = true;

                    entity.Entry(a).State = EntityState.Modified;
                    entity.SaveChanges();
                    var JobId = item;
                    var data = (from c in entity.JobGenerations where c.JobID == JobId select c).FirstOrDefault();
                    int acjid = 0;
                    if (data.AcJournalID != null)
                    {
                        acjid = data.AcJournalID.Value;
                    }
                    else
                    {
                        var LatestJournal = entity.AcJournalMasters.ToList().LastOrDefault();
                        var JournalId = 1;
                        if (LatestJournal != null)
                        {
                            JournalId = LatestJournal.AcJournalID + 1;
                        }
                        var acjournalmaster = new AcJournalMaster();
                        acjournalmaster.AcFinancialYearID = Convert.ToInt32(Session["fyearid"]);
                        acjournalmaster.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"]);
                        acjournalmaster.VoucherType = "CI";
                        acjournalmaster.TransType = null;
                        acjournalmaster.TransDate = DateTime.Now;
                        acjournalmaster.UserID = Convert.ToInt32(Session["UserID"]);
                        acjournalmaster.AcJournalID = JournalId;
                        acjournalmaster.VoucherNo = data.JobCode;
                        entity.AcJournalMasters.Add(acjournalmaster);
                        entity.SaveChanges();
                        var job = (from d in entity.JobGenerations where d.JobID == JobId select d).FirstOrDefault();
                        job.AcJournalID = JournalId;
                        job.JobStatusId = 3;
                        entity.Entry(job).State = EntityState.Modified;
                        entity.SaveChanges();
                        acjid = JournalId;
                    }
                    int acprovjid = 0;
                    if (data.AcProvisionCostJournalID != null)
                    {
                        acprovjid = data.AcProvisionCostJournalID.Value;
                    }

                    decimal shome = 0;
                    decimal phome = 0;
                    if (entity.JInvoices.Where(x => x.JobID == JobId && x.CancelledInvoice == false).Count() > 0)
                    {
                        shome = entity.JInvoices.Where(x => x.JobID == JobId && x.CancelledInvoice == false).Sum(x => x.SalesHome ?? 0);
                        phome = entity.JInvoices.Where(x => x.JobID == JobId && x.CancelledInvoice == false).Sum(x => x.ProvisionHome ?? 0);
                    }
                    var jinvoices = entity.JInvoices.Where(x => x.JobID == JobId && x.CancelledInvoice == false).ToList();
                    foreach (var items in jinvoices)
                    {
                        var Rev_achead = entity.RevenueTypes.Where(d => d.RevenueTypeID == items.RevenueTypeID).FirstOrDefault();
                        int acdet = (from x in entity.AcJournalDetails where x.AcJournalID == acjid && x.AcHeadID == Rev_achead.AcHeadID select x.AcJournalDetailID).FirstOrDefault();
                        var acdata = (from x in entity.AcJournalDetails where x.AcJournalDetailID == acdet select x).FirstOrDefault();
                        if (acdata != null)
                        {
                            acdata.Amount = items.SalesHome;
                            entity.Entry(acdata).State = EntityState.Modified;
                            entity.SaveChanges();
                        }
                        else
                        {
                            var acJournalDetailid = entity.AcJournalDetails.ToList().LastOrDefault();
                            var acjournalDet_id = 1;
                            if (acJournalDetailid != null)
                            {
                                acjournalDet_id = acJournalDetailid.AcJournalDetailID + 1;
                            }
                            acdata = new AcJournalDetail();
                            acdata.AcHeadID = Rev_achead.AcHeadID;
                            acdata.AcJournalID = acjid;
                            acdata.Amount = items.SalesHome;
                            acdata.AcJournalDetailID = acjournalDet_id;
                            entity.AcJournalDetails.Add(acdata);
                            entity.SaveChanges();


                        }
                    }


                    var pagecontrol = (from d in entity.PageControlMasters where d.ControlName.ToLower() == "generate invoice" select d).FirstOrDefault();
                    var accountsetup = (from d in entity.AcHeadControls where d.Pagecontrol == pagecontrol.Id select d).ToList();

                    //int custcontrolacid = (from c in entity.AcHeadAssigns select c.CustomerControlAcID.Value).FirstOrDefault();
                    ////int freightacheadid = 158;
                    ////int provcontrolacid = (from c in entity.AcHeadAssigns select c.ProvisionCostControlAcID.Value).FirstOrDefault();
                    ////int accruedcontrolacid = (from c in entity.AcHeadAssigns select c.AccruedCostControlAcID.Value).FirstOrDefault();
                    foreach (var acsetup in accountsetup)
                    {
                        var acjdetail1 = (from x in entity.AcJournalDetails where x.AcJournalID == acjid && x.AcHeadID == acsetup.AccountHeadID select x).FirstOrDefault();
                        int acjdetail2 = 0;
                        decimal amount = 0;
                        if (acsetup.AccountName.ToLower() == "cost total")
                        {
                            if (acsetup.AccountNature == true)
                            {
                                amount = shome * -1;
                            }
                            else
                            {
                                amount = shome;
                            }
                        }
                        else if (acsetup.AccountName.ToLower() == "sales total")
                        {
                            if (acsetup.AccountNature == true)
                            {
                                amount = phome * -1;
                            }
                            else
                            {
                                amount = phome;
                            }
                        }

                        if (acjdetail1 != null)
                        {
                            acjdetail2 = acjdetail1.AcJournalDetailID;
                        }
                        var data1 = (from x in entity.AcJournalDetails where x.AcJournalDetailID == acjdetail2 select x).FirstOrDefault();
                        if (data1 != null)
                        {
                            data1.Amount = amount;
                            entity.Entry(data1).State = EntityState.Modified;
                            entity.SaveChanges();
                        }
                        else
                        {
                            var acJournalDetailid = entity.AcJournalDetails.ToList().LastOrDefault();
                            var acjournalDet_id = 1;
                            if (acJournalDetailid != null)
                            {
                                acjournalDet_id = acJournalDetailid.AcJournalDetailID + 1;
                            }

                            data1 = new AcJournalDetail();
                            data1.AcHeadID = acsetup.AccountHeadID;
                            data1.AcJournalID = acjid;
                            data1.Amount = amount;
                            data1.AcJournalDetailID = acjournalDet_id;
                            entity.AcJournalDetails.Add(data1);
                            entity.SaveChanges();

                        }
                    }
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(new { success = false,message=e.Message.ToString() }, JsonRequestBehavior.AllowGet);

            }
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




        public ActionResult GetJob(DateTime fdate, DateTime tdate, int jobstatus)
        {

            var data = entity.SP_GetAllJobsDetailsByDate(Convert.ToDateTime(fdate), Convert.ToDateTime(tdate)).ToList();

            //var data1=entity.spgetall

            //from JobGeneration j left outer join CUSTOMER C on j.ConsignerID = c.CustomerID
            //left outer join CUSTOMER C1 on j.ConsigneeID = C1.CustomerID
            //left outer join JobType jt on j.JobTypeID = jt.JobTypeID
            //left outer join Employees E on j.EmployeeID = E.EmployeeID
            //where j.CostUpdatedOrNot = 'N' and JobStatus = 1
            var datas = new List<JobRegisterVM>();
            if (jobstatus == 0)
            {
                datas = (from j in entity.JobGenerations
                         join c in entity.CUSTOMERs on j.ConsignerID equals c.CustomerID
                         join c1 in entity.CUSTOMERs on j.ConsigneeID equals c1.CustomerID
                         join jt in entity.JobTypes on j.JobTypeID equals jt.JobTypeID
                         //join E in entity.Employees on j.EmployeeID equals E.EmployeeID
                         join L in entity.Ports on j.LoadPortID equals L.PortID
                         join D in entity.Ports on j.DestinationPortID equals D.PortID
                         join s in entity.JobStatus on j.JobStatusId equals s.JobStatusId
                         where j.CostUpdatedOrNot == "N"
                         select new JobRegisterVM
                         {
                             InvoiceNo = j.InvoiceNo,
                             JobID = j.JobID,
                             InvoiceDate = j.InvoiceDate,
                             JobDate = j.JobDate,
                             Description = jt.JobDescription,
                             JobCode = j.JobCode,
                             CostUpdatedOrNot = j.CostUpdatedOrNot,
                             ShipperName = c.Customer1,
                             ConsigneeName = c1.Customer1,
                             Customer = c.Customer1,
                             LoadPort = L.Port1,
                             DestinationPort = D.Port1,
                             Job_Status = s.StatusName


                         }).ToList();
            }
            else
            {

                datas = (from j in entity.JobGenerations
                         join c in entity.CUSTOMERs on j.ConsignerID equals c.CustomerID
                         join c1 in entity.CUSTOMERs on j.ConsigneeID equals c1.CustomerID
                         join jt in entity.JobTypes on j.JobTypeID equals jt.JobTypeID
                         //join E in entity.Employees on j.EmployeeID equals E.EmployeeID
                         join L in entity.Ports on j.LoadPortID equals L.PortID
                         join D in entity.Ports on j.DestinationPortID equals D.PortID
                         join s in entity.JobStatus on j.JobStatusId equals s.JobStatusId
                         where j.CostUpdatedOrNot == "N" && j.JobStatusId == jobstatus
                         select new JobRegisterVM
                         {
                             InvoiceNo = j.InvoiceNo,
                             JobID = j.JobID,
                             InvoiceDate = j.InvoiceDate,
                             JobDate = j.JobDate,
                             Description = jt.JobDescription,
                             JobCode = j.JobCode,
                             CostUpdatedOrNot = j.CostUpdatedOrNot,
                             ShipperName = c.Customer1,
                             ConsigneeName = c1.Customer1,
                             Customer = c.Customer1,
                             LoadPort = L.Port1,
                             DestinationPort = D.Port1,
                             Job_Status = s.StatusName

                         }).ToList();
            }
            datas.ForEach(d => d.InvoiceNumber = (from i in entity.JInvoices where i.JobID == d.JobID && i.CancelledInvoice == false select i).FirstOrDefault() == null ? "" : (from i in entity.JInvoices where i.JobID == d.JobID && i.CancelledInvoice == false select i).FirstOrDefault().InvoiceNumber);
            string view = this.RenderPartialView("_GetJob", datas);

            return new JsonResult
            {
                Data = new
                {
                    success = true,
                    view = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
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

            var Sdate = Convert.ToDateTime(fdate);
            var Edate = Convert.ToDateTime(tdate);
            var OpenedJob = (from j in entity.JobGenerations
                             join c in entity.CUSTOMERs on j.ConsignerID equals c.CustomerID
                             join c1 in entity.CUSTOMERs on j.ConsigneeID equals c1.CustomerID
                             join jt in entity.JobTypes on j.JobTypeID equals jt.JobTypeID
                             //join E in entity.Employees on j.EmployeeID equals E.EmployeeID
                             join L in entity.Ports on j.LoadPortID equals L.PortID
                             join D in entity.Ports on j.DestinationPortID equals D.PortID
                             join s in entity.JobStatus on j.JobStatusId equals s.JobStatusId
                             where j.CostUpdatedOrNot == "N" && j.JobDate >= Sdate && j.JobDate <= Edate && (j.IsClosed == false || j.IsClosed == null) && (j.StatusClose == false || j.StatusClose == null)
                             select new JobRegisterVM
                             {
                                 InvoiceNo = j.InvoiceNo,
                                 JobID = j.JobID,
                                 InvoiceDate = j.InvoiceDate,
                                 JobDate = j.JobDate,
                                 Description = jt.JobDescription,
                                 JobCode = j.JobCode,
                                 CostUpdatedOrNot = j.CostUpdatedOrNot,
                                 ShipperName = c.Customer1,
                                 ConsigneeName = c1.Customer1,
                                 Customer = c.Customer1,
                                 LoadPort = L.Port1,
                                 DestinationPort = D.Port1,
                                 Job_Status = s.StatusName,


                             }).ToList();
            foreach (var item in OpenedJob)
            {
                var Jinvoices = (from c in entity.JInvoices where c.JobID == item.JobID select c).ToList();
                item.InvoiceNumber = Jinvoices.FirstOrDefault().InvoiceNumber;
                item.ProvisionHome = Convert.ToDecimal(String.Format("{0:0.00}", Jinvoices.Sum(x => x.ProvisionHome)));
                item.Sales = Convert.ToDecimal(String.Format("{0:0.00}", Jinvoices.Sum(x => x.SalesHome)));
                item.ProvExRate = Convert.ToDecimal(String.Format("{0:0.00}", Jinvoices.Sum(x => x.SalesHome) - Jinvoices.Sum(x => x.ProvisionHome)));
            }
            //var open = entity.SP_GetAllClosedJobsDetailsByDate(Convert.ToDateTime(fdate), Convert.ToDateTime(tdate), FYearID).ToList();

            string view = this.RenderPartialView("ucClosedJob", OpenedJob);
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
        public JsonResult UpdateJobStatus(int Jobid, int statusid)
        {
            try
            {
                var job = (from d in entity.JobGenerations where d.JobID == Jobid select d).FirstOrDefault();
                job.JobStatusId = statusid;
                entity.Entry(job).State = EntityState.Modified;
                entity.SaveChanges();
                var jobstatus = new JStatu();
                jobstatus.JobId = Jobid;
                jobstatus.EmployeeId = Convert.ToInt32(Session["UserID"]);
                jobstatus.DateTime = DateTime.Now;
                jobstatus.JobStatusId = statusid;
                entity.JStatus.Add(jobstatus);
                entity.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message.ToString() }, JsonRequestBehavior.AllowGet);

            }
        }
        public JsonResult UpdateStaffNote(int Jobid, string staffnote)
        {
            try
            {
                var note = new StaffNote();
                note.Datetime = DateTime.Now;
                note.JobId = Jobid;
                note.TaskDetails = staffnote;
                note.PageTypeId = 1;//job 
                note.EmployeeId = Convert.ToInt32(Session["UserID"]);
                entity.StaffNotes.Add(note);
                entity.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message.ToString() }, JsonRequestBehavior.AllowGet);

            }
        }
        public JsonResult SendCustomerNotification(int JobId, string Message, int Customerid, bool whatsapp, bool Email, bool sms)
        {
            var customer = (from d in entity.CUSTOMERs where d.CustomerID == Customerid select d).FirstOrDefault();
            var isemail = false;
            var issms = false;
            var iswhatsapp = false;
            if (Email)
            {
                try
                {
                    var status = SendMailForCustomerNotification(customer.Customer1, Message, customer.Email);
                    isemail = true;
                }
                catch { }
            }
            if (sms)
            {
                issms = true;
            }
            if (whatsapp)
            {
                iswhatsapp = true;
            }
            try
            {
                UpdateCustomerNotification(JobId, Message, isemail, issms, iswhatsapp);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message.ToString() }, JsonRequestBehavior.AllowGet);

            }
        }

        public string SendMailForCustomerNotification(string UserName, string Message, string Email)
        {
            var Success = "False";
            System.IO.StreamReader objReader;
            objReader = new System.IO.StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("/Templates/CustomerNotification.html"));
            string content = objReader.ReadToEnd();


            objReader.Close();
            content = Regex.Replace(content, "@username", UserName);
            content = Regex.Replace(content, "@Message", Message);
            try
            {
                using (MailMessage msgMail = new MailMessage())
                {

                    msgMail.From = new MailAddress(ConfigurationManager.AppSettings["FromEmailAddress"].ToString());
                    msgMail.Subject = "Shipping System";
                    msgMail.IsBodyHtml = true;
                    msgMail.Body = HttpUtility.HtmlDecode(content);
                    msgMail.To.Add(Email);
                    msgMail.IsBodyHtml = true;

                    //client = new SmtpClient(ConfigurationManager.AppSettings["Host"].ToString());
                    //client.Port = int.Parse(ConfigurationManager.AppSettings["SMTPServerPort"].ToString());
                    //client.UseDefaultCredentials = false;
                    //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTPUserName"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString());
                    //client.EnableSsl = true;
                    //client.Send(msgMail);
                    using (SmtpClient smtp = new SmtpClient("smtp.mail.yahoo.com", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTPUserName"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString());
                        smtp.EnableSsl = true;
                        smtp.Send(msgMail);
                    }
                }
                Success = "True";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return Success;
        }
        public JsonResult UpdateCustomerNotification(int Jobid, string Messge, bool isemail, bool issms, bool iswhatsapp)
        {
            try
            {
                var note = new CustomerNotification();
                note.Datetime = DateTime.Now;
                note.JobId = Jobid;
                note.Message = Messge;
                note.PageTypeId = 1;//job 
                note.StaffId = Convert.ToInt32(Session["UserID"]);
                note.IsEmail = isemail;
                note.IsSms = issms;
                note.IsWhatsapp = iswhatsapp;
                entity.CustomerNotifications.Add(note);
                entity.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message.ToString() }, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult JobInvoice(int ID)
        {
            List<SP_GetAllJobsDetails_Result> AllJobs = new List<SP_GetAllJobsDetails_Result>();

            List<JInvoice> AllJobInvoices = new List<JInvoice>();

            AllJobInvoices = (from t in AllJobInvoices where (t.InvoiceDate >= Convert.ToDateTime(Session["FyearFrom"]) && t.InvoiceDate <= Convert.ToDateTime(Session["FyearTo"])) select t).ToList();
            var data = (from t in AllJobs where (t.InvoiceDate >= Convert.ToDateTime(Session["FyearFrom"]) && t.InvoiceDate <= Convert.ToDateTime(Session["FyearTo"])) select t).ToList();

            return View(AllJobInvoices);
        }
        public ActionResult GetJobInvoice(DateTime fdate, DateTime tdate, int InvoiceStatus)
        {


            var data = new List<JobInvoiceModel>();
            DateTime a = Convert.ToDateTime(Session["FyearFrom"]);
            DateTime b = Convert.ToDateTime(Session["FyearTo"]);
            List<JInvoice> JobInvoice = new List<JInvoice>();

            var dt = (from d in entity.JInvoices
                      join s in entity.Suppliers on d.SupplierID equals s.SupplierID
                      join j in entity.JobGenerations on d.JobID equals j.JobID
                      where (d.InvoiceDate >= a && d.InvoiceDate <= b) && d.InvoiceStatus == "1"
                      select new { d = d, s = s, j = j }).ToList();

            var result = dt.GroupBy(g => new { g.d.InvoiceNumber })
                          .Select(g => g.First())
                          .ToList();

            foreach (var item in result)
            {
                var amount = entity.JInvoices.Where(d => d.JobID == item.j.JobID && d.CancelledInvoice == false).ToList();
                var invoice = new JobInvoiceModel();
                invoice.Id = item.d.InvoiceID;
                invoice.JobID = item.d.JobID;
                invoice.InvoiceDate = item.d.InvoiceDate;
                invoice.Amount = amount.Sum(d => d.SalesHome);
                invoice.InvoiceNo = item.d.InvoiceNumber;
                invoice.InvoiceStatus = item.d.InvoiceStatus;
                invoice.PaymentStatus = item.d.CostUpdationStatus;
                invoice.JobNumber = item.j.JobCode;
                invoice.Supplier = item.s.SupplierName;
                invoice.IsCancelledInvoice = item.d.CancelledInvoice;
                data.Add(invoice);
            }

            //var d1 = datas.GroupBy(x => x.job).Select(y => y.First());
            if (InvoiceStatus == 1)
            {
                data = data.Where(d => d.IsCancelledInvoice == false).ToList();
            }
            else if (InvoiceStatus == 2)
            {
                data = data.Where(d => d.IsCancelledInvoice == true).ToList();

            }


            string view = this.RenderPartialView("_GetJobInvoice", data);

            return new JsonResult
            {
                Data = new
                {
                    success = true,
                    view = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }
        public JsonResult UpdateJobInvoiceCancel(string JobInvoiceid, string CancelReason)
        {
            try
            {
                var JobInvoice = entity.JInvoices.Where(d => d.InvoiceNumber == JobInvoiceid).ToList();
                JobInvoice.ForEach(d => { d.CancelledInvoice = true; d.CancelReason = CancelReason; });
                entity.SaveChanges();
                var JobId = JobInvoice.FirstOrDefault().JobID;
                var jobinvoice = (from d in entity.JobGenerations where d.JobID == JobId select d).FirstOrDefault();
                jobinvoice.InvoiceNo = 0;
                jobinvoice.AcJournalID = 0;
                entity.Entry(jobinvoice).State = EntityState.Modified;
                entity.SaveChanges();
                var jobcode = entity.JobGenerations.Where(d => d.JobID == JobId).FirstOrDefault();
                var acjournal = entity.AcJournalMasters.Where(d => d.VoucherNo == jobcode.JobCode).FirstOrDefault();
                if (acjournal != null)
                {
                    var acjournaldetails = entity.AcJournalDetails.Where(d => d.AcJournalID == acjournal.AcJournalID).ToList();
                    entity.AcJournalMasters.Remove(acjournal);
                    entity.SaveChanges();
                    foreach (var item in acjournaldetails)
                    {
                        entity.AcJournalDetails.Remove(item);
                        entity.SaveChanges();
                    }
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult RegenerateInvoice(int JobId)
        {
            try
            {
                var InvoiceNo = 10000;
                var JobInvoice = entity.JInvoices.Where(d => d.JobID == JobId).ToList();
                JobInvoice.ForEach(d => { d.CancelledInvoice = true; d.CancelReason = "Regenerate invoice"; });
                entity.SaveChanges();
                var JobInvoice1 = entity.JInvoices.Where(d => d.JobID == JobId && d.CancelledInvoice == true).ToList();
                var RecentInvoice = JobInvoice1.LastOrDefault().InvoiceNumber;

                //var result = JobInvoice.GroupBy(g => new { g.InvoiceNumber })
                //        .Select(g => g.FirstOrDefault())
                //        .ToList();
                var result = entity.JInvoices.Where(d => d.InvoiceNumber == RecentInvoice).ToList();
                var invoice = entity.JInvoices.ToList().LastOrDefault();
                var invoiceid = 1;
                if (invoice != null)
                {
                    invoiceid = invoice.InvoiceID + 1;
                }
                var invoiceNumber = entity.JInvoices.Select(d => d.InvoiceNumber).ToList().LastOrDefault();
                var invoicenum = "";

                if (invoiceNumber != null)
                {
                    var strInvoice = invoiceNumber.Split('-');
                    var strinvoicenum = strInvoice[1].Split('/');
                    if (strinvoicenum.Count() > 1)
                    {
                        var invnum = Convert.ToInt32(strinvoicenum[1]) + 1;
                        invoicenum = "/" + invnum;
                        InvoiceNo = Convert.ToInt32(strinvoicenum[0]);
                    }
                    else
                    {
                        invoicenum = "/1";
                        InvoiceNo = Convert.ToInt32(strinvoicenum[0]);
                    }


                }
                foreach (var item in result)
                {
                    var newinvoice = new JInvoice();
                    //newinvoice = item;
                    //newinvoice.InvoiceID = invoiceid;
                    newinvoice.AmtInWords = item.AmtInWords;
                    newinvoice.Cost = item.Cost;
                    newinvoice.CostUpdationStatus = item.CostUpdationStatus;
                    newinvoice.Description = item.Description;
                    newinvoice.InvoiceStatus = item.InvoiceStatus;
                    newinvoice.JobID = item.JobID;
                    newinvoice.Lock = item.Lock;
                    newinvoice.Margin = item.Margin;
                    newinvoice.PreInvID = item.PreInvID;
                    newinvoice.ProvisionCurrencyID = item.ProvisionCurrencyID;
                    newinvoice.ProvisionExchangeRate = item.ProvisionExchangeRate;
                    newinvoice.ProvisionForeign = item.ProvisionForeign;
                    newinvoice.ProvisionHome = item.ProvisionHome;
                    newinvoice.ProvisionQty = item.ProvisionQty;
                    newinvoice.ProvisionRate = item.ProvisionRate;
                    newinvoice.Quantity = item.Quantity;
                    newinvoice.RevenueCode = item.RevenueCode;
                    newinvoice.RevenueTypeID = item.RevenueTypeID;
                    newinvoice.SalesCurrencyID = item.SalesCurrencyID;
                    newinvoice.SalesExchangeRate = item.SalesExchangeRate;
                    newinvoice.SalesForeign = item.SalesForeign;
                    newinvoice.SalesHome = item.SalesHome;
                    newinvoice.SalesQty = item.SalesQty;
                    newinvoice.SalesRate = item.SalesRate;
                    newinvoice.SupplierID = item.SupplierID;
                    newinvoice.Tax = item.Tax;
                    newinvoice.TaxAmount = item.TaxAmount;
                    newinvoice.tempInvID = item.tempInvID;
                    newinvoice.UnitID = item.UnitID;
                    newinvoice.UserID = item.UserID;
                    newinvoice.CancelledInvoice = false;
                    newinvoice.InvoiceDate = DateTime.Now;
                    newinvoice.CancelReason = "";

                    //InvoiceNo = InvoiceNo + 1;
                    newinvoice.InvoiceNumber = "JI-" + InvoiceNo + invoicenum;
                    //entity.JInvoices.Add(newinvoice);
                    //entity.SaveChanges();
                    int iCharge = J.AddOrUpdateCharges(newinvoice, Session["UserID"].ToString());

                    invoiceid = invoiceid + 1;
                }
                var jobinvoice = (from d in entity.JobGenerations where d.JobID == JobId select d).FirstOrDefault();
                jobinvoice.InvoiceNo = 1;
                entity.Entry(jobinvoice).State = EntityState.Modified;
                entity.SaveChanges();

                var data = (from c in entity.JobGenerations where c.JobID == JobId select c).FirstOrDefault();
                int acjid = 0;
                if (data.AcJournalID != null && data.AcJournalID > 0)
                {
                    acjid = data.AcJournalID.Value;
                }
                else
                {
                    var LatestJournal = entity.AcJournalMasters.ToList().LastOrDefault();
                    var JournalId = 1;
                    if (LatestJournal != null)
                    {
                        JournalId = LatestJournal.AcJournalID + 1;
                    }
                    var job = (from d in entity.JobGenerations where d.JobID == JobId select d).FirstOrDefault();

                    var acjournalmaster = new AcJournalMaster();
                    acjournalmaster.AcFinancialYearID = Convert.ToInt32(Session["fyearid"]);
                    acjournalmaster.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"]);
                    acjournalmaster.VoucherType = "CI";
                    acjournalmaster.TransType = null;
                    acjournalmaster.TransDate = DateTime.Now;
                    acjournalmaster.UserID = Convert.ToInt32(Session["UserID"]);
                    acjournalmaster.AcJournalID = JournalId;
                    acjournalmaster.VoucherNo = job.JobCode;
                    entity.AcJournalMasters.Add(acjournalmaster);
                    entity.SaveChanges();
                    job.AcJournalID = JournalId;
                    job.JobStatusId = 3;
                    entity.Entry(job).State = EntityState.Modified;
                    entity.SaveChanges();
                    acjid = JournalId;
                }
                int acprovjid = 0;
                if (data.AcProvisionCostJournalID != null)
                {
                    acprovjid = data.AcProvisionCostJournalID.Value;
                }

                decimal shome = 0;
                decimal phome = 0;
                if (entity.JInvoices.Where(x => x.JobID == JobId && x.CancelledInvoice == false).Count() > 0)
                {
                    shome = entity.JInvoices.Where(x => x.JobID == JobId && x.CancelledInvoice == false).Sum(x => x.SalesHome ?? 0);
                    phome = entity.JInvoices.Where(x => x.JobID == JobId && x.CancelledInvoice == false).Sum(x => x.ProvisionHome ?? 0);
                }
                var jinvoices = entity.JInvoices.Where(x => x.JobID == JobId && x.CancelledInvoice == false).ToList();
                foreach (var item in jinvoices)
                {
                    var Rev_achead = entity.RevenueTypes.Where(d => d.RevenueTypeID == item.RevenueTypeID).FirstOrDefault();
                    int acdet = (from x in entity.AcJournalDetails where x.AcJournalID == acjid && x.AcHeadID == Rev_achead.AcHeadID select x.AcJournalDetailID).FirstOrDefault();
                    var acdata = (from x in entity.AcJournalDetails where x.AcJournalDetailID == acdet select x).FirstOrDefault();
                    if (acdata != null)
                    {
                        acdata.Amount = item.SalesHome;
                        entity.Entry(acdata).State = EntityState.Modified;
                        entity.SaveChanges();
                    }
                    else
                    {
                        var acJournalDetailid = entity.AcJournalDetails.ToList().LastOrDefault();
                        var acjournalDet_id = 1;
                        if (acJournalDetailid != null)
                        {
                            acjournalDet_id = acJournalDetailid.AcJournalDetailID + 1;
                        }
                        acdata = new AcJournalDetail();
                        acdata.AcHeadID = Rev_achead.AcHeadID;
                        acdata.AcJournalID = acjid;
                        acdata.Amount = item.SalesHome;
                        acdata.AcJournalDetailID = acjournalDet_id;
                        entity.AcJournalDetails.Add(acdata);
                        entity.SaveChanges();


                    }
                }


                var pagecontrol = (from d in entity.PageControlMasters where d.ControlName.ToLower() == "generate invoice" select d).FirstOrDefault();
                var accountsetup = (from d in entity.AcHeadControls where d.Pagecontrol == pagecontrol.Id select d).FirstOrDefault();

                //int custcontrolacid = (from c in entity.AcHeadAssigns select c.CustomerControlAcID.Value).FirstOrDefault();
                ////int freightacheadid = 158;
                ////int provcontrolacid = (from c in entity.AcHeadAssigns select c.ProvisionCostControlAcID.Value).FirstOrDefault();
                ////int accruedcontrolacid = (from c in entity.AcHeadAssigns select c.AccruedCostControlAcID.Value).FirstOrDefault();

                var acjdetail1 = (from x in entity.AcJournalDetails where x.AcJournalID == acjid && x.AcHeadID == accountsetup.AccountHeadID select x).FirstOrDefault();
                int acjdetail2 = 0;
                if (acjdetail1 != null)
                {
                    acjdetail2 = acjdetail1.AcJournalDetailID;
                }
                var data1 = (from x in entity.AcJournalDetails where x.AcJournalDetailID == acjdetail2 select x).FirstOrDefault();
                if (data1 != null)
                {
                    data1.Amount = -shome;
                    entity.Entry(data1).State = EntityState.Modified;
                    entity.SaveChanges();
                }
                else
                {
                    var acJournalDetailid = entity.AcJournalDetails.ToList().LastOrDefault();
                    var acjournalDet_id = 1;
                    if (acJournalDetailid != null)
                    {
                        acjournalDet_id = acJournalDetailid.AcJournalDetailID + 1;
                    }

                    data1 = new AcJournalDetail();
                    data1.AcHeadID = accountsetup.AccountHeadID;
                    data1.AcJournalID = acjid;
                    data1.Amount = -shome;
                    data1.AcJournalDetailID = acjournalDet_id;
                    entity.AcJournalDetails.Add(data1);
                    entity.SaveChanges();

                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult JobInvoiceReport(int jobid)
        {
            var job = entity.JobGenerations.Where(d => d.JobID == jobid).FirstOrDefault();
            var jobinvoices = entity.SP_GetInvoiceReport(jobid).ToList();
            var company = entity.AcCompanies.FirstOrDefault();
            int uid = Convert.ToInt32(Session["UserID"].ToString());
            var user = (from c in entity.UserRegistrations where c.UserID == uid select c.UserName).FirstOrDefault();
            var Invoices = entity.JInvoices.Where(d => d.JobID == jobid && d.CancelledInvoice == false && d.InvoiceStatus == "1").ToList();
            var consignee = entity.CUSTOMERs.Where(d => d.CustomerID == job.ConsigneeID).FirstOrDefault();
            var consigner = entity.CUSTOMERs.Where(d => d.CustomerID == job.ConsignerID).FirstOrDefault();
            ViewBag.Job = job;
            ViewBag.Jobinvoices = jobinvoices;
            ViewBag.Invoices = Invoices;
            ViewBag.Company = company;
            ViewBag.User = user;
            ViewBag.Consignee = consignee;
            ViewBag.Consigner = consigner;
            return View();
        }
        [HttpGet]
        public ActionResult JobEnquiry(int id)
        {
            var JobEnquiry = entity.JobEnquiries.Where(d => d.Id == id).FirstOrDefault();
            var CreditNotes = entity.JobEnquiries.ToList().LastOrDefault();
            var EnquiryNo = 1;
            if (CreditNotes != null)
            {
                var CreditNumstr = CreditNotes.EnquiryNo.Split('-');
                if (CreditNumstr.Count() > 1)
                {
                    EnquiryNo = Convert.ToInt32(CreditNumstr[1]) + 1;
                }
            }
            var CreditNumber = EnquiryNo.ToString("0000");
            var datetime = DateTime.Now;
            var CreditnoteNum = datetime.ToString("ddMMyyyy") + "-" + CreditNumber;
            if (JobEnquiry == null)
            {
                JobEnquiry = new JobEnquiry();
                JobEnquiry.EnquiryDate = DateTime.Now;
                JobEnquiry.EnquiryNo = CreditnoteNum;
            }
            return View(JobEnquiry);

        }
        [HttpPost]
        public ActionResult JobEnquiry(JobEnquiry JE)
        {
            var JobEnquiry = entity.JobEnquiries.Where(d => d.Id == JE.Id).FirstOrDefault();
            if (JobEnquiry == null)
            {
                JobEnquiry = new JobEnquiry();
            }
            try
            {

                JobEnquiry.InvoiceTo = JE.InvoiceTo;
                JobEnquiry.BranchId = Convert.ToInt32(Session["branchid"]);
                JobEnquiry.Consignee = JE.Consignee;
                JobEnquiry.CreatedDate = DateTime.Now;
                JobEnquiry.DeliveryPlace = JE.DeliveryPlace;
                JobEnquiry.DestinationPort = JE.DestinationPort;
                JobEnquiry.EnquiryDate = JE.EnquiryDate;
                JobEnquiry.EnquiryNo = JE.EnquiryNo;
                JobEnquiry.EnquiryType = JE.EnquiryType;
                JobEnquiry.InvoiceTo = JE.InvoiceTo;
                JobEnquiry.LoadPort = JE.LoadPort;
                JobEnquiry.NotifyTo = JE.NotifyTo;
                JobEnquiry.ReceiptPlace = JE.ReceiptPlace;
                JobEnquiry.Remarks = JE.Remarks;
                JobEnquiry.Shipper = JE.Shipper;
                JobEnquiry.UserId = Convert.ToInt32(Session["UserID"].ToString());
                if (JobEnquiry.Id == 0)
                {
                    entity.JobEnquiries.Add(JobEnquiry);
                }
                entity.SaveChanges();
                return RedirectToAction("JobEnquiryList", "Job", new { ID = JobEnquiry.Id });
            }
            catch
            {
                return View(JobEnquiry);
            }

        }
        public ActionResult JobEnquiryList(int ID)
        {
            List<JobEnquiry> AllJobs = new List<JobEnquiry>();
            var branchid = Convert.ToInt32(Session["branchid"]);
            AllJobs = entity.JobEnquiries.Where(d => d.BranchId == branchid).ToList();
            DateTime a = Convert.ToDateTime(Session["FyearFrom"]);
            DateTime b = Convert.ToDateTime(Session["FyearTo"]);
            var data = (from t in AllJobs where (t.EnquiryDate >= Convert.ToDateTime(Session["FyearFrom"]) && t.EnquiryDate <= Convert.ToDateTime(Session["FyearTo"])) select t).ToList();
            if (ID > 0)
            {
                ViewBag.SuccessMsg = "You have successfully added Enquiry.";
            }
            if (ID == 10)
            {
                ViewBag.SuccessMsg = "You have successfully deleted Enquiry.";
            }
            var EnquiryType = new string[] { "", "Telephone", "Email", "Walk-in", "Customer Login" };
            data.ForEach(d => d.EnquiryType = EnquiryType[Convert.ToInt32(d.EnquiryType)]);

            return View(data);
        }
        public ActionResult GetJobEnquiry(DateTime fdate, DateTime tdate)
        {

            List<JobEnquiry> AllJobs = new List<JobEnquiry>();
            var branchid = Convert.ToInt32(Session["branchid"]);
            AllJobs = entity.JobEnquiries.Where(d => d.BranchId == branchid).ToList();
            DateTime a = Convert.ToDateTime(Session["FyearFrom"]);
            DateTime b = Convert.ToDateTime(Session["FyearTo"]);
            var data = (from t in AllJobs where (t.EnquiryDate >= Convert.ToDateTime(Session["FyearFrom"]) && t.EnquiryDate <= Convert.ToDateTime(Session["FyearTo"])) select t).ToList();

            var EnquiryType = new string[] { "", "Telephone", "Email", "Walk-in", "Customer Login" };
            data.ForEach(d => d.EnquiryType = EnquiryType[Convert.ToInt32(d.EnquiryType)]);

            string view = this.RenderPartialView("_GetJobEnquiry", data);

            return new JsonResult
            {
                Data = new
                {
                    success = true,
                    view = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }
        [HttpGet]
        public ActionResult DeleteJobEnquiry(int id)
        {
            // int k = 0;
            if (id != 0)
            {
                var JobEnquiry = entity.JobEnquiries.Where(d => d.Id == id).FirstOrDefault();
                entity.JobEnquiries.Remove(JobEnquiry);
                entity.SaveChanges();
            }

            return RedirectToAction("JobEnquiryList", "Job", new { ID = 10 });

        }
        public JsonResult GetVessleById(int Id)
        {
            var data = entity.Vessels.Where(d => d.VesselID == Id).FirstOrDefault();
            if (data == null)
            {
                data = new Vessel();
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
