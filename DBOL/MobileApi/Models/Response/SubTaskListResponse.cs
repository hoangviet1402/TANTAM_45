using Newtonsoft.Json;
using System.Collections.Generic;

namespace TanTamApi.Models.Response
{
    public class SubTaskListResponse
    {
        [JsonProperty("items")]
        public List<SubTaskResponse> Items { get; set; }

        [JsonProperty("meta")]
        public TaskListMetaResponse Meta { get; set; }
    }
} 