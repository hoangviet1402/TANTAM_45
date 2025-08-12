using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BussinessObject.Models.Menu
{
    public class MenuDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("parentId")]
        public int? ParentId { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("apiRouteName")]
        public string ApiRouteName { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("order")]
        public int Order { get; set; }
        [JsonProperty("menuType")]
        public int MenuType { get; set; }
        [JsonProperty("subMenu")]
        public List<MenuDto> SubMenu { get; set; }
        [JsonProperty("children")]
        public List<MenuDto> Children { get; set; }
    }
} 