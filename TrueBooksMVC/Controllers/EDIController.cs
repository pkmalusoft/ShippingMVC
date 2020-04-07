using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using TrueBooksMVC.Models;
using DAL;

namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class EDIController : Controller
    {
        //
        // GET: /EDI/

        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
        EDIModel EM = new EDIModel();
       
        public ActionResult EDI()
        {

            var data = (from t in db.JobGenerations where t.RotationNo != null && t.RotationNo != "" select t).ToList();
            ViewBag.rotationNo = data;
            return View();
        }

        public JsonResult GetRotationNo(string RotationNo)
        {
            Session["RotationNo"] = RotationNo;
            return Json(true,JsonRequestBehavior.AllowGet);
        }
        public FileStreamResult CreateFile()
        {
            string rotationid=Session["RotationNo"].ToString();
            var string_with_your_data = EM.GetVoy(rotationid).ToString();

            var byteArray = Encoding.ASCII.GetBytes(string_with_your_data);
            var stream = new MemoryStream(byteArray);

            

            string path = AppDomain.CurrentDomain.BaseDirectory + "TempFile/";
            return File(stream, "text/plain", "EDI File.txt");
        }
    }

    
}
