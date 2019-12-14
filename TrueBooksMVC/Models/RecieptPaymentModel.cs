﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueBooksMVC.Models;
using System.Data.SqlClient;
using System.Data;
using DAL;
using System.Data.Entity;

namespace TrueBooksMVC.Models
{
    public class RecieptPaymentModel
    {
        SHIPPING_FinalEntities Context1 = new SHIPPING_FinalEntities();

        public List<SP_GetAllRecieptsDetails_Result> GetAllReciepts()
        {
            return Context1.SP_GetAllRecieptsDetails().ToList();
        }

        public List<SP_GetAllPaymentsDetails_Result> GetAllPayments()
        {
            return Context1.SP_GetAllPaymentsDetails().ToList();
        }

        public string GetMaxRecieptDocumentNo()
        {
            string Docno = "";

            var quary = Context1.SP_GetMaxRVID();

            foreach (var item in quary)
            {

                Docno = item.ToString();
            }

            return Docno;
        }

        public string GetMaxPaymentDocumentNo()
        {
            string Docno = "";

            var quary = Context1.SP_GetMaxPVID();

            foreach (var item in quary)
            {
                Docno = item.ToString();
            }

            return Docno;
        }

        public List<SP_GetExchageRateByCurrencyID_Result> GetExchgeRateByCurID(int CurID)
        {
            // string ExRate = "";

            return Context1.SP_GetExchageRateByCurrencyID(CurID).ToList();

            //foreach (var item in quary)
            //{
            //    ExRate = item.ToString();
            //}

            // return ExRate;
        }

        public List<SP_GetCustomerInvoiceDetailsForReciept_Result> GetCustomerInvoiceDetails(int CustomerID,DateTime fromdate,DateTime todate)
        {

            //todo:fix to run by sethu
            //  return Context1.SP_GetCustomerInvoiceDetailsForReciept(CustomerID, fromdate, todate).ToList();
            return Context1.SP_GetCustomerInvoiceDetailsForReciept(CustomerID).ToList();
        }

        public List<SP_GetSupplierCostDetailsForPayment_Result> GetSupplierCostDetails(int SupplierID)
        {
            return Context1.SP_GetSupplierCostDetailsForPayment(SupplierID).ToList();
        }

        public int AddCustomerRecieptPayment(CustomerRcieptVM RecPy, string UserID)
        {
            int query = Context1.SP_InsertRecPay(RecPy.RecPayDate, RecPy.DocumentNo, RecPy.CustomerID, RecPy.SupplierID, RecPy.BusinessCentreID, RecPy.BankName, RecPy.ChequeNo, RecPy.ChequeDate, RecPy.Remarks, RecPy.AcJournalID, RecPy.StatusRec, RecPy.StatusEntry, RecPy.StatusOrigin, RecPy.FYearID, RecPy.AcCompanyID, RecPy.EXRate, RecPy.FMoney, Convert.ToInt32(UserID));

            return query;
        }

        public decimal GetAdvanceAmount(int CustomerID)
        {
            decimal AdvanceAmt = 0;

            var query = Context1.SP_GetAdvanceAmountOfCustomer(CustomerID);

            foreach (var item in query)
            {
                AdvanceAmt = Convert.ToDecimal(item);
            }

            return AdvanceAmt;
        }

        public int InsertRecpayDetailsForCust(int RecPayID, int InvoiceID,int JInvoiceID, decimal Amount, string Remarks, string StatusInvoice, bool StatusAdvance, string statusReceip, string InvDate, string InvNo, int CurrencyID, int invoiceStatus,int JobID)
        {
            int query = Context1.SP_InsertRecPayDetailsForCustomer(RecPayID, JInvoiceID
                , Amount, Remarks, StatusInvoice
                , StatusAdvance, statusReceip, InvDate
                , InvNo, CurrencyID, invoiceStatus, JobID);

            return query;
        }

        public int InsertRecpayDetailsForSup(int RecPayID, int InvoiceID,int JInvoiceID, decimal Amount, string Remarks, string StatusInvoice, bool StatusAdvance, string statusReceip, string InvDate, string InvNo, int CurrencyID, int invoiceStatus, int JobID)
        {
            //todo:fix to run by sethu
            int query = Context1.SP_InsertRecPayDetailsForSupplier(RecPayID, InvoiceID, Amount, Remarks, StatusInvoice, StatusAdvance, statusReceip, InvDate, InvNo, CurrencyID, invoiceStatus, JobID);

            return query;
        }

