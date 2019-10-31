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
    
    public partial class AcOpeningMaster
    {
        public int AcOpeningID { get; set; }
        public Nullable<int> AcFinancialYearID { get; set; }
        public Nullable<System.DateTime> OPDate { get; set; }
        public Nullable<int> AcHeadID { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> PartyID { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<bool> StatusImport { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<int> AssetMasterID { get; set; }
        public Nullable<int> AcCompanyID { get; set; }
        public string InvRemarks { get; set; }
    
        public virtual AcHead AcHead { get; set; }
        public virtual AcFinancialYear AcFinancialYear { get; set; }
        public virtual AcCompany AcCompany { get; set; }
    }
}
