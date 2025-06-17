using BussinessObject.Helper;
using TanTamApi.Models.Request;
using Logger;
using MyConfig;
using MyUtility;
using Newtonsoft.Json;
using ResxLanguagesUtility;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using WebUtility;

namespace TanTamApi.Helper
{
    public class ApiHelper
    {
        public static string GetLink(string value)
        {
            if (value.StartsWith("http:"))
                return value;

            return MyConfiguration.Default.FullDomain + value;
        }


        /// <summary>
        ///     Convert json object to specific object
        ///     <para>Author: PhatVT</para>
        ///     <para>Created Date: 02/12/2015</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Convert<T>(object data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(data));
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("Convert", ex);
                return default(T);
            }
        }

        public static T GetModelRequest<T>(NameValueCollection formData) where T : class, new()
        {
            if (formData == null)
                return default(T);

            try
            {
                var listProperties = typeof(T).GetProperties();
                if (listProperties.Length > 0)
                {
                    var obj = new T();

                    foreach (var prop in listProperties)
                    {
                        var attributes = prop.GetCustomAttributes(true);
                        foreach (var attr in attributes)
                        {
                            var displayNameAttr = attr as DisplayNameAttribute;
                            if (displayNameAttr != null)
                            {
                                var displayName = displayNameAttr.DisplayName;
                                if (formData[displayName] != null)
                                {
                                    var value = formData[displayName];

                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        // Dựa vào từng loại mà gán dữ liệu
                                        if (prop.PropertyType.ToString().Contains("Int32"))
                                        {
                                            int intValue;
                                            if (int.TryParse(value, out intValue)) prop.SetValue(obj, intValue);
                                        }
                                        else if (prop.PropertyType.ToString().Contains("Int64"))
                                        {
                                            long longValue;
                                            if (long.TryParse(value, out longValue)) prop.SetValue(obj, longValue);
                                        }
                                        else if (prop.PropertyType.ToString().Contains("Decimal"))
                                        {
                                            decimal decimalValue;
                                            if (decimal.TryParse(value, out decimalValue))
                                                prop.SetValue(obj, decimalValue);
                                        }
                                        else if (prop.PropertyType.ToString().Contains("Double"))
                                        {
                                            double doubleValue;
                                            if (double.TryParse(value, out doubleValue))
                                                prop.SetValue(obj, doubleValue);
                                        }
                                        else if (prop.PropertyType.ToString().Contains("DateTime"))
                                        {
                                            DateTime dtValue;
                                            if (DateTime.TryParse(value, out dtValue)) prop.SetValue(obj, dtValue);
                                        }
                                        else if (prop.PropertyType.ToString().Contains("Boolean"))
                                        {
                                            bool dtValue;
                                            if (bool.TryParse(value, out dtValue)) prop.SetValue(obj, dtValue);
                                        }
                                        else
                                        {
                                            prop.SetValue(obj, value);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    return obj;
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("GetModelRequest", ex);
            }

            return default(T);
        }

        public static T ConvertToCustomType<T>(object obj)
        {
            if (obj == null) return default(T);

            var inputData = JsonConvert.SerializeObject(obj);
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(inputData)))
            {
                var jdss = new DataContractJsonSerializer(typeof(T));
                var responseObj = jdss.ReadObject(ms);
                ms.Close();
                return (T)System.Convert.ChangeType(responseObj, typeof(T));
            }
        }

        public static T TryDeserializeObject<T>(string body, HttpRequestMessage request)
            where T : ApiBaseRequest, new()
        {
            var result = Common.TryDeserializeObject<T>(body);

            if (result == null)
                return null;

            if (request == null)
                return result;

            var headers = request.Headers;

            if (headers != null)
                result.UserAgent = headers.UserAgent.ToString();

            result.IpAddress = WebUitility.GetIpAddressRequest();
            result.CountrySim = result.Country;

            if (!string.IsNullOrEmpty(result.IpAddress))
                result.CountryIp = CacheApiHelper.GetCountryCodeFromCacheApi(result.IpAddress);

            if (string.IsNullOrEmpty(result.Country) && !string.IsNullOrEmpty(result.CountryIp))
                result.Country = result.CountryIp;

            if (MyConfiguration.Default.EnableDebug)
                CommonLogger.DefaultLogger.DebugFormat("TryDeserializeObject result: {0}",
                    Common.TrySerializeObject(result));

            if (string.IsNullOrEmpty(result.Lang))
                result.Lang = ResxLanguages.GetCurrentLanguage;

            ResxLanguages.SetCurrentLanguages(result.Lang);

            if (string.IsNullOrEmpty(result.Imei))
                result.Imei = result.HardwareId;

            return result;
        }
    }
}