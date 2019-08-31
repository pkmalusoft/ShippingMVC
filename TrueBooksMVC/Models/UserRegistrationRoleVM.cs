using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrueBooksMVC.Models
{

    public class UserRegistrationRoleVM
    {


        public int UserID { get; set; }
    
        public string UserName { get; set; }
        public string password { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public string phone { get; set; }
        public bool? IsActive { get; set; }
        public int RoleID { get; set; }
        public int AcFinancialYearID { get; set; }
       
    }
}