using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eChannel.Models
{
    public class DoctorChannel
    {
        public int ChannelID { get; set; }
        public int WorkID { get; set; }
        public string PatientFullName { get; set; }
        public string Spec { get; set; }
        public string Service { get; set; }
        public int ChannelNumber { get; set; }
        public string Reason { get; set; }
        public int ChannelRating { get; set; }
        public string ChannelComments { get; set; }
    }
}