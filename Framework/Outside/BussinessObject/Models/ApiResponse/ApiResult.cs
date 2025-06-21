using BussinessObject.Enum;
using MyUtility.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace BussinessObject.Models.ApiResponse
{
    public class ApiResult<T>
    {
        [JsonProperty("error_code", NullValueHandling = NullValueHandling.Ignore)]
        public int Code { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        public static ApiResult<T> Success(T data, string message = "")
        {
            return new ApiResult<T>
            {
                Code = ResponseResultEnum.Success.Value(),
                Data = data,
                Message = message
            };
        }

        public static ApiResult<T> Failure(int code, string message, List<string> errors = null)
        {
            return new ApiResult<T>
            {
                Code = code,
                Message = message
            };
        }
    }
}