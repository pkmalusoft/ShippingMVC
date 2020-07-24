using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueBooksMVC.Models;
using System.Data.SqlClient;
using System.Data;
using DAL;
using System.Data.Entity;
using System.Data.Metadata.Edm;

namespace TrueBooksMVC.Models
{
    public class CostUpdationModel
    {
        SHIPPING_FinalEntities Context1 = new SHIPPING_FinalEntities();

        public List<SP_GetAllCostUpdation_Result> AllCostUpdation()
        {
            return Context1.SP_GetAllCostUpdation().ToList();
        }

        public List<SP_GetJobCodesForCostUpdation_Result> GetJobCodes()
        {
            return Context1.SP_GetJobCodesForCostUpdation().ToList();
        }

        public string GetMaxCostUpdationDocumentNo()
        {
            string Docno = "";

            var quary = Context1.SP_GetMaxCostUpdationDocNo();

            foreach (var item in quary)
            {
                Docno = item.ToString();
            }

            return Docno;
        }

        public List<SP_GetAllSupplierByJobCode_Result> GetSupplierByJobCode(string Jobcode)
        {
            return Context1.SP_GetAllSupplierByJobCode(Jobcode).ToList();
        }

        public List<SP_GetAllInvoiceChargesbySupplier_Result> GetAllInvoicesBySupplier(int SupplierID, string JobID)
        {

            return Context1.SP_GetAllInvoiceChargesbySupplier(SupplierID, JobID).ToList();
        }


        public List<CostUpdationDetail> GetCostUpdationDetailsbyCostUpdationID(int CostUpdationID)
        {
            var query = (from t in Context1.CostUpdationDetails where CostUpdationID == t.CostUpdationID select t).ToList();


            return Context1.CostUpdationDetails.Where(ite => ite.CostUpdationID == CostUpdationID).ToList();
        }

        public costUpdationVM GetCostupdationbyCostUpID(int CostID)
        {
            costUpdationVM CostUp = new costUpdationVM();

            if (CostID <= 0)
                return new costUpdationVM();

            var query = Context1.SP_GetCostUpdationByCostID(CostID);

            if (query != null)
            {
                var item = query.FirstOrDefault();
                CostUp.AcJournalID = item.AcJournalID.Value;
                CostUp.InvoiceNo = item.InvoiceNo;
                CostUp.DocumentNo = item.DocumentNo;
                CostUp.SupplierID = item.SupplierID;
                CostUp.SelectedSupplierID = item.SupplierID;
                CostUp.InvoiceDate = item.InvoiceDate;
                CostUp.EmployeeID = item.EmployeeID;
                CostUp.JobID = item.JobID;
                CostUp.CostUpdationID = item.CostUpdationID;
                if (item.JobCode != null)
                {
                    if (item.JobCode.StartsWith(","))
                    {
                        CostUp.JobCode = item.JobCode.Substring(1);
                    }
                    else
                    {
                        CostUp.JobCode = item.JobCode;
                    }
                }
                CostUp.SupplierName = item.SupplierName;
                CostUp.SupplierID = item.SupplierID;
                CostUp.TransactionDate = item.TransactionDate;
                CostUp.InvoiceAmount = item.InvoiceAmount;
            }

            else
            {
                return new costUpdationVM();
            }

            return CostUp;

        }