        public CustomerRcieptVM GetRecPayByRecpayID(int RecpayID)
        {
            CustomerRcieptVM cust = new CustomerRcieptVM();

            if (RecpayID <= 0)
                return new CustomerRcieptVM();
            var query = Context1.SP_GetCustomerRecieptByRecPayID(RecpayID);

            if (query != null)
            {
                var item = query.FirstOrDefault();
                cust.RecPayDate = item.RecPayDate;
                cust.DocumentNo = item.DocumentNo;
                cust.CustomerID = item.CustomerID;

                var cashOrBankID = (from t in Context1.AcHeads where t.AcHead1 == item.BankName select t.AcHeadID).FirstOrDefault();
                cust.CashBank = (cashOrBankID).ToString();
                cust.ChequeBank = (cashOrBankID).ToString();
                cust.ChequeNo = item.ChequeNo;
                cust.ChequeDate = item.ChequeDate;
                cust.Remarks = item.Remarks;
                cust.EXRate = item.EXRate;
                cust.FMoney = item.FMoney;
                cust.RecPayID = item.RecPayID;
                cust.SupplierID = item.SupplierID;
                cust.AcJournalID = item.AcJournalID;
                cust.StatusEntry = item.StatusEntry;

                var a = (from t in Context1.RecPayDetails where t.RecPayID == RecpayID select t.CurrencyID).FirstOrDefault();
                cust.CurrencyId = Convert.ToInt32(a.HasValue ? a.Value : 0);



            }

            else
            {
                return new CustomerRcieptVM();
            }

            return cust;
        }

        public CustomerRcieptVM GetSupplierRecPayByRecpayID(int RecpayID)
        {
            CustomerRcieptVM cust = new CustomerRcieptVM();
            if (RecpayID <= 0)
                return new CustomerRcieptVM();
            var query = Context1.SP_GetSupplierPaymentByRecPayID(RecpayID);

            if (query != null)
            {
                var item = query.FirstOrDefault();
                cust.RecPayDate = item.RecPayDate;
                cust.DocumentNo = item.DocumentNo;
                cust.SupplierID = item.SupplierID;
                var cashOrBankID = (from t in Context1.AcHeads where t.AcHead1 == item.BankName select t.AcHeadID).FirstOrDefault();
                cust.CashBank = (cashOrBankID).ToString();
                cust.ChequeBank = (cashOrBankID).ToString();
                cust.ChequeNo = item.ChequeNo;
                cust.ChequeDate = item.ChequeDate;
                cust.Remarks = item.Remarks;
                cust.EXRate = item.EXRate;
                cust.FMoney = item.FMoney;
                cust.RecPayID = item.RecPayID;
                cust.AcJournalID = item.AcJournalID;
                cust.StatusEntry = item.StatusEntry;
                var a = (from t in Context1.RecPayDetails where t.RecPayID == RecpayID select t.CurrencyID).FirstOrDefault();
                if (a.HasValue)
                    cust.CurrencyId = Convert.ToInt32(a.Value);


            }

            return cust;
        }

        public int GetMaxRecPayID()
        {
            int RecPayID = 0;
            var query = Context1.SP_GetMaxRecPayID();

            foreach (var item in query)
            {
                RecPayID = item.Value;
            }
            return RecPayID;
        }

        public void InsertJournalOfCustomer(int RecpayID, int fyaerId)
        {
            Context1.SP_InsertJournalEntryForRecPay(RecpayID, fyaerId);
        }

        public void InsertJournalOfSupplier(int RecpayID, int fyaerId)
        {
            Context1.SP_InsertJournalEntryForRecPay_SupplierPayment(RecpayID, fyaerId);
        }

        public int UpdateCostStatus(int CostUpdationID)
        {
            int i = Context1.SP_UpdateCostUpdatonStatus(CostUpdationID);

            return i;
        }

        public List<SP_GetJInvoiceDetailsByInvoiceID_Result> InvDtls(int InvoiceID)
        {

            return Context1.SP_GetJInvoiceDetailsByInvoiceID(InvoiceID).ToList();
        }

        public int DeleteCustomerDetails(int RecPayID)
        {
            int i = Context1.SP_DeleteCustomerReciepts(RecPayID);

            return i;
        }

        public int DeleteSupplierDetails(int RecPayID)
        {
            int i = Context1.SP_DeleteSupplierPayment(RecPayID);

            return i;
        }

