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
    
    public partial class GetBankReconciliation_Result
    {
        public int AcJournalID { get; set; }
        public int AcBankDetailID { get; set; }
        public string AcHead { get; set; }
        public string VoucherNo { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string PartyName { get; set; }
        public string Remarks { get; set; }
        public string VoucherType { get; set; }
        public int AcJournalID1 { get; set; }
        public Nullable<bool> StatusReconciled { get; set; }
    }
}
