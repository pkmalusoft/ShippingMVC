using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class PortCountryVM
    {
        public string Port { get; set; }
        public string PortCode { get; set; }
        public string CountryName { get; set; }
        public int PortID { get; set; }
    }
}