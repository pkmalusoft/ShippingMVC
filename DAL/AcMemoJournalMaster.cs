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
    using System.Collections.Generic;
    
    public partial class AcMemoJournalMaster
    {
        public int AcMemoJournalID { get; set; }
        public Nullable<int> AcJournalID { get; set; }
        public string VoucherNo { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public Nullable<int> AcFinancialYearID { get; set; }
        public string VoucherType { get; set; }
        public Nullable<bool> StatusDelete { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> AcCompanyID { get; set; }
    }
}