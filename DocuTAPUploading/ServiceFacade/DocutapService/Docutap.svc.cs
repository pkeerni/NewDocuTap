using Common;
using Common.DataModels;
using Common.General;
using System;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.Web;
using System.Web.Hosting;
using System.Collections.Generic;

namespace ServiceFacade.DocutapService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Docutap" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Docutap.svc or Docutap.svc.cs at the Solution Explorer and start debugging.
    public class Docutap : IDocutap
    {
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
                    var docutapDirectory = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["DocutapDirectory"]);

                    var dir = new DirectoryInfo(string.Format("{0}{1}", docutapDirectory, siteId));
                    var filename = string.Format("{0}_{1}_{2}", siteId, FlatFileHelper.GetDateFormatYYYY_MM_DD_HH_MM_SS(DateTime.Now), FileName);
                    // Path.GetFileName(docutapModel.docutapfile.FileName));
                    var filepath = System.IO.Path.Combine(dir.FullName, filename);
                    if (!dir.Exists)
                    {
                        Directory.CreateDirectory(string.Format("{0}{1}", docutapDirectory, siteId));
                    }

                    var downloadStatusDir = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["DownloadableStatus"]);
                    UnExpireSite(siteId, downloadStatusDir);
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
                Notifications.SendEmailToClinicRep(siteId, ResultValue, success);
            }
            return ResultValue;
        }

        public Boolean GenerateUploadLink(DocutapModel docutapModel) { return false; }

        public Boolean IsValidUpload(DocutapModel docutapModel) { return false; }

        public Boolean GenerateDL(DocutapModel docutapModel) { return false; }

        public Boolean GenerateMapping(Mappingmodel model)
        {
            var ResultValue = true;
            var mappingFilename = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["MappingFile"]);

            if (System.IO.File.Exists(mappingFilename) == false)
                using (System.IO.File.Create(mappingFilename)) ; // Touch a file

            if (FlatFileHelper.ValidateSiteId(model.SiteID) == false)
            {
                var mappedData = string.Format("{0},{1},{2}", model.SiteID, model.DocutapEid, model.ClinicEid);
                TextWriter tw = System.IO.File.AppendText(mappingFilename);
                tw.WriteLine(mappedData);
                tw.Close();
            }
            else
            {
                ResultValue = false;
            }
            return ResultValue;
        }

        public List<Site> AllSiteIds()
        {
            return FlatFileHelper.GetAllSiteIds();
        }

        private void UnExpireSite(string siteid, string downloadStatusDir)
        {
            var dir = new DirectoryInfo(downloadStatusDir);
            if (!dir.Exists)
                Directory.CreateDirectory(downloadStatusDir);

            string mappedData = string.Format("True, {0}", siteid);
            System.IO.File.WriteAllText(string.Format("{0}{1}.txt", downloadStatusDir, siteid), mappedData);
        }

        public Boolean ExpireSiteID(string siteID, string NewDirectory)
        {
            var ResultValue = false;
            Stream stream = null;
            string line = null;
            string content = null;
            string path = string.Empty;
            using (stream = new FileStream(NewDirectory, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                StreamReader tr = new StreamReader(stream);
                while ((line = tr.ReadLine()) != null)
                {
                    var siteExpireID = line.Split(',');
                    if (siteExpireID[0] == siteID)
                    {
                        ResultValue = false;
                        continue;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(content))
                            content = string.Format("{0}{1}{2}", content, Environment.NewLine, line);
                        else
                            content = line;
                    }
                }
                StreamWriter writer = new StreamWriter(NewDirectory);
                writer.Write(content);
                writer.Close();
            }

            if (stream != null)
                stream.Close();

            return ResultValue;
        }
    }
}