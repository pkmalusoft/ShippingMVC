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
                if (Session["JobID"] != null)
                {
                    if (Convert.ToInt32(Session["JobID"]) > 0)
                    {
                        Audit.JobID = Convert.ToInt32(Session["JobID"]);
                    }
                    else
                    {
                        Audit.JobID = 0;
                    }
                }
                else
                {
                    Audit.JobID = 0;
                }



                i = J.AddAuditLog(Audit, Session["UserID"].ToString());
                var AddLog = (from t in entity.JAuditLogs orderby t.JAuditLogID descending select t).FirstOrDefault();

                if (Session["AddLog"] == null)
                {
                    List<int> id = new List<int>();
                    id.Add(AddLog.JAuditLogID);
                    Session["AddLog"] = id;
                }
                else
                {
                    List<int> id = (List<int>)Session["AddLog"];
                    id.Add(AddLog.JAuditLogID);
                    Session["AddLog"] = id;
                }
            }

            return i.ToString();
        }

        public string AddBill(JBIllOfEntry Bill)
        {
            int i = 0;
            if (Session["UserID"] != null)
            {
                if (Session["JobID"] != null)
                {
                    if (Convert.ToInt32(Session["JobID"]) > 0)
                    {
                        Bill.JobID = Convert.ToInt32(Session["JobID"]);
                    }
                    else
                    {
                        Bill.JobID = 0;
                    }
                }
                else
                {
                    Bill.JobID = 0;
                }



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
                //   List<SP_GetCurrency_Result> Currency = new List<SP_GetCurrency_Result>();
                List<SP_GetAllSupplier_Result> Supplier = new List<SP_GetAllSupplier_Result>();
                List<SP_GetShippingAgents_Result> ShippingAgent = new List<SP_GetShippingAgents_Result>();

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
                //   Currency = MM.GetCurrency();
                // Supplier = MM.GetAllSupplier();
                ShippingAgent = MM.GetShippingAgents();

                ViewBag.Ports = new SelectList(Ports, "PortID", "Port");
                ViewBag.Customer = new SelectList(Customers, "CustomerID", "Customer");
                ViewBag.Employees = new SelectList(Employees, "EmployeeID", "EmployeeName");
                ViewBag.Vessels = new SelectList(Vessels, "VesselID", "Vessel");
                ViewBag.Countries = new SelectList(Countries, "CountryID", "CountryName");
                ViewBag.JobType = new SelectList(JobType, "JobTypeID", "JobDescription");
                ViewBag.Carriers = new SelectList(Carriers, "CarrierID", "Carrier");
                ViewBag.Transporters = new SelectList(Transporters, "TransPorterID", "TransPorter");
                ViewBag.ContainerTypes = new SelectList(ContainerTypes, "ContainerTypeID", "ContainerType");
                // ViewBag.RevenueT = new SelectList(RevenueType, "RevenueTypeID", "RevenueType");
                // ViewBag.Curency = new SelectList(Currency, "CurrencyID", "CurrencyName");
                //  ViewBag.Suplier = new SelectList(Supplier, "SupplierID", "SupplierName");
                ViewBag.ShippingA = new SelectList(ShippingAgent.OrderBy(x=>x.AgentName).ToList(), "ShippingAgentID", "AgentName");
                ViewBag.MaxInvoiceNumber = J.GetMaxInvoiceNumber();
                ViewBag.voyages = (from c in entity.Voyages select c).ToList();

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


           
           string  view = this.RenderPartialView("_GetJob", data);

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
