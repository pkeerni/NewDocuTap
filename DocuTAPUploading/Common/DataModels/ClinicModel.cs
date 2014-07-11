
using System;
using System.Runtime.Serialization;
using System.Web;
namespace Common.DataModels
{
    [Serializable]
    public class ClinicModel
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
        private HttpPostedFileBase _clinicfile;

        public HttpPostedFileBase clinicfile
        {
            get { return _clinicfile; }
            set { _clinicfile = value; }
        }

    }
}