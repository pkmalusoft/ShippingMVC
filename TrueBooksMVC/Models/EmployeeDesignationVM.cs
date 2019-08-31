using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class EmployeeDesignationVM
    {
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Nationality { get; set; }
        public DateTime? DOJ { get; set; }
        public Boolean StatusActive { get; set; }
        public int EmployeeID { get; set; }
    }
}