using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eChannel.Models
{
    public class Patient
    {
        public PatientLogin PatientLogin { get;private  set; }       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public DateTime Birthdate { get; set; }
        public string Gender { get; set; }
        public byte[] Picture { get; set; }

        public Patient(PatientLogin patientLogin)
        {
            PatientLogin = patientLogin;
        }
    }
}