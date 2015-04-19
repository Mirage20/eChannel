using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eChannel.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public int HospitalID { get; set; }
        public string RoomName { get; set; }
        public int Capacity { get; set; }
    }
}