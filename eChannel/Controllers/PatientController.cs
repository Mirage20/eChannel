using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eChannel.Models;
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

            if (Request.HttpMethod.Equals("POST"))
            {
                Patient OldExisting = DBContext.GetInstance().FindOneInPatient("patient_id", Session["userID"].ToString());

                OldExisting.FirstName = Request.Form["firstName"];
                OldExisting.LastName = Request.Form["lastName"];
                OldExisting.PhoneNumber = Request.Form["phone"];
                OldExisting.Birthdate = Request.Form["birthdate"].Equals("") ? new DateTime(0001, 01, 01) : Convert.ToDateTime(Request.Form["birthdate"]);
                OldExisting.Gender = Request.Form["gender"];
                HttpPostedFileBase file = Request.Files["picture"];

                if (file != null && file.ContentLength > 0)
                {
                    System.IO.Stream fileStream = file.InputStream;
                    byte[] data = new byte[file.ContentLength];
                    fileStream.Read(data, 0, data.Length);
                    fileStream.Close();
                    OldExisting.Picture = data;
                }
                DBContext.GetInstance().UpdatePatient(OldExisting);
                ViewData["success"] = 1;
            }

            Patient existing = DBContext.GetInstance().FindOneInPatient("patient_id", Session["userID"].ToString());
            ViewData["patient"] = existing;
            return View();
        }
    }
}