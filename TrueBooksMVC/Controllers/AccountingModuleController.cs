using System;
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
    [SessionExpire]
    [Authorize]
    public class AccountingModuleController : Controller
    {
        SHIPPING_FinalEntities context = new SHIPPING_FinalEntities();
        SourceMastersModel objectSourceModel = new SourceMastersModel();




        #region Masters
        //Methods For Account Category
        public IEnumerable<AcGroupModel> GetAllAcGroupsByBranch(int Branchid)
        {
            var parents = (from d in context.AcGroups
                           where d.AcBranchID == Branchid
                           select d).ToList();
            var accategory = (from d in context.AcCategories
                              select d).ToList();
            IEnumerable<AcGroupModel> data = (from d in context.AcGroups
                                              where d.AcBranchID == Branchid
                                              select
new AcGroupModel()
{

    AcGroupID = d.AcGroupID,
    AcGroup = d.AcGroup1,
    AcClass = d.AcClass,
    AcType = d.AcType,
    BranchID = d.AcBranchID,
    GroupCode = d.GroupCode,
    GroupOrder = d.GroupOrder,
    ParentID = d.ParentID,
    UserID = d.UserID,
    AcCategoryID = d.AcCategoryID

}).ToList();
            foreach (var item in data)
            {
                var ParentNode = parents.Where(d => d.AcGroupID == item.ParentID).FirstOrDefault();
                if (ParentNode != null)
                {
                    item.ParentNode = ParentNode.AcGroup1;
                }
                var accat = accategory.Where(d => d.AcCategoryID == item.AcCategoryID).FirstOrDefault();
                if (accat != null)
                {
                    item.AcCategory = accat.AcCategory1;
                }
            }
            return data;
        }
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


                    ViewBag.ErrorMsg = "Transaction in Use. Can not Delete";


                }
            }

            return View("IndexAcCategory", context.AcCategorySelectAll());

        }



        //Methods for Account Groups

        public ActionResult IndexAcGroup()
        {

            var x = GetAllAcGroupsByBranch(Convert.ToInt32(Session["branchid"].ToString()));
            return View(x);
        }

        public ActionResult CreateAcGroup(int frmpage)
        {
            Session["AcgroupPage"] = frmpage;
            ViewBag.Category = context.AcCategorySelectAll();
            var branchid = Convert.ToInt32(Session["branchid"].ToString());
            ViewBag.AccountType = (from d in context.AcTypes where d.BranchId == branchid select d).ToList();
            ViewBag.groups = GetAllAcGroupsByBranch(Convert.ToInt32(Session["branchid"].ToString()));

            int count = (from c in context.AcCompanies select c).ToList().Count();
            ViewBag.IsAuto = count;

            return View();
        }


        public bool GetDuplicateGroup(int AcgroupId, int ParentId, int CategoryID, string name)
        {
            var data = (from d in context.AcGroups where d.AcGroupID != AcgroupId && d.AcGroup1.ToLower() == name.ToLower() && d.AcCategoryID == CategoryID && d.ParentID == ParentId select d).FirstOrDefault();
            if (data == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpGet]
        public JsonResult GetGroupsByID(int Category)
        {
            var groups = context.AcGroupSelectByCategoryID(Category, Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
            return Json(groups, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAcCategoryByParentid(int parentId)
        {
            var groups = (from d in context.AcGroups where d.AcGroupID == parentId select d).FirstOrDefault();
            return Json(groups.AcCategoryID, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateAcGroup(AcGroupVM c)
        {

            var isexist = GetDuplicateGroup(0, c.AcGroup, c.AcCategoryID, c.subgroup);
            if (isexist == true)
            {
                var acgrps = (from d in context.AcGroups orderby d.AcGroupID descending select d.AcGroupID).FirstOrDefault();
                var maxid = acgrps + 1;
                var actype = Getactype(c.AcTypeId);


                if (c.AcGroup == 0)
                {

                    var acgroup = new AcGroup();
                    acgroup.AcGroupID = maxid;
                    acgroup.AcCategoryID = actype.AcCategoryId;
                    acgroup.AcGroup1 = c.subgroup;
                    acgroup.AcBranchID = Convert.ToInt32(Session["branchid"].ToString());
                    acgroup.ParentID = c.AcGroup;
                    acgroup.UserID = Convert.ToInt32(Session["UserID"].ToString());
                    acgroup.StaticEdit = 0;
                    acgroup.StatusHide = false;
                    acgroup.GroupCode = c.GroupCode;
                    acgroup.AcTypeId = c.AcTypeId;
                    context.AcGroups.Add(acgroup);
                    context.SaveChanges();


                    //context.AcGroupInsert(c.AcCategoryID, c.subgroup, null, Convert.ToInt32(Session["AcCompanyID"].ToString()), Convert.ToInt32(Session["UserID"].ToString()), c.IsGroupCodeAuto, c.GroupCode);
                }
                else
                {
                    var acgroup = new AcGroup();
                    acgroup.AcGroupID = maxid;
                    acgroup.AcCategoryID = actype.AcCategoryId;
                    acgroup.AcGroup1 = c.subgroup;
                    acgroup.AcBranchID = Convert.ToInt32(Session["branchid"].ToString());
                    acgroup.ParentID = c.AcGroup;
                    acgroup.UserID = Convert.ToInt32(Session["UserID"].ToString());
                    acgroup.StaticEdit = 0;
                    acgroup.StatusHide = false;
                    acgroup.GroupCode = c.GroupCode;
                    acgroup.AcTypeId = c.AcTypeId;
                    context.AcGroups.Add(acgroup);
                    context.SaveChanges();
                    // context.AcGroupInsert(c.AcCategoryID, c.subgroup, c.AcGroup, Convert.ToInt32(Session["AcCompanyID"].ToString()), Convert.ToInt32(Session["UserID"].ToString()), c.IsGroupCodeAuto, c.GroupCode);
                }
                //context.AcGroupInsert(c.AcCategoryID, c.subgroup, c.ParentID, Convert.ToInt32(Session["AcCompanyID"].ToString()), Convert.ToInt32(Session["UserID"].ToString()), c.IsGroupCodeAuto, c.GroupCode);

                //context.AcGroupInsert(c.AcCategoryID, c.AcGroup1, null, Convert.ToInt32(Session["AcCompanyID"].ToString()), Convert.ToInt32(Session["UserID"].ToString()), null, c.GroupCode);
                var acgroupfrompage = Convert.ToInt32(Session["AcgroupPage"].ToString());
                if (acgroupfrompage == 1)
                {
                    ViewBag.SuccessMsg = "You have successfully added Account Group";
                    return View("IndexAcGroup", GetAllAcGroupsByBranch(Convert.ToInt32(Session["branchid"].ToString())));
                }
                else
                {
                    ViewBag.AcgroupId = maxid;
                    return RedirectToAction("CreateAcHead", "AccountingModule", new { frmpage = Convert.ToInt32(Session["AcheadPage"]) });

                }

            }
            else
            {
                var branchid = Convert.ToInt32(Session["branchid"].ToString());
                ViewBag.AccountType = (from d in context.AcTypes where d.BranchId == branchid select d).ToList();

                ViewBag.ErrorMsg = "Account Group already exists !!";
                return View(c);
            }


        }

        public ActionResult EditAcGroup(int id)
        {
            var branchid = Convert.ToInt32(Session["branchid"].ToString());
            ViewBag.AccountType = (from d in context.AcTypes where d.BranchId == branchid select d).ToList();

            ViewBag.Category = context.AcCategorySelectAll();
            var groups = GetAllAcGroupsByBranch(Convert.ToInt32(Session["branchid"].ToString()));
            ViewBag.groups = groups.Where(d => d.AcGroupID != id).ToList();
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
                v.AcTypeId = data.AcTypeId;
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
        public AcType Getactype(int? id)
        {
            var actype = (from d in context.AcTypes where d.Id == id select d).FirstOrDefault();
            return actype;
        }
        [HttpPost]
        public ActionResult EditAcGroup(AcGroupVM c)
        {
            var groups = GetAllAcGroupsByBranch(Convert.ToInt32(Session["branchid"].ToString()));

            var isexist = GetDuplicateGroup(c.AcGroupID, c.AcGroup, c.AcCategoryID, c.subgroup);
            var actype = Getactype(c.AcTypeId);

            if (isexist == true)
            {

                var acgroup = (from d in context.AcGroups where d.AcGroupID == c.AcGroupID select d).FirstOrDefault();
                acgroup.ParentID = c.AcGroup;
                acgroup.AcGroup1 = c.subgroup;
                acgroup.AcTypeId = c.AcTypeId;
                acgroup.AcCategoryID = actype.AcCategoryId;
                acgroup.GroupCode = c.GroupCode;
                context.Entry(acgroup).State = EntityState.Modified;
                context.SaveChanges();
                //context.AcGroupUpdate(c.AcGroupID, c.AcGroup, c.subgroup, c.AcCategoryID, 0, c.GroupCode);

                ViewBag.SuccessMsg = "You have successfully updated Account Group";
                return View("IndexAcGroup", GetAllAcGroupsByBranch(Convert.ToInt32(Session["branchid"].ToString())));
            }
            else
            {
                ViewBag.Category = context.AcCategorySelectAll();
                ViewBag.groups = groups.Where(d => d.AcGroupID != c.AcGroupID).ToList();
                var branchid = Convert.ToInt32(Session["branchid"].ToString());
                ViewBag.AccountType = (from d in context.AcTypes where d.BranchId == branchid select d).ToList();

                ViewBag.ErrorMsg = "Account Group already exists !!";
                return View(c);
            }
        }


        public ActionResult DeleteAcGroup(int id)
        {
            AcGroup c = (from x in context.AcGroups where x.AcGroupID == id select x).FirstOrDefault();
            if (c != null)
            {
                try
                {
                    var x = (from a in context.AcHeads where a.AcGroupID == id select a).FirstOrDefault();
                    var p = (from a in context.AcGroups where a.ParentID == id select a).FirstOrDefault();
                    if (x != null)
                    {
                        ViewBag.ErrorMsg = "Transaction in Use. Can not Delete";
                        throw new Exception();

                    }
                    else if (p != null)
                    {
                        ViewBag.ErrorMsg = "Transaction in Use. Can not Delete";
                        throw new Exception();

                    }
                    else
                    {
                        context.AcGroups.Remove(c);
                        context.SaveChanges();


                        ViewBag.SuccessMsg = "You have successfully deleted Account Group";
                        return RedirectToAction("IndexAcGroup", GetAllAcGroupsByBranch(Convert.ToInt32(Session["branchid"].ToString())));

                    }

                }
                catch (Exception ex)
                {





                }
            }

            return View("IndexAcGroup", GetAllAcGroupsByBranch(Convert.ToInt32(Session["AcCompanyID"].ToString())));
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

        public ActionResult CreateAcHead(int frmpage)
        {
            Session["AcheadPage"] = frmpage;
            ViewBag.groups = GetAllAcGroupsByBranch(Convert.ToInt32(Session["branchid"].ToString()));
            return View();
        }

        [HttpPost]
        public ActionResult CreateAcHead(AcHead a)
        {

            int id = 0;
            AcHead x = context.AcHeads.OrderByDescending(item => item.AcHeadID).FirstOrDefault();
            if (x == null)
            {

                id = 1;
            }
            else
            {
                id = x.AcHeadID + 1;
            }
            context.AcHeadInsert(id, a.AcHeadKey, a.AcHead1, a.AcGroupID, Convert.ToInt32(Session["AcCompanyID"].ToString()), a.Prefix);
            var acheadfrompage = Convert.ToInt32(Session["AcheadPage"].ToString());
            if (acheadfrompage == 1)
            {
                ViewBag.SuccessMsg = "You have successfully created Account Head.";
                return View("IndexAcHead", context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())));
            }
            else
            {
                //return View("IndexAcHead", context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())));
                return RedirectToAction("Create", "RevenueType", new { acheadid = id });
            }
        }

        public ActionResult EditAcHead(int id)
        {
            var result = context.AcHeadSelectByID(id);
            ViewBag.groups = GetAllAcGroupsByBranch(Convert.ToInt32(Session["branchid"].ToString()));
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
            var analysisCode = context.AnalysisHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            var codeIsexist = analysisCode.Where(d => d.AnalysisCode == a.AnalysisCode).FirstOrDefault();
            if (codeIsexist == null)
            {
                context.AnalysisHeadInsert(a.AnalysisCode, a.AnalysisHead1, a.AnalysisGroupID, Convert.ToInt32(Session["AcCompanyID"].ToString()));
                ViewBag.SuccessMsg = "You have successfully added Analysis Head.";
                return View("IndexExpenseAnalysisHead", context.AnalysisHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())));
            }
            else
            {
                ViewBag.groups = context.AnalysisGroupSelectAll().ToList();
                ViewBag.ErrorMsg = "Analysis Code Already Exist.";
                return View();
            }
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
            var analysisCode = context.AnalysisHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            var codeIsexist = analysisCode.Where(d => d.AnalysisCode == a.AnalysisCode && d.AnalysisHeadID != a.AnalysisHeadID).FirstOrDefault();
            if (codeIsexist == null)
            {
                context.AnalysisHeadUpdate(a.AnalysisHeadID, a.AnalysisCode, a.AnalysisHead, a.AnalysisGroupID);

                ViewBag.SuccessMsg = "You have successfully updated Analysis Head.";
                return View("IndexExpenseAnalysisHead", context.AnalysisHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())));
            }
            else
            {
                var result = context.AnalysisHeadSelectByID(a.AnalysisHeadID);
                ViewBag.groups = context.AnalysisGroupSelectAll().ToList();
                ViewBag.ErrorMsg = "Analysis Code Already Exist.";

                return View(result.FirstOrDefault());
            }
        }



        //Methods for AcHeadAssign

        //public ActionResult IndexAcHeadAssign()
        //{
        //    return View(context.AcHeadAssignSelectAll());
        //}





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

            var paymentterms = (from d in context.PaymentTerms select d).ToList();
            var paytypes1 = new SelectList(paymentterms, "PaymentTermID", "PaymentTerm1");
            ViewBag.transtypes = transtypes;
            ViewBag.paytypes = paytypes;
            //ViewBag.heads = context.AcHeadSelectForCash(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            //ViewBag.headsreceived = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));


            ViewBag.heads = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            ViewBag.headsreceived = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));




            return View();
        }



        public JsonResult ExpAllocation(decimal amount, int acheadid)
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

            int MaxId = 0;
            MaxId = (from c in context.AcJournalMasters orderby c.ID descending select c.ID).FirstOrDefault();

            AcJournalMaster ajm = new AcJournalMaster();
            ajm.AcJournalID = max + 1;
            ajm.VoucherNo = (voucherno + 1).ToString();
            ajm.TransDate = v.transdate;
            ajm.TransType = Convert.ToInt16(v.transtype);
            if (v.transtype == 1)
            {
                v.TransactionNo = "RE" + (MaxId + 1).ToString().PadLeft(7, '0');
                //new { ID = "1", trans = "Receipt" },
                // new { ID = "2", trans = "Payment" },
            }
            else if (v.transtype == 2)
            {
                v.TransactionNo = "PA" + (MaxId + 1).ToString().PadLeft(7, '0');
            }
            ajm.TransactionNo = v.TransactionNo;
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

            if (v.chequeno != null)
            {

                var bankdetailid = (from c in context.AcBankDetails orderby c.AcBankDetailID descending select c.AcBankDetailID).FirstOrDefault();


                AcBankDetail acbankDetails = new AcBankDetail();
                acbankDetails.AcBankDetailID = bankdetailid + 1;
                acbankDetails.AcJournalID = ajm.AcJournalID;
                acbankDetails.BankName = v.bankname;
                acbankDetails.ChequeDate = v.chequedate;
                acbankDetails.ChequeNo = v.chequeno;
                acbankDetails.PartyName = v.partyname;
                acbankDetails.StatusTrans = StatusTrans;
                acbankDetails.StatusReconciled = false;
                if (acbankDetails.BankName == null)
                {
                    acbankDetails.BankName = "B";
                }
                if (acbankDetails.PartyName == null)
                {
                    acbankDetails.PartyName = "P";
                }
                DAL.InsertOrUpdateAcBankDetails(acbankDetails, 0);
            }

            decimal TotalAmt = 0;

            for (int i = 0; i < v.AcJDetailVM.Count; i++)
            {

                TotalAmt = TotalAmt + Convert.ToDecimal(v.AcJDetailVM[i].Amt);

            }


            AcJournalDetail ac = new AcJournalDetail();
            int maxAcJDetailID = 0;
            maxAcJDetailID = (from c in context.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();

            ac.AcJournalDetailID = maxAcJDetailID + 1;
            ac.AcJournalID = ajm.AcJournalID;
            ac.AcHeadID = v.SelectedAcHead;
            if (StatusTrans == "P")
            {
                ac.Amount = -(TotalAmt);
            }
            else
            {
                ac.Amount = TotalAmt;
            }
            ac.Remarks = v.AcJDetailVM[0].Rem;
            ac.BranchID = Convert.ToInt32(Session["branchid"].ToString());

            context.AcJournalDetails.Add(ac);
            context.SaveChanges();


            //int maxAcJDetailID = 0;

            for (int i = 0; i < v.AcJDetailVM.Count; i++)
            {
                AcJournalDetail acJournalDetail = new AcJournalDetail();

                maxAcJDetailID = (from c in context.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();

                acJournalDetail.AcJournalDetailID = maxAcJDetailID + 1;
                acJournalDetail.AcHeadID = v.AcJDetailVM[i].AcHeadID;

                acJournalDetail.BranchID = Convert.ToInt32(Session["branchid"]);
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
                //  context.Entry(acJournalDetail).State = EntityState.Added;
                context.SaveChanges();
                context.Entry(acJournalDetail).State = EntityState.Detached;

                if (v.AcJDetailVM[i].AcExpAllocationVM != null)
                {
                    for (int j = 0; j < v.AcJDetailVM[i].AcExpAllocationVM.Count; j++)
                    {
                        AcAnalysisHeadAllocation objAcAnalysisHeadAllocation = new AcAnalysisHeadAllocation();
                        objAcAnalysisHeadAllocation.AcjournalDetailID = acJournalDetail.AcJournalDetailID;
                        objAcAnalysisHeadAllocation.AnalysisHeadID = v.AcJDetailVM[i].AcExpAllocationVM[j].AcHead;
                        objAcAnalysisHeadAllocation.Amount = v.AcJDetailVM[i].AcExpAllocationVM[j].ExpAllocatedAmount;
                        context.AcAnalysisHeadAllocations.Add(objAcAnalysisHeadAllocation);
                        context.SaveChanges();
                        context.Entry(objAcAnalysisHeadAllocation).State = EntityState.Detached;
                    }
                }

            }

            ViewBag.SuccessMsg = "You have successfully added Record";
            return View("IndexAcBook", context.AcJournalMasterSelectAll(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString())));

        }

        public JsonResult AcBookDetails(int DetailId)
        {
            var lstAcJournalDetails = DAL.GetAcJournalDetails(DetailId);
            return Json(lstAcJournalDetails, JsonRequestBehavior.AllowGet);
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

            AcJournalMaster ajm = new AcJournalMaster();
            ajm.TransactionNo = v.TransactionNo;
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

            //ajm.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            ajm.AcCompanyID = Convert.ToInt32(Session["AcCompanyID"].ToString());
            ajm.Reference = v.reference;

            context.Entry(ajm).State = EntityState.Modified;
            context.SaveChanges();

            if (v.TransactionType == "CBR" || v.TransactionType == "BKR")
                StatusTrans = "R";
            else if (v.TransactionType == "CBP" || v.TransactionType == "BKP")
                StatusTrans = "P";
            int maxBankDetailID = 0;
            int isexistbankdetail = 0;

            if (v.chequeno != null)
            {

                if (v.AcBankDetailID > 0)
                {
                    maxBankDetailID = v.AcBankDetailID;
                    isexistbankdetail = 1;
                }
                else
                {
                    var bankdetailid = (from c in context.AcBankDetails orderby c.AcBankDetailID descending select c.AcBankDetailID).FirstOrDefault();
                    v.AcBankDetailID = bankdetailid + 1;
                    isexistbankdetail = 0;

                }
                AcBankDetail acbankDetails = new AcBankDetail();
                acbankDetails.AcBankDetailID = v.AcBankDetailID;
                acbankDetails.BankName = v.bankname;
                acbankDetails.ChequeDate = v.chequedate;
                acbankDetails.ChequeNo = v.chequeno;
                acbankDetails.PartyName = v.partyname;
                acbankDetails.AcJournalID = ajm.AcJournalID;
                acbankDetails.StatusTrans = StatusTrans;
                acbankDetails.StatusReconciled = false;
                if (acbankDetails.BankName == null)
                {
                    acbankDetails.BankName = "B";
                }
                if (acbankDetails.PartyName == null)
                {
                    acbankDetails.PartyName = "P";
                }
                DAL.InsertOrUpdateAcBankDetails(acbankDetails, isexistbankdetail);
            }
            else
            {

            }

            decimal TotalAmt = 0;

            for (int i = 0; i < v.AcJDetailVM.Count; i++)
            {
                TotalAmt = TotalAmt + Convert.ToDecimal(v.AcJDetailVM[i].Amt);
            }
            var ac = (from c in context.AcJournalDetails where c.AcJournalID == ajm.AcJournalID select c).FirstOrDefault();
            ac.AcJournalDetailID = ac.AcJournalDetailID;
            ac.AcJournalID = ajm.AcJournalID;
            ac.AcHeadID = v.SelectedAcHead;
            if (StatusTrans == "P")
            {
                ac.Amount = -(TotalAmt);
            }
            else
            {
                ac.Amount = TotalAmt;
            }
            ac.Remarks = v.AcJDetailVM[0].Rem;
            ac.BranchID = Convert.ToInt32(Session["branchid"].ToString());

            context.Entry(ac).State = EntityState.Modified;
            context.SaveChanges();

            int maxAcJDetailID = 0;

            for (int i = 0; i < v.AcJDetailVM.Count; i++)
            {
                AcJournalDetail acJournalDetail = new AcJournalDetail();
                int IdExists = 0;
                if (v.AcJDetailVM[i].AcJournalDetID > 0)
                {
                    //  IdExists = (from c in context.AcJournalDetails where c.AcJournalDetailID == v.AcJDetailVM[i].AcJournalDetID select c.AcJournalDetailID).FirstOrDefault();
                    IdExists = v.AcJDetailVM[i].AcJournalDetID;
                }
                //  AcJournalDetID
                if (IdExists > 0)
                {
                    acJournalDetail.AcJournalDetailID = v.AcJDetailVM[i].AcJournalDetID;
                }
                else
                {
                    maxAcJDetailID = (from c in context.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();
                    acJournalDetail.AcJournalDetailID = maxAcJDetailID + 1;
                }
                acJournalDetail.AcHeadID = v.AcJDetailVM[i].AcHeadID;
                acJournalDetail.BranchID = Convert.ToInt32(Session["branchid"]);
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
                if (acJournalDetail.AnalysisHeadID == null)
                {
                    acJournalDetail.AnalysisHeadID = 0;
                }
                if (IdExists > 0)
                {
                    DAL.UpdateAcJournalDetail(acJournalDetail);
                }
                else
                {
                    DAL.InsertAcJournalDetail(acJournalDetail);
                }

                if (v.AcJDetailVM[i].AcExpAllocationVM != null)
                {
                    for (int k = 0; k < v.AcJDetailVM[i].AcExpAllocationVM.Count; k++)
                    {
                        Nullable<int> AllocationIdExists = 0;
                        AcAnalysisHeadAllocation objAcAnalysisHeadAllocations = new AcAnalysisHeadAllocation();
                        if (v.AcJDetailVM[i].AcExpAllocationVM[k].AcAnalysisHeadAllocationID != null && v.AcJDetailVM[i].AcExpAllocationVM[k].AcAnalysisHeadAllocationID > 0)
                        {
                            AllocationIdExists = v.AcJDetailVM[i].AcExpAllocationVM[k].AcAnalysisHeadAllocationID;
                        }
                        if (AllocationIdExists > 0)
                        {
                            objAcAnalysisHeadAllocations.AcAnalysisHeadAllocationID = (int)AllocationIdExists;
                        }
                        else
                        {
                            objAcAnalysisHeadAllocations.AcAnalysisHeadAllocationID = 0;
                        }
                        objAcAnalysisHeadAllocations.AcjournalDetailID = acJournalDetail.AcJournalDetailID;
                        objAcAnalysisHeadAllocations.Amount = v.AcJDetailVM[i].AcExpAllocationVM[k].ExpAllocatedAmount;
                        objAcAnalysisHeadAllocations.AnalysisHeadID = v.AcJDetailVM[i].AcExpAllocationVM[k].AcHead;
                        if (AllocationIdExists > 0)
                        {
                            DAL.UpdateAcAnalysisHeadAllocation(objAcAnalysisHeadAllocations);
                        }
                        else
                        {
                            DAL.InsertAcAnalysisHeadAllocation(objAcAnalysisHeadAllocations);
                        }
                        //  AcJournalDetID
                    }
                }

            }

            string DeleteJournalDetails = Request["deletedJournalDetails"];
            string[] DeleteJournalDetailsArr = DeleteJournalDetails.Split(',');
            foreach (string JournalDetails in DeleteJournalDetailsArr)
            {
                int iDeleteJournalDetails = 0;
                int.TryParse(JournalDetails, out iDeleteJournalDetails);
                DAL.DeleteAcJournalDetail(iDeleteJournalDetails);
            }
            string DeleteAcAnalysisHeadAllocation = Request["deletedExpAllocations"];
            string[] DeleteAcAnalysisHeadAllocationArr = DeleteAcAnalysisHeadAllocation.Split(',');
            foreach (string AcAnalysisHeadAllocation in DeleteAcAnalysisHeadAllocationArr)
            {
                int iAcAnalysisHeadAllocation = 0;
                int.TryParse(AcAnalysisHeadAllocation, out iAcAnalysisHeadAllocation);
                DAL.DeleteAcAnalysisHeadAllocation(iAcAnalysisHeadAllocation);
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

        public ActionResult EditAcBook(int id)
        {
            AcBookVM v = new AcBookVM();

            AcJournalMaster ajm = context.AcJournalMasters.Find(id);
            AcBankDetail abank = (from a in context.AcBankDetails where a.AcJournalID == id select a).FirstOrDefault();
            v.TransactionNo = ajm.TransactionNo;
            v.transdate = ajm.TransDate.Value;
            v.SelectedAcHead = (from c in context.AcJournalDetails where c.AcJournalID == ajm.AcJournalID select c.AcHeadID).FirstOrDefault();
            v.AcHead = (from c in context.AcHeads where c.AcHeadID == v.SelectedAcHead select c.AcHead1).FirstOrDefault();
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
            var paymentterms = (from d in context.PaymentTerms select d).ToList();
            var paytypes1 = new SelectList(paymentterms, "PaymentTermID", "PaymentTerm1");
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




        public JsonResult GetAcJDetails(Nullable<int> id, int? transtype)
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

        public JsonResult GetAcJDetailsExpenseAllocation(int AcJournalDetailID)
        {
            //  AcAnalysisHeadAllocation objAcAnalysisHeadAllocation = new AcAnalysisHeadAllocation();
            //       objAcAnalysisHeadAllocation.AcjournalDetailID = acJournalDetail.AcJournalDetailID;

            var acjlist = DAL.GetAcJDetailsExpenseAllocation(AcJournalDetailID);

            //(from a in context.AcAnalysisHeadAllocations where a.AcjournalDetailID == AcJournalDetailID select a).ToList();


            return Json(acjlist, JsonRequestBehavior.AllowGet);
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


        public JsonResult ShowBankReconciliation(string acheadid, string from, string to)
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


        public JsonResult GetNewFYear(string cFyearFrom, string cFyearTo)
        {
            YearEndProcessVM v = new YearEndProcessVM();
            //using (StreamWriter _logData = new StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath("~/Logyearend.txt"), true))
            //{
            //try
            //{
            //_logData.WriteLine("Fyear :" + cFyearFrom);
            //_logData.WriteLine("toyear :" + cFyearTo);


            v.CurrentFYearFrom = cFyearFrom;
            v.CurrentFYearTo = cFyearTo;

            var fdate = cFyearFrom.Split('/');
            var tdate = cFyearTo.Split('/');
            if (Convert.ToInt32(fdate[0]) > 12)
            {
                cFyearFrom = fdate[1] + "/" + fdate[0] + "/" + fdate[2];

            }
            if (Convert.ToInt32(tdate[0]) > 12)
            {
                cFyearTo = tdate[1] + "/" + tdate[0] + "/" + tdate[2];

            }
            DateTime tnewfyear = Convert.ToDateTime(cFyearFrom).AddYears(1);

            //_logData.WriteLine("tnewfyear :" + tnewfyear);
            v.NewFYearFrom = tnewfyear.ToString("dd/MM/yyyy");
            //_logData.WriteLine("NewFYearFrom :" + v.NewFYearFrom);

            DateTime tnewtyear = Convert.ToDateTime(cFyearTo).AddYears(1);
            //_logData.WriteLine("tnewtyear :" + tnewtyear);

            v.NewFYearTo = tnewtyear.ToString("dd/MM/yyyy");
            //_logData.WriteLine("tnewtyear :" + v.NewFYearTo);

            v.Reference = tnewfyear.Year + "-" + tnewtyear.Year;
            //_logData.WriteLine("Reference :" + v.Reference);

            //}
            //catch(Exception ex)
            //{
            //    _logData.WriteLine("Error :" +ex.Message.ToString());

            //}
            //}
            return Json(v, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindOpenHead(string NewYearFrom, string NewYearTo, string ref1)
        {
            SHIPPING_FinalEntities context1 = new SHIPPING_FinalEntities();
            int NewFYearID = 0;
            AcFinancialYear a = (from c in context1.AcFinancialYears where c.ReferenceName == ref1 select c).FirstOrDefault();

            if (a != null)
            {
                NewFYearID = a.AcFinancialYearID;
            }
            var fdate = NewYearFrom.Split('/');
            var tdate = NewYearTo.Split('/');
            if (Convert.ToInt32(fdate[0]) > 12)
            {
                NewYearFrom = fdate[1] + "/" + fdate[0] + "/" + fdate[2];

            }
            if (Convert.ToInt32(tdate[0]) > 12)
            {
                NewYearTo = tdate[1] + "/" + tdate[0] + "/" + tdate[2];

            }
            //bool result = ESS.SOP.BLL.AcFinancialYear.SaveNewFinancialYear(Convert.ToInt32(Session["fyearid"]), Convert.ToInt32(Session["AcCompanyID"]), Convert.ToDateTime(dpNewFyearFrom.SelectedDate), Convert.ToDateTime(dpNewFyearTo.SelectedDate), txtReferenceName.Text, Convert.ToInt32(Session["userid"]), newFinancialYearID);

            int res = context1.SaveFinancialYear(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString()), Convert.ToDateTime(NewYearFrom), Convert.ToDateTime(NewYearTo), ref1, Convert.ToInt32(Session["UserID"].ToString()), NewFYearID);
            var Openbal = context1.GetOpeningBalanceForYE(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString()));
            int res1 = 10;
            return Json(Openbal, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindPLOpenBalance()
        {
            SHIPPING_FinalEntities context1 = new SHIPPING_FinalEntities();
            var Openbal = context1.GetPLOpeningAmount(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString()));
            return Json(Openbal, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindPLOpenBalanceFinish(string reference)
        {
            SHIPPING_FinalEntities context1 = new SHIPPING_FinalEntities();
            try
            {
                Yearend(reference);
            }
            catch (Exception ex)
            {
                var Openbal = context1.GetPLOpeningAmount(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString()));

                return Json(new { success = false, message = ex.Message.ToString(), bal = Openbal }, JsonRequestBehavior.AllowGet);

            }
            var Openbal1 = context1.GetPLOpeningAmount(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString()));

            return Json(new { success = true, message = "Year end process completed successfully.", bal = Openbal1 }, JsonRequestBehavior.AllowGet);
        }
        public void Yearend(string ref1)
        {

            var lstAcHead = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            var lstAcJournalMaster = new List<AcJournalMaster>();
            var acJournalMaster = new AcJournalMaster();
            List<AcJournalDetail> lstAcJournalDetail = new List<AcJournalDetail>();
            AcJournalDetail acJournalDetail = new AcJournalDetail(); ;
            var Openbal = context.GetPLOpeningAmount(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString()));

            foreach (var item in Openbal)
            {
                lstAcJournalDetail = new List<AcJournalDetail>();
                decimal Amount = Convert.ToDecimal(item.Balance);
                if (item.Balance == null)
                {
                    Amount = 0;
                }

                if (Amount != 0)
                {
                    //Add YearEnd in AcJournalMaster
                    int maxAcJDetailID = 0;
                    maxAcJDetailID = (from c in context.AcJournalDetails orderby c.AcJournalDetailID descending select c.AcJournalDetailID).FirstOrDefault();

                    acJournalMaster = new AcJournalMaster();

                    acJournalMaster.VoucherNo = "";
                    acJournalMaster.TransDate = DateTime.Now; ;
                    acJournalMaster.AcFinancialYearID = Convert.ToInt32(Session["fyearid"]);
                    acJournalMaster.VoucherType = "YE";
                    acJournalMaster.TransType = 1;
                    acJournalMaster.StatusDelete = false;
                    acJournalMaster.UserID = Convert.ToInt32(Session["userid"]);

                    //Add Year End in AcJournalDetail
                    acJournalDetail = new AcJournalDetail();
                    acJournalDetail.AcJournalDetailID = maxAcJDetailID + 1;

                    acJournalDetail.AcHeadID = item.AcHeadID;
                    acJournalDetail.Amount = Amount * -1;
                    acJournalDetail.Remarks = "Closing Adjustment";
                    lstAcJournalDetail.Add(acJournalDetail);

                    acJournalDetail = new AcJournalDetail();
                    var achead = (from d in context.AcHeads where d.AcHeadID == 30 select d).FirstOrDefault();
                    acJournalDetail.AcHeadID = achead.AcHeadID;
                    acJournalDetail.Amount = Amount;
                    acJournalDetail.Remarks = "";
                    acJournalDetail.AcJournalDetailID = maxAcJDetailID + 2;
                    lstAcJournalDetail.Add(acJournalDetail);

                    acJournalMaster.AcJournalDetails = lstAcJournalDetail;
                    lstAcJournalMaster.Add(acJournalMaster);
                }
            }
            foreach (var item in lstAcJournalMaster.ToList())
            {
                SHIPPING_FinalEntities c1 = new SHIPPING_FinalEntities();
                c1.AcJournalMasters.Add(item);
                c1.SaveChanges();
            }

            AddInAcOpeningMaster(lstAcHead.ToList(), ref1);

        }
        private void AddInAcOpeningMaster(List<AcHeadSelectAll_Result> lstAcHead, string ref1)
        {
            //AcOpening enter Assets and expenses
            List<AcOpeningMaster> lstAcOpeningMaster = new List<AcOpeningMaster>();
            Int32 acFinancialYearID = (from c in context.AcFinancialYears where c.ReferenceName == ref1 select c.AcFinancialYearID).FirstOrDefault(); ;
            if (acFinancialYearID == 0)
            {
                acFinancialYearID = Convert.ToInt32(Session["fyearid"]);
            }
            var acOpeningMaster = new AcOpeningMaster();
            var Openbal = context.GetPLOpeningAmount(Convert.ToInt32(Session["fyearid"].ToString()), Convert.ToInt32(Session["AcCompanyID"].ToString()));

            foreach (var item in Openbal)
            {
                decimal Amount = Convert.ToDecimal(item.Balance);
                if (item.Balance == null)
                {
                    Amount = 0;
                }
                if (Amount != 0)
                {
                    acOpeningMaster = new AcOpeningMaster();
                    acOpeningMaster.AcFinancialYearID = acFinancialYearID;
                    acOpeningMaster.OPDate = DateTime.Now;
                    acOpeningMaster.AcHeadID = item.AcHeadID;
                    acOpeningMaster.Amount = Amount;
                    acOpeningMaster.UserID = Convert.ToInt32(Session["userid"]);
                    lstAcOpeningMaster.Add(acOpeningMaster);
                }

            }
            //Enter PLAccount in AcOpening With New Financial Year ID
            acOpeningMaster = new AcOpeningMaster();
            if (ref1 != string.Empty)
            {
                acOpeningMaster.AcFinancialYearID = acFinancialYearID; //ESS.SOP.BLL.AcFinancialYear.GetNewFinancialYearID(txtReferenceName.Text);
            }
            //acOpeningMaster.OPDate
            var profitlossAccountID = (from d in context.AcHeads where d.AcHeadID == 30 select d).FirstOrDefault();
            acOpeningMaster.AcHeadID = profitlossAccountID.AcHeadID;
            acOpeningMaster.OPDate = DateTime.Now; ;
            var abc = (from p in context.AcJournalDetails
                       join l in context.AcJournalMasters on p.AcJournalID equals l.AcJournalID
                       where l.VoucherType == "YE" && l.TransType == 1 && p.AcHeadID == profitlossAccountID.AcHeadID
                       select p).ToList();
            decimal? plAmount = abc.Sum(i => i.Amount);
            acOpeningMaster.Amount = plAmount;

            acOpeningMaster.UserID = Convert.ToInt32(Session["userid"]);
            lstAcOpeningMaster.Add(acOpeningMaster);
            Int32 ID = -1;
            foreach (var item in lstAcOpeningMaster.ToList())
            {
                item.AcOpeningID = ID;
                context.AcOpeningMasters.Add(item);
                ID = ID - 1;
            }
            var sresult = context.SaveChanges();
            context.Dispose();


        }
        public ActionResult IndexAcHeadAssign()
        {
            var AcheadControl = context.AcHeadControls.ToList();
            var AcheadControlList = new List<AcHeadControlList>();
            foreach (var item in AcheadControl)
            {
                var model = new AcHeadControlList();
                model.AccountName = item.AccountName;
                model.Id = item.Id;
                model.PageControlName = context.PageControlMasters.Where(d => d.Id == item.Pagecontrol).FirstOrDefault().ControlName;
                model.PageControlId = item.Pagecontrol;
                model.PageControlField = item.Remarks;
                model.AcHeadId = item.AccountHeadID;
                var achead = context.AcHeads.Find(item.AccountHeadID);
                if (achead != null)
                {
                    model.AccountHeadName = achead.AcHead1;
                }
                model.Check_Sum = Convert.ToBoolean(item.CheckSum) ? "Page Field Value" : "Sum Value";
                if (item.Remarks == 0)
                {
                    model.PageControlFieldName = "Sum";
                }
                else
                {
                    model.PageControlFieldName = context.PageControlFields.Where(d => d.Id == item.Remarks).FirstOrDefault().FieldName;

                }
                model.AccountNature = (Convert.ToBoolean(item.AccountNature)) ? "Debit" : "Credit";
                AcheadControlList.Add(model);
            }
            return View(AcheadControlList);
        }
        [HttpGet]
        public ActionResult CreateAcHeadControl()
        {

            ViewBag.AccountHeadID = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString()));
            var PageControl = context.PageControlMasters.ToList();
            ViewBag.Pagecontrol = new SelectList(PageControl, "Id", "ControlName");
            var PageControlField = context.PageControlFields.ToList();
            ViewBag.Remarks = new SelectList(PageControlField, "Id", "FieldName");

            return View();
        }
        [HttpPost]
        public ActionResult CreateAcHeadControl(AcHeadControl acheadcontrol)
        {
            ViewBag.AccountHeadID = context.AcHeadSelectAll(Convert.ToInt32(Session["AcCompanyID"].ToString())).ToList();
            var PageControl = context.PageControlMasters.ToList();
            ViewBag.Pagecontrol = new SelectList(PageControl, "Id", "ControlName");
            var PageControlField = context.PageControlFields.ToList();
            ViewBag.Remarks = new SelectList(PageControlField, "Id", "FieldName");

            var data = new AcHeadControl();
            data.AccountHeadID = acheadcontrol.AccountHeadID;
            data.AccountName = acheadcontrol.AccountName;
            data.AccountNature = acheadcontrol.AccountNature;
            data.Remarks = acheadcontrol.Remarks;
            data.Pagecontrol = acheadcontrol.Pagecontrol;
            if (ModelState.IsValid)
            {
                if (acheadcontrol.Remarks == 0)
                {
                    data.CheckSum = false;
                }
                else
                {
                    data.CheckSum = true;
                }
                context.AcHeadControls.Add(data);
                context.SaveChanges();
                ViewBag.SuccessMsg = "You have successfully added Account Assign Head";
                return RedirectToAction("IndexAcHeadAssign");
            }
            return View(acheadcontrol);
        }
        [HttpGet]
        public ActionResult EditAcHeadControl(int Id)
        {
            var data = context.AcHeadControls.Find(Id);

            ViewBag.AccountHeadID = context.AcHeads.ToList();
            var PageControl = context.PageControlMasters.ToList();
            ViewBag.Pagecontrol = PageControl;
            var PageControlField = context.PageControlFields.Where(d => d.PageControlId == data.Pagecontrol).ToList();
            ViewBag.Remarks = PageControlField;

            return View(data);
        }
        [HttpPost]
        public ActionResult EditAcHeadControl(AcHeadControl acheadcontrol)
        {
            var data = context.AcHeadControls.Find(acheadcontrol.Id);

            ViewBag.AccountHeadID = context.AcHeads.ToList();
            var PageControl = context.PageControlMasters.ToList();
            ViewBag.Pagecontrol = PageControl;
            var PageControlField = context.PageControlFields.Where(d => d.PageControlId == data.Pagecontrol).ToList();
            ViewBag.Remarks = PageControlField;


            data.AccountHeadID = acheadcontrol.AccountHeadID;
            data.AccountName = acheadcontrol.AccountName;
            data.AccountNature = acheadcontrol.AccountNature;
            data.Remarks = acheadcontrol.Remarks;
            data.Pagecontrol = acheadcontrol.Pagecontrol;
            if (ModelState.IsValid)
            {
                if (acheadcontrol.Remarks == 0)
                {
                    data.CheckSum = false;
                }
                else
                {
                    data.CheckSum = true;
                }
                //context.AcHeadControls.Add(data);
                context.SaveChanges();
                ViewBag.SuccessMsg = "You have successfully added Account Assign Head";
                return RedirectToAction("IndexAcHeadAssign");
            }
            return View(acheadcontrol);
        }
        public JsonResult GetPageControlFields(int id)
        {
            return Json(new SelectList(context.PageControlFields.Where(c => c.PageControlId == id).OrderBy(o => o.Id), "Id", "FieldName"), JsonRequestBehavior.AllowGet);
        }


        ///////////////////////////////
        public ActionResult IndexAcType()
        {

            //var x = GetAllAcGroupsByBranch(Convert.ToInt32(Session["branchid"].ToString()));
            var Accategory = (from d in context.AcCategories select d).ToList();
            var branchid = Convert.ToInt32(Session["branchid"].ToString());
            var actype = (from d in context.AcTypes where d.BranchId == branchid select d).ToList();
            var Modellist = new List<AcTypeModel>();
            foreach (var item in actype)
            {
                var model = new AcTypeModel();
                model.Id = item.Id;
                model.AcType = item.AccountType;
                model.AcCategoryID = item.AcCategoryId;
                model.AcCategory = Accategory.Where(d => d.AcCategoryID == item.AcCategoryId).FirstOrDefault().AcCategory1;
                Modellist.Add(model);
            }
            return View(Modellist);
        }

        public ActionResult CreateAcType()
        {
            ViewBag.Category = context.AcCategorySelectAll();
            var model = new AcTypeModel();
            return View(model);
        }


        public bool GetDuplicateType(int AcTypeId, int? CategoryID, string name)
        {
            var branchid = Convert.ToInt32(Session["branchid"].ToString());

            var data = (from d in context.AcTypes where d.Id != AcTypeId && d.AccountType.ToLower() == name.ToLower() && d.AcCategoryId == CategoryID && d.BranchId == branchid select d).FirstOrDefault();
            if (data == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        [HttpPost]
        public ActionResult CreateAcType(AcTypeModel c)
        {

            var isexist = GetDuplicateType(0, c.AcCategoryID, c.AcType);
            if (isexist == true)
            {
                var actype = new AcType();
                actype.AcCategoryId = c.AcCategoryID;
                actype.AccountType = c.AcType;
                actype.BranchId = Convert.ToInt32(Session["branchid"].ToString());
                context.AcTypes.Add(actype);
                context.SaveChanges();
                ViewBag.SuccessMsg = "You have successfully added Account Type";
                return RedirectToAction("IndexAcType", "AccountingModule", new { id = 0 });


            }
            else
            {
                ViewBag.ErrorMsg = "Account Type already exists !!";
                return View(c);
            }


        }

        public ActionResult EditAcType(int id)
        {
            ViewBag.Category = context.AcCategorySelectAll();

            AcTypeModel v = new AcTypeModel();
            var data = context.AcTypes.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            else
            {
                v.Id = data.Id;
                v.AcCategoryID = data.AcCategoryId;
                v.AcType = data.AccountType;
            }

            return View(v);
        }

        [HttpPost]
        public ActionResult EditAcType(AcTypeModel c)
        {
            var isexist = GetDuplicateType(c.Id, c.AcCategoryID, c.AcType);
            if (isexist == true)
            {
                //var type = new AcType();
                var type = (from d in context.AcTypes where d.Id == c.Id select d).FirstOrDefault();
                //type.Id = c.Id;
                type.AccountType = c.AcType;
                type.AcCategoryId = c.AcCategoryID;
                context.Entry(type).State = EntityState.Modified;
                context.SaveChanges();

                ViewBag.SuccessMsg = "You have successfully updated Account Type";
                return RedirectToAction("IndexAcType", "AccountingModule", new { id = 0 });
            }
            else
            {
                ViewBag.Category = context.AcCategorySelectAll();
                ViewBag.ErrorMsg = "Account Type already exists !!";
                return View(c);
            }
        }


        public ActionResult DeleteAcType(int id)
        {
            AcType c = (from x in context.AcTypes where x.Id == id select x).FirstOrDefault();
            if (c != null)
            {
                try
                {
                    var p = (from a in context.AcGroups where a.AcTypeId == id select a).FirstOrDefault();
                    if (p != null)
                    {
                        ViewBag.ErrorMsg = "Transaction in Use. Can not Delete";
                        throw new Exception();

                    }
                    else
                    {
                        context.AcTypes.Remove(c);
                        context.SaveChanges();


                        ViewBag.SuccessMsg = "You have successfully deleted Account Type";
                        return RedirectToAction("IndexAcType", "AccountingModule", new { id = 0 });

                    }

                }
                catch (Exception ex)
                {





                }
            }

            return RedirectToAction("IndexAcType", "AccountingModule", new { id = 0 });
        }



    }



}
public class SessionExpireAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        HttpContext ctx = HttpContext.Current;
        // check  sessions here
        if (HttpContext.Current.Session["UserID"] == null)
        {
            filterContext.Result = new RedirectResult("~/Errors/SessionTimeOut");
            return;
        }
        base.OnActionExecuting(filterContext);
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