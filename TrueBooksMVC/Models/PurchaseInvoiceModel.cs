﻿using System;
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
            
            ObjectParameter objMaxInvoiceId = new ObjectParameter("PurchaseInvoiceID", 0);
            int query = Convert.ToInt32(Context1.SP_InsertPurchaseInvoice(PI.PurchaseInvoiceID, PI.PurchaseInvoiceNo, PI.PurchaseInvoiceDate, PI.Reference
                , PI.LPOReference, PI.SupplierID,PI.EmployeeID, PI.QuotationID, PI.CurrencyID, PI.ExchangeRate
                , PI.CreditDays, PI.DueDate, PI.AcJournalID, PI.BranchID, PI.Discount, PI.StatusDiscountAmt,PI.OtherCharges, PI.PaymentTerm, PI.Remarks, PI.FYearID));                                   
                       
            return Convert.ToInt32(objMaxInvoiceId.Value);
        }

        public int UpdateJob(PurchaseInvoice PI)
        {
            int query = Convert.ToInt32(Context1.SP_UpdatePurchaseInvoice(PI.PurchaseInvoiceID, PI.PurchaseInvoiceNo, PI.PurchaseInvoiceDate, PI.Reference, PI.LPOReference, PI.SupplierID, PI.EmployeeID, PI.QuotationID, PI.CurrencyID, PI.ExchangeRate, PI.CreditDays, PI.DueDate, PI.AcJournalID, PI.BranchID, PI.Discount, PI.StatusDiscountAmt, PI.OtherCharges, PI.PaymentTerm, PI.Remarks, PI.FYearID));
            return query;
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
