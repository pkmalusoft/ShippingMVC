using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;

using TrueBooksMVC.Models;


namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class CustomerInvoiceController : Controller
    {
        //
        // GET: /CustomerInvoice/

        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        public ActionResult Index()
        {
            var query = (from t in entity.JobGenerations where t.InvoiceNo > 0 select t).ToList();
            ViewBag.JobCode = query;//new SelectList(entity.JobGenerations, "JobID", "JobCode");
            ViewBag.Currency = new SelectList(entity.CurrencyMasters, "CurrencyID", "CurrencyName");

            return View();
        }

        public LargeJsonResult GetInvoiceDetails(int jobid, DateTime fromdate,DateTime todate)
        {
            int jobId = Convert.ToInt32(jobid);

            var data = entity.sp_GetCustomerInvoice(fromdate, todate, jobid).ToList();

            var view = this.RenderPartialView("ucCustomerInvoice", data);
            return new LargeJsonResult
            {
                MaxJsonLength = Int32.MaxValue,
                Data = new
                {
                    success = true,
                    view = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };


            //decimal provisionCurrency = (from t in entity.JInvoices where t.JobID == jobId select t.ProvisionExchangeRate.Value).FirstOrDefault();
            //decimal SalesCurrency = (from t in entity.JInvoices where t.JobID == jobId select t.SalesExchangeRate.Value).FirstOrDefault();

            //if (currencyid == 0)
            //{
            //    var query = (from t in entity.JobGenerations
            //                 join t1 in entity.JInvoices on t.JobID equals t1.JobID
            //                 join t2 in entity.JobTypes on t.JobTypeID equals t2.JobTypeID
            //                 join t3 in entity.CurrencyMasters on t1.ProvisionCurrencyID equals t3.CurrencyID
            //                 where t.JobID == jobid
            //                 select new customerInvoiceVM
            //                 {
            //                     InvoiceNo = t.InvoiceNo.Value,
            //                     InvoiceDate = t.InvoiceDate,
            //                     AmountRecieved = (t1.ProvisionHome / provisionCurrency),
            //                     InvoiceAmount = (t1.SalesHome / SalesCurrency),
            //                     Balence = ((t1.SalesHome / SalesCurrency) - (t1.ProvisionHome / provisionCurrency)),
            //                     JobId = t.JobID,
            //                     Currency = t3.CurrencyName



            //                 }).ToList();
            //    //return Json(query, JsonRequestBehavior.AllowGet);
            //    var view = this.RenderPartialView("ucCustomerInvoice", query);
            //    return new LargeJsonResult
            //    {
            //        MaxJsonLength = Int32.MaxValue,
            //        Data = new
            //        {
            //            success = true,
            //            view = view
            //        },
            //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //    };

            //}
            //else
            //{
            //    var BaceCurrencyID = (from t in entity.CurrencyMasters where t.StatusBaseCurrency == true select t.CurrencyID).FirstOrDefault();
            //    if (currencyid != BaceCurrencyID)
            //    {
            //        decimal exrate = (from x in entity.CurrencyMasters where x.CurrencyID == currencyid select x.ExchangeRate).FirstOrDefault().Value;



            //        var query = (from t in entity.JobGenerations
            //                     join t1 in entity.JInvoices on t.JobID equals t1.JobID
            //                     join t2 in entity.JobTypes on t.JobTypeID equals t2.JobTypeID
            //                     join t3 in entity.CurrencyMasters on t1.ProvisionCurrencyID equals t3.CurrencyID
            //                     where t.JobID == jobid
            //                     select new customerInvoiceVM
            //         {
            //             InvoiceNo = t.InvoiceNo.Value,
            //             InvoiceDate = t.InvoiceDate,
            //             AmountRecieved = (t1.ProvisionHome / provisionCurrency) ,
            //             InvoiceAmount = (t1.SalesHome / SalesCurrency) ,
            //             Balence = ((t1.SalesHome / SalesCurrency) - (t1.ProvisionHome / provisionCurrency)) ,
            //             JobId = t.JobID,
            //             Currency = t3.CurrencyName



            //         }).ToList();

            //        return Json(query, JsonRequestBehavior.AllowGet);
            //    }

            //    else
            //    {
            //        decimal exrate = (from x in entity.CurrencyMasters where x.CurrencyID == currencyid select x.ExchangeRate).FirstOrDefault().Value;



            //        var query = (from t in entity.JobGenerations
            //                     join t1 in entity.JInvoices on t.JobID equals t1.JobID
            //                     join t2 in entity.JobTypes on t.JobTypeID equals t2.JobTypeID
            //                     join t3 in entity.CurrencyMasters on t1.ProvisionCurrencyID equals t3.CurrencyID
            //                     where t.JobID == jobid
            //                     select new customerInvoiceVM
            //                     {
            //                         InvoiceNo = t.InvoiceNo.Value,
            //                         InvoiceDate = t.InvoiceDate,
            //                         AmountRecieved = (t1.ProvisionHome / provisionCurrency) /exrate,
            //                         InvoiceAmount = (t1.SalesHome / SalesCurrency) / exrate,
            //                         Balence = ((t1.SalesHome / SalesCurrency) - (t1.ProvisionHome / provisionCurrency)) / exrate,
            //                         JobId = t.JobID,
            //                         Currency = t3.CurrencyName



            //                     }).ToList();

            //        return Json(query, JsonRequestBehavior.AllowGet);
            //    }
            //}
        }
        public class Currency
        {
            public int CurrencyID { get; set; }
            public string CurrencyName { get; set; }
        }

        public JsonResult GetCurrency(int jobid)
        {
            var data1=(from  ji in entity.JInvoices where ji.JobID==jobid select ji.SalesCurrencyID).ToList();
            var data2 = (from t in entity.CurrencyMasters where t.StatusBaseCurrency == true select t.CurrencyID).FirstOrDefault();
            data1.Add(data2);
            var results = (from x in entity.CurrencyMasters
                           where data1.Contains(x.CurrencyID)
                           select x).ToList();

            List<Currency> lst = new List<Currency>();

            foreach (var item in results)
            {
                Currency c = new Currency();
                c.CurrencyID = item.CurrencyID;
                c.CurrencyName = item.CurrencyName;
                lst.Add(c);
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteCustmerInvoice(int invoiceNo)
        {
            var Jobgeneration = (from d in entity.JobGenerations where d.InvoiceNo == invoiceNo select d).FirstOrDefault();
            var jinvoice = (from d in entity.JInvoices where d.JobID == Jobgeneration.JobID select d).ToList();
            entity.JobGenerations.Remove(Jobgeneration);
            foreach (var item in jinvoice)
            {
                entity.JInvoices.Remove(item);
            }
            entity.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
