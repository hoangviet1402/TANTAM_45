using Newtonsoft.Json;

namespace TanTamApi.Models.Request
{
    public class CaptchaRequestModel : ApiBaseRequest
    {
        [JsonProperty("prf")] public string prefix { get; set; }

        [JsonProperty("w")] public int? Width { get; set; }

        [JsonProperty("h")] public int? Height { get; set; }
    }
}