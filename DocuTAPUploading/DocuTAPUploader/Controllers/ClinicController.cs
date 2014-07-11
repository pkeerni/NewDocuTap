using DocuTAPUploader.Models;
using DocuTAPUploader.Models;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace DocuTAPUploader.Controllers
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
        public ActionResult ClinicUpload(ClinicModel clinicModel)
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

            if (clinicModel.clinicfile != null || clinicModel.clinicfile.ContentLength > 0)
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(ConfigurationManager.AppSettings["MappingPath"]) + "ClinicUpload");
                var clinicDirectory = Server.MapPath(ConfigurationManager.AppSettings["ClinicDirectory"]);
                var directoryName = string.Format("{0}{1}", clinicDirectory, clinicModel.siteid);
                var dir = new DirectoryInfo(directoryName);
                var filename = string.Format("{0}_{1}", clinicModel.siteid, FlatFileHelper.GetDateFormatYYYY_MM_DD_HH_MM_SS(DateTime.Now));
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
                if ((System.IO.File.Exists(filepath)) && (System.IO.File.GetLastAccessTime(filepath).Date.Date == System.DateTime.Now.Date))
                {
                    sendEMail(clinicModel.siteid, filename, true);
                }
                else
                {
                    sendEMail(clinicModel.siteid, filepath, false);
                }
                
            }
            return RedirectToAction("ClinicUpload");
        }

        /// <summary>
        /// Send Acknowledgement Mail to both
        /// </summary>
        /// <param name="siteid"></param>
        /// <param name="filename"></param>
        private void sendEMail(string siteid, string filename, bool sucessFailure)
        {

            var toMail = FlatFileHelper.GetEmailIdBySiteId(siteid, FlatFileHelper.UserTypes.Client );
            var frommail = ConfigurationManager.AppSettings["FromMail"];
            var Msg = new MailMessage();
            // Sender e-mail address.
            Msg.From = new MailAddress(frommail);

            // Recipient e-mail address.
            Msg.To.Add(toMail);

            // Subject of e-mail
            Msg.Subject = ConfigurationManager.AppSettings["SubjectSucess"];
            Msg.IsBodyHtml = true;

            string body = "Hi,";
            if (sucessFailure)
            {
                body += "<br /><br />This is to acknowledge that, I Uploaded the sheet please check your mails.";
                // body += "<br /><a href = '" + Downloadlink(siteid, filename) + "'> Click here to Download.</a>";
                body += "<br /><br />Thanks";
            }
            else
            {
                Msg.Subject = ConfigurationManager.AppSettings["SubjectFailure"];
                body += string.Format("<br/><br> File {0} uploaded for the Site {1} could not be saved.", filename, siteid);
                body += "<br /><br />Thanks";
            }
            Msg.Body = body;

            // your remote SMTP server IP.
            var smtp = new SmtpClient();
            smtp.Host = ConfigurationManager.AppSettings["SmtpServer"];
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = true;
            var NetworkCred = new NetworkCredential(frommail, ConfigurationManager.AppSettings["Password"]);
            smtp.Credentials = NetworkCred;
            smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            smtp.Send(Msg);
        }
    }
}
