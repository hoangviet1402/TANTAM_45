using Newtonsoft.Json;

namespace TanTamApi.Models.Response
{
    public class UserSimpleResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("identification")]
        public int? Identification { get; set; }

        [JsonProperty("branch_id")]
        public int? BranchId { get; set; }
    }
} 