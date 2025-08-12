using Newtonsoft.Json;

namespace BussinessObject.Models.Company
{
    public class DeletePositionRequest
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
    }
} 