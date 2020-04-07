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
    public class AirlineController : Controller
    {
        SHIPPING_FinalEntities context = new SHIPPING_FinalEntities();


        public ActionResult Index()
        {
            return View(context.Airlines.OrderBy(x => x.Airline1).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Airline u)
        {
            context.Airlines.Add(u);
            context.SaveChanges();

            TempData["SuccessMSG"] = "You have successfully added Airline.";
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
            var d = context.Airlines.Find(id);
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
        public ActionResult Edit(Airline u)
        {
            if (ModelState.IsValid)
            {
                context.Entry(u).State = EntityState.Modified;
                context.SaveChanges();
                TempData["SuccessMSG"] = "You have successfully updated Airline.";
                return RedirectToAction("Index");
            }
            return View();


        }

        public ActionResult DeleteConfirmed(int id)
        {
            var d = context.Airlines.Find(id);
            if (d == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Airlines.Remove(d);
                context.SaveChanges();
                TempData["SuccessMSG"] = "You have successfully deleted Airline.";
                return RedirectToAction("Index");
            }
        }

    }
}
