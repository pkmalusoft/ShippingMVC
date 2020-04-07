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
    public class Report_TredingLossAndProfitAccountController : Controller
    {
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        
        //
        // GET: /Report_TredingLossAndProfitAccount/

        public ActionResult TredingLossAndProfitAcoount()
        {
            return View();
        }

        public LargeJsonResult GetTredinglossAndProfitData(DateTime tdate)
        {
            string view = "";
            var data = db.Report_TradingProfitAndLoss(Convert.ToDateTime( Session["FyearFrom"]), tdate).ToList();
            view = this.RenderPartialView("ucTredinlossandProfitPartial", data);
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
