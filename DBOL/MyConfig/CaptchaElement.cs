using System.Configuration;

namespace MyConfig
{
    public class CaptchaElement : ConfigurationElement
    {
        [ConfigurationProperty("Enable", DefaultValue = true)]
        public bool Enable => (bool)this["Enable"];

        /// <summary>
        ///     So tien rut toi thieu
        /// </summary>
        [ConfigurationProperty("MaxRandom", DefaultValue = "30")]
        public int MaxRandom => (int)this["MaxRandom"];

        /// <summary>
        ///     min random result
        /// </summary>
        [ConfigurationProperty("MinRandom", DefaultValue = "1")]
        public int MinRandom => (int)this["MinRandom"];

        /// <summary>
        ///     random operator: 0 cng, 1 tru, 2 nhan 3 chia
        /// </summary>
        [ConfigurationProperty("OperatorRandom", DefaultValue = "2")]
        public int OperatorRandom => (int)this["OperatorRandom"];

        /// <summary>
        ///     Bật tắt captcha đăng ký
        /// </summary>
        [ConfigurationProperty("EnableRegister", DefaultValue = true)]
        public bool EnableRegister => (bool)this["EnableRegister"];

        [ConfigurationProperty("IsEnableCaptchaLoginMobile", DefaultValue = false)]
        public bool IsEnableCaptchaLoginMobile => (bool)this["IsEnableCaptchaLoginMobile"];

        [ConfigurationProperty("CaptchaLoginMobileMaxError", DefaultValue = 0)]
        public int CaptchaLoginMobileMaxError => (int)this["CaptchaLoginMobileMaxError"];

        /// <summary>
        ///     Số ký tự random captcha
        /// </summary>
        [ConfigurationProperty("NumCharRandom", DefaultValue = 5)]
        public int NumCharRandom => (int)this["NumCharRandom"];

        /// <summary>
        ///     Danh sách các ký tự captcha
        /// </summary>
        [ConfigurationProperty("ArrCharacterCaptcha", DefaultValue = "")]
        public string ArrCharacterCaptcha => (string)this["ArrCharacterCaptcha"];

        /// <summary>
        ///     Thời gian tồn tại
        /// </summary>
        [ConfigurationProperty("TimeLifeCaptcha", DefaultValue = 30)]
        public int TimeLifeCaptcha => (int)this["TimeLifeCaptcha"];


        [ConfigurationProperty("WidthCaptcha", DefaultValue = 100)]
        public int WidthCaptcha => (int)this["WidthCaptcha"];

        [ConfigurationProperty("HeightCaptcha", DefaultValue = 50)]
        public int HeightCaptcha => (int)this["HeightCaptcha"];
    }
}