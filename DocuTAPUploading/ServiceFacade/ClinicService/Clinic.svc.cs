using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text; 
using Common;
using Common.DataModels;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using Microsoft.SqlServer;
using Common.General;
using System.Web;
using System.Web.Hosting;


namespace ServiceFacade.ClinicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Clinic" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Clinic.svc or Clinic.svc.cs at the Solution Explorer and start debugging.
    public class Clinic : IClinic
    {
        public string MacroStriping(string fileName)
        {
            return fileName;
        }

        public string UploadContent123(ClinicModel clinicModel, string directoryMapPath, string clinicDirectory, string MapFilePath, string FileDownloadStatus)
        {
            var ResultValue = string.Empty;
            if (IsValidClinic(clinicModel.siteid, FileDownloadStatus) == false)
                throw new System.Web.HttpException(403, "Unfortunately, this appear to be an invalid site. Please provide a valid Site Id");

            if (clinicModel.clinicfile != null || clinicModel.clinicfile.ContentLength > 0)
            {
                System.IO.Directory.CreateDirectory(directoryMapPath);
                var dir = new DirectoryInfo(string.Format("{0}{1}", clinicDirectory, clinicModel.siteid));
                var filename = string.Format("{0}_{1}_{2}", clinicModel.siteid, FlatFileHelper.GetDateFormatYYYY_MM_DD_HH_MM_SS(DateTime.Now), clinicModel.clinicfile.FileName);
                // Path.GetFileName(docutapModel.docutapfile.FileName));
                var filepath = System.IO.Path.Combine(dir.FullName, filename);
                if (!dir.Exists)
                {
                    Directory.CreateDirectory(string.Format("{0}{1}", clinicDirectory, clinicModel.siteid));
                }
                //UnExpireSite(docutapModel.siteid, MapFilePath, FileDownloadStatus);
                clinicModel.clinicfile.SaveAs(filepath);
                ResultValue = filename;
            }
            return ResultValue;
        }

        public string UploadContent(string siteId, byte[] data, string FileName)
        {
            var ResultValue = string.Empty;

            //if (postedFile != null || postedFile.ContentLength > 0)
            {
                bool success = true;
                try
                {                    
                    // var directoryMapPath = string.Format("{0}{1}", HostingEnvironment.MapPath(ConfigurationManager.AppSettings["MappingPath"]), "DocutapUpload");
                    // System.IO.Directory.CreateDirectory(directoryMapPath);
                    var docutapDirectory = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["ClinicDirectory"]);

                    var dir = new DirectoryInfo(string.Format("{0}{1}", docutapDirectory, siteId));
                    var filename = string.Format("{0}_{1}_{2}", siteId, FlatFileHelper.GetDateFormatYYYY_MM_DD_HH_MM_SS(DateTime.Now), FileName);
                    // Path.GetFileName(docutapModel.docutapfile.FileName));
                    var filepath = System.IO.Path.Combine(dir.FullName, filename);
                    if (!dir.Exists)
                    {
                        Directory.CreateDirectory(string.Format("{0}{1}", docutapDirectory, siteId));
                    }
                    var downloadStatusDir = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["DownloadableStatus"]);
                    var directory = new DirectoryInfo(downloadStatusDir);
                    UnExpireSite(siteId, downloadStatusDir);
                    if (IsValidClinic(siteId, downloadStatusDir) == false)
                        throw new System.Web.HttpException(403, "Unfortunately, this appear to be an invalid site. Please provide a valid Site Id");
                    using (BinaryWriter b = new BinaryWriter(File.Open(filepath, FileMode.Create)))
                    {
                        // postedFile.SaveAs(filepath);
                        b.Write(data);
                    }

                    ResultValue = filename;
                }
                catch (HttpException e)
                {
                    success = false;
                    throw;
                }
                Notifications.SendEmailToDocuTAPRep(siteId, ResultValue, success);
            }
            return ResultValue;
        }

        private bool IsValidClinic(string siteid, string downloadFileStatus)
        {
            string siteidStatusFile = string.Format("{0}{1}.txt", downloadFileStatus, siteid);
            if (System.IO.File.Exists(siteidStatusFile))
            {
                var tr = new StreamReader(siteidStatusFile);
                var data = tr.ReadLine().Split(',');
                return (data[0].ToString() == "True");
            }
            return false;
        }

        private void UnExpireSite(string siteid, string downloadStatusDir)
        {
            var dir = new DirectoryInfo(downloadStatusDir);
            if (!dir.Exists)
                Directory.CreateDirectory(downloadStatusDir);

            string mappedData = string.Format("True, {0}", siteid);
            System.IO.File.WriteAllText(string.Format("{0}{1}.txt", downloadStatusDir, siteid), mappedData);
        }
    }
}
