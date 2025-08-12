using System.Collections.Generic;
using Newtonsoft.Json;

namespace BussinessObject.Models.Permission
{
    public class UpdateEmployeePermissionsRequest
    {
        [JsonProperty("type")]
        public string Type { get; set; } // "web" hoặc "mobile"
        [JsonProperty("employee_id")]
        public int EmployeeId { get; set; }
        [JsonProperty("permission_ids")]
        public List<int> PermissionIds { get; set; }
    }
} 