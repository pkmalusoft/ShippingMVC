using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueBooksMVC.Models;
using System.Data.SqlClient;
using System.Data;
using DAL;

namespace TrueBooksMVC.Models
{
    public class MastersModel
    {
        SHIPPING_FinalEntities Context1 = new SHIPPING_FinalEntities();
        
        public List<SP_GetAllJobType_Result> GetJobTypeS()
        {

            var query = Context1.SP_GetAllJobType().OrderBy(x => x.JobDescription).ToList();

            return query;
        }

        public List<SP_GetAllCustomers_Result> GetAllCustomer()
        {
            var query = Context1.SP_GetAllCustomers().OrderBy(x => x.Customer).ToList();

            return query;
        }
        public List<SP_GetAllCustomers_Result> GetAllCustomer(string Term)
        {
            var query = Context1.SP_GetAllCustomers().Where(c => c.Customer.ToLower().Contains(Term.ToLower())).OrderBy(x => x.Customer).ToList();

            return query;
        }
        public List<SP_GetAllCustomers_Result> GetCustomerById(int Id)
        {
            var query = Context1.SP_GetAllCustomers().Where(c => c.CustomerID.Equals(Id)).OrderBy(x => x.Customer).ToList();

            return query;
        }
        public List<SP_GetAllSupplier_Result> GetAllSupplier()
        {
            var query = Context1.SP_GetAllSupplier().OrderBy(x => x.SupplierName).ToList();

            return query;
        }

        public List<SP_GetAllSupplier_Result> GetAllSupplier(string Term)
        {
            var query = Context1.SP_GetAllSupplier().Where(c => c.SupplierName.ToLower().Contains(Term.ToLower())).OrderBy(x => x.SupplierName).ToList();
            return query;
        }
        public List<SP_GetAllSupplier_Result> GetSupplierById(int Id)
        {
            var query = Context1.SP_GetAllSupplier().Where(c => c.SupplierID.Equals(Id)).ToList();
            return query;
        }
        public List<SP_GetAllPorts_Result> GetAllPort()
        {
            var query = Context1.SP_GetAllPorts().OrderBy(x => x.Port).ToList();

            return query;
        }

        public List<SP_GetAllPorts_Result> GetAllPorts(string Term)
        {
            var query = Context1.SP_GetAllPorts().Where(c => c.Port.ToLower().Contains(Term.ToLower())).OrderBy(x => x.Port).ToList();
            return query;
        }

        public List<SP_GetAllPorts_Result> GetPortById(int Id)
        {
            var query = Context1.SP_GetAllPorts().Where(c => c.PortID.Equals(Id)).OrderBy(x => x.Port).ToList();
            return query;
        }

        public List<SP_GetAllEmployees_Result> GetAllSales()
        {
            var query = Context1.SP_GetAllEmployees().OrderBy(x => x.EmployeeName).ToList();

            return query;
        }

        public List<SP_GetAllTransporters_Result> GetAllTransporters()
        {
            var query = Context1.SP_GetAllTransporters().OrderBy(x => x.TransPorter).ToList();

            return query;
        }

        public List<SP_GetAllVessels_Result> GetAllVessels()
        {
            var query = Context1.SP_GetAllVessels().OrderBy(x => x.Vessel).ToList();

            return query;
        }

        public List<SP_GetAllCarrier_Result> GetAllCarrier()
        {
            var query = Context1.SP_GetAllCarrier().OrderBy(x => x.Carrier).ToList();

            return query;
        }

        public List<SP_GetAllEmployees_Result> GetAllEmployees()
        {
            var query = Context1.SP_GetAllEmployees().OrderBy(x => x.EmployeeName).ToList();

            return query;
        }

        public List<SP_GetAllEmployees_Result> GetAllEmployees(string Term)
        {
            var query = Context1.SP_GetAllEmployees().Where(c => c.EmployeeName.ToLower().Contains(Term.ToLower())).OrderBy(x => x.EmployeeName).ToList();

            return query;
        }
        public List<SP_GetAllEmployees_Result> GetEmployeeById(int Id)
        {
            var query = Context1.SP_GetAllEmployees().Where(c => c.EmployeeID.Equals(Id)).OrderBy(x => x.EmployeeName).ToList();

            return query;
        }
        public List<SP_GetAllCountries_Result> GetAllCountries()
        {
            var query = Context1.SP_GetAllCountries().OrderBy(x => x.CountryName).ToList();

            return query;
        }

