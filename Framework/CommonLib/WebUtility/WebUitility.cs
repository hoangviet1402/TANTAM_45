using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace WebUtility
{
    public static class WebUitility
    {
        /// <summary>
        ///     <para>Author: TrungLD</para>
        ///     <para>DateCreated: 22/12/2014</para>
        ///     tạo antiforgerytoken for post ajax
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString AntiForgeryTokenForAjaxPost(this HtmlHelper helper)
        {
            var antiForgeryInputTag = helper.AntiForgeryToken().ToString();
            // Above gets the following: <input name="__RequestVerificationToken" type="hidden" value="PnQE7R0MIBBAzC7SqtVvwrJpGbRvPgzWHo5dSyoSaZoabRjf9pCyzjujYBU_qKDJmwIOiPRDwBV1TNVdXFVgzAvN9_l2yt9-nf4Owif0qIDz7WRAmydVPIm6_pmJAI--wvvFQO7g0VvoFArFtAR2v6Ch1wmXCZ89v0-lNOGZLZc1" />
            var removedStart =
                antiForgeryInputTag.Replace(@"<input name=""__RequestVerificationToken"" type=""hidden"" value=""", "");
            var tokenValue = removedStart.Replace(@""" />", "");
            if (antiForgeryInputTag == removedStart || removedStart == tokenValue)
                throw new InvalidOperationException(
                    "Oops! The Html.AntiForgeryToken() method seems to return something I did not expect.");
            return new MvcHtmlString(string.Format(@"{0}:""{1}""", "__RequestVerificationToken", tokenValue));
        }

        public static string Controller(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("controller"))
                return (string)routeValues["controller"];

            return string.Empty;
        }

        public static string Action(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("action"))
                return (string)routeValues["action"];

            return string.Empty;
        }

        /// <summary>
        ///     <para>Author: PhatVT</para>
        ///     <para>DateCreated: 31/12/2014</para>
        ///     Lấy IP
        /// </summary>
        /// <returns></returns>
        public static string GetIpAddressRequest()
        {
            var ct = HttpContext.Current;
            try
            {
                var sIpAddress = ct.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(sIpAddress) || sIpAddress == "unknown")
                    return ct.Request.ServerVariables["REMOTE_ADDR"];
                var ipArray = sIpAddress.Split(',');
                return ipArray[0];
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetUserAgent()
        {
            return HttpContext.Current.Request.UserAgent;
        }

        /// <summary>
        ///     ThongNT : Lay Ip truc tiep khong uu tien Ip proxy
        /// </summary>
        /// <returns></returns>
        public static string GetIpAddressDirect()
        {
            var ct = HttpContext.Current;
            var sIPAddress = string.Empty;
            try
            {
                sIPAddress = ct.Request.ServerVariables["REMOTE_ADDR"];
                if (string.IsNullOrEmpty(sIPAddress)) return ct.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                var ipArray = sIPAddress.Split(',');
                return ipArray[0];
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///     PhatVT: Lấy domain [không có subdomain]
        /// </summary>
        /// <returns></returns>
        public static string GetDomain(string url)
        {
            var domain = string.Empty;
            try
            {
                var hostParts = new Uri(url).Host.Split('.');
                domain = string.Join(".", hostParts.Skip(Math.Max(0, hostParts.Length - 2)).Take(2));
            }
            catch
            {
            }

            return domain;
        }

        /// <summary>
        ///     PhatVT: Lấy domain [có subdomain]
        /// </summary>
        /// <returns></returns>
        public static string GetFullDomain(string url)
        {
            var domain = string.Empty;
            try
            {
                domain = new Uri(url).Host;
            }
            catch
            {
            }

            return domain;
        }

        public static string GetCurrentDomain()
        {
            try
            {
                if (HttpContext.Current == null || HttpContext.Current.Request == null ||
                    HttpContext.Current.Request.Url == null)
                    return string.Empty;

                var scheme = HttpContext.Current.Request.Url.Scheme;
                var authority = HttpContext.Current.Request.Url.Authority;

                if (string.IsNullOrEmpty(scheme) || string.IsNullOrEmpty(authority))
                    return string.Empty;

                return string.Format("{0}://{1}", scheme, authority);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///     <para>Author:TrungLD</para>
        ///     <para>DateCreated: 24/01/2015</para>
        ///     <para>Hàm dùng chung cho paging</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByProperty"></param>
        /// <param name="isAscendingOrder"></param>
        /// <param name="rowsCount"></param>
        /// <returns></returns>
        public static IQueryable<T> PagedResult<T, TResult>(IQueryable<T> query, int pageNum, int pageSize,
            Expression<Func<T, TResult>> orderByProperty, bool isAscendingOrder, out int rowsCount)
        {
            if (pageSize <= 0) pageSize = 20;

            //Total result count
            rowsCount = query.Count();

            //If page number should be > 0 else set to first page
            if (rowsCount <= pageSize || pageNum <= 0) pageNum = 1;

            //Calculate nunber of rows to skip on pagesize
            var excludedRows = (pageNum - 1) * pageSize;

            query = isAscendingOrder ? query.OrderBy(orderByProperty) : query.OrderByDescending(orderByProperty);

            //Skip the required rows for the current page and take the next records of pagesize count
            return query.Skip(excludedRows).Take(pageSize);
        }

        public static KeyValuePair<string, object> GetRouteName(string keyGet)
        {
            var t = new ViewContext();
            return t.RouteData.Values.FirstOrDefault(x => x.Key == keyGet);
        }

        public static string GenerateTimeString(DateTime dateTime)
        {
            string timeString;
            var rangeTime = DateTime.Now - dateTime;
            if (rangeTime.Days > 365)
                timeString =
                    (rangeTime.Days % 365 == 0
                        ? rangeTime.Days / 365
                        : Math.Round(Convert.ToDecimal(rangeTime.Days / 365), 1)) + " năm trước";
            else if (rangeTime.Days > 30)
                timeString =
                    (rangeTime.Days % 30 == 0
                        ? rangeTime.Days / 30
                        : Math.Round(Convert.ToDecimal(rangeTime.Days / 30), 1)) + " tháng trước";
            else if (rangeTime.Days > 0)
                timeString = rangeTime.Days + " ngày trước";
            else if (rangeTime.Hours > 0)
                timeString = rangeTime.Hours + " giờ trước";
            else if (rangeTime.Minutes > 0)
                timeString = rangeTime.Minutes + " phút trước";
            else
                timeString = "vài giây trước";

            return timeString;
        }

        public static string GetRequestReferer()
        {
            var refer = HttpContext.Current.Request.UrlReferrer;
            if (refer != null) return refer.Host;
            return null;
        }

        public static string SmartLimTitle(string strTitle, int lim)
        {
            var strReturn = string.Empty;
            var tmp = string.Empty;
            var needSplit = false;
            var needAdd2 = false;
            if (strTitle != null) tmp = strTitle;
            if (tmp.Length <= lim) return tmp;
            for (var i = 0; i < tmp.Length; i++)
            {
                strReturn = strReturn + tmp[i];
                if (i == lim && tmp[i].ToString().Trim() == string.Empty) break;

                if (i == lim && tmp[i].ToString().Trim() != string.Empty)
                {
                    needSplit = true;
                    break;
                }
            }

            if (needSplit)
            {
                var lastSpace = strReturn.LastIndexOf(' ');
                if (lastSpace > 0) strReturn = strReturn.Substring(0, lastSpace);
            }

            if (strReturn.IndexOf('(') > 0 && strReturn.IndexOf(')') < 0)
            {
                needAdd2 = true;
                strReturn = strReturn.Trim() + ")";
            }

            if (strReturn.Length < tmp.Length)
            {
                if (needAdd2)
                    strReturn = strReturn.Trim() + "..";
                else
                    strReturn = strReturn.Trim() + "...";
            }

            return strReturn;
        }

        public static bool IsMobileBrowser()
        {
            //GETS THE CURRENT USER CONTEXT
            var context = HttpContext.Current;

            //FIRST TRY BUILT IN ASP.NT CHECK
            if (context.Request.Browser.IsMobileDevice) return true;
            //THEN TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER
            if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null) return true;
            //THEN TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP
            if (context.Request.ServerVariables["HTTP_ACCEPT"] != null &&
                context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
                return true;
            //AND FINALLY CHECK THE HTTP_USER_AGENT 
            //HEADER VARIABLE FOR ANY ONE OF THE FOLLOWING
            if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                //Create a list of all mobile types
                string[] mobiles =
                {
                    "midp", "j2me", "avant", "docomo",
                    "novarra", "palmos", "palmsource",
                    "240x320", "opwv", "chtml",
                    "pda", "windows ce", "mmp/",
                    "blackberry", "mib/", "symbian",
                    "wireless", "nokia", "hand", "mobi",
                    "phone", "cdm", "up.b", "audio",
                    "SIE-", "SEC-", "samsung", "HTC",
                    "mot-", "mitsu", "sagem", "sony", "alcatel", "lg", "eric", "vx",
                    "NEC", "philips", "mmm", "xx",
                    "panasonic", "sharp", "wap", "sch",
                    "rover", "pocket", "benq", "java",
                    "pt", "pg", "vox", "amoi",
                    "bird", "compal", "kg", "voda",
                    "sany", "kdd", "dbt", "sendo",
                    "sgh", "gradi", "jb", "dddi",
                    "moto", "iphone"
                };

                //Loop through each item in the list created above 
                //and check if the header contains that text
                return mobiles.Any(s =>
                    context.Request.ServerVariables["HTTP_USER_AGENT"].ToLower().Contains(s.ToLower()));
            }

            return false;
        }

        public static bool DoesCultureExist(string cultureName)
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures).Any(culture =>
                string.Equals(culture.Name, cultureName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}