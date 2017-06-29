using BusinessAccessLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ConGun.Controllers
{
    public class UsedController : Controller
    {
        UsedEquipmentDAL objUsedEquipmentDAL = new UsedEquipmentDAL();
        AccountDAL objAccountDAL = new AccountDAL();

        // GET: Used
        public ActionResult List()
        {
            //DataTable dtList = objUsedEquipmentDAL.GetUsedEquipmentList();
            List<UsedModel> objListUsedModel = new List<UsedModel>();
            //if (dtList != null)
            //{
            //    foreach (DataRow item in dtList.Rows)
            //    {
            //        UsedModel objUsedModel = new UsedModel();
            //        objUsedModel.EquipmentID = Convert.ToInt16(item["EquipmentID"].ToString());
            //        objUsedModel.EquipmentType = item["EquipmentType"].ToString();
            //        if (Convert.ToString(item["Make"]) != "")
            //            objUsedModel.Make = item["Make"].ToString();
            //        else
            //            objUsedModel.Make = "NA";
            //        if (Convert.ToString(item["ModelNumber"]) != "")
            //            objUsedModel.ModelNumber = item["ModelNumber"].ToString();
            //        else
            //            objUsedModel.ModelNumber = "NA";
            //        if (Convert.ToString(item["Readings"]) != "")
            //            objUsedModel.Readings = item["Readings"].ToString();
            //        else
            //            objUsedModel.Readings = "NA";
            //        if (Convert.ToString(item["ContactNumber"]) != "")
            //            objUsedModel.ContactNumber = item["ContactNumber"].ToString();
            //        else
            //            objUsedModel.ContactNumber = "NA";
            //        if (Convert.ToString(item["EmailID"]) != "")
            //            objUsedModel.EmailID = item["EmailID"].ToString();
            //        else
            //            objUsedModel.EmailID = "NA";
            //        if (Convert.ToString(item["Price"]) != "")
            //        {
            //            string fare = item["Price"].ToString();
            //            decimal parsed = decimal.Parse(fare, CultureInfo.InvariantCulture);
            //            CultureInfo hindi = new CultureInfo("hi-IN");
            //            string text = string.Format(hindi, "{0:#,#}", parsed);

            //            objUsedModel.Price = text;
            //        }
            //        else
            //            objUsedModel.Price = "NA";
            //        if (Convert.ToString(item["Location"]) != "")
            //            objUsedModel.Location = item["Location"].ToString();
            //        else
            //            objUsedModel.Location = "NA";
            //        if (Convert.ToString(item["Comments"]) != "")
            //            objUsedModel.Comments = item["Comments"].ToString();
            //        else
            //            objUsedModel.Comments = "NA";
            //        if (Convert.ToString(item["ImageData1"]) != "")
            //        {
            //            byte[] bytes = (byte[])item["ImageData1"];
            //            var base64 = Convert.ToBase64String(bytes);
            //            objUsedModel.ImagePath = String.Format("data:{0};base64,{1}", item["FileType"].ToString(), base64);
            //        }
            //        if (Convert.ToString(item["IsFixed"]) != "")
            //            objUsedModel.PriceCategory = item["IsFixed"].ToString();
            //        else
            //            objUsedModel.PriceCategory = "NA";


            //        objListUsedModel.Add(objUsedModel);
            //    }
            //}
            return View(objListUsedModel);
        }

        public ActionResult Details(int id)
        {
            UsedModel objUsedModel = new UsedModel();
            DataTable dtList = new DataTable();
            DataRow dtRow = dtList.NewRow();
            DataColumn dtcol = new DataColumn();

         


            //objUsedEquipmentDAL.GetUsedEquipmentDetailByID(id);
            foreach (DataRow item in dtList.Rows)
            {
                objUsedModel.EquipmentType = item["EquipmentType"].ToString();

                if (Convert.ToString(item["Make"]) != "")
                    objUsedModel.Make = item["Make"].ToString();
                else
                    objUsedModel.Make = "NA";
                if (Convert.ToString(item["ModelNumber"]) != "")
                    objUsedModel.ModelNumber = item["ModelNumber"].ToString();
                else
                    objUsedModel.ModelNumber = "NA";
                if (Convert.ToString(item["Readings"]) != "")
                    objUsedModel.Readings = item["Readings"].ToString();
                else
                    objUsedModel.Readings = "NA";
                if (Convert.ToString(item["ContactNumber"]) != "")
                    objUsedModel.ContactNumber = item["ContactNumber"].ToString();
                else
                    objUsedModel.ContactNumber = "NA";
                if (Convert.ToString(item["EmailID"]) != "")
                    objUsedModel.EmailID = item["EmailID"].ToString();
                else
                    objUsedModel.EmailID = "NA";
                if (Convert.ToString(item["Price"]) != "")
                {
                    string fare = item["Price"].ToString();
                    decimal parsed = decimal.Parse(fare, CultureInfo.InvariantCulture);
                    CultureInfo hindi = new CultureInfo("hi-IN");
                    string text = string.Format(hindi, "{0:c}", parsed);

                    objUsedModel.Price = text;
                }
                else
                    objUsedModel.Price = "NA";
                if (Convert.ToString(item["Location"]) != "")
                    objUsedModel.Location = item["Location"].ToString();
                else
                    objUsedModel.Location = "NA";
                if (Convert.ToString(item["Comments"]) != "")
                    objUsedModel.Comments = item["Comments"].ToString();
                else
                    objUsedModel.Comments = "NA";
                if (Convert.ToString(item["ImageData1"]) != "")
                {
                    byte[] bytes = (byte[])item["ImageData1"];
                    var base64 = Convert.ToBase64String(bytes);
                    objUsedModel.ImagePath = String.Format("data:{0};base64,{1}", item["FileType"].ToString(), base64);
                }
                else
                {
                    objUsedModel.ImagePath = "#";
                }
                if (Convert.ToString(item["ImageData2"]) != "")
                {
                    byte[] bytes = (byte[])item["ImageData2"];
                    var base64 = Convert.ToBase64String(bytes);
                    objUsedModel.ImagePath2 = String.Format("data:{0};base64,{1}", item["FileType"].ToString(), base64);
                }
                else
                {
                    objUsedModel.ImagePath2 = "#";
                }
                if (Convert.ToString(item["ImageData3"]) != "")
                {
                    byte[] bytes = (byte[])item["ImageData3"];
                    var base64 = Convert.ToBase64String(bytes);
                    objUsedModel.ImagePath3 = String.Format("data:{0};base64,{1}", item["FileType"].ToString(), base64);
                }
                else
                {
                    objUsedModel.ImagePath3 = "#";
                }
                if (Convert.ToString(item["ImageData4"]) != "")
                {
                    byte[] bytes = (byte[])item["ImageData4"];
                    var base64 = Convert.ToBase64String(bytes);
                    objUsedModel.ImagePath4 = String.Format("data:{0};base64,{1}", item["FileType"].ToString(), base64);
                }
                else
                {
                    objUsedModel.ImagePath4 = "#";
                }
                if (Convert.ToString(item["ImageData5"]) != "")
                {
                    byte[] bytes = (byte[])item["ImageData5"];
                    var base64 = Convert.ToBase64String(bytes);
                    objUsedModel.ImagePath5 = String.Format("data:{0};base64,{1}", item["FileType"].ToString(), base64);
                }
                else
                {
                    objUsedModel.ImagePath5 = "#";
                }

                if (Convert.ToString(item["IsFixedDetail"]) != "")
                    objUsedModel.PriceCategory = item["IsFixedDetail"].ToString();
                else
                    objUsedModel.PriceCategory = "NA";
            }
            return View(objUsedModel);
        }

        // GET: Used/Create
        public ActionResult Create()
        {
            UsedModel objUsedModel = new UsedModel();
            DataTable deEquipmentType = objUsedEquipmentDAL.GetEquipmentType();

            List<EquipmentTypeNew> objListEquipTemp = new List<EquipmentTypeNew>();
            if (deEquipmentType != null)
            {
                foreach (DataRow item in deEquipmentType.Rows)
                {
                    EquipmentTypeNew objEquipTemp = new EquipmentTypeNew();
                    objEquipTemp.EquipmentTypeID = Convert.ToInt16(item["EquipmentTypeID"].ToString());
                    objEquipTemp.EquipmentType = item["EquipmentType"].ToString();
                    objListEquipTemp.Add(objEquipTemp);
                }
            }
            objUsedModel.EquipmentTypeOption = objListEquipTemp;
            ViewBag.EquipmentTypeOption = new SelectList(objUsedModel.EquipmentTypeOption, "EquipmentTypeID", "EquipmentType", objUsedModel.EquipmentType);

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
            objUsedModel.YearOption = YearArray;
            ViewBag.YearOption = new SelectList(objUsedModel.YearOption, "Id", "Value", objUsedModel.Readings);

            List<PriceCategoryList> objPriceCategoryList = new List<PriceCategoryList>();

            objPriceCategoryList.Add(new PriceCategoryList { Id = "1", Value = "Fixed" });
            objPriceCategoryList.Add(new PriceCategoryList { Id = "2", Value = "Negotiable" });
            objUsedModel.PriceList = objPriceCategoryList;

            if (Convert.ToString(Session["UserID"]) != "")
            {
                objUsedModel.CheckForUser = true;
            }

            AccountModel.LoginViewModel objLogin = new AccountModel.LoginViewModel();
            objUsedModel.LoginModel = objLogin;


            return View(objUsedModel);
        }

        // POST: Used/Create
        [HttpPost]
        public ActionResult Create(UsedModel objUsedModel, HttpPostedFileBase[] file_Uploader)
        {
            try
            {
                if (Convert.ToString(Session["UserID"]) != "")
                {
                    objUsedModel.UserID = Convert.ToInt16(Session["UserID"].ToString());
                }
                SaveEquipmentDetails(objUsedModel, file_Uploader);
                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        // GET: Used/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Used/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Used/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Used/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private void SaveEquipmentDetails(UsedModel objUsedModel, HttpPostedFileBase[] file_Uploader)
        {
            int imageCount = 1;
            foreach (var file_item in file_Uploader)
            {
                if (file_item != null)
                {
                    if (file_item.ContentLength > 0)
                    {
                        // Due to the limit of the max for a int type, the largest file can be
                        // uploaded is 2147483647, which is very large anyway.
                        int size = file_item.ContentLength;
                        string name = file_item.FileName;
                        int position = name.LastIndexOf("\\");
                        name = name.Substring(position + 1);
                        string contentType = file_item.ContentType;
                        byte[] fileData = new byte[size];
                        file_item.InputStream.Read(fileData, 0, size);

                        switch (imageCount)
                        {
                            case 1:
                                objUsedModel.FileName1 = name;
                                objUsedModel.ImageData1 = fileData;
                                imageCount++;
                                break;
                            case 2:
                                objUsedModel.FileName2 = name;
                                objUsedModel.ImageData2 = fileData;
                                imageCount++;
                                break;
                            case 3:
                                objUsedModel.FileName3 = name;
                                objUsedModel.ImageData3 = fileData;
                                imageCount++;
                                break;
                            case 4:
                                objUsedModel.FileName4 = name;
                                objUsedModel.ImageData4 = fileData;
                                imageCount++;
                                break;
                            case 5:
                                objUsedModel.FileName5 = name;
                                objUsedModel.ImageData5 = fileData;
                                imageCount++;
                                break;
                        }
                    }
                }
            }

            int intEquipmentID = objUsedEquipmentDAL.SaveUsedEquipmentDetail(objUsedModel);
        }

        [HttpPost]
        public string ValidateUserDetail(string ContactNumber, string Password)
        {
            bool process = false;

            AccountModel.LoginViewModel objLoginModel = new AccountModel.LoginViewModel();
            objLoginModel.LoginContactNumber = ContactNumber;
            objLoginModel.Password = Password;
            DataTable dtData = objAccountDAL.LoginDetail(objLoginModel);
            if (dtData.Rows.Count > 0)
            {
                Session["UserID"] = dtData.Rows[0]["UserID"];
                ViewBag.ErrorMessageLogin = "";
                process = true;
            }
            else
            {
                Session["UserID"] = null;
            }

            return process.ToString();
        }

        public ActionResult EquipmentTypeSearch(string term)
        {
            string[] rowAsString = new string[0];
            DataTable dtDriverDetail = objUsedEquipmentDAL.GetEquipmentType();
            if (dtDriverDetail != null)
            {
                rowAsString = new string[dtDriverDetail.Rows.Count];
                for (int i = 0; i < dtDriverDetail.Rows.Count; i++)
                {
                    rowAsString[i] = dtDriverDetail.Rows[i]["EquipmentType"].ToString().ToLower() + " - " + dtDriverDetail.Rows[i]["EquipmentTypeID"].ToString();
                }

            }
            return this.Json(rowAsString.Where(t => t.StartsWith(term)),
                           JsonRequestBehavior.AllowGet);
        }

    }
}