        public int SaveCostUpdation(costUpdationVM CU)
        {
            int costupdationId = 0;
            CostUpdation objcostUpdation = new CostUpdation();
            //int i = Context1.SP_InsertCostUpdationAndDetails(CU.SupplierID, CU.JobID, CU.InvoiceNo, CU.InvoiceDate, CU.EmployeeID,
            //    CU.DocumentNo, CU.SupplierPaymentStatus, CU.UserID, SupplierRef, Convert.ToDecimal(amountPaidTillDate));

            //Edit Code
            if (CU.CostUpdationID > 0)
            {
                objcostUpdation = Context1.CostUpdations.Where(ite => ite.CostUpdationID == CU.CostUpdationID).FirstOrDefault();
                if (objcostUpdation != null)
                {
                    objcostUpdation.SupplierID = CU.SupplierID;
                    objcostUpdation.JobID = CU.JobID;
                    objcostUpdation.InvoiceNo = CU.InvoiceNo;
                    objcostUpdation.InvoiceDate = CU.InvoiceDate;
                    objcostUpdation.DocumentNo = CU.DocumentNo;
                    objcostUpdation.SupplierPaymentStatus = CU.SupplierPaymentStatus;
                    objcostUpdation.UserID = CU.UserID;
                    objcostUpdation.EmployeeID = CU.EmployeeID;
                    objcostUpdation.CostUpdationID = CU.CostUpdationID;
                    objcostUpdation.AcJournalID = CU.AcJournalID;
                    objcostUpdation.TransactionDate = CU.TransactionDate;
                    objcostUpdation.InvoiceAmount = CU.InvoiceAmount;
                    Context1.Entry(objcostUpdation).State = EntityState.Modified;
                    Context1.SaveChanges();
                    costupdationId = objcostUpdation.CostUpdationID;
                }
            }
            else
            {
                //  foreach (var item in CU.MultiJobID)
                // {

                // CU.InvoiceNo = GetInvoiceNoByJobID(0);
                objcostUpdation = new CostUpdation();
                var costupdations = Context1.CostUpdations;
                var maxValue = 0;
                int MaxcostupdationID = 0;
                if (costupdations.Count() > 0)
                {
                    maxValue = Context1.CostUpdations.Max(x => x.CostUpdationID);
                    MaxcostupdationID = Context1.CostUpdations.First(x => x.CostUpdationID == maxValue).CostUpdationID;

                }
                if (Convert.ToInt32(MaxcostupdationID) <= 0)
                    MaxcostupdationID = 1;
                else
                {
                    MaxcostupdationID = (MaxcostupdationID) + 1;
                }
                //     int MaxcostupdationID = 33831;
                objcostUpdation.CostUpdationID = MaxcostupdationID;
                objcostUpdation.SupplierID = CU.SupplierID;
                objcostUpdation.JobID = 0;
                objcostUpdation.InvoiceNo = CU.InvoiceNo;
                objcostUpdation.InvoiceDate = CU.InvoiceDate;
                objcostUpdation.DocumentNo = CU.DocumentNo;
                objcostUpdation.SupplierPaymentStatus = CU.SupplierPaymentStatus;
                objcostUpdation.UserID = CU.UserID;
                objcostUpdation.EmployeeID = CU.EmployeeID;
                // objcostUpdation.AcJournalID = CU.AcJournalID;
                objcostUpdation.PrevCostupID = CU.PrevCostupID;
                objcostUpdation.TransactionDate = CU.TransactionDate;
                objcostUpdation.InvoiceAmount = CU.InvoiceAmount;

                //Updation Jinvoice Entry
                //todo: sethu. move this to cost detail insert
                /*  var joinvoice = Context1.JInvoices.Where(ite => ite.JobID == item && ite.SupplierID == CU.SupplierID).ToList();
                  foreach (var objjoince in joinvoice)
                  {
                      objjoince.CostUpdationStatus = "2";
                  }*/

                //  5000,1,1,'','2016-1-1',1,1,'','',1,0
                Context1.SP_InsertCostUpdation(objcostUpdation.CostUpdationID, objcostUpdation.SupplierID.Value,
objcostUpdation.JobID.Value, objcostUpdation.InvoiceNo, objcostUpdation.InvoiceDate, 0, objcostUpdation.EmployeeID.Value,
objcostUpdation.DocumentNo, objcostUpdation.PrevCostupID, objcostUpdation.SupplierPaymentStatus, objcostUpdation.UserID.Value, false, objcostUpdation.TransactionDate, objcostUpdation.InvoiceAmount);
                //   Context1.CostUpdations.Add(objcostUpdation);
                Context1.SaveChanges();
                costupdationId = objcostUpdation.CostUpdationID;
                // }
            }
            return costupdationId;
        }

