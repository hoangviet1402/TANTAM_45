using BussinessObject.Enum;
using MyUtility.Extensions;
using Newtonsoft.Json;

namespace BussinessObject.Models.ApiResponse
{
    public class ApiBaseResponse
    {
        public ApiBaseResponse()
        {
            Code = ApiStatusCode.Failed.Value();
        }

        public ApiStatusCode Result { get; set; }

        [JsonProperty("c", NullValueHandling = NullValueHandling.Ignore)]
        public int Code { get; set; }

        [JsonProperty("m", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("d", NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }
    }
}