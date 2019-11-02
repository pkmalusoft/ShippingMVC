using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueBooksMVC.Models;
using System.Data.SqlClient;
using System.Data;
using DAL;

namespace TrueBooksMVC.Models
{
    public class ProductServiceModel 
    {
        // GET: ProductServiceModel
        public ActionResult Index()
        {
            return View();
        }
    }
}