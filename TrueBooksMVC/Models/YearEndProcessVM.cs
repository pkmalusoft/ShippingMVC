using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
namespace TrueBooksMVC.Models
{
    public class YearEndProcessVM
    {
        SHIPPING_FinalEntities Context1 = new SHIPPING_FinalEntities();
        public string CurrentFYearFrom { get; set; }
        public string CurrentFYearTo { get; set; }
        public string Reference { get; set; }
        public string NewFYearFrom { get; set; }
        public string NewFYearTo { get; set; }


       
        
    }
}