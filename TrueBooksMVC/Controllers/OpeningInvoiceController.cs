using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;
using DAL;

namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    [Authorize]

    public class OpeningInvoiceController : Controller
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();

        SourceMastersModel objectSourceModel = new SourceMastersModel();

        public ActionResult openingInvoice()
        {
            var query = from t in entity.CUSTOMERs select t;
            ViewBag.customers = entity.CUSTOMERs.OrderBy(x => x.Customer1).ToList();

            var query1 = from t in entity.Suppliers select t;
            ViewBag.supplier = entity.Suppliers.OrderBy(x => x.SupplierName).ToList();

            var query3 = from t in entity.AcHeads select t;
            ViewBag.acHead = new SelectList(entity.AcHeads, "AcHeadID", "AcHead1");
            return View();

            var DebitAndCr = new SelectList(new[] 
                                        {
                                            new { ID = "1", trans = "Credit" },
                                            new { ID = "2", trans = "Debit" },
                                           
                                        },
                                      "ID", "trans", 1);
            ViewBag.Achead = entity.AcHeads.OrderBy(x => x.AcHead1).ToList();
            ViewBag.crdr = DebitAndCr;


        }

     [HttpPost]
        public JsonResult GetGridData(OpeningInvoiceVM obj)
        {

            bool drORcr = false;
            if (obj.DebitCreditID == true)
            {
                drORcr = true;

            }
            else

            {
                drORcr = false;
            }
         //Save data in acJournalMaster
            AcJournalMaster objACJournalMaster = new AcJournalMaster();
            objACJournalMaster.TransDate = DateTime.Now;

            objACJournalMaster.AcCompanyID =Convert.ToInt32( Session["AcCompanyID"]);
            objACJournalMaster.AcFinancialYearID = Convert.ToInt32( Session["fyearid"]);
            objACJournalMaster.AcJournalID = objectSourceModel.GetMaxNumberAcJournalMasters(); ;
            objACJournalMaster.VoucherNo = "";
            objACJournalMaster.StatusDelete = false;
            objACJournalMaster.VoucherType = "";
            objACJournalMaster.Remarks = obj.Remark;
            objACJournalMaster.VoucherNo = "";
            if (obj.CustomerHeadId > 0)
            {
                objACJournalMaster.TransType = 1;
            }
            if (obj.SupplierHeadId > 0)
            {
                objACJournalMaster.TransType = 2;
            }
            objACJournalMaster.UserID=Convert.ToInt32( Session["UserID"]);
            objACJournalMaster.Lock = false;

            entity.AcJournalMasters.Add(objACJournalMaster);
            entity.SaveChanges();

         //save data in acOpInvoiceMaster
         AcOPInvoiceMaster acOpMaster=new AcOPInvoiceMaster();
         int id =objectSourceModel.GetMaxNumberAcOpeningInvoiceMaster();
         if (id == 0)
         {
             acOpMaster.AcOPInvoiceMasterID = 1;
         }
         else
         {
             acOpMaster.AcOPInvoiceMasterID = objectSourceModel.GetMaxNumberAcOpeningInvoiceMaster();
         }
         acOpMaster.AcFinancialYearID=Convert.ToInt32( Session["fyearid"]);
        acOpMaster.OPDate=obj.OPDate;
        if (obj.CustomerHeadId > 0)
        {
            acOpMaster.AcHeadID = obj.CustomerHeadId;
        }
        if (obj.SupplierHeadId > 0)
        {
            acOpMaster.AcHeadID = obj.SupplierHeadId;
        }
        
        // acOpMaster.PartyID=Convert.ToInt32( Session["AcCompanyID"]);
         acOpMaster.Remarks=obj.Remark;

      
         if(obj.SupplierHeadId>0)
         {
             acOpMaster.PartyID=obj.SupplierHeadId;
         }
         else
         {
             acOpMaster.PartyID=obj.CustomerHeadId;
         }


           entity.AcOPInvoiceMasters.Add(acOpMaster);
            entity.SaveChanges();

         //save data in acJournalDetails
            //if (objACJournalMaster.AcJournalID > 0)
            //{
            //    var achead = objectSourceModel.GetAcHeadAssign();
            //    if (achead != null)
            //    {

            //        AcJournalDetail acJournalDetailsDR = new AcJournalDetail();
                   
            //        acJournalDetailsDR.Amount = obj.InvoiceAmount;
            //        acJournalDetailsDR.Remarks = obj.Remark;
            //        acJournalDetailsDR.AcJournalID = objACJournalMaster.AcJournalID;
            //        acJournalDetailsDR.BranchID =Convert.ToInt32( Session["branchid"]);
                    
            //        acJournalDetailsDR.AcJournalDetailID = objectSourceModel.GetMaxNumberAcJournalDetails();
            //        if (obj.CustomerHeadId != 0)
            //        {
            //            acJournalDetailsDR.AcHeadID = achead.CustomerControlAcID;
            //        }
            //        else
            //        {
            //            acJournalDetailsDR.AcHeadID = achead.SupplierControlAcID;
            //        }
            //        entity.AcJournalDetails.Add(acJournalDetailsDR);
            //        entity.SaveChanges();

            //        AcJournalDetail acJournalDetailsCR = new AcJournalDetail();

            //        acJournalDetailsCR.Amount = (-1) * Convert.ToDecimal(obj.InvoiceAmount);
            //        acJournalDetailsDR.Remarks = obj.Remark;
            //        acJournalDetailsDR.AcJournalID = objACJournalMaster.AcJournalID;
            //        acJournalDetailsDR.BranchID = Convert.ToInt32(Session["branchid"]);
            //        acJournalDetailsCR.AcJournalDetailID = objectSourceModel.GetMaxNumberAcJournalDetails();
            //        if (obj.CustomerHeadId != 0)
            //        {
            //            acJournalDetailsCR.AcHeadID = achead.CustomerControlAcID;
            //        }
            //        else
            //        {
            //            acJournalDetailsCR.AcHeadID = achead.SupplierControlAcID;
            //        }
            //        entity.AcJournalDetails.Add(acJournalDetailsCR);
            //        entity.SaveChanges();
            //    }
            //}

            return new JsonResult
            {
                Data = new
                {
                    success = true,
                   AcJournalID = objACJournalMaster.AcJournalID,
                   AcOpeningMasterID=acOpMaster.AcOPInvoiceMasterID,
                  DebitOrCredit = drORcr
                     
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };


        }


     public void saveGridData(string InvoiceNo, string JobCode, DateTime InvoiceDate, DateTime LastTransDate, decimal Amount, int AcJournalID, int AcOPInvoiceMasterId)
        {
         //save data in acjournalDetails
            AcJournalDetail acjournalDetailsDR = new AcJournalDetail();
            acjournalDetailsDR.AcJournalID = AcJournalID;
            acjournalDetailsDR.AcJournalDetailID = objectSourceModel.GetMaxNumberAcJournalDetails();
         //if(CustomerHeadID==0)
         //{
         //    acjournalDetailsDR.AcHeadID =SupplierHeadId;
         //}
         //if (SupplierHeadId == 0)
         //{
         //    acjournalDetailsDR.AcHeadID = CustomerHeadID;
         //}
         acjournalDetailsDR.BranchID =Convert.ToInt32( Session["branchid"]);
         acjournalDetailsDR.PartyID = Convert.ToInt32(Session["AcCompanyID"]);
            
                acjournalDetailsDR.Amount = (-1) * Convert.ToDecimal(Amount); 
           
            entity.AcJournalDetails.Add(acjournalDetailsDR);
            entity.SaveChanges();



            AcJournalDetail acjournalDetailsCR = new AcJournalDetail();
            acjournalDetailsCR.AcJournalID = AcJournalID;
            acjournalDetailsCR.AcJournalDetailID = objectSourceModel.GetMaxNumberAcJournalDetails();
            //if (CustomerHeadID == 0)
            //{
            //    acjournalDetailsDR.AcHeadID = SupplierHeadId;
            //}
            //if (SupplierHeadId == 0)
            //{
            //    acjournalDetailsDR.AcHeadID = CustomerHeadID;
            //}
            acjournalDetailsDR.BranchID = Convert.ToInt32(Session["branchid"]);
            acjournalDetailsDR.PartyID = Convert.ToInt32(Session["AcCompanyID"]);
            acjournalDetailsCR.Amount = Convert.ToDecimal(Amount);
           
            entity.AcJournalDetails.Add(acjournalDetailsCR);
            entity.SaveChanges();
         //save data in acopInvoiceDetails

            AcOPInvoiceDetail acOPInvoiceDetails = new AcOPInvoiceDetail();
            acOPInvoiceDetails.AcOPInvoiceDetailID = objectSourceModel.GetMaxNumberAcOpeningInvoiceDetails();
            acOPInvoiceDetails.StatusClose = false;
            acOPInvoiceDetails.LastTransDate = LastTransDate;
            acOPInvoiceDetails.JobCode = JobCode;
            acOPInvoiceDetails.InvoiceNo = InvoiceNo;
            acOPInvoiceDetails.InvoiceDate = InvoiceDate;
            acOPInvoiceDetails.Amount = Amount;
            acOPInvoiceDetails.AcJournalID = AcJournalID;
            acOPInvoiceDetails.AcOPInvoiceMasterID = AcOPInvoiceMasterId;
            entity.AcOPInvoiceDetails.Add(acOPInvoiceDetails);
            entity.SaveChanges();

           

        }

     public ActionResult Index()
     {
         return View();
     }

     public JsonResult GetOpeningInvoice(int transtype = 1)
     {

         var query = (from t in entity.AcOPInvoiceMasters

                      join s in entity.AcOPInvoiceDetails on t.AcOPInvoiceMasterID equals s.AcOPInvoiceMasterID
                      join ajm in entity.AcJournalMasters on s.AcJournalID equals ajm.AcJournalID
                      where ajm.TransType == transtype
                      select new OpeningInvoiceVM
                      {

                          Amount = s.Amount,
                          Remark = t.Remarks,
                          JobCode = s.JobCode,
                          invoiceNo = s.InvoiceNo,
                          InvoiceDate = s.InvoiceDate,
                          AcjournalMasterId = s.AcJournalID.Value
                      }).ToList();


         string view = this.RenderPartialView("ucOpeningInvoice", query);

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

            AcOPInvoiceDetail acopInvoiceD = entity.AcOPInvoiceDetails.Find(id);
            int acOPInvoiceMasterID = acopInvoiceD.AcOPInvoiceMasterID.Value;
            int acjournalid = acopInvoiceD.AcJournalID.Value;
            entity.AcOPInvoiceDetails.Remove(acopInvoiceD);
            entity.SaveChanges();

            AcOPInvoiceMaster acOPInvoiceM = entity.AcOPInvoiceMasters.Find(acOPInvoiceMasterID);
            entity.AcOPInvoiceMasters.Remove(acOPInvoiceM);
            entity.SaveChanges();

            AcJournalMaster acJournalMaster = entity.AcJournalMasters.Find(acjournalid);
            entity.AcJournalMasters.Remove(acJournalMaster);
            entity.SaveChanges();

            AcJournalDetail acJournalD = entity.AcJournalDetails.Where(item => item.AcJournalID == acjournalid).FirstOrDefault();
            entity.AcJournalDetails.Remove(acJournalD);
            entity.SaveChanges();

            return RedirectToAction("Index");
        }

        //public ActionResult Edit(int opinvoicemasterid = 0)
        //{
        //    var opinvmasterdata = (from o in entity.AcOPInvoiceMasters where o.AcOPInvoiceMasterID == opinvoicemasterid select o).FirstOrDefault();

        //    if (opinvmasterdata == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    else
        //    {
        //        CustomerJournalVM v = new CustomerJournalVM();
        ////         public int CustomerHeadId { get; set; }
        ////public int SupplierHeadId { get; set; }
        ////public bool IsCustomerSelected { get; set; }
        //////public decimal Amount { get; set; }
        ////public bool DebitCreditID { get; set; }
        ////public string Remark { get; set; }
        ////public int AcJournalDetailID { get; set; }
        ////public string AcHead { get; set; }
        ////public decimal? amount { get; set; }

          



        //    }

        //}

    }
}


   
