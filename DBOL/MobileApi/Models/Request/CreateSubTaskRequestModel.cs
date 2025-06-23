using Newtonsoft.Json;

namespace TanTamApi.Models.Request
{
    public class CreateSubTaskRequestModel
    {
        [JsonProperty("bundle_id")]
        public int BundleId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("created_user_id")]
        public int? CreatedUserId { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }
    }
} 