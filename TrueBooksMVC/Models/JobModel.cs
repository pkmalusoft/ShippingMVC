using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueBooksMVC.Models;
using System.Data.SqlClient;
using System.Data;
using DAL;
using System.Data.Entity;
using System.Data.Objects;

namespace TrueBooksMVC.Models
{
    public class JobModel
    {
        SHIPPING_FinalEntities Context1 = new SHIPPING_FinalEntities();
        DateTimeZoneConversionModel DTZC = new DateTimeZoneConversionModel();

        public int AddJob(JobGeneration JG)
        {
            ObjectParameter objMaxJobId = new ObjectParameter("MaxJObID",0);
            int query = Convert.ToInt32(Context1.SP_InsertJob(JG.JobCode, JG.JobTypeID, JG.JobDate, JG.ConsignerID, JG.ConsigneeID, JG.InvoiceToID, JG.EmployeeID, JG.ShipperID, JG.IPTNo, JG.RefNo, JG.BillOfEnquiry, JG.BLDate, JG.DeliveryOrderNo, JG.BLStatus, JG.CLFValue, JG.DepositAmount, JG.DepositDate, JG.ReceiptNo, JG.RefundDate, JG.RefundAmount, JG.VesselID, JG.VoyageNo, JG.Freight, JG.SailingDate, JG.ArrivalDate, JG.MBL, JG.HBL, JG.LoadPortID, JG.DestinationPortID, JG.DeliveryPlaceID, JG.ReceiptPlaceID, JG.CountryofOriginID, JG.TransporterID, JG.CollectionDate, JG.DeliveryInstructions, JG.TruckRegNo, JG.DriverDetails, JG.Remarks, JG.RotationNo, JG.CarrierID, JG.CollectionPoint, JG.DeliveryNote, JG.DeliveryPoint, JG.CollectionInstructions, Convert.ToInt32(JG.InvoiceNo.Value), Convert.ToDateTime(JG.InvoiceDate), JG.MainJobID.HasValue ? JG.MainJobID.Value : 0,JG.DeliveryDate,JG.DepartingDate,JG.Flight,JG.MAWB,JG.HAWB, JG.MRN,JG.DeliveryOrderDate, JG.shippingInstruction, objMaxJobId));

            return Convert.ToInt32(objMaxJobId.Value);
        }

        public int UpdateJob(JobGeneration JG)
        {
            int query = Convert.ToInt32(Context1.SP_UpdateJob(JG.JobCode, JG.JobTypeID, JG.JobDate, JG.ConsignerID, JG.ConsigneeID, JG.InvoiceToID, JG.EmployeeID, JG.ShipperID, JG.IPTNo, JG.RefNo, JG.BillOfEnquiry, JG.BLDate, JG.DeliveryOrderNo, JG.BLStatus, JG.CLFValue, JG.DepositAmount, JG.DepositDate, JG.ReceiptNo, JG.RefundDate, JG.RefundAmount, JG.VesselID, JG.VoyageNo, JG.Freight, JG.SailingDate, JG.ArrivalDate, JG.MBL, JG.HBL, JG.LoadPortID, JG.DestinationPortID, JG.DeliveryPlaceID, JG.ReceiptPlaceID, JG.CountryofOriginID, JG.TransporterID, JG.CollectionDate, JG.DeliveryInstructions, JG.TruckRegNo, JG.DriverDetails, JG.Remarks, JG.RotationNo, JG.CarrierID, JG.CollectionPoint, JG.DeliveryNote, JG.DeliveryPoint, JG.CollectionInstructions, Convert.ToInt32(JG.InvoiceNo), JG.InvoiceDate, JG.JobID, JG.MainJobID.HasValue ? JG.MainJobID.Value : 0, JG.shippingInstruction, JG.DeliveryDate, JG.DepartingDate, JG.Flight, JG.MAWB, JG.HAWB, JG.MRN, JG.DeliveryOrderDate));
            return query;
        }

        public int AddOrUpdateCargo(JCargoDescription JCD, string UserID)
        {
            int query = Context1.SP_InsertCargoDescription(JCD.CargoDescriptionID, JCD.JobID, JCD.Mark, JCD.Description, JCD.weight, JCD.volume, JCD.Packages, JCD.GrossWeight, UserID);
            return query;
        }

        public int AddOrUpdateCharges(JInvoice JI, string UserID)
        {
            JI.CostUpdationStatus = "1";
            JI.InvoiceStatus = "1";
            int query = Context1.SP_InsertCharges(JI.InvoiceID,JI.JobID, JI.RevenueTypeID, JI.ProvisionCurrencyID, JI.ProvisionExchangeRate, JI.ProvisionForeign, JI.ProvisionHome, JI.SalesCurrencyID, JI.SalesExchangeRate, JI.SalesForeign, JI.SalesHome, JI.Cost, JI.SupplierID, JI.RevenueCode, JI.Quantity, JI.UnitID, JI.ProvisionRate, JI.SalesRate, JI.AmtInWords, JI.InvoiceStatus, JI.CostUpdationStatus, Convert.ToInt32(UserID),JI.Description, JI.Tax, JI.TaxAmount, JI.Margin);
            return query;
        }

