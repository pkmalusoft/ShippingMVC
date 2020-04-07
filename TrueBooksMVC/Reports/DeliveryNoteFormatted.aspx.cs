using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
namespace TrueBooksMVC.Reports
{
    [SessionExpire]
    public partial class DeliveryNoteFormatted : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
            int JobID = 6764;
            //JobID = Convert.ToInt32(JobID);
            JobID = Convert.ToInt32(Request.QueryString["jobid"].ToString());


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
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/DeliveryNoteFormatted.rdlc");
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





            var dd = entity.SP_GetDeliveryNoteReport(JobID).ToList();
            _rsource = new ReportDataSource("DeliveryNoteDS", dd);


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


            ReportViewer1.LocalReport.Refresh();
        }
    }
}