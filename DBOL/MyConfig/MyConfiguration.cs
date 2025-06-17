using MyConfig.ResxLanguagesUtility;
using System.Configuration;

namespace MyConfig
{
    public class MyConfiguration
    {
        private static MyConfigSection _instance;

        private static MyConfigSection Instance =>
            _instance ?? (_instance = (MyConfigSection)ConfigurationManager.GetSection("MyConfig"));

        public static DefaultElement Default => Instance.DefaultElement;

        //public static CacheApiElement CacheApi => Instance.CacheApiElement;

        public static DefaultResxLanguagesUtilityElement DefaultLanguagesUtility =>
            Instance.DefaultResxLanguagesUtilityElement;

        public static CaptchaElement Captcha => Instance.CaptchaElement;

    }
}