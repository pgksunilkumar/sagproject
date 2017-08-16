using BusinessAccessLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConGun.Controllers
{
    public class BListController : Controller
    {
        BListDAL objBListDAL = new BListDAL();
        public ActionResult Create()
        {
            BListModel objBListModel = new BListModel();
            DataTable deEquipmentType = objBListDAL.GetCategoryType();

            List<BListCategory> objListEquipTemp = new List<BListCategory>();
            if (deEquipmentType != null)
            {
                foreach (DataRow item in deEquipmentType.Rows)
                {
                    BListCategory objEquipTemp = new BListCategory();
                    objEquipTemp.CategoryID = Convert.ToInt16(item["CategoryID"].ToString());
                    objEquipTemp.Category = item["Category"].ToString();
                    objListEquipTemp.Add(objEquipTemp);
                }
            }
            objBListModel.CategoryOption = objListEquipTemp;
            ViewBag.CategoryOption = new SelectList(objBListModel.CategoryOption, "CategoryID", "Category", objBListModel.Category);

            List<YearNew> YearArray = new List<YearNew>();
            var startYear = Convert.ToInt16(ConfigurationManager.AppSettings["startYearReading"]);
            int yearCount = 1;
            while (yearCount == 1)
            {
                YearNew objYear = new YearNew();
                objYear.Id = startYear.ToString();
                objYear.Value = startYear.ToString();
                YearArray.Add(objYear);
                startYear++;
                if (startYear == DateTime.Now.Year + 1)
                    yearCount = 0;
            }
            objBListModel.YearOption = YearArray;
            ViewBag.YearOption = new SelectList(objBListModel.YearOption, "Id", "Value", objBListModel.Year);

            if (Convert.ToString(Session["UserID"]) != "")
            {
                objBListModel.CheckForUser = true;
            }

            AccountModel.LoginViewModel objLogin = new AccountModel.LoginViewModel();
            objBListModel.LoginModel = objLogin;

            return View(objBListModel);
        }

        // POST: Rental/Create
        [HttpPost]
        public ActionResult Create(BListModel objBListModel)
        {
            try
            {
                if (Convert.ToString(Session["UserID"]) != "")
                    objBListModel.UserID = Convert.ToInt16(Convert.ToString(Session["UserID"]));

                int intBListID = objBListDAL.SaveBListDetail(objBListModel);

                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult List(int? id)
        {
            if (id != null)
            {
                if (Convert.ToString(Session["UserID"]) != "")
                {
                    if (Convert.ToInt16(Session["UserID"].ToString()) != id)
                        return RedirectToAction("Exception", "Account");
                }
                if (Convert.ToString(Session["UserID"]) == "") { return RedirectToAction("Exception", "Account"); }
            }

            DataTable dtList = objBListDAL.GetBusinessList(id,"");
            List<BListModel> objBListModel = FillBListModel(dtList);

            return View(objBListModel);
        }

        public ActionResult Details(int id)
        {
            BListModel objUsedModel = new BListModel();
            DataSet dtList = objBListDAL.GetBListItemById(id);
            foreach (DataRow item in dtList.Tables[0].Rows)
            {
                objUsedModel.Category = item["Category"].ToString();
                objUsedModel.BusniessListID = Convert.ToInt16(item["BusinessListID"].ToString());

                if (Convert.ToString(item["CompanyName"]) != "")
                    objUsedModel.CompanyName = item["CompanyName"].ToString();
                else
                    objUsedModel.CompanyName = "NA";
                if (Convert.ToString(item["Year"]) != "")
                    objUsedModel.Year = item["Year"].ToString();
                else
                    objUsedModel.Year = "NA";
                if (Convert.ToString(item["Website"]) != "")
                    objUsedModel.Website = item["Website"].ToString();
                else
                    objUsedModel.Website = "NA";
                if (Convert.ToString(item["FBLink"]) != "")
                    objUsedModel.FBLink = item["FBLink"].ToString();
                else
                    objUsedModel.FBLink = "NA";
                if (Convert.ToString(item["GooglePlusLink"]) != "")
                    objUsedModel.GooglePlusLink = item["GooglePlusLink"].ToString();
                else
                    objUsedModel.GooglePlusLink = "NA";
                
                if (Convert.ToString(item["LinkedInLink"]) != "")
                    objUsedModel.LinkedInLink = item["LinkedInLink"].ToString();
                else
                    objUsedModel.LinkedInLink = "NA";
                if (Convert.ToString(item["LandPhone"]) != "")
                    objUsedModel.LandPhone = item["LandPhone"].ToString();
                else
                    objUsedModel.LandPhone = "NA";
                if (Convert.ToString(item["MobilePhone"]) != "")
                    objUsedModel.MobilePhone = item["MobilePhone"].ToString();
                else
                    objUsedModel.MobilePhone = "NA";
                if (Convert.ToString(item["AlternatePhone"]) != "")
                    objUsedModel.AlternatePhone = item["AlternatePhone"].ToString();
                else
                    objUsedModel.AlternatePhone = "NA";
                if (Convert.ToString(item["EmailID"]) != "")
                    objUsedModel.EmailID = item["EmailID"].ToString();
                else
                    objUsedModel.EmailID = "NA";
                if (Convert.ToString(item["Address"]) != "")
                    objUsedModel.Address = item["Address"].ToString();
                else
                    objUsedModel.Address = "NA";
                if (Convert.ToString(item["Location"]) != "")
                    objUsedModel.Location = item["Location"].ToString();
                else
                    objUsedModel.Location = "NA";
                if (Convert.ToString(item["District"]) != "")
                    objUsedModel.District = item["District"].ToString();
                else
                    objUsedModel.District = "NA";
                if (Convert.ToString(item["State"]) != "")
                    objUsedModel.State = item["State"].ToString();
                else
                    objUsedModel.State = "NA";
                if (Convert.ToString(item["Description"]) != "")
                    objUsedModel.Description = item["Description"].ToString();
                else
                    objUsedModel.Description = "NA";
                if (Convert.ToString(item["CreatedDate"]) != "")
                    objUsedModel.CreatedDate = Convert.ToDateTime(item["CreatedDate"].ToString());
            }

            //List<PostComment> lstPostComment = new List<PostComment>();
            //foreach (DataRow item in dtList.Tables[1].Rows)
            //{
            //    PostComment objPostComment = new PostComment();
            //    objPostComment.Name = item["Name"].ToString();
            //    objPostComment.Comments = item["Comments"].ToString();
            //    lstPostComment.Add(objPostComment);
            //}

            //objUsedModel.PostCommentList = lstPostComment;

            return View(objUsedModel);
        }

        public List<BListModel> FillBListModel(DataTable dtList)
        {
            List<BListModel> objBListModel = new List<BListModel>();
            if (dtList != null)
            {
                foreach (DataRow item in dtList.Rows)
                {
                    BListModel objRentalModel = new BListModel();
                    objRentalModel.BusniessListID = Convert.ToInt16(item["BusinessListID"].ToString());
                    objRentalModel.Category = item["Category"].ToString();
                    if (Convert.ToString(item["CompanyName"]) != "")
                        objRentalModel.CompanyName = item["CompanyName"].ToString();
                    else
                        objRentalModel.CompanyName = "NA";
                    if (Convert.ToString(item["Year"]) != "")
                        objRentalModel.Year = item["Year"].ToString();
                    else
                        objRentalModel.Year = "NA";
                    if (Convert.ToString(item["Website"]) != "")
                        objRentalModel.Website = item["Website"].ToString();
                    else
                        objRentalModel.Website = "NA";
                    if (Convert.ToString(item["FBLink"]) != "")
                        objRentalModel.FBLink = item["FBLink"].ToString();
                    else
                        objRentalModel.FBLink = "NA";
                    if (Convert.ToString(item["GooglePlusLink"]) != "")
                        objRentalModel.GooglePlusLink = item["GooglePlusLink"].ToString();
                    else
                        objRentalModel.GooglePlusLink = "NA";
                    if (Convert.ToString(item["LinkedInLink"]) != "")
                        objRentalModel.LinkedInLink = item["LinkedInLink"].ToString();
                    else
                        objRentalModel.LinkedInLink = "NA";
                    if (Convert.ToString(item["LandPhone"]) != "")
                        objRentalModel.LandPhone = item["LandPhone"].ToString();
                    else
                        objRentalModel.LandPhone = "NA";
                    if (Convert.ToString(item["MobilePhone"]) != "")
                        objRentalModel.MobilePhone = item["MobilePhone"].ToString();
                    else
                        objRentalModel.MobilePhone = "NA";
                    if (Convert.ToString(item["AlternatePhone"]) != "")
                        objRentalModel.AlternatePhone = item["AlternatePhone"].ToString();
                    else
                        objRentalModel.AlternatePhone = "NA";
                    if (Convert.ToString(item["EmailID"]) != "")
                        objRentalModel.EmailID = item["EmailID"].ToString();
                    else
                        objRentalModel.EmailID = "NA";
                    if (Convert.ToString(item["Address"]) != "")
                        objRentalModel.Address = item["Address"].ToString();
                    else
                        objRentalModel.Address = "NA";
                    if (Convert.ToString(item["Location"]) != "")
                        objRentalModel.Location = item["Location"].ToString();
                    else
                        objRentalModel.Location = "NA";
                    if (Convert.ToString(item["State"]) != "")
                        objRentalModel.State = item["State"].ToString();
                    else
                        objRentalModel.State = "NA";
                    if (Convert.ToString(item["District"]) != "")
                        objRentalModel.District = item["District"].ToString();
                    else
                        objRentalModel.District = "NA";
                    if (Convert.ToString(item["Description"]) != "")
                        objRentalModel.Description = item["Description"].ToString();
                    else
                        objRentalModel.Description = "NA";

                    objBListModel.Add(objRentalModel);
                }
            }
            ViewBag.Categories = objBListDAL.GetCategoryType();
            return objBListModel;
        }

        [HttpGet]
        public ActionResult GetBListForSearch(string Category)
        {
            Category = Category.TrimEnd(',');
            DataTable dtList = objBListDAL.GetBusinessList(null, Category);

            List<BListModel> objBListModel = FillBListModel(dtList);

            return PartialView("_BusinessListPartial", objBListModel);
        }
    }
}