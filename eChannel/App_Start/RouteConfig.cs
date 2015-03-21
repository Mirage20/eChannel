using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eChannel
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
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index"}
            );

            routes.MapRoute(
                name: "RegisterAsDoctor",
                url: "register-doctor",
                defaults: new { controller = "User", action = "RegisterDoctor" }
            );

            routes.MapRoute(
                name: "RegisterAsPatient",
                url: "register-patient",
                defaults: new { controller = "User", action = "RegisterPatient" }
            );
        }
    }
}
