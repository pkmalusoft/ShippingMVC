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
    public class UnitController : Controller
    {
        SHIPPING_FinalEntities context = new SHIPPING_FinalEntities();


        public ActionResult Index()
        {
            return View(context.ItemUnits.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ItemUnit u)
        {
            u.StatusActive = true;
            context.ItemUnits.Add(u);
            context.SaveChanges();

            TempData["SuccessMSG"] = "You have successfully added Unit.";
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
            var d = context.ItemUnits.Find(id);
            if (d == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(d);
            }
        }

        [HttpPost]
        public ActionResult Edit(ItemUnit u)
        {
            if (ModelState.IsValid)
            {
                context.Entry(u).State = EntityState.Modified;
                context.SaveChanges();
                TempData["SuccessMSG"] = "You have successfully updated Unit.";
                return RedirectToAction("Index");
            }
            return View();

           
        }

        public ActionResult DeleteConfirmed(int id)
        {
            var d = context.ItemUnits.Find(id);
            if (d == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.ItemUnits.Remove(d);
                context.SaveChanges();
                TempData["SuccessMSG"] = "You have successfully deleted Unit.";
                return RedirectToAction("Index");
            }
        }


    }
}
