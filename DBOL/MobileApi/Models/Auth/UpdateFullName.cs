using Newtonsoft.Json;

namespace TanTamApi.Models.Auth
{
    public class UpdateFullNameSigupResponse
    {
        [JsonProperty("refresh_token", NullValueHandling = NullValueHandling.Ignore)]
        public string RefreshToken { get; set; }

        [JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; }

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? UserId { get; set; }

        [JsonProperty("shop_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? ShopId { get; set; }
    }

    public class UpdateFullNameResquest
    {
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string FullName { get; set; }
    }
}

