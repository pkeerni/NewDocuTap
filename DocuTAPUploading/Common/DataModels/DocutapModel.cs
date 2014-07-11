
using System;
using System.Runtime.Serialization;
using System.Web;

namespace Common.DataModels
{
    [DataContract]
    public class DocutapModel
    {  
        [DataMember]
        public string siteid { get; set; }
        [DataMember] 
        public string Title { get; set; }
         [DataMember]
        public string AltText { get; set; }
         [DataMember]
        public string Caption { get; set; }


        [NonSerialized]
        private HttpPostedFileBase _docutapfile;
 
        public HttpPostedFileBase docutapfile
        {
            get { return _docutapfile; }
            set { _docutapfile = value; }
        }
    }
}