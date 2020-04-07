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
    public class PortController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        //
        // GET: /Designation/

        public ActionResult Index()
        {
            List<PortCountryVM> model = new List<PortCountryVM>();
            var query = (from t in entity.Ports
                         join t1 in entity.CountryMasters on t.CountryID equals t1.CountryID
                         select new PortCountryVM

                         {
                             Port = t.Port1,
                             PortCode = t.PortCode,
                             CountryName = t1.CountryName,
                             PortID=t.PortID


                         }).ToList();

           // var port = objSourceMastersModel.GetPort();
            return View(query);
        }

        //
        // GET: /Port/Details/5

        public ActionResult Details(int id = 0)
        {
            Port port = objSourceMastersModel.GetJobPortById(id);

            if (port == null)
            {
                return HttpNotFound();
            }
            return View(port);
        }

        //
        // GET: /Port/Create

        public ActionResult Create()
        {
            ViewBag.country = DropDownList<CountryMaster>.LoadItems(
                    objSourceMastersModel.GetCountry(), "CountryID", "CountryName");

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
        // POST: /Port/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Port port)
        {

            if (ModelState.IsValid)
            {
                ViewBag.country = DropDownList<CountryMaster>.LoadItems(
                    objSourceMastersModel.GetCountry().OrderBy(x => x.CountryName).ToList(), "CountryID", "CountryName");


                var query = (from t in entity.Ports where t.Port1 == port.Port1 select t).ToList();

                if (query.Count > 0)
                {
                   
                    ViewBag.SuccessMsg = "Port is already exist";
                    return View();
                }
                objSourceMastersModel.SavePort(port);
                TempData["SuccessMSG"] = "You have successfully added Port.";
                return RedirectToAction("Index");
            }

            return View(port);
        }

        //
        // GET: /Port/Edit/5

        public ActionResult Edit(int id )
        {
            Port port = objSourceMastersModel.GetJobPortById(id);
            if (port == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.country = new SelectList(objSourceMastersModel.GetCountry(), "CountryID", "CountryName", port.CountryID);
            return View(port);
        }

        //
        // POST: /Port/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Port port)
        {
                if (ModelState.IsValid)
                {
                    ViewBag.country = new SelectList(objSourceMastersModel.GetCountry(), "CountryID", "CountryName", port.CountryID);
                    objSourceMastersModel.SavePortById(port);
                    TempData["SuccessMSG"] = "You have successfully updated Port.";
                    return RedirectToAction("Index");
                }

                return View(port);
        }

        //
        // GET: /Port/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Port port = objSourceMastersModel.GetJobPortById(id);
        //    if (port == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(port);
        //}

        //
        // POST: /Port/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            objSourceMastersModel.DeletePort(id);
            TempData["SuccessMSG"] = "You have successfully deleted Port.";
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}