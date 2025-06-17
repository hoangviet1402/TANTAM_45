using System.Web.Http;

namespace TanTamApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "",
                new { Controller = "ApiHome" }
            );
        }
    }
}