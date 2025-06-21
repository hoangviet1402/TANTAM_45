
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BussinessObject.Models.Shift
{
    public class ShiftAssignmentData
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("branch_ids")]
        public List<int> BranchIds { get; set; }

        [JsonProperty("position_ids")]
        public List<int> PositionIds { get; set; }

        [JsonProperty("user_ids")]
        public List<int> UserIds { get; set; }

        [JsonProperty("department_ids")]
        public List<int> DepartmentIds { get; set; }

        [JsonProperty("sort_index")]
        public int SortIndex { get; set; }

        [JsonProperty("auto_approve")]
        public int? AutoApprove { get; set; }

        [JsonProperty("payroll_config_type")]
        public string PayrollConfigType { get; set; } = string.Empty;

        [JsonProperty("payroll_config_type")]
        public string AssignmentType { get; set; } = "weekly_loop";

        [JsonProperty("generate_timekeeping_type")]
        public string GenerateTimekeepingType { get; set; } = "generate_from_start_of_month";

        [JsonProperty("type")]
        public string Type { get; set; } = "shift_assignment";

        [JsonProperty("assignments")]
        public List<int> Assignments { get; set; }
        
    }


}
