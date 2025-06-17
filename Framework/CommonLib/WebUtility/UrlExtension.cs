using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace WebUtility
{
    public static class UrlExtension
    {
        public static string Content(this UrlHelper urlHelper, string relativePath)
        {
            return urlHelper.Content(relativePath);
        }
    }
}
