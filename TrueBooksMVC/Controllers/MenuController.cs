using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;

namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
     [Authorize]
    public class MenuController : Controller
       
    {
         SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities(); 
        //
        // GET: /Menu/
        
        public ActionResult Index()
        {
            var Query = from t in entity.Menus select t;
            ViewBag.Menu = Query;
            return View();
        }

    }
}
