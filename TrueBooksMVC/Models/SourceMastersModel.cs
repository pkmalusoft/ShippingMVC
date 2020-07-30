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
    public class SourceMastersModel
    {
        SHIPPING_FinalEntities Context1 = new SHIPPING_FinalEntities();


        #region ACCompnay
        public List<AcCompany> GetAllAcCompanies()
        {

            var query = Context1.AcCompanies.OrderBy(x => x.AcCompany1).ToList();

            return query;
        }

        public AcCompany GetAcCompaniesById(int id)
        {

            var query = Context1.AcCompanies.Where(item => item.AcCompanyID == id).FirstOrDefault();

            return query;
        }

        public bool SaveAcCompaniesById(AcCompany iAcCompany)
        {

            try
            {
                if (iAcCompany.AcCompanyID > 0)
                {
                    Context1.Entry(iAcCompany).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    Context1.AcCompanies.Add(iAcCompany);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            // return false;

        }
        public bool SaveAcCompanies(AcCompany iAcCompany)
        {

            try
            {
                {
                    iAcCompany.AcCompanyID = GetMaxNumberAcCompany();
                    Context1.AcCompanies.Add(iAcCompany);
                    Context1.SaveChanges();

                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            // return false;

        }
        public int GetMaxNumberAcCompany()
        {

            var query = Context1.AcCompanies.OrderByDescending(item => item.AcCompanyID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.AcCompanyID + 1;
            }


        }
        //public bool EditAcCompaniesById(AcCompany iAcCompany)
        //{
        //    var query = Context1.AcCompanies.Where(item => item.AcCompanyID == id).FirstOrDefault();
        //    Context1.Entry(iAcCompany).State = EntityState.Modified;
        //    Context1.SaveChanges();

        //}

        public bool DeleteCompany(int iAcCompanyID)
        {

            try
            {
                if (iAcCompanyID > 0)
                {
                    AcCompany accompany = Context1.AcCompanies.Find(iAcCompanyID);
                    Context1.AcCompanies.Remove(accompany);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;

        }

        #endregion
        #region Branch
        public List<BranchMaster> GetBranchMasters(int CompId)
        {

            var query = Context1.BranchMasters.Where(d=>d.AcCompanyID==CompId).ToList();

            return query;
        }
        #endregion
        #region ACGroup

        public List<AcGroup> GetAllAcGroup()
        {

            var query = Context1.AcGroups.ToList();
            return query;
        }

        public AcGroup GetAcGroupById(int id)
        {

            var query = Context1.AcGroups.Where(item => item.AcGroupID == id).FirstOrDefault();

            return query;
        }
        public bool SaveAcGroupsById(AcGroup iAcGroup)
        {
            try
            {
                if (iAcGroup.AcBranchID > 0)
                {
                    Context1.Entry(iAcGroup).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    Context1.AcGroups.Add(iAcGroup);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            // return false;

        }
        public bool SaveAcGroups(AcGroup iAcGroup)
        {
            try
            {


                {
                    iAcGroup.AcGroupID = GetMaxNumberAcgroup();
                    Context1.AcGroups.Add(iAcGroup);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            // return false;

        }
        public int GetMaxNumberAcgroup()
        {

            var query = Context1.AcGroups.OrderByDescending(item => item.AcGroupID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.AcGroupID + 1;
            }


        }

        public bool DeleteAcGroup(int iAcGroupID)
        {

            try
            {
                if (iAcGroupID > 0)
                {
                    AcGroup acgroup = Context1.AcGroups.Find(iAcGroupID);
                    Context1.AcGroups.Remove(acgroup);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }


        #endregion

        #region carrier

        public List<Carrier> GetCarrier()
        {

            var query = Context1.Carriers.OrderBy(x => x.Carrier1).ToList();

            return query;
        }

        public Carrier GetAcCarrierById(int id)
        {

            var query = Context1.Carriers.Where(item => item.CarrierID == id).FirstOrDefault();

            return query;
        }
        public bool SaveCarrierById(Carrier iCarrier)
        {

            try
            {
                if (iCarrier.CarrierID > 0)
                {
                    Context1.Entry(iCarrier).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    Context1.Carriers.Add(iCarrier);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool SaveCarrier(Carrier iCarrier)
        {

            try
            {
                {
                    iCarrier.CarrierID = GetMaxNumberCarrier();
                    Context1.Carriers.Add(iCarrier);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool DeleteCarrier(int iCarrierId)
        {

            try
            {
                if (iCarrierId > 0)
                {
                    Carrier carrier = Context1.Carriers.Find(iCarrierId);
                    Context1.Carriers.Remove(carrier);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        public int GetMaxNumberCarrier()
        {

            var query = Context1.Carriers.OrderByDescending(item => item.CarrierID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.CarrierID + 1;
            }


        }

        #endregion

        #region country

        public List<CountryMaster> GetCountry()
        {

            var query = Context1.CountryMasters.OrderBy(x => x.CountryName).ToList();

            return query;
        }

        public CountryMaster GetCountryById(int id)
        {

            var query = Context1.CountryMasters.Where(item => item.CountryID == id).FirstOrDefault();

            return query;
        }
        public bool SaveCountryById(CountryMaster iCountry)
        {

            try
            {
                if (iCountry.CountryID > 0)
                {
                    Context1.Entry(iCountry).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    Context1.CountryMasters.Add(iCountry);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            // return false;

        }
        public bool SaveCountry(CountryMaster iCountry)
        {

            try
            {
                {
                    iCountry.CountryID = GetMaxNumberCountry();
                    Context1.CountryMasters.Add(iCountry);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }

        public bool DeleteCountry(int iCountryId)
        {

            try
            {
                if (iCountryId > 0)
                {
                    CountryMaster country = Context1.CountryMasters.Find(iCountryId);
                    Context1.CountryMasters.Remove(country);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        public int GetMaxNumberCountry()
        {

            var query = Context1.CountryMasters.OrderByDescending(item => item.CountryID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.CountryID + 1;
            }


        }

        #endregion

        #region coustomer

        public List<CUSTOMER> GetCoustomer()
        {

            //var query = Context1.CUSTOMERs.Take(100).OrderBy(x => x.Customer1).ToList();
            var query = Context1.CUSTOMERs.OrderBy(x => x.Customer1).ToList();

            return query;
        }
        public List<CUSTOMER> GetCoustomer(int CustomerType)
        {

            var query = Context1.CUSTOMERs.Where(c=> c.CustomerType==CustomerType).OrderBy(x => x.Customer1).ToList();

            return query;
        }
        public CUSTOMER GetCustomerById(int id)
        {

            var query = Context1.CUSTOMERs.Where(item => item.CustomerID == id).FirstOrDefault();

            return query;
        }
        public bool SaveCoustomerById(CUSTOMER iCustomer)
        {

            try
            {
                if (iCustomer.CustomerID > 0)
                {

                    Context1.Entry(iCustomer).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    iCustomer.CustomerID = GetMaxNumberCustomer();
                    Context1.CUSTOMERs.Add(iCustomer);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool SaveCoustomer(CUSTOMER iCustomer)
        {

            try
            {

                {
                    iCustomer.CustomerID = GetMaxNumberCustomer();
                    Context1.CUSTOMERs.Add(iCustomer);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool DeleteCustomer(int iCustomerId)
        {

            try
            {
                if (iCustomerId > 0)
                {
                    CUSTOMER cust = Context1.CUSTOMERs.Find(iCustomerId);
                    Context1.CUSTOMERs.Remove(cust);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        public int GetMaxNumberCustomer()
        {

            var query = Context1.CUSTOMERs.OrderByDescending(item => item.CustomerID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.CustomerID + 1;
            }


        }


        #endregion

        #region department

        public List<Department> GetDepartment()
        {

            var query = Context1.Departments.OrderBy(x => x.Department1).ToList();

            return query;
        }

        public Department GetepartmentById(int id)
        {

            var query = Context1.Departments.Where(item => item.DepartmentID == id).FirstOrDefault();

            return query;
        }
        public bool SaveDepartmentById(Department iDepartment)
        {

            try
            {
                if (iDepartment.DepartmentID > 0)
                {
                    Context1.Entry(iDepartment).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    Context1.Departments.Add(iDepartment);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool SaveDepartment(Department iDepartment)
        {

            try
            {

                {
                    iDepartment.DepartmentID = GetMaxNumberDepartment();
                    Context1.Departments.Add(iDepartment);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool DeleteDepartment(int iDepartmentId)
        {

            try
            {
                if (iDepartmentId > 0)
                {
                    Department dept = Context1.Departments.Find(iDepartmentId);
                    Context1.Departments.Remove(dept);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        public int GetMaxNumberDepartment()
        {

            var query = Context1.Departments.OrderByDescending(item => item.DepartmentID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.DepartmentID + 1;
            }


        }

        #endregion

        #region Designation

        public List<Designation> GetDesignationt()
        {

            var query = Context1.Designations.OrderBy(x => x.Designation1).ToList();

            return query;
        }

        public Designation GetDesignationById(int id)
        {

            var query = Context1.Designations.Where(item => item.DesignationID == id).FirstOrDefault();

            return query;
        }
        public bool SaveDesignationById(Designation idesignation)
        {
            try
            {
                if (idesignation.DesignationID > 0)
                {
                    Context1.Entry(idesignation).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    Context1.Designations.Add(idesignation);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool SaveDesignation(Designation idesignation)
        {
            try
            {
                idesignation.DesignationID = GetMaxNumberDesignation();
                Context1.Designations.Add(idesignation);
                Context1.SaveChanges();

                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool DeleteDesignation(int iDesignationId)
        {

            try
            {
                if (iDesignationId > 0)
                {
                    Designation designation = Context1.Designations.Find(iDesignationId);
                    Context1.Designations.Remove(designation);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        public int GetMaxNumberDesignation()
        {

            var query = Context1.Designations.OrderByDescending(item => item.DesignationID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.DesignationID + 1;
            }


        }

        #endregion

        #region Employee

        public List<Employee> GetEmployeet()
        {

            var query = Context1.Employees.OrderBy(x => x.EmployeeName).ToList();

            return query;
        }

        public Employee GetEmployeeById(int id)
        {

            var query = Context1.Employees.Where(item => item.EmployeeID == id).FirstOrDefault();

            return query;
        }
        public bool SaveEmployeeById(Employee iEmployee)
        {

            try
            {
                if (iEmployee.EmployeeID > 0)
                {
                    Context1.Entry(iEmployee).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    Context1.Employees.Add(iEmployee);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool SaveEmployee(Employee iEmployee)
        {

            try
            {

                {
                    iEmployee.EmployeeID = GetMaxNumberEmployee();
                    Context1.Employees.Add(iEmployee);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool DeleteEmployee(int iEmployeeId)
        {

            try
            {
                if (iEmployeeId > 0)
                {
                    Employee emp = Context1.Employees.Find(iEmployeeId);
                    Context1.Employees.Remove(emp);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        public int GetMaxNumberEmployee()
        {

            var query = Context1.Employees.OrderByDescending(item => item.EmployeeID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.EmployeeID + 1;
            }


        }

        #endregion



        #region jobMode

        public List<JobMode> GetJobMode()
        {

            var query = Context1.JobModes.OrderBy(x => x.JobMode1).ToList();

            return query;
        }

        public JobMode GetJobModeById(int id)
        {

            var query = Context1.JobModes.Where(item => item.JobModeID == id).FirstOrDefault();

            return query;
        }
        public bool SaveJobModeById(JobMode iJobmode)
        {


            try
            {
                if (iJobmode.JobModeID > 0)
                {
                    Context1.Entry(iJobmode).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    Context1.JobModes.Add(iJobmode);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool SaveJobMode(JobMode iJobmode)
        {


            try
            {

                {
                    iJobmode.JobModeID = GetMaxNumberJobMode();
                    Context1.JobModes.Add(iJobmode);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool DeleteJobMode(int iJobmodeId)
        {

            try
            {
                if (iJobmodeId > 0)
                {
                    JobMode jobmode = Context1.JobModes.Find(iJobmodeId);
                    Context1.JobModes.Remove(jobmode);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        public int GetMaxNumberJobMode()
        {

            var query = Context1.JobModes.OrderByDescending(item => item.JobModeID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.JobModeID + 1;
            }


        }

        #endregion

        #region jobType

        public List<JobType> GetJobType()
        {

            var query = Context1.JobTypes.OrderBy(x => x.JobDescription).ToList();

            return query;
        }

        public JobType GetJobTypeById(int id)
        {

            var query = Context1.JobTypes.Where(item => item.JobTypeID == id).FirstOrDefault();

            return query;
        }
        public bool SaveJobTypeById(JobType iJobtype)
        {

            try
            {
                if (iJobtype.JobTypeID > 0)
                {

                    Context1.Entry(iJobtype).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    Context1.JobTypes.Add(iJobtype);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //  return false;

        }
        public bool SaveJobType(JobType iJobtype)
        {

            try
            {
                {
                    JobType j = (from c in Context1.JobTypes select c).FirstOrDefault();
                    if (j == null)
                    {
                        iJobtype.JobTypeID = 1;
                    }
                    else
                    {
                        iJobtype.JobTypeID = GetMaxNumberJobType();
                    }
                    Context1.JobTypes.Add(iJobtype);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool DeleteJobType(int iJobtypeId)
        {

            try
            {
                if (iJobtypeId > 0)
                {
                    JobType jobtype = Context1.JobTypes.Find(iJobtypeId);
                    Context1.JobTypes.Remove(jobtype);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        public int GetMaxNumberJobType()
        {

            var query = Context1.JobTypes.OrderByDescending(item => item.JobTypeID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.JobTypeID + 1;
            }


        }

        #endregion

        #region Location

        public List<Location> GetLocation()
        {

            var query = Context1.Locations.OrderBy(x => x.Location1).ToList();

            return query;
        }

        public Location GetJobLocationById(int id)
        {

            var query = Context1.Locations.Where(item => item.LocationID == id).FirstOrDefault();

            return query;
        }
        public bool SaveLocationById(Location iLocation)
        {


            try
            {
                if (iLocation.LocationID > 0)
                {
                    Context1.Entry(iLocation).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    Context1.Locations.Add(iLocation);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool SaveLocation(Location iLocation)
        {


            try
            {
                iLocation.LocationID = GetMaxNumberLocation();
                Context1.Locations.Add(iLocation);
                Context1.SaveChanges();

                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool DeleteLocation(int iLocationId)
        {

            try
            {
                if (iLocationId > 0)
                {
                    Location location = Context1.Locations.Find(iLocationId);
                    Context1.Locations.Remove(location);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        public int GetMaxNumberLocation()
        {

            var query = Context1.Locations.OrderByDescending(item => item.LocationID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.LocationID + 1;
            }


        }


        #endregion

        #region port

        public List<Port> GetPort()
        {

            var query = Context1.Ports.OrderBy(x => x.Port1).ToList();

            return query;
        }

        public Port GetJobPortById(int id)
        {

            var query = Context1.Ports.Where(item => item.PortID == id).FirstOrDefault();

            return query;
        }
        public bool SavePortById(Port iPort)
        {

            try
            {
                //if (iPort.PortID > 0)
                //{
                //    iPort.PortID = GetMaxNumberPort();
                //    Context1.Entry(iPort).State = EntityState.Modified;
                //    Context1.SaveChanges();
                //}
                //else
                //{
                //    Context1.Ports.Add(iPort);
                //    Context1.SaveChanges();
                //}

                Context1.Entry(iPort).State = EntityState.Modified;
                Context1.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool SavePort(Port iPort)
        {

            try
            {

                iPort.PortID = GetMaxNumberPort();
                //Context1.Entry(iPort).State = EntityState.Modified;
                Context1.Ports.Add(iPort);
                Context1.SaveChanges();

                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool SavePortByID(Port iPort)
        {

            try
            {
                if (iPort.PortID > 0)
                {
                    iPort.PortID = GetMaxNumberPort();
                    Context1.Entry(iPort).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    Context1.Ports.Add(iPort);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool DeletePort(int iPortId)
        {

            try
            {
                if (iPortId > 0)
                {
                    Port port = Context1.Ports.Find(iPortId);
                    Context1.Ports.Remove(port);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        public int GetMaxNumberPort()
        {

            var query = Context1.Ports.OrderByDescending(item => item.PortID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.PortID + 1;
            }


        }

        #endregion

        #region RevenueType

        public List<RevenueType> GetRevenueType()
        {

            var query = Context1.RevenueTypes.OrderBy(x => x.RevenueType1).ToList();

            return query;
        }

        public RevenueType GetRevenueTypeById(int id)
        {

            var query = Context1.RevenueTypes.Where(item => item.RevenueTypeID == id).FirstOrDefault();

            return query;
        }
        public bool SaveRevenueTypeById(RevenueType iRevenuetype)
        {

            try
            {
                if (iRevenuetype.RevenueTypeID > 0)
                {
                    Context1.Entry(iRevenuetype).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    iRevenuetype.RevenueTypeID = GetMaxNumberRevenueType();
                    Context1.RevenueTypes.Add(iRevenuetype);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }

        public bool SaveRevenueType(RevenueType iRevenuetype)
        {

            try
            {



                iRevenuetype.RevenueTypeID = GetMaxNumberRevenueType();
                Context1.RevenueTypes.Add(iRevenuetype);
                Context1.SaveChanges();

                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool DeleteRevenueType(int iRevenueTypeId)
        {

            try
            {
                if (iRevenueTypeId > 0)
                {
                    RevenueType revenueType = Context1.RevenueTypes.Find(iRevenueTypeId);
                    Context1.RevenueTypes.Remove(revenueType);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        public int GetMaxNumberRevenueType()
        {

            var query = Context1.RevenueTypes.OrderByDescending(item => item.RevenueTypeID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.RevenueTypeID + 1;
            }


        }

        #endregion

        #region ShippingAgent

        public List<ShippingAgent> GetShippingAgent()
        {

            var query = Context1.ShippingAgents.OrderBy(x => x.AgentName).ToList();

            return query;
        }

        public ShippingAgent GetShippingAgentById(int id)
        {

            var query = Context1.ShippingAgents.Where(item => item.ShippingAgentID == id).FirstOrDefault();

            return query;
        }
        public bool SaveShippingAgentById(ShippingAgent iShippingagent)
        {

            try
            {
                if (iShippingagent.ShippingAgentID > 0)
                {
                    Context1.Entry(iShippingagent).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    iShippingagent.ShippingAgentID = GetMaxNumberShippingAgent();
                    Context1.ShippingAgents.Add(iShippingagent);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool DeleteShippingAgent(int iShippingAgentId)
        {

            try
            {
                if (iShippingAgentId > 0)
                {
                    ShippingAgent shippingAgent = Context1.ShippingAgents.Find(iShippingAgentId);
                    Context1.ShippingAgents.Remove(shippingAgent);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        public int GetMaxNumberShippingAgent()
        {

            var query = Context1.ShippingAgents.OrderByDescending(item => item.ShippingAgentID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.ShippingAgentID + 1;
            }


        }

        #endregion

        #region Transporter

        public List<Transporter> GetTransporter()
        {

            var query = Context1.Transporters.OrderBy(x => x.TransPorter1).ToList();

            return query;
        }

        public Transporter GetTransporterById(int id)
        {

            var query = Context1.Transporters.Where(item => item.TransPorterID == id).FirstOrDefault();

            return query;
        }
        public bool SaveTransporterById(Transporter iTransporter)
        {

            try
            {
                if (iTransporter.TransPorterID > 0)
                {
                    Context1.Entry(iTransporter).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    Context1.Transporters.Add(iTransporter);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            // return false;

        }
        public bool SaveTransporter(Transporter iTransporter)
        {

            try
            {

                {
                    iTransporter.TransPorterID = GetMaxNumberTransporter();
                    Context1.Transporters.Add(iTransporter);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            // return false;

        }
        public bool DeleteTransportert(int iTransporterId)
        {

            try
            {
                if (iTransporterId > 0)
                {
                    Transporter tansporter = Context1.Transporters.Find(iTransporterId);
                    Context1.Transporters.Remove(tansporter);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        public int GetMaxNumberTransporter()
        {

            var query = Context1.Transporters.OrderByDescending(item => item.TransPorterID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.TransPorterID + 1;
            }


        }

        #endregion

        #region Vayege

        public List<Voyage> GetVoyage()
        {

            var query = Context1.Voyages.OrderBy(x => x.Voyage1).ToList();

            return query;
        }

        public Voyage GetVoyageById(int id)
        {

            var query = Context1.Voyages.Where(item => item.VoyageID == id).FirstOrDefault();

            return query;
        }
        public bool SaveVoyageById(Voyage iVoyage)
        {

            try
            {
                if (iVoyage.VoyageID > 0)
                {
                    Context1.Entry(iVoyage).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {

                    Context1.Voyages.Add(iVoyage);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            // return false;

        }
        public bool SaveVoyage(Voyage iVoyage)
        {

            try
            {

                {
                    iVoyage.VoyageID = GetMaxNumberVoyage();
                    Context1.Voyages.Add(iVoyage);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            // return false;

        }
        public bool DeleteVoyage(int iVoyageId)
        {

            try
            {
                if (iVoyageId > 0)
                {
                    Voyage voyage = Context1.Voyages.Find(iVoyageId);
                    Context1.Voyages.Remove(voyage);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        public int GetMaxNumberVoyage()
        {

            var query = Context1.Voyages.OrderByDescending(item => item.VoyageID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.VoyageID + 1;
            }


        }

        #endregion

        #region Vessel

        public List<VesselList> GetVessel()
        {

            List<VesselList> query = new List<VesselList>();
            query = (from v in Context1.Vessels join c in Context1.Carriers on v.CarrierID equals c.CarrierID select new VesselList { VesselID = v.VesselID, Vessel = v.Vessel1, Carrier = c.Carrier1 }).OrderBy(x => x.Vessel).ToList();

            return query;
        }

        public Vessel GetVesselById(int id)
        {

            var query = Context1.Vessels.Where(item => item.VesselID == id).FirstOrDefault();

            return query;
        }
        public bool SaveVesselById(Vessel iVessel)
        {

            try
            {
                if (iVessel.VesselID > 0)
                {
                    Context1.Entry(iVessel).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    iVessel.VesselID = GetMaxNumberVessel();
                    Context1.Vessels.Add(iVessel);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //  return false;

        }
        public bool DeleteVessel(int iVesselId)
        {

            try
            {
                if (iVesselId > 0)
                {
                    Vessel vessel = Context1.Vessels.Find(iVesselId);
                    Context1.Vessels.Remove(vessel);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        public int GetMaxNumberVessel()
        {

            var query = Context1.Vessels.OrderByDescending(item => item.VesselID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.VesselID + 1;
            }


        }

        #endregion

        #region Menu
        public int GetMaxNumberMenu()
        {

            var query = Context1.Menus.OrderByDescending(item => item.MenuID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.MenuID + 1;
            }


        }
        #endregion
        #region Role
        public int GetMaxNumberRole()
        {

            var query = Context1.AspNetRoles.OrderByDescending(item => item.Id).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return (Convert.ToInt32(query.Id) + 1);
            }


        }
        #endregion
        #region Currency
        public int GetMaxNumberCurrency()
        {

            var query = Context1.CurrencyMasters.OrderByDescending(item => item.CurrencyID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.CurrencyID + 1;
            }


        }
        #endregion
        #region Registration
        public int GetMaxNumberRegistration()
        {

            var query = Context1.UserRegistrations.OrderByDescending(item => item.UserID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.UserID + 1;
            }


        }

        public UserRegistration GetUserRegistrationById(int id)
        {

            var query = Context1.UserRegistrations.Where(item => item.UserID == id).FirstOrDefault();

            return query;
        }
        #endregion
        #region IsAccessible
        public bool IsAccessibleMenu(int menuID, List<int> RoleIDs)
        {
            SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();

            var result = (from t in entity.MenuAccessLevels
                          join role in entity.Menus on t.MenuID equals role.MenuID
                          where t.MenuID == menuID && RoleIDs.Contains(t.RoleID.Value)
                          select t).ToList();
            if (result != null && result.Count > 0)
            {
                return true;
            }
            return false;
        }

        #endregion
        #region Container Type

        public int GetMaxNumberContainer()
        {

            var query = Context1.ContainerTypes.OrderByDescending(item => item.ContainerTypeID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.ContainerTypeID + 1;
            }


        }
        #endregion

        #region supplier

        public int GetMaxNumberSupplier()
        {

            var query = Context1.Suppliers.OrderByDescending(item => item.SupplierID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.SupplierID + 1;
            }


        }
        #endregion

        #region Branch

        public int GetMaxNumberBranch()
        {

            var query = Context1.BranchMasters.OrderByDescending(item => item.BranchID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.BranchID + 1;
            }


        }

        #endregion
        # region AcFinancialYear
        public int GetMaxNumberAcFinancialYear()
        {

            var query = Context1.AcFinancialYears.OrderByDescending(item => item.AcFinancialYearID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.AcFinancialYearID + 1;
            }


        }

        public AcFinancialYear GetACFinancialYearByBranchId(int iBranchID)
        {

            var query = Context1.AcFinancialYears.Where(item => item.BranchID == iBranchID).FirstOrDefault();

            if (query == null)
            {
                return new AcFinancialYear();
            }
            else
            {
                return query;
            } 
        }
        #endregion

        # region AcHeadAssign
        public int GetMaxNumberAcHeadAssign()
        {

            var query = Context1.AcHeadAssigns.OrderByDescending(item => item.ID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.ID + 1;
            }


        }

        public AcHeadAssign GetAcHeadAssign()
        {

            var query = Context1.AcHeadAssigns.FirstOrDefault();

            if (query == null)
            {
                return null;
            }
            else
            {
                return query;
            }


        }
        #endregion

        # region AcJournalDetails
        public int GetMaxNumberAcJournalDetails()
        {

            var query = Context1.AcJournalDetails.OrderByDescending(item => item.AcJournalDetailID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.AcJournalDetailID + 1;
            }
        }


        public int GetMaxNumberAcJournalMasters()
        {

            var query = Context1.AcJournalMasters.OrderByDescending(item => item.AcJournalID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.AcJournalID + 1;
            }



        }

        public int GetMaxNumberAcOpeningInvoiceMaster()
        {

            var query = Context1.AcOPInvoiceMasters.OrderByDescending(item => item.AcOPInvoiceMasterID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.AcOPInvoiceMasterID + 1;
            }



        }

        public int GetMaxNumberAcOpeningInvoiceDetails()
        {

            var query = Context1.AcOPInvoiceDetails.OrderByDescending(item => item.AcOPInvoiceDetailID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.AcOPInvoiceDetailID + 1;
            }



        }
        #endregion



        #region country

        public List<ProductService> GetProduct()
        {

            var query = Context1.ProductServices.OrderBy(x => x.ProductName).ToList();

            return query;
        }


        public ProductService GetProductById(int id)
        {

            var query = Context1.ProductServices.Where(item => item.ProductID == id).FirstOrDefault();

            return query;
        }

        public bool SaveProductById(ProductService iProduct)
        {

            try
            {
                if (iProduct.ProductID > 0)
                {
                    Context1.Entry(iProduct).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    Context1.ProductServices.Add(iProduct);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            // return false;

        }
        public bool SaveProduct(ProductService iProduct)
        {

            try
            {
                {
                 //   iProduct.ProductID = GetMaxNumberProduct();
                    Context1.ProductServices.Add(iProduct);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            //return false;

        }

        public bool DeleteProduct(int iProductId)
        {

            try
            {
                if (iProductId > 0)
                {
                    ProductService product = Context1.ProductServices.Find(iProductId);
                    Context1.ProductServices.Remove(product);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        public int GetMaxNumberProduct()
        {

            var query = Context1.ProductServices.OrderByDescending(item => item.ProductID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.ProductID + 1;
            }


        }










        #endregion

        #region MenuAccesslevel
       
        public bool GetAddpermission(int RoleId,string href)
        {
            var menus = Context1.Menus;
            var menuid = menus.Where(d => d.Link.Contains(href)).FirstOrDefault().MenuID;
            var permission = Context1.MenuAccessLevels.Where(d => d.RoleID == RoleId && d.MenuID == menuid && d.IsAdd == true).FirstOrDefault();
            if(permission!=null)
            {
                return true;
            }
            return false;
        }
        public bool GetModifypermission(int RoleId, string href)
        {
            var menus = Context1.Menus;
            var menuid = menus.Where(d => d.Link.Contains(href)).FirstOrDefault().MenuID;
            var permission = Context1.MenuAccessLevels.Where(d => d.RoleID == RoleId && d.MenuID == menuid && d.IsModify == true).FirstOrDefault();
            if (permission != null)
            {
                return true;
            }
            return false;
        }
        public bool GetDeletepermission(int RoleId, string href)
        {
            var menus = Context1.Menus;
            var menuid = menus.Where(d => d.Link.Contains(href)).FirstOrDefault().MenuID;
            var permission = Context1.MenuAccessLevels.Where(d => d.RoleID == RoleId && d.MenuID == menuid && d.IsDelete == true).FirstOrDefault();
            if (permission != null)
            {
                return true;
            }
            return false;
        }
        public bool GetPrintpermission(int RoleId, string href)
        {
            var menus = Context1.Menus;
            var menuid = menus.Where(d => d.Link.Contains(href)).FirstOrDefault().MenuID;
            var permission = Context1.MenuAccessLevels.Where(d => d.RoleID == RoleId && d.MenuID == menuid && d.Isprint == true).FirstOrDefault();
            if (permission != null)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region JobStatus

        public List<JobStatu> GetJobStatus()
        {

            var query = Context1.JobStatus.OrderBy(x => x.StatusName).ToList();

            return query;
        }

        public JobStatu GetJobStatusById(int id)
        {

            var query = Context1.JobStatus.Where(item => item.JobStatusId == id).FirstOrDefault();

            return query;
        }
        public bool SaveJobStatusById(JobStatu iJobstatus)
        {


            try
            {
                if (iJobstatus.JobStatusId > 0)
                {
                    Context1.Entry(iJobstatus).State = EntityState.Modified;
                    Context1.SaveChanges();
                }
                else
                {
                    Context1.JobStatus.Add(iJobstatus);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool SaveJobStaus(JobStatu iJobstatus)
        {


            try
            {

                {
                    Context1.JobStatus.Add(iJobstatus);
                    Context1.SaveChanges();
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            //return false;

        }
        public bool DeleteJobStatus(int iJobstatusId)
        {

            try
            {
                if (iJobstatusId > 0)
                {
                    JobStatu jobmode = Context1.JobStatus.Find(iJobstatusId);
                    Context1.JobStatus.Remove(jobmode);
                    Context1.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
    

        #endregion
    }
}


