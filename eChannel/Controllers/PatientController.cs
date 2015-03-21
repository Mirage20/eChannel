using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eChannel.Controllers
{
    public class PatientController : Controller
    {

        public ActionResult Dashboard()
        {
            if (Session["isDoctor"] != null && (bool)Session["isDoctor"])
            {
                return RedirectToAction("Dashboard", "Doctor");
            }
            else if (Session["isDoctor"] == null)
            {
                return RedirectToAction("LoginPatient", "User");
            }
            return View();
        }

        public ActionResult Settings()
        {
            if (Session["isDoctor"] != null && (bool)Session["isDoctor"])
            {
                return RedirectToAction("Dashboard", "Doctor");
            }
            else if (Session["isDoctor"] == null)
            {
                return RedirectToAction("LoginPatient", "User");
            }
            return View();
        }
    }
}