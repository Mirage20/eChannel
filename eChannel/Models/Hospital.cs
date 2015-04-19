using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eChannel.Models
{
    public class Hospital
    {
        public int HospitalID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
    }
}