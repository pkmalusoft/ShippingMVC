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
    
    public partial class Menu
    {
        public int MenuID { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public Nullable<int> ParentID { get; set; }
        public Nullable<int> Ordering { get; set; }
        public Nullable<int> SubLevel { get; set; }
        public string RoleID { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> IsActive { get; set; }
        public string imgclass { get; set; }
        public Nullable<bool> PermissionRequired { get; set; }
        public Nullable<int> MenuOrder { get; set; }
        public Nullable<bool> IsAccountMenu { get; set; }
    }
}
