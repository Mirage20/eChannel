using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eChannel.Controllers
{
    public class UserController : Controller
    {
      
        public ActionResult RegisterDoctor()
        {
            if (Request.HttpMethod.Equals("POST"))
            {
                //ViewData["username"] = Request.Form["username"];
            }

            return View();
        }

        public ActionResult RegisterPatient()
        {
            return View();
        }
	}
}