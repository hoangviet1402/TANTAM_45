using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebUtility
{
    /// <summary>
    /// Quản lý cookie
    /// <para>Author: PhatVT</para>
    /// <para>Create Date: 26/12/2014</para>
    /// </summary>
    public class MyCookieManager
    {
        /// <summary>
        /// Set cookie
        /// </summary>
        /// <param name="model"></param>
        public static void Set(CookieModel model)
        {
            var cookie = new HttpCookie(model.Name, model.Value);
            if (model.ExpireTime.HasValue)
            {
                cookie.Expires = model.ExpireTime.Value;
            }
            //cookie.Domain = model.Domain;
            cookie.Path = !string.IsNullOrEmpty(model.Path) ? model.Path : "/";

            if (HttpContext.Current.Request.Cookies.Get(model.Name) != null)
            {
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
            else
            {
                HttpContext.Current.Response.Cookies.Add(cookie);
                //HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        /// <summary>
        /// Lấy thông tin cookie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Get(string name)
        {
            return HttpContext.Current.Request.Cookies[name] == null
                    ? ""
                    : HttpContext.Current.Request.Cookies[name].Value;
        }

        /// <summary>
        /// Xóa cookie
        /// </summary>
        /// <param name="name"></param>
        public static void Delete(string name)
        {
            var cookie = new HttpCookie(name, string.Empty) { Expires = DateTime.Now };
            if (HttpContext.Current.Response.Cookies.Get(name) != null)
            {
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }
    }
}
