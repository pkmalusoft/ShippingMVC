using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using TrueBooksMVC.Models;

namespace TrueBooksMVC.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        //
        // GET: /Default1/
        public ActionResult List()
        {
            List<MenuRoleVM> model = new List<MenuRoleVM>();
            var query =
                            (from t in entity.MenuAccessLevels
                             join t1 in entity.RoleMasters on t.RoleID equals t1.RoleID
                             join t2 in entity.Menus on t.MenuID equals t2.MenuID
                             select new MenuRoleVM

                             {
                                 Name = t1.RoleName,
                                 Title = t2.Title,
                                 MenuAccessID = t.MenuAccessID,
                                 MenuID=t.MenuID
                                
                                 //    MenuAccessID = Convert.ToInt32(t1.Id),

                             }).ToList();



            return View(query);
        }

        public ActionResult Index()
        {
            var query = from t in entity.Menus
                        where t.ParentID == null
                        select t;

            ViewBag.Menu = query;
            var query1 = from t in entity.RoleMasters
                         select t;
            ViewBag.Role = query1;
           // ViewBag.Role = entity.RoleMasters;

            return View();
        }
        [HttpPost]
        public ActionResult Index(MenuRoleVM model)
        {

            MenuAccessLevel obj = new MenuAccessLevel();
            obj.RoleID = model.RoleId;
            obj.MenuID = model.MenuID;
            obj.CreatedOn = DateTime.Now;
            obj.CreatedBy = Convert.ToString(Session["UserName"]);
            obj.ParentID = model.ParentID;
            obj.IsActive = 1;
            obj.ModifiedBy = Convert.ToString(Session["UserName"]);
            obj.ModifiedOn = DateTime.Now;


            entity.MenuAccessLevels.Add(obj);
            entity.SaveChanges();
            ViewBag.SuccessMsg = "You have successfully added Menu Role List.";
            return RedirectToAction("List");
        }

        public ActionResult Edit(int id = 0)
        {


            MenuRoleVM objViewmodel = (from t in entity.MenuAccessLevels
                                       where t.MenuAccessID==id
                                       select new MenuRoleVM

                       {
                           RoleId = t.RoleID,
                           MenuID = t.MenuID,
                           MenuAccessID = t.MenuAccessID

                           //    MenuAccessID = Convert.ToInt32(t1.Id),

                       }).FirstOrDefault();


            MenuAccessLevel obj = entity.MenuAccessLevels.Find(id);
            ViewBag.Menu = new SelectList(entity.Menus, "MenuID", "Title");
            ViewBag.Role = new SelectList(entity.RoleMasters, "RoleID", "RoleName");
            return View(objViewmodel);
        }

        [HttpPost]
        public ActionResult Edit(MenuRoleVM model,int id=0)
        {
           
            MenuAccessLevel obj = entity.MenuAccessLevels.Find(id);
            obj.RoleID = model.RoleId;
            obj.MenuID = model.MenuID;
            obj.ModifiedBy = Convert.ToString(Session["UserName"]);
            obj.ModifiedOn = DateTime.Now;


           //entity.Entry(obj).State = System.Data.EntityState.Modified;
            entity.SaveChanges();
            ViewBag.SuccessMsg = "You have successfully added Menu Role List.";
            return RedirectToAction("List");
           
        }
        //public ActionResult Delete(int id = 0)
        //{
        //    MenuAccessLevel menuAccessLevel = entity.MenuAccessLevels.Find(id);
        //    if (menuAccessLevel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(menuAccessLevel);
           
        //}
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MenuAccessLevel menuAccessLevel = entity.MenuAccessLevels.Find(id);
          //  if (menuAccessLevel == null)
                entity.MenuAccessLevels.Remove(menuAccessLevel);
            entity.SaveChanges();
            ViewBag.SuccessMsg = "You have successfully added Menu Role List.";
            return RedirectToAction("List");
        }

        public JsonResult GetSubmenuByParentId(int ParentId)
        {
            var submenus = (from d in entity.Menus where d.ParentID == ParentId select d).ToList();
            //var lstAcJournalDetails = DAL.GetAcJournalDetails(ParentId);
            return Json(submenus, JsonRequestBehavior.AllowGet);
        }

    }
}
