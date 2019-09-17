﻿using DAL;
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
    [Authorize]
    public class CustomerController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();

        //
        // GET: /Customer/

        public ActionResult Index()
        {
            var customer = objSourceMastersModel.GetCoustomer();
            return View(customer);
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
            return View();
        }

        //
        // POST: /Customer/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CUSTOMER customer)
        {
            if (ModelState.IsValid)
            {

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
            if (customer == null)
            {
                return HttpNotFound();
            }

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