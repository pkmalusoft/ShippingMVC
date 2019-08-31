using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class JobAnalysisReportVM
    {
        public string Jobtype { get; set; }
        public int JobTypeID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string JobCode { get; set; }
        public decimal? ActualCost { get; set; }
        public decimal? SalesHome { get; set; }
        public decimal? Profit { get; set; }
        public int invoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int JobID { get; set; }
        public decimal? ProfitPercent { get; set; }
        public decimal? Cost { get; set; }

       
    }
}