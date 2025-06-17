using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TanTamApi.Models.Auth
{
    public class OrganizationDto
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("is_tanca_email", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsTancaEmail { get; set; }

        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [JsonProperty("phone_code", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneCode { get; set; }

        [JsonProperty("is_tanca_phone", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsTancaPhone { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        [JsonProperty("birthday", NullValueHandling = NullValueHandling.Ignore)]
        public string Birthday { get; set; }

        [JsonProperty("sex", NullValueHandling = NullValueHandling.Ignore)]
        public int Sex { get; set; }

        [JsonProperty("identify_card", NullValueHandling = NullValueHandling.Ignore)]
        public string IdentifyCard { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty("is_root", NullValueHandling = NullValueHandling.Ignore)]
        public int IsRoot { get; set; }

        [JsonProperty("birthplace", NullValueHandling = NullValueHandling.Ignore)]
        public string Birthplace { get; set; }

        [JsonProperty("sub_department", NullValueHandling = NullValueHandling.Ignore)]
        public string SubDepartment { get; set; }

        [JsonProperty("sub_team", NullValueHandling = NullValueHandling.Ignore)]
        public string SubTeam { get; set; }

        [JsonProperty("nationality", NullValueHandling = NullValueHandling.Ignore)]
        public string Nationality { get; set; }

        [JsonProperty("seniority_date", NullValueHandling = NullValueHandling.Ignore)]
        public string SeniorityDate { get; set; }

        [JsonProperty("name_no_sign_shopee", NullValueHandling = NullValueHandling.Ignore)]
        public string NameNoSignShopee { get; set; }

        [JsonProperty("direct_manager_id", NullValueHandling = NullValueHandling.Ignore)]
        public string DirectManagerId { get; set; }

        [JsonProperty("alias", NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }

        [JsonProperty("view_allowance_info", NullValueHandling = NullValueHandling.Ignore)]
        public bool ViewAllowanceInfo { get; set; }

        [JsonProperty("view_promotion_history", NullValueHandling = NullValueHandling.Ignore)]
        public bool ViewPromotionHistory { get; set; }

        [JsonProperty("disable_integration_menu", NullValueHandling = NullValueHandling.Ignore)]
        public bool DisableIntegrationMenu { get; set; }

        [JsonProperty("can_update_permission", NullValueHandling = NullValueHandling.Ignore)]
        public bool CanUpdatePermission { get; set; }

        [JsonProperty("can_update_timetracking_config", NullValueHandling = NullValueHandling.Ignore)]
        public bool CanUpdateTimetrackingConfig { get; set; }

        [JsonProperty("assign_import_task_timesheet_setting", NullValueHandling = NullValueHandling.Ignore)]
        public bool AssignImportTaskTimesheetSetting { get; set; }

        [JsonProperty("crud_task_timesheet_setting", NullValueHandling = NullValueHandling.Ignore)]
        public bool CrudTaskTimesheetSetting { get; set; }

        [JsonProperty("can_use_publish_shifts", NullValueHandling = NullValueHandling.Ignore)]
        public bool CanUsePublishShifts { get; set; }

        [JsonProperty("export_file_permission", NullValueHandling = NullValueHandling.Ignore)]
        public ExportFilePermissionDto ExportFilePermission { get; set; }

        [JsonProperty("lock_timesheet_permission", NullValueHandling = NullValueHandling.Ignore)]
        public int LockTimesheetPermission { get; set; }

        [JsonProperty("organization_id", NullValueHandling = NullValueHandling.Ignore)]
        public string OrganizationId { get; set; }

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public string UserId { get; set; }

        [JsonProperty("package", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Package { get; set; }

        [JsonProperty("shop_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ShopId { get; set; }

        [JsonProperty("region_id", NullValueHandling = NullValueHandling.Ignore)]
        public string RegionId { get; set; }

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public string BranchId { get; set; }

        [JsonProperty("position_id", NullValueHandling = NullValueHandling.Ignore)]
        public string PositionId { get; set; }

        [JsonProperty("department_id", NullValueHandling = NullValueHandling.Ignore)]
        public string DepartmentId { get; set; }

        [JsonProperty("group_id", NullValueHandling = NullValueHandling.Ignore)]
        public string GroupId { get; set; }

        [JsonProperty("salary", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Salary { get; set; }

        [JsonProperty("tax", NullValueHandling = NullValueHandling.Ignore)]
        public string Tax { get; set; }

        [JsonProperty("union", NullValueHandling = NullValueHandling.Ignore)]
        public string Union { get; set; }

        [JsonProperty("client_role", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientRole { get; set; }

        [JsonProperty("identification", NullValueHandling = NullValueHandling.Ignore)]
        public string Identification { get; set; }

        [JsonProperty("payroll_config_element_id", NullValueHandling = NullValueHandling.Ignore)]
        public string PayrollConfigElementId { get; set; }

        [JsonProperty("payroll_config_id", NullValueHandling = NullValueHandling.Ignore)]
        public string PayrollConfigId { get; set; }

        [JsonProperty("payroll_config_date", NullValueHandling = NullValueHandling.Ignore)]
        public string PayrollConfigDate { get; set; }

        [JsonProperty("labour_end_date", NullValueHandling = NullValueHandling.Ignore)]
        public string LabourEndDate { get; set; }

        [JsonProperty("working_date", NullValueHandling = NullValueHandling.Ignore)]
        public string WorkingDate { get; set; }

        [JsonProperty("is_multi_mobile", NullValueHandling = NullValueHandling.Ignore)]
        public int IsMultiMobile { get; set; }

        [JsonProperty("is_in_out_without_wifi", NullValueHandling = NullValueHandling.Ignore)]
        public int IsInOutWithoutWifi { get; set; }

        [JsonProperty("is_no_need_timekeeping", NullValueHandling = NullValueHandling.Ignore)]
        public int IsNoNeedTimekeeping { get; set; }

        [JsonProperty("is_auto_timekeeping", NullValueHandling = NullValueHandling.Ignore)]
        public int IsAutoTimekeeping { get; set; }

        [JsonProperty("is_help_check_shift", NullValueHandling = NullValueHandling.Ignore)]
        public int IsHelpCheckShift { get; set; }

        [JsonProperty("is_clock_in_using_image", NullValueHandling = NullValueHandling.Ignore)]
        public int IsClockInUsingImage { get; set; }

        [JsonProperty("is_clock_out_using_image", NullValueHandling = NullValueHandling.Ignore)]
        public int IsClockOutUsingImage { get; set; }

        [JsonProperty("is_location_tracking", NullValueHandling = NullValueHandling.Ignore)]
        public int IsLocationTracking { get; set; }

        [JsonProperty("is_enable_auto_login", NullValueHandling = NullValueHandling.Ignore)]
        public int IsEnableAutoLogin { get; set; }

        [JsonProperty("is_inactive", NullValueHandling = NullValueHandling.Ignore)]
        public int IsInactive { get; set; }

        [JsonProperty("is_allowing_early_check_in_out", NullValueHandling = NullValueHandling.Ignore)]
        public int IsAllowingEarlyCheckInOut { get; set; }

        [JsonProperty("is_allowing_lately_check_in_out", NullValueHandling = NullValueHandling.Ignore)]
        public int IsAllowingLatelyCheckInOut { get; set; }

        [JsonProperty("is_help_check_shift_using_image", NullValueHandling = NullValueHandling.Ignore)]
        public int IsHelpCheckShiftUsingImage { get; set; }

        [JsonProperty("is_auto_checkout", NullValueHandling = NullValueHandling.Ignore)]
        public int IsAutoCheckout { get; set; }

        [JsonProperty("setting", NullValueHandling = NullValueHandling.Ignore)]
        public OrganizationSettingDto Setting { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("is_head_of_department", NullValueHandling = NullValueHandling.Ignore)]
        public int IsHeadOfDepartment { get; set; }

        [JsonProperty("is_quit", NullValueHandling = NullValueHandling.Ignore)]
        public int IsQuit { get; set; }

        [JsonProperty("crm_noti_count", NullValueHandling = NullValueHandling.Ignore)]
        public int CrmNotiCount { get; set; }

        [JsonProperty("sort_index", NullValueHandling = NullValueHandling.Ignore)]
        public int SortIndex { get; set; }

        [JsonProperty("checkin_late", NullValueHandling = NullValueHandling.Ignore)]
        public int CheckinLate { get; set; }

        [JsonProperty("checkout_early", NullValueHandling = NullValueHandling.Ignore)]
        public int CheckoutEarly { get; set; }

        [JsonProperty("total_minutes_check_late_early", NullValueHandling = NullValueHandling.Ignore)]
        public int TotalMinutesCheckLateEarly { get; set; }

        [JsonProperty("total_minutes_check_late_early_flag", NullValueHandling = NullValueHandling.Ignore)]
        public int TotalMinutesCheckLateEarlyFlag { get; set; }

        [JsonProperty("checkin_late_ranges", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> CheckinLateRanges { get; set; }

        [JsonProperty("checkout_early_ranges", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> CheckoutEarlyRanges { get; set; }

        [JsonProperty("total_minutes_check_late_early_ranges", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> TotalMinutesCheckLateEarlyRanges { get; set; }

        [JsonProperty("shop_username", NullValueHandling = NullValueHandling.Ignore)]
        public string ShopUsername { get; set; }

        [JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
        public string Position { get; set; }

        [JsonProperty("department", NullValueHandling = NullValueHandling.Ignore)]
        public string Department { get; set; }

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
        public object PositionObj { get; set; }

        [JsonProperty("department_obj", NullValueHandling = NullValueHandling.Ignore)]
        public object DepartmentObj { get; set; }

        [JsonProperty("shop_obj", NullValueHandling = NullValueHandling.Ignore)]
        public ShopObjDto ShopObj { get; set; }

        [JsonProperty("region_obj", NullValueHandling = NullValueHandling.Ignore)]
        public object RegionObj { get; set; }

        [JsonProperty("branch_obj", NullValueHandling = NullValueHandling.Ignore)]
        public object BranchObj { get; set; }

        [JsonProperty("payroll_config_obj", NullValueHandling = NullValueHandling.Ignore)]
        public object PayrollConfigObj { get; set; }

        [JsonProperty("group_obj", NullValueHandling = NullValueHandling.Ignore)]
        public GroupObjDto GroupObj { get; set; }

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public UserDto User { get; set; }

        [JsonProperty("lock_timesheet_status", NullValueHandling = NullValueHandling.Ignore)]
        public int LockTimesheetStatus { get; set; }

        [JsonProperty("shop_amount", NullValueHandling = NullValueHandling.Ignore)]
        public int ShopAmount { get; set; }
    }

    public class ExportFilePermissionDto
    {
        [JsonProperty("employee", NullValueHandling = NullValueHandling.Ignore)]
        public int Employee { get; set; }
        [JsonProperty("insurance", NullValueHandling = NullValueHandling.Ignore)]
        public int Insurance { get; set; }
        [JsonProperty("contract", NullValueHandling = NullValueHandling.Ignore)]
        public int Contract { get; set; }
        [JsonProperty("asset", NullValueHandling = NullValueHandling.Ignore)]
        public int Asset { get; set; }
        [JsonProperty("timesheet", NullValueHandling = NullValueHandling.Ignore)]
        public int Timesheet { get; set; }
        [JsonProperty("time_tracking", NullValueHandling = NullValueHandling.Ignore)]
        public int TimeTracking { get; set; }
        [JsonProperty("edit_time_tracking", NullValueHandling = NullValueHandling.Ignore)]
        public int EditTimeTracking { get; set; }
        [JsonProperty("request", NullValueHandling = NullValueHandling.Ignore)]
        public int Request { get; set; }
        [JsonProperty("payroll", NullValueHandling = NullValueHandling.Ignore)]
        public int Payroll { get; set; }
        [JsonProperty("kpi", NullValueHandling = NullValueHandling.Ignore)]
        public int Kpi { get; set; }
        [JsonProperty("task", NullValueHandling = NullValueHandling.Ignore)]
        public int Task { get; set; }
        [JsonProperty("my_report", NullValueHandling = NullValueHandling.Ignore)]
        public int MyReport { get; set; }
        [JsonProperty("request_approval", NullValueHandling = NullValueHandling.Ignore)]
        public int RequestApproval { get; set; }
        [JsonProperty("gps", NullValueHandling = NullValueHandling.Ignore)]
        public int Gps { get; set; }
        [JsonProperty("wifi", NullValueHandling = NullValueHandling.Ignore)]
        public int Wifi { get; set; }
        [JsonProperty("qr", NullValueHandling = NullValueHandling.Ignore)]
        public int Qr { get; set; }
        [JsonProperty("wanip", NullValueHandling = NullValueHandling.Ignore)]
        public int Wanip { get; set; }
        [JsonProperty("project", NullValueHandling = NullValueHandling.Ignore)]
        public int Project { get; set; }
        [JsonProperty("shift_list", NullValueHandling = NullValueHandling.Ignore)]
        public int ShiftList { get; set; }
        [JsonProperty("who_is_working", NullValueHandling = NullValueHandling.Ignore)]
        public int WhoIsWorking { get; set; }
        [JsonProperty("meal", NullValueHandling = NullValueHandling.Ignore)]
        public int Meal { get; set; }
        [JsonProperty("employee_dayleft", NullValueHandling = NullValueHandling.Ignore)]
        public int EmployeeDayleft { get; set; }
        [JsonProperty("promotion_history", NullValueHandling = NullValueHandling.Ignore)]
        public int PromotionHistory { get; set; }
        [JsonProperty("timesheet_task", NullValueHandling = NullValueHandling.Ignore)]
        public int TimesheetTask { get; set; }
    }

    public class OrganizationSettingDto
    {
        [JsonProperty("auto_confirm_shift", NullValueHandling = NullValueHandling.Ignore)]
        public int AutoConfirmShift { get; set; }
        [JsonProperty("clock_out_notification", NullValueHandling = NullValueHandling.Ignore)]
        public int ClockOutNotification { get; set; }
    }

    public class ShopObjDto
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class GroupObjDto
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("client_role", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientRole { get; set; }
    }

    public class UserDto
    {
        [JsonProperty("active", NullValueHandling = NullValueHandling.Ignore)]
        public bool Active { get; set; }
        [JsonProperty("avatarOrigin", NullValueHandling = NullValueHandling.Ignore)]
        public string AvatarOrigin { get; set; }
        [JsonProperty("birthday", NullValueHandling = NullValueHandling.Ignore)]
        public object Birthday { get; set; }
        [JsonProperty("brandRepresent", NullValueHandling = NullValueHandling.Ignore)]
        public object BrandRepresent { get; set; }
        [JsonProperty("company", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Company { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("job", NullValueHandling = NullValueHandling.Ignore)]
        public object Job { get; set; }
        [JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
        public string Language { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("sex", NullValueHandling = NullValueHandling.Ignore)]
        public int Sex { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
        [JsonProperty("statusConnection", NullValueHandling = NullValueHandling.Ignore)]
        public string StatusConnection { get; set; }
        [JsonProperty("statusDefault", NullValueHandling = NullValueHandling.Ignore)]
        public string StatusDefault { get; set; }
        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }
        [JsonProperty("utcOffset", NullValueHandling = NullValueHandling.Ignore)]
        public int UtcOffset { get; set; }
        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("_version", NullValueHandling = NullValueHandling.Ignore)]
        public int Version { get; set; }
        [JsonProperty("apiToken", NullValueHandling = NullValueHandling.Ignore)]
        public string ApiToken { get; set; }
    }
}