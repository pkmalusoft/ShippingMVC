using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueBooksMVC.Models;
using System.Data.SqlClient;
using System.Data;
using DAL;
using System.Data.Entity;

namespace TrueBooksMVC.Models
{
    public class ReportModel
    {
        SHIPPING_FinalEntities Context1 = new SHIPPING_FinalEntities();

        public List<SP_GetInvoiceReport_Result> GetInvoiceReport(int JobID)
        {
            return Context1.SP_GetInvoiceReport(JobID).ToList();
        }

        public List<SP_GetDeliveryNoteReport_Result> GetDeliveryNote(int JobID)
        {
            return Context1.SP_GetDeliveryNoteReport(JobID).ToList();
        }

        public List<SP_GetJobArrivalReport_Result> GetArrivalReport(int JobID)
        {
            return Context1.SP_GetJobArrivalReport(JobID).ToList();
        }

        public List<SP_GetJobTruckConsignmentNoteReport_Result> GetTruckConsignmentReport(int JobID)
        {
            return Context1.SP_GetJobTruckConsignmentNoteReport(JobID).ToList();
        }

        public List<SP_CustomerLedgerReport_Result> GetCustomerLedgerReportForPrint(int id)
        {
            return Context1.SP_GetCustomerLedgerReport(id).ToList();
        }
    }
}