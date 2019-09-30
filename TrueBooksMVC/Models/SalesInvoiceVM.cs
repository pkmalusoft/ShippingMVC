using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrueBooksMVC.Models
{
    public class SalesInvoiceVM 
    {
        public string RevenueType { get; set; }
        public string AcHead { get; set; }
        public string RevenueCode { get; set; }
        public int RevenueTypeID { get; set; }

    }
}
