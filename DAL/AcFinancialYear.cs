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
    
    public partial class AcFinancialYear
    {
        public AcFinancialYear()
        {
            this.AcOpeningMasters = new HashSet<AcOpeningMaster>();
        }
    
        public int AcFinancialYearID { get; set; }
        public Nullable<int> AcCompanyID { get; set; }
        public Nullable<System.DateTime> AcFYearFrom { get; set; }
        public Nullable<System.DateTime> AcFYearTo { get; set; }
        public string ReferenceName { get; set; }
        public Nullable<bool> StatusClose { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<bool> Lock { get; set; }
    
        public virtual ICollection<AcOpeningMaster> AcOpeningMasters { get; set; }
    }
}