using Newtonsoft.Json;

namespace BussinessObject.Models.Auth
{
    public class RefeshTokenResponse
    {
        [JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; }
    }
}