        public int AddOrUpdateContainerDetails(JContainerDetail JConD, string UserID)
        {
            int query = Context1.SP_InsertContainerDetails(JConD.JContainerDetailID, JConD.JobID, JConD.ContainerTypeID, JConD.ContainerNo, JConD.SealNo, JConD.Description, Convert.ToInt32(UserID));
            return query;
        }

        public int AddOrUpdateBillOfEntry(JBIllOfEntry JBE, string UserID)
        {
            int query = Context1.SP_InsertBillOfEntry(JBE.BIllOfEntryID, JBE.BIllOfEntry, JBE.JobID, JBE.BillofEntryDate, JBE.ShippingAgentID, Convert.ToInt32(UserID));
            return query;
        }

        //public int AddBillOfLoading(JBIllOfLoading JBE, string UserID)
        //{
        //    int query = Context1.SP_InsertBillOfEntry(JBE.BIllOfEntry, JBE.JobID, JBE.BillofEntryDate, JBE.ShippingAgentID, Convert.ToInt32(UserID));
        //    return query;
        //}

        public int AddOrUpdateAuditLog(JAuditLog JAL, string UserID)
        {
            //string userDateTimeString = DTZC.ConvertDateTimeZone(Convert.ToDateTime(JAL.TransDate));

            //JAL.TransDate = Convert.ToDateTime(userDateTimeString);

            int query = Context1.SP_InsertAuditlog(JAL.JAuditLogID, JAL.TransDate, JAL.Remarks, JAL.JobID, Convert.ToInt32(UserID));

            return query;
        }

        public List<SP_GetAllJobsDetails_Result> AllJobsDetails()
        {
            return Context1.SP_GetAllJobsDetails().ToList();
        }

        public JobGeneration JobGenerationByJobID(int ID)
        {
            JobGeneration JG1 = new JobGeneration();

            var query = Context1.SP_GetJobGenerationByJobID(ID);

            foreach (var item in query)
            {
                JG1.JobCode = item.JobCode;
                JG1.JobDate = item.JobDate.Value;
                JG1.JobTypeID = item.JobTypeID;
                JG1.MainJobID = item.MainJobID;
                JG1.JobID = item.JobID;
                JG1.ArrivalDate = item.ArrivalDate;
                JG1.InvoiceToID = item.InvoiceToID;
                JG1.EmployeeID = item.EmployeeID;
                JG1.DeliveryPlaceID = item.DeliveryPlaceID;
                JG1.DestinationPortID = item.DestinationPortID;
                JG1.ShipperID = item.ShipperID;
                JG1.DeliveryPlaceID = item.DeliveryPlaceID;
                JG1.ReceiptPlaceID = item.ReceiptPlaceID;
                JG1.LoadPortID = item.LoadPortID;

                JG1.Freight = item.Freight;
                JG1.BLStatus= item.BLStatus1;
                JG1.CarrierID = item.CarrierID;
                JG1.CLFValue = item.CLFValue;
                JG1.CollectionDate = item.CollectionDate;
                JG1.CollectionInstructions = item.CollectionInstructions;
                JG1.CollectionPoint = item.CollectionPoint;
                JG1.ConsigneeID = item.ConsigneeID;
                JG1.ConsignerID = item.ConsignerID;
                JG1.CountryofOriginID = item.CountryofOriginID;
                JG1.DeliveryDate = item.DeliveryDate;
                JG1.DeliveryInstructions = item.DeliveryInstructions;
                JG1.DeliveryNote = item.DeliveryNote;
                JG1.DeliveryOrderDate = item.DeliveryOrderDate;
                JG1.DeliveryOrderNo = item.DeliveryOrderNo;
                JG1.DeliveryPoint = item.DeliveryPoint;
                JG1.DepositAmount = item.DepositAmount;
                JG1.DepositDate = item.DepositDate;
                JG1.DriverDetails = item.DriverDetails;
                JG1.HBL = item.HBL;
                JG1.BLStatus = item.BLStatus;
                JG1.IPTNo = item.IPTNo;
                JG1.RefNo = item.RefNo;
                JG1.MBL = item.MBL;
                JG1.RefundAmount = item.RefundAmount;
                JG1.RefundDate = item.RefundDate;
                JG1.Remarks = item.Remarks;
                JG1.RotationNo = item.RotationNo;
                JG1.SailingDate = item.SailingDate;
                JG1.VoyageNo = item.VoyageNo;
                JG1.shippingInstruction = item.shippingInstruction;
                JG1.CustRef = item.CustRef;
                JG1.MRN = item.MRN;
                JG1.ReceiptNo = item.ReceiptNo;
                JG1.BLDate = item.BLDate;
                JG1.TruckRegNo = item.TruckRegNo;
                JG1.InvoiceNo = item.InvoiceNo;
                JG1.VesselID = item.VesselID;
                JG1.TransporterID = item.TransporterID;
                JG1.HAWB = item.HAWB;
                JG1.MAWB = item.MAWB;
                JG1.Flight = item.Flight;
                JG1.DepartingDate = item.DepartingDate;
              
                //JG1.BLStatus1 = JG1.BLStatus1;




            }

            return JG1;
        }

