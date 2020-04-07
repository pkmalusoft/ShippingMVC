using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;
using DAL;
using System.Data;

namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class RemarksController : Controller
    {
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        public ActionResult Index()
        {
            var data = db.Remarks.ToList();
            List<RemarksVM> lst = new List<RemarksVM>();

            foreach (var item in data)
            {
                RemarksVM obj = new RemarksVM();

                obj.RemarksID = item.RemarksID;
                obj.Remarks = item.Remarks;
                lst.Add(obj);
            }
            return View(lst);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RemarksVM item)
        {
            Remark obj = new Remark();
            obj.RemarksID = item.RemarksID;
            obj.Remarks = item.Remarks;

            db.Remarks.Add(obj);
            db.SaveChanges();
            TempData["SuccessMsg"] = "You have successfully added Remarks.";
            return RedirectToAction("Index");
        }



        public ActionResult Edit(int id)
        {
            RemarksVM obj = new RemarksVM();
            var data = (from c in db.Remarks where c.RemarksID == id select c).FirstOrDefault();
            if (data == null)
            {
                return HttpNotFound();
            }
            else
            {
                obj.RemarksID = data.RemarksID;
                obj.Remarks = data.Remarks;

            }
            return View(obj);
        }




        [HttpPost]
        public ActionResult Edit(RemarksVM data)
        {
            Remark obj = new Remark();
            obj.RemarksID = data.RemarksID;
            obj.Remarks = data.Remarks;



            db.Entry(obj).State = EntityState.Modified;
            db.SaveChanges();
            TempData["SuccessMsg"] = "You have successfully Updated Remarks.";
            return RedirectToAction("Index");
        }


        public ActionResult DeleteConfirmed(int id)
        {
            Remark re = db.Remarks.Find(id);
            db.Remarks.Remove(re);
            db.SaveChanges();
            TempData["SuccessMsg"] = "You have successfully Delete Remarks.";
            return RedirectToAction("Index");
        }

    }
}
