using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace BussinessObject.Models.Shift
{
    #region CreateShiftRequest
    public class ShiftCreateAndAssignRequest
    {
        [JsonProperty("shift", NullValueHandling = NullValueHandling.Ignore)]
        public ShiftData Shift { get; set; }

        [JsonProperty("shift_assignment", NullValueHandling = NullValueHandling.Ignore)]
        public ShiftAssignmentData ShiftAssignment { get; set; }

        [JsonProperty("break_times", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> BreakTimes { get; set; }

        [JsonProperty("onboarding_code", NullValueHandling = NullValueHandling.Ignore)]
        public string OnboardingCode { get; set; }

        [JsonProperty("is_onboarding", NullValueHandling = NullValueHandling.Ignore)]
        public int IsOnboarding { get; set; }
    }

    public class ShiftData
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("shift_key", NullValueHandling = NullValueHandling.Ignore)]
        public string ShiftKey { get; set; }

        [JsonProperty("start_hour_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? StartHourId { get; set; }

        [JsonProperty("start_minute_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? StartMinuteId { get; set; }

        [JsonProperty("end_hour_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? EndHourId { get; set; }

        [JsonProperty("end_minute_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? EndMinuteId { get; set; }

        [JsonProperty("coefficient", NullValueHandling = NullValueHandling.Ignore)]
        public int? Coefficient { get; set; }

        [JsonProperty("minimum_workinghour", NullValueHandling = NullValueHandling.Ignore)]
        public int? MinimumWorkingHour { get; set; }

        [JsonProperty("note", NullValueHandling = NullValueHandling.Ignore)]
        public string Note { get; set; }

        [JsonProperty("early_check_out", NullValueHandling = NullValueHandling.Ignore)]
        public int? EarlyCheckOut { get; set; }

        [JsonProperty("lately_check_in", NullValueHandling = NullValueHandling.Ignore)]
        public int? LatelyCheckIn { get; set; }

        [JsonProperty("max_late_check_in_out_minute", NullValueHandling = NullValueHandling.Ignore)]
        public int MaxLateCheckInOutMinute { get; set; }

        [JsonProperty("min_soon_check_in_out_minute", NullValueHandling = NullValueHandling.Ignore)]
        public int MinSoonCheckInOutMinute { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int? Status { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("sort_index", NullValueHandling = NullValueHandling.Ignore)]
        public int? SortIndex { get; set; }

        [JsonProperty("is_overtime_shift", NullValueHandling = NullValueHandling.Ignore)]
        public int? IsOvertimeShift { get; set; }

        [JsonProperty("branch_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> BranchIds { get; set; }

        [JsonProperty("meal_coefficient", NullValueHandling = NullValueHandling.Ignore)]
        public int? MealCoefficient { get; set; }

        [JsonProperty("timezone", NullValueHandling = NullValueHandling.Ignore)]
        public string Timezone { get; set; }

        [JsonProperty("start_check_in_minute_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? StartCheckInMinuteId { get; set; }

        [JsonProperty("end_check_in_minute_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? EndCheckInMinuteId { get; set; }

        [JsonProperty("start_check_out_minute_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? StartCheckOutMinuteId { get; set; }

        [JsonProperty("end_check_out_minute_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? EndCheckOutMinuteId { get; set; }

        [JsonProperty("start_check_in_hour_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? StartCheckInHourId { get; set; }

        [JsonProperty("end_check_in_hour_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? EndCheckInHourId { get; set; }

        [JsonProperty("start_check_out_hour_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? StartCheckOutHourId { get; set; }

        [JsonProperty("end_check_out_hour_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? EndCheckOutHourId { get; set; }        
    }


    #endregion

    #region CreateShiftResponse
    public class ShiftCreateAndAssignResponse
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("assignment_type", NullValueHandling = NullValueHandling.Ignore)]
        public string AssignmentType { get; set; }

        [JsonProperty("auto_approve", NullValueHandling = NullValueHandling.Ignore)]
        public int AutoApprove { get; set; }

        [JsonProperty("approver_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? ApproverId { get; set; }

        [JsonProperty("department_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> DepartmentIds { get; set; }

        [JsonProperty("user_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> UserIds { get; set; }

        [JsonProperty("position_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> PositionIds { get; set; }

        [JsonProperty("assignments", NullValueHandling = NullValueHandling.Ignore)]
        public List<int?> Assignments { get; set; }

        [JsonProperty("payroll_config_type", NullValueHandling = NullValueHandling.Ignore)]
        public string PayrollConfigType { get; set; }

        [JsonProperty("sort_index", NullValueHandling = NullValueHandling.Ignore)]
        public int SortIndex { get; set; }

        [JsonProperty("meal_coefficient", NullValueHandling = NullValueHandling.Ignore)]
        public int MealCoefficient { get; set; }

        [JsonProperty("branches", NullValueHandling = NullValueHandling.Ignore)]
        public List<BranchInfo> Branches { get; set; }

        [JsonProperty("positions", NullValueHandling = NullValueHandling.Ignore)]
        public List<PositionInfo> Positions { get; set; }

        [JsonProperty("departments", NullValueHandling = NullValueHandling.Ignore)]
        public List<DepartmentInfo> Departments { get; set; }

        [JsonProperty("assignment_objs", NullValueHandling = NullValueHandling.Ignore)]
        public List<AssignmentObj> AssignmentObjs { get; set; }

        [JsonProperty("generate_timekeeping_type_obj", NullValueHandling = NullValueHandling.Ignore)]
        public TypeObject GenerateTimekeepingTypeObj { get; set; }

        [JsonProperty("assignment_type_obj", NullValueHandling = NullValueHandling.Ignore)]
        public TypeObject AssignmentTypeObj { get; set; }

        [JsonProperty("shift", NullValueHandling = NullValueHandling.Ignore)]
        public ShiftResponse Shift { get; set; }
    }

    public class BranchInfo
    {
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public int Value { get; set; }

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }
    }

    public class DepartmentInfo
    {
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public int Value { get; set; }

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }
    }

    public class PositionInfo
    {
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public int Value { get; set; }

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }
    }

    public class AssignmentObj
    {
        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public int Key { get; set; }

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }
    }

    public class TypeObject
    {
        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }

        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }
    }

    public class ShiftResponse
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("name_nosign", NullValueHandling = NullValueHandling.Ignore)]
        public string NameNoSign { get; set; }

        [JsonProperty("shift_key", NullValueHandling = NullValueHandling.Ignore)]
        public string ShiftKey { get; set; }

        [JsonProperty("shift_type_obj", NullValueHandling = NullValueHandling.Ignore)]
        public ShiftTypeObject ShiftTypeObj { get; set; }

        [JsonProperty("start_hour_obj", NullValueHandling = NullValueHandling.Ignore)]
        public TimeObject StartHourObj { get; set; }

        [JsonProperty("start_minute_obj", NullValueHandling = NullValueHandling.Ignore)]
        public TimeObject StartMinuteObj { get; set; }

        [JsonProperty("end_hour_obj", NullValueHandling = NullValueHandling.Ignore)]
        public TimeObject EndHourObj { get; set; }

        [JsonProperty("end_minute_obj", NullValueHandling = NullValueHandling.Ignore)]
        public TimeObject EndMinuteObj { get; set; }

        [JsonProperty("coefficient", NullValueHandling = NullValueHandling.Ignore)]
        public int Coefficient { get; set; }

        [JsonProperty("shop_obj", NullValueHandling = NullValueHandling.Ignore)]
        public ShopObject ShopObj { get; set; }

        [JsonProperty("shop_id", NullValueHandling = NullValueHandling.Ignore)]
        public int ShopId { get; set; }

        [JsonProperty("note", NullValueHandling = NullValueHandling.Ignore)]
        public string Note { get; set; }

        [JsonProperty("start_check_in_hour_obj", NullValueHandling = NullValueHandling.Ignore)]
        public TimeObject StartCheckInHourObj { get; set; }

        [JsonProperty("start_check_in_minute_obj", NullValueHandling = NullValueHandling.Ignore)]
        public TimeObject StartCheckInMinuteObj { get; set; }    

        [JsonProperty("end_check_in_hour_obj", NullValueHandling = NullValueHandling.Ignore)]
        public TimeObject EndCheckInHourObj { get; set; }     

        [JsonProperty("end_check_in_minute_obj", NullValueHandling = NullValueHandling.Ignore)]
        public TimeObject EndCheckInMinuteObj { get; set; }        

        [JsonProperty("start_check_out_hour_obj", NullValueHandling = NullValueHandling.Ignore)]
        public TimeObject StartCheckOutHourObj { get; set; }        

        [JsonProperty("start_check_out_minute_obj", NullValueHandling = NullValueHandling.Ignore)]
        public TimeObject StartCheckOutMinuteObj { get; set; }
      
        [JsonProperty("end_check_out_hour_obj", NullValueHandling = NullValueHandling.Ignore)]
        public TimeObject EndCheckOutHourObj { get; set; }

        [JsonProperty("end_check_out_minute_obj", NullValueHandling = NullValueHandling.Ignore)]
        public TimeObject EndCheckOutMinuteObj { get; set; }

        [JsonProperty("early_check_out", NullValueHandling = NullValueHandling.Ignore)]
        public int EarlyCheckOut { get; set; }

        [JsonProperty("max_late_check_in_out_minute", NullValueHandling = NullValueHandling.Ignore)]
        public int MaxLateCheckInOutMinute { get; set; }

        [JsonProperty("min_soon_check_in_out_minute", NullValueHandling = NullValueHandling.Ignore)]
        public int MinSoonCheckInOutMinute { get; set; }

        [JsonProperty("lately_check_in", NullValueHandling = NullValueHandling.Ignore)]
        public int LatelyCheckIn { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int Status { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("rest_start_hour_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? RestStartHourId { get; set; }

        [JsonProperty("rest_start_minute_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? RestStartMinuteId { get; set; }

        [JsonProperty("rest_end_hour_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? RestEndHourId { get; set; }

        [JsonProperty("rest_end_minute_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? RestEndMinuteId { get; set; }

        [JsonProperty("working_hour", NullValueHandling = NullValueHandling.Ignore)]
        public double WorkingHour { get; set; }

        [JsonProperty("branch_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<BranchDetail> BranchIds { get; set; }      

        [JsonProperty("sort_index", NullValueHandling = NullValueHandling.Ignore)]
        public int SortIndex { get; set; }


        [JsonProperty("start_time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartTime { get; set; }

        [JsonProperty("end_time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EndTime { get; set; }

        [JsonProperty("start_check_in_time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartCheckInTime { get; set; }

        [JsonProperty("end_check_in_time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EndCheckInTime { get; set; }

        [JsonProperty("start_check_out_time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartCheckOutTime { get; set; }

        [JsonProperty("end_check_out_time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EndCheckOutTime { get; set; }

        [JsonProperty("rest_start_time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime RestStartTime { get; set; }

        [JsonProperty("rest_end_time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime RestEndTime { get; set; }

        [JsonProperty("is_overtime_shift", NullValueHandling = NullValueHandling.Ignore)]
        public int? IsOvertimeShift { get; set; }

        [JsonProperty("meal_coefficient", NullValueHandling = NullValueHandling.Ignore)]
        public decimal MealCoefficient { get; set; }

        [JsonProperty("list_enable_clock", NullValueHandling = NullValueHandling.Ignore)]
        public object ListEnableClock { get; set; }

        [JsonProperty("timekeeping_config_in", NullValueHandling = NullValueHandling.Ignore)]
        public object TimekeepingConfigIn { get; set; }

        [JsonProperty("timekeeping_config_out", NullValueHandling = NullValueHandling.Ignore)]
        public object TimekeepingConfigOut { get; set; }

        [JsonProperty("symbol", NullValueHandling = NullValueHandling.Ignore)]
        public string Symbol { get; set; }

        [JsonProperty("minimum_workinghour", NullValueHandling = NullValueHandling.Ignore)]
        public decimal MinimumWorkingHour { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        [JsonProperty("meal_type_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? MealTypeId { get; set; }

        [JsonProperty("break_times", NullValueHandling = NullValueHandling.Ignore)]
        public object BreakTimes { get; set; }

        [JsonProperty("timezone", NullValueHandling = NullValueHandling.Ignore)]
        public string Timezone { get; set; }
    }

    public class ShiftTypeObject
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
    }

    public class TimeObject
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public int Value { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
    }

    public class ShopObject
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class BranchDetail
    {
        [JsonProperty("branch_id_obj", NullValueHandling = NullValueHandling.Ignore)]
        public BranchObject BranchIdObj { get; set; }

        [JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
        public int Index { get; set; }
    }

    public class BranchObject
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }
    }

    #endregion

    #region EmployeeShiftSummary
    /// <summary>
    /// Request model for Employee Shift Summary API
    /// </summary>
    public class EmployeeShiftSummaryRequest
    {
        [JsonProperty("company_id", NullValueHandling = NullValueHandling.Ignore)]
        public int CompanyId { get; set; }

        [JsonProperty("month", NullValueHandling = NullValueHandling.Ignore)]
        public int Month { get; set; }

        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public int Year { get; set; }

        [JsonProperty("is_shift", NullValueHandling = NullValueHandling.Ignore)]
        public int IsShift { get; set; }

        [JsonProperty("is_status_shift", NullValueHandling = NullValueHandling.Ignore)]
        public int IsStatusShift { get; set; }

        [JsonProperty("is_quit", NullValueHandling = NullValueHandling.Ignore)]
        public int IsQuit { get; set; }

        [JsonProperty("is_no_need_timekeeping", NullValueHandling = NullValueHandling.Ignore)]
        public int IsNoNeedTimekeeping { get; set; }

        [JsonProperty("task_status_filter", NullValueHandling = NullValueHandling.Ignore)]
        public int TaskStatusFilter { get; set; }

        [JsonProperty("start_date", NullValueHandling = NullValueHandling.Ignore)]
        public string StartDate { get; set; }

        [JsonProperty("end_date", NullValueHandling = NullValueHandling.Ignore)]
        public string EndDate { get; set; }

        [JsonProperty("limit", NullValueHandling = NullValueHandling.Ignore)]
        public int Limit { get; set; }

        [JsonProperty("employee_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> EmployeeIds { get; set; }

        /// <summary>
        /// Filter để chỉ lấy ca làm việc (không bao gồm các thông tin khác)
        /// </summary>
        [JsonProperty("is_shift_only", NullValueHandling = NullValueHandling.Ignore)]
        public int IsShiftOnly { get; set; }

        /// <summary>
        /// ID của employee shift (user working day) để lọc dữ liệu cụ thể
        /// </summary>
        [JsonProperty("employee_shift_id", NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeShiftId { get; set; }

        /// <summary>
        /// Có bao gồm thông tin project hay không
        /// </summary>
        [JsonProperty("with_project", NullValueHandling = NullValueHandling.Ignore)]
        public int WithProject { get; set; }

        /// <summary>
        /// Có bao gồm thông tin chi nhánh hay không
        /// </summary>
        [JsonProperty("with_branch", NullValueHandling = NullValueHandling.Ignore)]
        public int WithBranch { get; set; }

        /// <summary>
        /// Chế độ hiển thị: tasks, calendar, list, etc.
        /// </summary>
        [JsonProperty("view_mode", NullValueHandling = NullValueHandling.Ignore)]
        public string ViewMode { get; set; }

        public EmployeeShiftSummaryRequest()
        {
            EmployeeIds = new List<string>();
            ViewMode = "tasks"; // Default view mode
        }
    }
    #endregion

    #region RejectShift

    /// <summary>
    /// Request model for Reject Shift API
    /// </summary>
    public class RejectShiftRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }
    }

    /// <summary>
    /// Response model for Reject Shift API
    /// </summary>
    public class RejectShiftResponse
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("working_day")]
        public string working_day { get; set; }

        [JsonProperty("shift_name")]
        public string shift_name { get; set; }
    }

    #endregion

    #region RegisterShift

    /// <summary>
    /// Request model for Register Shift API
    /// </summary>
    public class RegisterShiftRequest
    {
        [JsonProperty("shift_id")]
        public int shift_id { get; set; }

        [JsonProperty("working_day")]
        public string working_day { get; set; }

        [JsonProperty("user_id")]
        public int user_id { get; set; }
    }

    /// <summary>
    /// Response model for Register Shift API
    /// </summary>
    public class RegisterShiftResponse
    {
        [JsonProperty("total_updated")]
        public int total_updated { get; set; }

        [JsonProperty("working_day")]
        public string working_day { get; set; }

        [JsonProperty("shift_name")]
        public string shift_name { get; set; }
    }

    #endregion

    #region ListShift

    /// <summary>
    /// Request model for List Shift API
    /// </summary>
    public class ListShiftRequest
    {
        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("week")]
        public int Week { get; set; }

        [JsonProperty("week_of_year")]
        public int WeekOfYear { get; set; }

        [JsonProperty("branch_id")]
        public string BranchId { get; set; }

        [JsonProperty("weekly_rebuild")]
        public int WeeklyRebuild { get; set; }

        [JsonProperty("is_only_shift_week")]
        public int IsOnlyShiftWeek { get; set; }

        [JsonProperty("is_bypass_week_register_shift")]
        public int IsBypassWeekRegisterShift { get; set; }

        [JsonProperty("working_day")]
        public string WorkingDay { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }

    /// <summary>
    /// Response model for List Shift API  
    /// </summary>
    public class ListShiftResponse : Dictionary<string, ShiftListItem>
    {
    }

    /// <summary>
    /// Shift list item model
    /// </summary>
    public class ShiftListItem
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("shift_key")]
        public string shift_key { get; set; }

        [JsonProperty("shift_id")]
        public string shift_id { get; set; }

        [JsonProperty("start_time")]
        public string start_time { get; set; }

        [JsonProperty("end_time")]
        public string end_time { get; set; }

        [JsonProperty("working_hour")]
        public double working_hour { get; set; }

        [JsonProperty("working_day")]
        public string working_day { get; set; }

        [JsonProperty("week_of_year")]
        public int week_of_year { get; set; }

        [JsonProperty("branch_id")]
        public string branch_id { get; set; }

        [JsonProperty("total_register")]
        public int total_register { get; set; }

        [JsonProperty("is_confirm")]
        public object is_confirm { get; set; }

        [JsonProperty("sort_index")]
        public int sort_index { get; set; }

        [JsonProperty("end_working_date")]
        public object end_working_date { get; set; }

        [JsonProperty("timezone")]
        public string timezone { get; set; }
    }

    #endregion

    #region  HistoryEmployeeShift

    public class HistoryEmployeeShiftRequest
    {
        [JsonProperty("shift_key", NullValueHandling = NullValueHandling.Ignore)]
        public int ShiftID { get; set; }
        [JsonProperty("week_of_year", NullValueHandling = NullValueHandling.Ignore)]
        public int WeekOfYear { get; set; }
        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public int Year { get; set; }
        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int BranchId { get; set; }
        //?shift_key=H
        //&branch_id=682ef049dc534fa14b0dedf4
        //&week_of_year=26&year=2025
    }
    public class HistoryEmployeeShiftResponse
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("shift_key", NullValueHandling = NullValueHandling.Ignore)]
        public string ShiftKey { get; set; }

        [JsonProperty("shift_id", NullValueHandling = NullValueHandling.Ignore)]
        public int ShiftId { get; set; }

        [JsonProperty("start_time", NullValueHandling = NullValueHandling.Ignore)]
        public string StartTime { get; set; }

        [JsonProperty("end_time", NullValueHandling = NullValueHandling.Ignore)]
        public string EndTime { get; set; }

        [JsonProperty("working_hour", NullValueHandling = NullValueHandling.Ignore)]
        public double WorkingHour { get; set; }

        [JsonProperty("working_day", NullValueHandling = NullValueHandling.Ignore)]
        public string WorkingDay { get; set; }

        [JsonProperty("week_of_year", NullValueHandling = NullValueHandling.Ignore)]
        public int WeekOfYear { get; set; }

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int BranchId { get; set; }

        [JsonProperty("total_register", NullValueHandling = NullValueHandling.Ignore)]
        public int TotalRegister { get; set; }

        [JsonProperty("is_confirm", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsConfirm { get; set; }

        [JsonProperty("sort_index", NullValueHandling = NullValueHandling.Ignore)]
        public int SortIndex { get; set; }

        [JsonProperty("end_working_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EndWorkingDate { get; set; }

        [JsonProperty("timezone", NullValueHandling = NullValueHandling.Ignore)]
        public string Timezone { get; set; }

        [JsonProperty("employees", NullValueHandling = NullValueHandling.Ignore)]
        public List<HistoryEmployeeShiftResponse_EmployeeInfo> Employees { get; set; }
    }

    public class HistoryEmployeeShiftResponse_EmployeeInfo
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public int UserId { get; set; }

        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class ShiftLite_ForRegisterRequest
    {
        [JsonProperty("week_of_year", NullValueHandling = NullValueHandling.Ignore)]
        public int WeekOfYear { get; set; }
        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public int Year { get; set; }
        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int BranchId { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        //?week_of_year=26
        //&year=2025
        //&branch_id=682ef049dc534fa14b0dedf4
        //&type=register-shift
    }
    public class ShiftLite_ForRegisterResponse
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("shift_key", NullValueHandling = NullValueHandling.Ignore)]
        public string ShiftKey { get; set; }

        [JsonProperty("shift_id", NullValueHandling = NullValueHandling.Ignore)]
        public int ShiftId { get; set; }   // Yêu cầu để int, cần đảm bảo JSON trả về số thay vì chuỗi

        [JsonProperty("start_time", NullValueHandling = NullValueHandling.Ignore)]
        public string StartTime { get; set; }

        [JsonProperty("working_day", NullValueHandling = NullValueHandling.Ignore)]
        public string WorkingDay { get; set; }

        [JsonProperty("end_time", NullValueHandling = NullValueHandling.Ignore)]
        public string EndTime { get; set; }

        [JsonProperty("working_hour", NullValueHandling = NullValueHandling.Ignore)]
        public double WorkingHour { get; set; }

        [JsonProperty("end_working_date", NullValueHandling = NullValueHandling.Ignore)]
        public string EndWorkingDate { get; set; }

        [JsonProperty("timezone", NullValueHandling = NullValueHandling.Ignore)]
        public string Timezone { get; set; }
    }
    #endregion

    #region  list-by-shift-assignment
    public class ListForAddShiftAssignmentResponse
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
        public string ShopId { get; set; }

        [JsonProperty("identification", NullValueHandling = NullValueHandling.Ignore)]
        public string Identification { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("is_tanca_phone", NullValueHandling = NullValueHandling.Ignore)]
        public int IsTancaPhone { get; set; }

        [JsonProperty("is_tanca_email", NullValueHandling = NullValueHandling.Ignore)]
        public int IsTancaEmail { get; set; }

        [JsonProperty("sort_index", NullValueHandling = NullValueHandling.Ignore)]
        public int SortIndex { get; set; }

        [JsonProperty("last_activity", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastActivity { get; set; }

        [JsonProperty("region_id", NullValueHandling = NullValueHandling.Ignore)]
        public string RegionId { get; set; }

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public string BranchId { get; set; }

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
        public ListForAddShiftAssignmentResponse_PositionObj PositionObj { get; set; }

        [JsonProperty("department_obj", NullValueHandling = NullValueHandling.Ignore)]
        public ListForAddShiftAssignmentResponse_DepartmentObj DepartmentObj { get; set; }

        [JsonProperty("branch_obj", NullValueHandling = NullValueHandling.Ignore)]
        public ListForAddShiftAssignmentResponse_BranchObj BranchObj { get; set; }

        [JsonProperty("group_obj", NullValueHandling = NullValueHandling.Ignore)]
        public ListForAddShiftAssignmentResponse_GroupObj GroupObj { get; set; }

        [JsonProperty("region_obj")]
        public ListForAddShiftAssignmentResponse_RegionObj RegionObj { get; set; }
    }

    public class ListForAddShiftAssignmentResponse_PositionObj
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class ListForAddShiftAssignmentResponse_DepartmentObj
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class ListForAddShiftAssignmentResponse_BranchObj
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class ListForAddShiftAssignmentResponse_GroupObj
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("client_role", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientRole { get; set; }
    }

    public class ListForAddShiftAssignmentResponse_RegionObj
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }
    #endregion 
}
