using System.Data.SqlClient;
using System.Data;
using DAL;
using System.Data.Entity;
using System.Data.Objects;
using System;

namespace TrueBooksMVC.Models
{
    public class SalesInvoiceModel 
    {
        SHIPPING_FinalEntities Context1 = new SHIPPING_FinalEntities();
        DateTimeZoneConversionModel DTZC = new DateTimeZoneConversionModel();

        public int AddSalesInvoice(SalesInvoice SI)
        {

            ObjectParameter objMaxInvoiceId = new ObjectParameter("SalesInvoiceID", 0);
            int query = Convert.ToInt32(Context1.SP_InsertSalesInvoice(SI.SalesInvoiceID, SI.SalesInvoiceNo, SI.SalesInvoiceDate, SI.Reference
                , SI.LPOReference, SI.CustomerID, SI.EmployeeID, SI.QuotationID, SI.CurrencyID, SI.ExchangeRate
                , SI.CreditDays, SI.DueDate, SI.AcJournalID, SI.BranchID, SI.Discount, SI.StatusDiscountAmt, SI.OtherCharges, SI.PaymentTerm, SI.Remarks, SI.FYearID));

            return Convert.ToInt32(objMaxInvoiceId.Value);
        }

        public int UpdateJob(SalesInvoice SI)
        {
            int query = Convert.ToInt32(Context1.SP_UpdatePurchaseInvoice(SI.SalesInvoiceID, SI.SalesInvoiceNo, SI.SalesInvoiceDate, SI.Reference
                , SI.LPOReference, SI.CustomerID, SI.EmployeeID, SI.QuotationID, SI.CurrencyID, SI.ExchangeRate
                , SI.CreditDays, SI.DueDate, SI.AcJournalID, SI.BranchID, SI.Discount, SI.StatusDiscountAmt, SI.OtherCharges, SI.PaymentTerm, SI.Remarks, SI.FYearID));
            return query;
        }


        public SalesInvoice GetSalesInvoiceByID(int ID)
        {
            SalesInvoice SI = new SalesInvoice();
            SI.SalesInvoiceID = 0;
            SI.SalesInvoiceDate = DateTime.Now;

            var query = Context1.SP_GetSalesInvoiceByID(ID);
            foreach (var item in query)
            {
                SI.SalesInvoiceID = item.SalesInvoiceID;

                SI.SalesInvoiceNo = item.SalesInvoiceNo;
                SI.SalesInvoiceDate = item.SalesInvoiceDate;
                SI.Reference = item.Reference;
                SI.LPOReference = item.LPOReference;
                SI.SalesInvoiceID = item.SalesInvoiceID;
                SI.CustomerID = item.CustomerID;
                SI.EmployeeID = item.EmployeeID;
                SI.QuotationID = item.QuotationID;
                SI.CurrencyID = item.CurrencyID;
                SI.ExchangeRate = item.ExchangeRate;
                SI.CreditDays = item.CreditDays;
                SI.DueDate = item.DueDate;
                SI.AcJournalID = item.AcJournalID;
                SI.BranchID = item.BranchID;
                SI.Discount = item.Discount;
                SI.StatusDiscountAmt = item.StatusDiscountAmt;
                SI.OtherCharges = item.OtherCharges;
                SI.PaymentTerm = item.PaymentTerm;
                SI.Remarks = item.Remarks;
                SI.FYearID = item.FYearID;

       

    }

            return SI;

    }

      

        public string GetCurrencyExchange(int CurrencyID)
        {
            string ExRate = "";

            var query = Context1.SP_GetCurrencyExchangeRate(CurrencyID);

            foreach (var item in query)
            {
                if (item.ToString() != "")
                {
                    ExRate = item.ToString();
                }
                else
                {
                    ExRate = "0.00";
                }
            }

            return ExRate;
        }
    }
}



  