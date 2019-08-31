using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class MenuRoleVM
    {
        public int? MenuID { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public int? RoleId { get; set; }
        public int ParentID { get; set; }
        public long MenuAccessID { get; set; }
    }
}