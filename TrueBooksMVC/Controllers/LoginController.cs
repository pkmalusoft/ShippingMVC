using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;
using DAL;
using System.Web.Security;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
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
            //var version = typeof(Controller).Assembly.GetName().Version;
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
            //else if (UR.BranchID <= 0)
            //    status = false;
            //else if (UR.AcFinancialYearID <= 0)
            //    status = false;



            if (status)
            {

                List<SP_LoginUser_Result> Log = new List<SP_LoginUser_Result>();

                Log = rgm.LoginUser(UR);
                var userlogin = entity.UserRegistrations.Where(d => d.EmailId == UR.UserName && d.Password == UR.Password).ToList();
                if (userlogin.Count > 0)
                {
                    foreach (var item in userlogin)
                    {
                        FormsAuthentication.SetAuthCookie(item.UserName, true);
                        var User_Registration = (from t in entity.UserRegistrations
                                     where t.UserID == item.UserID select t
                                     ).FirstOrDefault();
                        if(User_Registration.BranchID==0 || User_Registration.BranchID==null)
                        {
                            User_Registration.BranchID = entity.BranchMasters.FirstOrDefault().BranchID;
                        }
                        var acfinancialyearid = entity.AcFinancialYears.Where(d => d.BranchID == User_Registration.BranchID).FirstOrDefault().AcFinancialYearID;
                       
                        int? BranchCurrencyId = (from e in entity.BranchMasters where e.BranchID == User_Registration.BranchID select e.CurrencyID).FirstOrDefault();
                        var basecurrency = (from t in entity.CurrencyMasters where t.CurrencyID == BranchCurrencyId select t).FirstOrDefault();
                        Session["BaseCurrencyId"] = BranchCurrencyId;
                        Session["BaseCurrency"] = basecurrency.CurrencyName;
                        Session["BaseCurrencySymbol"] = basecurrency.Symbol;
                        Session["UserID"] = item.UserID;
                        Session["UserName"] = item.UserName;
                        Session["branchid"] = User_Registration.BranchID;
                        var AccompanyId = (from c in entity.AcCompanies select c.AcCompanyID).FirstOrDefault();
                        Session["AcCompanyID"] = AccompanyId;
                        Session["fyearid"] = acfinancialyearid;
                        SourceMastersModel objSourceMastersModel = new SourceMastersModel();
                        Session["Company"] = objSourceMastersModel.GetAcCompaniesById(AccompanyId).AcCompany1;
                        Session["BranchName"] = entity.BranchMasters.Find(User_Registration.BranchID).BranchName;
                        var fyearFrom = (from t in entity.AcFinancialYears where t.AcFinancialYearID == acfinancialyearid select t.AcFYearFrom).FirstOrDefault();
                        Session["FyearFrom"] = fyearFrom;
                        var fyearTo = (from t in entity.AcFinancialYears where t.AcFinancialYearID == acfinancialyearid select t.AcFYearTo).FirstOrDefault();

                        Session["FyearTo"] = fyearTo;
                        var query = (from t in entity.UserRegistrations
                                     where t.UserID == item.UserID && t.RoleID.HasValue
                                     select t.RoleID.Value).ToList();
                        var UserRoleID = (from t in entity.UserRegistrations
                                     where t.UserID == item.UserID select t.RoleID).FirstOrDefault();
                        Session["UserRoleID"] = UserRoleID;
                        if (query != null)
                        {
                            Session["RoleID"] = query;
                            if (query[0] == 1)
                            {
                                var menuaccesslevels = new List<MenuAccessLevel>();
                                var menus = (from t in entity.Menus where t.IsAccountMenu.Value == true orderby t.MenuOrder select t).ToList();

                                Session["Menu"] = menus;
                            }
                            else
                            {
                                Int32 roleid = query[0];
                                var menudata = (from t in entity.MenuAccessLevels
                                                join s in entity.Menus on t.MenuID equals s.MenuID
                                                where t.RoleID == roleid && t.IsView == true
                                                orderby s.MenuOrder
                                                select s).ToList();
                                Session["menu"] = menudata;
                            }
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

            return RedirectToAction("Index");
        }

        public ActionResult Signout()
        {

            Session.Abandon();

            // @ViewBag.SignOut = "You have successfully signout.";
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
        public ActionResult GetChangeFyear(int FyearId)
        {
            Session["fyearid"] = FyearId.ToString();
            int fid = Convert.ToInt32(Session["fyearid"]);
            var fyearFrom = (from t in entity.AcFinancialYears where t.AcFinancialYearID == fid select t.AcFYearFrom).FirstOrDefault();

            Session["FyearFrom"] = fyearFrom;

            var fyearTo = (from t in entity.AcFinancialYears where t.AcFinancialYearID == fid select t.AcFYearTo).FirstOrDefault();

            Session["FyearTo"] = fyearTo;
            return RedirectToAction("Index", "Login");
            // return Json(new { Url = "Home/Home" });

        }

        public ActionResult Index()
        {
            var compdetail = entity.AcCompanies.FirstOrDefault();
            ViewBag.CompanyName = compdetail.AcCompany1;
            //var version = typeof(Controller).Assembly.GetName().Version;
            ViewBag.Branch = entity.BranchMasters.ToList();

            ViewBag.fyears = entity.AcFinancialYearSelect(Convert.ToInt32(Session["branchid"])).ToList();
            // Session["fyearid"] = 1;

           
            return View();
        }
        public ActionResult ForgotPassword()
        {
            var compdetail = entity.AcCompanies.FirstOrDefault();
            ViewBag.CompanyName = compdetail.AcCompany1;
            return View();
        }

        public ActionResult ChangePassword()
        {
            var compdetail = entity.AcCompanies.FirstOrDefault();
            ViewBag.CompanyName = compdetail.AcCompany1;
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(UserLoginVM vm)
        {
            string emailid = vm.UserName;
            var _user = entity.UserRegistrations.Where(cc => cc.EmailId == emailid).FirstOrDefault();
            if (_user != null)
            {
                string newpassword = RandomPassword(6);

                _user.Password = newpassword;
                entity.Entry(_user).State = EntityState.Modified;
                entity.SaveChanges();
                EmailDAO _emaildao = new EmailDAO();
                _emaildao.SendForgotMail(_user.EmailId, "User", newpassword);
                TempData["SuccessMsg"] = "Reset Password Details are sent,Check Email!";

                return RedirectToAction("Index", "Login");
                //return Json(new { status = "ok", message = "Reset Password Details are sent,Check Email" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Session["ForgotStatus"] = "Forgot";
                Session["StatusMessage"] = "Invalid EmailId!";
                return RedirectToAction("Index", "Login");
                //return Json(new { status = "Failed", message = "Invalid EmailId!" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult ChangePassword(UserLoginVM vm)
        {
            string emailid = vm.UserName;
            var _user = entity.UserRegistrations.Where(cc => cc.EmailId == emailid && cc.Password == vm.Password).FirstOrDefault();
            if (_user != null)
            {

                _user.Password = vm.NewPassword;
                entity.Entry(_user).State = EntityState.Modified;
                entity.SaveChanges();
                EmailDAO _emaildao = new EmailDAO();
                _emaildao.SendForgotMail(_user.EmailId, "User", vm.NewPassword);
                TempData["SuccessMsg"] = "Password Changed Successfully!";
                return RedirectToAction("Index", "Login");
                //return Json(new { status = "ok", message = "Reset Password Details are sent,Check Email" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //TempData["ErrorMsg"] = "Invalid EmailId or Password!";
                Session["ResetStatus"] = "Reset";
                Session["StatusMessage"] = "Invalid Credential!";
                return RedirectToAction("Index", "Login");
                //return Json(new { status = "Failed", message = "Invalid EmailId!" }, JsonRequestBehavior.AllowGet);
            }

        }
        public string RandomPassword(int size = 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

    }
}
