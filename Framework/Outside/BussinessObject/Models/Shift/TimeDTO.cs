

using System.Collections.Generic;
using Newtonsoft.Json;

namespace BussinessObject.Models.Shift
{
    public class GetTimesRequest
    {
        [JsonProperty("lang")]
        public string Lang { get; set; }

      
    }
    public class TimesResponse
    {
        [JsonProperty("hours")]
        public List<HourResponse> Hours { get; set; }

        [JsonProperty("minutes")]
        public List<MinuteResponse> Minutes { get; set; }
    }

    public class HourResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class MinuteResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

}
