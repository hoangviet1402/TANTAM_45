using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models.Report
{
    public class EmployeePayrollReportResponse
    {
        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public EmployeePayrollReportInfo Info { get; set; }
    }

    public class EmployeePayrollReportInfo
    {
        [JsonProperty("work_day", NullValueHandling = NullValueHandling.Ignore)]
        public WorkDay WorkDay { get; set; }

        [JsonProperty("work_hour", NullValueHandling = NullValueHandling.Ignore)]
        public WorkHour WorkHour { get; set; }

        [JsonProperty("other_field", NullValueHandling = NullValueHandling.Ignore)]
        public OtherField OtherField { get; set; }
    }

    public class WorkDay
    {
        [JsonProperty("NGAYCONG_THUCTE", NullValueHandling = NullValueHandling.Ignore)]
        public Item NGAYCONG_THUCTE { get; set; }

        [JsonProperty("GIOCONG_THUCTE", NullValueHandling = NullValueHandling.Ignore)]
        public Item GIOCONG_THUCTE { get; set; }

        [JsonProperty("SOGIO_LAMDUGIO", NullValueHandling = NullValueHandling.Ignore)]
        public Item SOGIO_LAMDUGIO { get; set; }

        [JsonProperty("SOGIO_LAMTHEM", NullValueHandling = NullValueHandling.Ignore)]
        public Item SOGIO_LAMTHEM { get; set; }

        [JsonProperty("SOPHUT_DILAMSOM", NullValueHandling = NullValueHandling.Ignore)]
        public Item SOPHUT_DILAMSOM { get; set; }

        [JsonProperty("GIOCONG_TIEUCHUAN", NullValueHandling = NullValueHandling.Ignore)]
        public Item GIOCONG_TIEUCHUAN { get; set; }

        [JsonProperty("SONGAYNGHI_TIEUCHUAN", NullValueHandling = NullValueHandling.Ignore)]
        public Item SONGAYNGHI_TIEUCHUAN { get; set; }

        [JsonProperty("SONGAYNGHI_KHONGLUONG", NullValueHandling = NullValueHandling.Ignore)]
        public Item SONGAYNGHI_KHONGLUONG { get; set; }

        [JsonProperty("CONG_CHUAN", NullValueHandling = NullValueHandling.Ignore)]
        public Item CONG_CHUAN { get; set; }

        [JsonProperty("SONGAYNGHI_LE", NullValueHandling = NullValueHandling.Ignore)]
        public Item SONGAYNGHI_LE { get; set; }

        [JsonProperty("payable_coefficient", NullValueHandling = NullValueHandling.Ignore)]
        public Item PayableCoefficient { get; set; }
    }

    public class WorkHour
    {
        [JsonProperty("SOGIO_VESOM", NullValueHandling = NullValueHandling.Ignore)]
        public Item SOGIO_VESOM { get; set; }

        [JsonProperty("SOGIO_DIMUON", NullValueHandling = NullValueHandling.Ignore)]
        public Item SOGIO_DIMUON { get; set; }

        [JsonProperty("SOGIO_DIMUON_VESOM", NullValueHandling = NullValueHandling.Ignore)]
        public Item SOGIO_DIMUON_VESOM { get; set; }
    }

    public class OtherField
    {
        [JsonProperty("SOLAN_QUENCHECKIN", NullValueHandling = NullValueHandling.Ignore)]
        public Item SOLAN_QUENCHECKIN { get; set; }

        [JsonProperty("SOLAN_QUENCHECKOUT", NullValueHandling = NullValueHandling.Ignore)]
        public Item SOLAN_QUENCHECKOUT { get; set; }

        [JsonProperty("SOLAN_QUENCHECKINOUT", NullValueHandling = NullValueHandling.Ignore)]
        public Item SOLAN_QUENCHECKINOUT { get; set; }
    }

    public class Item
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public object Value { get; set; }

        [JsonProperty("is_detail", NullValueHandling = NullValueHandling.Ignore)]
        public int? IsDetail { get; set; }
    }


    #region detail 
    public class EmployeePayrollReportDetailResponse
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public List<PayrollReportDetail_ShiftData> Data { get; set; }
    }
    public class PayrollReportDetail_ShiftStatus
    {
        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        [JsonProperty("status_color", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> StatusColor { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("detail", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Detail { get; set; }
    }

    public class PayrollReportDetail_ShiftData
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("shift_id", NullValueHandling = NullValueHandling.Ignore)]
        public int ShiftId { get; set; }

        [JsonProperty("timezone", NullValueHandling = NullValueHandling.Ignore)]
        public string Timezone { get; set; }

        [JsonProperty("employee_shift_id", NullValueHandling = NullValueHandling.Ignore)]
        public int EmployeeShiftId { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public PayrollReportDetail_ShiftStatus Status { get; set; }

        [JsonProperty("checkin_time", NullValueHandling = NullValueHandling.Ignore)]
        public string CheckInTime { get; set; }

        [JsonProperty("checkout_time", NullValueHandling = NullValueHandling.Ignore)]
        public string CheckOutTime { get; set; }

        [JsonProperty("approved", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Approved { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }
    }

    
    #endregion 
}
