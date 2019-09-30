using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;
using DAL;
using System.Web.Security;
using System.Data;

namespace TrueBooksMVC.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        RegistrationModel rgm = new RegistrationModel();
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        public ActionResult Login()
        {
            ViewBag.Branch = entity.BranchMasters.ToList();

            ViewBag.fyears = entity.AcFinancialYearSelect(Convert.ToInt32(Session["branchid"])).ToList();
            // Session["fyearid"] = 1;

            if (Session["UserID"] == null)
            {
                if (Request.QueryString["ID"] != null)
                {

                    if (Request.QueryString["ID"] == "1")
                    {
                        ViewBag.Message = "Your session has been expired!. please login again.";
                    }

                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }


        public JsonResult GetFYear(int id)
                {
            var x = entity.AcFinancialYearSelect(id).ToList();

            return Json(x, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllYears()
        {
            int id = Convert.ToInt32(Session["branchid"]);
            var x = entity.AcFinancialYearSelect(id).ToList();

            return Json(x, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Login(UserRegistration UR)
        {
            bool status = true;
            if (string.IsNullOrEmpty(UR.UserName))
                status = false;
            else if (string.IsNullOrEmpty(UR.Password))
                status = false;
            else if (UR.BranchID <= 0)
                  status = false;
              else if (UR.AcFinancialYearID <= 0)
                status = false;



            if (status)
            {

                List<SP_LoginUser_Result> Log = new List<SP_LoginUser_Result>();

                Log = rgm.LoginUser(UR);

                if (Log.Count > 0)
                {
                    foreach (var item in Log)
                    {
                        FormsAuthentication.SetAuthCookie(item.Username, false);
                        int? BranchCurrencyId = (from e in entity.BranchMasters where e.BranchID == UR.BranchID select e.CurrencyID).FirstOrDefault();
                        string basecurrency = (from t in entity.CurrencyMasters where t.CurrencyID == BranchCurrencyId select t.CurrencyName).FirstOrDefault();
                        Session["BaseCurrencyId"] = BranchCurrencyId;
                        Session["BaseCurrency"] = basecurrency;
                        Session["UserID"] = item.UserID;
                        Session["UserName"] = item.Username;
                        Session["branchid"] = UR.BranchID;
                        Session["AcCompanyID"] = (from c in entity.AcCompanies select c.AcCompanyID).FirstOrDefault();
                        Session["fyearid"] = UR.AcFinancialYearID;

                        var fyearFrom = (from t in entity.AcFinancialYears where t.AcFinancialYearID == UR.AcFinancialYearID select t.AcFYearFrom).FirstOrDefault();
                        
                        Session["FyearFrom"] = fyearFrom;
                        var fyearTo = (from t in entity.AcFinancialYears where t.AcFinancialYearID == UR.AcFinancialYearID select t.AcFYearTo).FirstOrDefault();

                        Session["FyearTo"] = fyearTo;
                        var query = (from t in entity.UserRegistrations
                                     where t.UserID == item.UserID && t.RoleID.HasValue
                                     select t.RoleID.Value).ToList();
                        if (query != null)
                        {
                            Session["RoleID"] = query;
                        }
                        else
                        {
                            Session["RoleID"] = new List<int>();
                        }
                    }

                    return RedirectToAction("Home", "Home");
                }
                ViewBag.ErrorMessage = "Invalid Credentials";
                ViewBag.Branch = entity.BranchMasters.ToList();

                ViewBag.fyears = entity.AcFinancialYearSelect(Convert.ToInt32(Session["branchid"])).ToList();
                return View();
            }
            else
            {
                ViewBag.ErrorMessage = "Please fill mandatory fields";
                ViewBag.Branch = entity.BranchMasters.ToList();

                ViewBag.fyears = entity.AcFinancialYearSelect(Convert.ToInt32(Session["branchid"])).ToList();
                return View();
            }

        }

        public ActionResult Logput()
        {

            return RedirectToAction("Login");
        }

        public ActionResult Signout()
        {

            Session.Abandon();

            // @ViewBag.SignOut = "You have successfully signout.";
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult GetChangeFyear(int FyearId)
        {
            Session["fyearid"] = FyearId.ToString();
            int fid =Convert.ToInt32( Session["fyearid"]);
            var fyearFrom = (from t in entity.AcFinancialYears where t.AcFinancialYearID == fid select t.AcFYearFrom).FirstOrDefault();

            Session["FyearFrom"] = fyearFrom;

            var fyearTo = (from t in entity.AcFinancialYears where t.AcFinancialYearID == fid select t.AcFYearTo).FirstOrDefault();

            Session["FyearTo"] = fyearTo;
            return RedirectToAction("Home", "Home");
           // return Json(new { Url = "Home/Home" });

        }


    }
}
