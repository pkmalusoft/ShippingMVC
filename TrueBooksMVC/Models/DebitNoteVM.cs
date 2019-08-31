using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class DebitNoteVM
    {
        public string DebitNoteNo { get; set; }
        public DateTime? Date { get; set; }
        public int SupplierID { get; set; }
        public int AcHeadID { get; set; }
        public int InvoiceNo { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Amount { get; set; }

        public string JobNo { get; set; }
        public string SupplierName { get; set; }
    }
}