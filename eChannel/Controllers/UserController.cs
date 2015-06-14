using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eChannel.Models;
using System.Web.Helpers;
namespace eChannel.Controllers
{
    public class UserController : Controller
    {
      
        public ActionResult RegisterDoctor()
        {
            if (Session["isDoctor"] != null)
            {
                if ((bool)Session["isDoctor"])
                {
                    return RedirectToAction("Dashboard", "Doctor");
                }
                else
                {
                    return RedirectToAction("Dashboard", "Patient");
                }

            }

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
                    Password =Crypto.SHA256(Request.Form["password"].ToString()),
                    Username = Request.Form["username"]
                };
                DBContext.GetInstance().CreateDoctorLogin(newDoctorLogin);
                ViewData["success"] = 1;
            }
                      
            return View();
        }

        public ActionResult RegisterPatient()
        {

            if (Session["isDoctor"] != null)
            {
                if ((bool)Session["isDoctor"])
                {
                    return RedirectToAction("Dashboard", "Doctor");
                }
                else
                {
                    return RedirectToAction("Dashboard", "Patient");
                }

            }

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
                    Password = Crypto.SHA256(Request.Form["password"].ToString()),
                    Username = Request.Form["username"]
                };
                DBContext.GetInstance().CreatePatientLogin(newDoctorLogin);
                ViewData["success"] = 1;
            }

            return View();
        }

        public ActionResult LoginPatient()
        {
            if (Session["isDoctor"] != null)
            {
                if ((bool)Session["isDoctor"])
                {
                    return RedirectToAction("Dashboard", "Doctor");
                }
                else
                {
                    return RedirectToAction("Dashboard", "Patient");
                }

            }

            ViewData["success"] = 0;
            ViewData["hasError"] = 0;
            ViewData["errorMsg"] = "";
            
            if (Request.HttpMethod.Equals("POST"))
            {
                PatientLogin existing=DBContext.GetInstance().FindOneInPatientLogin("username", Request.Form["username"]);
                if (existing != null && existing.Password.Equals(Crypto.SHA256(Request.Form["password"].ToString())))
                {
                    
                    ViewData["success"] = 1;
                    Session["userID"] = existing.PatientID;
                    Session["username"]=existing.Username;
                    Session["isDoctor"] = false;
                    Patient patient=DBContext.GetInstance().FindOneInPatient("patient_id",existing.PatientID.ToString());
                    Session["picture"] = "data:image/png;base64," + Convert.ToBase64String(patient.Picture);
                    return RedirectToAction("Dashboard","Patient");
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

            if (Session["isDoctor"]!=null)
            {
                if((bool)Session["isDoctor"])
                {
                    return RedirectToAction("Dashboard", "Doctor");
                }
                else
                {
                    return RedirectToAction("Dashboard", "Patient");
                }

            }

            ViewData["success"] = 0;
            ViewData["hasError"] = 0;
            ViewData["errorMsg"] = "";

            if (Request.HttpMethod.Equals("POST"))
            {
                DoctorLogin existing = DBContext.GetInstance().FindOneInDoctorLogin("username", Request.Form["username"]);
                if (existing != null && existing.Password.Equals(Crypto.SHA256(Request.Form["password"].ToString())))
                {
                    
                    ViewData["success"] = 1;
                    Session["userID"] = existing.DoctorID;
                    Session["username"] = existing.Username;
                    Session["isDoctor"] = true;
                    Doctor doctor = DBContext.GetInstance().FindOneInDoctor("doctor_id", existing.DoctorID.ToString());
                    Session["picture"] = "data:image/png;base64," + Convert.ToBase64String(doctor.Picture);
                    return RedirectToAction("Dashboard", "Doctor");
                }
                else
                {
                    ViewData["hasError"] = 1;
                    ViewData["errorMsg"] = "Username or Password not match";
                }

            }

            return View();
        }

        public ActionResult Logout()
        {
            if (Session["isDoctor"] != null)
            {
                bool isDoctor=(bool)Session["isDoctor"];
                Session.Clear();
                Session.Abandon();
                if (isDoctor)
                {
                    return RedirectToAction("LoginDoctor", "User");
                }
                else
                {
                    return RedirectToAction("LoginPatient", "User");
                }
            }
            return RedirectToAction("Index", "Home");
        }
	}
}