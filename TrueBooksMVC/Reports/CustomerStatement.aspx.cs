using DAL;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TrueBooksMVC.Reports
{
    [SessionExpire]
    public partial class CustomerStatement : System.Web.UI.Page
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!IsPostBack)
            {
                int vCustomerID = 0;
                vCustomerID = Convert.ToInt32(Request.QueryString["custid"].ToString());
                DateTime fromdate = Convert.ToDateTime(Request.QueryString["fromdate"].ToString());
                DateTime todate = Convert.ToDateTime(Request.QueryString["todate"].ToString());


                //int custid = 0;
                //if (vCustomerID == "")
                //{
                //    custid = 0;
                //}
                //else
                //{
                //    custid = Convert.ToInt32(vCustomerID);
                //}
                ////DateTime fromdate = Convert.ToDateTime(Request.QueryString["frmdate"].ToString());
                ////DateTime todate = Convert.ToDateTime(Request.QueryString["todate"].ToString());
                //DateTime fromdate = new DateTime(2016, 01, 01);
                //DateTime todate = new DateTime(2016, 08, 31);







                ReportViewer1.SizeToReportContent = true;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/RepCustomerStatement.rdlc");
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

                DataTable dtuser = new DataTable();
                dtuser.Columns.Add("UserName");

                DataRow dr1 = dtuser.NewRow();
                int uid = Convert.ToInt32(Session["UserID"].ToString());
                dr1[0] = (from c in entity.UserRegistrations where c.UserID == uid select c.UserName).FirstOrDefault();
                dtuser.Rows.Add(dr1);

                ReportDataSource _rsource;
                if (vCustomerID == 0)
                {
                    var dt = entity.CustStatement().Where(x => x.InvoiceDate >= fromdate && x.InvoiceDate <= todate).ToList();
                    _rsource = new ReportDataSource("CustStatement", dt);
                }
                else
                {
                    var dt = entity.CustStatement().Where(x => x.InvoiceToID == vCustomerID && (x.InvoiceDate >= fromdate && x.InvoiceDate <= todate)).ToList();
                    _rsource = new ReportDataSource("CustStatement", dt);
                }



                ReportDataSource _rsource1 = new ReportDataSource("Company", dtcompany);
                ReportDataSource _rsource2 = new ReportDataSource("User", dtuser);
                ReportViewer1.LocalReport.DataSources.Add(_rsource);
                ReportViewer1.LocalReport.DataSources.Add(_rsource1);
                ReportViewer1.LocalReport.DataSources.Add(_rsource2);
                ReportViewer1.LocalReport.Refresh();
            }

        }

       
    }
}