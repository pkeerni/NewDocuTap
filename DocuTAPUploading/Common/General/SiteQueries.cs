using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Common.General
{
    public class SiteQueries
    {
        public bool IsValidClinic(string siteid)
        {
            /*
            string siteidStatusFile = Server.MapPath(string.Format("{0}{1}.txt", ConfigurationManager.AppSettings["DownloadableStatus"], siteid));
            if (System.IO.File.Exists(siteidStatusFile))
            {
                var tr = new StreamReader(siteidStatusFile);
                var data = tr.ReadLine().Split(',');
                return (data[0].ToString() == "True");
            }
             * */
            return false;
        }
    }
}
