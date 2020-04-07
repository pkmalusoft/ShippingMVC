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
    public partial class JobCostReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();

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

                DateTime fromdate = Convert.ToDateTime(Request.QueryString["fromdate"].ToString());
                DateTime todate = Convert.ToDateTime(Request.QueryString["todate"].ToString());
                int custid = Convert.ToInt32(Request.QueryString["custid"].ToString());
                int jobid = Convert.ToInt32(Request.QueryString["jobid"].ToString());
                int status = Convert.ToInt32(Request.QueryString["status"].ToString());
                //DateTime fromdate = Convert.ToDateTime("01 Jan 2016");
                //DateTime todate = Convert.ToDateTime("31 Dec 2016");



                ReportViewer1.SizeToReportContent = true;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/JobCostReport.rdlc");
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
                dr[6] = "";

                dtcompany.Rows.Add(dr);


                ReportDataSource _rsource;




               //todo:fix to run by sethu
                //   var data = entity.SP_GetJOBCostReport(fromdate, todate, custid, jobid,status).ToList();
                var data = entity.SP_GetJOBCostReport(fromdate, todate, custid, jobid).ToList();
                _rsource = new ReportDataSource("JobCostReport", data);
                ReportViewer1.LocalReport.DataSources.Add(_rsource);

                ReportDataSource _rsource1 = new ReportDataSource("Company", dtcompany);


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





                DataTable dtfilter = new DataTable();
                dtfilter.Columns.Add("FromDate");
                dtfilter.Columns.Add("UptoDate");


                DataRow drfilterrow = dtfilter.NewRow();
                drfilterrow[0] = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(fromdate);
                drfilterrow[1] = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(todate);

                dtfilter.Rows.Add(drfilterrow);
                ReportDataSource _rsource3 = new ReportDataSource("FilterDate", dtfilter);
                ReportViewer1.LocalReport.DataSources.Add(_rsource3);





                DataTable dtcustomer = new DataTable();
                dtcustomer.Columns.Add("CustomerName");
                DataRow dtcust = dtcustomer.NewRow();
                if (custid == 0)
                {
                    dtcust[0] = "";
                }
                else
                {
                    string custname = (from c in entity.CUSTOMERs where c.CustomerID == custid select c.Customer1).FirstOrDefault();
                    dtcust[0] = custname;
                }



                dtcustomer.Rows.Add(dtcust);
                ReportDataSource _rsource4 = new ReportDataSource("Customer", dtcustomer);
                ReportViewer1.LocalReport.DataSources.Add(_rsource4);

                ReportViewer1.LocalReport.Refresh();
            }

        }


        //public static string NumberToWords(int number)
        //{
        //    if (number == 0)
        //        return "zero";

        //    if (number < 0)
        //        return "minus " + NumberToWords(Math.Abs(number));

        //    string words = "";

        //    if ((number / 1000000) > 0)
        //    {
        //        words += NumberToWords(number / 1000000) + " million ";
        //        number %= 1000000;
        //    }

        //    if ((number / 1000) > 0)
        //    {
        //        words += NumberToWords(number / 1000) + " thousand ";
        //        number %= 1000;
        //    }

        //    if ((number / 100) > 0)
        //    {
        //        words += NumberToWords(number / 100) + " hundred ";
        //        number %= 100;
        //    }

        //    if (number > 0)
        //    {
        //        if (words != "")
        //            words += "and ";

        //        var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        //        var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

        //        if (number < 20)
        //            words += unitsMap[number];
        //        else
        //        {
        //            words += tensMap[number / 10];
        //            if ((number % 10) > 0)
        //                words += "-" + unitsMap[number % 10];
        //        }
        //    }

        //    return words;
        //}
    }
}
