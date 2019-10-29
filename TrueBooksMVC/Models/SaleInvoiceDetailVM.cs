using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class SalesInvoiceDetail
    {
        public int SalesInvoiceDetailID { get; set; }
        public Nullable<int> SalesInvoiceID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> ItemUnitID { get; set; }
        public string RateType { get; set; }
        public Nullable<decimal> RateLC { get; set; }
        public Nullable<decimal> RateFC { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<decimal> ValueLC { get; set; }
        public Nullable<decimal> ValueFC { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<decimal> NetValue { get; set; }
        public Nullable<int> JobID { get; set; }
        public string JobCode { get; set; }
        public string Description { get; set; }
    }
}