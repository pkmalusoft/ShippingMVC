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
    public class MenuCreationController : Controller
    {
        private SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        SourceMastersModel objectSourceMaster = new SourceMastersModel();

        //
        // GET: /MenuCreation/

        public ActionResult Index()
        {
            return View(db.Menus.ToList());
        }

        //
        // GET: /MenuCreation/Details/5

        public ActionResult Details(int id = 0)
        {
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        //
        // GET: /MenuCreation/Create

        public ActionResult Create()
        {
            ViewBag.Menu = db.Menus.ToList();

            return View();
        }

        //
        // POST: /MenuCreation/Create

        [HttpPost]
        public ActionResult Create(Menu menu)
        {

            var query = (from t in db.Menus where t.Title == menu.Title select t).ToList();

            if (query.Count > 0)
            {
                ViewBag.Menu = db.Menus.ToList();
                ViewBag.SuccessMsg = "Menu is already exist";
                return View();
            }

                menu.MenuID = objectSourceMaster.GetMaxNumberMenu();
                menu.IsAccountMenu = false;
                db.Menus.Add(menu);
                db.SaveChanges();
                ViewBag.SuccessMsg = "You have successfully added Menu Creation.";
                return View("Index", db.Menus.ToList());
           
           
           
        }

        //
        // GET: /MenuCreation/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Menu menu = db.Menus.Find(id);
            ViewBag.Menu = db.Menus.ToList();
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        //
        // POST: /MenuCreation/Edit/5

        [HttpPost]
        public ActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                menu.IsAccountMenu = false;
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.SuccessMsg = "You have successfully updated Menu Creation.";
                return View("Index", db.Menus.ToList());
            }
            return View(menu);
        }

        //
        // GET: /MenuCreation/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Menu menu = db.Menus.Find(id);
        //    if (menu == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(menu);
        //}

        //
        // POST: /MenuCreation/Delete/5

        //[HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Menu menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
            db.SaveChanges();
            ViewBag.SuccessMsg = "You have successfully deleted Menu Creation.";
            return View("Index", db.Menus.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}