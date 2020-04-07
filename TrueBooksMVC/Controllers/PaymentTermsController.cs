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
    public class PaymentTermsController : Controller
    {
 
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();

        public ActionResult Index()
        {
            var data = db.PaymentTerms.ToList();
            List<PaymentTermsVM> lst = new List<PaymentTermsVM>();

            foreach (var item in data)
            {
                PaymentTermsVM obj = new PaymentTermsVM();

                obj.PaymentTermID = item.PaymentTermID;
                obj.PaymentTerm = item.PaymentTerm1;
                lst.Add(obj);
            }

            return View(lst);
        }


        public ActionResult Create()
        {
            return View();
        }

         [HttpPost]
        public ActionResult Create(PaymentTermsVM data)
        {
            PaymentTerm obj = new PaymentTerm();

            obj.PaymentTermID = data.PaymentTermID;
            obj.PaymentTerm1 = data.PaymentTerm;

            db.PaymentTerms.Add(obj);
            db.SaveChanges();
            TempData["SuccessMsg"] = "You have successfully Added Payment Term.";


            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            PaymentTermsVM obj = new PaymentTermsVM();
            var data = (from c in db.PaymentTerms where c.PaymentTermID == id select c).FirstOrDefault();
            if (data == null)
            {
                return HttpNotFound();
            }
            else
            {
                obj.PaymentTermID = data.PaymentTermID;
                obj.PaymentTerm = data.PaymentTerm1;


            }
            return View(obj);
        }

    


        [HttpPost]
        public ActionResult Edit(PaymentTermsVM data)
        {
            PaymentTerm obj = new PaymentTerm();
            obj.PaymentTermID = data.PaymentTermID;
            obj.PaymentTerm1 = data.PaymentTerm;

        

            db.Entry(obj).State = EntityState.Modified;
            db.SaveChanges();
            TempData["SuccessMsg"] = "You have successfully Updated Payment Term.";

            return RedirectToAction("Index");
        }


        public ActionResult DeleteConfirmed(int id)
        {
            PaymentTerm pay = db.PaymentTerms.Find(id);
            db.PaymentTerms.Remove(pay);
            db.SaveChanges();
            TempData["SuccessMsg"] = "You have successfully Deleted Payment Terms.";
            return RedirectToAction("Index");
        }
    }
}
