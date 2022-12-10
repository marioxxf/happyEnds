using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace gioiaflix
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "auth-my-account",
                "auth/myaccount",
                new { controller = "Auth", action = "MyAccount" }
            );

            routes.MapRoute(
                "auth-choose-user",
                "auth/chooseuser",
                new { controller = "Auth", action = "ChooseUserOfUserAccount" }
            );

            routes.MapRoute(
                "auth-login",
                "auth/login",
                new { controller = "Auth", action = "UserAccountLogin" }
            );

            routes.MapRoute(
                "auth-disconnect",
                "auth/disconnect",
                new { controller = "Auth", action = "UserAccountDisconnect" }
            );

            routes.MapRoute(
                "auth-new-useraccount",
                "auth/new",
                new { controller = "Auth", action = "RegisterUserAccountInterface" }
            );

            routes.MapRoute(
                "auth-register-new-useraccount",
                "auth/new/register",
                new { controller = "Auth", action = "RegisterUserAccount" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
