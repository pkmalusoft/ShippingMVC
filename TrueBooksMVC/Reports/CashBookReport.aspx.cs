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
    public partial class CashBookReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();

               
                int acid = Convert.ToInt32(Request.QueryString["acheadid"].ToString());



                DateTime fromdate = Convert.ToDateTime(Request.QueryString["fromdate"].ToString());
                DateTime todate = Convert.ToDateTime(Request.QueryString["todate"].ToString());







                ReportViewer1.SizeToReportContent = true;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/ReportCashBook.rdlc");
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
                //dr[5] = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(todate);
                dr[5] = todate;
                dr[6] = achead;

                dtcompany.Rows.Add(dr);

                ReportDataSource _rsource;

                //var dt = entity.Report_CashAndBankBook(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToDateTime("01 Jan 2016"), Convert.ToDateTime("04 Apr 2016"), acid, Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
                var dt = entity.Report_CashBook(11, Convert.ToDateTime("01 Jan 2016"), Convert.ToDateTime("04 Apr 2016"), acid, 1).ToList();
                _rsource = new ReportDataSource("CashBook", dt);



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

                ReportViewer1.LocalReport.Refresh();
            }

        }
    }
}