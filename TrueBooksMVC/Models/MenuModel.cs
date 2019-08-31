using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class MenuModel
    {
        public int MenuID { get; set; }
        public string Title { get; set; }
        public int ParentID { get; set; }
        public int Ordering { get; set; }
    }
}