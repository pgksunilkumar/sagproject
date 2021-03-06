﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;
using System.Configuration;

namespace ConGun.Controllers
{
    public class HomeController : Controller
    {
        UsedEquipmentDAL objUsedEquipmentDAL = new UsedEquipmentDAL();
        public ActionResult Index()
        {
            DataSet dtData = objUsedEquipmentDAL.GetEquipmentList_Dashboard();
            var imageCount = 1;
            if (dtData != null)
            {
                if (dtData.Tables[0] != null)
                {
                    foreach (DataRow item in dtData.Tables[0].Rows)
                    {
                        if (Convert.ToString(item["FileName1"]) != "")
                        {
                            //byte[] bytes = (byte[])item["ImageData1"];
                            //var base64 = Convert.ToBase64String(bytes);

                            switch (imageCount)
                            {
                                case 1:
                                    ViewBag.ImagePath1 = ConfigurationManager.AppSettings["UsedEquipmentImageLoc"].ToString().Replace("~", "") + item["ContactNumber"] + "/" + Convert.ToString(item["FileName1"]);
                                    ViewBag.Price1 = item["Price"].ToString();
                                    break;
                                case 2:
                                    ViewBag.ImagePath2 = ConfigurationManager.AppSettings["UsedEquipmentImageLoc"].ToString().Replace("~", "") + item["ContactNumber"] + "/" + Convert.ToString(item["FileName1"]);
                                    ViewBag.Price2 = item["Price"].ToString();
                                    break;
                                case 3:
                                    ViewBag.ImagePath3 = ConfigurationManager.AppSettings["UsedEquipmentImageLoc"].ToString().Replace("~", "") + item["ContactNumber"] + "/" + Convert.ToString(item["FileName1"]);
                                    break;
                                case 4:
                                    ViewBag.ImagePath4 = ConfigurationManager.AppSettings["UsedEquipmentImageLoc"].ToString().Replace("~", "") + item["ContactNumber"] + "/" + Convert.ToString(item["FileName1"]);
                                    break;
                                case 5:
                                    ViewBag.ImagePath5 = ConfigurationManager.AppSettings["UsedEquipmentImageLoc"].ToString().Replace("~", "") + item["ContactNumber"] + "/" + Convert.ToString(item["FileName1"]);
                                    break;
                                case 6:
                                    ViewBag.ImagePath6 = ConfigurationManager.AppSettings["UsedEquipmentImageLoc"].ToString().Replace("~", "") + item["ContactNumber"] + "/" + Convert.ToString(item["FileName1"]);
                                    break;
                            }
                            imageCount++;
                        }
                    }
                }
                if (dtData.Tables[1] != null)
                {
                    var DescCount = 1;
                    foreach (DataRow item in dtData.Tables[1].Rows)
                    {
                        switch (DescCount)
                        {
                            case 1:
                                ViewBag.RentDetail1 = String.Format("<p>Looking for a <b>{0}</b> for <b>{1}</b> Weeks</p> <br/><span><i class='glyphicon glyphicon-map-marker'></i>&nbsp;{2}</span> <span>|</span> <span><i class='glyphicon glyphicon-phone'></i>&nbsp;{3}</span> <br/>", Convert.ToString(item["EquipmentType"]), Convert.ToString(item["Days"]), Convert.ToString(item["Location"]), Convert.ToString(item["ContactNumber"]));
                                break;
                            case 2:
                                ViewBag.RentDetail2 = String.Format("<p>Looking for a <b>{0}</b> for <b>{1}</b> Weeks </p> <br/><span><i class='glyphicon glyphicon-map-marker'></i>&nbsp;{2}</span> <span>|</span> <span><i class='glyphicon glyphicon-phone'></i>&nbsp;{3}</span> <br/>", Convert.ToString(item["EquipmentType"]), Convert.ToString(item["Days"]), Convert.ToString(item["Location"]), Convert.ToString(item["ContactNumber"]));
                                break;
                            case 3:
                                ViewBag.RentDetail3 = String.Format("<p>Looking for a <b>{0}</b> for <b>{1}</b> Weeks </p> <br/><span><i class='glyphicon glyphicon-map-marker'></i>&nbsp;{2}</span> <span>|</span> <span><i class='glyphicon glyphicon-phone'></i>&nbsp;{3}</span> <br/>", Convert.ToString(item["EquipmentType"]), Convert.ToString(item["Days"]), Convert.ToString(item["Location"]), Convert.ToString(item["ContactNumber"]));
                                break;
                            case 4:
                                ViewBag.RentDetail4 = String.Format("<p>Looking for a <b>{0}</b> for <b>{1}</b> Weeks </p> <br/><span><i class='glyphicon glyphicon-map-marker'></i>&nbsp;{2}</span> <span>|</span> <span><i class='glyphicon glyphicon-phone'></i>&nbsp;{3}</span> <br/>", Convert.ToString(item["EquipmentType"]), Convert.ToString(item["Days"]), Convert.ToString(item["Location"]), Convert.ToString(item["ContactNumber"]));
                                break;
                            case 5:
                                ViewBag.RentDetail5 = String.Format("<p>Looking for a <b>{0}</b> for <b>{1}</b> Weeks </p> <br/><span><i class='glyphicon glyphicon-map-marker'></i>&nbsp;{2}</span> <span>|</span> <span><i class='glyphicon glyphicon-phone'></i>&nbsp;{3}</span> <br/>", Convert.ToString(item["EquipmentType"]), Convert.ToString(item["Days"]), Convert.ToString(item["Location"]), Convert.ToString(item["ContactNumber"]));
                                break;
                        }
                        DescCount++;
                    }
                }
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}