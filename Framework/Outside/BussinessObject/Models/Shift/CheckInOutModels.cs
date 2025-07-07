using Newtonsoft.Json;

namespace BussinessObject.Models.Shift
{
    /// <summary>
    /// Request model for check-in/out shift API with exact format specified by user
    /// </summary>
    public class CheckInOutShiftUpdateRequest
    {
        [JsonProperty("reason", NullValueHandling = NullValueHandling.Ignore)]
        public string Reason { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; } // User working day ID (ShiftAssignment_User_WorkingDay.Id)

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public string BranchId { get; set; }

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public string UserId { get; set; }

        [JsonProperty("checkin_time", NullValueHandling = NullValueHandling.Ignore)]
        public string CheckinTime { get; set; } // Format: yyyy-MM-dd HH:mm:ss

        [JsonProperty("checkout_time", NullValueHandling = NullValueHandling.Ignore)]
        public string CheckoutTime { get; set; } // Format: yyyy-MM-dd HH:mm:ss

        [JsonProperty("is_checkin", NullValueHandling = NullValueHandling.Ignore)]
        public int IsCheckin { get; set; } // 1 = update checkin, 0 = don't update

        [JsonProperty("is_checkout", NullValueHandling = NullValueHandling.Ignore)]
        public int IsCheckout { get; set; } // 1 = update checkout, 0 = don't update

        [JsonProperty("working_day", NullValueHandling = NullValueHandling.Ignore)]
        public string WorkingDay { get; set; } // Format: yyyy-MM-dd HH:mm:ss
    }

    /// <summary>
    /// Response model for check-in/out shift API using snake_case convention
    /// </summary>
    public class CheckInOutShiftUpdateResponse
    {
        [JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
        public int success { get; set; }

        [JsonProperty("suw_id", NullValueHandling = NullValueHandling.Ignore)]
        public int suw_id { get; set; }

        [JsonProperty("working_day", NullValueHandling = NullValueHandling.Ignore)]
        public string working_day { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string message { get; set; }

        [JsonProperty("is_check_in", NullValueHandling = NullValueHandling.Ignore)]
        public bool is_check_in { get; set; }

        [JsonProperty("is_check_out", NullValueHandling = NullValueHandling.Ignore)]
        public bool is_check_out { get; set; }

        [JsonProperty("start_check_in_time", NullValueHandling = NullValueHandling.Ignore)]
        public string start_check_in_time { get; set; }

        [JsonProperty("start_check_out_time", NullValueHandling = NullValueHandling.Ignore)]
        public string start_check_out_time { get; set; }
    }

    /// <summary>
    /// Request model for uncheckin/uncheckout shift API with exact format specified by user
    /// </summary>
    public class UncheckInOutShiftRequest
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; } // User working day ID (ShiftAssignment_User_WorkingDay.Id)

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public string BranchId { get; set; }

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public string UserId { get; set; }

        [JsonProperty("is_uncheckin", NullValueHandling = NullValueHandling.Ignore)]
        public int IsUncheckin { get; set; } // 1 = cancel checkin, 0 = don't cancel

        [JsonProperty("is_uncheckout", NullValueHandling = NullValueHandling.Ignore)]
        public int IsUncheckout { get; set; } // 1 = cancel checkout, 0 = don't cancel (optional, defaults to 0)

        [JsonProperty("reason", NullValueHandling = NullValueHandling.Ignore)]
        public string Reason { get; set; } // Optional reason for unchecking
    }

    /// <summary>
    /// Response model for uncheckin/uncheckout shift API using snake_case convention
    /// </summary>
    public class UncheckInOutShiftResponse
    {
        [JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
        public int success { get; set; }

        [JsonProperty("suw_id", NullValueHandling = NullValueHandling.Ignore)]
        public int suw_id { get; set; }

        [JsonProperty("working_day", NullValueHandling = NullValueHandling.Ignore)]
        public string working_day { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string message { get; set; }

        [JsonProperty("is_check_in", NullValueHandling = NullValueHandling.Ignore)]
        public bool is_check_in { get; set; }

        [JsonProperty("is_check_out", NullValueHandling = NullValueHandling.Ignore)]
        public bool is_check_out { get; set; }

        [JsonProperty("start_check_in_time", NullValueHandling = NullValueHandling.Ignore)]
        public string start_check_in_time { get; set; }

        [JsonProperty("start_check_out_time", NullValueHandling = NullValueHandling.Ignore)]
        public string start_check_out_time { get; set; }

        [JsonProperty("reason", NullValueHandling = NullValueHandling.Ignore)]
        public string reason { get; set; }
    }
} 