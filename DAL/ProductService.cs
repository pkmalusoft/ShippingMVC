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
    
    public partial class ProductService
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string HSNCode { get; set; }
        public Nullable<decimal> EndUser { get; set; }
        public Nullable<decimal> Reseller { get; set; }
        public Nullable<decimal> SpecialPrice { get; set; }
        public Nullable<int> Status { get; set; }
    }
}