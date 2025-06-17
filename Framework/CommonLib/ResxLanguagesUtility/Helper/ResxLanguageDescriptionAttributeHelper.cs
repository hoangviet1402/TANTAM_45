using System;
using System.ComponentModel;
using Logger;
using ResxLanguagesUtility.Enums;

namespace ResxLanguagesUtility.Helper
{
    /// <summary>
    ///     Author: QuocTuan
    ///     CreateDate: 2019-04-19
    ///     Description: Attribute đa ngôn ngữ description cho enum
    /// </summary>
    public class ResxLanguageDescriptionAttributeHelper : DescriptionAttribute
    {
        private readonly string resourceKeyName;
        private readonly ResxLanguagesEnum resxLanguagesEnum;

        public ResxLanguageDescriptionAttributeHelper(string _resourceKeyName, ResxLanguagesEnum _resxLanguagesEnum)
        {
            resourceKeyName = _resourceKeyName;
            resxLanguagesEnum = _resxLanguagesEnum;
        }

        public override string Description
        {
            get
            {
                try
                {
                    return ResxLanguages.GetText(resourceKeyName, resxLanguagesEnum);
                }
                catch (Exception ex)
                {
                    CommonLogger.DefaultLogger.Error("BoHelper -- ResxLanguageDescriptionAttributeHelper -- ex", ex);
                    return resourceKeyName;
                }
            }
        }
    }
}