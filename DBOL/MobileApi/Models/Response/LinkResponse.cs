using Newtonsoft.Json;

namespace TanTamApi.Models.Response
{
    public class LinkResponse
    {
        [JsonProperty(PropertyName = "swv", NullValueHandling = NullValueHandling.Ignore)]
        public bool ShowWebView { get; set; }

        [JsonProperty(PropertyName = "lnk", NullValueHandling = NullValueHandling.Ignore)]
        public string Link { get; set; }
    }
}