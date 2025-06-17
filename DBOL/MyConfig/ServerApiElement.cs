using System.Configuration;

namespace MyConfig
{
    public class ServerApiElement : ConfigurationElement
    {
        [ConfigurationProperty("IsEnable", DefaultValue = true)]
        public bool IsEnable
        {
            get { return (bool)this["IsEnable"]; }
        }

        [ConfigurationProperty("ListServerApiDomain", DefaultValue = "http://192.168.1.57:2025/api|http://192.168.1.57:2026/api")]
        public string ListServerApiDomain
        {
            get { return (string)this["ListServerApiDomain"]; }
        }

        [ConfigurationProperty("ApiMobiUrl", DefaultValue = "https://nvap.epexilo.com/nv/sl?")]
        public string ApiMobiUrl
        {
            get { return (string)this["ApiMobiUrl"]; }
        }

        [ConfigurationProperty("ApiFallbackUrl", DefaultValue = "https://raw.githubusercontent.com/NicholasPages/pickalbal/refs/heads/main/testj.json")]
        public string ApiFallbackUrl
        {
            get { return (string)this["ApiFallbackUrl"]; }
        }

        [ConfigurationProperty("ServerApkApiDomain", DefaultValue = "http://192.168.1.57:2026/api")]
        public string ServerApkApiDomain
        {
            get { return (string)this["ServerApkApiDomain"]; }
        }
    }
}
