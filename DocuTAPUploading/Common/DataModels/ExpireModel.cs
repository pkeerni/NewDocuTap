
using System.Runtime.Serialization;

namespace Common.DataModels
{
    [DataContract]
    public class ExpireModel
    {
        [DataMember]
        public string siteid { get; set; }
    }
}