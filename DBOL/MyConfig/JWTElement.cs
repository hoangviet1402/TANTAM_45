using System.Configuration;

namespace MyConfig
{
    public class JWTElement : ConfigurationElement
    {
        [ConfigurationProperty("IsEnable", DefaultValue = true)]
        public bool IsEnable
        {
            get { return (bool)this["IsEnable"]; }
        }

        [ConfigurationProperty("SecretKey", DefaultValue = "your-super-secret-key-at-least-32-characters-long-and-very-secure")]
        public string SecretKey
        {
            get { return (string)this["SecretKey"]; }
        }

        [ConfigurationProperty("Issuer", DefaultValue = "TanTamApi")]
        public string Issuer
        {
            get { return (string)this["Issuer"]; }
        }

        [ConfigurationProperty("Audience", DefaultValue = "TanTamClient")]
        public string Audience
        {
            get { return (string)this["Audience"]; }
        }

        [ConfigurationProperty("ExpiryInMinutes", DefaultValue = 30)]
        public int ExpiryInMinutes
        {
            get { return (int)this["ExpiryInMinutes"]; }
        }

        [ConfigurationProperty("LifeTime", DefaultValue = 30)]
        public int LifeTime
        {
            get { return (int)this["LifeTime"]; }
        }

      
    }
    
}
