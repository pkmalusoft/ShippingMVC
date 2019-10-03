using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrueBooksMVC.Models
{
    public class PurchaseInvoiceVM
    {
        public int PurchaseInvoiceID { get; set; }
        public string PurchaseInvoiceNo { get; set; }
        public Nullable<System.DateTime> PurchaseInvoiceDate { get; set; }
        public string Reference { get; set; }
        public string LPOReference { get; set; }
        public int SupplierID { get; set; }
        public int EmployeeID { get; set; }
        public int QuotationID { get; set; }
        public int CurrencyID { get; set; }
        public int AcJournalID { get; set; }
        public int CreditDays { get; set; }
        public int BranchID { get; set; }
        public int FYearID { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> OtherCharges { get; set; }
        public string PaymentTerm { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public int PurchaseInvoiceDetailID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int ItemUnitID { get; set; }
        public int CustomerRateTypeID { get; set; }
        public int JobID { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public string Description { get; set; }

    }
}
