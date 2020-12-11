using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class ExportImportVM
    {
        
    }
    public class DatePicker
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Create { get; set; }

        public int? StatusId { get; set; }
        public int? AgentId { get; set; }
        public int? CustomerId { get; set; }

        public string CustomerName { get; set; }
        public string MovementId { get; set; }
        public int[] SelectedValues { get; set; }

        public int paymentId { get; set; }
    }

    public class AccountsReportParam
    {
        public int AcHeadId { get; set; }
        public string AcHeadName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int AcTypeId { get; set; }
        public int AcGroupId { get; set; }
        public string Output { get; set; } //printer ,pdf,word,excel
        public string ReportType { get; set; } //sumary details
        public string ReportFileName { get; set; }
        public string Filters { get; set; }


    }

    public class AWBReportParam
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Create { get; set; }

        public int? PaymentModeId { get; set; }
        public int? ParcelTypeId { get; set; }
        public string MovementId { get; set; }
        public int[] SelectedValues { get; set; }
        public string Output { get; set; }
        public string ReportType { get; set; } //sumary details
        public string ReportFileName { get; set; }
        public string Filters { get; set; }
        public string SortBy { get; set; }
    }

    public class TaxReportParam
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Create { get; set; }

        public string TransactionType { get; set; }
        public string Output { get; set; }
        public string ReportType { get; set; } //sumary details
        public string ReportFileName { get; set; }
        public string Filters { get; set; }
        public string SortBy { get; set; }
    }
}