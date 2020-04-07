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
    [SessionExpire]
    [Authorize]
    public class ContainerTypeController : Controller
    {
        private SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        SourceMastersModel sourceModelobject = new SourceMastersModel();
        //
        // GET: /ContainerType/

        public ActionResult Index()
        {
            return View(db.ContainerTypes.ToList());
        }

        //
        // GET: /ContainerType/Details/5

        public ActionResult Details(int id = 0)
        {
            ContainerType containertype = db.ContainerTypes.Find(id);
            if (containertype == null)
            {
                return HttpNotFound();
            }
            return View(containertype);
        }

        //
        // GET: /ContainerType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ContainerType/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContainerType containertype)
        {
            if (ModelState.IsValid)
            {

                var query = (from t in db.ContainerTypes where t.ContainerType1 == containertype.ContainerType1 select t).ToList();

                if (query.Count > 0)
                {
                    ViewBag.SuccessMsg = "Container name is already exist";
                    return View();
                }

                containertype.ContainerTypeID = sourceModelobject.GetMaxNumberContainer();
                db.ContainerTypes.Add(containertype);
                db.SaveChanges();
                ViewBag.SuccessMsg = "You have successfully added Container Type.";

                return View("Index", db.ContainerTypes.ToList());
            }

            return View(containertype);
        }

        //
        // GET: /ContainerType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ContainerType containertype = db.ContainerTypes.Find(id);
            if (containertype == null)
            {
                return HttpNotFound();
            }
            return View(containertype);
        }

        //
        // POST: /ContainerType/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContainerType containertype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(containertype).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.SuccessMsg = "You have successfully updated Container Type.";

                return View("Index", db.ContainerTypes.ToList());
            }
            return View(containertype);
        }

        //
        // GET: /ContainerType/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    ContainerType containertype = db.ContainerTypes.Find(id);
        //    if (containertype == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(containertype);
        //}

        //
        // POST: /ContainerType/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContainerType containertype = db.ContainerTypes.Find(id);
            db.ContainerTypes.Remove(containertype);
            db.SaveChanges();
            ViewBag.SuccessMsg = "You have successfully Deleted Container Type.";

            return View("Index", db.ContainerTypes.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}