        public int SaveCostUpdationDetails(costUpdationDetailVM costupdationdetailsid, int costUpdationID)
        {
            //int i = Context1.SP_InsertCostUpdationAndDetails(CU.SupplierID, CU.JobID, CU.InvoiceNo, CU.InvoiceDate, CU.EmployeeID,
            //    CU.DocumentNo, CU.SupplierPaymentStatus, CU.UserID, SupplierRef, Convert.ToDecimal(amountPaidTillDate));
            try
            {
                //foreach (var CU in CUdetailslist)
                //{
                /*    if (costupdationdetailsid.CostUpdationDetailID > 0)
                    {
                        //Edit Code
                        CostUpdationDetail objCostUpdationDetail = Context1.CostUpdationDetails.Where(item => item.CostUpdationDetailID == costupdationdetailsid.CostUpdationDetailID).FirstOrDefault();
                        objCostUpdationDetail.RevenueTypeID = objCostUpdationDetail.RevenueTypeID;
                        objCostUpdationDetail.ProvisionCurrencyID = costupdationdetailsid.ProvisionCurrencyID;
                        objCostUpdationDetail.ProvisionExchangeRate = costupdationdetailsid.ProvisionExchangeRate;
                        objCostUpdationDetail.ProvisionForeign = costupdationdetailsid.ProvisionForeign;
                        objCostUpdationDetail.ProvisionHome = costupdationdetailsid.ProvisionHome;
                        objCostUpdationDetail.SalesCurrencyID = costupdationdetailsid.SalesCurrencyID;
                        objCostUpdationDetail.SalesExchangeRate = costupdationdetailsid.SalesExchangeRate;
                        objCostUpdationDetail.SalesForeign = costupdationdetailsid.SalesForeign;
                        objCostUpdationDetail.SalesHome = costupdationdetailsid.SalesHome;
                        objCostUpdationDetail.Cost = costupdationdetailsid.Cost;
                        objCostUpdationDetail.SupplierID = costupdationdetailsid.SupplierID;
                        objCostUpdationDetail.JInvoiceID = costupdationdetailsid.JInvoiceID;
                        objCostUpdationDetail.PaidOrNot = "N";
                        objCostUpdationDetail.SupplierReference = costupdationdetailsid.supplierReference;
                        //todo:fix to run by sethu
                        //    objCostUpdationDetail.AmountPaidTillDate = costupdationdetailsid.InvoiceAmount + costupdationdetailsid.AmountPaidTillDate;
                        objCostUpdationDetail.AmountPaidTillDate = costupdationdetailsid.AmountPaidTillDate;
                        objCostUpdationDetail.SupplierPayStatus = "1";
                        objCostUpdationDetail.CostUpdationID = costUpdationID;
                        Context1.Entry(objCostUpdationDetail).State = EntityState.Modified;
                    }
                    else*/
                if (costupdationdetailsid.InvoiceAmount > 0)
                {
                    var isnew = 0;
                    CostUpdationDetail objCostUpdationDetail = new CostUpdationDetail();
                    objCostUpdationDetail = (from d in Context1.CostUpdationDetails where d.CostUpdationDetailID == costupdationdetailsid.CostUpdationDetailID select d).FirstOrDefault();
                    if (objCostUpdationDetail == null)
                    {
                        objCostUpdationDetail = new CostUpdationDetail();
                        var mvalue = Context1.CostUpdationDetails;
                        if (mvalue.Count() > 0)
                        {
                            var maxValue = mvalue.Max(x => x.CostUpdationDetailID);

                            objCostUpdationDetail.CostUpdationDetailID = maxValue + 1;

                        }
                        else
                        {
                            objCostUpdationDetail.CostUpdationDetailID = 1;
                        }
                        isnew = 1;
                    }
                    //objCostUpdationDetail.CostUpdationDetailID = costupdationdetailsid.CostUpdationDetailID;
                    objCostUpdationDetail.RevenueTypeID = costupdationdetailsid.RevenueTypeID;
                    objCostUpdationDetail.ProvisionCurrencyID = costupdationdetailsid.ProvisionCurrencyID;
                    objCostUpdationDetail.ProvisionExchangeRate = costupdationdetailsid.ProvisionExchangeRate;
                    objCostUpdationDetail.ProvisionForeign = costupdationdetailsid.ProvisionForeign;
                    objCostUpdationDetail.ProvisionHome = costupdationdetailsid.ProvisionHome;
                    objCostUpdationDetail.SalesCurrencyID = costupdationdetailsid.SalesCurrencyID;
                    objCostUpdationDetail.SalesExchangeRate = costupdationdetailsid.SalesExchangeRate;
                    objCostUpdationDetail.SalesForeign = costupdationdetailsid.SalesForeign;
                    objCostUpdationDetail.SalesHome = costupdationdetailsid.SalesHome;
                    objCostUpdationDetail.Cost = costupdationdetailsid.Cost;
                    objCostUpdationDetail.SupplierID = costupdationdetailsid.SupplierID;
                    objCostUpdationDetail.JInvoiceID = costupdationdetailsid.JInvoiceID;
                    objCostUpdationDetail.Variance = null;
                    objCostUpdationDetail.AmountPaidTillDate = costupdationdetailsid.AmountPaidTillDate;
                    objCostUpdationDetail.SupplierReference = costupdationdetailsid.supplierReference;
                
                    if (costupdationdetailsid.InvoiceAmount != null)
                    {

                    }
                    else
                    {
                        costupdationdetailsid.InvoiceAmount = 0;
                    }
                    //todo:fix to run by sethu
                    //  objCostUpdationDetail.AmountPaidTillDate = costupdationdetailsid.InvoiceAmount;
                    if (objCostUpdationDetail.AmountPaidTillDate != null)
                    {
                        objCostUpdationDetail.AmountPaidTillDate = objCostUpdationDetail.AmountPaidTillDate + costupdationdetailsid.InvoiceAmount;
                    }
                    else
                    {
                        objCostUpdationDetail.AmountPaidTillDate = costupdationdetailsid.InvoiceAmount;
                    }

                    if (objCostUpdationDetail.AmountPaidTillDate > 0)
                    {
                        objCostUpdationDetail.PaidOrNot = "Y";
                    }
                    else
                    {
                        objCostUpdationDetail.PaidOrNot = "N";
                    }
                    objCostUpdationDetail.SupplierPayStatus = "1";
                    objCostUpdationDetail.CostUpdationID = costUpdationID;
                    if (isnew == 1)
                    {
                        Context1.CostUpdationDetails.Add(objCostUpdationDetail);
                    }
                    else
                    {
                        Context1.Entry(objCostUpdationDetail).State = EntityState.Modified;
                    }
                    //    Context1.SP_InsertCostUpdationDetails(objCostUpdationDetail.CostUpdationDetailID, objCostUpdationDetail.CostUpdationID, objCostUpdationDetail.RevenueTypeID
                    //    , objCostUpdationDetail.ProvisionCurrencyID, objCostUpdationDetail.ProvisionExchangeRate, objCostUpdationDetail.ProvisionForeign,
                    //    objCostUpdationDetail.ProvisionHome, objCostUpdationDetail.SalesCurrencyID, objCostUpdationDetail.SalesExchangeRate,
                    //    objCostUpdationDetail.SalesForeign, objCostUpdationDetail.SalesHome, objCostUpdationDetail.Variance,
                    //    objCostUpdationDetail.SupplierID, objCostUpdationDetail.JInvoiceID, objCostUpdationDetail.Cost, 0, objCostUpdationDetail.AmountPaidTillDate,
                    //    objCostUpdationDetail.PaidOrNot, objCostUpdationDetail.SupplierReference, objCostUpdationDetail.SupplierPayStatus, false);
                    //}
                    var jinvoice = (from d in Context1.JInvoices where d.InvoiceID == costupdationdetailsid.JInvoiceID && d.SupplierID == costupdationdetailsid.SupplierID select d).FirstOrDefault();
                    jinvoice.CostUpdationStatus = "2";
                    Context1.Entry(jinvoice).State = EntityState.Modified;

                }
                //}
                Context1.SaveChanges();
                //}

                return 1;
            }
            catch (Exception e)
            {

                return 0;
            }

            //return 0;


        }

        public string GetInvoiceNoByJobID(int jobId)
        {
            string InvNo = "";


            var quary = Context1.SP_GetInvoiceNumberByJobID(jobId);

            foreach (var item1 in quary)
            {
                InvNo = item1.ToString();
            }

            return InvNo;
        }

        public int DeleteCostUpdationsDetails(int CostUpdationID)
        {
            int i = Context1.SP_DeleteCostUpdationDetails(CostUpdationID);

            return i;
        }

        public void saveAcJournalDetails(int costupdationId, int fyearId)
        {
            try
            {

                var query = Context1.SP_InsertJournalEntryForCostUpdation(costupdationId, fyearId);


            }
            catch (Exception)
            {


            }

        }

    }
}