using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Logger;

namespace TanTamApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");

            var origin = HttpContext.Current.Request.Headers["Origin"];
            if (!string.IsNullOrEmpty(origin))
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", origin);
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Origin, Content-Type, Accept, Authorization");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            }

            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.StatusCode = 200;
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        ///     Ghi log cho lỗi exception
        ///     <para>Author: PhatVT</para>
        ///     <para>Created Date: 25/12/2014</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            Response.Clear();

            CommonLogger.DefaultLogger.Error("Ghi log", exception);
            //HttpException httpException = exception as HttpException;

            //if (httpException != null)
            //{
            //    string action;

            //    switch (httpException.GetHttpCode())
            //    {
            //        case 404:
            //            // page not found
            //            action = "HttpError404";
            //            break;
            //        case 500:
            //            // server error
            //            action = "HttpError500";
            //            break;
            //        default:
            //            action = "General";
            //            break;
            //    }

            //    // clear error on server
            //    Server.ClearError();

            //    Response.Redirect(String.Format("~/Error/{0}/?message={1}", action, exception.Message));
            //}
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath != null
                   && HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.ToLower().StartsWith("~/api")
                   && HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.ToLower().StartsWith("~/portal");
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest()) HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        }
    }
}