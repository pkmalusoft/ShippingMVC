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
            int result = DAL.SP_InsertSalesInvoice(SI);

            return result;
        }

        public int UpdateSalesInvoice(SalesInvoice SI)
        {

            int result = DAL.SP_UpdateSalesInvoice(SI);

            return result;

            
        }

        public int DeleteSalesInvoice(int SalesInvoiceId)
        { 
        
            int result = DAL.SP_DeleteSalesInvoice(SalesInvoiceId);

            return result;


        }

        public SalesInvoice GetSalesInvoiceByID(int ID)
        {
            SalesInvoice SI = new SalesInvoice();
            SI.SalesInvoiceID = 0;
            SI.SalesInvoiceDate = DateTime.Now;
            SI.DueDate = DateTime.Now;
            var query = Context1.SP_GetSalesInvoiceByID(ID);
            foreach (var item in query)
            {
                SI.SalesInvoiceID = item.SalesInvoiceID;
                SI.SalesInvoiceNo = item.SalesInvoiceNo;
                SI.SalesInvoiceDate = item.SalesInvoiceDate;
                SI.QuotationNumber = item.QuotationNumber;
                SI.Reference = item.Reference;
                SI.LPOReference = item.LPOReference;
                SI.SalesInvoiceID = item.SalesInvoiceID;
                SI.CustomerID = item.CustomerID;
                SI.EmployeeID = item.EmployeeID;
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
                SI.DeliveryId = item.DeliveryId;
                SI.DiscountType = item.DiscountType;
                SI.DiscountValueLC = item.DiscountValueLC;
                SI.DiscountValueFC = item.DiscountValueFC;

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



  