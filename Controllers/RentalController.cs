using BusinessAccessLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConGun.Controllers
{
    public class RentalController : Controller
    {
        RentalEquipmentDAL objRentalEquipmentDAL = new RentalEquipmentDAL();
        AccountDAL objAccountDAL = new AccountDAL();
        // GET: Rental
        public ActionResult List()
        {
            DataTable dtList = objRentalEquipmentDAL.GetRentalEquipmentList();
            List<RentalModel> objListRentalModel = new List<RentalModel>();
            if (dtList != null)
            {
                foreach (DataRow item in dtList.Rows)
                {
                    RentalModel objRentalModel = new RentalModel();
                    objRentalModel.RentalID = Convert.ToInt16(item["RentalID"].ToString());
                    objRentalModel.EquipmentType = item["EquipmentType"].ToString();
                    objRentalModel.FromDate = DateTime.ParseExact(Convert.ToDateTime(item["FromDate"].ToString()).ToShortDateString(), "M/d/yyyy", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                    objRentalModel.ToDate = DateTime.ParseExact(Convert.ToDateTime(item["ToDate"].ToString()).ToShortDateString(), "M/d/yyyy", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                    if (Convert.ToString(item["ContactNumber"]) != "")
                        objRentalModel.ContactNumber = item["ContactNumber"].ToString();
                    else
                        objRentalModel.ContactNumber = "NA";
                    if (Convert.ToString(item["EmailID"]) != "")
                        objRentalModel.EmailID = item["EmailID"].ToString();
                    else
                        objRentalModel.EmailID = "NA";
                    if (Convert.ToString(item["Location"]) != "")
                        objRentalModel.Location = item["Location"].ToString();
                    else
                        objRentalModel.Location = "NA";
                    if (Convert.ToString(item["Comments"]) != "")
                        objRentalModel.Comments = item["Comments"].ToString();
                    else
                        objRentalModel.Comments = "NA";

                    objListRentalModel.Add(objRentalModel);
                }
            }
            return View(objListRentalModel);
        }

        // GET: Rental/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Rental/Create
        public ActionResult Create()
        {
            RentalModel objUsedModel = new RentalModel();
            DataTable deEquipmentType = objRentalEquipmentDAL.GetEquipmentType();

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

            if (Convert.ToString(Session["UserID"]) != "")
            {
                objUsedModel.CheckForUser = true;
            }

            AccountModel.LoginViewModel objLogin = new AccountModel.LoginViewModel();
            objUsedModel.LoginModel = objLogin;

            return View(objUsedModel);
        }

        // POST: Rental/Create
        [HttpPost]
        public ActionResult Create(RentalModel objRentalModel)
        {
            try
            {
                if (Convert.ToString(Session["UserID"]) != "")
                    objRentalModel.UserID = Convert.ToInt16(Convert.ToString(Session["UserID"]));

                int intRentalID = objRentalEquipmentDAL.SaveRentalEquipmentDetail(objRentalModel);

                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        // GET: Rental/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Rental/Edit/5
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

        // GET: Rental/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Rental/Delete/5
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

        public ActionResult Index()
        {
            return View();
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
    }
}
