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
    public class AddRoleController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        private SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();

        //
        // GET: /AddRole/

        public ActionResult Index()
        {
            return View(db.AspNetRoles.ToList());
        }

        //
        // GET: /AddRole/Details/5

        public ActionResult Details(string id = null)
        {
            AspNetRole aspnetrole = db.AspNetRoles.Find(id);
            if (aspnetrole == null)
            {
                return HttpNotFound();
            }
            return View(aspnetrole);
        }

        //
        // GET: /AddRole/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AddRole/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AspNetRole aspnetrole)
        {
            if (ModelState.IsValid)
            {
                var query = (from t in db.AspNetRoles where t.Name == aspnetrole.Name select t).ToList();

                if (query.Count > 0)
                {
                    ViewBag.SuccessMsg = "Role name is already exist";
                    return View();
                }
                aspnetrole.Id =Convert.ToString( objSourceMastersModel.GetMaxNumberRole());
                db.AspNetRoles.Add(aspnetrole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspnetrole);
        }

        //
        // GET: /AddRole/Edit/5

        public ActionResult Edit(string id = null)
        {
            AspNetRole aspnetrole = db.AspNetRoles.Find(id);
            if (aspnetrole == null)
            {
                return HttpNotFound();
            }
            return View(aspnetrole);
        }

        //
        // POST: /AddRole/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AspNetRole aspnetrole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspnetrole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspnetrole);
        }

        //
        // GET: /AddRole/Delete/5

        public ActionResult Delete(string id = null)
        {
            AspNetRole aspnetrole = db.AspNetRoles.Find(id);
            if (aspnetrole == null)
            {
                return HttpNotFound();
            }
            return View(aspnetrole);
        }

        //
        // POST: /AddRole/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetRole aspnetrole = db.AspNetRoles.Find(id);
            db.AspNetRoles.Remove(aspnetrole);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}