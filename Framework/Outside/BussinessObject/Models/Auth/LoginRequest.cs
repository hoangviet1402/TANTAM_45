using Newtonsoft.Json;

namespace BussinessObject.Models.Auth
{
    public class AppInfo
    {
        [JsonProperty("package_name", NullValueHandling = NullValueHandling.Ignore)]
        public string PackageName { get; set; }
    }

    public class SignupRequest
    {
        [JsonProperty("phone_code", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneCode { get; set; }

        [JsonProperty("mail", NullValueHandling = NullValueHandling.Ignore)]
        public string Mail { get; set; }

        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [JsonProperty("fullname", NullValueHandling = NullValueHandling.Ignore)]
        public string Fullname { get; set; }

        [JsonProperty("stage", NullValueHandling = NullValueHandling.Ignore)]
        public string Stage { get; set; }

        [JsonProperty("provider", NullValueHandling = NullValueHandling.Ignore)]
        public string Provider { get; set; }

        [JsonProperty("device_id", NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceId { get; set; }

        [JsonProperty("appInfo", NullValueHandling = NullValueHandling.Ignore)]
        public AppInfo AppInfo { get; set; }

        [JsonProperty("ignoreToken", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IgnoreToken { get; set; }

        [JsonProperty("is_mobile_menu", NullValueHandling = NullValueHandling.Ignore)]
        public int? IsMobileMenu { get; set; }

        [JsonProperty("is_no_otp_flow", NullValueHandling = NullValueHandling.Ignore)]
        public int? Is_no_otp_flow { get; set; }

    }



    public class SigninRequest
    {
        [JsonProperty("mail", NullValueHandling = NullValueHandling.Ignore)]
        public string Mail { get; set; }

        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [JsonProperty("provider", NullValueHandling = NullValueHandling.Ignore)]
        public string Provider { get; set; }

        [JsonProperty("shop_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? ShopId { get; set; }

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? UserId { get; set; }

        [JsonProperty("stage", NullValueHandling = NullValueHandling.Ignore)]
        public string Stage { get; set; }

        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }

        [JsonProperty("device_id", NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceId { get; set; }

        [JsonProperty("appInfo", NullValueHandling = NullValueHandling.Ignore)]
        public AppInfo AppInfo { get; set; }

        [JsonProperty("phone_code", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneCode { get; set; }

        [JsonProperty("for_test_brute_force", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ForTestBruteForce { get; set; }

        [JsonProperty("is_mobile_menu", NullValueHandling = NullValueHandling.Ignore)]
        public int? IsMobileMenu { get; set; }
    }
}