using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrueBooksMVC.Controllers
{
    public class PurchaseInvoiceController : Controller
    {
        //
        // GET: /PurchaseInvoice/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Invoice()
        {
            return View();
        }
    }
}
