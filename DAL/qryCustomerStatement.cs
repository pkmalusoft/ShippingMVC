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
    
    public partial class qryCustomerStatement
    {
        public int InvoiceID { get; set; }
        public Nullable<int> InvoiceToID { get; set; }
        public string JobCode { get; set; }
        public Nullable<int> InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public decimal AmtToBeReceived { get; set; }
        public decimal AmtAlreadyReceived { get; set; }
        public string REMARKS { get; set; }
        public Nullable<int> EmployeeID { get; set; }
    }
}
