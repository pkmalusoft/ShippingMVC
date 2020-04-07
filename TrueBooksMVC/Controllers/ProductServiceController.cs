using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;

namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class ProductServiceController : Controller
    {
        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();


        // GET: ProductService
        public ActionResult Index()
        {
            var product = objSourceMastersModel.GetProduct();
            return View(product);
        }

        //
        // GET: / ProductService/Details/5

        public ActionResult Details(int id = 0)
        {
            ProductService product = objSourceMastersModel.GetProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /ProductService/Create

        public ActionResult Create()
        {
            return View();
        }


        //
        // POST: /ProductService/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductService productservice)
        {
            if (ModelState.IsValid)
            {

                var query = (from t in db.ProductServices where t.ProductName == productservice.ProductName select t).ToList();

                if (query.Count > 0)
                {
                    ViewBag.SuccessMsg = "Product name is already exist";
                    return View();
                }

                if (objSourceMastersModel.SaveProduct(productservice))
                {
                    ViewBag.SuccessMsg = "You have successfully added Product.";
                    return View("Index", objSourceMastersModel.GetProduct());
                }
                else
                {
                    return View(productservice);
                }
            }

            return View(productservice);


        }

        //
        // GET: /Country/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ProductService product = objSourceMastersModel.GetProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        //
        // POST: /Country/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductService productservice)
        {
            if (ModelState.IsValid)
            {
                if (productservice.TaxPercent == null)
                {
                    productservice.TaxPercent = 0;
                }
                objSourceMastersModel.SaveProductById(productservice);
                ViewBag.SuccessMsg = "You have successfully updated Product.";
                return View("Index", objSourceMastersModel.GetProduct());
            }
            return View(productservice);
        }

        //
        // GET: /Country/Delete/5

        //public ActionResult Delete(int id = 0)
        //{

        //    CountryMaster country = objSourceMastersModel.GetCountryById(id);
        //    if (country == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(country);
        //}

        //
        // POST: /Country/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            objSourceMastersModel.DeleteProduct(id);
            ViewBag.SuccessMsg = "You have successfully deleted Country.";
            return View("Index", objSourceMastersModel.GetProduct());
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}







    }
}