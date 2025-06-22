namespace DataAccess.Model.Shift
{
    public class Ins_Shift_Create_Parameter
    {
        public int CompanyID { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string NameNosign { get; set; } = string.Empty;
        public string ShiftKey { get; set; } = string.Empty;
        public int StartHourId { get; set; } = 9;
        public int StartMinuteId { get; set; } = 1;
        public int EndHourId { get; set; } = 18;
        public int EndMinuteId { get; set; } = 31;
        public decimal Coefficient { get; set; } = 1;
        public decimal MinimumWorkingHour { get; set; } = 0;
        public string Note { get; set; } = string.Empty;
        public int EarlyCheckOut { get; set; } = 0;
        public int LatelyCheckIn { get; set; } = 0;
        public int MaxLateCheckInOutMinute { get; set; } = 0;
        public int MinSoonCheckInOutMinute { get; set; } = 0;
        public int Status { get; set; } = 1;
        public string Type { get; set; }
        public int SortIndex { get; set; } = 0;
        public int IsOvertimeShift { get; set; } 
        public decimal MealCoefficient { get; set; } = 0;
        public string Timezone { get; set; } = "Asia/Bangkok";
        public int StartCheckInMinuteId { get; set; } = 1;
        public int EndCheckInMinuteId { get; set; } = 1;
        public int StartCheckOutMinuteId { get; set; } = 1;
        public int EndCheckOutMinuteId { get; set; } = 1;
        public int StartCheckInHourId { get; set; } = 1;
        public int EndCheckInHourId { get; set; } = 1;
        public int StartCheckOutHourId { get; set; } = 1;
        public int EndCheckOutHourId { get; set; } = 1;
        public int ShiftId { get; set; } = 0;
    }
} 