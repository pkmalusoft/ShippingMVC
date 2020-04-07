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
    public class CustomerJournalController : Controller
    {
        //
        // GET: /CustomerJournal/
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();

        SourceMastersModel objectSourceModel = new SourceMastersModel();

        public ActionResult Index()
        {
            var query = from t in entity.CUSTOMERs select t;
            ViewBag.customers = entity.CUSTOMERs.OrderBy(x => x.Customer1).ToList();

            var query1 = from t in entity.Suppliers select t;
            ViewBag.supplier = entity.Suppliers.OrderBy(x => x.SupplierName).ToList();

            var query3 = from t in entity.AcHeads select t;
            ViewBag.acHead = new SelectList(entity.AcHeads, "AcHeadID", "AcHead1");


            return View(new CustomerJournalVM());

        }

        [HttpPost]
        public LargeJsonResult GetGridData(CustomerJournalVM cust)
        {
            AcJournalMaster objACJournalMaster = new AcJournalMaster();
            objACJournalMaster.TransDate = DateTime.Now;

            objACJournalMaster.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"]);
            objACJournalMaster.AcFinancialYearID =Convert.ToInt32( Session["fyearid"]);
            objACJournalMaster.AcJournalID = objectSourceModel.GetMaxNumberAcJournalMasters(); ;
            objACJournalMaster.VoucherNo = "12345";
            objACJournalMaster.StatusDelete = false;
            objACJournalMaster.VoucherType = "";
            objACJournalMaster.Remarks = cust.Remark;
            if (cust.CustomerHeadId > 0)
            {
                objACJournalMaster.TransType = 1;
            }
            if (cust.SupplierHeadId > 0)
            {
                objACJournalMaster.TransType = 2;
            }
           
            objACJournalMaster.VoucherNo = "";
            entity.AcJournalMasters.Add(objACJournalMaster);
            entity.SaveChanges();

            if (objACJournalMaster.AcJournalID > 0)
            {
                var achead = objectSourceModel.GetAcHeadAssign();
                if (achead != null)
                {

                    AcJournalDetail acJournalDetailsDR = new AcJournalDetail();
                   
                        acJournalDetailsDR.Amount = Convert.ToDecimal(cust.amount);
                    acJournalDetailsDR.Remarks = cust.Remark;
                    acJournalDetailsDR.AcJournalID = objACJournalMaster.AcJournalID;
                    acJournalDetailsDR.AcJournalDetailID = objectSourceModel.GetMaxNumberAcJournalDetails();
                    if (cust.CustomerHeadId != 0)
                    {
                        acJournalDetailsDR.AcHeadID = achead.CustomerControlAcID;
                    }
                    else
                    {
                        acJournalDetailsDR.AcHeadID = achead.SupplierControlAcID;
                    }
                    entity.AcJournalDetails.Add(acJournalDetailsDR);
                    entity.SaveChanges();

                    AcJournalDetail acJournalDetailsCR = new AcJournalDetail();

                    acJournalDetailsCR.Amount = (-1) * Convert.ToDecimal(cust.amount);
                    acJournalDetailsCR.Remarks = cust.Remark;
                    acJournalDetailsCR.AcJournalID = objACJournalMaster.AcJournalID;
                    acJournalDetailsCR.AcJournalDetailID = objectSourceModel.GetMaxNumberAcJournalDetails();
                    if (cust.CustomerHeadId != 0)
                    {
                        acJournalDetailsCR.AcHeadID = achead.CustomerControlAcID;
                    }
                    else
                    {
                        acJournalDetailsCR.AcHeadID = achead.SupplierControlAcID;
                    }
                    entity.AcJournalDetails.Add(acJournalDetailsCR);
                    entity.SaveChanges();
                }
            }

            return new LargeJsonResult
            {
                MaxJsonLength=Int32.MaxValue,
                Data = new
                {
                    success = true,
                    AcJournalID = objACJournalMaster.AcJournalID
                  
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };


        }


        public void saveGridData(string AcHeadid, string amount, string remark, string debiteorcredit, int AcJournalID)
        {
            AcJournalDetail acjournalDetailsDR = new AcJournalDetail();
            acjournalDetailsDR.AcJournalID = AcJournalID;
            acjournalDetailsDR.AcJournalDetailID = objectSourceModel.GetMaxNumberAcJournalDetails();
            acjournalDetailsDR.AcHeadID = Convert.ToInt32(AcHeadid);
                acjournalDetailsDR.Amount = (-1) * Convert.ToDecimal(amount); 
            acjournalDetailsDR.Remarks = remark;
            entity.AcJournalDetails.Add(acjournalDetailsDR);
            entity.SaveChanges();



            AcJournalDetail acjournalDetailsCR = new AcJournalDetail();
            acjournalDetailsCR.AcJournalID = AcJournalID;
            acjournalDetailsCR.AcJournalDetailID = objectSourceModel.GetMaxNumberAcJournalDetails();
            acjournalDetailsCR.AcHeadID = Convert.ToInt32(AcHeadid);
            acjournalDetailsCR.Amount = Convert.ToDecimal(amount);
            acjournalDetailsCR.Remarks = remark;
            entity.AcJournalDetails.Add(acjournalDetailsCR);
            entity.SaveChanges();
        }

        public ActionResult CustomerJList()
        {

            return View();
        }

        public LargeJsonResult GetCustomerJList(int id = 1)
        {
           DateTime fromdate= Convert.ToDateTime(Session["FyearFrom"].ToString());
           DateTime todate = Convert.ToDateTime(Session["FyearTo"].ToString());

           AcHeadAssign a = entity.AcHeadAssigns.FirstOrDefault();
           if (id == 1)
           {
               var query = (from t in entity.AcJournalMasters
                            join ajd in entity.AcJournalDetails on t.AcJournalID equals ajd.AcJournalID
                            join p in entity.AcHeads on ajd.AcHeadID equals p.AcHeadID
                            where t.TransType == id && p.AcHeadID==a.CustomerControlAcID && t.TransDate >= fromdate && t.TransDate <= todate
                            select new CustomerJournalVM
                            {
                                AcHead = p.AcHead1,
                                amount = ajd.Amount,
                                Remark = ajd.Remarks,
                                AcJournalDetailID = ajd.AcJournalDetailID
                            }).ToList();
               string view = this.RenderPartialView("ucCustomerJList", query);
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
           else
           {
               var query = (from t in entity.AcJournalMasters
                            join ajd in entity.AcJournalDetails on t.AcJournalID equals ajd.AcJournalID
                            join p in entity.AcHeads on ajd.AcHeadID equals p.AcHeadID
                            where t.TransType == id && p.AcHeadID==a.SupplierControlAcID && t.TransDate >= fromdate && t.TransDate <= todate
                            select new CustomerJournalVM
                            {
                                AcHead = p.AcHead1,
                                amount = ajd.Amount,
                                Remark = ajd.Remarks,
                                AcJournalDetailID = ajd.AcJournalDetailID
                            }).ToList();
               string view = this.RenderPartialView("ucCustomerJList", query);
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

       

      
        public ActionResult DeleteCustomerJournal( int id)
        {
            AcJournalDetail acJournalD = entity.AcJournalDetails.Find(id);
            int acjournalid = acJournalD.AcJournalID.Value;
            var data = entity.AcJournalDetails.Where(item => item.AcJournalID == acjournalid).ToList();
            foreach (var item in data)
            {
                entity.AcJournalDetails.Remove(item);
                entity.SaveChanges();
            }
           

            AcJournalMaster acJournalMaster = entity.AcJournalMasters.Find(acjournalid);
        entity.AcJournalMasters.Remove(acJournalMaster);
        entity.SaveChanges();

       
            
            return RedirectToAction("CustomerJList");
        }

    }
}
