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
    
    public partial class CustomerEnquiryDetail
    {
        public int CustomerEnquiryDetailID { get; set; }
        public Nullable<int> CustomerEnquiryID { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<int> UnitID { get; set; }
        public string Remarks { get; set; }
        public string StatusQtnDetail { get; set; }
        public string StatusQtn { get; set; }
        public Nullable<int> ReferenceID { get; set; }
        public Nullable<int> ContainerTypeID { get; set; }
        public string Volume { get; set; }
    }
}