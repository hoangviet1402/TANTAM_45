using MyConfig.ResxLanguagesUtility;
using System.Configuration;

namespace MyConfig
{
    public class MyConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Default")] public DefaultElement DefaultElement => (DefaultElement)this["Default"];

        //[ConfigurationProperty("CacheApi")] public CacheApiElement CacheApiElement => (CacheApiElement)this["CacheApi"];

        [ConfigurationProperty("DefaultResxLanguagesUtility")]
        public DefaultResxLanguagesUtilityElement DefaultResxLanguagesUtilityElement =>
            (DefaultResxLanguagesUtilityElement)this["DefaultResxLanguagesUtility"];

        [ConfigurationProperty("Captcha")] public CaptchaElement CaptchaElement => (CaptchaElement)this["Captcha"];
        [ConfigurationProperty("JWT")] public JWTElement JWTElement => (JWTElement)this["JWT"];
    }
}