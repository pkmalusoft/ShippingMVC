using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class JobInvoiceModel
    {

        //SL.NO INVOICE_NO.INVOICE_DATE   JOB_NUMBER CUSTOMER   AMOUNT PAYMENT_STATUS  ACTION INVOICE_STATUS
        public int Id { get; set; }
        public int? JobID { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string JobNumber { get; set; }
        public string Supplier { get; set; }
        public decimal? Amount { get; set; }
        public string PaymentStatus { get; set; }
        public string InvoiceStatus { get; set; }
        public Boolean? IsCancelledInvoice { get; set; }
       

    }
}