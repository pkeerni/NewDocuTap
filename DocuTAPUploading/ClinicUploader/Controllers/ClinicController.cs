using Common;
using Common.Models;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

using Common.DataModels;
using Common.Models;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Web.Mvc;

namespace ClinicUploader.Controllers
{
    public class ClinicController : Controller
    {
        //
        // GET: /Clinic/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClinicUpload()
        { 
            return View();
        }

        [HttpPost]
        public ActionResult ClinicUpload123(Common.Models.ClinicModel clinicModel)
        {

            #region BlockCode
            //Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            //Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(@"C:\\Users\\pchaturvedi\\Documents\\Empty_Macro.xlsm", Type.Missing, Type.Missing, 6, Type.Missing, Type.Missing, Type.Missing, Type.Missing, ",", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //xlApp.Visible = true;
            //if (xlWorkBook.HasVBProject)
            //{
            //    var vbCom = xlApp.VBE.ActiveVBProject.VBComponents;
            //      xlApp.DisplayAlerts =false;
            //      xlApp.ScreenUpdating = false;
            //     // vbCom.Remove VBComponent:=vbCom.Item("basPopWkshts");
            //}
            //Microsoft.Vbe.Interop.VBComponents com = xlWorkBook.VBProject.VBComponents;
            //foreach (Microsoft.Vbe.Interop.VBComponent c in com)
            //{
            //    if (c.Type == Microsoft.Vbe.Interop.vbext_ComponentType.vbext_ct_StdModule)
            //    {
            //        com.Remove(c);
            //    }                    
            //}
            #endregion

            if (IsValidClinic(clinicModel.siteid) == false)
                throw new System.Web.HttpException(403, "Unfortunately, this appear to be an invalid site. Please provide a valid Site Id");

            if (clinicModel.clinicfile != null || clinicModel.clinicfile.ContentLength > 0)
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(ConfigurationManager.AppSettings["MappingPath"]) + "ClinicUpload");
                var clinicDirectory = Server.MapPath(ConfigurationManager.AppSettings["ClinicDirectory"]);
                var directoryName = string.Format("{0}{1}", clinicDirectory, clinicModel.siteid);
                var dir = new DirectoryInfo(directoryName);
                var filename = string.Format("{0}_{1}_{2}", clinicModel.siteid, FlatFileHelper.GetDateFormatYYYY_MM_DD_HH_MM_SS(DateTime.Now), clinicModel.clinicfile.FileName);
                // Path.GetFileName(docutapModel.docutapfile.FileName));
                var filepath = System.IO.Path.Combine(dir.FullName, filename);

                if (!dir.Exists)
                {
                    Directory.CreateDirectory(directoryName);
                }
                using (var clinicClient = new ClinicService.ClinicClient())
                {

                    filepath = clinicClient.MacroStriping(filepath);
                }
                clinicModel.clinicfile.SaveAs(filepath);
                /*
                if ((System.IO.File.Exists(filepath)) && (System.IO.File.GetLastAccessTime(filepath).Date.Date == System.DateTime.Now.Date))
                {
                    SendEmailToDocuTAPRep(clinicModel.siteid, filename, true);
                }
                else
                {
                    SendEmailToDocuTAPRep(clinicModel.siteid, filepath, false);
                }
                 */
            }
            return RedirectToAction("ClinicUpload");
        }

        [HttpPost]
        public ActionResult ClinicUpload(Common.Models.ClinicModel docutapModel)
        {
            Common.DataModels.ClinicModel docuTapDataModel = new Common.DataModels.ClinicModel()
            {
                AltText = docutapModel.AltText,
                Caption = docutapModel.Caption,
                clinicfile = docutapModel.clinicfile,
                siteid = docutapModel.siteid,
                Title = docutapModel.Title
            };

            //Email should be sent to client Email Id. 
            using (ClinicService.ClinicClient clinicClient = new ClinicService.ClinicClient())
            {
                // Making call to DocuTap Service to upload Content
                using (BinaryReader b = new BinaryReader(docuTapDataModel.clinicfile.InputStream))
                {
                    byte[] data = b.ReadBytes(docuTapDataModel.clinicfile.ContentLength);
                    string filename = clinicClient.UploadContent(docuTapDataModel.siteid, data, docuTapDataModel.clinicfile.FileName);

                    if (!(filename.Equals(string.Empty)))
                    {
                        // TODO: Display a success message with filename (customisation)
                    }
                }
            }
            return RedirectToAction("DocutapUpload");
        }

        private bool IsValidClinic(string siteid)
        {
            string siteidStatusFile = Server.MapPath(string.Format("{0}{1}.txt", ConfigurationManager.AppSettings["DownloadableStatus"], siteid));
            if (System.IO.File.Exists(siteidStatusFile))
            {
                var tr = new StreamReader(siteidStatusFile);
                var data = tr.ReadLine().Split(',');
                return (data[0].ToString() == "True");
            }
            return false;
        }
    }
}
