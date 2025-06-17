/**********************************************************************
 * Author: LongNP
 * DateCreate: 2017-06-13
 *********************************************************************/

using System.Configuration;

namespace MyConfig
{
    public class CacheApiElement : ConfigurationElement
    {
        [ConfigurationProperty("CacheKey", DefaultValue = "CACHE_API_KEY!@#456")]
        public string CacheKey => (string)this["CacheKey"];

        [ConfigurationProperty("Enable", DefaultValue = true)]
        public bool Enable => (bool)this["Enable"];

        [ConfigurationProperty("IsUserVip", DefaultValue = false)]
        public bool IsUserVip => (bool)this["IsUserVip"];

        [ConfigurationProperty("IsUseCacheListGame", DefaultValue = true)]
        public bool IsUseCacheListGame => (bool)this["IsUseCacheListGame"];

        /// <summary>
        ///     list game lobby
        /// </summary>
        [ConfigurationProperty("IsUseCacheListGame_CacheApi", DefaultValue = false)]
        public bool IsUseCacheListGame_CacheApi => (bool)this["IsUseCacheListGame_CacheApi"];

        /// <summary>
        ///     config mobi api
        /// </summary>
        [ConfigurationProperty("IsUseMyConfigChannel_CacheApi", DefaultValue = false)]
        public bool IsUseMyConfigChannel_CacheApi => (bool)this["IsUseMyConfigChannel_CacheApi"];
    }
}