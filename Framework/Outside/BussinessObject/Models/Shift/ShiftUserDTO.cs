using Newtonsoft.Json;
using System.Collections.Generic;

namespace BussinessObject.Models.Shift
{
    public class StatusClockInOutShiftResponse
    {
        [JsonProperty("clock_setting", NullValueHandling = NullValueHandling.Ignore)]
        public ClockSetting ClockSetting { get; set; }

        [JsonProperty("clock_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ClockType { get; set; }

        [JsonProperty("current_employee_shift", NullValueHandling = NullValueHandling.Ignore)]
        public CurrentEmployeeShift CurrentEmployeeShift { get; set; }

        [JsonProperty("employee_shifts", NullValueHandling = NullValueHandling.Ignore)]
        public List<EmployeeShift> EmployeeShifts { get; set; }

        [JsonProperty("timekeeper_log", NullValueHandling = NullValueHandling.Ignore)]
        public TimekeeperLog TimekeeperLog { get; set; }
    }

    public class ClockInOutShiftResponse
    {

        [JsonProperty("next_clock_type", NullValueHandling = NullValueHandling.Ignore)]
        public string NextClockType { get; set; }

        [JsonProperty("current_employee_shift", NullValueHandling = NullValueHandling.Ignore)]
        public ClockInOut_Shift CurrentEmployeeShift { get; set; }


        [JsonProperty("timekeeper_log", NullValueHandling = NullValueHandling.Ignore)]
        public TimekeeperLog TimekeeperLog { get; set; }
    }

    public class ClockSetting
    {
        [JsonProperty("clock_in_out_requirements", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ClockInOutRequirements { get; set; }

        [JsonProperty("is_location_tracking", NullValueHandling = NullValueHandling.Ignore)]
        public int? IsLocationTracking { get; set; }

        [JsonProperty("distance", NullValueHandling = NullValueHandling.Ignore)]
        public int? Distance { get; set; }

        [JsonProperty("debug", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Debug { get; set; }

        [JsonProperty("logLevel", NullValueHandling = NullValueHandling.Ignore)]
        public int? LogLevel { get; set; }
    }

    public class CurrentEmployeeShift
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class EmployeeShift
    {
        [JsonProperty("shift", NullValueHandling = NullValueHandling.Ignore)]
        public ClockInOut_Shift ClockInOut_Shift_Info { get; set; }

        [JsonProperty("is_reason", NullValueHandling = NullValueHandling.Ignore)]
        public int? IsReason { get; set; }

        [JsonProperty("is_yesterday", NullValueHandling = NullValueHandling.Ignore)]
        public int? IsYesterday { get; set; }

        [JsonProperty("is_end_next_day", NullValueHandling = NullValueHandling.Ignore)]
        public int? IsEndNextDay { get; set; }
    }

    public class ClockInOut_Shift
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("shift_key", NullValueHandling = NullValueHandling.Ignore)]
        public string ShiftKey { get; set; }

        [JsonProperty("shift_id", NullValueHandling = NullValueHandling.Ignore)]
        public int ShiftId { get; set; }

        [JsonProperty("shift_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ShiftType { get; set; }

        [JsonProperty("start_time", NullValueHandling = NullValueHandling.Ignore)]
        public string StartTime { get; set; }

        [JsonProperty("end_time", NullValueHandling = NullValueHandling.Ignore)]
        public string EndTime { get; set; }

        [JsonProperty("working_hour", NullValueHandling = NullValueHandling.Ignore)]
        public double? WorkingHour { get; set; }

        [JsonProperty("working_day", NullValueHandling = NullValueHandling.Ignore)]
        public string WorkingDay { get; set; }

        [JsonProperty("week_of_year", NullValueHandling = NullValueHandling.Ignore)]
        public int? WeekOfYear { get; set; }

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int BranchId { get; set; }

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public int UserId { get; set; }

        [JsonProperty("checkin_time", NullValueHandling = NullValueHandling.Ignore)]
        public object CheckinTime { get; set; }

        [JsonProperty("checkout_time", NullValueHandling = NullValueHandling.Ignore)]
        public object CheckoutTime { get; set; }

        [JsonProperty("is_confirm", NullValueHandling = NullValueHandling.Ignore)]
        public int? IsConfirm { get; set; }

        [JsonProperty("is_overtime_shift", NullValueHandling = NullValueHandling.Ignore)]
        public int? IsOvertimeShift { get; set; }

        [JsonProperty("shop_id", NullValueHandling = NullValueHandling.Ignore)]
        public int ShopId { get; set; }

        [JsonProperty("meal_coefficient", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MealCoefficient { get; set; }

        [JsonProperty("timezone", NullValueHandling = NullValueHandling.Ignore)]
        public string Timezone { get; set; }

        [JsonProperty("is_open_shift", NullValueHandling = NullValueHandling.Ignore)]
        public int? IsOpenShift { get; set; }

        [JsonProperty("dynamic_user_id", NullValueHandling = NullValueHandling.Ignore)]
        public object DynamicUserId { get; set; }

        [JsonProperty("checkin_type", NullValueHandling = NullValueHandling.Ignore)]
        public string CheckinType { get; set; }

        [JsonProperty("checkout_type", NullValueHandling = NullValueHandling.Ignore)]
        public string CheckoutType { get; set; }

        [JsonProperty("checkout_log_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? CheckoutLogId { get; set; }

        [JsonProperty("checkout_branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? CheckoutBranchId { get; set; }

        [JsonProperty("checkin_log_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? CheckinLogId { get; set; }

        [JsonProperty("checkin_branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? CheckinBranchId { get; set; }
    }

    public class TimekeeperLog
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public string Time { get; set; }

        [JsonProperty("clock_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ClockType { get; set; }

        [JsonProperty("employee_shift_id", NullValueHandling = NullValueHandling.Ignore)]
        public int PayrollUserID { get; set; }
    }

    public class ClockInOutShiftRequest
    {
        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? BranchId { get; set; }

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? UserId { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }

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
        public int? EmployeeShiftId { get; set; }

        [JsonProperty("clock_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ClockType { get; set; }

        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public float? Latitude { get; set; }

        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public float? Longitude { get; set; }
    }
} 