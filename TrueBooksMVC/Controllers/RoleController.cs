using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using TrueBooksMVC.Models;

namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
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
                             where t.ParentID == 0
                             select new MenuRoleVM

                             {
                                 Name = t1.RoleName,
                                 Title = t2.Title,
                                 MenuAccessID = t.MenuAccessID,
                                 MenuID = t.MenuID,
                                 RoleId = t.RoleID

                                 //    MenuAccessID = Convert.ToInt32(t1.Id),

                             }).ToList();


            ViewBag.Roles = entity.RoleMasters;
            return View(query);
        }

        public ActionResult Index()
        {
            var query = from t in entity.Menus
                        where t.ParentID == null && t.IsAccountMenu==false
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
            obj.IsView = true;
            obj.IsAdd = true;
            obj.IsDelete = true;
            obj.IsModify = true;
            obj.Isprint = true;

            entity.MenuAccessLevels.Add(obj);
            entity.SaveChanges();

            var ChildMenus = entity.Menus.Where(d => d.ParentID == model.MenuID && d.IsAccountMenu == false).ToList();
            foreach (var item in ChildMenus)
            {
                var data = entity.MenuAccessLevels.Where(d => d.MenuID == item.MenuID && d.RoleID == model.RoleId).FirstOrDefault();
                if (data == null)
                {
                    data = new MenuAccessLevel();
                    data.MenuID = item.MenuID;
                    data.ParentID = item.ParentID;
                    data.RoleID = model.RoleId;
                    data.CreatedOn = DateTime.Now;
                    data.CreatedBy = Convert.ToString(Session["UserName"]);
                    data.IsActive = 1;
                    data.ModifiedBy = Convert.ToString(Session["UserName"]);
                    data.ModifiedOn = DateTime.Now;
                    data.IsView = true;
                    data.IsAdd = true;
                    data.IsDelete = true;
                    data.IsModify = true;
                    data.Isprint = true;
                    entity.MenuAccessLevels.Add(data);
                    entity.SaveChanges();


                    var secondchilds = entity.Menus.Where(d => d.ParentID == item.MenuID && d.IsAccountMenu == false).ToList();

                    foreach (var childs in secondchilds)
                    {
                        var data1 = entity.MenuAccessLevels.Where(d => d.MenuID == childs.MenuID && d.RoleID == model.RoleId).FirstOrDefault();
                        if (data1 == null)
                        {
                            data1 = new MenuAccessLevel();
                            data1.MenuID = childs.MenuID;
                            data1.ParentID = childs.ParentID;
                            data1.RoleID = model.RoleId;
                            data1.CreatedOn = DateTime.Now;
                            data1.CreatedBy = Convert.ToString(Session["UserName"]);
                            data1.IsActive = 1;
                            data1.ModifiedBy = Convert.ToString(Session["UserName"]);
                            data1.ModifiedOn = DateTime.Now;
                            data1.IsView = true;
                            data1.IsAdd = true;
                            data1.IsDelete = true;
                            data1.IsModify = true;
                            data1.Isprint = true;
                            entity.MenuAccessLevels.Add(data1);
                            entity.SaveChanges();
                        }
                    }

                }
            }
            ViewBag.SuccessMsg = "You have successfully added Menu Role List.";
            return RedirectToAction("List");
        }

        public ActionResult Edit(int id = 0)
        {


            MenuRoleVM objViewmodel = (from t in entity.MenuAccessLevels
                                       where t.MenuAccessID == id
                                       select new MenuRoleVM

                                       {
                                           RoleId = t.RoleID,
                                           MenuID = t.MenuID,
                                           MenuAccessID = t.MenuAccessID

                                           //    MenuAccessID = Convert.ToInt32(t1.Id),

                                       }).FirstOrDefault();


            MenuAccessLevel obj = entity.MenuAccessLevels.Find(id);
            ViewBag.Menu = new SelectList(entity.Menus.Where(d=>d.IsAccountMenu==false).ToList(), "MenuID", "Title");
            ViewBag.Role = new SelectList(entity.RoleMasters, "RoleID", "RoleName");
            return View(objViewmodel);
        }

        [HttpPost]
        public ActionResult Edit(MenuRoleVM model, int id = 0)
        {

            MenuAccessLevel obj = entity.MenuAccessLevels.Find(id);
            obj.RoleID = model.RoleId;
            obj.MenuID = model.MenuID;
            obj.IsView = true;
            obj.IsAdd = true;
            obj.IsDelete = true;
            obj.IsModify = true;
            obj.Isprint = true;
            obj.ModifiedBy = Convert.ToString(Session["UserName"]);
            obj.ModifiedOn = DateTime.Now;


            //entity.Entry(obj).State = System.Data.EntityState.Modified;
            entity.SaveChanges();
            var ChildMenus = entity.Menus.Where(d => d.ParentID == model.MenuID && d.IsAccountMenu == false).ToList();
            foreach (var item in ChildMenus)
            {
                var data = entity.MenuAccessLevels.Where(d => d.MenuID == item.MenuID && d.RoleID == model.RoleId).FirstOrDefault();
                if (data == null)
                {
                    data = new MenuAccessLevel();
                    data.MenuID = item.MenuID;
                    data.ParentID = item.ParentID;
                    data.RoleID = model.RoleId;
                    data.CreatedOn = DateTime.Now;
                    data.CreatedBy = Convert.ToString(Session["UserName"]);
                    data.IsActive = 1;
                    data.ModifiedBy = Convert.ToString(Session["UserName"]);
                    data.ModifiedOn = DateTime.Now;
                    data.IsView = true;
                    data.IsAdd = true;
                    data.IsDelete = true;
                    data.IsModify = true;
                    data.Isprint = true;
                    entity.MenuAccessLevels.Add(data);
                    entity.SaveChanges();


                    var secondchilds = entity.Menus.Where(d => d.ParentID == item.MenuID && d.IsAccountMenu==false).ToList();

                    foreach (var childs in secondchilds)
                    {
                        var data1 = entity.MenuAccessLevels.Where(d => d.MenuID == childs.MenuID && d.RoleID == model.RoleId).FirstOrDefault();
                        if (data1 == null)
                        {
                            data1 = new MenuAccessLevel();
                            data1.MenuID = childs.MenuID;
                            data1.ParentID = childs.ParentID;
                            data1.RoleID = model.RoleId;
                            data1.CreatedOn = DateTime.Now;
                            data1.CreatedBy = Convert.ToString(Session["UserName"]);
                            data1.IsActive = 1;
                            data1.ModifiedBy = Convert.ToString(Session["UserName"]);
                            data1.ModifiedOn = DateTime.Now;
                            data1.IsView = true;
                            data1.IsAdd = true;
                            data1.IsDelete = true;
                            data1.IsModify = true;
                            data1.Isprint = true;
                            entity.MenuAccessLevels.Add(data1);
                            entity.SaveChanges();
                        }
                    }

                }              
            }

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
            var ChildMenus = entity.MenuAccessLevels.Where(d => d.ParentID == menuAccessLevel.MenuID && d.RoleID==menuAccessLevel.RoleID).ToList();
            foreach (var item in ChildMenus)
            {
                var secChilds= entity.MenuAccessLevels.Where(d => d.ParentID == item.MenuID && d.RoleID == menuAccessLevel.RoleID).ToList();
                foreach (var data in secChilds)
                {
                    entity.MenuAccessLevels.Remove(data);
                    entity.SaveChanges();
                }
                entity.MenuAccessLevels.Remove(item);
                entity.SaveChanges();

            }
                ViewBag.SuccessMsg = "You have successfully added Menu Role List.";
            return RedirectToAction("List");
        }

        public JsonResult GetSubmenuByParentId(int ParentId,int RoleId)
        {
            var submenus = (from d in entity.MenuAccessLevels
                            join s in entity.Menus on d.MenuID equals s.MenuID 
                            where d.ParentID == ParentId && d.RoleID== RoleId
                            select new { menu=s,access=d }).ToList();
            //var lstAcJournalDetails = DAL.GetAcJournalDetails(ParentId);
            return Json(submenus, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubmitMenuAccess(string selected)
        {
            var strarr = selected.Split(',');
            foreach(var item in strarr)
            {
                var data = item.Split('_');
                var menuaccess = entity.MenuAccessLevels.Find(Convert.ToInt32(data[1]));              
                if (data[0] == "Add")
                {
                    menuaccess.IsAdd =Convert.ToBoolean(data[2]);
                }
                else if (data[0] == "View")
                {
                    menuaccess.IsView = Convert.ToBoolean(data[2]);
                }
                else if (data[0] == "Modify")
                {
                    menuaccess.IsModify = Convert.ToBoolean(data[2]);
                }
                else if (data[0] == "Delete")
                {
                    menuaccess.IsDelete = Convert.ToBoolean(data[2]);
                }
                else if (data[0] == "Print")
                {
                    menuaccess.Isprint = Convert.ToBoolean(data[2]);
                }
                menuaccess.ModifiedBy = Convert.ToString(Session["UserName"]);
                menuaccess.ModifiedOn = DateTime.Now;
                entity.SaveChanges();
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubmitMenuAccessPermission(int MenuAccessId,string Permission,bool value)
        {
            var data = entity.MenuAccessLevels.Find(MenuAccessId);
            if(Permission=="Add")
            {
                data.IsAdd = value;
            }
            else if (Permission == "View")
            {
                data.IsView = value;
            }
            else if (Permission == "Modify")
            {
                data.IsModify = value;
            }
            else if (Permission == "Delete")
            {
                data.IsDelete = value;
            }
            else if (Permission == "Print")
            {
                data.Isprint = value;
            }
            data.ModifiedBy = Convert.ToString(Session["UserName"]);
            data.ModifiedOn = DateTime.Now;
            entity.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}
