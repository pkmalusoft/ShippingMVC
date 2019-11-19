using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class AcExpenseAllocationVM
    {
        public Nullable<int> AcHead { get; set; }
        public Nullable<decimal> ExpAllocatedAmount { get; set; }
    }
}