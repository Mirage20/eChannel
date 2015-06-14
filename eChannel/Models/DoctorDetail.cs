using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eChannel.Models
{
    public class DoctorDetail
    {
        public int DoctorID { get; set; }    
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }
        public string Email { get; set; }
        public float AverageRating { get; set; }
        public byte[] Picture { get; set; }

        
    }

}