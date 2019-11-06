using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class costUpdationVM
    {

        public int CostUpdationID { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<int> SelectedSupplierID { get; set; }
        public Nullable<int> JobID { get; set; }
        public List<int> MultiJobID { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<int> AcJournalID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public string DocumentNo { get; set; }
        public Nullable<int> PrevCostupID { get; set; }
        public string SupplierPaymentStatus { get; set; }
        public Nullable<int> UserID { get; set; }
        public string ReferenceNo { get; set; }
        public string SupplierName { get; set; }
        public String JobCode { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public Nullable<decimal> InvoiceAmount { get; set; }

        public virtual List<costUpdationDetailVM> CostUpdationDetails { get; set; }

    }
}