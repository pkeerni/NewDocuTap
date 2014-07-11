
using System.Runtime.Serialization;
namespace Common.Models
{
    [DataContract]
    public class Site
    {
        [DataMember]
        public string SiteId { get; set; }
        [DataMember]
        public string SiteName { get; set; } 
    }
}