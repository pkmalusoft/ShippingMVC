using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace TrueBooksMVC.Models
{
    public class SupplierMasterVM:Supplier
    {
        public string SupplierInfo { get; set; }
    }
}