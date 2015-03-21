using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eChannel.Controllers
{
    public class DoctorController : Controller
    {

        public ActionResult Dashboard()
        {
            if (Session["isDoctor"] != null && !((bool)Session["isDoctor"]))
            {
                return RedirectToAction("Dashboard", "Patient");
            }
            else if (Session["isDoctor"] == null)
            {
                return RedirectToAction("LoginDoctor", "User");
            }
            return View();
        }
    }
}