        public List<SP_GetAllContainerTypes_Result> GetAllContainerTypes()
        {
            var query = Context1.SP_GetAllContainerTypes().OrderBy(x => x.ContainerType).ToList();

            return query;
        }

        public List<SP_GetCurrency_Result> GetCurrency()
        {
            var query = Context1.SP_GetCurrency().OrderBy(x => x.CurrencyName).ToList();

            return query;
        }

        public List<SP_GetCurrency_Result> GetCurrency(string Term)
        {
            var query = Context1.SP_GetCurrency().Where(c => c.CurrencyName.ToLower().Contains(Term.ToLower())).OrderBy(x => x.CurrencyName).ToList();

            return query;
        }
        public List<SP_GetCurrency_Result> GetCurrencyById(int Id)
        {
            var query = Context1.SP_GetCurrency().Where(c => c.CurrencyID.Equals(Id)).ToList();
            return query;
        }
        public List<SP_GetAllRevenueType_Result> GetRevenueType()
        {
            return Context1.SP_GetAllRevenueType().OrderBy(x => x.RevenueType).ToList();
        }

        public List<SP_GetAllItemUnit_Result> GetItemUnit()
        {
            return Context1.SP_GetAllItemUnit().OrderBy(x => x.ItemUnit).ToList();
        }

        public List<SP_GetShippingAgents_Result> GetShippingAgents()
        {
            return Context1.SP_GetShippingAgents().OrderBy(x => x.AgentName).ToList();
        }

        public List<SP_GetCustomerByID_Result> GetCustByID(int customerID)
        {
            return Context1.SP_GetCustomerByID(customerID).OrderBy(x => x.Customer).ToList();
        }

        public List<AcHeadSelectAll_Result> AcHeadSelectAll(int branchID)
        {
            return Context1.AcHeadSelectAll(branchID).OrderBy(x => x.AcHead).ToList();
        }

        public List<AcHeadSelectAll_Result> AcHeadSelectAll(int branchID,string Term)
        {
            return Context1.AcHeadSelectAll(branchID).Where(c => c.AcHead.ToLower().Contains(Term.ToLower())).OrderBy(x => x.AcHead).ToList();
        }

        public List<AcHeadSelectAll_Result> AcHeadById(int branchID, int  Id)
        {
            return Context1.AcHeadSelectAll(branchID).Where(c => c.AcHeadID.Equals(Id)).OrderBy(x => x.AcHead).ToList();
        }

        public List<SP_GetAllJobsDetails_Result> GetAllJobsDetails()
        {
            return Context1.SP_GetAllJobsDetails().OrderBy(x => x.JobCode).ToList();
        }

        public List<SP_GetAllJobsDetails_Result> GetAllJobsDetails(string Term)
        {
            try
            {
                return Context1.SP_GetAllJobsDetails().Where(c => c.JobDescription.ToLower().Contains(Term.ToLower())).OrderBy(c => c.JobCode).ToList();
            }catch(Exception ex)
            {
                List<SP_GetAllJobsDetails_Result>  obj = new List<SP_GetAllJobsDetails_Result>();
                return obj;
            }
        }

        public List<SP_GetAllJobsDetails_Result> JobById(int Id)
        {
            return Context1.SP_GetAllJobsDetails().Where(c => c.JobID.Equals(Id)).OrderBy(x => x.JobCode).ToList();
        }

        public List<SP_GetAllProductServices_Result> GetAllProductServices()
        {
            return Context1.SP_GetAllProductServices().OrderBy(x => x.ProductName).ToList();
        }

        public List<SP_GetAllProductServices_Result> GetAllProductServices(string Term)
        {
            return Context1.SP_GetAllProductServices().Where(c => c.ProductName.ToLower().Contains(Term.ToLower())).OrderBy(x => x.ProductName).ToList();
        }
        public List<SP_GetAllProductServices_Result> ProductById(int Id)
        {
            return Context1.SP_GetAllProductServices().Where(c => c.ProductID.Equals(Id)).OrderBy(x => x.ProductName).ToList();
        }

        public List<AnalysisHeadSelectAll_Result> GetAnalysisHeadSelectList(int BranchId,string Term)
        {
            return Context1.AnalysisHeadSelectAll(BranchId).Where(c => c.AnalysisHead.ToLower().Contains(Term.ToLower())).OrderBy(x => x.AnalysisGroup).ToList();
        }
        
    }
}