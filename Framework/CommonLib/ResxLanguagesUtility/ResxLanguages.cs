using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using Logger;
using MyConfig;
using ResxLanguagesUtility.Enums;
using WebUtility;

namespace ResxLanguagesUtility
{
    public class ResxLanguages
    {
        /// <summary>
        ///     Author: QuocTuan
        ///     CreateDate: 03/04/2019
        ///     Description: Lấy ngôn ngữ hiện tại của user => Thứ tự ưu tiên Cookie, culture, myconfig
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentLanguage => CurrentLanguage();

        /// <summary>
        ///     Author: QuocTuan
        ///     CreateDate: 03/04/2019
        ///     Description: Lấy giá trị trong file xml với language keyName
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="localLanguagesEnum"></param>
        /// <returns></returns>
        public static string GetText(string keyName, ResxLanguagesEnum resxLanguagesEnum = ResxLanguagesEnum.Account)
        {
            var text = string.Empty;

            if (MyConfiguration.DefaultLanguagesUtility.IsShowKeyNotFound)
                text = keyName;

            try
            {
                XElement xelement = null;

                var fullFilepath = GetFilePath(resxLanguagesEnum);
                using (var fileStream =
                       new FileStream(fullFilepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    xelement = XElement.Load(fileStream);
                }

                // Lấy element với key
                var elementData = (from xData in xelement.Elements("data")
                    where (string)xData.Attribute("name") == keyName
                    select xData).FirstOrDefault();

                if (elementData == null)
                {
                    CommonLogger.DefaultLogger.ErrorFormat(
                        "[LanguagesUtility.Languages.GetText] - Chua dinh nghia keyName: {0} vao file {1}", keyName,
                        fullFilepath);
                    return text;
                }

                // Lấy giá trị của element "value" trong element "data"
                try
                {
                    text = elementData.Element("value").Value;
                }
                catch (Exception ex)
                {
                    CommonLogger.DefaultLogger.ErrorFormat(
                        "[LanguagesUtility.Languages.GetText] - Chua dinh nghia gia tri cho keyName: {0} trong file {1}",
                        keyName, fullFilepath);
                    return text;
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("[LanguagesUtility.Languages.GetText] - {0} {1}", keyName, ex);
            }

            return text;
        }

        /// <summary>
        ///     Author: QuocTuan
        ///     CreateDate: 17/05/2019
        ///     Description: Lấy giá trị trong file xml với language tùy chọn keyName
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="resxLanguagesEnum"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string GetText(string keyName, ResxLanguagesEnum resxLanguagesEnum, string language)
        {
            var text = string.Empty;

            if (MyConfiguration.DefaultLanguagesUtility.IsShowKeyNotFound)
                text = keyName;

            try
            {
                XElement xelement = null;

                var fullFilepath = GetFilePath(resxLanguagesEnum, language);
                using (var fileStream =
                       new FileStream(fullFilepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    xelement = XElement.Load(fileStream);
                }

                // Lấy element với key
                var elementData = (from xData in xelement.Elements("data")
                    where (string)xData.Attribute("name") == keyName
                    select xData).FirstOrDefault();

                if (elementData == null)
                {
                    CommonLogger.DefaultLogger.ErrorFormat(
                        "[LanguagesUtility.Languages.GetText] - Chua dinh nghia keyName: {0} vao file {1}", keyName,
                        fullFilepath);
                    return text;
                }

                // Lấy giá trị của element "value" trong element "data"
                try
                {
                    text = elementData.Element("value").Value;
                }
                catch (Exception ex)
                {
                    CommonLogger.DefaultLogger.ErrorFormat(
                        "[LanguagesUtility.Languages.GetText] - Chua dinh nghia gia tri cho keyName: {0} trong file {1}",
                        keyName, fullFilepath);
                    return text;
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("[LanguagesUtility.Languages.GetText] - {0} {1}", keyName, ex);
            }

            return text;
        }

        /// <summary>
        ///     Author: QuocTuan
        ///     CreateDate: 20/11/2019
        ///     Description: Lấy giá trị trong file xml với format
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="resxLanguagesEnum"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string GetTextFormat(string keyName, ResxLanguagesEnum resxLanguagesEnum,
            params object[] argsParam)
        {
            var text = string.Empty;

            if (MyConfiguration.DefaultLanguagesUtility.IsShowKeyNotFound)
                text = keyName;

            try
            {
                XElement xelement = null;

                var fullFilepath = GetFilePath(resxLanguagesEnum);
                using (var fileStream =
                       new FileStream(fullFilepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    xelement = XElement.Load(fileStream);
                }

                // Lấy element với key
                var elementData = (from xData in xelement.Elements("data")
                    where (string)xData.Attribute("name") == keyName
                    select xData).FirstOrDefault();

                if (elementData == null)
                {
                    CommonLogger.DefaultLogger.ErrorFormat(
                        "[LanguagesUtility.Languages.GetTextFormat] - Chua dinh nghia keyName: {0} vao file {1}",
                        keyName, fullFilepath);
                    return text;
                }

                // Lấy giá trị của element "value" trong element "data"
                try
                {
                    text = elementData.Element("value").Value;
                }
                catch (Exception ex)
                {
                    CommonLogger.DefaultLogger.ErrorFormat(
                        "[LanguagesUtility.Languages.GetTextFormat] - Chua dinh nghia gia tri cho keyName: {0} trong file {1}",
                        keyName, fullFilepath);
                    return text;
                }

                try
                {
                    var totalMatchCount = 0;
                    if (!string.IsNullOrEmpty(text))
                    {
                        var pattern = @"{(.*?)}";
                        var matches = Regex.Matches(text, pattern);
                        //totalMatchCount = matches.Count;
                        var uniqueMatchCount = matches.OfType<Match>().Select(m => m.Value).Distinct().Count();
                        totalMatchCount = uniqueMatchCount;
                    }

                    // Kiểm tra index format với param khớp
                    if (totalMatchCount > 0 && argsParam.Length > 0 && argsParam.Length == totalMatchCount)
                    {
                        text = string.Format(text, argsParam);
                    }
                    else if (totalMatchCount >= 0 && argsParam.Length >= 0 && argsParam.Length != totalMatchCount)
                    {
                        text = keyName;
                        CommonLogger.DefaultLogger.ErrorFormat(
                            "[LanguagesUtility.Languages.GetTextFormat (-1)] - StringFormat khong khop cho keyName: {0} NumberIndex: {1} NumberParam: {2} trong file {3}",
                            keyName, totalMatchCount, argsParam.Length, fullFilepath);
                    }
                }
                catch (Exception ex)
                {
                    text = keyName;
                    CommonLogger.DefaultLogger.ErrorFormat("[LanguagesUtility.Languages.GetTextFormat (-1)] - {0}", ex);
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("[LanguagesUtility.Languages.GetTextFormat (-2)] - {0}", ex);
            }

            return text;
        }

        /// <summary>
        ///     Author: QuocTuan
        ///     CreateDate: 03/04/2019
        ///     Description: Lấy keyName theo 1 giá trị văn bản
        /// </summary>
        /// <returns></returns>
        public static string GetKey(string textFind, ResxLanguagesEnum resxLanguagesEnum)
        {
            var text = string.Empty;

            if (MyConfiguration.DefaultLanguagesUtility.IsShowKeyNotFound)
                text = textFind;
            try
            {
                XElement xelement = null;

                var fullFilepath = GetFilePath(resxLanguagesEnum);
                using (var fileStream =
                       new FileStream(fullFilepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    xelement = XElement.Load(fileStream);
                }

                // Lấy element với key
                var elementData = (from xData in xelement.Elements("data")
                    where xData.Element("value").Value == textFind
                    select xData).FirstOrDefault();

                if (elementData == null)
                {
                    CommonLogger.DefaultLogger.ErrorFormat(
                        "[LanguagesUtility.Languages.GetKey] - Khong tim thay keyName voi text : {0} trong file {1}",
                        textFind, fullFilepath);
                    return text;
                }

                // Lấy giá trị của element "value" trong element "data"
                text = elementData.Attribute("name").Value;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("[LanguagesUtility.Languages.GetKey] - {0}", ex);
            }

            return text;
        }

        public static void SetCurrentLanguages(string language)
        {
            try
            {
                if (string.IsNullOrEmpty(language) || !DoesCultureExist(language))
                    language = MyConfiguration.Default.DefaultLanguage;

                // Set Language cookie
                MyItemManager.Set(MyConfiguration.Default.DefaultLanguageCookieName, language);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("[LanguagesUtility.Languages.SetCurrentLanguages] - {0}", ex);
            }
        }

        /// <summary>
        ///     Author: QuocTuan
        ///     CreateDate: 03/04/2019
        ///     Description: Hỗ trợ lấy ngôn ngữ hiện tại của user => Thứ tự ưu tiên Cookie, culture, myconfig
        /// </summary>
        /// <returns></returns>
        private static string CurrentLanguage()
        {
            var lang = string.Empty;

            try
            {
                // B1: Ưu tiên lấy ngôn ngữ từ cookie trước
                lang = MyItemManager.Get(MyConfiguration.Default.DefaultLanguageCookieName);

                // B3: Culture không có thì lấy theo MyConfig
                if (string.IsNullOrEmpty(lang) || lang.Equals("undefined", StringComparison.OrdinalIgnoreCase))
                    lang = MyConfiguration.Default.DefaultLanguage;
            }
            catch (Exception ex)
            {
                lang = MyConfiguration.Default.DefaultLanguage;
                CommonLogger.DefaultLogger.ErrorFormat("[LanguagesUtility.Languages.CurrentLanguage] - {0}", ex);
            }

            return lang;
        }

        private static string GetFileName(ResxLanguagesEnum _enum, string language)
        {
            var fileName = string.Empty;
            switch (_enum)
            {
                case ResxLanguagesEnum.Account:
                    fileName = "Account\\Account." + language + ".xml";
                    break;
                case ResxLanguagesEnum.Cashout:
                    fileName = "Cashout\\Cashout." + language + ".xml";
                    break;
                case ResxLanguagesEnum.Common:
                    fileName = "Common\\Common." + language + ".xml";
                    break;
                case ResxLanguagesEnum.Event:
                    fileName = "Event\\Event." + language + ".xml";
                    break;
                case ResxLanguagesEnum.Game:
                    fileName = "Game\\Game." + language + ".xml";
                    break;
                case ResxLanguagesEnum.Guild:
                    fileName = "Guild\\Guild." + language + ".xml";
                    break;
                case ResxLanguagesEnum.Home:
                    fileName = "Home\\Home." + language + ".xml";
                    break;
                case ResxLanguagesEnum.Payment:
                    fileName = "Payment\\Payment." + language + ".xml";
                    break;
                case ResxLanguagesEnum.Reason:
                    fileName = "Reason\\Reason." + language + ".xml";
                    break;
                case ResxLanguagesEnum.Other:
                    fileName = "Other\\Other." + language + ".xml";
                    break;
                case ResxLanguagesEnum.BO:
                    fileName = "BO\\BO." + language + ".xml";
                    break;
                case ResxLanguagesEnum.Forum:
                    fileName = "Forum\\Forum." + language + ".xml";
                    break;
                case ResxLanguagesEnum.Me:
                    fileName = "Me\\Me." + language + ".xml";
                    break;
                default:
                    fileName = "Account\\Account." + language + ".xml";
                    break;
            }

            return fileName;
        }

        /// <summary>
        ///     Author: QuocTuan
        ///     CreateDate: 23/05/2019
        ///     Description: Lấy full đường dẫn chứa ngôn ngữ
        /// </summary>
        /// <returns></returns>
        private static string GetFilePath(ResxLanguagesEnum resxLanguagesEnum)
        {
            try
            {
                // Lấy ngôn ngữ hiện tại
                var language = GetCurrentLanguage;
                if (string.IsNullOrEmpty(language) || !DoesCultureExist(language))
                    language = MyConfiguration.Default.DefaultLanguage;

                var fileName = GetFileName(resxLanguagesEnum, language);
                var dicrectory =
                    HttpContext.Current.Server.MapPath("~/" + MyConfiguration.DefaultLanguagesUtility.DefaultDirectory);

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["RESX_LANGUAGE_PATH"]))
                    dicrectory = ConfigurationManager.AppSettings["RESX_LANGUAGE_PATH"];

                return Path.Combine(dicrectory, fileName);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("[GetRootPath] ex " + ex);
                return string.Empty;
            }
        }

        /// <summary>
        ///     Author: QuocTuan
        ///     CreateDate: 23/05/2019
        ///     Description: Lấy full đường dẫn chứa ngôn ngữ tùy chọn
        /// </summary>
        /// <returns></returns>
        private static string GetFilePath(ResxLanguagesEnum resxLanguagesEnum, string language)
        {
            try
            {
                if (string.IsNullOrEmpty(language) || !DoesCultureExist(language))
                    language = MyConfiguration.Default.DefaultLanguage;

                var fileName = GetFileName(resxLanguagesEnum, language);
                var dicrectory =
                    HttpContext.Current.Server.MapPath("~/" + MyConfiguration.DefaultLanguagesUtility.DefaultDirectory);

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["RESX_LANGUAGE_PATH"]))
                    dicrectory = ConfigurationManager.AppSettings["RESX_LANGUAGE_PATH"];

                return Path.Combine(dicrectory, fileName);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("[GetRootPath] ex " + ex);
                return string.Empty;
            }
        }

        private static bool DoesCultureExist(string cultureName)
        {
            //return System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures).Any(culture => string.Equals(culture.Name, cultureName, StringComparison.CurrentCultureIgnoreCase));

            if (CultureInfo.GetCultures(CultureTypes.AllCultures).Any(culture =>
                    string.Equals(culture.Name, cultureName, StringComparison.CurrentCultureIgnoreCase)))
                return true;

            var langs = new List<string> { "en", "km", "ms", "my", "th", "vi", "zh", "ph" };

            return langs.Exists(dt => dt.Equals(cultureName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}