        public string JobPrefixByJobTypeID(int JobTypeID)
        {
            string prefix = "";
            var query = Context1.SP_GetJobTypePrefix(JobTypeID);

            foreach (var item in query)
            {
                prefix = item.ToString();
            }

            return prefix;
        }

        public string MaxJobID()
        {
            string maxID = "";
            var query = Context1.SP_GetMaxJobID();

            foreach (var item in query)
            {
                maxID = item.ToString();
            }

            return maxID;
        }

        public string GetMaxInvoiceNumber()
        {
            string maxID = "";
            var query = Context1.SP_GetMaxInvoiceNumber();

            foreach (var item in query)
            {
                maxID = item.ToString();
            }

            return maxID;
        }

        public List<Supplier> GetSuppliersByRevenueTypeID(string RevenueTypeID)
        {

            var suppliers = Context1.Suppliers.Where(ite => ite.RevenuTypeIds.Contains(RevenueTypeID)).ToList();

            return suppliers;// Context1.SP_GetSupplierByRevenueTypeID(RevenueTypeID).ToList();
        }

        public string GetCurrencyExchange(int CurrencyID)
        {
            string ExRate = "";

            var query = Context1.SP_GetCurrencyExchangeRate(CurrencyID);

            foreach (var item in query)
            {
                if (item.ToString() != "")
                {
                    ExRate = item.ToString();
                }
                else
                {
                    ExRate = "0.00";
                }
            }

            return ExRate;
        }

        public List<SP_GetChargesbyJobIDandUser_Result> GetChargesByJob(int JobID, int Userid)
        {
            return Context1.SP_GetChargesbyJobIDandUser(JobID, Userid).ToList();
        }

        public List<SP_GetCargoDecbyJobIDandUser_Result> GetCargoByJob(int JobID, int Userid)
        {
            return Context1.SP_GetCargoDecbyJobIDandUser(JobID, Userid).ToList();
        }

        public List<SP_GetContainerDecbyJobIDandUser_Result> GetContainerByJob(int JobID, int Userid)
        {
            return Context1.SP_GetContainerDecbyJobIDandUser(JobID, Userid).ToList();
        }

        public List<SP_GetAuditbyJobIDandUser_Result> GetAuditByJob(int JobID, int Userid)
        {
            return Context1.SP_GetAuditbyJobIDandUser(JobID, Userid).ToList();
        }

        public List<SP_GetBillsbyJobIDandUser_Result> GetBillByJob(int JobID, int Userid)
        {
            return Context1.SP_GetBillsbyJobIDandUser(JobID, Userid).ToList();
        }

        public int DeleteContainers(int ContainerID)
        {

            int i = Context1.SP_DeleteContainerbyJobIDandUser(ContainerID);

            return i;
        }

        public int DeleteJobDetailsByJobID(int JobID,string InvoiceIds,string DeletedCargoIds,string DeletedContainerIds,string DeletedBillOfEntryIds,string DeletedAuditLogIDs)
        {
            int i = Context1.SP_DeleteJobDetailsByJobID(JobID, InvoiceIds, DeletedCargoIds, DeletedContainerIds, DeletedBillOfEntryIds, DeletedAuditLogIDs);
            return i;
        }

        public int DeleteAudit(int AuditID)
        {

            int i = Context1.SP_DeleteJAuditLogbyID(AuditID);

            return i;
        }

        public int DeleteCargo(int CargoID)
        {

            int i = Context1.SP_DeleteJCargoDescriptionbyID(CargoID);

            return i;
        }

        public int DeleteBill(int BillID)
        {

            int i = Context1.SP_DeleteBillbyID(BillID);

            return i;
        }

        public int DeleteInvoice(int InvoiceID)
        {

            int i = Context1.SP_DeleteJInvoicebyID(InvoiceID);

            return i;
        }

        public int UpdateJobIDinAllModules(int JobID, int UserID,int fyearid)
        {
            int i = Context1.SP_UpdateCargoContainerBillByJobID(JobID, UserID,fyearid);

            return i;
        }

        public int UpdateInvoiceNumber(int JobID, int InvoiceNumber, DateTime Invdate,int Fyearid)
        {
            int i = Context1.SP_UpdateInvoiceNumber(InvoiceNumber, Invdate, JobID);

            return i;
        }

        public int DeleteJobDetails(int JobID)
        {
            int i = Context1.SP_DeleteJobByJobID(JobID);

            return i;
        }
    }
}