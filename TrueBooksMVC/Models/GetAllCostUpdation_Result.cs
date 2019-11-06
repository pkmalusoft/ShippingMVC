using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class GetAllCostUpdation_Result
    {
        public int SINo { get; set; }
        public string SupplierInvoiceNumber { get; set; }
        public Nullable<DateTime> InvoiceDate { get; set; }
        public string SupplierName { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<decimal> InvoiceAmount { get; set; }
        public string JobNumbers { get; set; }
        public string JobDates { get; set; }
        public string JobValues { get; set; }
        public string YearToDateInvoiced { get; set; }
        public Nullable<int> CostUpdationID { get; set; }
    }
}
