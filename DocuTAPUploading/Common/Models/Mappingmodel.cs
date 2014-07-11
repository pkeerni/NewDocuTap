﻿using System.ComponentModel.DataAnnotations; 
using System.Runtime.Serialization;

namespace Common.Models
{ 
    public class Mappingmodel
    {
        [Required]
        [Display(Name = "Site ID")] 
        public string SiteID { get; set; }

        [Required]
        [Display(Name = "Clinic Email ID")]
        [DataType(DataType.EmailAddress)] 
        public string ClinicEid { get; set; }

        [Required]
        [Display(Name = "Docutap Email ID")]
        [DataType(DataType.EmailAddress)] 
        public string DocutapEid { get; set; } 

    }
}