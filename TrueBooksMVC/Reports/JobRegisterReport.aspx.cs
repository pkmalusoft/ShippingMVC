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
    public partial class JobRegisterReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();

            if (!IsPostBack)
            {
                int jobid = 0;
                jobid = Convert.ToInt32(Request.QueryString["jobid"].ToString());

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
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/JobRegisterReport.rdlc");
                ReportViewer1.LocalReport.DataSources.Clear();

                ReportDataSource _rsource;
                DataSet ds = DAL.SP_JobRegisterReportPrintByJObID(jobid);
                _rsource = new ReportDataSource("JobRegister", ds.Tables[0]);

                ReportDataSource _rsCargo;
                _rsCargo = new ReportDataSource("Cargo", ds.Tables[1]);

                ReportDataSource _rsBillOfEntry;
                _rsBillOfEntry = new ReportDataSource("BillOfEntry", ds.Tables[2]);

                ReportDataSource _rsource1 = new ReportDataSource("Comapny", dtcompany);

               // DataTable ddcharges = new DataTable();
             //   var dd = entity.GetChargesByJobIDForJobRegister(jobid).ToList();
                ReportDataSource _rsource2 = new ReportDataSource("Charges", ds.Tables[3]);
                ReportDataSource _rsourceContainerDetails = new ReportDataSource("ContainerDetails", ds.Tables[4]);
                ReportDataSource _rsourceAuditLog = new ReportDataSource("AuditLog", ds.Tables[5]);

                DataTable druser = new DataTable();
                druser.Columns.Add("UserName");
                DataRow dr1 = druser.NewRow();
                //int uid = Convert.ToInt32(Session["UserID"].ToString());
                int uid = Convert.ToInt32(Session["UserID"].ToString());
                dr1[0] = (from c in entity.UserRegistrations where c.UserID == uid select c.UserName).FirstOrDefault();
                druser.Rows.Add(dr1);

                ReportDataSource _rsource3 = new ReportDataSource("User", druser);

                ReportViewer1.LocalReport.DataSources.Add(_rsource2);
                ReportViewer1.LocalReport.DataSources.Add(_rsource);
                ReportViewer1.LocalReport.DataSources.Add(_rsCargo);
                ReportViewer1.LocalReport.DataSources.Add(_rsBillOfEntry);
                ReportViewer1.LocalReport.DataSources.Add(_rsource1);
                ReportViewer1.LocalReport.DataSources.Add(_rsource3);
                ReportViewer1.LocalReport.DataSources.Add(_rsourceContainerDetails);
                ReportViewer1.LocalReport.DataSources.Add(_rsourceAuditLog);
                ReportViewer1.LocalReport.Refresh();
            }
        }

        protected void ReportViewer1_PageNavigation(object sender, PageNavigationEventArgs e)
        {
            ReportViewer1.CurrentPage = e.NewPage;



        }
    }
}