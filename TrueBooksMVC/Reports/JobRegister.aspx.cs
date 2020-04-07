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
    public partial class JobRegister : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();

            int custid = 0;
            DateTime frmdate;
            DateTime todate;

            // supid = Convert.ToInt32(Request.QueryString["supid"].ToString());
            // frmdate = Convert.ToDateTime(Request.QueryString["fromdate"].ToString()) ;
            //todate = Convert.ToDateTime(Request.QueryString["todate"].ToString());


            if (!IsPostBack)
            {



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
                dr[5] = DateTime.Now;
                dr[6] = "";
                dtcompany.Rows.Add(dr);


                ReportViewer1.SizeToReportContent = true;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Job_Register.rdlc");
                ReportViewer1.LocalReport.DataSources.Clear();

                ReportDataSource _rsource;
                var dt = entity.CustOutstanding(0, Convert.ToDateTime("01 Jan 2013"), Convert.ToDateTime("30-Dec-2016")).ToList();
                _rsource = new ReportDataSource("CustOutStanding", dt);

                ReportDataSource _rsource1 = new ReportDataSource("Company", dtcompany);

                ReportViewer1.LocalReport.DataSources.Add(_rsource);
                ReportViewer1.LocalReport.DataSources.Add(_rsource1);
                ReportViewer1.LocalReport.Refresh();
            }




        }

        protected void ReportViewer1_PageNavigation(object sender, PageNavigationEventArgs e)
        {
            ReportViewer1.CurrentPage = e.NewPage;



        }
    }
}