using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class OpeningInvoiceVM
    {
         
        public int CustomerHeadId { get; set; }
        public int SupplierHeadId { get; set; }
        public bool IsCustomerSelected { get; set; }
         public decimal? Amount { get; set; }
        public bool DebitCreditID { get; set; }
        public string Remark { get; set; }
        public DateTime OPDate { get; set; }
           public int AcjournalMasterId { get; set; }
           public decimal? InvoiceAmount { get; set; }
           public string invoiceNo { get; set; }
           public string JobCode { get; set; }
           public DateTime?  InvoiceDate { get; set; }
           public DateTime LastTransDate { get; set; }
           public string achead { get; set; }
           public int acOPInvoiceMasterID { get; set; }
           public int acopinvoiceDetailsID { get; set; }
       

           public List<OpeningInvoiceVM> listOPInvoiceVM { get; set; }
    }
}