﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using TrueBooksMVC.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Text;
using System.IO;
namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    public class UserRegistrationController : Controller
    {
        private SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        SourceMastersModel objectSourceMaster = new SourceMastersModel();

        //
        // GET: /UserRegistration/

        public ActionResult Index()
        {
            List<UserRegistrationRoleVM> userRoleObject = new List<UserRegistrationRoleVM>();
            var query = (from t in db.UserRegistrations
                         join t1 in db.RoleMasters
                         on t.RoleID equals t1.RoleID
                         select new UserRegistrationRoleVM
                         {
                             RoleName = t1.RoleName,
                             UserName = t.UserName,
                             phone = t.Phone,
                             Email = t.EmailId,
                             IsActive = t.IsActive,
                             UserID = t.UserID

                         }).ToList();


            return View(query);
        }

        //
        // GET: /UserRegistration/Details/5

        public ActionResult Details(int id = 0)
        {
            UserRegistration userregistration = db.UserRegistrations.Find(id);
            if (userregistration == null)
            {
                return HttpNotFound();
            }
            return View(userregistration);
        }

        //
        // GET: /UserRegistration/Create

        public ActionResult Create()
        {
            ViewBag.UserRole = db.RoleMasters.ToList();
            ViewBag.Employee = db.Employees.ToList();
            ViewBag.Branch = db.BranchMasters.ToList();
            return View();
        }

        //
        // POST: /UserRegistration/Create

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult Create(UserRegistration userregistration,HttpPostedFileBase Image)
        {
            ViewBag.UserRole = db.RoleMasters.ToList();
            ViewBag.Branch = db.BranchMasters.ToList();

            var data = db.UserRegistrations.Where(p => p.UserName.Equals(userregistration.UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (data == null)
            {
                userregistration.UserID = objectSourceMaster.GetMaxNumberRegistration();
                foreach (string item in Request.Files)
                {
                    var fileContent = Request.Files[item];
                    if (fileContent != null && fileContent.FileName != null)
                    {
                        if (item == "Image")
                        {
                            var image = ImagetoBase64(fileContent);


                            userregistration.Image = image;

                        }

                    }
                }
                db.UserRegistrations.Add(userregistration);
                db.SaveChanges();
                TempData["SuccessMSG"] = "You have successfully added User.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["SuccessMSG"] = "Sorry, this name already exists ";
                return RedirectToAction("Create", userregistration);
            }


        }

        //
        // GET: /UserRegistration/Edit/5
        public string ImagetoBase64(HttpPostedFileBase Path)
        {
            string theFileName = Path.FileName;
            byte[] thePictureAsBytes = new byte[Path.ContentLength];
            using (BinaryReader theReader = new BinaryReader(Path.InputStream))
            {
                thePictureAsBytes = theReader.ReadBytes(Path.ContentLength);
            }
            string thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);
            return thePictureDataAsString;
        }
        public ActionResult Edit(int id = 0)
        {
            ViewBag.UserRole = db.RoleMasters.ToList();
            ViewBag.Employee = db.Employees.ToList();
            ViewBag.Branch = db.BranchMasters.ToList();

            UserRegistration userRegistration = objectSourceMaster.GetUserRegistrationById(id);
            //UserRegistration userregistration = db.UserRegistrations.Find(id);
            if (userRegistration == null)
            {
                return HttpNotFound();
            }
            return View(userRegistration);
        }

        //
        // POST: /UserRegistration/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserRegistration userregistration, HttpPostedFileBase Image)
        {
            ViewBag.Branch = db.BranchMasters.ToList();

            if (ModelState.IsValid)
            {
                foreach (string item in Request.Files)
                {
                    var fileContent = Request.Files[item];
                    if (fileContent != null && fileContent.FileName != null)
                    {
                        if (item == "Image")
                        {
                            var image = ImagetoBase64(fileContent);


                            userregistration.Image = image;

                        }

                    }
                }
                db.Entry(userregistration).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMSG"] = "You have successfully updated User.";
                return RedirectToAction("Index");
            }
            return View(userregistration);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordVM changepass)
        {
            string uname = Session["UserName"].ToString();
            UserRegistration u = (from user in db.UserRegistrations where (user.UserName == uname) && (user.Password == changepass.Password) select user).FirstOrDefault();
            if (u == null)
            {
                TempData["message"] = "Previous password is Not Valid";
                return RedirectToAction("ChangePassword");
            }
            else if (changepass.NewPassword != changepass.ConfirmPassword)
            {
                TempData["message"] = "New Password Do not Match";
                return RedirectToAction("ChangePassword");
            }
            else
            {
                u.Password = changepass.NewPassword;
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Signout", "Login");

            }



        }

        //
        // GET: /UserRegistration/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    UserRegistration userregistration = db.UserRegistrations.Find(id);
        //    if (userregistration == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userregistration);
        //}

        //
        // POST: /UserRegistration/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserRegistration userregistration = db.UserRegistrations.Find(id);
            db.UserRegistrations.Remove(userregistration);
            db.SaveChanges();
            TempData["SuccessMSG"] = "You have successfully deleted User.";
            return RedirectToAction("Index");
        }



        //public string CheckForDuplication(string UserName)
        //{
        //    var data = db.UserRegistrations.Where(p => p.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

        //    if (data.UserName == null)
        //    {
        //        return "";
        //    }
        //    else
        //    {
        //        return data.UserName;
        //    }


        //    //if (data != null)
        //    //{

        //    //    return Json("Sorry, this name already exists", JsonRequestBehavior.AllowGet);
        //    //}
        //    //else
        //    //{
        //    //    return Json(true, JsonRequestBehavior.AllowGet);
        //    //}
        //} public JsonResult GetEmployeeByid(int Id)
        public JsonResult GetEmployeeByid(int Id)
        {
            var data = db.Employees.Find(Id);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GeneratePassword()
        {
            var length = 8;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            var password = res.ToString();
            return Json(password, JsonRequestBehavior.AllowGet);
        }


    }
}