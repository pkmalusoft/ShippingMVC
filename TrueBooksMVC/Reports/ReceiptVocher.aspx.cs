﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using Microsoft.Reporting.WebForms;
namespace TrueBooksMVC.Reports
{
    [SessionExpire]
    public partial class ReceiptVocher : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int recpayid = 32998;
            recpayid = Convert.ToInt32(Request.QueryString["recpayid"].ToString());
            ////int JobID = 2;
            //int JobID = 6764;
            //JobID = Convert.ToInt32(JobID);
            //int acid = 0;
            //if (jobid == "")
            //{
            //    acid = 0;
            //}
            //else
            //{
            //    acid = Convert.ToInt32(jobid);
            //}



            SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
            ReportViewer1.SizeToReportContent = true;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/ReceiptVoucher.rdlc");
            ReportViewer1.LocalReport.DataSources.Clear();

            DataTable dtcompany = new DataTable();
            dtcompany.Columns.Add("CompanyName");
            dtcompany.Columns.Add("Address1");
            dtcompany.Columns.Add("Address2");
            dtcompany.Columns.Add("Address3");
            dtcompany.Columns.Add("Phone");
            dtcompany.Columns.Add("AcHead");
            dtcompany.Columns.Add("Todate");

            var company = entity.AcCompanies.FirstOrDefault();

            DataRow dr = dtcompany.NewRow();
            dr[0] = company.AcCompany1;
            dr[1] = company.Address1;
            dr[2] = company.Address2;
            dr[3] = company.Address3;
            dr[4] = company.Phone;
            dr[5] = "";
            dr[6] = DateTime.Now;

            dtcompany.Rows.Add(dr);

            var receipt = (from d in entity.RecPays where d.RecPayID == recpayid select d).FirstOrDefault();
            var recpaydetails = (from d in entity.RecPayDetails where d.RecPayID == recpayid where d.InvoiceID>0 select d).ToList();
            var cust= entity.CUSTOMERs.Where(d => d.CustomerID == receipt.CustomerID).FirstOrDefault();
            var listofdet = new List<ReportCustomerReceipt_Result>();
            foreach (var item in recpaydetails)
            {
                var sinvoicedet = (from d in entity.SalesInvoiceDetails where d.SalesInvoiceDetailID == item.InvoiceID select d).FirstOrDefault();
                var sinvoice = (from d in entity.SalesInvoices where d.SalesInvoiceID == sinvoicedet.SalesInvoiceID select d).FirstOrDefault();
                var customerrecpay = new ReportCustomerReceipt_Result();
                customerrecpay.Date = receipt.RecPayDate.Value.ToString("dd-MMM-yyyy");
                customerrecpay.ReceivedFrom = cust.Customer1;
                customerrecpay.DocumentNo = receipt.DocumentNo;
                customerrecpay.Amount =Convert.ToDecimal(receipt.FMoney);
                customerrecpay.Remarks = receipt.Remarks;               
                customerrecpay.Account = receipt.BankName;
                if (receipt.ChequeDate != null)
                {
                    customerrecpay.ChequeDate = receipt.ChequeDate.Value.ToString("dd-MMM-yyyy");
                }
                else
                {
                    customerrecpay.ChequeDate = "";
                }
                if (!string.IsNullOrEmpty(receipt.ChequeNo))
                {
                    customerrecpay.ChequeNo = Convert.ToDecimal(receipt.ChequeNo);
                }
                customerrecpay.CustomerBank = "";
                customerrecpay.DetailDocNo = sinvoice.SalesInvoiceNo;
                customerrecpay.DocDate = sinvoice.SalesInvoiceDate.Value.ToString("dd-MMM-yyyy");
                customerrecpay.DocAmount =Convert.ToDecimal(sinvoicedet.NetValue);
               
                if (item.Amount > 0)
                {
                    customerrecpay.SettledAmount = Convert.ToDecimal(item.Amount) ;
                    customerrecpay.AdjustmentAmount = Convert.ToInt32(item.AdjustmentAmount);
                }
                else
                {
                    customerrecpay.SettledAmount = Convert.ToDecimal(item.Amount) *-1;
                    customerrecpay.AdjustmentAmount = Convert.ToInt32( item.AdjustmentAmount);
                }
                listofdet.Add(customerrecpay);
            }

            ReportDataSource _rsource;

            var dd = entity.ReportCustomerReceipt(recpayid).ToList();
            _rsource = new ReportDataSource("ReceiptVoucher", listofdet);


            ReportDataSource _rsource1 = new ReportDataSource("Company", dtcompany);

            ReportViewer1.LocalReport.DataSources.Add(_rsource);
          
            ReportViewer1.LocalReport.DataSources.Add(_rsource1);

            int totalamt = 0;

            //foreach (var item in dd)
            //{
            //    totalamt = 5000;
            //}

            totalamt = 5000;

            //DataTable dtuser = new DataTable();
            //dtuser.Columns.Add("UserName");

            //DataRow dr1 = dtuser.NewRow();
            //int uid = Convert.ToInt32(Session["UserID"].ToString());
            //dr1[0] = (from c in entity.UserRegistrations where c.UserID == uid select c.UserName).FirstOrDefault();
            //dtuser.Rows.Add(dr1);

            //ReportDataSource _rsource2 = new ReportDataSource("User", dtuser);

            //ReportViewer1.LocalReport.DataSources.Add(_rsource2);


            DataTable dtcurrency = new DataTable();
            dtcurrency.Columns.Add("SalesCurrency");
            dtcurrency.Columns.Add("ForeignCurrency");
            dtcurrency.Columns.Add("SalesCurrencySymbol");
            dtcurrency.Columns.Add("ForeignCurrencySymbol");
            dtcurrency.Columns.Add("InWords");



      
            DataRow r = dtcurrency.NewRow();
            r[0] = "";
            r[1] = "";
            r[2] = "";
            r[3] = "";
            r[4] = NumberToWords(totalamt);


            dtcurrency.Rows.Add(r);


            ReportDataSource _rsource3 = new ReportDataSource("Currency", dtcurrency);

            ReportViewer1.LocalReport.DataSources.Add(_rsource3);
            ReportViewer1.LocalReport.Refresh();

        }


        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
    }
}