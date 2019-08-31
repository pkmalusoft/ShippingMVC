using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class SupplierLedgerReportVM
    {
        public int recpayId { get; set; }
        public DateTime invoicedate { get; set; }
        public string DocumentNo { get; set; }
        public string particulers { get; set; }
        public int supplierID { get; set; }
        public decimal? Credit { get; set;}
        public decimal? debit { get; set; }
        public decimal? Balence { get; set; }


    }
}