using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class CustomerJournalVM
    {
        public CustomerJournalVM() { }
        public int CustomerHeadId { get; set; }
        public int SupplierHeadId { get; set; }
        public bool IsCustomerSelected { get; set; }
        //public decimal Amount { get; set; }
        public bool DebitCreditID { get; set; }
        public string Remark { get; set; }
        public int AcJournalDetailID { get; set; }
        public string AcHead { get; set; }
        public decimal? amount { get; set; }


        public List<CustomerJournalVM> CustomerJournalVMDetails { get; set; }

    }
}