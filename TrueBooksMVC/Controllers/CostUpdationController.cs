using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;
using DAL;
using System.Dynamic;


namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class CostUpdationController : Controller
    {
        CostUpdationModel CU = new CostUpdationModel();
        MastersModel MM = new MastersModel();
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();

        //
        // GET: /CostUpdation/

        public ActionResult CostUpdation(int id,int Jobid)
        {
            costUpdationVM Cost = new costUpdationVM();
            Session["CostUpdationJobid"] = Jobid;
            if (Session["UserID"] != null)
            {
                if (id > 0)
                {
                    Cost = CU.GetCostupdationbyCostUpID(id);
                    //  Cost.InvoiceDate = System.DateTime.UtcNow;
                    //BindMasters();
                    Cost.CostUpdationDetails = DAL.GetCostUpdationDetailsbyCostUpdationID(id);
                    foreach (var item in Cost.CostUpdationDetails)
                    {
                        string currency = (from c in entity.CurrencyMasters where c.CurrencyID == item.ProvisionCurrencyID select c.CurrencyName).FirstOrDefault();
                        string revenuename = (from r in entity.RevenueTypes where r.RevenueTypeID == item.RevenueTypeID select r.RevenueType1).FirstOrDefault();
                        //  item.RevenueTypeName = revenuename;
                        //  item.CurrencyName = currency;
                    }
                    BindMasters_ForEdit(Cost);
                }
                else
                {
                    Cost.CostUpdationDetails = new List<costUpdationDetailVM>();
                    BindMasters();
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

            return View(Cost);
        }

        [HttpPost]
        public ActionResult CostUpdation(costUpdationVM CostU, string Command)
        {
            BindMasters();

            int costupdationId = 0;

            if (Session["UserID"] != null)
            {
                CostU.UserID = Convert.ToInt32(Session["UserID"]);
                CostU.SupplierPaymentStatus = "1";
                CostU.SupplierID = CostU.SelectedSupplierID;
                CostU.JobID = 0;
                // CostU.SupplierID = Convert.ToInt32(Suppliers);
                /*  if (CostU.JobID == null)
                  {

                     CostU.InvoiceNo = CU.GetInvoiceNoByJobID(Convert.ToInt32(CostU.MultiJobID.First()));
                     CostU.JobID = CostU.MultiJobID.First();
                  }
                  else
                  {
                      CostU.InvoiceNo = CU.GetInvoiceNoByJobID(Convert.ToInt32(CostU.JobID));
                  }*/
                costupdationId = CU.SaveCostUpdation(CostU);

                if (costupdationId > 0)
                {
                    int n = 0;
                    foreach (var item in CostU.CostUpdationDetails)
                    {
                        ////////////////////////////////////

                        CU.SaveCostUpdationDetails(item, costupdationId);

                        //todo:fix to run by sethu
                        //  int jid = (from c in entity.CostUpdations where c.CostUpdationID == item select c.CostUpdationID).FirstOrDefault();
                        //todo:fix to run by sethu
                        //   var d = CostU.CostUpdationDetails.Where(x => x.CostUpdationID == jid).ToList();
                        //  if (d.Count==0)
                        //    {
                        //  var query1 = (from t in CostU.CostUpdationDetails where t.CostUpdationID == item select t).ToList();
                        //   foreach (var j in query1)
                        //   {
                        //    CU.SaveCostUpdationDetails(CostU.CostUpdationDetails[n], jid);
                        //  }
                        //    CU.SaveCostUpdationDetails(query1, item);
                        //   // CU.saveAcJournalDetails(item, Convert.ToInt32(Session["fyearid"]));
                        //  }
                        //  else
                        //  {
                        //  foreach (var i in d)
                        //  {
                        //var data = (from t in entity.CostUpdations where t.CostUpdationID == item select t.JobID).FirstOrDefault();
                        //var query = (from context in CostU.CostUpdationDetails where context.JobID == data select context).FirstOrDefault();
                        //if (query == null)
                        //{
                        //    var query1 = (from t in CostU.CostUpdationDetails where t.CostUpdationID == item select t).FirstOrDefault();
                        //    CU.SaveCostUpdationDetails(query1, item);
                        //    // CU.saveAcJournalDetails(item, Convert.ToInt32(Session["fyearid"]));
                        //}
                        //else
                        //{
                        //    CU.SaveCostUpdationDetails(query, item);
                        //    CU.saveAcJournalDetails(item, Convert.ToInt32(Session["fyearid"]));

                        //}

                        //  }

                        //  }
                        //var data=(from t in entity.CostUpdations where t.CostUpdationID==item select t.JobID).FirstOrDefault();
                        //var query=(from context in CostU.CostUpdationDetails where context.JobID==data select context).FirstOrDefault();
                        //if (query == null)
                        //{

                        //    var query1 = (from t in CostU.CostUpdationDetails where t.CostUpdationID == item select t).FirstOrDefault();
                        //    CU.SaveCostUpdationDetails(query1, item);
                        //   // CU.saveAcJournalDetails(item, Convert.ToInt32(Session["fyearid"]));


                        //}
                        //else
                        //{
                        //    CU.SaveCostUpdationDetails(query, item);
                        //    CU.saveAcJournalDetails(item, Convert.ToInt32(Session["fyearid"]));

                        //}
                        n = n + 1;
                    }

                    var data = (from c in entity.CostUpdations where c.CostUpdationID == costupdationId select c).FirstOrDefault();
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
                        acjournalmaster.VoucherNo = data.DocumentNo;
                        entity.AcJournalMasters.Add(acjournalmaster);
                        entity.SaveChanges();
                        var Costupdate = (from d in entity.CostUpdations where d.CostUpdationID == costupdationId select d).FirstOrDefault();
                        Costupdate.AcJournalID = JournalId;
                        entity.SaveChanges();
                        acjid = JournalId;
                    }




                    var pagecontrol = (from d in entity.PageControlMasters where d.ControlName.ToLower() == "cost update" select d).FirstOrDefault();
                    var accountsetup = (from d in entity.AcHeadControls where d.Pagecontrol == pagecontrol.Id select d).ToList();

                    //int custcontrolacid = (from c in entity.AcHeadAssigns select c.CustomerControlAcID.Value).FirstOrDefault();
                    ////int freightacheadid = 158;
                    ////int provcontrolacid = (from c in entity.AcHeadAssigns select c.ProvisionCostControlAcID.Value).FirstOrDefault();
                    ////int accruedcontrolacid = (from c in entity.AcHeadAssigns select c.AccruedCostControlAcID.Value).FirstOrDefault();
                    foreach (var acsetup in accountsetup)
                    {
                        var acjdetail1 = (from x in entity.AcJournalDetails where x.AcJournalID == acjid && x.AcHeadID == acsetup.AccountHeadID select x).FirstOrDefault();
                        int acjdetail2 = 0;
                        decimal? amount = 0;
                        if (acsetup.AccountName.ToLower() == "invoice amount")
                        {
                            if (acsetup.AccountNature == true)
                            {
                                amount = CostU.InvoiceAmount * -1;
                            }
                            else
                            {
                                amount = CostU.InvoiceAmount;
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



                        //CU.saveAcJournalDetails(costupdationId, Convert.ToInt32(Session["fyearid"]));

                    }
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            return RedirectToAction("CostUpdationDetails", "CostUpdation", new { ID = 15 });
        }

        public ActionResult CostUpdationDetails(int ID)
        {
            Session["CostUpdationJobid"] = 0;
            List<GetAllCostUpdation_Result> CostUpdations = new List<GetAllCostUpdation_Result>();

            CostUpdations = DAL.SP_GetAllCostUpdation(Convert.ToDateTime(Session["FyearFrom"]), Convert.ToDateTime(Session["FyearTo"]));

            //var data = (from t in CostUpdations where (t.JobDate >= Convert.ToDateTime(Session["FyearFrom"]) && t.JobDate <= Convert.ToDateTime(Session["FyearTo"])) select t).ToList();
            if (ID == 15)
            {
                ViewBag.SuccessMsg = "You have successfully added cost.";
            }

            if (ID == 10)
            {
                ViewBag.SuccessMsg = "You have successfully deleted cost.";
            }

            if (ID == 20)
            {
                ViewBag.SuccessMsg = "You have successfully updated cost.";
            }

            Session["CUID"] = ID;

            return View(CostUpdations);
        }

        public string getSuccessID()
        {
            string ID = "";

            if (Session["CUID"] != null)
            {
                ID = Session["CUID"].ToString();
            }

            return ID;
        }

        //public JsonResult BindSupplierByJobCode(string ID)
        //{
        //    //  List<SP_GetAllSupplierByJobCode_Result> Suppliers = new List<SP_GetAllSupplierByJobCode_Result>();

        //var Suppliers = CU.GetSupplierByJobCode(ID);

        //    //ViewBag.Suppliers = new SelectList(Suppliers, "SupplierID", "SupplierName");
        //    return Json(Suppliers, JsonRequestBehavior.AllowGet);
        //}


        public JsonResult BindJobCodeBySupplier(int supplierID)
        {
            DateTime frmdate = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime tdate = Convert.ToDateTime(Session["FyearTo"].ToString());
            //var query = (from t in entity.JInvoices
            //             join j in entity.JobGenerations on t.JobID equals j.JobID
            //             where t.SupplierID == supplierID && (j.JobDate >= frmdate) &&
            //              (j.JobDate <= tdate)
            //             select j).Distinct().ToList();

            var query1 = entity.GetJobDetailsBySupplieRID(supplierID, frmdate, tdate).ToList();

            //var query1 = (from p in entity.JobGenerations 
            //              where  &&
            //              query.Contains(p.JobID) select p).ToList();




            //ViewBag.Suppliers = new SelectList(Suppliers, "SupplierID", "SupplierName");
            return Json(query1, JsonRequestBehavior.AllowGet);
        }



        public JsonResult BindCostGrid(string ID, string JobIDList)
        {
            //List<int> JObIDList = new List<int>();

            //var joblist = JobIDList.Split(',');
            //foreach (var item in joblist)
            //{
            //    JObIDList.Add(Convert.ToInt32(item));
            //}

            string[] data = ID.Split(',');
            //var AllInvoices = CU.GetAllInvoicesBySupplier(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]), Convert.ToInt32(data[2]));
            var AllInvoices = CU.GetAllInvoicesBySupplier(Convert.ToInt32(ID), JobIDList);
            return Json(AllInvoices, JsonRequestBehavior.AllowGet);
        }

        public List<SP_GetAllInvoiceChargesbySupplier_Result> BindCostGridtemp()
        {


            ////var AllInvoices = CU.GetAllInvoicesBySupplier(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]), Convert.ToInt32(data[2]));
            //return CU.GetAllInvoicesBySupplier(2,"49");
            return null;
        }

        //public JsonResult BindCostGrid_Edit(int costupdationID)
        //{
        //    var query = (from t in entity.CostUpdationDetails
        //                 where t.CostUpdationID == costupdationID
        //                 select t).ToList();


        //    return Json(query, JsonRequestBehavior.AllowGet);
        //}


        public void BindMasters()
        {
            List<SP_GetJobCodesForCostUpdation_Result> JobCodes = new List<SP_GetJobCodesForCostUpdation_Result>();
            JobCodes = CU.GetJobCodes();

            List<SP_GetAllEmployees_Result> Employees = new List<SP_GetAllEmployees_Result>();
            Employees = MM.GetAllEmployees();

            string DocNo = CU.GetMaxCostUpdationDocumentNo();

            ViewBag.DocumentNos = DocNo;

            ViewBag.AllJobCodes = new SelectList(JobCodes, "JobID", "JobCode");
            // var query = (from t in entity.Suppliers select t).OrderBy(x => x.SupplierName).ToList();

            //  ViewBag.Suppliers = new SelectList(query.ToList(), "SupplierID", "SupplierName");

            //ViewBag.Suppliers = new SelectList(new[] { new { SupplierID = "0", SupplierName = "Select" } }, "SupplierID", "SupplierName");

            ViewBag.Employes = new SelectList(Employees, "EmployeeID", "EmployeeName");

        }

        public void BindMasters_ForEdit(costUpdationVM costupdation)
        {
            List<SP_GetJobCodesForCostUpdation_Result> JobCodes = new List<SP_GetJobCodesForCostUpdation_Result>();
            JobCodes = CU.GetJobCodes();

            List<SP_GetAllEmployees_Result> Employees = new List<SP_GetAllEmployees_Result>();
            Employees = MM.GetAllEmployees();



            ViewBag.DocumentNos = costupdation.DocumentNo;

            ViewBag.AllJobCodes = new SelectList(JobCodes, "JobID", "JobCode", costupdation.JobID);

            //   ViewBag.Suppliers = new SelectList(new[] { new { SupplierID = "0", SupplierName = "Select" } }, "SupplierID", "SupplierName");

            ViewBag.Employes = new SelectList(Employees, "EmployeeID", "EmployeeName", costupdation.EmployeeID);

        }

        [HttpGet]
        public ActionResult DeleteCost(int id)
        {
            // int k = 0;

            if (id != 0)
            {
                CU.DeleteCostUpdationsDetails(id);
            }

            return RedirectToAction("CostUpdationDetails", "CostUpdation", new { ID = 10 });

        }


        public JsonResult GetAllCurrencyCostUpdation()
        {
            //List<SP_GetCustomerInvoiceDetailsForReciept_Result> AllInvoices = new List<SP_GetCustomerInvoiceDetailsForReciept_Result>();
            // var AllInvoices;


            var CostUpdation = (from t in entity.SPGetAllLocalCurrencyCostUpdation(Convert.ToInt32(Session["fyearid"].ToString()))
                                select t).ToList();
            return Json(CostUpdation, JsonRequestBehavior.AllowGet);



        }

        public JsonResult GetAllCOstUpdation(DateTime fdate, DateTime todate)
        {
            // var cupadtion = entity.SP_GetAllCostUpdation().Where(x => x.JobDate >= fdate & x.JobDate <= todate).ToList();
            var cupadtion = DAL.SP_GetAllCostUpdation(fdate, todate);
            foreach (var item in cupadtion)
            {
                var costupdationdetails = (from d in entity.CostUpdationDetails where d.CostUpdationID == item.CostUpdationID select d).ToList();
                var jobgeneration = (from d in entity.CostUpdationDetails
                                     join s in entity.JInvoices on
                                     d.JInvoiceID equals s.InvoiceID
                                     join JG in entity.JobGenerations on
                                     s.JobID equals JG.JobID
                                     where d.CostUpdationID == item.CostUpdationID
                                     select JG).ToList();
                item.JobDates = string.Join(",", jobgeneration.Select(d => d.JobDate));
                item.JobValues = string.Join(",", costupdationdetails.Select(d => d.ProvisionHome));
                item.JobNumbers = string.Join(",", jobgeneration.Select(d => d.JobCode));
                item.CurrencyName = string.Join(",", costupdationdetails.Select(d => d.AmountPaidTillDate));
                item.YearToDateInvoiced = string.Join(",", costupdationdetails.Select(d => d.Cost));

            }

            string view = this.RenderPartialView("_GetAllCOstUpdation", cupadtion);
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
