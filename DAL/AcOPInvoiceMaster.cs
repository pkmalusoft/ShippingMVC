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
    
    public partial class AcOPInvoiceMaster
    {
        public int AcOPInvoiceMasterID { get; set; }
        public Nullable<int> AcHeadID { get; set; }
        public Nullable<int> AcFinancialYearID { get; set; }
        public Nullable<System.DateTime> OPDate { get; set; }
        public string Remarks { get; set; }
        public string StatusSDSC { get; set; }
        public Nullable<int> PartyID { get; set; }
    }
}
