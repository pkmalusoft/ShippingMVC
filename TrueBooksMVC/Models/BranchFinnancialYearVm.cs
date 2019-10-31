using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class BranchFinnancialYearVm
    {
        public int AcFinancialYearID { get; set; }
        public int AcCompanyId { get; set; }
        public int BranchID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string BranchName { get; set; }
        public int CompanyId { get; set; }
        public int CountryID { get; set; }
        public int currencyId { get; set; }
        public string Address { get; set; }
        public string phone { get; set; }
        public string  Email { get; set; }
        public string City { get; set; }
        public string Website { get; set; }
        public string ContactPerson { get; set; }
    }
}