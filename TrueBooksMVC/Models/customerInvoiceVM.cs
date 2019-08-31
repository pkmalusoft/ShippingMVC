using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class customerInvoiceVM
    {
        public int InvoiceNo { get; set; }
        public Nullable<DateTime>InvoiceDate { get; set; }
        public string Job { get; set; }
        public string Currency { get; set; }
        public Decimal? InvoiceAmount { get; set; }
        public Decimal? AmountRecieved { get; set; }
        public Decimal? Balence { get; set; }
        public int JobId { get; set; }
        public int CurrencyID { get; set; }
        
        
    }
}