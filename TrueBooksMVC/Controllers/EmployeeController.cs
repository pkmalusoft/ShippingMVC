using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;

namespace ShippingFinal.Controllers
{
    [SessionExpire]
    [Authorize]
    public class EmployeeController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        //
        // GET: /Designation/

        public ActionResult Index()
        {
            List<EmployeeDesignationVM> model = new List<EmployeeDesignationVM>();
            var query = (from t in entity.Employees
                         join t1 in entity.Designations on t.DesignationID equals t1.DesignationID
                         select new EmployeeDesignationVM

                         {
                             EmployeeName = t.EmployeeName,
                            Designation = t1.Designation1,
                             Nationality = t.Nationality,
                             DOJ = t.DOJ.Value,
                            StatusActive = t.StatusActive.HasValue?t.StatusActive.Value:false,
                            EmployeeID=t.EmployeeID

                             

                         }).ToList();

           // var employee = objSourceMastersModel.GetEmployeet();
            return View(query);
        }
        //
        // GET: /Employee/Details/5

        public ActionResult Details(int id = 0)
        {
            Employee employee = objSourceMastersModel.GetEmployeeById(id);

            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        //
        // GET: /Employee/Create

        public ActionResult Create()
        {
            ViewBag.Designation = DropDownList<Designation>.LoadItems(
                    objSourceMastersModel.GetDesignationt(), "DesignationID", "Designation1");

            ViewBag.Department = DropDownList<Department>.LoadItems(
                  objSourceMastersModel.GetDepartment(), "DepartmentID", "Department1");

            ViewBag.Location = DropDownList<Location>.LoadItems(
                objSourceMastersModel.GetLocation(), "LocationID", "Location1");
            ViewBag.Country = DropDownList<CountryMaster>.LoadItems(objSourceMastersModel.GetCountry(), "CountryID", "CountryName");

            //ViewBag.Designation = Enum.GetValues(typeof(Designation)).Cast<Designation>().Select(c => new SelectListItem { Text = c.ToString(), Value = c.ToString() });
            return View();
        }


        //  //

        public static class DropDownList<T>
        {
            public static SelectList LoadItems(IList<T> collection, string value, string text)
            {
                return new SelectList(collection, value, text);
            }
        }





        //
        // POST: /Employee/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {

                var query = (from t in entity.Employees where (t.EmployeeName == employee.EmployeeName && t.ContactNo==employee.ContactNo)||(t.ContactNo==employee.ContactNo)  select t).ToList();

                if (query.Count > 0)
                {

                    ViewBag.Designation = DropDownList<Designation>.LoadItems(
                    objSourceMastersModel.GetDesignationt(), "DesignationID", "Designation1");

                    ViewBag.Department = DropDownList<Department>.LoadItems(
                          objSourceMastersModel.GetDepartment(), "DepartmentID", "Department1");

                    ViewBag.Location = DropDownList<Location>.LoadItems(
                        objSourceMastersModel.GetLocation(), "LocationID", "Location1");
                    ViewBag.Country = DropDownList<CountryMaster>.LoadItems(objSourceMastersModel.GetCountry(), "CountryID", "CountryName");

                    ViewBag.SuccessMsg = "Employee details are already exist";
                    return View();
                }
                objSourceMastersModel.SaveEmployee(employee);
                TempData["SuccessMSG"] = "You have successfully added Employee.";
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        public JsonResult GetEmployeeName(string empname)
        {
            var employee = (from c in entity.Employees where c.EmployeeName == empname select c).FirstOrDefault();
            Status s = new Status();
            if (employee == null)
            {
                s.flag = 0;
            }
            else
            {
                s.flag = 1;
            }
            
            
          


            return Json(s,JsonRequestBehavior.AllowGet);
        }

        public class Status
        {
            public int flag { get; set; }
        }

        public ActionResult Edit(int id = 0)
        {
            Employee employee = objSourceMastersModel.GetEmployeeById(id);
            if (employee == null)
            {
                return HttpNotFound();
            }


            ViewBag.Designation = new SelectList(objSourceMastersModel.GetDesignationt(), "DesignationID", "Designation1", employee.DesignationID);

            ViewBag.Department = new SelectList(objSourceMastersModel.GetDepartment(), "DepartmentID", "Department1", employee.DepartmentID);

            ViewBag.Location = new SelectList(objSourceMastersModel.GetLocation(), "LocationID", "Location1", employee.LocationID);


            return View(employee);
        }

        //
        // POST: /Employee/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Designation = new SelectList(objSourceMastersModel.GetDesignationt(), "DesignationID", "Designation1", employee.DesignationID);

                ViewBag.Department = new SelectList(objSourceMastersModel.GetDepartment(), "DepartmentID", "Department1", employee.DepartmentID);

                ViewBag.Location = new SelectList(objSourceMastersModel.GetLocation(), "LocationID", "Location1", employee.LocationID);


                if (ModelState.IsValid)
                {
                    objSourceMastersModel.SaveEmployeeById(employee);
                    TempData["SuccessMSG"] = "You have successfully updated Employee.";
                    return RedirectToAction("Index");
                }
                return View(employee);
            }
            return View(employee);
        }

        //
        // GET: /Employee/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Employee emp = objSourceMastersModel.GetEmployeeById(id);
        //    if (emp == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(emp);
        //}

        //
        // POST: /Employee/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            objSourceMastersModel.DeleteEmployee(id);
            TempData["SuccessMSG"] = "You have successfully deleted Employee.";
            return RedirectToAction("Index");
        }
    }
}

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    

