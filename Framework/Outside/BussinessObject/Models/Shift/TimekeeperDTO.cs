using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace BussinessObject.Models.Shift
{
    public class ListTimekeeperLogRequest
    {
        [JsonProperty("from_date", NullValueHandling = NullValueHandling.Ignore)]
        public string from_date { get; set; }
        [JsonProperty("to_date", NullValueHandling = NullValueHandling.Ignore)]
        public string to_date { get; set; }
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int page { get; set; }
    }
    public class ListTimekeeperLogRequestV2
    {
        [JsonProperty("employee_shift_id", NullValueHandling = NullValueHandling.Ignore)]
        public int employee_shift_id { get; set; }

        [JsonProperty("employee_id", NullValueHandling = NullValueHandling.Ignore)]
        public int employee_id { get; set; }
    }
    public class StatusClockInOutShiftRequest
    {
        [JsonProperty("timekeeper_device", NullValueHandling = NullValueHandling.Ignore)]
        public string timekeeper_device { get; set; }
        [JsonProperty("is_show_button", NullValueHandling = NullValueHandling.Ignore)]
        public int? is_show_button { get; set; }
        [JsonProperty("isInitial", NullValueHandling = NullValueHandling.Ignore)]
        public bool? isInitial { get; set; }
    }
}
