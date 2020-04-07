using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;

namespace ShippingFinal.Controllers
{
    [SessionExpire]
    [Authorize]
    public class ShippingAgentController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        //
        // GET: /Designation/

        public ActionResult Index()
        {
            List<ShippingUserVM> model = new List<ShippingUserVM>();
            var query = (from t in entity.ShippingAgents
                         join t1 in entity.CountryMasters on t.CountryID equals t1.CountryID
                         select new ShippingUserVM

                         {
                             AgentName = t.AgentName,
                             ReferenceCode = t.ReferenceCode,
                             ContactPerson = t.ContactPerson,
                             Phone = t.Phone,
                             Email=t.Email,
                             CountryName = t1.CountryName,
                             ShippingAgentID=t.ShippingAgentID

                         }).ToList();
          
            //var shippingAgent = objSourceMastersModel.GetShippingAgent();
            
            return View(query);
        }
        //
        // GET: /ShippingAgent/Details/5

        public ActionResult Details(int id = 0)
        {
            ShippingAgent shippingagent = objSourceMastersModel.GetShippingAgentById(id);
            if (shippingagent == null)
            {
                return HttpNotFound();
            }
            return View(shippingagent);
        }

        //
        // GET: /ShippingAgent/Create

        public ActionResult Create()
        {

            ViewBag.country = DropDownList<CountryMaster>.LoadItems(
                   objSourceMastersModel.GetCountry(), "CountryID", "CountryName");
            
            return View();
        }

        public static class DropDownList<T>
        {
            public static SelectList LoadItems(IList<T> collection, string value, string text)
            {
                return new SelectList(collection, value, text);
            }
        }


        //
        // POST: /ShippingAgent/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShippingAgent shippingagent)
        { 
            
            if (ModelState.IsValid)
            {
                ViewBag.country = DropDownList<CountryMaster>.LoadItems(
                  objSourceMastersModel.GetCountry(), "CountryID", "CountryName");
            
                var query = (from t in entity.ShippingAgents where t.AgentName == shippingagent.AgentName select t).ToList();

                if (query.Count > 0)
                {
                   
                    ViewBag.SuccessMsg = "Agent name is already exist";
                    ViewBag.country = DropDownList<CountryMaster>.LoadItems(
                   objSourceMastersModel.GetCountry(), "CountryID", "CountryName");
                    return View();
                }


                objSourceMastersModel.SaveShippingAgentById(shippingagent);
                TempData["SuccessMSG"] = "You have successfully added Shipping Agent.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["SuccessMSG"] = "Invalid Model State";
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach(var item in errors)
                {
                    TempData["SuccessMSG"] = TempData["SuccessMSG"].ToString() + item.ErrorMessage + ";";
                }
            }
            ViewBag.country = DropDownList<CountryMaster>.LoadItems(
                   objSourceMastersModel.GetCountry(), "CountryID", "CountryName");
            return View(shippingagent);
            }

           
        

        //
        // GET: /ShippingAgent/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ShippingAgent shippingagent = objSourceMastersModel.GetShippingAgentById(id);
            if (shippingagent == null)
            {
                return HttpNotFound();
            }
            ViewBag.country = new SelectList(objSourceMastersModel.GetCountry(), "CountryID", "CountryName", shippingagent.CountryID);
            return View(shippingagent);
        }

        //
        // POST: /ShippingAgent/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShippingAgent shippingagent)
        {
            if (ModelState.IsValid)
            {

                objSourceMastersModel.SaveShippingAgentById(shippingagent);
                TempData["SuccessMSG"] = "You have successfully updated Shipping Agent.";
                return RedirectToAction("Index");
            }

            return View(shippingagent);
        }

        //
        // GET: /ShippingAgent/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    ShippingAgent shippingagent = objSourceMastersModel.GetShippingAgentById(id);
        //    if (shippingagent == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(shippingagent);
        //}

        //
        // POST: /ShippingAgent/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            objSourceMastersModel.DeleteShippingAgent(id);
            TempData["SuccessMSG"] = "You have successfully deleted Shipping Agent.";
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}