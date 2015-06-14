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
                defaults: new { controller = "Home", action = "Index" }
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

            routes.MapRoute(
               name: "SettingsDoctor",
               url: "settings-doctor",
               defaults: new { controller = "Doctor", action = "Settings" }
           );

            routes.MapRoute(
                name: "SettingsPatient",
                url: "settings-patient",
                defaults: new { controller = "Patient", action = "Settings" }
            );

            //Hospital

            routes.MapRoute(
                name: "GetAllHospitals",
                url: "get-all-hospitals",
                defaults: new { controller = "Hospital", action = "GetAllHospitals" }
            );

            routes.MapRoute(
                name: "GetRooms",
                url: "get-hospital-rooms/{hospitalID}",
                defaults: new { controller = "Hospital", action = "GetRooms" }
            );

            //Doctor

            routes.MapRoute(
                name: "DoctorAddSchedule",
                url: "dashboard-doctor-add-schedule",
                defaults: new { controller = "Doctor", action = "AddSchedule" }
            );

            routes.MapRoute(
                name: "DoctorMySchedules",
                url: "dashboard-doctor-my-schedules",
                defaults: new { controller = "Doctor", action = "MySchedules" }
            );

            routes.MapRoute(
                name: "DoctorViewHospitals",
                url: "dashboard-doctor-view-hospitals",
                defaults: new { controller = "Doctor", action = "ViewHospitals" }
            );

            routes.MapRoute(
                name: "DoctorViewChannels",
                url: "dashboard-doctor-view-channels",
                defaults: new { controller = "Doctor", action = "ViewChannels"}
            );

            routes.MapRoute(
                name: "GetAllSpecializations",
                url: "get-all-specializations",
                defaults: new { controller = "Doctor", action = "GetAllSpecializations" }
            );

            routes.MapRoute(
            name: "GetAllServices",
            url: "get-all-services",
            defaults: new { controller = "Doctor", action = "GetAllServices" }
            );

            routes.MapRoute(
            name: "GetDoctorScheduleBySpecializationID",
            url: "get-doctor-schedule-by-specialization/{specializationID}",
            defaults: new { controller = "Doctor", action = "GetDoctorScheduleBySpecializationID" }
            );

            routes.MapRoute(
            name: "GetDoctorChannelPatient",
            url: "get-doctor-channel-patient/{channelID}",
            defaults: new { controller = "Doctor", action = "GetChannelPatient" }
            );

            //Patient

            routes.MapRoute(
                name: "PatientAddChannel",
                url: "dashboard-patient-add-channel",
                defaults: new { controller = "Patient", action = "AddChannel" }
            );

            routes.MapRoute(
                name: "PatientMyChannels",
                url: "dashboard-patient-my-channels",
                defaults: new { controller = "Patient", action = "MyChannels" }
            );

            routes.MapRoute(
            name: "GetChannel",
            url: "get-patient-channel/{channelID}",
            defaults: new { controller = "Patient", action = "GetChannel" }
            );

            routes.MapRoute(
            name: "PatientViewDoctors",
            url: "get-patient-view-doctors",
            defaults: new { controller = "Patient", action = "ViewDoctors" }
            );

            //routes.MapRoute(
            //    name: "DoctorViewHospitals",
            //    url: "dashboard-doctor-view-hospitals",
            //    defaults: new { controller = "Doctor", action = "ViewHospitals" }
            //);

        }
    }
}
