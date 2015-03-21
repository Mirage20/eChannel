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
            ViewData["errorMsg"] = "";

            if (Request.HttpMethod.Equals("POST"))
            {
                if(DBContext.GetInstance().FindOneInDoctorLogin("username",Request.Form["username"])!=null)
                {
                    ViewData["hasError"] = 1;
                    ViewData["errorMsg"] = "Username alrady exist";
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
            ViewData["success"] = 0;
            ViewData["hasError"] = 0;
            ViewData["errorMsg"] = "";

            if (Request.HttpMethod.Equals("POST"))
            {
                if (DBContext.GetInstance().FindOneInPatientLogin("username", Request.Form["username"]) != null)
                {
                    ViewData["hasError"] = 1;
                    ViewData["errorMsg"] = "Username alrady exist";
                    return View();
                }
                PatientLogin newDoctorLogin = new PatientLogin()
                {
                    Email = Request.Form["email"],
                    Password = Request.Form["password"],
                    Username = Request.Form["username"]
                };
                DBContext.GetInstance().CreatePatientLogin(newDoctorLogin);
                ViewData["success"] = 1;
            }

            return View();
        }

        public ActionResult LoginPatient()
        {
            ViewData["success"] = 0;
            ViewData["hasError"] = 0;
            ViewData["errorMsg"] = "";
            
            if (Request.HttpMethod.Equals("POST"))
            {
                PatientLogin existing=DBContext.GetInstance().FindOneInPatientLogin("username", Request.Form["username"]);
                if (existing != null && existing.Password.Equals(Request.Form["password"]))
                {

                    ViewData["success"] = 1;
                    Session["userID"] = existing.PatientID;
                    Session["username"]=existing.Username;
                    Session["isDoctor"] = false;
                    return View();
                }
                else
                {
                    ViewData["hasError"] = 1;
                    ViewData["errorMsg"] = "Username or Password not match";
                }
                
            }

            return View();
        }


        public ActionResult LoginDoctor()
        {
            
            return View();
        }

	}
}