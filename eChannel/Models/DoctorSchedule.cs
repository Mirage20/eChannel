using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eChannel.Models
{
    public class DoctorSchedule
    {
        public int WorkID { get; set; }
        public string DoctorName { get; set; }
        public string HospitalName { get; set; }
        public string RoomName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int MaxChannels { get; set; }
        public int PatientApplied { get; set; }
    }
}