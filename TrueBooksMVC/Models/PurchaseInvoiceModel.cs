using System;
using System.Web;
using TrueBooksMVC.Models;
using System.Data.SqlClient;
using System.Data;
using DAL;
using System.Data.Entity;
using System.Data.Objects;

namespace TrueBooksMVC.Models
{
    public class PurchaseInvoiceModel
    {       

        SHIPPING_FinalEntities Context1 = new SHIPPING_FinalEntities();
        DateTimeZoneConversionModel DTZC = new DateTimeZoneConversionModel();

        public int AddPurchaseInvoice(PurchaseInvoice PI)
        {
            int result = DAL.SP_InsertPurchaseInvoice(PI);

            return result;
         
        }

        public int UpdatePurchaseInvoice(PurchaseInvoice PI)
        {
            int result = DAL.SP_UpdatePurchaseInvoice(PI);

            return result;

           
        }
        public int DeletePurchaseInvoice(int PurchaseInvoiceId)
        {
            int result = DAL.SP_DeletePurchaseInvoice(PurchaseInvoiceId);

            return result;


        }

        public PurchaseInvoice GetPurchaseInvoiceByID(int ID)
        {
            PurchaseInvoice PI = new PurchaseInvoice();
            PI.PurchaseInvoiceID = 0;
            PI.PurchaseInvoiceDate = DateTime.Now;

            var query = Context1.SP_GetPurchaseInvoiceByID(ID);
            foreach (var item in query)
            {
                PI.PurchaseInvoiceID = item.PurchaseInvoiceID;
                PI.PurchaseInvoiceNo = item.PurchaseInvoiceNo;
                PI.PurchaseInvoiceDate = item.PurchaseInvoiceDate;
                PI.Reference = item.Reference;
                PI.LPOReference = item.LPOReference;
                PI.SupplierID = item.SupplierID;
                PI.EmployeeID = item.EmployeeID;
                PI.QuotationNumber = item.QuotationNumber;
                PI.CurrencyID = item.CurrencyID;
                PI.ExchangeRate = item.ExchangeRate;
                PI.CreditDays = item.CreditDays;
                PI.DueDate = item.DueDate;
                PI.AcJournalID = item.AcJournalID;
                PI.BranchID = item.BranchID;
                PI.Discount = item.Discount;
                PI.StatusDiscountAmt = item.StatusDiscountAmt;
                PI.OtherCharges = item.OtherCharges;
                PI.PaymentTerm = item.PaymentTerm;
                PI.Remarks = item.Remarks;
                PI.FYearID = item.FYearID;
                PI.DiscountType = item.DiscountType;
                PI.DiscountValueFC = item.DiscountValueFC;
                PI.DiscountValueLC = item.DiscountValueLC;


            }

            return PI;
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
