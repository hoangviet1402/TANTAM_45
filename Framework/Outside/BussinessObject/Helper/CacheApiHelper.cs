/**********************************************************************
 * Author: LongNP
 * DateCreate: 2017-08-15
 * Description: load data from cache api
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using Logger;
using MyConfig;
using MyUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace BussinessObject.Helper
{
    public class CacheApiHelper
    {
        private static string GetCacheApiDomain()
        {
            return "";
        }


        #region country

        public static string GetCountryCodeFromCacheApi(string ip)
        {
            //if (!MyConfiguration.CacheApi.Enable)
            //    return string.Empty;

            try
            {
                var url = "{0}/api/country/getcountrycode?ip={1}&unix={2}&sign={3}";
                var unix = Common.ConvetDateTimeToUnixTimeSpan(DateTime.Now);
                var sign = Common.MD5_encode(string.Format("{0}{1}{2}", ip, unix, "MyConfiguration.CacheApi.CacheKey"));
                //url = string.Format(url, MyConfiguration.CacheApi.CacheDomain, ip, unix, sign);
                url = string.Format(url, GetCacheApiDomain(), ip, unix, sign);
                var result = NetworkCommon.SendGetRequest(url);

                //if (DateTime.Now <= new DateTime(2020, 3, 26, 16, 0, 0))
                //    CommonLogger.DefaultLogger.DebugFormat("GetCountryCodeFromCacheApi ip: {0} -- result: {1}", ip, result);

                if (string.IsNullOrEmpty(result))
                    return string.Empty;

                return result;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("GetCountryCodeFromCacheApi", ex);
                return string.Empty;
            }
        }

        public static string GetIpRegisterFromCacheApi(string ip)
        {
            //if (!MyConfiguration.CacheApi.Enable)
            //    return string.Empty;

            // {domain}/api/country/ipfullinfo?ip=14.161.22.186
            var url = "{0}/api/country/ipfullinfo?ip={1}";
            url = string.Format(url, GetCacheApiDomain(), ip);
            var result = NetworkCommon.SendGetRequestSimple(url);

            if (string.IsNullOrEmpty(result))
                return string.Empty;

            return result;
        }

        #endregion

        #region simple

        /*
        [GET]api/cache/get?key=asdsd
        [GET]api/cache/delete?key=asdsd
        [POST]api/cache/set?key=asdsd&time=123 [BODY]data
        */

        public static bool Simple_Get<T>(string key, ref T result)
        {
            //if (!MyConfiguration.CacheApi.Enable)
            //    return false;

            var url = "{0}/api/cache/get?key={1}";
            url = string.Format(url, GetCacheApiDomain(), key);

            var response = NetworkCommon.SendGetRequest(url);

            if (string.IsNullOrEmpty(response))
                return false;

            if (typeof(T) == typeof(string))
                result = (T)(object)response;
            else
                result = Common.TryDeserializeObject<T>(response);

            return true;
        }

        public static bool Simple_Set(string key, string data, int time)
        {
            //if (!MyConfiguration.CacheApi.Enable)
            //    return false;

            try
            {
                var url = "{0}/api/cache/set?key={1}&time={2}";
                url = string.Format(url, GetCacheApiDomain(), key, time);
                NetworkCommon.SendPOSTRequestJsonSimple(url, data, "text/plain;charset=utf-8");

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }
}