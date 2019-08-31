using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;
using DAL;

namespace TrueBooksMVC.Controllers
{
   
    public class CreditNoteController : Controller
    {

        SourceMastersModel objSourceMastersModel = new SourceMastersModel();

        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();

        public ActionResult Index()
        {
            var data = db.CreditNotes.ToList().Where(x => x.CreditNoteID > 792).ToList();

            List<CreditNoteVM> lst = new List<CreditNoteVM>();
            foreach (var item in data)
            {
                int jobid = (from c in db.JInvoices where c.InvoiceID == item.InvoiceID select c.JobID).FirstOrDefault().Value;
                string jobcode = (from j in db.JobGenerations where j.JobID == jobid select j.JobCode).FirstOrDefault();
                string customer = (from c in db.CUSTOMERs where c.CustomerID == item.CustomerID select c.Customer1).FirstOrDefault();

                CreditNoteVM v = new CreditNoteVM();
                v.JobNO = jobcode;
                v.Date = item.CreditNoteDate.Value;
                v.CustomerName = customer;
                v.Amount = item.Amount.Value;


                lst.Add(v);

            }

            return View(lst);

        }


        public ActionResult Create()
        {
            ViewBag.customer = db.CUSTOMERs.OrderBy(x => x.Customer1).ToList();
            ViewBag.achead = db.AcHeads.ToList();
            List<jobno> lst = new List<jobno>();
            ViewBag.jobno = lst;
            
            return View();


        }

        [HttpPost]
        public ActionResult Create(CreditNoteVM v)
        {
            AcJournalMaster ajm = new AcJournalMaster();

            int acjm = 0;
            acjm = (from c in db.AcJournalMasters orderby c.AcJournalID descending select c.AcJournalID).FirstOrDefault();

            ajm.AcJournalID = acjm + 1;
            ajm.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            ajm.AcFinancialYearID = Convert.ToInt32(Session["fyearid"].ToString());
            ajm.PaymentType = 1;
            ajm.Remarks = "Credit Note";
            ajm.StatusDelete = false;
            ajm.TransDate = v.Date;
            ajm.VoucherNo = "C-" + ajm.AcJournalID;
            ajm.TransType = 2;
            ajm.VoucherType = "";

            db.AcJournalMasters.Add(ajm);
            db.SaveChanges();


            AcJournalDetail b = new AcJournalDetail();

            int maxacj = 0;
            maxacj = (from c in db.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();

            b.AcJournalDetailID = maxacj + 1;
            b.AcJournalID = ajm.AcJournalID;
            b.AcHeadID = v.AcHeadID;
            b.Amount = -v.Amount;
            b.BranchID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            b.Remarks = "";

            db.AcJournalDetails.Add(b);
            db.SaveChanges();


            AcJournalDetail a = new AcJournalDetail();
            maxacj = (from c in db.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();
            a.AcJournalDetailID = maxacj + 1;
            a.AcJournalID = ajm.AcJournalID;
            a.AcHeadID = v.AcHeadID;
            a.Amount = v.Amount;
            a.BranchID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            a.Remarks = "";
            


           



            db.AcJournalDetails.Add(a);
            db.SaveChanges();

            int jobid = (from j in db.JobGenerations where j.JobCode == v.JobNO select j.JobID).FirstOrDefault();

            int invid = (from c in db.JInvoices where c.JobID == jobid select c.InvoiceID).FirstOrDefault();


            int recpayid = (from c in db.RecPayDetails where c.InvoiceID == invid select c.RecPayID).FirstOrDefault().Value;

            CreditNote d = new CreditNote();

            //int max = (from c in db.CreditNotes orderby c.CreditNoteNo descending select c.CreditNoteNo).FirstOrDefault().Value;
            int maxid = 0;

            var data = (from c in db.CreditNotes orderby c.CreditNoteID descending select c).FirstOrDefault();

            if (data == null)
            {
                maxid = 1;
            }
            else
            {
                maxid = data.CreditNoteID + 1;
            }

            d.CreditNoteID = maxid;
            d.CreditNoteNo = maxid;
            d.InvoiceID = invid;
            d.CreditNoteDate = v.Date;
            d.Amount = v.Amount;
            d.AcJournalID = ajm.AcJournalID;
            d.FYearID = Convert.ToInt32(Session["fyearid"].ToString());
            d.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            d.RecPayID = recpayid;
            d.AcHeadID = v.AcHeadID;
            d.CustomerID = v.CustomerID;
            d.statusclose = false;
            d.InvoiceType = "C";
            

            db.CreditNotes.Add(d);
            db.SaveChanges();

            TempData["SuccessMsg"] = "Successfully Added Credit Note";
            return RedirectToAction("Index", "CreditNote");

            

           

        }

        public JsonResult GetJobNo(int id)
        {
            List<jobno> lst = new List<jobno>();
            var jobs = (from c in db.JobGenerations where c.ShipperID == id select c).ToList();
            if (jobs != null)
            {
                foreach (var item in jobs)
                {
                    jobno obj = new jobno();
                    obj.JobNo = item.JobCode;
                    lst.Add(obj);
                }
            }
           
            return Json(lst,JsonRequestBehavior.AllowGet);
        }
        public class jobno
        {
          
            public string JobNo { get; set; }
         
        }

        public JsonResult GetAmount(string id)
        {
            Getamtclass ob = new Getamtclass();

            int jobid=(from j in db.JobGenerations where j.JobCode==id select j.JobID).FirstOrDefault();

           int invid=(from c in db.JInvoices where c.JobID==jobid select c.InvoiceID).FirstOrDefault();

            decimal invamt=(from c in db.JInvoices where c.InvoiceID==invid select c.SalesHome).FirstOrDefault().Value;

            decimal recamt=(from r in db.RecPayDetails where r.InvoiceID==invid select r.Amount).FirstOrDefault().Value;


            ob.invoiceamt = invamt;
            ob.recamt = recamt;


        
            return Json(ob, JsonRequestBehavior.AllowGet);
        }

        public class Getamtclass
        {
            public decimal? invoiceamt { get; set; }
            public decimal? recamt { get; set; }

        }

    }
}
