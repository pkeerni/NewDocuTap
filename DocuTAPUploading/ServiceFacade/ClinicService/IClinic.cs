using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text; 
using Common.DataModels;
using System.Web;

namespace ServiceFacade.ClinicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IClinic" in both code and config file together.
    [ServiceContract]
    public interface IClinic
    {
        [OperationContract]
        string MacroStriping(string fileName);

        [OperationContract]
        string UploadContent(string siteId, byte[] data, string FileName);
    }
}
