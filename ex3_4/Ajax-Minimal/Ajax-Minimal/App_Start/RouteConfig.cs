using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // display one location
            routes.MapRoute("showLocations", "display/{ip}/{port}/{time}",
            defaults: new { controller = "Display", action = "showLocations" });

            // display dynamic location
            routes.MapRoute("showOneLocation", "display/{ip}/{port}",
            defaults: new { controller = "Display", action = "showOneLocation" });

            // save
            routes.MapRoute(
               name: "save",
               url: "save/{ip}/{port}/{time}/{len}/{fileName}",
               defaults: new { controller = "Save", action = "save" }
            );

            // Default
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Display", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
