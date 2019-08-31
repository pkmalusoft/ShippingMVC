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
    
    public partial class SP_GetAllCustomers_Result
    {
        public int CustomerID { get; set; }
        public string Customer { get; set; }
        public string ReferenceCode { get; set; }
        public string ContactPerson { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public Nullable<int> MaxCreditDays { get; set; }
        public Nullable<double> MaxCreditLimit { get; set; }
        public Nullable<float> DiscountPercentage { get; set; }
        public Nullable<bool> StatusActive { get; set; }
        public Nullable<bool> StatusBlocked { get; set; }
        public Nullable<bool> StatusConsignor { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> StatusReserved { get; set; }
        public string ExporterCode { get; set; }
        public string POBoxNo { get; set; }
        public Nullable<bool> InvoiceTo { get; set; }
    }
}
