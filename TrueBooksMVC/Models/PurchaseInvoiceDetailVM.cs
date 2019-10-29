using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class PurchaseInvoiceDetailVM
    {
            public int PurchaseInvoiceDetailID { get; set; }
            public Nullable<int> PurchaseInvoiceID { get; set; }
            public Nullable<int> ProductID { get; set; }
            public string ProductName { get; set; }
            public Nullable<int> Quantity { get; set; }
            public Nullable<int> ItemUnitID { get; set; }
            public Nullable<decimal> Rate { get; set; }
            public Nullable<decimal> RateFC { get; set; }
            public Nullable<decimal> Value { get; set; }
            public Nullable<decimal> ValueFC { get; set; }
            public Nullable<decimal> Taxprec { get; set; }
            public Nullable<decimal> Tax { get; set; }
            public Nullable<decimal> NetValue { get; set; }
            public Nullable<int> AcHeadID { get; set; }
            public string AcHead { get; set; }
            public Nullable<int> JobID { get; set; }
            public string JobCode { get; set; }
            public string Description { get; set; }
            
    }
}