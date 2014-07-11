using Common.DataModels;
using Common.Models;
using System.Configuration;
using System.Web.Mvc; 

namespace DocuTAPUploader.Controllers
{
    public class MappingController : Controller
    { 
        public ActionResult Index()
        { 
            return View();
        }

        public ActionResult Mapping()
        {
            ViewBag.MessageInfo = "Provide unique Site Id, Valid Doctup and Clinic Email Id";
            return View();
        }

        [HttpPost]
        public ActionResult Mapping(Common.Models.Mappingmodel model)
        { 
            if (ModelState.IsValid)
            {
                Common.DataModels.Mappingmodel MappingDataModels = new Common.DataModels.Mappingmodel()
                {
                    ClinicEid = model.ClinicEid,
                    DocutapEid = model.DocutapEid,
                    SiteID = model.SiteID
                };

                using (DocutapService.DocutapClient docutapClient = new DocutapService.DocutapClient())
                {
                    if (!docutapClient.GenerateMapping(MappingDataModels))
                    {
                        ViewBag.MessageInfo = "Site ID exists. Please provide Unique Site Id.";
                    }
                }
            }
            else
            {
                ViewBag.MessageInfo = "Error adding Site";
            }
            return View();
        }
    }
}
