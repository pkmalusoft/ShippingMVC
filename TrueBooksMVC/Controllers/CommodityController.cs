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
    public class CommodityController : Controller
    {
        SHIPPING_FinalEntities context = new SHIPPING_FinalEntities();


        public ActionResult Index()
        {
            return View(context.Commodities.OrderBy(x => x.Commodity1).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Commodity u)
        {
            context.Commodities.Add(u);
            context.SaveChanges();

            TempData["SuccessMSG"] = "You have successfully added Commodity.";
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
            var d = context.Commodities.Find(id);
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
        public ActionResult Edit(Commodity u)
        {
            if (ModelState.IsValid)
            {
                context.Entry(u).State = EntityState.Modified;
                context.SaveChanges();
                TempData["SuccessMSG"] = "You have successfully updated Commodity.";
                return RedirectToAction("Index");
            }
            return View();


        }

        public ActionResult DeleteConfirmed(int id)
        {
            var d = context.Commodities.Find(id);
            if (d == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Commodities.Remove(d);
                context.SaveChanges();
                TempData["SuccessMSG"] = "You have successfully deleted Commodity.";
                return RedirectToAction("Index");
            }
        }


    }
}
