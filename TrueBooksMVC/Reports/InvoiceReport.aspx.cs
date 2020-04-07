using DAL;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TrueBooksMVC.Reports
{
    [SessionExpire]
    public partial class InvoiceReport : System.Web.UI.Page
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            int JobID = 6764;
            JobID = Convert.ToInt32(Request.QueryString["jobid"].ToString());
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

            DateTime fromdate = new DateTime(2016, 01, 01);
            DateTime todate = new DateTime(2016, 04, 25);


            ReportViewer1.SizeToReportContent = true;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/InvoiceReport.rdlc");
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


            ReportDataSource _rsource;





            var dd = entity.SP_GetInvoiceReport(JobID).ToList();
            _rsource = new ReportDataSource("InvoiceReport", dd);


            ReportDataSource _rsource1 = new ReportDataSource("Company", dtcompany);

            //ReportViewer1.LocalReport.DataSources.Add(_rsource);
            ReportViewer1.LocalReport.DataSources.Add(_rsource);
            ReportViewer1.LocalReport.DataSources.Add(_rsource1);

            DataTable dtuser = new DataTable();
            dtuser.Columns.Add("UserName");

            DataRow dr1 = dtuser.NewRow();
            int uid = Convert.ToInt32(Session["UserID"].ToString());
            dr1[0] = (from c in entity.UserRegistrations where c.UserID == uid select c.UserName).FirstOrDefault();
            dtuser.Rows.Add(dr1);

            ReportDataSource _rsource2 = new ReportDataSource("User", dtuser);

            ReportViewer1.LocalReport.DataSources.Add(_rsource2);




            DataTable dtcurrency = new DataTable();
            dtcurrency.Columns.Add("SalesCurrency");
            dtcurrency.Columns.Add("ForeignCurrency");
            dtcurrency.Columns.Add("SalesCurrencySymbol");
            dtcurrency.Columns.Add("ForeignCurrencySymbol");
            dtcurrency.Columns.Add("InWords");

            decimal shome = dd.Sum(x => x.SalesHome).Value;

            var cur = (from c in entity.CurrencyMasters where c.StatusBaseCurrency == true select c).FirstOrDefault();


            DataRow r = dtcurrency.NewRow();
            r[0] = cur.CurrencyName;
            r[1] = dd.First().CurrencyName;
            r[2] = "(" + cur.Symbol + ")";
            r[3] = "(" + dd.First().Symbol + ")";
            r[4] = NumberToWords(Convert.ToInt32(shome));


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

                var unitsMap = new[] { "Zeero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
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