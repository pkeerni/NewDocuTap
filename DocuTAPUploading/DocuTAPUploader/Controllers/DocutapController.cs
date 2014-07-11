using Common.DataModels;
using Common.Models;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Web.Mvc;

namespace DocuTAPUploader.Controllers
{
    public class DocutapController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DocutapUpload()
        {
            using (DocutapService.DocutapClient docutapClient = new DocutapService.DocutapClient())
            {
                ViewBag.Sites = new SelectList(docutapClient.AllSiteIds(), "SiteId", "SiteName");
            }
            return View();
        }


        [HttpPost]
        public ActionResult DocutapUpload(Common.Models.DocutapModel docutapModel)
        {
            Common.DataModels.DocutapModel docuTapDataModel = new Common.DataModels.DocutapModel()
            {
                AltText = docutapModel.AltText,
                Caption = docutapModel.Caption,
                docutapfile = docutapModel.docutapfile,
                siteid = docutapModel.siteid,
                Title = docutapModel.Title
            };

            //Email should be sent to client Email Id. 
            using (DocutapService.DocutapClient docutapClient = new DocutapService.DocutapClient())
            { 
                // Making call to DocuTap Service to upload Content
                using(BinaryReader b = new BinaryReader(docuTapDataModel.docutapfile.InputStream))
                {
                    byte[] data = b.ReadBytes(docuTapDataModel.docutapfile.ContentLength);
                    string filename = docutapClient.UploadContent(docuTapDataModel.siteid, data, docuTapDataModel.docutapfile.FileName);

                if (!(filename.Equals(string.Empty)))
                {
                    // TODO: Display a success message with filename (customisation)
                }
                }
            }   
            return RedirectToAction("DocutapUpload");
        }

        /// <summary>
        /// For Downloading the Excel File 
        /// </summary>
        /// <param name="siteid"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ActionResult Download(string siteid, string filename)
        {
            var path = string.Empty;
            var Domain = string.Empty;
            var contentType = string.Empty;
            var textFile = string.Format("{0}{1}.txt", Server.MapPath(ConfigurationManager.AppSettings["DownloadableStatus"]), siteid);
            var MapPathTextFile = Server.MapPath(textFile);
            var tr = new StreamReader(MapPathTextFile);
            var data = tr.ReadLine().Split(',');
            if (data[0].ToString() == "True")
            {
                path = HttpContext.Server.MapPath(string.Format("~/Content/DocutapUpload/{0}/{1}",siteid, filename));
                var defaultPort = (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                Domain = string.Format("{0}{1}{2}{3}", Request.Url.Scheme, System.Uri.SchemeDelimiter, Request.Url.Host, defaultPort);
                contentType = "application/vnd.ms-excel";
                //string fullpathfilename = ConfigurationManager.AppSettings["DocutapDirectory"] + siteid + "/" + filename;
                tr.Close();
                ExpireSite(siteid, textFile); 
                return File(path, contentType, filename);
            }
            else
            {
                return RedirectToAction("Index", "Docutap");
            }
        }

        private void ExpireSite(string siteid, string textFile)
        {
            var twText = System.IO.File.CreateText(textFile);
            var mappedData = string.Format("False,{0}", siteid);
            twText.WriteLine(mappedData);
            twText.Close();
        } 
    }
}
