using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eChannel.Models;
namespace eChannel.Controllers
{
    public class HospitalController : Controller
    {
        //
        // GET: /Hospital/
        public ActionResult GetAllHospitals()
        {
            List<Hospital>hospitals= DBContext.GetInstance().FindAllInHospital();
            return Json(hospitals,JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRooms(string hospitalID)
        {
            List<Room> rooms = DBContext.GetInstance().FindAllInRoom("hospital_id",hospitalID);
 
            return Json(rooms, JsonRequestBehavior.AllowGet);
        }
	}
}