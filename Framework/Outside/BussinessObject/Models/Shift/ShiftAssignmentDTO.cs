
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

        [JsonProperty("assignment_type")]
        public string AssignmentType { get; set; } = "weekly_loop";

        [JsonProperty("generate_timekeeping_type")]
        public string GenerateTimekeepingType { get; set; } = "generate_from_start_of_month";

        [JsonProperty("type")]
        public string Type { get; set; } = "shift_assignment";

        [JsonProperty("assignments")]
        public List<int> Assignments { get; set; }
    }

    public class EmployeesInfo_ForAddShiftRequest
    {
        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int BranchId { get; set; }

        [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
        public Filter Filter { get; set; }

        [JsonProperty("is_only_branch", NullValueHandling = NullValueHandling.Ignore)]
        public int IsOnlyBranch { get; set; }

        [JsonProperty("is_quit", NullValueHandling = NullValueHandling.Ignore)]
        public int IsQuit { get; set; }

        [JsonProperty("keyword", NullValueHandling = NullValueHandling.Ignore)]
        public string Keyword { get; set; }

        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int Page { get; set; }

        [JsonProperty("shift_id", NullValueHandling = NullValueHandling.Ignore)]
        public int ShiftId { get; set; }

        [JsonProperty("working_day", NullValueHandling = NullValueHandling.Ignore)]
        public string WorkingDay { get; set; }
    }

    public class Filter
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class EmployeesInfo_ForAddShiftResponse
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("shop_id", NullValueHandling = NullValueHandling.Ignore)]
        public int ShopId { get; set; }

        [JsonProperty("identification", NullValueHandling = NullValueHandling.Ignore)]
        public string Identification { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedAt { get; set; }

        [JsonProperty("is_tanca_phone", NullValueHandling = NullValueHandling.Ignore)]
        public int IsTancaPhone { get; set; }

        [JsonProperty("is_tanca_email", NullValueHandling = NullValueHandling.Ignore)]
        public int IsTancaEmail { get; set; }

        [JsonProperty("sort_index", NullValueHandling = NullValueHandling.Ignore)]
        public int SortIndex { get; set; }

        [JsonProperty("last_activity", NullValueHandling = NullValueHandling.Ignore)]
        public string LastActivity { get; set; }

        [JsonProperty("region_id", NullValueHandling = NullValueHandling.Ignore)]
        public int RegionId { get; set; }

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int BranchId { get; set; }

        [JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
        public int Position { get; set; }

        [JsonProperty("department", NullValueHandling = NullValueHandling.Ignore)]
        public int Department { get; set; }

        [JsonProperty("shop", NullValueHandling = NullValueHandling.Ignore)]
        public string Shop { get; set; }

        [JsonProperty("region", NullValueHandling = NullValueHandling.Ignore)]
        public string Region { get; set; }

        [JsonProperty("branch", NullValueHandling = NullValueHandling.Ignore)]
        public string Branch { get; set; }

        [JsonProperty("payroll_config", NullValueHandling = NullValueHandling.Ignore)]
        public string PayrollConfig { get; set; }

        [JsonProperty("group", NullValueHandling = NullValueHandling.Ignore)]
        public string Group { get; set; }

        [JsonProperty("position_obj", NullValueHandling = NullValueHandling.Ignore)]
        public PositionObj PositionObj { get; set; }

        [JsonProperty("department_obj", NullValueHandling = NullValueHandling.Ignore)]
        public DepartmentObj DepartmentObj { get; set; }

        [JsonProperty("branch_obj", NullValueHandling = NullValueHandling.Ignore)]
        public BranchObj BranchObj { get; set; }

        [JsonProperty("group_obj", NullValueHandling = NullValueHandling.Ignore)]
        public GroupObj GroupObj { get; set; }

        [JsonProperty("region_obj", NullValueHandling = NullValueHandling.Ignore)]
        public RegionObj RegionObj { get; set; }
    }

    public class PositionObj
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class DepartmentObj
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class BranchObj
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class GroupObj
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("client_role", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientRole { get; set; }
    }

    public class RegionObj
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }   

    public class EmployeeRegisterShiftRequest
    {
        [JsonProperty("shift_week_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ShiftWeekId { get; set; }

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public string BranchId { get; set; }

        [JsonProperty("shift_id", NullValueHandling = NullValueHandling.Ignore)]
        public int shiftAssignmentId { get; set; }

        [JsonProperty("shift", NullValueHandling = NullValueHandling.Ignore)]
        public string Shift { get; set; }

        [JsonProperty("user_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> UserIds { get; set; }

        [JsonProperty("week", NullValueHandling = NullValueHandling.Ignore)]
        public string Week { get; set; }

        [JsonProperty("working_day", NullValueHandling = NullValueHandling.Ignore)]
        public string WorkingDay { get; set; }
    }
     
    public class EmployeeRejectShiftRequest
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int id { get; set; }

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public int UserId { get; set; }

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int BranchId { get; set; }
    }
}
