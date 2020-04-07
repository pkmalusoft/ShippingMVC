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
    public partial class ChartOfAccount : System.Web.UI.Page
    {
      SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
  
    
         
         protected void Page_Load(object sender, EventArgs e)
        {
             //string vSupplierD = Request.QueryString["supplierId"].ToString();
            if (!IsPostBack)
            {
                string accomp = "1";


                int supid = 0;
                if (accomp == "")
                {
                    supid = 0;
                }
                else
                {
                    supid = Convert.ToInt32(accomp);
                }

                ReportViewer1.SizeToReportContent = true;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/ChartOFAcount.rdlc");
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
                dr[5] = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(DateTime.Now);
                dr[6] = "";

                dtcompany.Rows.Add(dr);

                var dt = entity.AcHeadSelectAll(1).ToList();

                ReportDataSource _rsource2 = new ReportDataSource("ChartOFAccount", dt);
                //ReportDataSource _rsource;
                //if (supid == 0)
                //{
                //    var dt = entity.ProCustomerLedger1(1,"01 Jan 2016","04 Apr 2016").ToList();
                //    _rsource = new ReportDataSource("DataSet2", dt);
                //}
                //else
                //{
                //    var dt = entity.ProCustomerLedger1(1, "01 Jan 2016", "04 Apr 2016").ToList();
                //    _rsource = new ReportDataSource("DataSet2", dt);
                //}



                ReportDataSource _rsource1 = new ReportDataSource("Company", dtcompany);
                ReportViewer1.LocalReport.DataSources.Add(_rsource2);
                // ReportViewer1.LocalReport.DataSources.Add(_rsource);
                ReportViewer1.LocalReport.DataSources.Add(_rsource1);

                DataTable dtuser = new DataTable();
                dtuser.Columns.Add("UserName");

                DataRow dr1 = dtuser.NewRow();
                int uid = Convert.ToInt32(Session["UserID"].ToString());
                dr1[0] = (from c in entity.UserRegistrations where c.UserID == uid select c.UserName).FirstOrDefault();
                dtuser.Rows.Add(dr1);

                ReportDataSource _rsource3 = new ReportDataSource("User", dtuser);

                ReportViewer1.LocalReport.DataSources.Add(_rsource3);


                ReportViewer1.LocalReport.Refresh();
            }
        
        }
    }
}