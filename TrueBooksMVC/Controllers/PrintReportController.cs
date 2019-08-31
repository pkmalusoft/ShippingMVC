using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrueBooksMVC.Controllers
{
    [Authorize]
    public class PrintReportController : Controller
    {
        //
        // GET: /PrintReport/

        public ActionResult PrintReport( )
        {
            return View();
        }

    }
}
