//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class SeaFright
    {
        public int SeaFrightID { get; set; }
        public Nullable<int> ShippingLineID { get; set; }
        public Nullable<int> LoadPortID { get; set; }
        public Nullable<int> DestinationPortID { get; set; }
        public Nullable<int> ReceiptPlaceID { get; set; }
        public Nullable<int> DestinationPlaceID { get; set; }
        public Nullable<int> CommodityID { get; set; }
        public string OriginDocumentation { get; set; }
        public Nullable<decimal> OtherOriginCharges { get; set; }
        public string TransitTime { get; set; }
        public string Frequency { get; set; }
        public Nullable<System.DateTime> Validity { get; set; }
        public string Remarks { get; set; }
    }
}