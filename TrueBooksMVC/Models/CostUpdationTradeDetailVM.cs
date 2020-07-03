using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class CostUpdationTradeDetailVM
    {
        public int InvoiceID { get; set; }
        public Nullable<int> JobID { get; set; }
        public Nullable<int> RevenueTypeID { get; set; }
        public Nullable<int> ProvisionCurrencyID { get; set; }
        public Nullable<decimal> ProvisionExchangeRate { get; set; }
        public Nullable<decimal> ProvisionForeign { get; set; }
        public Nullable<decimal> ProvisionHome { get; set; }
        public Nullable<int> SalesCurrencyID { get; set; }
        public Nullable<decimal> SalesExchangeRate { get; set; }
        public Nullable<decimal> SalesForeign { get; set; }
        public Nullable<decimal> SalesHome { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public string RevenueCode { get; set; }
        public Nullable<int> tempInvID { get; set; }
        public Nullable<int> PreInvID { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<decimal> ProvisionRate { get; set; }
        public Nullable<decimal> SalesRate { get; set; }
        public string AmtInWords { get; set; }
        public Nullable<int> ProvisionQty { get; set; }
        public Nullable<int> SalesQty { get; set; }
        public string InvoiceStatus { get; set; }
        public string CostUpdationStatus { get; set; }
        public Nullable<int> UserID { get; set; }
        public string RevenueType { get; set; }
        public string SupplierName { get; set; }
        public string CurrencyName { get; set; }
        public string JobCode { get; set; }
        public int CostUpdationDetailsID { get; set; }
        public string Referenceno { get; set; }
        public decimal AmountPaid { get; set; }
        public string supplierReference { get; set; }
        public int CostUpdationDetailID { get; set; }
        public int CostUpdationID { get; set; }
        public Nullable<decimal> Variance { get; set; }
        public Nullable<int> JInvoiceID { get; set; }
        public Nullable<int> PrevCostDetailID { get; set; }
        public Nullable<decimal> AmountPaidTillDate { get; set; }
        public Nullable<decimal> InvoiceAmount { get; set; }
        public string PaidOrNot { get; set; }
        public string SupplierPayStatus { get; set; }
        public Nullable<bool> Lock { get; set; }
        public decimal? Balance { get; set; }
        public string DateTime { get; set; }
        public decimal? AdjustmentAmount { get; set; }
        public int? PurchaseInvoiceId { get; set; }
        public int? PurchaseInvoiceDetailId { get; set; }
        public string InvoiceNo { get; set; }
    }
}