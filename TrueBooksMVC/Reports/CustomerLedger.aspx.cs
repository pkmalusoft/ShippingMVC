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
    public partial class CustomerLedger : System.Web.UI.Page
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int custid = Convert.ToInt32(Request.QueryString["custid"].ToString());
                string fromdate =Request.QueryString["fromdate"].ToString();
                string todate = Request.QueryString["todate"].ToString();
             


                //int supid = 0;
                //if (custmorid == "")
                //{
                //    supid = 0;
                //}
                //else
                //{
                //    supid = Convert.ToInt32(custmorid);
                //}

                //DateTime fromdate = new DateTime(2016, 01, 01);
                //DateTime todate = new DateTime(2016, 04, 25);







                ReportViewer1.SizeToReportContent = true;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/CustomerLedger.rdlc");
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

                ReportDataSource _rsource3 = new ReportDataSource("User", dtuser);
                ReportViewer1.LocalReport.DataSources.Add(_rsource3);


                ReportDataSource _rsource2 = new ReportDataSource("DataSet1", dtcompany);
                ReportDataSource _rsource;
              
                    var dt = entity.ProCustomerLedger1(custid,fromdate,todate).ToList();
                    _rsource = new ReportDataSource("DataSet2", dt);



                    DataTable dtfilter = new DataTable();
                    dtfilter.Columns.Add("FromDate");
                    dtfilter.Columns.Add("UptoDate");


                    DataRow drfilterrow = dtfilter.NewRow();
                    drfilterrow[0] = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(fromdate);
                    drfilterrow[1] = TrueBooksMVC.Models.CommanFunctions.GetShortDateFormat(todate);

                    dtfilter.Rows.Add(drfilterrow);
                    ReportDataSource _rsource4 = new ReportDataSource("FilterDate", dtfilter);
                    ReportViewer1.LocalReport.DataSources.Add(_rsource4);




                ReportDataSource _rsource1 = new ReportDataSource("Company", dtcompany);
                ReportViewer1.LocalReport.DataSources.Add(_rsource2);
                ReportViewer1.LocalReport.DataSources.Add(_rsource);
                ReportViewer1.LocalReport.DataSources.Add(_rsource1);
                ReportViewer1.LocalReport.Refresh();
            }
        }
    }
}