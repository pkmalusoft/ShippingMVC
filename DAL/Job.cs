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
    
    public partial class Job
    {
        public int JobID { get; set; }
        public Nullable<System.DateTime> JobDate { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string DocumentNo { get; set; }
        public Nullable<int> PaymentTermID { get; set; }
        public Nullable<int> ValidityID { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> LocationID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> User1 { get; set; }
        public Nullable<int> LoadPortID { get; set; }
        public Nullable<int> DestinationPortID { get; set; }
        public Nullable<int> ConsigneeID { get; set; }
        public Nullable<int> QuotationID { get; set; }
        public Nullable<int> JobTypeID { get; set; }
        public string JobCode { get; set; }
    }
}
