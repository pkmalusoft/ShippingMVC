using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using DAL;
namespace TrueBooksMVC.Reports
{
    [SessionExpire]
    public partial class Report_JobCost : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();


                DateTime fromdate = Convert.ToDateTime(Request.QueryString["fromdate"].ToString());
                DateTime todate = Convert.ToDateTime(Request.QueryString["todate"].ToString());
                int CustomerID = Convert.ToInt32(Request.QueryString["custid"].ToString());
                int JobTypeID = Convert.ToInt32(Request.QueryString["jobtypeid"].ToString());
                int Employee = Convert.ToInt32(Request.QueryString["empid"].ToString());

                ReportViewer1.SizeToReportContent = true;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Report_JobCost.rdlc");
                ReportViewer1.LocalReport.DataSources.Clear();






                DataTable dtcompany = new DataTable();
                dtcompany.Columns.Add("CompanyName");
                dtcompany.Columns.Add("Address1");
                dtcompany.Columns.Add("Address2");
                dtcompany.Columns.Add("Address3");
                dtcompany.Columns.Add("Phone");
                dtcompany.Columns.Add("Todate");
                dtcompany.Columns.Add("AcHead");


                var company = entity.AcCompanies.FirstOrDefault();

                DataRow dr = dtcompany.NewRow();
                dr[0] = company.AcCompany1;
                dr[1] = company.Address1;
                dr[2] = company.Address2;
                dr[3] = company.Address3;
                dr[4] = company.Phone;
                dr[5] = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(todate);
                dr[6] = "";

                dtcompany.Rows.Add(dr);

                ReportDataSource _rsource;

                //var dt = entity.Report_CashAndBankBook(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToDateTime("01 Jan 2016"), Convert.ToDateTime("04 Apr 2016"), acid, Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();

                var dt = entity.JobAnalysisReport(fromdate, todate).ToList();


                _rsource = new ReportDataSource("JobAnalysisRepor", dt);



                ReportDataSource _rsource1 = new ReportDataSource("Company", dtcompany);

                ReportViewer1.LocalReport.DataSources.Add(_rsource);
                ReportViewer1.LocalReport.DataSources.Add(_rsource1);
                DataTable dtuser = new DataTable();
                dtuser.Columns.Add("UserName");

                DataRow dr1 = dtuser.NewRow();
                int uid = Convert.ToInt32(Session["UserID"].ToString());
                dr1[0] = (from c in entity.UserRegistrations where c.UserID == uid select c.UserName).FirstOrDefault();
                dtuser.Rows.Add(dr1);

                ReportDataSource _rsource3 = new ReportDataSource("User", dtuser);
                ReportViewer1.LocalReport.DataSources.Add(_rsource3);


                DataTable dtfilter = new DataTable();
                dtfilter.Columns.Add("FromDate");
                dtfilter.Columns.Add("UptoDate");


                DataRow drfilterrow = dtfilter.NewRow();
                drfilterrow[0] = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(fromdate);
                drfilterrow[1] = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(todate);

                dtfilter.Rows.Add(drfilterrow);
                ReportDataSource _rsource4 = new ReportDataSource("ReportPeriod", dtfilter);
                ReportViewer1.LocalReport.DataSources.Add(_rsource4);






                ReportViewer1.LocalReport.Refresh();
            }
        }
    }
}