using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
namespace TanTamApi.Models.Auth
{
    public class CreatePassRequest
    {
        [Required]
        [JsonProperty("shop_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? ShopId { get; set; }

        [Required]
        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? UserId { get; set; }

        [Required]
        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }


        [JsonProperty("comfirmPass", NullValueHandling = NullValueHandling.Ignore)]
        public string ComfirmPass { get; set; }
    }
}


