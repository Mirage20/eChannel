using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eChannel.Models
{
    public class PatientChannel
    {
        public int ChannelID { get; set; }
        public int WorkID { get; set; }
        public string Spec { get; set; }
        public string Service { get; set; }
        public int ChannelNumber { get; set; }
        public string Reason { get; set; }
        public int ChannelRating { get; set; }
        public string ChannelComments { get; set; }
        public int DoctorID { get; set; }
        public string DoctorFullName { get; set; }
        public int HospitalID { get; set; }
        public string HospitalName { get; set; }
        public string HospitalLocation { get; set; }

    }
}