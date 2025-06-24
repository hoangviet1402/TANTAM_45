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

    #region ClockInOutShift 
    public class ClockInOutShiftRequest
    {
        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int BranchId { get; set; }

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public int UserId { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("working_day", NullValueHandling = NullValueHandling.Ignore)]
        public string WorkingDay { get; set; }

        [JsonProperty("bssid", NullValueHandling = NullValueHandling.Ignore)]
        public string Bssid { get; set; }

        [JsonProperty("ssid", NullValueHandling = NullValueHandling.Ignore)]
        public string Ssid { get; set; }

        [JsonProperty("connection_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ConnectionType { get; set; }

        [JsonProperty("timekeeper_device", NullValueHandling = NullValueHandling.Ignore)]
        public string TimekeeperDevice { get; set; }

        [JsonProperty("employee_shift_id", NullValueHandling = NullValueHandling.Ignore)]
        public int EmployeeShiftId { get; set; }

        [JsonProperty("clock_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ClockType { get; set; }
    }
    #endregion

    public class ListOpenShiftRequest
    {
        [JsonProperty("startdate")]
        public string StartDate { get; set; }

        [JsonProperty("enddate")]
        public string EndDate { get; set; }
    }
}
