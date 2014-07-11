using Common.DataModels;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Hosting;

namespace Common
{
    public static class FlatFileHelper
    {
        public enum UserTypes
        {
            DocuTap,
            Client
        };

        /// <summary>
        /// Validates that the given site id is an existing site
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns>True if the site id exists. False otherwise</returns>
        public static bool ValidateSiteId(string siteId)
        {
            var line = string.Empty;
            var fileName = HostingEnvironment.MapPath((ConfigurationManager.AppSettings["MappingFile"]));
            if ((siteId == null) || (siteId == string.Empty))
                return false;

            foreach (var oLine in GetFlatFileLines(fileName))
            {
                var lineArray = oLine.Split(',');
                if (lineArray[0].Trim().Equals(siteId.Trim()))
                {
                    return true;
                }
            }
            return false;
        }
         
        /// <summary>
        /// Sends an email based on the site id and who is sending this email. If Docutap representative is sending then the clinic gets the email and vice versa
        /// </summary>
        /// <param name="siteId"> site id as listed in the site to email mapping</param>
        /// <param name="DocClient">Identifier for who is sending this email</param>
        /// <returns></returns>
        public static string GetEmailIdBySiteId(string siteId, UserTypes DocClient) 
        {
            var emailId = string.Empty;
            var fileName = HostingEnvironment.MapPath((ConfigurationManager.AppSettings["MappingFile"]));
            try
            {
                if ((siteId == null) || (siteId == string.Empty))
                    return emailId;
                foreach (var oLine in GetFlatFileLines(fileName))
                {
                    var lineArray = oLine.Split(',');
                    if (lineArray[0].Trim().Equals(siteId.Trim()))
                    {
                        if (DocClient == UserTypes.Client)
                        {
                            emailId = lineArray[1];
                        }
                        else
                        {
                            emailId = lineArray[2];
                        }
                        return emailId;
                    }
                }
            }
            catch
            {
            }
            return emailId;
        }


        public static List<Site> GetAllSiteIds()
        {
            var siteIds = new List<Site>();
            var line = string.Empty;
            var fileName = HostingEnvironment.MapPath((ConfigurationManager.AppSettings["MappingFile"]));
            try
            {
                foreach (var oLine in GetFlatFileLines(fileName))
                {
                    var lineArray = oLine.Split(',');
                    siteIds.Add(new Site() { SiteId  = lineArray[0],  SiteName  = lineArray[0] });
                }
            }
            catch
            {
            }
            return siteIds;
        }

        private static IEnumerable<string> GetFlatFileLines(string fileName)
        {
            string line;
            try
            {
                using (var fileStream = new StreamReader(fileName))
                {

                }
            }
            catch (System.Exception ex)
            {

            }
            using (var fileStream = new StreamReader(fileName))
            {
                while ((line = fileStream.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public static string GetDateFormatYYYY_MM_DD_HH_MM_SS(System.DateTime dateTime)
        { 
            return dateTime.ToString("yyyy_MM_dd_HH'_'mm'_'ss");
        }
    }
}