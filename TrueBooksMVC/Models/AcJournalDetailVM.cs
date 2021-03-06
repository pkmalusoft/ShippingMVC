﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class AcJournalDetailVM
    {
        public int AcHeadID { get; set; }
        public string AcHead { get; set; }
        public string Rem { get; set; }
        public decimal Amt { get; set; }

        public int AcJournalDetID { get; set; }
        public int? SupplierID { get; set; }
        public decimal? Taxpercent { get; set; }
        public decimal? TaxAmount { get; set; }
        public string SupplierName { get; set; }

        public List<AcExpenseAllocationVM> AcExpAllocationVM { get; set; }
    }
}