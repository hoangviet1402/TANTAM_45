using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace BussinessObject.Models.Shift
{
    #region CreateShiftRequest
    public class ShiftCreateAndAssignRequest
    {
        [JsonProperty("shift")]
        public ShiftData Shift { get; set; }

        [JsonProperty("shift_assignment")]
        public ShiftAssignmentData ShiftAssignment { get; set; }

        [JsonProperty("break_times")]
        public List<object> BreakTimes { get; set; }

        [JsonProperty("onboarding_code")]
        public string OnboardingCode { get; set; }

        [JsonProperty("is_onboarding")]
        public int IsOnboarding { get; set; }
    }

    public class ShiftData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("shift_key")]
        public string ShiftKey { get; set; }

        [JsonProperty("start_hour_id")]
        public int? StartHourId { get; set; }

        [JsonProperty("start_minute_id")]
        public int? StartMinuteId { get; set; }

        [JsonProperty("end_hour_id")]
        public int? EndHourId { get; set; }

        [JsonProperty("end_minute_id")]
        public int? EndMinuteId { get; set; }

        [JsonProperty("coefficient")]
        public int? Coefficient { get; set; }

        [JsonProperty("minimum_workinghour")]
        public int? MinimumWorkingHour { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("early_check_out")]
        public int? EarlyCheckOut { get; set; }

        [JsonProperty("lately_check_in")]
        public int? LatelyCheckIn { get; set; }

        [JsonProperty("max_late_check_in_out_minute")]
        public int MaxLateCheckInOutMinute { get; set; }

        [JsonProperty("min_soon_check_in_out_minute")]
        public int MinSoonCheckInOutMinute { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("sort_index")]
        public int? SortIndex { get; set; }

        [JsonProperty("is_overtime_shift")]
        public int? IsOvertimeShift { get; set; }

        [JsonProperty("branch_ids")]
        public List<int> BranchIds { get; set; }

        [JsonProperty("meal_coefficient")]
        public int? MealCoefficient { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("start_check_in_minute_id")]
        public int? StartCheckInMinuteId { get; set; }

        [JsonProperty("end_check_in_minute_id")]
        public int? EndCheckInMinuteId { get; set; }

        [JsonProperty("start_check_out_minute_id")]
        public int? StartCheckOutMinuteId { get; set; }

        [JsonProperty("end_check_out_minute_id")]
        public int? EndCheckOutMinuteId { get; set; }

        [JsonProperty("start_check_in_hour_id")]
        public int? StartCheckInHourId { get; set; }

        [JsonProperty("end_check_in_hour_id")]
        public int? EndCheckInHourId { get; set; }

        [JsonProperty("start_check_out_hour_id")]
        public int? StartCheckOutHourId { get; set; }

        [JsonProperty("end_check_out_hour_id")]
        public int? EndCheckOutHourId { get; set; }

    }


    #endregion

    #region CreateShiftResponse
    public class ShiftCreateAndAssignResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("assignment_type")]
        public string AssignmentType { get; set; }

        [JsonProperty("auto_approve")]
        public int AutoApprove { get; set; }

        [JsonProperty("approver_id")]
        public int? ApproverId { get; set; }

        [JsonProperty("department_ids")]
        public List<int> DepartmentIds { get; set; }

        [JsonProperty("user_ids")]
        public List<int> UserIds { get; set; }

        [JsonProperty("position_ids")]
        public List<int> PositionIds { get; set; }

        [JsonProperty("assignments")]
        public List<int?> Assignments { get; set; }

        [JsonProperty("payroll_config_type")]
        public string PayrollConfigType { get; set; }

        [JsonProperty("sort_index")]
        public int SortIndex { get; set; }

        [JsonProperty("meal_coefficient")]
        public int MealCoefficient { get; set; }

        [JsonProperty("branches")]
        public List<BranchInfo> Branches { get; set; }

        [JsonProperty("positions")]
        public List<PositionInfo> Positions { get; set; }

        [JsonProperty("departments")]
        public List<DepartmentInfo> Departments { get; set; }

        [JsonProperty("assignment_objs")]
        public List<AssignmentObj> AssignmentObjs { get; set; }

        [JsonProperty("generate_timekeeping_type_obj")]
        public TypeObject GenerateTimekeepingTypeObj { get; set; }

        [JsonProperty("assignment_type_obj")]
        public TypeObject AssignmentTypeObj { get; set; }

        [JsonProperty("shift")]
        public ShiftResponse Shift { get; set; }
    }

    public class BranchInfo
    {
        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }

    public class DepartmentInfo
    {
        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }

    public class PositionInfo
    {
        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }

    public class AssignmentObj
    {
        [JsonProperty("key")]
        public int Key { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }

    public class TypeObject
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }

    public class ShiftResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("name_nosign")]
        public string NameNoSign { get; set; }

        [JsonProperty("shift_key")]
        public string ShiftKey { get; set; }

        [JsonProperty("shift_type_obj")]
        public ShiftTypeObject ShiftTypeObj { get; set; }

        [JsonProperty("start_hour_obj")]
        public TimeObject StartHourObj { get; set; }

        [JsonProperty("start_minute_obj")]
        public TimeObject StartMinuteObj { get; set; }

        [JsonProperty("end_hour_obj")]
        public TimeObject EndHourObj { get; set; }

        [JsonProperty("end_minute_obj")]
        public TimeObject EndMinuteObj { get; set; }

        [JsonProperty("coefficient")]
        public int Coefficient { get; set; }

        [JsonProperty("shop_obj")]
        public ShopObject ShopObj { get; set; }

        [JsonProperty("shop_id")]
        public int ShopId { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("start_check_in_hour_obj")]
        public TimeObject StartCheckInHourObj { get; set; }

        [JsonProperty("start_check_in_minute_obj")]
        public TimeObject StartCheckInMinuteObj { get; set; }    

        [JsonProperty("end_check_in_hour_obj")]
        public TimeObject EndCheckInHourObj { get; set; }     

        [JsonProperty("end_check_in_minute_obj")]
        public TimeObject EndCheckInMinuteObj { get; set; }        

        [JsonProperty("start_check_out_hour_obj")]
        public TimeObject StartCheckOutHourObj { get; set; }        

        [JsonProperty("start_check_out_minute_obj")]
        public TimeObject StartCheckOutMinuteObj { get; set; }
      
        [JsonProperty("end_check_out_hour_obj")]
        public TimeObject EndCheckOutHourObj { get; set; }

        [JsonProperty("end_check_out_minute_obj")]
        public TimeObject EndCheckOutMinuteObj { get; set; }

        [JsonProperty("early_check_out")]
        public int EarlyCheckOut { get; set; }

        [JsonProperty("max_late_check_in_out_minute")]
        public int MaxLateCheckInOutMinute { get; set; }

        [JsonProperty("min_soon_check_in_out_minute")]
        public int MinSoonCheckInOutMinute { get; set; }

        [JsonProperty("lately_check_in")]
        public int LatelyCheckIn { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("rest_start_hour_id")]
        public int? RestStartHourId { get; set; }

        [JsonProperty("rest_start_minute_id")]
        public int? RestStartMinuteId { get; set; }

        [JsonProperty("rest_end_hour_id")]
        public int? RestEndHourId { get; set; }

        [JsonProperty("rest_end_minute_id")]
        public int? RestEndMinuteId { get; set; }

        [JsonProperty("working_hour")]
        public double WorkingHour { get; set; }

        [JsonProperty("branch_ids")]
        public List<BranchDetail> BranchIds { get; set; }      

        [JsonProperty("sort_index")]
        public int SortIndex { get; set; }


        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }

        [JsonProperty("end_time")]
        public DateTime EndTime { get; set; }

        [JsonProperty("start_check_in_time")]
        public DateTime StartCheckInTime { get; set; }

        [JsonProperty("end_check_in_time")]
        public DateTime EndCheckInTime { get; set; }

        [JsonProperty("start_check_out_time")]
        public DateTime StartCheckOutTime { get; set; }

        [JsonProperty("end_check_out_time")]
        public DateTime EndCheckOutTime { get; set; }

        [JsonProperty("rest_start_time")]
        public DateTime RestStartTime { get; set; }

        [JsonProperty("rest_end_time")]
        public DateTime RestEndTime { get; set; }

        [JsonProperty("is_overtime_shift")]
        public int? IsOvertimeShift { get; set; }

        [JsonProperty("meal_coefficient")]
        public decimal MealCoefficient { get; set; }

        [JsonProperty("list_enable_clock")]
        public object ListEnableClock { get; set; }

        [JsonProperty("timekeeping_config_in")]
        public object TimekeepingConfigIn { get; set; }

        [JsonProperty("timekeeping_config_out")]
        public object TimekeepingConfigOut { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("minimum_workinghour")]
        public decimal MinimumWorkingHour { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("meal_type_id")]
        public int? MealTypeId { get; set; }

        [JsonProperty("break_times")]
        public object BreakTimes { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }
    }

    public class ShiftTypeObject
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class TimeObject
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class ShopObject
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class BranchDetail
    {
        [JsonProperty("branch_id_obj")]
        public BranchObject BranchIdObj { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }
    }

    public class BranchObject
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }
    }

    #endregion

    #region ClockInOutShift 
    public class ClockInOutShiftRequest
    {
        [JsonProperty("branch_id")]
        public int BranchId { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("working_day")]
        public string WorkingDay { get; set; }

        [JsonProperty("bssid")]
        public string Bssid { get; set; }

        [JsonProperty("ssid")]
        public string Ssid { get; set; }

        [JsonProperty("connection_type")]
        public string ConnectionType { get; set; }

        [JsonProperty("timekeeper_device")]
        public string TimekeeperDevice { get; set; }

        [JsonProperty("employee_shift_id")]
        public int EmployeeShiftId { get; set; }

        [JsonProperty("clock_type")]
        public string ClockType { get; set; }
    }
     #endregion
}
