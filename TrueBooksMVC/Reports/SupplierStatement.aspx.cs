using DAL;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TrueBooksMVC.Views.Report_SupplierStatement
{
    [SessionExpire]
    public partial class SupplierStatement : System.Web.UI.Page
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                int vSupplierD = Convert.ToInt32(Request.QueryString["supid"].ToString());
                DateTime fromdate = Convert.ToDateTime(Request.QueryString["fromdate"].ToString());
               DateTime todate = Convert.ToDateTime(Request.QueryString["todate"].ToString());
                int? currencyId = 0;


                //string vSupplierD = "0";


                //int supid = 0;
                //if (vSupplierD == "")
                //{
                //    supid = 0;
                //}
                //else
                //{
                //    supid = Convert.ToInt32(vSupplierD);
                //}







                ReportViewer1.SizeToReportContent = true;
                ReportViewer1.ShowPrintButton = true;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Supplier_Statement.rdlc");
                ReportViewer1.LocalReport.DataSources.Clear();




                var dt1 = entity.qrySupplierOutstandings.ToList();

                DataTable dtcompany = new DataTable();
                dtcompany.Columns.Add("CompanyName");
                dtcompany.Columns.Add("Address1");
                dtcompany.Columns.Add("Address2");
                dtcompany.Columns.Add("Address3");
                dtcompany.Columns.Add("Phone");
                dtcompany.Columns.Add("Todate");
                dtcompany.Columns.Add("AcHead");

                var company = entity.AcCompanies.FirstOrDefault();
                string imagePath = new Uri(Server.MapPath("~/Content/Logo/" + company.logo)).AbsoluteUri;

                DataRow dr = dtcompany.NewRow();
                dr[0] = company.AcCompany1;
                dr[1] = company.Address1;
                dr[2] = company.Address2;
                dr[3] = company.Address3;
                dr[4] = company.Phone;
                dr[5] = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(todate);
                dr[6] = imagePath;

                dtcompany.Rows.Add(dr);

                ReportDataSource _rsource2 = new ReportDataSource("shipping", dt1);
                ReportDataSource _rsource;
                if (vSupplierD == 0)
                {
                    var dt = entity.SupStatement().Where(x => x.InvoiceDate >= fromdate && x.InvoiceDate <= todate).ToList();
                    _rsource = new ReportDataSource("SupState", dt);
                }
                else
                {
                    var dt = entity.SupStatement().Where(x => x.SupplierID == vSupplierD && (x.InvoiceDate >= fromdate && x.InvoiceDate <= todate)).ToList();
                    _rsource = new ReportDataSource("SupState", dt);
                }

                DataTable dtuser = new DataTable();
                dtuser.Columns.Add("UserName");

                DataRow dr1 = dtuser.NewRow();
                int uid = Convert.ToInt32(Session["UserID"].ToString());
                dr1[0] = (from c in entity.UserRegistrations where c.UserID == uid select c.UserName).FirstOrDefault();
                dtuser.Rows.Add(dr1);

                ReportDataSource _rsource3 = new ReportDataSource("User", dtuser);

                ReportViewer1.LocalReport.DataSources.Add(_rsource2);






                ReportDataSource _rsource1 = new ReportDataSource("Company", dtcompany);
                ReportViewer1.LocalReport.DataSources.Add(_rsource2);
                ReportViewer1.LocalReport.DataSources.Add(_rsource);
                ReportViewer1.LocalReport.DataSources.Add(_rsource1);
                ReportViewer1.LocalReport.DataSources.Add(_rsource3);
                ReportViewer1.LocalReport.Refresh();
            }
        }
    }
}