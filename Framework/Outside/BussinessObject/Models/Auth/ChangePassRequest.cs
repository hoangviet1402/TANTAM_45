using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BussinessObject.Models.Auth
{
    public class ChangePassRequest
    {
        [Required]
        [JsonProperty("shop_id", NullValueHandling = NullValueHandling.Ignore)]
        public int ShopId { get; set; }

        [Required]
        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public int UserId { get; set; }

        [Required]
        [JsonProperty("new_pass", NullValueHandling = NullValueHandling.Ignore)]
        public string NewPass { get; set; }


        [JsonProperty("old_pass", NullValueHandling = NullValueHandling.Ignore)]
        public string OldPass { get; set; }
    }
}