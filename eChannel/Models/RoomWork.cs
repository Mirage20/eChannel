using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eChannel.Models
{
    public class RoomWork
    {
        public int WorkID { get; set; }
        public int DoctorID { get; set; }
        public int RoomID { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int MaxChannels { get; set; }
    }
}