using System.Web.Mvc;
using System.Web.Routing;

namespace TanTamApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            
            // ESP32-CAM route - chấp nhận cả esp32 và ESP32
            routes.MapRoute(
                "ESP32",
                "esp32/{action}/{id}",
                new { controller = "ESP32Mvc", action = "Index", id = UrlParameter.Optional }
            );
            
            // Thêm route cho ESP32 (chữ hoa)
            routes.MapRoute(
                "ESP32Upper",
                "ESP32/{action}/{id}",
                new { controller = "ESP32Mvc", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}