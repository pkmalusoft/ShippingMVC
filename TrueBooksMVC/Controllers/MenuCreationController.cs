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

            var query = (from t in db.Menus where t.Title == menu.Title && t.IsAccountMenu==menu.IsAccountMenu select t).ToList();

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
            if (menu.ParentID != null || menu.ParentID > 0)
            {
                var roles = (from d in db.RoleMasters select d).ToList();
                foreach (var item in roles)
                {
                    var menuaccess = (from d in db.MenuAccessLevels where d.ParentID == menu.ParentID && d.RoleID==item.RoleID select d).ToList();
                    if (menuaccess.Count > 0)
                    {
                        var menuaccesslevel = new MenuAccessLevel();
                        menuaccesslevel.MenuID = menu.MenuID;
                        menuaccesslevel.RoleID = item.RoleID;
                        menuaccesslevel.CreatedBy = Convert.ToString(Session["UserName"]);
                        menuaccesslevel.CreatedOn = DateTime.Now;
                        menuaccesslevel.ParentID = menu.ParentID;
                        menuaccesslevel.IsActive = 1;
                        menuaccesslevel.ModifiedBy = Convert.ToString(Session["UserName"]);
                        menuaccesslevel.ModifiedOn = DateTime.Now;
                        menuaccesslevel.IsView = false;
                        menuaccesslevel.IsAdd = false;
                        menuaccesslevel.IsDelete = false;
                        menuaccesslevel.IsModify = false;
                        menuaccesslevel.Isprint = false;
                        db.MenuAccessLevels.Add(menuaccesslevel);
                        db.SaveChanges();
                    }
                }
            }
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