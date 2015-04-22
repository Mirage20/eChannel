using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eChannel.Models
{
    public class Channel
    {
        public int ChannelID { get; set; }
        public int WorkID { get; set; }
        public int PatientID { get; set; }
        public int SpecID { get; set; }
        public int ServiceID { get; set; }
        public int ChannelNumber { get; set; }
        public string Reason { get; set; }
        public int ChannelRating { get; set; }
        public string ChannelComments { get; set; }
    }
}