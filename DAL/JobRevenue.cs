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
    
    public partial class JobRevenue
    {
        public int JobRevenueID { get; set; }
        public Nullable<int> JobTypeID { get; set; }
        public Nullable<int> RevenueTypeID { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Cost { get; set; }
    }
}
