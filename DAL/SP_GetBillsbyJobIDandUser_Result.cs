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
    
    public partial class SP_GetBillsbyJobIDandUser_Result
    {
        public int BIllOfEntryID { get; set; }
        public string BIllOfEntry { get; set; }
        public Nullable<int> JobID { get; set; }
        public Nullable<System.DateTime> BillofEntryDate { get; set; }
        public Nullable<int> ShippingAgentID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string AgentName { get; set; }
    }
}
