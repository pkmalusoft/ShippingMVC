using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class ShippingUserVM
    {
        public string AgentName { get; set; }
        public int ShippingAgentID { get; set; }
        public string ReferenceCode { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CountryName { get; set; }
    }
}