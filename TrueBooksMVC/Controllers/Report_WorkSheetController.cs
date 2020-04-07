using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;
using DAL;

namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class Report_WorkSheetController : Controller
    {
        //
        // GET: /Report_WorkSheet/

        public ActionResult WorkSheet()
        {
            return View();
        }
        public LargeJsonResult GetWorksheetData(DateTime frmdate, DateTime tdate)
        {


            string view = "";

            //var data = from t in db.AcJournalMasters select t;
            //view = this.RenderPartialView("TredingAccount_Partial",data);
            return new LargeJsonResult
            {
                Data = new
                {
                    success = true,
                    view = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}
