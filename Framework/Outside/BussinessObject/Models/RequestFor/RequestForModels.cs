using Newtonsoft.Json;
using System.Collections.Generic;

namespace BussinessObject.Models.RequestFor
{
    #region  RequestForRequest
    public class RequestForRequest
    {
        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public string UserId { get; set; }

        [JsonProperty("handover_to", NullValueHandling = NullValueHandling.Ignore)]
        public string HandoverTo { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int Status { get; set; }

        [JsonProperty("tel", NullValueHandling = NullValueHandling.Ignore)]
        public string Tel { get; set; }

        [JsonProperty("exchange_content", NullValueHandling = NullValueHandling.Ignore)]
        public string ExchangeContent { get; set; }

        [JsonProperty("from_date", NullValueHandling = NullValueHandling.Ignore)]
        public string FromDate { get; set; }

        [JsonProperty("to_date", NullValueHandling = NullValueHandling.Ignore)]
        public string ToDate { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public int Type { get; set; }

        [JsonProperty("type_id", NullValueHandling = NullValueHandling.Ignore)]
        public int TypeId { get; set; }

        [JsonProperty("reason", NullValueHandling = NullValueHandling.Ignore)]
        public string Reason { get; set; }

        [JsonProperty("workingday_config_id", NullValueHandling = NullValueHandling.Ignore)]
        public string WorkingdayConfigId { get; set; }

        [JsonProperty("employee_dayleft_id", NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeDayleftId { get; set; }

        [JsonProperty("is_next_start_time", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsNextStartTime { get; set; }

        [JsonProperty("is_next_end_time", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsNextEndTime { get; set; }

        [JsonProperty("shift_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> ShiftIds { get; set; }
    }

    public class RequestForResponse
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public int UserId { get; set; }

        [JsonProperty("from_date", NullValueHandling = NullValueHandling.Ignore)]
        public string FromDate { get; set; }

        [JsonProperty("to_date", NullValueHandling = NullValueHandling.Ignore)]
        public string ToDate { get; set; }

        [JsonProperty("status_id", NullValueHandling = NullValueHandling.Ignore)]
        public int StatusId { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public int Type { get; set; }

        [JsonProperty("total_day", NullValueHandling = NullValueHandling.Ignore)]
        public decimal TotalDay { get; set; }

        [JsonProperty("confirm_user", NullValueHandling = NullValueHandling.Ignore)]
        public int ConfirmUser { get; set; }

        [JsonProperty("confirm_date", NullValueHandling = NullValueHandling.Ignore)]
        public string ConfirmDate { get; set; }

        [JsonProperty("handover_to", NullValueHandling = NullValueHandling.Ignore)]
        public string HandoverTo { get; set; }

        [JsonProperty("reason", NullValueHandling = NullValueHandling.Ignore)]
        public string Reason { get; set; }

        [JsonProperty("tel", NullValueHandling = NullValueHandling.Ignore)]
        public string Tel { get; set; }

        [JsonProperty("leave_wage_by_leave_coefficient", NullValueHandling = NullValueHandling.Ignore)]
        public decimal LeaveWageByLeaveCoefficient { get; set; }

        [JsonProperty("coefficient_by_shift", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, decimal> CoefficientByShift { get; set; }

        [JsonProperty("exchange_content", NullValueHandling = NullValueHandling.Ignore)]
        public string ExchangeContent { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }

        [JsonProperty("shift_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> ShiftIds { get; set; }

        [JsonProperty("start_time", NullValueHandling = NullValueHandling.Ignore)]
        public string StartTime { get; set; }

        [JsonProperty("end_time", NullValueHandling = NullValueHandling.Ignore)]
        public string EndTime { get; set; }

        [JsonProperty("shifts", NullValueHandling = NullValueHandling.Ignore)]
        public List<RequestForResponse_Shift> Shifts { get; set; }

        [JsonProperty("workingday_config", NullValueHandling = NullValueHandling.Ignore)]
        public RequestForResponse_WorkingdayConfig WorkingdayConfig { get; set; }

        [JsonProperty("type_obj", NullValueHandling = NullValueHandling.Ignore)]
        public RequestForResponse_TypeObj TypeObj { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public RequestForResponse_StatusObj Status { get; set; }
    }

    public class RequestForResponse_Shift
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class RequestForResponse_WorkingdayConfig
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty("type_onleave", NullValueHandling = NullValueHandling.Ignore)]
        public int TypeOnleave { get; set; }

        [JsonProperty("symbol", NullValueHandling = NullValueHandling.Ignore)]
        public string Symbol { get; set; }
    }

    public class RequestForResponse_TypeObj
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public int Value { get; set; }

        [JsonProperty("number_day", NullValueHandling = NullValueHandling.Ignore)]
        public decimal NumberDay { get; set; }
    }

    public class RequestForResponse_StatusObj
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public int Value { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("api", NullValueHandling = NullValueHandling.Ignore)]
        public string Api { get; set; }

        [JsonProperty("index_num", NullValueHandling = NullValueHandling.Ignore)]
        public int? IndexNum { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        [JsonProperty("keyIndex", NullValueHandling = NullValueHandling.Ignore)]
        public string KeyIndex { get; set; }

        [JsonProperty("select_type", NullValueHandling = NullValueHandling.Ignore)]
        public string SelectType { get; set; }

        [JsonProperty("dropDownData", NullValueHandling = NullValueHandling.Ignore)]
        public object DropDownData { get; set; }

        [JsonProperty("optionList", NullValueHandling = NullValueHandling.Ignore)]
        public object OptionList { get; set; }

        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public object Children { get; set; }

        [JsonProperty("titleIndex", NullValueHandling = NullValueHandling.Ignore)]
        public string TitleIndex { get; set; }

        [JsonProperty("is_default", NullValueHandling = NullValueHandling.Ignore)]
        public int IsDefault { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
    }
    #endregion

    #region RequestTypeResponse
    public class RequestTypeResponse
    {
        [JsonProperty("meta", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Meta { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<RequestTypeResponse_Items> Items { get; set; }
   
    }

    public class RequestTypeResponse_Items
    {
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("setting", NullValueHandling = NullValueHandling.Ignore)]
        public RequestTypeResponse_Items_Setting Setting { get; set; }
    }

    public class RequestTypeResponse_Items_Setting
    {
        [JsonProperty("alias", NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }

        [JsonProperty("serial_prefix", NullValueHandling = NullValueHandling.Ignore)]
        public string SerialPrefix { get; set; }

        [JsonProperty("serial_suffix", NullValueHandling = NullValueHandling.Ignore)]
        public string SerialSuffix { get; set; }

        [JsonProperty("is_disabled_for_employees", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsDisabledForEmployees { get; set; }

        [JsonProperty("is_disabled_for_manager", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsDisabledForManager { get; set; }

        [JsonProperty("enable_employee_delete_request", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EnableEmployeeDeleteRequest { get; set; }

        [JsonProperty("allow_request_when_shift_still_working", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowRequestWhenShiftStillWorking { get; set; }

        [JsonProperty("minimum_day", NullValueHandling = NullValueHandling.Ignore)]
        public int? MinimumDay { get; set; }

        [JsonProperty("is_shortcut", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsShortcut { get; set; }

        [JsonProperty("enable_auto_approval", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EnableAutoApproval { get; set; }

        [JsonProperty("day_required_before_send_request", NullValueHandling = NullValueHandling.Ignore)]
        public int? DayRequiredBeforeSendRequest { get; set; }
    }
    #endregion
}