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
    
    public partial class SalesAndCollectionRpt_Result
    {
        public int InvoiceID { get; set; }
        public Nullable<int> JobID { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string JobCode { get; set; }
        public Nullable<System.DateTime> JobDate { get; set; }
        public string Customer { get; set; }
        public Nullable<decimal> AmtReceived { get; set; }
        public Nullable<decimal> Balance { get; set; }
    }
}
