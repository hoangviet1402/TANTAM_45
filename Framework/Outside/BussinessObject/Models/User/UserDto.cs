using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BussinessObject.Models.User
{
    public class UserDto
    {
        [JsonProperty("id")]
        public int UserId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("role")]
        public int? Role { get; set; }

        [JsonProperty("shop_id")]
        public int CompanyId { get; set; }

        [JsonProperty("shop_name")]
        public string CompanyFullName { get; set; }

        [JsonProperty("employee_code")]
        public string EmployeeCode { get; set; }

        [JsonProperty("birth_date")]
        public DateTime? BirthDate { get; set; }

        [JsonProperty("gender")]
        public int? Gender { get; set; }
    }

    public class UserListRequest
    {
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int? Page { get; set; }

        [JsonProperty("limit", NullValueHandling = NullValueHandling.Ignore)]
        public int? Limit { get; set; }

        [JsonProperty("search", NullValueHandling = NullValueHandling.Ignore)]
        public string Search { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int? Status { get; set; }

        [JsonProperty("department_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? DepartmentId { get; set; }

        [JsonProperty("role_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? RoleId { get; set; }

        public UserListRequest()
        {
            Page = 1;
            Limit = 10;
            Search = string.Empty;
        }
    }

    public class UserListResponse
    {
        [JsonProperty("meta", NullValueHandling = NullValueHandling.Ignore)]
        public MetaResponse Meta { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<UserDto> Items { get; set; }

        public UserListResponse()
        {
            Meta = new MetaResponse();
            Items = new List<UserDto>();
        }
    }

    public class MetaResponse
    {
        public int total { get; set; }
        public int count { get; set; }
        public int perPage { get; set; }
        public int currentPage { get; set; }
        public int totalPages { get; set; }
    }

    public class UserDetailRequest
    {
        [Required]
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [Required]
        [JsonProperty("company_id")]
        public int CompanyId { get; set; }
    }

    public class UserDetailResponse
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("phone_code")]
        public string PhoneCode { get; set; }

        [JsonProperty("phone_full")]
        public string PhoneFull { get; set; }

        [JsonProperty("account_is_active")]
        public bool AccountIsActive { get; set; }

        [JsonProperty("account_created_at")]
        public DateTime AccountCreatedAt { get; set; }

        [JsonProperty("employee_account_map_id")]
        public int EmployeeAccountMapId { get; set; }

        [JsonProperty("shop_id")]
        public int CompanyId { get; set; }

        [JsonProperty("shop_name")]
        public string CompanyFullName { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("is_active")]
        public bool UserIsActive { get; set; }

        [JsonProperty("is_new_user")]
        public bool IsNewUser { get; set; }

        [JsonProperty("need_set_password")]
        public bool NeedSetPassword { get; set; }

        [JsonProperty("user_created_at")]
        public DateTime? UserCreatedAt { get; set; }

        [JsonProperty("role")]
        public int? Role { get; set; }
        [JsonProperty("role_name")]
        public string RoleName { get; set; }

        [JsonProperty("employee_info_id")]
        public int? EmployeeInfoId { get; set; }

        [JsonProperty("employee_code")]
        public string EmployeeCode { get; set; }

        [JsonProperty("birth_date")]
        public DateTime? BirthDate { get; set; }

        [JsonProperty("gender")]
        public int? Gender { get; set; }

        [JsonProperty("contact_address")]
        public string ContactAddress { get; set; }

        [JsonProperty("is_root")]
        public int? IsRoot { get; set; }
        [JsonProperty("view_allowance_info")]
        public bool ViewAllowanceInfo { get; set; }
        [JsonProperty("view_promotion_history")]
        public bool ViewPromotionHistory { get; set; }

        [JsonProperty("can_update_permission")]
        public bool CanUpdatePermission { get; set; }

        [JsonProperty("can_update_timetracking_config")]
        public bool CanUpdateTimeTrackingConfig { get; set; }

        [JsonProperty("export_file_permission")]
        public ExportFilePermission ExportFilePermission { get; set; } = new ExportFilePermission();
    }

    public class ExportFilePermission {
        [JsonProperty("employee")]
        public bool Employee { get; set; }
        [JsonProperty("insurance")]
        public bool Insurance { get; set; }
        [JsonProperty("contract")]
        public bool Contract { get; set; }
        [JsonProperty("asset")]
        public bool Asset { get; set; }
        [JsonProperty("timesheet")]
        public bool Timesheet { get; set; }
        [JsonProperty("time_tracking")]
        public bool TimeTracking { get; set; }
        [JsonProperty("edit_time_tracking")]
        public bool EditTimeTracking { get; set; }
        [JsonProperty("request")]
        public bool Request { get; set; }
        [JsonProperty("payroll")]
        public bool Payroll { get; set; }
        [JsonProperty("kpi")]
        public bool Kpi { get; set; }
        [JsonProperty("task")]
        public bool Task { get; set; }
        [JsonProperty("my_report")]
        public bool MyReport { get; set; }
        [JsonProperty("request_approval")]
        public bool RequestApproval { get; set; }
        [JsonProperty("gps")]
        public bool Gps { get; set; }
        [JsonProperty("wifi")]
        public bool Wifi { get; set; }
        [JsonProperty("qr")]
        public bool Qr { get; set; }
        [JsonProperty("wanip")]
        public bool Wanip { get; set; }
        [JsonProperty("project")]
        public bool Project { get; set; }
        [JsonProperty("shift_list")]
        public bool ShiftList { get; set; }
        [JsonProperty("who_is_working")]
        public bool WhoIsWorking { get; set; }
        [JsonProperty("meal")]
        public bool Meal { get; set; }
        [JsonProperty("employee_dayleft")]
        public bool EmployeeDayleft { get; set; }
        [JsonProperty("promotion_history")]
        public bool PromotionHistory { get; set; }
        [JsonProperty("timesheet_task")]
        public bool TimesheetTask { get; set; }
    }
} 