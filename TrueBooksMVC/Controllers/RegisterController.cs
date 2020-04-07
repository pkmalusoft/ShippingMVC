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
    public class RegisterController : Controller
    {
        //
        // GET: /Register/
        RegistrationModel rgm = new RegistrationModel();

        [HttpGet]
        public ActionResult RegisterDetails()
        {
            List<SP_GetUserDetails_Result> UR = new List<SP_GetUserDetails_Result>();

            UR = rgm.GetUserDetails();

            return View(UR);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
           
            if (id != 0)
            {
                rgm.DeleteUser(id);
            }
                    
            return RedirectToAction("RegisterDetails", "Register");
           
        }

        [HttpGet]
        public ActionResult Register(int id)
        {
            UserRegistration UR = new UserRegistration();

            var timeZones = TimeZoneInfo.GetSystemTimeZones();

            foreach (var timeZone in timeZones)
            {
                string Name = timeZone.DisplayName;
                string dln = timeZone.DaylightName;
                TimeSpan dt = timeZone.BaseUtcOffset;
                string dn = timeZone.Id;
            }

            DateTime serverDateTime = DateTime.Now;

            DateTime dbDateTime = serverDateTime.ToUniversalTime();

            DateTimeOffset dbDateTimeOffset = new DateTimeOffset(dbDateTime, TimeSpan.Zero);

            TimeZoneInfo userTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

            DateTimeOffset userDateTimeOffset =TimeZoneInfo.ConvertTime(dbDateTimeOffset, userTimeZone);

            string userDateTimeString = userDateTimeOffset.ToString("dd MMM yyyy - HH:mm:ss (zzz)");

            UR = rgm.GetUsetDetailByID(id);

            return View(UR);
        }

        [HttpPost]
        public ActionResult Register(UserRegistration UR)
        {
            if (UR.UserID != 0)
            {
                rgm.EditUser(UR);
            }
            else
            {
                rgm.AddUser(UR);

                //return RedirectToAction("Login", "Login");
            }

            return RedirectToAction("Login", "Login");
        }



    }
}
