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
    
    public partial class CurrencyRate
    {
        public int CurrencyRateID { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public Nullable<decimal> CurrencyRate1 { get; set; }
    }
}