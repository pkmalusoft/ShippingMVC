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
    
    public partial class UMUserMaster
    {
        public int UID { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Name { get; set; }
        public Nullable<int> GID { get; set; }
        public Nullable<bool> Blocked { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> User1 { get; set; }
    }
}