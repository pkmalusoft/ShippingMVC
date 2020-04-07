using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;

namespace TrueBooksMVC.Controllers

{
    [SessionExpire]
    [Authorize]
    public class RoleMasterController : Controller
    {
        private SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();

        //
        // GET: /RoleMaster/

        public ActionResult Index()
        {
            return View(db.RoleMasters.ToList());
        }

        //
        // GET: /RoleMaster/Details/5

        public ActionResult Details(int id = 0)
        {
            RoleMaster rolemaster = db.RoleMasters.Find(id);
            if (rolemaster == null)
            {
                return HttpNotFound();
            }
            return View(rolemaster);
        }

        //
        // GET: /RoleMaster/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /RoleMaster/Create

        [HttpPost]
        public ActionResult Create(RoleMaster rolemaster)
        {
            if (ModelState.IsValid)
            {
                var query = (from t in db.RoleMasters where t.RoleName == rolemaster.RoleName select t).ToList();

                if (query.Count > 0)
                {
                   
                    ViewBag.SuccessMsg = "Role is already exist";
                    return View();
                }
                db.RoleMasters.Add(rolemaster);
                db.SaveChanges();
                ViewBag.SuccessMsg = "You have successfully added Role.";
                return View("Index", db.RoleMasters.ToList());
            }

            return View(rolemaster);
        }

        //
        // GET: /RoleMaster/Edit/5

        public ActionResult Edit(int id = 0)
        {
            RoleMaster rolemaster = db.RoleMasters.Find(id);
            if (rolemaster == null)
            {
                return HttpNotFound();
            }
            return View(rolemaster);
        }

        //
        // POST: /RoleMaster/Edit/5

        [HttpPost]
        public ActionResult Edit(RoleMaster rolemaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolemaster).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.SuccessMsg = "You have successfully updated Role.";
                return View("Index", db.RoleMasters.ToList());
            }
            return View(rolemaster);
        }

        //
        // GET: /RoleMaster/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    RoleMaster rolemaster = db.RoleMasters.Find(id);
        //    if (rolemaster == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(rolemaster);
        //}

        //
        // POST: /RoleMaster/Delete/5

        //[HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            RoleMaster rolemaster = db.RoleMasters.Find(id);
            db.RoleMasters.Remove(rolemaster);
            db.SaveChanges();
            ViewBag.SuccessMsg = "You have successfully deleted Role.";
            return View("Index", db.RoleMasters.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}