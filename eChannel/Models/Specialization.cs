﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eChannel.Models
{
    public class DoctorSpecialization
    {
        public int DoctorID { get; set; }
        public string University { get; set; }
        public int SpecID { get; set; }
        public string SpecType { get; set; }
        public string SpecDegree { get; set; }
    }
}