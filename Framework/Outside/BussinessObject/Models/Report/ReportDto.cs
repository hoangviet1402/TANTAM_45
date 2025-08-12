using Newtonsoft.Json;
using System.Collections.Generic;

namespace BussinessObject.Models.Report
{
    /// <summary>
    /// Request model for Dashboard Employees Growth API
    /// </summary>
    public class DashboardEmployeesGrowthRequest
    {
        [JsonProperty("company_id", NullValueHandling = NullValueHandling.Ignore)]
        public int CompanyId { get; set; }

        [JsonProperty("days_ago", NullValueHandling = NullValueHandling.Ignore)]
        public int DaysAgo { get; set; } = 30;
    }

    /// <summary>
    /// Response model for Dashboard Employees Growth API
    /// </summary>
    public class DashboardEmployeesGrowthResponse
    {
        [JsonProperty("current_employees_count", NullValueHandling = NullValueHandling.Ignore)]
        public int current_employees_count { get; set; }

        [JsonProperty("old_employees_count", NullValueHandling = NullValueHandling.Ignore)]
        public int old_employees_count { get; set; }

        [JsonProperty("diff_employees_count", NullValueHandling = NullValueHandling.Ignore)]
        public int diff_employees_count { get; set; }

        [JsonProperty("diff_employees_percent", NullValueHandling = NullValueHandling.Ignore)]
        public decimal diff_employees_percent { get; set; }
    }

    /// <summary>
    /// Request model for Dashboard Working Time Statistics API  
    /// Supports time periods: 1, 3, 7, 30, 90 days
    /// Values will be auto-normalized to nearest supported period
    /// </summary>
    public class DashboardWorkingTimeRequest
    {
        [JsonProperty("company_id", NullValueHandling = NullValueHandling.Ignore)]
        public int CompanyId { get; set; }

        [JsonProperty("days_ago", NullValueHandling = NullValueHandling.Ignore)]
        public int DaysAgo { get; set; } = 7;

        /// <summary>
        /// Validate and normalize days_ago to supported values
        /// Supported values: 1, 3, 7, 30, 90
        /// </summary>
        public bool IsValidPeriod()
        {
            // Normalize to nearest supported value
            if (DaysAgo <= 0) DaysAgo = 7;
            else if (DaysAgo <= 1) DaysAgo = 1;
            else if (DaysAgo <= 3) DaysAgo = 3;
            else if (DaysAgo <= 7) DaysAgo = 7;
            else if (DaysAgo <= 30) DaysAgo = 30;
            else DaysAgo = 90;
            
            return true;
        }
    }

    /// <summary>
    /// Response model for Dashboard Working Time Statistics API
    /// Returns percentage statistics for different working time categories
    /// Categories:
    /// - no_timekeeping: Chưa chấm công - Hoàn toàn không có dữ liệu check in và check out (%) 
    /// - no_clock_in_or_out: Thiếu chấm công - Chỉ có 1 trong 2 (hoặc check in hoặc check out) (%)
    /// - good_timekeeping: Chấm công tốt - Có đủ cả check in và check out đúng giờ (%)
    /// - in_late_out_soon: Trễ giờ công - Check in muộn hoặc check out sớm (%)
    /// </summary>
    public class DashboardWorkingTimeResponse
    {
        [JsonProperty("no_timekeeping", NullValueHandling = NullValueHandling.Ignore)]
        public decimal no_timekeeping { get; set; }

        [JsonProperty("no_clock_in_or_out", NullValueHandling = NullValueHandling.Ignore)]
        public decimal no_clock_in_or_out { get; set; }

        [JsonProperty("good_timekeeping", NullValueHandling = NullValueHandling.Ignore)]
        public decimal good_timekeeping { get; set; }

        [JsonProperty("in_late_out_soon", NullValueHandling = NullValueHandling.Ignore)]
        public decimal in_late_out_soon { get; set; }
    }

    /// <summary>
    /// Request model for Dashboard Devices Statistics API
    /// Company ID is automatically extracted from JWT token
    /// Working day is automatically set to current date
    /// </summary>
    public class DashboardDevicesRequest
    {
        [JsonProperty("company_id", NullValueHandling = NullValueHandling.Ignore)]
        public int CompanyId { get; set; }

        [JsonProperty("working_day", NullValueHandling = NullValueHandling.Ignore)]
        public System.DateTime? WorkingDay { get; set; }
    }

