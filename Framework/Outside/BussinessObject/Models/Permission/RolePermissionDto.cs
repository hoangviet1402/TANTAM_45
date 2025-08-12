using System;
using Newtonsoft.Json;

namespace BussinessObject.Models.Permission
{
    public class RolePermissionDto
    {
        [JsonProperty("role_id")]
        public int RoleId { get; set; }

        [JsonProperty("permission_id")]
        public int PermissionId { get; set; }

        [JsonProperty("permission_key")]
        public string PermissionKey { get; set; }

        [JsonProperty("permission_name")]
        public string PermissionName { get; set; }
    }
} 