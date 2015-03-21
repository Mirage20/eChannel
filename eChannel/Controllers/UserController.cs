using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eChannel.Models;
namespace eChannel.Controllers
{
    public class UserController : Controller
    {
      
        public ActionResult RegisterDoctor()
        {
            ViewData["success"] = 0;
            ViewData["hasError"] = 0;
            ViewData["ErrorMsg"] = "";

            if (Request.HttpMethod.Equals("POST"))
            {
                if(DBContext.GetInstance().FindOneInDoctorLogin("username",Request.Form["username"])!=null)
                {
                    ViewData["hasError"] = 1;
                    ViewData["ErrorMsg"] = "User alrady exist";
                    return View();
                }
                DoctorLogin newDoctorLogin = new DoctorLogin()
                {
                    Email = Request.Form["email"],
                    Password = Request.Form["password"],
                    Username = Request.Form["username"]
                };
                DBContext.GetInstance().CreateDoctorLogin(newDoctorLogin);
                ViewData["success"] = 1;
            }
                      
            return View();
        }

        public ActionResult RegisterPatient()
        {
            return View();
        }
	}
}