    /// <summary>
    /// Response model for Dashboard Devices Statistics API
    /// Returns statistics about employees who have checked in or out
    /// Following the requested format: {"admin": {"percent": 66.7, "count": 2}}
    /// </summary>
    public class DashboardDevicesResponse
    {
        [JsonProperty("admin", NullValueHandling = NullValueHandling.Ignore)]
        public DashboardDevicesAdmin admin { get; set; }
    }

    /// <summary>
    /// Admin statistics for dashboard devices
    /// </summary>
    public class DashboardDevicesAdmin
    {
        [JsonProperty("percent", NullValueHandling = NullValueHandling.Ignore)]
        public decimal percent { get; set; }

        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int count { get; set; }
    }

    #region report chấm công
    public class ClockTabEmployeeClockResponse
    {
        [JsonProperty("data1", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<long>> EmployeeAccountMap { get; set; }

        [JsonProperty("data2", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<long>> Payroll { get; set; }

        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public ClockTabEmployeeClock_Info info { get; set; }
    }

    public class ClockTabEmployeeClock_Info
    {
        [JsonProperty("data1", NullValueHandling = NullValueHandling.Ignore)]
        public string data1 { get; set; }

        [JsonProperty("data2", NullValueHandling = NullValueHandling.Ignore)]
        public string data2 { get; set; }
    }
    #endregion

    #region report chấm công trễ / về sớm
    public class ClockTabClockLateSoonResponse
    {
        [JsonProperty("data1", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<long>> EmployeeAccountMap { get; set; }

        [JsonProperty("data2", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<long>> Soon { get; set; }

        [JsonProperty("data3", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<long>> Late { get; set; }

        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public ClockTabClockLateSoon_Info Info { get; set; }
    }

    public class ClockTabClockLateSoon_Info
    {
        [JsonProperty("data1", NullValueHandling = NullValueHandling.Ignore)]
        public string data1 { get; set; }

        [JsonProperty("data2", NullValueHandling = NullValueHandling.Ignore)]
        public string data2 { get; set; }

        [JsonProperty("data3", NullValueHandling = NullValueHandling.Ignore)]
        public string data3 { get; set; }
    }
    #endregion

    #region report không chấm công
    public class ClockTabNotClockInOutResponse
    {
        [JsonProperty("data1", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<long>> EmployeeAccountMap { get; set; }

        [JsonProperty("data2", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<long>> NotCheckIn { get; set; }

        [JsonProperty("data3", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<long>> NotCheckOut { get; set; }

        [JsonProperty("data4", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<long>> NotCheckInAndOut { get; set; }

        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public ClockTabNotClockInOut_Info Info { get; set; }
    }

    public class ClockTabNotClockInOut_Info
    {
        [JsonProperty("data1", NullValueHandling = NullValueHandling.Ignore)]
        public string data1 { get; set; }

        [JsonProperty("data2", NullValueHandling = NullValueHandling.Ignore)]
        public string data2 { get; set; }

        [JsonProperty("data3", NullValueHandling = NullValueHandling.Ignore)]
        public string data3 { get; set; }

        [JsonProperty("data4", NullValueHandling = NullValueHandling.Ignore)]
        public string data4 { get; set; }
    }
    #endregion


    #region report chấm công
    public class TimesheetTabWorkingHoursResponse
    {

        [JsonProperty("data2", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<double>> GIOCONG_THUCTE { get; set; }

        [JsonProperty("data1", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<double>> GIOCONG_TIEUCHUAN { get; set; }

        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public TimesheetTabWorkingHours_Info Info { get; set; }
    }

    public class TimesheetTabWorkingHours_Info
    {
        [JsonProperty("data1", NullValueHandling = NullValueHandling.Ignore)]
        public string data1 { get; set; }

        [JsonProperty("data2", NullValueHandling = NullValueHandling.Ignore)]
        public string data2 { get; set; }
    }

    public class TimesheetTabWorkingDayResponse
    {
        [JsonProperty("data2", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<decimal>> NGAYCONG_THUCTE { get; set; }

        [JsonProperty("data1", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<decimal>> NGAYCONG_TIEUCHUAN { get; set; }

        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public TimesheetTabWorkingDay_Info Info { get; set; }
    }

    public class TimesheetTabWorkingDay_Info
    {
        [JsonProperty("data1", NullValueHandling = NullValueHandling.Ignore)]
        public string data1 { get; set; }

        [JsonProperty("data2", NullValueHandling = NullValueHandling.Ignore)]
        public string data2 { get; set; }
    }
    #endregion
}