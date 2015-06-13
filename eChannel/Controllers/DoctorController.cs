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
                    byte[] data = new byte[file.ContentLength];
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


        public PartialViewResult AddSchedule()
        {

            if (Request.HttpMethod.Equals("POST"))
            {
                RoomWork newRoomWork = new RoomWork()
                {
                    DoctorID = (int)Session["userID"],
                    RoomID = Convert.ToInt32(Request.Form["room_number"]),
                    StartDateTime = Convert.ToDateTime(Request.Form["start_time"]),
                    EndDateTime = Convert.ToDateTime(Request.Form["finish_time"]),
                    MaxChannels = Convert.ToInt32(Request.Form["max_channels"])
                };
                DBContext.GetInstance().CreateRoomWork(newRoomWork);
                ViewData["success"] = 1;

            }

            return PartialView();
        }

        public PartialViewResult MySchedules()
        {
            List<DoctorSchedule> schedules = DBContext.GetInstance().FindAllDoctorSchedule((int)Session["userID"]);
            ViewData["doctor_schedules"] = schedules;
            return PartialView();
        }

        public PartialViewResult ViewHospitals()
        {
            List<Hospital> hospitals = DBContext.GetInstance().FindAllInHospital();
            ViewData["hospitals"] = hospitals;
            return PartialView();
        }

        public PartialViewResult ViewChannels()
        {
            if (Request.HttpMethod.Equals("POST"))
            {
                string keyword = Request.Form["key"];
                List<DoctorChannel> doctorChannels = null;
                if (Request.Form["by"].Equals("workID"))
                {
                    doctorChannels = DBContext.GetInstance().FindAllInDoctorChannel((int)Session["userID"], keyword, "work_id");
                }
                else if (Request.Form["by"].Equals("patientFirstName"))
                {
                    doctorChannels = DBContext.GetInstance().FindAllInDoctorChannel((int)Session["userID"], keyword, "first_name");
                }
                else if (Request.Form["by"].Equals("patientLastName"))
                {
                    doctorChannels = DBContext.GetInstance().FindAllInDoctorChannel((int)Session["userID"], keyword, "last_name");

                }
                else if (Request.Form["by"].Equals("service"))
                {
                    doctorChannels = DBContext.GetInstance().FindAllInDoctorChannel((int)Session["userID"], keyword, "service_name");
                }
                else if (Request.Form["by"].Equals("channelNumber"))
                {
                    doctorChannels = DBContext.GetInstance().FindAllInDoctorChannel((int)Session["userID"], keyword, "channel_number");
                }
                else if (Request.Form["by"].Equals("channelRating"))
                {
                    doctorChannels = DBContext.GetInstance().FindAllInDoctorChannel((int)Session["userID"], keyword, "channel_rating");
                }

                ViewData["doctor-channels"] = doctorChannels;

                if(keyword.Equals(""))
                {
                    doctorChannels = DBContext.GetInstance().FindAllInDoctorChannel((int)Session["userID"]);
                    ViewData["doctor-channels"] = doctorChannels;
                }

            }
            else
            {
                List<DoctorChannel> doctorChannels = DBContext.GetInstance().FindAllInDoctorChannel((int)Session["userID"]);
                ViewData["doctor-channels"] = doctorChannels;
            }
            return PartialView();

        }

        public ActionResult GetAllSpecializations()
        {
            List<Specialization> specializations = DBContext.GetInstance().FindAllInSpecialization();
            return Json(specializations, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllServices()
        {
            List<Service> services = DBContext.GetInstance().FindAllInService();
            return Json(services, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChannelPatient(string channelID)
        {
            int patientID = DBContext.GetInstance().FindOneInChannel("channel_id", channelID).PatientID;
            Patient channelPatient = DBContext.GetInstance().FindOneInPatient("patient_id", patientID.ToString());
            channelPatient.PatientLogin.Username = "";
            channelPatient.PatientLogin.Password = "";

            return Json(channelPatient, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDoctorScheduleBySpecializationID(string specializationID)
        {
            List<DoctorSchedule> schedules = DBContext.GetInstance().FindAllDoctorScheduleBySpecializationID(Convert.ToInt32(specializationID));
            return Json(schedules, JsonRequestBehavior.AllowGet);
        }
    }
}