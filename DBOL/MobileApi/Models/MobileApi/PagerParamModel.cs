using TanTamApi.Models.Request;
using Newtonsoft.Json;

namespace TanTamApi.Models.MobileApi
{
    public class PagerParamModel : ApiBaseRequest
    {
        [JsonProperty("pn")] public int PageNumber { get; set; }

        [JsonProperty("pz")] public int PageSize { get; set; }
    }

    public class GamePagerParamModel : ApiBaseRequest
    {
        [JsonProperty("gi")] public int GameID { get; set; }

        [JsonProperty("pgn")] public int PageNumber { get; set; }

        [JsonProperty("pgs")] public int PageSize { get; set; }
    }
}