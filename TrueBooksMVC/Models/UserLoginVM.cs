using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class UserLoginVM
    {
        public string UserName { get; set; }

        public string Password { get; set; }
        public string NewPassword { get; set; }
        public int BranchID { get; set; }


        public int AcFinancialYearID { get; set; }
        public string UserType { get; set; }
    }
}