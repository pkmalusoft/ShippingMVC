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
    public partial class PurchaseInvoiceReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindReport();
            }
        }

        private void BindReport()
        {
            SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();

            int InvoiceId = 0;
            InvoiceId = Convert.ToInt32(Request.QueryString["id"].ToString());

            DateTime fromdate = new DateTime(2016, 01, 01);
            DateTime todate = new DateTime(2016, 04, 25);

            ReportViewer1.SizeToReportContent = true;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/PurchaseInvoiceReport.rdlc");
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
            var invoice = entity.PurchaseInvoices.Where(d=>d.PurchaseInvoiceID==InvoiceId).FirstOrDefault();
            var getsupplier = entity.Suppliers.Where(d => d.SupplierID == invoice.SupplierID).FirstOrDefault();

            DataRow dr = dtcompany.NewRow();
            dr[0] = company.AcCompany1;
            dr[1] = company.Address1;
            dr[2] = company.Address2;
            dr[3] = company.Address3;
            dr[4] = company.Phone;
            dr[5] = "";
            dr[6] = DateTime.Now;

            dtcompany.Rows.Add(dr);
            ReportDataSource _rsourceInvoice;

            DataSet ds = DAL.SP_GetPurchaseInvoiceReport(InvoiceId);
            _rsourceInvoice = new ReportDataSource("PurchaseInvoice", ds.Tables[0]);


            DataTable dtsupplier = new DataTable();
            dtsupplier.Columns.Add("SupplierName");
            dtsupplier.Columns.Add("Address1");
            dtsupplier.Columns.Add("Address2");
            dtsupplier.Columns.Add("Address3");
            dtsupplier.Columns.Add("Phone");
            dtsupplier.Columns.Add("Country");
            dtsupplier.Columns.Add("City");

            DataRow dr2 = dtsupplier.NewRow();
            dr2[0] = getsupplier.SupplierName;
            dr2[1] = getsupplier.Address1;
            dr2[2] = getsupplier.Address2;
            dr2[3] = getsupplier.Address3;
            dr2[4] = getsupplier.Phone;
            dr2[5] = getsupplier.CountryID;
            dr2[6] = invoice.ExchangeRate;

            dtsupplier.Rows.Add(dr2);

            ReportDataSource _resourceSupplier = new ReportDataSource("Supplier", dtsupplier);
            ReportDataSource _rsourceCompany = new ReportDataSource("Company", dtcompany);
            ReportDataSource _rsourceInvoiceDetails = new ReportDataSource("PurchaseInvoiceDetails", ds.Tables[1]);

            ReportViewer1.LocalReport.DataSources.Add(_rsourceInvoice);
            ReportViewer1.LocalReport.DataSources.Add(_rsourceCompany);
            ReportViewer1.LocalReport.DataSources.Add(_rsourceInvoiceDetails);
            ReportViewer1.LocalReport.DataSources.Add(_resourceSupplier);

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