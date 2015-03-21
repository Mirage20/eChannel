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

            routes.MapRoute(
                name: "LoginAsDoctor",
                url: "login-doctor",
                defaults: new { controller = "User", action = "LoginDoctor" }
            );

            routes.MapRoute(
                name: "LoginAsPatient",
                url: "login-patient",
                defaults: new { controller = "User", action = "LoginPatient" }
            );

            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new { controller = "User", action = "Logout" }
            );

            routes.MapRoute(
               name: "DashboardDoctor",
               url: "dashboard-doctor",
               defaults: new { controller = "Doctor", action = "Dashboard" }
           );

            routes.MapRoute(
                name: "DashboardPatient",
                url: "dashboard-patient",
                defaults: new { controller = "Patient", action = "Dashboard" }
            );
        }
    }
}
