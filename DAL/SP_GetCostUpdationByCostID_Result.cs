//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    
    public partial class SP_GetCostUpdationByCostID_Result
    {
        public int CostUpdationID { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<int> JobID { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<int> AcJournalID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public string DocumentNo { get; set; }
        public Nullable<int> PrevCostupID { get; set; }
        public string SupplierPaymentStatus { get; set; }
        public Nullable<int> UserID { get; set; }
        public string JobCode { get; set; }
        public string SupplierName { get; set; }
    }
}
