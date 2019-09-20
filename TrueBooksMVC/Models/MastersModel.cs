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

        public List<SP_GetAllSupplier_Result> GetAllSupplier()
        {
            var query = Context1.SP_GetAllSupplier().OrderBy(x => x.SupplierName).ToList();

            return query;
        }

        public List<SP_GetAllPorts_Result> GetAllPort()
        {
            var query = Context1.SP_GetAllPorts().OrderBy(x => x.Port).ToList();

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
    }
}