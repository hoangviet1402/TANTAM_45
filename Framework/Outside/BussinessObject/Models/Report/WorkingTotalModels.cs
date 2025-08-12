using Newtonsoft.Json;
using System.Collections.Generic;

namespace BussinessObject.Models.Report
{
    /// <summary>
    /// Request model for who-is-working-total API
    /// Company ID is extracted from JWT token automatically
    /// Working day is automatically set to current date
    /// </summary>
    public class WorkingTotalRequest
    {
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int? page { get; set; } // Optional: for detailed mode

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; } // Optional: "on_working", "on_time", "late_working", "not_working", "onleave"
    }

    /// <summary>
    /// Response model for who-is-working-total API following snake_case convention
    /// </summary>
    public class WorkingTotalResponse
    {
        [JsonProperty("on_working", NullValueHandling = NullValueHandling.Ignore)]
        public int on_working { get; set; } // Đã check in nhưng chưa check out

        [JsonProperty("late_working", NullValueHandling = NullValueHandling.Ignore)]
        public int late_working { get; set; } // Check in/out không đúng thời gian

        [JsonProperty("not_working", NullValueHandling = NullValueHandling.Ignore)]
        public int not_working { get; set; } // Ca đã đăng ký nhưng chưa check in/out

        [JsonProperty("onleave", NullValueHandling = NullValueHandling.Ignore)]
        public int onleave { get; set; } // Ca bị từ chối hoặc không active

        [JsonProperty("on_time", NullValueHandling = NullValueHandling.Ignore)]
        public int on_time { get; set; } // Đã check in và check out đúng thời gian

        [JsonProperty("share_location", NullValueHandling = NullValueHandling.Ignore)]
        public int share_location { get; set; } // Placeholder for location sharing feature

        public WorkingTotalResponse()
        {
            on_working = 0;
            late_working = 0;
            not_working = 0;
            onleave = 0;
            on_time = 0;
            share_location = 0;
        }
    }

    /// <summary>
    /// Detailed response model for who-is-working-total API with pagination
    /// </summary>
    public class WorkingDetailResponse
    {
        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<WorkingDetailItem> items { get; set; }

        [JsonProperty("meta", NullValueHandling = NullValueHandling.Ignore)]
        public WorkingDetailMeta meta { get; set; }

        public WorkingDetailResponse()
        {
            items = new List<WorkingDetailItem>();
            meta = new WorkingDetailMeta();
        }
    }

    /// <summary>
    /// Working detail item for detailed response
    /// </summary>
    public class WorkingDetailItem
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; } // Payroll_User.Id

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; } // Employee name

        [JsonProperty("checkin_time", NullValueHandling = NullValueHandling.Ignore)]
        public string checkin_time { get; set; } // Format: yyyy-MM-dd HH:mm:ss

        [JsonProperty("checkout_time", NullValueHandling = NullValueHandling.Ignore)]
        public string checkout_time { get; set; } // Format: yyyy-MM-dd HH:mm:ss

        [JsonProperty("employee", NullValueHandling = NullValueHandling.Ignore)]
        public WorkingDetailEmployee employee { get; set; }

        [JsonProperty("shift", NullValueHandling = NullValueHandling.Ignore)]
        public WorkingDetailShift shift { get; set; }

        [JsonProperty("start_time", NullValueHandling = NullValueHandling.Ignore)]
        public string start_time { get; set; } // Shift start time

        [JsonProperty("end_time", NullValueHandling = NullValueHandling.Ignore)]
        public string end_time { get; set; } // Shift end time

        [JsonProperty("request", NullValueHandling = NullValueHandling.Ignore)]
        public WorkingDetailRequest request { get; set; }

        [JsonProperty("workingday_config", NullValueHandling = NullValueHandling.Ignore)]
        public object workingday_config { get; set; } // Empty object for now

        [JsonProperty("branch_obj", NullValueHandling = NullValueHandling.Ignore)]
        public WorkingDetailBranch branch_obj { get; set; }

        public WorkingDetailItem()
        {
            employee = new WorkingDetailEmployee();
            shift = new WorkingDetailShift();
            request = new WorkingDetailRequest();
            workingday_config = new { };
            branch_obj = new WorkingDetailBranch();
        }
    }

    /// <summary>
    /// Employee info for working detail
    /// </summary>
    public class WorkingDetailEmployee
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; } // EmployeeAccountMap.Id

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; } // Employee full name
    }

    /// <summary>
    /// Shift info for working detail
    /// </summary>
    public class WorkingDetailShift
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; } // Shift.Id

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; } // Shift name
    }

    /// <summary>
    /// Request info for working detail (placeholder)
    /// </summary>
    public class WorkingDetailRequest
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; } = "";

        [JsonProperty("start_time", NullValueHandling = NullValueHandling.Ignore)]
        public string start_time { get; set; } = "";

        [JsonProperty("end_time", NullValueHandling = NullValueHandling.Ignore)]
        public string end_time { get; set; } = "";
    }

    /// <summary>
    /// Branch info for working detail
    /// </summary>
    public class WorkingDetailBranch
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; } // Branch.Id

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; } // Branch name
    }

    /// <summary>
    /// Pagination meta for working detail
    /// </summary>
    public class WorkingDetailMeta
    {
        [JsonProperty("total", NullValueHandling = NullValueHandling.Ignore)]
        public int total { get; set; } // Total items

        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int count { get; set; } // Items in current page

        [JsonProperty("per_page", NullValueHandling = NullValueHandling.Ignore)]
        public int per_page { get; set; } // Items per page

        [JsonProperty("current_page", NullValueHandling = NullValueHandling.Ignore)]
        public int current_page { get; set; } // Current page number

        [JsonProperty("total_pages", NullValueHandling = NullValueHandling.Ignore)]
        public int total_pages { get; set; } // Total pages

        public WorkingDetailMeta()
        {
            total = 0;
            count = 0;
            per_page = 15;
            current_page = 1;
            total_pages = 0;
        }
    }
} 