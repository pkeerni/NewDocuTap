using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DocuTAPUploader
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Mapping", action = "Mapping", id = UrlParameter.Optional },
                   namespaces: new[] { "DocuTAPUploader.Controller" }
            );

//            routes.MapRoute(
//     "Default", // Route name
//     "{controller}/{action}/{id}", // URL with parameters
//     new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
//     new string[] { "MyCompany.MyProject.WebMvc.Controllers" }
//);
        }
    }
}