        public int EditCustomerRecieptDetails(List<RecPayDetail> rpayDetails, int recpayID)
        {
            //code for edit
            try
            {
                foreach (var CU in rpayDetails)
                {
                    if (recpayID > 0)
                    {
                        RecPayDetail objrpayDetails = Context1.RecPayDetails.Where(item => item.RecPayDetailID == CU.RecPayDetailID).FirstOrDefault();
                        objrpayDetails.Amount = CU.Amount;
                        objrpayDetails.CurrencyID = CU.CurrencyID;
                        objrpayDetails.InvDate = CU.InvDate;

                        objrpayDetails.InvNo = CU.InvNo;

                        objrpayDetails.InvoiceID = CU.InvoiceID;

                        objrpayDetails.Remarks = CU.Remarks;
                        Context1.Entry(objrpayDetails).State = EntityState.Modified;
                    }
                    Context1.SaveChanges();
                }
                return 1;
            }
            catch (Exception)
            {

                return 0;
            }


        }


        public int EditSupplierRecieptDetails(List<RecPayDetail> rpayDetails, int recpayID)
        {
            //code for edit
            try
            {
                foreach (var CU in rpayDetails)
                {
                    if (recpayID > 0)
                    {
                        RecPayDetail objrpayDetails = Context1.RecPayDetails.Where(item => item.RecPayDetailID == CU.RecPayDetailID).FirstOrDefault();
                        objrpayDetails.Amount = CU.Amount;
                        objrpayDetails.CurrencyID = CU.CurrencyID;
                        objrpayDetails.InvDate = CU.InvDate;

                        objrpayDetails.InvNo = CU.InvNo;

                        objrpayDetails.InvoiceID = CU.InvoiceID;

                        objrpayDetails.Remarks = CU.Remarks;
                        Context1.Entry(objrpayDetails).State = EntityState.Modified;
                    }
                    Context1.SaveChanges();
                }
                return 1;
            }
            catch (Exception)
            {

                return 0;
            }


        }

        public int EditCustomerRecPay(CustomerRcieptVM RecPy, string UserID)
        {
            if (RecPy.RecPayID > 0)
            {
                try
                {
                    //Edit Code
                    RecPay objRecPay = Context1.RecPays.Where(item => item.RecPayID == RecPy.RecPayID).FirstOrDefault();

                    objRecPay.BankName = RecPy.BankName;
                    objRecPay.ChequeDate = RecPy.ChequeDate;
                    objRecPay.ChequeNo = RecPy.ChequeNo;
                    objRecPay.EXRate = RecPy.EXRate;
                    objRecPay.FMoney = RecPy.FMoney;
                    objRecPay.RecPayDate = RecPy.RecPayDate;
                    objRecPay.Remarks = RecPy.Remarks;
                    objRecPay.FYearID = RecPy.FYearID;
                    //objRecPay.AcJournalID = RecPy.AcJournalID;
                    objRecPay.CustomerID = RecPy.CustomerID;
                    objRecPay.UserID = Convert.ToInt32(UserID);
                    if (RecPy.CashBank != null)
                    {
                        objRecPay.BankName = RecPy.CashBank;
                    }
                    else
                    {
                        objRecPay.BankName = RecPy.ChequeBank;
                    }

                    Context1.Entry(objRecPay).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return 1;
        }



        public int EditSupplierRecPay(CustomerRcieptVM RecPy, string UserID)
        {
            if (RecPy.RecPayID > 0)
            {
                try
                {
                    //Edit Code
                    RecPay objRecPay = Context1.RecPays.Where(item => item.RecPayID == RecPy.RecPayID).FirstOrDefault();

                    objRecPay.BankName = RecPy.BankName;
                    objRecPay.ChequeDate = RecPy.ChequeDate;
                    objRecPay.ChequeNo = RecPy.ChequeNo;
                    objRecPay.EXRate = RecPy.EXRate;
                    objRecPay.FMoney = RecPy.FMoney;
                    objRecPay.RecPayDate = RecPy.RecPayDate;
                    objRecPay.Remarks = RecPy.Remarks;
                    objRecPay.FYearID = RecPy.FYearID;
                    objRecPay.AcJournalID = RecPy.AcJournalID;
                    objRecPay.SupplierID = RecPy.SupplierID;
                    objRecPay.UserID = Convert.ToInt32(UserID);
                    if (RecPy.CashBank != null)
                    {
                        objRecPay.BankName = RecPy.CashBank;
                    }
                    else
                    {
                        objRecPay.BankName = RecPy.ChequeBank;
                    }

                    Context1.Entry(objRecPay).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return 1;
        }

    }
}