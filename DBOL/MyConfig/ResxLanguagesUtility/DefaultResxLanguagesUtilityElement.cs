/**********************************************************************
 * Author: QuocTuan
 * DateCreate: 17-04-2019
 * Description: Quan ly thong tin cau hinh chung cho project
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using System.Configuration;

namespace MyConfig.ResxLanguagesUtility
{
    public class DefaultResxLanguagesUtilityElement : ConfigurationElement
    {
        [ConfigurationProperty("DefaultDirectory", DefaultValue = "ResxLanguages")]
        public string DefaultDirectory => (string)this["DefaultDirectory"];

        [ConfigurationProperty("IsShowKeyNotFound", DefaultValue = true)]
        public bool IsShowKeyNotFound => (bool)this["IsShowKeyNotFound"];
    }
}