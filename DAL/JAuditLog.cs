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
    
    public partial class JAuditLog
    {
        public int JAuditLogID { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> LoginID { get; set; }
        public Nullable<int> JobID { get; set; }
        public Nullable<int> UserID { get; set; }
    }
}
