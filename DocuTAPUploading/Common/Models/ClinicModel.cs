using System.ComponentModel.DataAnnotations;
using System.Web;  
using System.Runtime.Serialization;
namespace Common.Models
{ 
    public class ClinicModel
    {
        [Required] 
        public string siteid { get; set; } 
        public string Title { get; set; } 
        public string AltText { get; set; } 
        [DataType(DataType.Html)] 
        public string Caption { get; set; } 
        [Required] 
        public HttpPostedFileBase clinicfile { get; set; } 
    }
}