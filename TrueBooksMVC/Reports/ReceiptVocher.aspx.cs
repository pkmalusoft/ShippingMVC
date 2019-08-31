using System;
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


            ReportDataSource _rsource;
            var dd = entity.ReportCustomerReceipt(recpayid).ToList();
            _rsource = new ReportDataSource("ReceiptVoucher", dd);


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
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

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