using Common.DataModels;
using Common.Models;
using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;

namespace DocuTAPUploader.Controllers
{
    public class ExpireController : Controller
    {
        //
        // GET: /Expire/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExpireSiteID()
        {
            using (DocutapService.DocutapClient docutapClient = new DocutapService.DocutapClient())
            {
                ViewBag.Sites = new SelectList(Common.FlatFileHelper.GetAllSiteIds(), "SiteId", "SiteName");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ExpireSiteID(Common.Models.ExpireModel expiremodel)
        {
            var NewDirectory = Server.MapPath((ConfigurationManager.AppSettings["MappingFile"]));
            ViewBag.Sites = new SelectList(Common.FlatFileHelper.GetAllSiteIds(), "SiteId", "SiteName"); 
            try
            { 
                using (DocutapService.DocutapClient docutapClient = new DocutapService.DocutapClient())
                {
                    if (docutapClient.ExpireSiteID(expiremodel.siteid, NewDirectory))
                    {
                        ViewBag.MessageInfo = String.Format("Site Id {0} is expired.", expiremodel.siteid);
                    }
                    else
                    {
                        ViewBag.MessageInfo = String.Format("Site Id {0} can't be deleted as its being used..! ", expiremodel.siteid);
                    }
                }
            }
            catch (IOException)
            {
                ViewBag.MessageInfo = String.Format("Site Id {0} can't be deleted as its being used..! ", expiremodel.siteid);
            }  
            return View();
        }
    }
}
