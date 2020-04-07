using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;

namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class QuotationvalidityController : Controller
    {
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();

        public ActionResult Index()
        {
           



            return View(db.Validities.ToList());
            
        }

        public ActionResult Details(int id = 0)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        //
        // GET: /Department/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Department/Create

        [HttpPost]
        public ActionResult Create(ValidityVM v)
        {
            if (ModelState.IsValid)
            {

                Validity d = new Validity();
                int vid = db.Validities.OrderByDescending(x => x.ValidityID).Select(x => x.ValidityID).FirstOrDefault();
                vid = vid + 1;

                d.Validity1 = v.Validity;
                d.ValidityID = vid;
              

                db.Validities.Add(d);
                db.SaveChanges();
                TempData["SuccessMsg"] = "You have successfully added Validity.";
                return RedirectToAction("Index");
            }

            return View();
        }

        //
        // GET: /Department/Edit/5

        public ActionResult Edit(int id)
        {

            ValidityVM c = new ValidityVM();
            var data = (from d in db.Validities where d.ValidityID == id select d).FirstOrDefault();

            if (data == null)
            {
                return HttpNotFound();
            }
            else
            {
                c.ValidityID = data.ValidityID;
                c.Validity = data.Validity1;
                

            }



            return View(c);
        }

        //
        // POST: /Department/Edit/5

        [HttpPost]
        public ActionResult Edit(ValidityVM v)
        {
            if (ModelState.IsValid)
            {
                Validity c = new Validity();
                c.ValidityID = v.ValidityID;
                c.Validity1 = v.Validity;
               
                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMsg"] = "You have successfully Upadated Validity.";
                return RedirectToAction("Index");
            }
            return View();
        }




        public ActionResult DeleteConfirmed(int id)
        {
            Validity Validity = db.Validities.Find(id);
            db.Validities.Remove(Validity);
            db.SaveChanges();
            TempData["SuccessMsg"] = "You have successfully Deleted Validity.";
            return RedirectToAction("Index");
        }


    }
}
