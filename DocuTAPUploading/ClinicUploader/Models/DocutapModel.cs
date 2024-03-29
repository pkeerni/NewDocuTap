﻿using System.ComponentModel.DataAnnotations;
using System.Web;


namespace ClinicUploader.Models
{
    public class DocutapModel
    {
        [Required]
        public string siteid { get; set; }

        public string Title { get; set; }

        public string AltText { get; set; }

        [DataType(DataType.Html)]
        public string Caption { get; set; }

        [Required]
        //[DataType(DataType.Upload)]
        public HttpPostedFileBase docutapfile { get; set; } 
        
    }
}