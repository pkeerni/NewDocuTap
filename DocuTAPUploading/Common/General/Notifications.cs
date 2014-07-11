using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Common.General
{
    public class Notifications
    {
        public static void SendEmailToDocuTAPRep(string siteid, string filename, bool success)
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

            string body = "";
            if (success)
            {
                body = "<br /><br />Hello. This is a notification for activity on <b>" + siteid + "</b>. Name of the file uploaded:" + Path.GetFileName(filename) + "<br /><br />Thanks"; 
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
            smtp.Send(Msg);
        }

        public static void SendEmailToClinicRep(string siteid, string filename, bool isSuccessful)
        {
            var toMail = FlatFileHelper.GetEmailIdBySiteId(siteid, FlatFileHelper.UserTypes.DocuTap);
            var frommail = ConfigurationManager.AppSettings["FromMail"];
            var Msg = new MailMessage();

            // Sender e-mail address.
            Msg.From = new MailAddress(frommail);
            // Recipient e-mail address.
            Msg.To.Add(toMail);
            // Subject of e-mail
            Msg.Subject = ConfigurationManager.AppSettings["Subject"];
            Msg.IsBodyHtml = true;

            var body = "Hi,";
            if (isSuccessful)
            {
                body += "<br /><br />Please click the following link to download Excel Sheet";
                // body += string.Format("<br /><a href ={0}{1}>Click here to Download.</a>", Domain, url.Action("Download", "Docutap", new { siteid = siteid, filename = filename }));
                body += string.Format("<br /><a href ={0}>Click here to Download.</a>", GenerateDownloadUrl(siteid, filename));
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
            smtp.Send(Msg);

        }

        /// <summary>
        /// Generates a download url from site id and file name for a Clinic Representative to download.
        /// Note that this is a url with the clinic controller as DocuTAP controller is not accessible from external systems
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static string GenerateDownloadUrl(string siteId, string filename)
        {
            var request = new HttpRequest("/", ConfigurationManager.AppSettings["ClinicDownloadURL"], "");
            var response = new HttpResponse(new StringWriter());
            var httpContext = new HttpContext(request, response);
            var httpContextBase = new HttpContextWrapper(httpContext);
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContextBase, routeData);
            var url = new UrlHelper(requestContext);

            var port = (request.Url.IsDefaultPort ? "" : ":" + request.Url.Port);
            var Domain = string.Format("{0}{1}{2}{3}", request.Url.Scheme, System.Uri.SchemeDelimiter, request.Url.Host, port);

            // temp is always null. don't know how to fix it.
            string temp = url.Action("Download", "Docutap", new { siteid = siteId, filename = filename });
            return "Missing";
        }
    }
}
