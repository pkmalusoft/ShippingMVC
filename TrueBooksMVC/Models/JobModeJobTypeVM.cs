using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class JobModeJobTypeVM
    {
        public int JobTypeID { get; set; }
        public string JobDescription { get; set; }
        public string Remarks { get; set; }
        public string JobCode { get; set; }
        public string JobPrefix { get; set; }
        public Boolean StatusSea { get; set; }
        public string JobMode { get; set; }
    }
}