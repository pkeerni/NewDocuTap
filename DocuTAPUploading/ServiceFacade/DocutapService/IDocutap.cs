using Common.DataModels;
using System;
using System.ServiceModel;
using System.Web;

namespace ServiceFacade.DocutapService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDocutap" in both code and config file together.
    [ServiceContract]
    public interface IDocutap
    {
        [OperationContract]
        string UploadContent(string siteId, byte[] data, string FileName);

        [OperationContract]
        Boolean GenerateUploadLink(DocutapModel docutapModel);

        [OperationContract]
        Boolean IsValidUpload(DocutapModel docutapModel);

        [OperationContract]
        Boolean GenerateDL(DocutapModel docutapModel);

        [OperationContract]
        Boolean GenerateMapping(Mappingmodel model);

        [OperationContract]
        Boolean ExpireSiteID(string siteID, string NewDirectory);

        [OperationContract]
        System.Collections.Generic.List<Site> AllSiteIds();
    }
}
