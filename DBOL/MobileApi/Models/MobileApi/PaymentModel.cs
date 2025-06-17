using TanTamApi.Models.Request;
using Newtonsoft.Json;

namespace TanTamApi.Models.MobileApi
{
    public class UserPagingModel : ApiBaseRequest
    {
        public UserPagingModel()
        {
            PageNumber = 1;
            PageSize = 40;
        }

        [JsonProperty("pgn")] public int PageNumber { get; set; }

        [JsonProperty("pgs")] public int PageSize { get; set; }
    }

    public class CofferParamModel : ApiBaseRequest
    {
        public CofferParamModel()
        {
            ActionType = 0;
        }

        [JsonProperty("gdt")] public decimal GoldTransfer { get; set; }

        [JsonProperty("act")] public int ActionType { get; set; }
    }
}