using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BussinessObject.Models.Permission
{
    public class PermissionGroupDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("parentId")]
        public int? ParentId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("apiRouteName")]
        public string ApiRouteName { get; set; }
        [JsonProperty("isSystem")]
        public bool IsSystem { get; set; }
        [JsonProperty("sortIndex")]
        public int SortIndex { get; set; }
        [JsonProperty("children")]
        public List<PermissionGroupDto> Children { get; set; }
        [JsonProperty("permissions")]
        public List<PermissionDto> Permissions { get; set; }
    }
} 