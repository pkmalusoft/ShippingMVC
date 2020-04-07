using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using Microsoft.Reporting.WebForms;

namespace TrueBooksMVC.Reports
{
    [SessionExpire]
    public partial class DayBook : System.Web.UI.Page
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!IsPostBack)
            {


                int acid = Convert.ToInt32(Request.QueryString["acheadid"].ToString());
                DateTime fromdate = Convert.ToDateTime(Request.QueryString["fromdate"].ToString());
                DateTime todate = Convert.ToDateTime(Request.QueryString["todate"].ToString());
                int rentflag = Convert.ToInt32(Request.QueryString["rentflag"].ToString());
                int m1 = Convert.ToInt32(Request.QueryString["m1"].ToString());
                int m2 = Convert.ToInt32(Request.QueryString["m2"].ToString());


                ReportViewer1.SizeToReportContent = true;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/DayBook.rdlc");
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
                string achead = (from c in entity.AcHeads where c.AcHeadID == acid select c.AcHead1).FirstOrDefault();
                DataRow dr = dtcompany.NewRow();
                dr[0] = company.AcCompany1;
                dr[1] = company.Address1;
                dr[2] = company.Address2;
                dr[3] = company.Address3;
                dr[4] = company.Phone;
                dr[5] = todate;
                dr[6] = achead;


                dtcompany.Rows.Add(dr);


                ReportDataSource _rsource;



                var dt = entity.Report_DayBook(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString()), fromdate, todate, acid, rentflag, m1, m2).ToList();
                _rsource = new ReportDataSource("DayBook", dt);




                ReportDataSource _rsource1 = new ReportDataSource("Company", dtcompany);

                DataTable dtuser = new DataTable();
                dtuser.Columns.Add("UserName");

                DataRow dr1 = dtuser.NewRow();
                int uid = Convert.ToInt32(Session["UserID"].ToString());
                dr1[0] = (from c in entity.UserRegistrations where c.UserID == uid select c.UserName).FirstOrDefault();
                dtuser.Rows.Add(dr1);

                ReportDataSource _rsource2 = new ReportDataSource("User", dtuser);

                ReportViewer1.LocalReport.DataSources.Add(_rsource2);



                ReportViewer1.LocalReport.DataSources.Add(_rsource);
                ReportViewer1.LocalReport.DataSources.Add(_rsource1);
                ReportViewer1.LocalReport.Refresh();
            }

        }
    }
}