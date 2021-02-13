using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace СмарТимСервисСайт
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Upl",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Upl" }
            );
            routes.MapRoute(
                name: "Dwl",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Dwl" }
                );
        }
    }
}
