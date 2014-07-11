
using System.Runtime.Serialization;
namespace Common.DataModels
{
    [DataContract]
    public class Mappingmodel
    { 
        [DataMember]
        public string SiteID { get; set; } 

        [DataMember]
        public string ClinicEid { get; set; }
         
        [DataMember]
        public string DocutapEid { get; set; } 

    }
}