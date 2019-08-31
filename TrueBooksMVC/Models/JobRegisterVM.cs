using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class JobRegisterVM
    {
       
        public string JobCode { get; set; }
       
        public string Customer { get; set; }
        public string JobTypeName { get; set; }
        public string ShipperName { get; set; }
        public string ConsigneeName { get; set; }
        public string InvoiceTo { get; set; }
        public string Notifyto { get; set; }
        public string EmployeeName { get; set; }
        public string LoadPort { get; set; }
        public string DestinationPort { get; set; }
        public string PlaceOfReciept { get; set; }
        public string PlaceOfDelivery { get; set; }


        public int CargoDescriptionID { get; set; }
      
        public string Mark { get; set; }
       
        public Nullable<decimal> weight { get; set; }


        public Nullable<decimal> ProvisionHome { get; set; }
        public Nullable<decimal> Cost { get; set; }

        public int JContainerDetailID { get; set; }
       
        public Nullable<int> ContainerTypeID { get; set; }
        public string ContainerType { get; set; }
        public string Containerdescription { get; set; }
        public string ContainerNo { get; set; }
        public string SealNo { get; set; }
        public string Description { get; set; }

        public string vesselname { get; set; }
        public string carrierName { get; set; }
        public string transporter { get; set; }
        public string SupplierName { get; set; }
        public string RevenueType { get; set; }
        public int BIllOfEntryID { get; set; }
        public string BIllOfEntry { get; set; }
       
        public Nullable<System.DateTime> BillofEntryDate { get; set; }
        public Nullable<int> ShippingAgentID { get; set; }
       

        public decimal? Provision { get; set; }
        public decimal? Sales { get; set; }
        public decimal? Profit { get; set; }

        public decimal? ProvExRate { get; set; }
        public decimal? SalesExRate { get; set; }

        public int JobID { get; set; }
      
        public Nullable<int> JobTypeID { get; set; }

        public Nullable<System.DateTime> JobDate { get; set; }
        public Nullable<int> ConsignerID { get; set; }
        public Nullable<int> ConsigneeID { get; set; }
        public Nullable<int> InvoiceToID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> ShipperID { get; set; }
        public string IPTNo { get; set; }
        public string RefNo { get; set; }
        public string BillOfEnquiry { get; set; }
        public Nullable<System.DateTime> BLDate { get; set; }
        public string DeliveryOrderNo { get; set; }
        public Nullable<System.DateTime> DeliveryOrderDate { get; set; }
        public string BLStatus1 { get; set; }
        public string CLFValue { get; set; }
        public Nullable<decimal> DepositAmount { get; set; }
        public Nullable<System.DateTime> DepositDate { get; set; }
        public string ReceiptNo { get; set; }
        public Nullable<System.DateTime> RefundDate { get; set; }
        public Nullable<decimal> RefundAmount { get; set; }
        public Nullable<int> VesselID { get; set; }
        public string VoyageNo { get; set; }
        public string Freight { get; set; }
        public Nullable<System.DateTime> SailingDate { get; set; }
        public Nullable<System.DateTime> ArrivalDate { get; set; }
        public string MBL { get; set; }
        public string HBL { get; set; }
        public string BLStatus { get; set; }
        public string StausInvoiceType { get; set; }
        public Nullable<int> AcJournalID { get; set; }
        public Nullable<int> AcProvisionCostJournalID { get; set; }
        public Nullable<int> LoadPortID { get; set; }
        public Nullable<int> DestinationPortID { get; set; }
        public Nullable<int> DeliveryPlaceID { get; set; }
        public Nullable<int> ReceiptPlaceID { get; set; }
        public Nullable<int> AirlineID { get; set; }
        public string Flight { get; set; }
        public string MAWB { get; set; }
        public string HAWB { get; set; }
        public Nullable<decimal> GrossWeight { get; set; }
        public Nullable<decimal> Volume { get; set; }
        public decimal Package { get; set; }
        public Nullable<int> CountryofOriginID { get; set; }
        public Nullable<bool> StatusSub { get; set; }
        public Nullable<int> MainJobID { get; set; }
        public Nullable<int> InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<int> TransporterID { get; set; }
        public Nullable<System.DateTime> CollectionDate { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public string DeliveryInstructions { get; set; }
        public string TruckRegNo { get; set; }
        public string DriverDetails { get; set; }
        public string TDN { get; set; }
        public string Remarks { get; set; }
        public string MRN { get; set; }
        public string RotationNo { get; set; }
        public Nullable<int> CarrierID { get; set; }
        public Nullable<int> QuotationID { get; set; }
        public string CollectionPoint { get; set; }
        public string DeliveryNote { get; set; }
        public string DeliveryPoint { get; set; }
        public string CollectionInstructions { get; set; }
        public Nullable<bool> IsClosed { get; set; }
        public string CustRef { get; set; }
        public Nullable<int> JobCloseAcJournalID { get; set; }
        public Nullable<bool> StatusClose { get; set; }
        public Nullable<int> PreJobID { get; set; }
        public Nullable<int> PreFYearID { get; set; }
        public Nullable<bool> statusPreInvoice { get; set; }
        public string shippingInstruction { get; set; }
        public Nullable<bool> RecPayStatus { get; set; }
        public Nullable<bool> JobStatus { get; set; }
        public string CostUpdatedOrNot { get; set; }
        public Nullable<System.DateTime> DepartingDate { get; set; }
        public Nullable<bool> Lock { get; set; }

    }
}