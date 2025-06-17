using System;

namespace TanTamApi.Models.Captcha
{
    [Serializable]
    public class CaptchaModel
    {
        public string Text { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}