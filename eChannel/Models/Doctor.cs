using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eChannel.Models
{
    public class Doctor
    {
        public DoctorLogin DoctorLogin { get;private  set; }       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }
        public byte[] Picture { get; set; }

        public Doctor(DoctorLogin doctorLogin)
        {
            DoctorLogin = doctorLogin;
        }
    }

}