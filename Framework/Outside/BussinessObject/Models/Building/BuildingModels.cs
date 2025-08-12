using Newtonsoft.Json;
using System.Collections.Generic;

namespace BussinessObject.Models.RequestFor
{
    public class ListDeviceEspRequest
    {
        [JsonProperty("mac", NullValueHandling = NullValueHandling.Ignore)]
        public string MAC { get; set; }
    }

    public class ListDeviceEspResponse
    {
        [JsonProperty("gpio", NullValueHandling = NullValueHandling.Ignore)]
        public int GPIO { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int DeviceStatus { get; set; }
    }
}