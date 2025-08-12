using System;
using Newtonsoft.Json;

namespace BussinessObject.Models.Permission
{
    public class PermissionDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("sortIndex")]
        public int SortIndex { get; set; }
        [JsonProperty("routeName")]
        public string RouteName { get; set; }
        [JsonProperty("groupId")]
        public int GroupId { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; } // 1=Web, 2=Mobile
    }
} 