﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;
using DAL;
using System.IO;
using System.Data;
using System.Data.SqlClient;



namespace TrueBooksMVC.Controllers
{
     [Authorize]
    public class AccountingModuleController : Controller
    {
        SHIPPING_FinalEntities context = new SHIPPING_FinalEntities();
        SourceMastersModel objectSourceModel = new SourceMastersModel();




        #region Masters
        //Methods For Account Category

        public ActionResult IndexAcCategory()
        {

            var x = context.AcCategorySelectAll();
            return View(x);
        }

        public ActionResult CreateAcCategory()
        {

            return View();
        }

        [HttpPost]
        public ActionResult CreateAcCategory(AcCategory c)
        {
            if (ModelState.IsValid)
            {
                context.AcCategoryInsert(c.AcCategory1);
                ViewBag.SuccessMsg = "You have successfully added Account Category";
                return View("IndexAcCategory", context.AcCategorySelectAll());
            }
            return View();
        }

        public ActionResult EditAcCategory(int id)
        {
            var x = context.AcCategorySelectByID(id);
            if (x == null)
            {
                return HttpNotFound();
            }
            return View(x.FirstOrDefault());
        }

        [HttpPost]
        public ActionResult EditAcCategory(AcCategorySelectByID_Result c)
        {
            context.AcCategoryUpdate(c.AcCategoryID, c.AcCategory);
            ViewBag.SuccessMsg = "You have successfully updated Account Category";
            return View("IndexAcCategory", context.AcCategorySelectAll());
        }


        public ActionResult DeleteAcCategory(int id)
        {
            AcCategory c = (from x in context.AcCategories where x.AcCategoryID == id select x).FirstOrDefault();
            if (c != null)
            {
                try
                {
                    context.AcCategories.Remove(c);
                    context.SaveChanges();
                  
                    ViewBag.SuccessMsg = "You have successfully deleted Account Category";
                 
                }
                catch (Exception ex)
                {

                
                        ViewBag.ErrorMsg= "Transaction in Use. Can not Delete";
                       
                 
                }
            }

            return View("IndexAcCategory", context.AcCategorySelectAll());
           
        }



        //Methods for Account Groups

        public ActionResult IndexAcGroup()
        {

            var x = context.AcGroupSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            return View(x);
        }

        public ActionResult CreateAcGroup()
        {
            ViewBag.Category = context.AcCategorySelectAll();
          
            ViewBag.groups = context.AcGroupSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));

            int count = (from c in context.AcCompanies select c).ToList().Count();
            ViewBag.IsAuto = count;
            
