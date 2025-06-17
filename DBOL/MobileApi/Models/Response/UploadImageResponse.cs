using Newtonsoft.Json;

namespace TanTamApi.Models.Response
{
    public class UploadImageResponse
    {
        [JsonProperty(PropertyName = "imp", NullValueHandling = NullValueHandling.Ignore)]
        public string ImagePath { get; set; }

        [JsonProperty(PropertyName = "cs", NullValueHandling = NullValueHandling.Ignore)]
        public string CheckSum { get; set; }
    }
}