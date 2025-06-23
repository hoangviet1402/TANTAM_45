
using System;

namespace DataAccess.Model.Shift
{
    public class Payroll_User_CreateMultiDayParameter
    {
        public int AssignmentUserID { get; set; }
        public int AccountMapID { get; set; }     
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? WeekOfYear { get; set; }
        public string CheckinType { get; set; }
        public string CheckouType { get; set; }
        public double? RealWorkingHour { get; set; }
        public double? RealWorkingMinute { get; set; }
        public string RestStartTimeShort { get; set; }
        public string RestEndTimeShort { get; set; }
        public double? RealCoefficient { get; set; }
        public int? Status { get; set; }
    }
}
