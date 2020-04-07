﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;
using DAL;

namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    public class DebitNoteController : Controller
    {
        SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();


        public ActionResult List()
        {
            var data = db.DebitNotes.ToList();

            List<DebitNoteVM> lst = new List<DebitNoteVM>();
            foreach (var item in data)
            {
                int jobid = (from c in db.JInvoices where c.InvoiceID == item.InvoiceID select c.JobID).FirstOrDefault().Value;
                string jobcode = (from j in db.JobGenerations where j.JobID == jobid select j.JobCode).FirstOrDefault();
                string supplier = (from c in db.Suppliers where c.SupplierID == item.SupplierID select c.SupplierName).FirstOrDefault();

                DebitNoteVM v = new DebitNoteVM();
                v.JobNo = jobcode;
                v.Date = item.DebitNoteDate.Value;
                v.SupplierName = supplier;
                v.Amount = item.Amount.Value;


                lst.Add(v);

            }

            return View(lst);

        }


        public ActionResult Index()
        {
            List<Invoices> lst = new List<Invoices>();
            ViewBag.Supplier = db.Suppliers.OrderBy(x => x.SupplierName).ToList();
            ViewBag.AcHead = db.AcHeads.OrderBy(x => x.AcHead1).ToList();
            ViewBag.Invoice = lst;
            return View();
        }


        [HttpPost]
        public ActionResult Index(DebitNoteVM v)
        {
            AcJournalMaster ajm = new AcJournalMaster();

            int acjm = 0;
            acjm = (from c in db.AcJournalMasters orderby c.AcJournalID descending select c.AcJournalID).FirstOrDefault();

            ajm.AcJournalID = acjm + 1;

            ajm.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            ajm.AcFinancialYearID = Convert.ToInt32(Session["fyearid"].ToString());
            ajm.PaymentType = 1;
            ajm.Remarks = "Debit Note";
            ajm.StatusDelete = false;
            ajm.TransDate = v.Date;
            ajm.VoucherNo = "DB-" + ajm.AcJournalID;
            ajm.TransType = 1;
            ajm.VoucherType = "";

            db.AcJournalMasters.Add(ajm);
            db.SaveChanges();


            AcJournalDetail a = new AcJournalDetail();

            int maxacj = 0;
            maxacj = (from c in db.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();

            a.AcJournalDetailID = maxacj + 1;

            a.AcJournalID = ajm.AcJournalID;
            a.AcHeadID = v.AcHeadID;
            a.Amount = v.Amount;
            a.BranchID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            a.Remarks = "";

            db.AcJournalDetails.Add(a);
            db.SaveChanges();


            AcJournalDetail b = new AcJournalDetail();
            maxacj = (from c in db.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();
            b.AcJournalDetailID = maxacj + 1;
            b.AcJournalID = ajm.AcJournalID;
            b.AcHeadID = v.AcHeadID;
            b.Amount = -v.Amount;
            b.BranchID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            b.Remarks = "";

          

         

            db.AcJournalDetails.Add(b);
            db.SaveChanges();


            int recpayid = (from c in db.RecPayDetails where c.InvoiceID == v.InvoiceNo select c.RecPayID).FirstOrDefault().Value;

            int max = 0;

            var data = (from c in db.DebitNotes orderby c.DebitNoteID descending select c).FirstOrDefault();
            if (data == null)
            {
                max = 1;
            }
            else
            {
                max = data.DebitNoteID + 1;
            }


            DebitNote d = new DebitNote();

            d.DebitNoteID = max + 1;
            d.DebitNoteNo = "D-" + (max + 1);
            d.InvoiceID = v.InvoiceNo;
            d.DebitNoteDate = v.Date;
            d.Amount = v.Amount;
            d.AcJournalID = ajm.AcJournalID;
            d.FYearID = Convert.ToInt32(Session["fyearid"].ToString());
            d.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            d.RecPayID = recpayid;
            d.AcHeadID = v.AcHeadID;
            d.SupplierID = v.SupplierID;
            d.InvoiceType = "S";
            db.DebitNotes.Add(d);
            db.SaveChanges();

            TempData["SuccessMsg"] = "Successfully Added Debit Note";
            return RedirectToAction("List", "DebitNote");

            


        }

        public class Invoices
        {
            public int InvoiceNo { get; set; }
        }


        public JsonResult GetInvoices(int id)
        {
            List<Invoices> lst = new List<Invoices>();

            var data = (from c in db.JInvoices where c.SupplierID == id select c).ToList();
         

            foreach (var item in data)
            {
                Invoices s = new Invoices();
                s.InvoiceNo = item.InvoiceID;

                lst.Add(s);
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public class Amounts
        {
            public decimal InvAmt { get; set; }
            public decimal AmtPaid { get; set; }
        }

        public JsonResult GetAmount(int invno)
        {
            Amounts a = new Amounts();

            decimal iamt = (from c in db.JInvoices where c.InvoiceID == invno select c.ProvisionHome).FirstOrDefault().Value;
            decimal pamt = Math.Abs((from x in db.RecPayDetails where x.InvoiceID == invno select x.Amount).FirstOrDefault().Value);

            a.InvAmt = iamt;
            a.AmtPaid = pamt;

            return Json(a, JsonRequestBehavior.AllowGet);
        }


    }
}