            return View();
        }


      

        [HttpGet]
        public JsonResult GetGroupsByID(int Category)
        {
            var groups = context.AcGroupSelectByCategoryID(Category, Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
            return Json(groups, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateAcGroup(AcGroupVM c)
        {
            
                if (c.AcGroup == 0)
                {
                    context.AcGroupInsert(c.AcCategoryID, c.subgroup, null, Convert.ToInt32(Session["AcCompanyID"].ToString()), Convert.ToInt32(Session["UserID"].ToString()), c.IsGroupCodeAuto, c.GroupCode);
                }
                else
                {
                    context.AcGroupInsert(c.AcCategoryID, c.subgroup, c.AcGroup, Convert.ToInt32(Session["AcCompanyID"].ToString()), Convert.ToInt32(Session["UserID"].ToString()), c.IsGroupCodeAuto, c.GroupCode);
                }
                //context.AcGroupInsert(c.AcCategoryID, c.subgroup, c.ParentID, Convert.ToInt32(Session["AcCompanyID"].ToString()), Convert.ToInt32(Session["UserID"].ToString()), c.IsGroupCodeAuto, c.GroupCode);
             
                //context.AcGroupInsert(c.AcCategoryID, c.AcGroup1, null, Convert.ToInt32(Session["AcCompanyID"].ToString()), Convert.ToInt32(Session["UserID"].ToString()), null, c.GroupCode);
                ViewBag.SuccessMsg = "You have successfully added Account Group";
                return View("IndexAcGroup", context.AcGroupSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())));
          

          
          
        }

        public ActionResult EditAcGroup(int id)
        {
            ViewBag.Category = context.AcCategorySelectAll();
            ViewBag.groups = context.AcGroupSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            //var x = context.AcGroupSelectByID(id);
            //if (x == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(x.FirstOrDefault());

            AcGroupVM v = new AcGroupVM();
            var data = context.AcGroups.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            else
            {
                v.AcGroupID = data.AcGroupID;
                v.subgroup = data.AcGroup1;
                if (data.ParentID == null)
                {

                    v.ParentID = 0;
                }
                else
                {
                    v.ParentID = data.ParentID.Value;
                }
                v.GroupCode = data.GroupCode;
                v.AcCategoryID = data.AcCategoryID.Value;
            }

            return View(v);
        }

        [HttpPost]
        public ActionResult EditAcGroup(AcGroupVM c)
        {
           
          
                context.AcGroupUpdate(c.AcGroupID, c.ParentID, c.subgroup, c.AcCategoryID, 0, c.GroupCode);
         
            ViewBag.SuccessMsg = "You have successfully updated Account Group";
            return View("IndexAcGroup", context.AcGroupSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())));
        }


        public ActionResult DeleteAcGroup(int id)
        {
            AcGroup c = (from x in context.AcGroups where x.AcGroupID == id select x).FirstOrDefault();
            if (c != null)
            {
                try
                {
                    var x = (from a in context.AcHeads where a.AcGroupID == id select a).ToList();
                    if (x != null)
                    {
                        ViewBag.ErrorMsg = "Transaction in Use. Can not Delete";
                        throw new Exception();

                    }
                    else
                    {
                        context.AcGroups.Remove(c);
                        context.SaveChanges();


                        ViewBag.SuccessMsg = "You have successfully deleted Account Group";
                    }

                }
                catch (Exception ex)
                {


                    


                }
            }
         
            return View("IndexAcGroup", context.AcGroupSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())));
        }




        //Methods for Expense Analysis Group
        public ActionResult IndexExpenseAnalysisGroup()
        {

            var x = context.AnalysisGroupSelectAll();
            return View(x);
        }

        public ActionResult CreateExpenseAnalysisGroup()
        {

            return View();
        }

        [HttpPost]
        public ActionResult CreateExpenseAnalysisGroup(AnalysisGroup c)
        {
            if (ModelState.IsValid)
            {

                context.AnalysisGroupInsert(GetMaxAnalysisGroupID(), c.AnalysisGroup1);
                ViewBag.SuccessMsg = "You have successfully added Expense Analysis Group";
                return View("IndexExpenseAnalysisGroup", context.AnalysisGroupSelectAll());
            }
            return View();
        }


        public ActionResult EditExpenseAnalysisGroup(int id)
        {
            var result = context.AnalysisGroupSelectByID(id);

            return View(result.FirstOrDefault());
        }

        [HttpPost]
        public ActionResult EditExpenseAnalysisGroup(AnalysisGroupSelectByID_Result a)
        {
            context.AnalysisGroupUpdate(a.AnalysisGroupID, a.AnalysisGroup);
            ViewBag.SuccessMsg = "You have successfully updated Expense Analysis Group";
            return View("IndexExpenseAnalysisGroup", context.AnalysisGroupSelectAll());
        }

        public ActionResult DeleteExpenseAnalysisGroup(int id)
        {
            AnalysisGroup c = (from x in context.AnalysisGroups where x.AnalysisGroupID == id select x).FirstOrDefault();
            if (c != null)
            {
                try
                {
                    context.AnalysisGroups.Remove(c);
                    context.SaveChanges();
               
                    ViewBag.SuccessMsg = "You have successfully deleted Expense Analysis Group.";

                }
                catch (Exception ex)
                {


                    ViewBag.ErrorMsg = "Transaction in Use. Can not Delete";


                }
            }
         
            return View("IndexExpenseAnalysisGroup", context.AnalysisGroupSelectAll());
        }

        public int GetMaxAnalysisGroupID()
        {
            var query = context.AnalysisGroups.OrderByDescending(item => item.AnalysisGroupID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.AnalysisGroupID + 1;
            }
        }





        //Methods For AcHead

        public ActionResult IndexAcHead()
        {
            return View(context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())));
        }

        public ActionResult DeleteAcHead(int id)
        {
            AcHead a = (from c in context.AcHeads where c.AcHeadID == id select c).FirstOrDefault();
            context.AcHeads.Remove(a);
            context.SaveChanges();
            ViewBag.SuccessMsg = "You have successfully deleted Account Head.";
            return View("IndexAcHead", context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())));
        }

        public ActionResult CreateAcHead()
        {
            ViewBag.groups = context.AcGroupSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            return View();
        }

        [HttpPost]
        public ActionResult CreateAcHead(AcHead a)
        {

            int id = 0;
            AcHead x = context.AcHeads.OrderByDescending(item => item.AcHeadID).FirstOrDefault();
            if(x==null)
            {

                id=1;
            }
            else{
                id=x.AcHeadID+1;
            }
            context.AcHeadInsert(id, a.AcHeadKey, a.AcHead1, a.AcGroupID, Convert.ToInt32(Session["AcCompanyID"].ToString()), a.Prefix);
            ViewBag.SuccessMsg = "You have successfully created Account Head.";
            return View("IndexAcHead", context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())));
        }

        public ActionResult EditAcHead(int id)
        {
            var result = context.AcHeadSelectByID(id);
            ViewBag.groups = context.AcGroupSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            return View(result.FirstOrDefault());
        }

        [HttpPost]
        public ActionResult EditAcHead(AcHeadSelectByID_Result a)
        {
            context.AcHeadUpdate(a.AcHeadKey, a.AcHeadID, a.AcHead, a.AcGroupID, a.Prefix);
            ViewBag.SuccessMsg = "You have successfully updated Account Head.";
            return View("IndexAcHead", context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())));
        }




        //Methods for Expense Analysis Head


        public ActionResult IndexExpenseAnalysisHead()
        {
            return View(context.AnalysisHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())));
        }

        public ActionResult DeleteExpenseAnalysisHead(int id)
        {
            AnalysisHead a = (from c in context.AnalysisHeads where c.AnalysisHeadID == id select c).FirstOrDefault();
            if (a != null)
            {
                context.AnalysisHeads.Remove(a);
                context.SaveChanges();
                ViewBag.SuccessMsg = "You have successfully deleted Analysis Head.";
            }
            return View("IndexExpenseAnalysisHead", context.AnalysisHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())));
        }


        public ActionResult CreateExpenseAnalysisHead()
        {
            ViewBag.groups = context.AnalysisGroupSelectAll().ToList();
            return View();
        }

        [HttpPost]
        public ActionResult CreateExpenseAnalysisHead(AnalysisHead a)
        {
            context.AnalysisHeadInsert(a.AnalysisCode, a.AnalysisHead1, a.AnalysisGroupID, Convert.ToInt32(Session["AcCompanyID"].ToString()));
            ViewBag.SuccessMsg = "You have successfully added Analysis Head.";
            return View("IndexExpenseAnalysisHead", context.AnalysisHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())));
        }



        public ActionResult EditExpenseAnalysisHead(int id)
        {
            var result = context.AnalysisHeadSelectByID(id);
            ViewBag.groups = context.AnalysisGroupSelectAll().ToList();
            return View(result.FirstOrDefault());
        }

        [HttpPost]
        public ActionResult EditExpenseAnalysisHead(AnalysisHeadSelectByID_Result a)
        {

            context.AnalysisHeadUpdate(a.AnalysisHeadID, a.AnalysisCode, a.AnalysisHead, a.AnalysisGroupID);

            ViewBag.SuccessMsg = "You have successfully updated Analysis Head.";
            return View("IndexExpenseAnalysisHead", context.AnalysisHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())));
        }



        //Methods for AcHeadAssign

        public ActionResult IndexAcHeadAssign()
        {
            return View(context.AcHeadAssignSelectAll());
        }



        public ActionResult CreateAcHeadAssign()
        {

            ViewBag.provisionheads = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            ViewBag.accruedcost = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            ViewBag.openjobrevenue = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            ViewBag.custmorcontrol = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            ViewBag.cashcontrol = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            ViewBag.controlacid = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            return View();
        }

        [HttpPost]
        public ActionResult CreateAcHeadAssign(AcHeadAssign a)
        {
            context.AcHeadAssignInsert(a.ProvisionCostControlAcID, a.AccruedCostControlAcID, a.OpenJobRevenueAcID, a.CustomerControlAcID, a.CashControlAcID, a.SupplierControlAcID);
            ViewBag.SuccessMsg = "You have successfully added Account Assign Head";
            return View("IndexAcHeadAssign", context.AcHeadAssignSelectAll());
        }

        public ActionResult EditAcHeadAssign(int id)
        {
            ViewBag.provisionheads = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            ViewBag.accruedcost = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            ViewBag.openjobrevenue = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            ViewBag.custmorcontrol = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            ViewBag.cashcontrol = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            ViewBag.controlacid = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));

            var result = (from c in context.AcHeadAssigns where c.ID == id select c).FirstOrDefault();
            return View(result);
        }

        [HttpPost]
        public ActionResult EditAcHeadAssign(AcHeadAssign a)
        {
            context.AcHeadAssignUpdate(a.ProvisionCostControlAcID, a.AccruedCostControlAcID, a.OpenJobRevenueAcID, a.CustomerControlAcID, a.CashControlAcID, a.SupplierControlAcID, a.ID);
            ViewBag.SuccessMsg = "You have successfully updated Account Assign Head";
            return View("IndexAcHeadAssign", context.AcHeadAssignSelectAll());
        }




        #endregion Masters



        //Cash And Bank Transactions



        public ActionResult AcJournalVoucherIndex()
        {

            return View(context.AcJournalMasterSelectAllJV(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString())));
        }

        public ActionResult AcJournalVoucherCreate()
        {
            var DebitAndCr = new SelectList(new[] 
                                        {
                                            new { ID = "1", trans = "Dr" },
                                            new { ID = "2", trans = "Cr" },
                                           
                                        },
                                      "ID", "trans", 1);
            ViewBag.Achead = context.AcHeads.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AcJournalVoucherCreate(AcJournalMasterVoucherVM data)
        {

            AcJournalMaster acJournalMaster = new AcJournalMaster();
            acJournalMaster.AcFinancialYearID = Convert.ToInt32(Session["fyearid"].ToString());
            acJournalMaster.AcJournalID = objectSourceModel.GetMaxNumberAcJournalMasters();
            acJournalMaster.VoucherType = "JV";

            int max = (from c in context.AcJournalMasters select c).ToList().Count();

            acJournalMaster.VoucherNo = (max + 1).ToString();
            acJournalMaster.UserID = Convert.ToInt32(Session["UserID"].ToString());
            acJournalMaster.TransDate = data.TransDate;
            acJournalMaster.StatusDelete = false;
            acJournalMaster.ShiftID = null;
            acJournalMaster.Remarks = data.Remark;
            acJournalMaster.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            acJournalMaster.Reference = data.Refference;
            context.AcJournalMasters.Add(acJournalMaster);
            context.SaveChanges();


            for (int i = 0; i < data.acJournalDetailsList.Count; i++)
            {
                AcJournalDetail acjournalDetails = new AcJournalDetail();
                if (data.acJournalDetailsList[i].IsDebit == 1)
                {
                    acjournalDetails.Amount = Convert.ToDecimal(data.acJournalDetailsList[i].Amount);
                }
                else
                {
                    acjournalDetails.Amount = (-1) * Convert.ToDecimal(data.acJournalDetailsList[i].Amount);
                }

                acjournalDetails.AcJournalID = acJournalMaster.AcJournalID;
                acjournalDetails.AcHeadID = data.acJournalDetailsList[i].acHeadID;
                acjournalDetails.Remarks = data.acJournalDetailsList[i].AcRemark;
                acjournalDetails.BranchID = Convert.ToInt32(Session["AcCompanyID"]);
                int maxAcJDetailID = 0;
                maxAcJDetailID = (from c in context.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();

                acjournalDetails.AcJournalDetailID = maxAcJDetailID + 1;
                context.AcJournalDetails.Add(acjournalDetails);
                context.SaveChanges();
            }
            ViewBag.SuccessMsg = "You have successfully added Journal Voucher.";
            return RedirectToAction("AcJournalVoucherIndex");
            


        }


        public ActionResult AcJournalVoucherEdit(int id = 0)
        {
            AcJournalMasterVoucherVM obj = new AcJournalMasterVoucherVM();
            ViewBag.achead = context.AcHeads.ToList();

            var data = (from d in context.AcJournalMasters where d.AcJournalID == id select d).FirstOrDefault();



            if (data == null)
            {
                return HttpNotFound();
            }
            else
            {
                obj.AcFinancialYearID = Convert.ToInt32(Session["fyearid"].ToString());
                obj.AcJournalID = data.AcJournalID;
                obj.VoucherType = "JV";
                obj.VoucherNo = data.VoucherNo;
                obj.userId = Convert.ToInt32(Session["UserID"].ToString());
                obj.TransDate = data.TransDate.Value;
                obj.statusDelete = false;
                //obj.ShiftID = null;
                obj.Remark = data.Remarks;
                obj.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
                obj.Refference = data.Reference;

            }
            return View(obj);

        }
        [HttpPost]
        public ActionResult AcJournalVoucherEdit(AcJournalMasterVoucherVM data)
        {
            AcJournalMaster obj = new AcJournalMaster();
            obj.AcFinancialYearID = Convert.ToInt32(Session["fyearid"].ToString());
            obj.AcJournalID = data.AcJournalID;
            obj.VoucherType = "JV";
            obj.VoucherNo = data.VoucherNo;
            obj.UserID = Convert.ToInt32(Session["UserID"].ToString());
            obj.TransDate = data.TransDate;
            obj.StatusDelete = false;
            //obj.ShiftID = null;
            obj.Remarks = data.Remark;
            obj.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            obj.Reference = data.Refference;
            context.Entry(obj).State = EntityState.Modified;
            context.SaveChanges();


            var x = (from c in context.AcJournalDetails where c.AcJournalID == data.AcJournalID select c).ToList();

            foreach (var i in x)
            {
                context.AcJournalDetails.Remove(i);
                context.SaveChanges();
            }


            for (int i = 0; i < data.acJournalDetailsList.Count; i++)
            {
                AcJournalDetail acjournalDetails = new AcJournalDetail();
                int maxAcJDetailID = 0;
                maxAcJDetailID = (from c in context.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();

                acjournalDetails.AcJournalDetailID = maxAcJDetailID + 1;
                if (data.acJournalDetailsList[i].IsDebit == 1)
                {
                    acjournalDetails.Amount = Convert.ToDecimal(data.acJournalDetailsList[i].Amount);
                }
                else
                {
                    acjournalDetails.Amount = (-1) * Convert.ToDecimal(data.acJournalDetailsList[i].Amount);
                }

        
                acjournalDetails.AcJournalID = obj.AcJournalID;
                acjournalDetails.AcHeadID = data.acJournalDetailsList[i].acHeadID;
                acjournalDetails.Remarks = data.acJournalDetailsList[i].AcRemark;
                acjournalDetails.BranchID = Convert.ToInt32(Session["AcCompanyID"]);
                context.AcJournalDetails.Add(acjournalDetails);
                context.SaveChanges();
            }
            ViewBag.SuccessMsg = "You have successfully added Journal Voucher.";
            return RedirectToAction("AcJournalVoucherIndex");


        }


        public JsonResult GetAcJVDetails(int id)
        {
            var lst = (from c in context.AcJournalDetails where c.AcJournalID == id select c).ToList();

            List<AcJournalDetailsList> acdetails = new List<AcJournalDetailsList>();

            foreach (var item in lst)
            {
                AcJournalDetailsList v = new AcJournalDetailsList();
                string x = (from a in context.AcHeads where a.AcHeadID == item.AcHeadID select a.AcHead1).FirstOrDefault();

                v.acHeadID = item.AcHeadID.Value;
                v.AcHead = x;
                v.AcRemark = item.Remarks;
               
                if (item.Amount < 0)
                {
                    v.IsDebit = 0;
                    v.drcr = "Cr";
                    v.Amount = (-item.Amount.Value);
                }
                else
                {
                    v.IsDebit = 1;
                    v.drcr = "Dr";
                    v.Amount = item.Amount.Value;
                }

                v.AcJournalDetID = item.AcJournalDetailID;

                acdetails.Add(v);

            }
            return Json(acdetails, JsonRequestBehavior.AllowGet);
        }


        public ActionResult IndexAcBook()
        {
            return View(context.AcJournalMasterSelectAll(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString())));
        }



        public ActionResult IndexOpenningBalance()
        {
            var list = new SelectList(new[] 
            {
                new { ID = "1", Name = "Cr" },
                new { ID = "2", Name = "Dr" },
               
            },
            "ID", "Name", 1);
            ViewBag.crdr = list;
            List<OpennnigBalanceVM> ob = new List<OpennnigBalanceVM>();
            var data = context.AcOpeningMasterSelectAll(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();


            foreach (var item in data)
            {
                OpennnigBalanceVM b = new OpennnigBalanceVM();
                b.AcHeadID = item.AcHeadID.Value;
                b.AcHead = item.AcHead;
                b.AcFinancialYearID = item.AcFinancialYearID.Value;
                if (item.Amount < 0)
                {
                    b.CrDr = 1;
                }
                else if (item.Amount > 0)
                {
                    b.CrDr = 2;
                }
                else
                {
                    b.CrDr = 2;
                }

                b.Amount = item.Amount.Value;

                ob.Add(b);

            }
            //ob.Items = context.AcOpeningMasterSelectAll(1, 1).ToList();
            //return View(context.AcOpeningMasterSelectAll(Convert.ToInt32(Session["fyearid"].ToString()),Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList());

            return View(ob);
        }


        [HttpPost]
        public ActionResult IndexOpenningBalance(List<OpennnigBalanceVM> lst)
        {


            for (int i = 0; i < lst.Count; i++)
            {


                // int fyearid = 1;
                int AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());

                if (lst[i].CrDr == 1)
                {
                    context.AcOpeningMasterInsert(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToDateTime(Session["FyearFrom"].ToString()), lst[i].AcHeadID, -lst[i].Amount, Convert.ToInt32(Session["AcCompanyID"].ToString()));
                }
                else
                {
                    context.AcOpeningMasterInsert(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToDateTime(Session["FyearFrom"].ToString()), lst[i].AcHeadID, lst[i].Amount, Convert.ToInt32(Session["AcCompanyID"].ToString()));
                }

                //var a = (from x in context.AcOpeningMasters where ((x.StatusImport == null) && (x.AcFinancialYearID == fyearid) && (x.BranchID == AcCompanyID) && (x.AcHeadID == lst[i].AcHeadID)) select x);
                //if (a != null)
                //{
                //    context.AcOpeningMasters.Remove(a.FirstOrDefault());
                //    context.SaveChanges();
                //}

                //AcOpeningMaster aom = new AcOpeningMaster();
                //aom.AcFinancialYearID = fyearid;
                //aom.OPDate = Convert.ToDateTime("01 Jan 2015");
                //aom.AcHeadID = lst[i].AcHeadID;

                //if (lst[i].CrDr == "Cr")
                //{
                //    aom.Amount = -lst[i].Amount;
                //}

                //aom.BranchID = AcCompanyID;
                //aom.AcCompanyID = AcCompanyID;

                //context.AcOpeningMasters.Add(aom);
                //context.SaveChanges();

            }
            ViewBag.SuccessMsg = "Your Record is Successfully Added";
            return RedirectToAction("IndexOpenningBalance", context.AcOpeningMasterSelectAll(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList());
        }

        public ActionResult CreateAcBook()
        {
            var transtypes = new SelectList(new[] 
                                        {
                                            new { ID = "1", trans = "Receipt" },
                                            new { ID = "2", trans = "Payment" },
                                           
                                        },
            "ID", "trans", 1);

            var paytypes = new SelectList(new[]{
                                            new { ID = "1", pay = "Cash" },
                                             new { ID = "2", pay = "Cheque" },
                                              new { ID = "3", pay = "Credit Card" },
                                               new { ID = "4", pay = "Bank Transfer" },
                                                new { ID = "5", pay = "Bank Deposit" },
                                        }, "ID", "pay", 1);


            ViewBag.transtypes = transtypes;
            ViewBag.paytypes = paytypes;
            //ViewBag.heads = context.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            //ViewBag.headsreceived = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));


            ViewBag.heads = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            ViewBag.headsreceived = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));




            return View();
        }



        public JsonResult ExpAllocation(decimal amount,int acheadid)
        {

            ViewBag.amt = amount;
            ViewBag.headid = acheadid;
            ViewBag.heads = context.AcHeads.ToList();
            string view = this.RenderPartialView("_ExpAllocate", null);

            return new JsonResult
            {
                Data = new
                {
                    success = true,
                    view = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }


        [HttpPost]
        public ActionResult CreateAcBook(AcBookVM v)
        {
            string cheque = "";
            string StatusTrans = "";

            if (v.paytype > 1)
            {
                cheque = v.chequeno;
            }
            else
            {
                cheque = "";
            }


            //string voucherno = "B123";
            int voucherno = 0;
            voucherno = (from c in context.AcJournalMasters select c).ToList().Count();

            int max = 0;
            max = (from c in context.AcJournalMasters orderby c.AcJournalID descending select c.AcJournalID).FirstOrDefault();


            AcJournalMaster ajm = new AcJournalMaster();
            ajm.AcJournalID = max + 1;
            ajm.VoucherNo = (voucherno + 1).ToString();
            ajm.TransDate = v.transdate;
            ajm.TransType = Convert.ToInt16(v.transtype);
            ajm.AcFinancialYearID = Convert.ToInt32(Session["fyearid"].ToString());
            ajm.VoucherType = v.TransactionType;
            ajm.StatusDelete = false;
            ajm.Remarks = v.remarks;
            ajm.UserID = Convert.ToInt32(Session["UserID"].ToString());
            ajm.ShiftID = null;
            ajm.PaymentType = v.paytype;

            //ajm.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            ajm.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            ajm.Reference = v.reference;

            context.AcJournalMasters.Add(ajm);
            context.SaveChanges();

            if (v.TransactionType == "CBR" || v.TransactionType == "BKR")
                StatusTrans = "R";
            else if (v.TransactionType == "CBP" || v.TransactionType == "BKP")
                StatusTrans = "P";

            if (v.chequeno!=null)
            {
                AcBankDetail acbankDetails = new AcBankDetail();
                int maxacbid=0;
                maxacbid = (from c in context.AcBankDetails orderby c.AcBankDetailID descending select c.AcBankDetailID).FirstOrDefault();
                acbankDetails.AcBankDetailID = maxacbid + 1;
                acbankDetails.AcJournalID = ajm.AcJournalID;
                acbankDetails.BankName = v.bankname;
                acbankDetails.ChequeDate = v.chequedate;
                acbankDetails.ChequeNo = v.chequeno;
                acbankDetails.PartyName = v.partyname;
                acbankDetails.StatusTrans = StatusTrans;
                acbankDetails.StatusReconciled = false;
                context.AcBankDetails.Add(acbankDetails);
                context.SaveChanges();
            }

            decimal TotalAmt=0;

             for (int i = 0; i < v.AcJDetailVM.Count; i++)
            {
              
                  TotalAmt=TotalAmt + Convert.ToDecimal(v.AcJDetailVM[i].Amt);
               
            }


            AcJournalDetail ac = new AcJournalDetail();
            int maxAcJDetailID = 0;
            maxAcJDetailID = (from c in context.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();

            ac.AcJournalDetailID = maxAcJDetailID + 1;
            ac.AcJournalID = ajm.AcJournalID;
            ac.AcHeadID = v.AcHead;
            if (StatusTrans == "P")
            {
                ac.Amount = -(TotalAmt);
            }
            else
            {
                ac.Amount = TotalAmt;
            }
            ac.Remarks = v.AcJDetailVM[0].Rem;
            ac.BranchID = Convert.ToInt32(Session["AcCompanyID"].ToString());

            context.AcJournalDetails.Add(ac);
            context.SaveChanges();



            for (int i = 0; i < v.AcJDetailVM.Count; i++)
            {
                AcJournalDetail acJournalDetail = new AcJournalDetail();
               
                maxAcJDetailID = (from c in context.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();

                acJournalDetail.AcJournalDetailID = maxAcJDetailID + 1;
                acJournalDetail.AcHeadID = v.AcJDetailVM[i].AcHeadID;

                acJournalDetail.BranchID = Convert.ToInt32(Session["AcCompanyID"]);
                acJournalDetail.AcJournalID = ajm.AcJournalID;
                acJournalDetail.Remarks = v.AcJDetailVM[i].Rem;

                if (StatusTrans == "P")
                {
                    acJournalDetail.Amount = (v.AcJDetailVM[i].Amt);
                }
                else
                {
                    acJournalDetail.Amount = -v.AcJDetailVM[i].Amt;
                }

                context.AcJournalDetails.Add(acJournalDetail);
                context.SaveChanges();

            }


            ViewBag.SuccessMsg = "You have successfully added Record";
            return View("IndexAcBook", context.AcJournalMasterSelectAll(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString())));

        }


        public ActionResult EditAcBook(int id)
        {
            AcBookVM v = new AcBookVM();

            AcJournalMaster ajm = context.AcJournalMasters.Find(id);
            AcBankDetail abank = (from a in context.AcBankDetails where a.AcJournalID == id select a).FirstOrDefault();
            v.transdate = ajm.TransDate.Value;
            v.AcHead = (from c in context.AcJournalDetails where c.AcJournalID == ajm.AcJournalID select c.AcHeadID).FirstOrDefault().Value;
            v.remarks = ajm.Remarks;
            v.reference = ajm.Reference;
            v.VoucherType = ajm.VoucherType;
            v.AcJournalID = ajm.AcJournalID;
            v.VoucherNo = ajm.VoucherNo;
            v.TransactionType = v.VoucherType;
            v.paytype = Convert.ToInt16(ajm.PaymentType);
            v.transtype = Convert.ToInt32(ajm.TransType);

            if (abank != null)
            {
                v.AcBankDetailID = abank.AcBankDetailID;
                v.bankname = abank.BankName;
                v.partyname = abank.PartyName;
                v.chequedate = abank.ChequeDate.Value;
                v.chequeno = abank.ChequeNo;
            }



            var transtypes = new SelectList(new[] 
                                        {
                                            new { ID = "1", trans = "Receipt" },
                                            new { ID = "2", trans = "Payment" },
                                           
                                        },
           "ID", "trans", 1);

            var paytypes = new SelectList(new[]{
                                            new { ID = "1", pay = "Cash" },
                                             new { ID = "2", pay = "Cheque" },
                                              new { ID = "3", pay = "Credit Card" },
                                               new { ID = "4", pay = "Bank Transfer" },
                                                new { ID = "5", pay = "Bank Deposit" },
                                        }, "ID", "pay", 1);


            ViewBag.transtypes = transtypes;
            ViewBag.paytypes = paytypes;
            if (v.VoucherType == "CBR" || v.VoucherType == "CBP")
            {
                var data = context.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString()));
                //ViewBag.heads = context.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString()));
                ViewBag.heads = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            }
            else if (v.VoucherType == "BKP" || v.VoucherType == "BKR" || v.VoucherType == "RP" || v.VoucherType == "BK")
            {
                //ViewBag.heads = context.AcHeadSelectForBank(Convert.ToInt32(Session["AcCompanyID"].ToString()));
                ViewBag.heads = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            }
            ViewBag.headsreceived = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));


            return View(v);


        }


        [HttpPost]
        public ActionResult EditAcBook(AcBookVM v)
        {
            string cheque = "";
            string StatusTrans = "";

            if (v.paytype > 1)
            {
                cheque = v.chequeno;
            }
            else
            {
                cheque = "";
            }


            //string voucherno = "B123";
            //int voucherno = 0;
            //voucherno = (from c in context.AcJournalMasters select c).ToList().Count();

            //int max = 0;
            //max = (from c in context.AcJournalMasters orderby c.AcJournalID descending select c.AcJournalID).FirstOrDefault();


            AcJournalMaster ajm = new AcJournalMaster();
            ajm.AcJournalID = v.AcJournalID;
            ajm.VoucherNo = v.VoucherNo;
            ajm.TransDate = v.transdate;
            ajm.TransType = Convert.ToInt16(v.transtype);
            ajm.AcFinancialYearID = Convert.ToInt32(Session["fyearid"].ToString());
           ajm.VoucherType = v.TransactionType;
            ajm.StatusDelete = false;
            ajm.Remarks = v.remarks;
            ajm.UserID = Convert.ToInt32(Session["UserID"].ToString());
            ajm.ShiftID = null;
            ajm.PaymentType = v.paytype;

            ajm.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            ajm.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            ajm.Reference = v.reference;

            context.Entry(ajm).State = EntityState.Modified;
            context.SaveChanges();

            if (v.TransactionType == "CBR" || v.TransactionType == "BKR")
                StatusTrans = "R";
            else if (v.TransactionType == "CBP" || v.TransactionType == "BKP")
                StatusTrans = "P";

            if (v.chequeno != null)
            {
                AcBankDetail acbankDetails = new AcBankDetail();
                acbankDetails.AcBankDetailID = v.AcBankDetailID;
                acbankDetails.BankName = v.bankname;
                acbankDetails.ChequeDate = v.chequedate;
                acbankDetails.ChequeNo = v.chequeno;
                acbankDetails.PartyName = v.partyname;

                context.Entry(acbankDetails).State = EntityState.Modified;
                context.SaveChanges();
            }

            decimal TotalAmt = 0;

            for (int i = 0; i < v.AcJDetailVM.Count; i++)
            {

                TotalAmt = TotalAmt + Convert.ToDecimal(v.AcJDetailVM[i].Amt);

            }


            var acjdetails = (from c in context.AcJournalDetails where c.AcJournalID == v.AcJournalID select c).ToList();

            foreach (var i in acjdetails)
            {
                context.AcJournalDetails.Remove(i);
                context.SaveChanges();
            }



            AcJournalDetail ac = new AcJournalDetail();

            int maxAcJDetailID = 0;
            maxAcJDetailID = (from c in context.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();

            ac.AcJournalID = ajm.AcJournalID;
            ac.AcJournalDetailID = maxAcJDetailID + 1;
            ac.AcHeadID = v.AcHead;
            if (StatusTrans == "P")
            {
                ac.Amount = -(TotalAmt);
            }
            else
            {
                ac.Amount = TotalAmt;
            }
            ac.Remarks = v.AcJDetailVM[0].Rem;
            ac.BranchID = Convert.ToInt32(Session["AcCompanyID"].ToString());

            context.AcJournalDetails.Add(ac);
            context.SaveChanges();



            for (int i = 0; i < v.AcJDetailVM.Count; i++)
            {
                AcJournalDetail acJournalDetail = new AcJournalDetail();
                maxAcJDetailID = (from c in context.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();

                acJournalDetail.AcJournalDetailID = maxAcJDetailID + 1;
                acJournalDetail.AcHeadID = v.AcJDetailVM[i].AcHeadID;

                acJournalDetail.BranchID = Convert.ToInt32(Session["AcCompanyID"]);
                acJournalDetail.AcJournalID = ajm.AcJournalID;
                acJournalDetail.Remarks = v.AcJDetailVM[i].Rem;

                if (StatusTrans == "P")
                {
                    acJournalDetail.Amount = (v.AcJDetailVM[i].Amt);
                }
                else
                {
                    acJournalDetail.Amount = -v.AcJDetailVM[i].Amt;
                }

                context.AcJournalDetails.Add(acJournalDetail);
                context.SaveChanges();

            }


            ViewBag.SuccessMsg = "You have successfully added Record";
            return View("IndexAcBook", context.AcJournalMasterSelectAll(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString())));
                                    //string cheque = "";
                                    //string StatusTrans = "";

                                    ////if (v.paytype > 1)
                                    ////{
                                    ////    cheque = v.chequeno;
                                    ////}
                                    ////else
                                    ////{
                                    ////    cheque = "";
                                    ////}


                                    ////string voucherno = "B123";
                                    ////int voucherno = 0;
                                    ////voucherno = (from c in context.AcJournalMasters select c).ToList().Count();



                                    //AcJournalMaster ajm = new AcJournalMaster();
                                    //ajm.AcJournalID = v.AcJournalID;
                                    //ajm.VoucherNo = v.VoucherNo;
                                    //ajm.TransDate = v.transdate;
                                    //ajm.TransType = Convert.ToInt16(v.transtype);
                                    //ajm.AcFinancialYearID = Convert.ToInt32(Session["fyearid"].ToString());
                                    ////ajm.VoucherType = v.TransactionType;
                                    //ajm.VoucherType = v.VoucherType;

                                    //ajm.StatusDelete = false;
                                    //ajm.Remarks = v.remarks;
                                    //ajm.UserID = Convert.ToInt32(Session["UserID"].ToString());
                                    //ajm.ShiftID = null;

                                    ////ajm.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
                                    //ajm.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
                                    //ajm.Reference = v.reference;

                                    //context.Entry(ajm).State = EntityState.Modified;
                                    //context.SaveChanges();

                                    ////if (v.TransactionType == "CBR" || v.TransactionType == "BKR")
                                    ////    StatusTrans = "R";
                                    ////else if (v.TransactionType == "CBP" || v.TransactionType == "BKP")
                                    ////    StatusTrans = "P";

                                    //if (v.VoucherType == "CBR" || v.VoucherType == "BKR")
                                    //{
                                    //    StatusTrans = "R";
                                    //}
                                    //else if (v.VoucherType == "CBP" || v.VoucherType == "BKP")
                                    //{
                                    //    StatusTrans = "P";
                                    //}

                                    //if (v.chequeno != null)
                                    //{
                                    //    AcBankDetail acbankDetails = new AcBankDetail();
                                    //    acbankDetails.AcBankDetailID = v.AcBankDetailID;
                                    //    acbankDetails.AcJournalID = ajm.AcJournalID;
                                    //    acbankDetails.BankName = v.bankname;
                                    //    acbankDetails.ChequeDate = v.chequedate;
                                    //    acbankDetails.ChequeNo = v.chequeno;
                                    //    acbankDetails.PartyName = v.partyname;
                                    //    acbankDetails.StatusTrans = StatusTrans;
                                    //    acbankDetails.StatusReconciled = false;
                                    //    context.Entry(acbankDetails).State = EntityState.Modified;
                                    //    context.SaveChanges();
                                    //}



                                    ////AcJournalDetail ac = new AcJournalDetail();
                                    ////ac.AcJournalID = ajm.AcJournalID;
                                    ////ac.AcHeadID = v.AcHead;
                                    ////if (StatusTrans == "P")
                                    ////{
                                    ////    ac.Amount = -(v.TotalAmt);
                                    ////}
                                    ////else
                                    ////{
                                    ////    ac.Amount = v.TotalAmt;
                                    ////}
                                    ////ac.Remarks = v.remarks;
                                    ////ac.BranchID = Convert.ToInt32(Session["AcCompanyID"].ToString());

                                    ////context.AcJournalDetails.Add(ac);
                                    ////context.SaveChanges();



                                    //for (int i = 0; i < v.AcJDetailVM.Count; i++)
                                    //{
                                    //    AcJournalDetail acJournalDetail = new AcJournalDetail();
                                    //    acJournalDetail.AcHeadID = v.AcJDetailVM[i].AcHeadID;
                                    //    acJournalDetail.AcJournalID = ajm.AcJournalID;
                                    //    acJournalDetail.BranchID = Convert.ToInt32(Session["AcCompanyID"]);
                                    //    acJournalDetail.AcJournalDetailID = v.AcJDetailVM[i].AcJournalDetID;
                                    //    acJournalDetail.Remarks = v.AcJDetailVM[i].Rem;

                                    //    if (StatusTrans == "P")
                                    //    {
                                    //        acJournalDetail.Amount = -(v.AcJDetailVM[i].Amt);
                                    //    }
                                    //    else
                                    //    {
                                    //        acJournalDetail.Amount = v.AcJDetailVM[i].Amt;
                                    //    }

                                    //    context.Entry(acJournalDetail).State = EntityState.Modified;
                                    //    context.SaveChanges();

                                    //}


                                    //ViewBag.SuccessMsg = "You have successfully added Record";
                                    //return View("IndexAcBook", context.AcJournalMasterSelectAll(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString())));
        }

        public JsonResult GetAcJDetails(Nullable<int> id,int? transtype)
        {
            //var acjlist = (from c in context.AcJournalDetails where c.AcJournalID == id select c).ToList();

            string TransType = "";
            if (transtype == 1)
            {
                TransType = "R";
            }
            else
            {
                TransType = "P";
            }

            var acjlist = context.AcJournalDetailSelectByAcJournalID(id, TransType).ToList();
            
            List<AcJournalDetailVM> AcJDetailVM = new List<AcJournalDetailVM>();
            foreach (var item in acjlist)
            {
                AcJournalDetailVM v = new AcJournalDetailVM();
                string x = (from a in context.AcHeads where a.AcHeadID == item.AcHeadID select a.AcHead1).FirstOrDefault();

                v.AcHeadID = item.AcHeadID.Value;
                v.AcHead = x;

                if (item.Amount < 0)
                {
                    v.Amt = (-item.Amount.Value);
                }
                else
                {
                    v.Amt = item.Amount.Value;
                }
                v.Rem = item.Remarks;
                v.AcJournalDetID = item.AcJournalDetailID;
                AcJDetailVM.Add(v);
            }

            return Json(AcJDetailVM, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetHeadsForCash()
        {

            int AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            //List<AcHeadSelectForCash_Result> x = null;

            //x = context.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();


            List<AcHeadSelectAll_Result> x = null;
            x = context.AcHeadSelectAll(AcCompanyID).ToList();



            return Json(x, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetHeadsForBank()
        {
            int AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            //List<AcHeadSelectForBank_Result> x = null;

            //x = context.AcHeadSelectForBank(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();


            List<AcHeadSelectAll_Result> x = null;
            x = context.AcHeadSelectAll(AcCompanyID).ToList();

            return Json(x, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetBalance(int acheadid)
        {
            var x = context.GetAccountBalanceByHeadID(acheadid, Convert.ToInt32(Session["fyearid"].ToString()));
            return Json(x, JsonRequestBehavior.AllowGet);
        }






        #region BankReconciliation

        //Bank Reconciliation
        public ActionResult BankReconcilation()
        {

            int AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());

            //ViewBag.Data = context.AcHeadSelectForBank(AcCompanyID);

            ViewBag.Data = context.AcHeadSelectAll(AcCompanyID);

            //List<BankReconcilVM> lst = new List<BankReconcilVM>();

            //var data = (from c in context.AcBankDetails where (c.StatusReconciled == false) select c).ToList();

            //foreach (var item in data)
            //{
            //    BankReconcilVM v = new BankReconcilVM();
            //    v.AcBankDetailID = item.AcBankDetailID;
            //    v.AcJournalID = item.AcJournalID.Value;
            //    v.BankName = item.BankName;
            //    v.ChequeNo = item.ChequeNo;
            //    if (item.ChequeDate.HasValue)
            //        v.ChequeDate = item.ChequeDate.Value;
            //    v.PartyName = item.PartyName;
            //    v.StatusTrans = item.StatusTrans;
            //    if (item.StatusReconciled.HasValue)
            //        v.StatusReconciled = item.StatusReconciled.Value;
            //    v.ValueDate = Convert.ToDateTime(item.ValueDate);
            //    v.IsSelected = false;
            //    lst.Add(v);


            //}

            //return View(lst);

            return View();

        }

        //[HttpPost]
        //public ActionResult GetBankReconciliation(List<TrueBooksMVC.Models.BankReconcilVM> lst)
        //{

        //    //Update AcBankDetails Table 
        //    var selectedrecords = lst.Where(item => item.IsSelected == true).ToList();
        //    foreach (var item in selectedrecords)
        //    {
        //        AcBankDetail a = (from c in context.AcBankDetails where c.AcBankDetailID == item.AcBankDetailID select c).FirstOrDefault();
        //        a.ValueDate = item.ValueDate;
        //        a.StatusReconciled = true;
        //        context.Entry(a).State = EntityState.Modified;
        //        context.SaveChanges();

        //    } 
        //    return RedirectToAction("BankReconcilation");

        //}


        public JsonResult ShowBankReconciliation(string acheadid,string from,string to)
        {

            int vacheadid = 0;

            if (acheadid != null)
            {
                vacheadid = Convert.ToInt32(acheadid);
            }
            else
            {
                vacheadid = 0;
            }

            DateTime frm = Convert.ToDateTime(from);
            DateTime dto = Convert.ToDateTime(to);

            var data = context.GetBankReconciliationOutStandings(vacheadid, frm, dto);



            List<BankReconcilVM> lst = new List<BankReconcilVM>();



            foreach (var item in data)
            {
                BankReconcilVM v = new BankReconcilVM();
                v.AcBankDetailID = item.AcBankDetailID;
                v.AcJournalID = item.AcJournalID;
                v.BankName = item.BankName;
                v.ChequeNo = item.ChequeNo;
                if (item.ChequeDate.HasValue)
                    v.ChequeDate = item.ChequeDate.Value;
                v.PartyName = item.PartyName;
                v.StatusReconciled = false;
                v.Remarks = item.Remarks;
                v.AcHead = item.AcHead;
                v.VoucherNo = item.VoucherNo;
                v.VoucherDate = item.TransDate.Value;
                v.Amount = item.Amount.Value;

                v.IsSelected = false;
                lst.Add(v);


            }

            lst = lst.ToList();

            string view = this.RenderPartialView("GetBankReconciliation", lst);
            return new JsonResult
            {
                Data = new
                {
                    success = true,
                    view = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }


    [HttpPost]
        public ActionResult ShowBankReconciliation(List<TrueBooksMVC.Models.BankReconcilVM> lst)
        {
            var selectedrecords = lst.Where(item => item.IsSelected == true).ToList();
            foreach (var item in selectedrecords)
            {
                AcBankDetail b = (from c in context.AcBankDetails where c.AcBankDetailID == item.AcBankDetailID select c).FirstOrDefault();
                b.ValueDate = item.ValueDate;
                b.StatusReconciled = true;

                context.Entry(b).State = EntityState.Modified;
                context.SaveChanges();

            }
            return RedirectToAction("BankReconcilation");
        }

        //public ActionResult GetBankReconciliation(string acheadid="325", string from="01 Jan 2016", string to="31 Dec 2016")
        //{

        //    int vacheadid = 0;

        //    if (acheadid != null)
        //    {
        //        vacheadid = Convert.ToInt32(acheadid);
        //    }
        //    else
        //    {
        //        vacheadid = 0;
        //    }

        //    DateTime frm = Convert.ToDateTime(from);
        //    DateTime dto = Convert.ToDateTime(to);

        //    var data = context.GetBankReconciliationOutStandings(vacheadid, frm, dto);



        //    List<BankReconcilVM> lst = new List<BankReconcilVM>();



        //    foreach (var item in data)
        //    {
        //        BankReconcilVM v = new BankReconcilVM();

        //        v.AcJournalID = item.AcJournalID;
        //        v.BankName = item.BankName;
        //        v.ChequeNo = item.ChequeNo;
        //        if (item.ChequeDate.HasValue)
        //            v.ChequeDate = item.ChequeDate.Value;
        //        v.PartyName = item.PartyName;
        //        v.StatusReconciled = false;
        //        v.Remarks = item.Remarks;
        //        v.VoucherDate = item.TransDate.Value;
        //        v.Amount = item.Amount.Value;

        //        v.IsSelected = false;
        //        lst.Add(v);


        //    }

        //    return View(lst.ToList());

        //}



        #endregion BankReconciliation



        #region PDCTransaction

        public ActionResult IndexPDCTransaction()
        {

            var data = context.AcMemoJournalMasterSelectAll(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();

            return View(data);
        }

        public ActionResult CreatePDCTransaction()
        {
            var transtypes = new SelectList(new[] 
                                        {
                                            new { ID = "1", trans = "Receipt" },
                                            new { ID = "2", trans = "Payment" },
                                           
                                        },
                                   "ID", "trans", 1);

            ViewBag.transtypes = transtypes;

            //ViewBag.heads = context.AcHeadSelectForBank(Convert.ToInt32(Session["AcCompanyID"].ToString()));

            ViewBag.heads = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            ViewBag.headsreceived = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));


            return View();

        }


        [HttpPost]
        public ActionResult CreatePDCTransaction(PDCVM pdctrans)
        {

            string StatusTrans = "";

            if (pdctrans.transtype == 1)
                StatusTrans = "R";
            else
                StatusTrans = "P";

            //string Vouchern = (from c in context.AcMemoJournalMasters select c).FirstOrDefault();
            //string vno = "";
            //if (Vouchern == "")
            //{
            //    vno = "PD-" + 1;
            //}
            //else
            //{
            //    vno = "PD-" + Convert.ToInt32(Vouchern) + 1;
            //}

            AcMemoJournalMaster acm = new AcMemoJournalMaster();

            acm.VoucherNo = "PD-125";
            acm.TransDate = pdctrans.transdate;
            acm.AcFinancialYearID = Convert.ToInt32(Session["fyearid"].ToString());
            acm.VoucherType = pdctrans.TransactionType;
            acm.StatusDelete = false;
            acm.Remarks = pdctrans.remarks;
            acm.UserID = Convert.ToInt32(Session["UserID"].ToString());
            acm.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());

            context.AcMemoJournalMasters.Add(acm);
            context.SaveChanges();


            if (pdctrans.chequeno.Length > 0)
            {
                AcMemoBankDetail acmbank = new AcMemoBankDetail();
                acmbank.AcMemoBankDetailID = GetMaxAcMemoBankDetailNumber();
                acmbank.AcMemoJournalID = acm.AcMemoJournalID;
                acmbank.BankName = pdctrans.bankname;
                acmbank.ChequeNo = pdctrans.chequeno;
                acmbank.ChequeDate = pdctrans.chequedate;
                acmbank.PartyName = pdctrans.partyname;
                acmbank.StatusTrans = StatusTrans;

                context.AcMemoBankDetails.Add(acmbank);
                context.SaveChanges();

            }

            decimal total = 0;
            for (int i = 0; i < pdctrans.AcJMDetailVM.Count; i++)
            {
                total = total + pdctrans.AcJMDetailVM[i].Amt;
            }



            AcMemoJournalDetail acmd = new AcMemoJournalDetail();
            acmd.AcMemoJournalID = acm.AcMemoJournalID;
            acmd.AcHeadID = pdctrans.AcHead;
            if (StatusTrans == "P")
            {
                acmd.Amount = -total;
            }
            else
            {
                acmd.Amount = total;
            }
            acmd.BranchID = Convert.ToInt32(Session["AcCompanyID"].ToString());

            context.AcMemoJournalDetails.Add(acmd);
            context.SaveChanges();


            for (int i = 0; i < pdctrans.AcJMDetailVM.Count; i++)
            {

                AcMemoJournalDetail a = new AcMemoJournalDetail();
                a.AcMemoJournalID = acm.AcMemoJournalID;
                a.AcHeadID = pdctrans.AcJMDetailVM[i].AcHeadID;

                if (StatusTrans == "P")
                {
                    a.Amount = pdctrans.AcJMDetailVM[i].Amt;
                }
                else
                {
                    a.Amount = -pdctrans.AcJMDetailVM[i].Amt;
                }
            
                a.Remarks = pdctrans.AcJMDetailVM[i].Rem;
                a.BranchID = Convert.ToInt32(Session["AcCompanyID"].ToString());

                context.AcMemoJournalDetails.Add(a);
                context.SaveChanges();

            }

            ViewBag.SuccessMsg = "You have successfully added Record";
            return View("IndexPDCTransaction", context.AcMemoJournalMasterSelectAll(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList());


        }


        public JsonResult GetAcMemoJDeatils(int id)
        {
            var acjlist = (from c in context.AcMemoJournalDetails where c.AcMemoJournalID == id orderby c.AcMemoJournalDetailID ascending select c).Skip(1).ToList();
            List<AcMemoJournalDetailVM> AcJDetailVM = new List<AcMemoJournalDetailVM>();
            foreach (var item in acjlist)
            {
                AcMemoJournalDetailVM v = new AcMemoJournalDetailVM();
                string x = (from a in context.AcHeads where a.AcHeadID == item.AcHeadID select a.AcHead1).FirstOrDefault();

                v.AcHeadID = item.AcHeadID.Value;
                v.AcHead = x;

                if (item.Amount < 0)
                {
                    v.Amt = (-item.Amount.Value);
                }
                else
                {
                    v.Amt = item.Amount.Value;
                }
                v.Rem = item.Remarks;
                v.AcMemoDetailID = item.AcMemoJournalDetailID;
                AcJDetailVM.Add(v);
            }

            return Json(AcJDetailVM, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditPDC(int id)
        {
            var transtypes = new SelectList(new[] 
                                        {
                                            new { ID = "1", trans = "Receipt" },
                                            new { ID = "2", trans = "Payment" },
                                           
                                        },
                                 "ID", "trans", 1);

            ViewBag.transtypes = transtypes;

            //ViewBag.heads = context.AcHeadSelectForBank(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            ViewBag.heads = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            ViewBag.headsreceived = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));


            PDCVM v = new PDCVM();
            AcMemoJournalMaster ajm = context.AcMemoJournalMasters.Find(id);
            AcMemoBankDetail acb = (from a in context.AcMemoBankDetails where a.AcMemoJournalID == id select a).FirstOrDefault();
            v.AcHead = (from c in context.AcMemoJournalDetails where c.AcMemoJournalID == ajm.AcMemoJournalID select c.AcHeadID).FirstOrDefault().Value;
            v.AcJournalID = ajm.AcMemoJournalID;
            v.AcBankDetailID = acb.AcMemoBankDetailID;
            if (acb.StatusTrans == "P")
            {
                v.transtype = 2;
            }
            else
            {
                v.transtype = 1;
            }
            v.transdate = ajm.TransDate.Value;
            v.remarks = ajm.Remarks;

            v.bankname = acb.BankName;
            v.chequeno = acb.ChequeNo;
            v.chequedate = acb.ChequeDate.Value;
            v.partyname = acb.PartyName;

            v.VoucherNo = ajm.VoucherNo;
            v.TransactionType = ajm.VoucherType;

            return View(v);

        }


        [HttpPost]
        public ActionResult EditPDC(PDCVM pdctrans)
        {
            string StatusTrans = "";

            if (pdctrans.transtype == 1)
                StatusTrans = "R";
            else
                StatusTrans = "P";


            AcMemoJournalMaster acm = new AcMemoJournalMaster();
            acm.AcMemoJournalID = pdctrans.AcJournalID;
            acm.AcJournalID = null;
            acm.VoucherNo = "PD-125";
            acm.TransDate = pdctrans.transdate;
            acm.AcFinancialYearID = Convert.ToInt32(Session["fyearid"].ToString());
            acm.VoucherType = pdctrans.TransactionType;
            acm.StatusDelete = false;
            acm.Remarks = pdctrans.remarks;
            acm.UserID = Convert.ToInt32(Session["UserID"].ToString());
            acm.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());

            context.Entry(acm).State = EntityState.Modified;
            context.SaveChanges();


            if (pdctrans.chequeno.Length > 0)
            {
                AcMemoBankDetail acmbank = new AcMemoBankDetail();
                acmbank.AcMemoBankDetailID = pdctrans.AcBankDetailID;
                acmbank.AcMemoJournalID = acm.AcMemoJournalID;
                acmbank.BankName = pdctrans.bankname;
                acmbank.ChequeNo = pdctrans.chequeno;
                acmbank.ChequeDate = pdctrans.chequedate;
                acmbank.PartyName = pdctrans.partyname;
                acmbank.StatusTrans = StatusTrans;

                context.Entry(acmbank).State = EntityState.Modified;
                context.SaveChanges();

            }

            var x = (from c in context.AcMemoJournalDetails where c.AcMemoJournalID == acm.AcMemoJournalID select c).ToList();

            foreach (var i in x)
            {
                context.AcMemoJournalDetails.Remove(i);
                context.SaveChanges();
            }

            decimal total = 0;
            for (int i = 0; i < pdctrans.AcJMDetailVM.Count; i++)
            {
                total = total + pdctrans.AcJMDetailVM[i].Amt;
            }


            AcMemoJournalDetail acmd = new AcMemoJournalDetail();
          
            acmd.AcMemoJournalID = acm.AcMemoJournalID;
            acmd.AcHeadID = pdctrans.AcHead;
            acmd.Amount = total * (-1);
            acmd.BranchID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            acmd.Remarks = acm.Remarks;
            context.AcMemoJournalDetails.Add(acmd);
            context.SaveChanges();

           

            for (int i = 0; i < pdctrans.AcJMDetailVM.Count; i++)
            {

                AcMemoJournalDetail a = new AcMemoJournalDetail();
            
                a.AcMemoJournalID = acm.AcMemoJournalID;
                a.AcHeadID = pdctrans.AcJMDetailVM[i].AcHeadID;
                a.Amount = pdctrans.AcJMDetailVM[i].Amt;
                a.Remarks = pdctrans.AcJMDetailVM[i].Rem;
                a.BranchID = Convert.ToInt32(Session["AcCompanyID"].ToString());

                context.AcMemoJournalDetails.Add(a);
                context.SaveChanges();

            }

            ViewBag.SuccessMsg = "You have successfully updated Record";
            return View("IndexPDCTransaction", context.AcMemoJournalMasterSelectAll(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList());
        }

        public int GetMaxAcMemoBankDetailNumber()
        {

            var query = context.AcMemoBankDetails.OrderByDescending(item => item.AcMemoBankDetailID).FirstOrDefault();

            if (query == null)
            {
                return 1;
            }
            else
            {
                return query.AcMemoBankDetailID + 1;
            }


        }
        #endregion PDCTransaction


        #region PDCOutstandings

        public ActionResult IndexPDCOutstandings()
        {

            return View();
        }


        public JsonResult GetPDCOutstandings(DateTime iMatureDate)
        {
            List<PDCOutstandingVM> objPDCOutstandingVMList = new List<PDCOutstandingVM>();
            var pdcreminder = context.GetPDCReminder(iMatureDate, 1, Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
            foreach (var item in pdcreminder)
            {
                PDCOutstandingVM objPDCOutstandingVM = new PDCOutstandingVM();
                objPDCOutstandingVM.AcHead = item.AcHead;
                objPDCOutstandingVM.Amount = item.Amount.Value;
                objPDCOutstandingVM.VoucherNo = item.VoucherNo;
                objPDCOutstandingVM.VoucherDate = item.TransDate.Value;
                objPDCOutstandingVM.ChequeNo = item.ChequeNo;
                objPDCOutstandingVM.ChequeDate = item.ChequeDate.Value;
                objPDCOutstandingVM.AcMemoJournalID = item.AcMemoJournalID;
                objPDCOutstandingVM.IsSelected = false;
                objPDCOutstandingVMList.Add(objPDCOutstandingVM);
               
            }

            var view = this.RenderPartialView2("ucPDCOutstandings", objPDCOutstandingVMList);
            return new JsonResult
            {
                Data = new
                {
                    success = true,
                    view = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public ActionResult IndexPDCOutstandings(List<PDCOutstandingVM> iPDCOutstandingVM)
        {
            foreach (var item in iPDCOutstandingVM)
            {

            }
            return RedirectToAction("IndexPDCOutstandings");
        }


        #endregion PDCOutstandings



        //public JsonResult YearEndProcess()
        //{
        //    YearEndProcessVM v = new YearEndProcessVM();
        //    v.CurrentFYearFrom = (from c in context.AcFinancialYears where c.AcFinancialYearID == Convert.ToInt32(Session["fyearid"].ToString()) select c.AcFYearFrom).FirstOrDefault().Value;
        //    v.CurrentFYearTo = v.CurrentFYearFrom = (from c in context.AcFinancialYears where c.AcFinancialYearID == Convert.ToInt32(Session["fyearid"].ToString()) select c.AcFYearTo).FirstOrDefault().Value;

        //    v.NewFYearFrom = v.CurrentFYearFrom.AddDays(1);
        //    v.NewFYearTo = v.CurrentFYearTo.AddDays(1);

        //    v.Reference = v.CurrentFYearFrom.AddYears(1).Year.ToString() + "-" + v.CurrentFYearTo.AddYears(1).Year.ToString();

        //    return Json(v, JsonRequestBehavior.AllowGet);

        //}




        public ActionResult YearEndProcess()
        {
            ViewBag.currentFyearFrom = Convert.ToDateTime(Session["FyearFrom"].ToString()).ToString("dd/MM/yyyy");
            ViewBag.currentFyearTo = Convert.ToDateTime(Session["FyearTo"].ToString()).ToString("dd/MM/yyyy");

            return View();
        }

       
        public JsonResult GetNewFYear(string cFyearFrom,string cFyearTo)
        {
            YearEndProcessVM v = new YearEndProcessVM();
          
            v.CurrentFYearFrom = cFyearFrom;
            v.CurrentFYearTo = cFyearTo;

            DateTime tnewfyear = Convert.ToDateTime(cFyearFrom).AddYears(1);


            v.NewFYearFrom = tnewfyear.ToString("dd/MM/yyyy");

            DateTime tnewtyear = Convert.ToDateTime(cFyearTo).AddYears(1);
            v.NewFYearTo = tnewtyear.ToString("dd/MM/yyyy");
            v.Reference = tnewfyear.Year + "-" + tnewtyear.Year;

            return Json(v, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindOpenHead(string NewYearFrom, string NewYearTo,string ref1)
        {
                SHIPPING_FinalEntities context1=new SHIPPING_FinalEntities();
                int NewFYearID=0;
                AcFinancialYear a=(from c in context1.AcFinancialYears where c.ReferenceName==ref1 select c).FirstOrDefault();

                if (a != null)
                {
                    NewFYearID = a.AcFinancialYearID;
                }

              //bool result = ESS.SOP.BLL.AcFinancialYear.SaveNewFinancialYear(Convert.ToInt32(Session["fyearid"]), Convert.ToInt32(Session["AcCompanyID"]), Convert.ToDateTime(dpNewFyearFrom.SelectedDate), Convert.ToDateTime(dpNewFyearTo.SelectedDate), txtReferenceName.Text, Convert.ToInt32(Session["userid"]), newFinancialYearID);

              //int res = context1.SaveFinancialYear(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString()), Convert.ToDateTime(NewYearFrom), Convert.ToDateTime(NewYearTo), ref1, Convert.ToInt32(Session["UserdID"].ToString()), NewFYearID);
                int res = 10;
              return Json( res, JsonRequestBehavior.AllowGet);
        }


    }




   
}
public static class MvcHelpers
{
    public static string RenderPartialView2(this Controller controller, string viewName, object model)
    {
        if (string.IsNullOrEmpty(viewName))
            viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

        controller.ViewData.Model = model;
        using (var sw = new StringWriter())
        {
            ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
            var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
            viewResult.View.Render(viewContext, sw);

            return sw.GetStringBuilder().ToString();
        }
    }

  
}