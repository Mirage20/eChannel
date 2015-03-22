using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eChannel.Models;
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
        public ActionResult Settings()
        {
            if (Session["isDoctor"] != null && !((bool)Session["isDoctor"]))
            {
                return RedirectToAction("Dashboard", "Patient");
            }
            else if (Session["isDoctor"] == null)
            {
                return RedirectToAction("LoginDoctor", "User");
            }

            if (Request.HttpMethod.Equals("POST"))
            {
                Doctor OldExisting = DBContext.GetInstance().FindOneInDoctor("doctor_id", Session["userID"].ToString());

                OldExisting.FirstName = Request.Form["firstName"];
                OldExisting.LastName = Request.Form["lastName"];
                OldExisting.PhoneNumber = Request.Form["phone"];
                OldExisting.Gender = Request.Form["gender"];
                HttpPostedFileBase file = Request.Files["picture"];

                if (file != null && file.ContentLength > 0)
                {
                    System.IO.Stream fileStream = file.InputStream;
                    byte[] data= new byte[file.ContentLength];
                    fileStream.Read(data, 0, data.Length);
                    fileStream.Close();
                    OldExisting.Picture = data;
                }
                DBContext.GetInstance().UpdateDoctor(OldExisting);
                ViewData["success"] = 1;
            }

            Doctor existing = DBContext.GetInstance().FindOneInDoctor("doctor_id", Session["userID"].ToString());
            ViewData["doctor"] = existing;
            return View();
        }
    }
}