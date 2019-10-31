using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using TrueBooksMVC.Models;

namespace TrueBooksMVC.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        private SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        SourceMastersModel ObjectSourceModel = new SourceMastersModel();
        //
        // GET: /Supplier/

        public ActionResult Index()
        {
            return View(db.Suppliers.OrderBy(x => x.SupplierName).ToList());
        }

        //
        // GET: /Supplier/Details/5

        public ActionResult Details(int id = 0)
        {
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        //
        // GET: /Supplier/Create

        public ActionResult Create()
        {
            ViewBag.country = DropDownList<CountryMaster>.LoadItems(
                ObjectSourceModel.GetCountry(), "CountryID", "CountryName");
            var data = db.RevenueTypes.ToList();
            ViewBag.revenue = data;
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
        // POST: /Supplier/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                ViewBag.country = DropDownList<CountryMaster>.LoadItems(
                    ObjectSourceModel.GetCountry(), "CountryID", "CountryName");
                var query = (from t in db.Suppliers where t.SupplierName == supplier.SupplierName select t).ToList();

                if (query.Count > 0)
                {

                    ViewBag.SuccessMsg = "Supplier name is already exist";
                    return View();
                }

                supplier.SupplierID = ObjectSourceModel.GetMaxNumberSupplier();
                db.Suppliers.Add(supplier);
                db.SaveChanges();
                ViewBag.SuccessMsg = "You have successfully added Supplier.";
                return View("Index", db.Suppliers.ToList());
            }

            return View(supplier);
        }

        //
        // GET: /Supplier/Edit/5

        public ActionResult Edit(int id = 0)
        {

            var data = db.RevenueTypes.ToList();
            ViewBag.revenue = data;

            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            ViewBag.country = new SelectList(ObjectSourceModel.GetCountry(), "CountryID", "CountryName", supplier.CountryID);
            return View(supplier);
        }

        //
        // POST: /Supplier/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.SuccessMsg = "You have successfully updated Supplier.";
                return View("Index", db.Suppliers.ToList());
            }
            return View(supplier);
        }

        //
        // GET: /Supplier/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Supplier supplier = db.Suppliers.Find(id);
        //    if (supplier == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(supplier);
        //}

        //
        // POST: /Supplier/Delete/5

        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            db.Suppliers.Remove(supplier);
            db.SaveChanges();
            ViewBag.SuccessMsg = "You have successfully deleted supplier.";
            return View("Index", db.Suppliers.ToList());
        }


        public JsonResult GetID(string supid)
        {
            int sid = Convert.ToInt32(supid);

            string x = (from c in db.Suppliers where c.SupplierID == sid select c.RevenuTypeIds).FirstOrDefault();

            return Json(x, JsonRequestBehavior.AllowGet);

        }


        public class Rev
        {
            public int RevenueTypeID { get; set; }
            public string RevenueType1 { get; set; }
        }
        public JsonResult GetRevenue()
        {
            List<Rev> lst = new List<Rev>();

            var data = db.RevenueTypes.ToList();

            foreach (var item in data)
            {
                Rev v = new Rev();
                v.RevenueTypeID = item.RevenueTypeID;
                v.RevenueType1 = item.RevenueType1;
                lst.Add(v);

            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
    }
}