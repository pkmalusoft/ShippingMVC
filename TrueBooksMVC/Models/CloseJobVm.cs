using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class CloseJobVm
    {

        public int JobDetailID { get; set; }
        public Nullable<int> JobID { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<int> PriceID { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Remarks { get; set; }
        public string StatusQtnDetail { get; set; }
        public string StatusQtn { get; set; }
        public Nullable<int> ReferenceID { get; set; }
        public Nullable<int> ContainerTypeID { get; set; }
        public string Volume { get; set; }
        public Nullable<int> RevenueTypeID { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public bool IsSelected { get; set; }
        public string JobCode { get; set; }
        public DateTime JobDate { get; set; }
        public string JobDescription { get; set; }
        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public string Customer { get; set; }
        public int InvoiceNo { get; set; }
        public Nullable<DateTime> InvoiceDate { get; set; }
        List<CloseJobVm> lst1  { get; set; }

    }
}