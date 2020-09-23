using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;

namespace ShippingFinal.Controllers
{
    [SessionExpire]
    [Authorize]
    public class CustomerController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();

        //
        // GET: /Customer/

        public ActionResult Index()
        {
            Session["SearchCustType"] = 0;

            var customer = objSourceMastersModel.GetCoustomer();
            return View(customer);
        }

        public ActionResult SearchCustomer()
        {
            var customer = objSourceMastersModel.GetCoustomer();
            return View("Index", customer);
        }

        [HttpPost]
        public ActionResult SearchCustomer(int SearchName)
        {
            Session["SearchCustType"] = SearchName;

            if (SearchName == 0)
            {
                var customer = objSourceMastersModel.GetCoustomer();
                return View("Index", customer);
            }
            else
            {
                var customer = objSourceMastersModel.GetCoustomer(SearchName);
                return View("Index", customer);
            }
        }
        //
        // GET: /Customer/Details/5

        public ActionResult Details(int id = 0)
        {
            CUSTOMER cust = objSourceMastersModel.GetCustomerById(id);
            if (cust == null)
            {
                return HttpNotFound();
            }
            return View(cust);
        }

        //
        // GET: /Customer/Create

        public ActionResult Create()
        {
            var maximumcust = (from d in db.CUSTOMERs orderby d.CustomerID descending select d).FirstOrDefault();
            var custnum = "10000";
            if (maximumcust != null)
            {
                custnum = maximumcust.ReferenceCode.Substring(maximumcust.ReferenceCode.Length - 5);
            }
            ViewBag.custnum = Convert.ToInt32(custnum) + 1;
            ViewBag.country = DropDownList<CountryMaster>.LoadItems(
                   objSourceMastersModel.GetCountry(), "CountryID", "CountryName");
           ViewBag.CustomerType = new SelectList(new[]
                                     {
                                            new { ID = 1, trans = "Shipping Industry" },
                                            new { ID = 2, trans = "Service Industry" },

                                        },"ID", "trans", 1);
            return View();
        }

        public static class DropDownList<T>
        {
            public static SelectList LoadItems(IList<T> collection, string value, string text)
            {
                return new SelectList(collection, value, text);
            }
        }
        //
        // POST: /Customer/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CUSTOMER customer)
        {
            if (ModelState.IsValid)
            {
                ViewBag.country = DropDownList<CountryMaster>.LoadItems(
                     objSourceMastersModel.GetCountry(), "CountryID", "CountryName");
                ViewBag.CustomerType = new SelectList(new[]
                                    {
                                            new { ID = 1, trans = "Shipping Industry" },
                                            new { ID = 2, trans = "Service Industry" },

                                        }, "ID", "trans", 1);
                var query = (from t in db.CUSTOMERs where t.Customer1 == customer.Customer1 select t).ToList();

                if (query.Count > 0)
                {
                    ViewBag.SuccessMsg = "Customer name is already exist";
                    return View();
                }
                objSourceMastersModel.SaveCoustomer(customer);
                ViewBag.SuccessMsg = "You have successfully added Customer";
                return View("Index", objSourceMastersModel.GetCoustomer());
            }

            return View(customer);

           
        }

        //
        // GET: /Customer/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CUSTOMER customer = objSourceMastersModel.GetCustomerById(id);
            var custnum = customer.ReferenceCode.Substring(customer.ReferenceCode.Length - 5);
            ViewBag.custnum = Convert.ToInt32(custnum);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerType = new SelectList(new[]
                                  {
                                            new { ID = 1, trans = "Shipping Industry" },
                                            new { ID =2, trans = "Service Industry" },

                                        }, "ID", "trans", customer.CustomerType);
            ViewBag.country = new SelectList(objSourceMastersModel.GetCountry(), "CountryID", "CountryName", customer.CountryID);
            return View(customer);
        }

        //
        // POST: /Customer/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CUSTOMER customer)
        {
            if (ModelState.IsValid)
            {
                objSourceMastersModel.SaveCoustomerById(customer);
                ViewBag.SuccessMsg = "You have successfully updated Customer.";
                return View("Index", objSourceMastersModel.GetCoustomer());
            }
            return View(customer);
        }

        //
        // GET: /Customer/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    CUSTOMER customer = objSourceMastersModel.GetCustomerById(id);
        //    if (customer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(customer);
        //}

        //
        // POST: /Customer/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            objSourceMastersModel.DeleteCustomer(id);
            ViewBag.SuccessMsg = "You have successfully deleted Customer.";
            return View("Index", objSourceMastersModel.GetCoustomer());
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}