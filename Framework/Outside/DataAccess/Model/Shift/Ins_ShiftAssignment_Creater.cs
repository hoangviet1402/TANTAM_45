
namespace DataAccess.Model.Shift
{
    public class Ins_ShiftAssignment_Create_Parameter
    {
        public int CompanyID { get; set; }
        public int ShiftID { get; set; }
        public string Title { get; set; } = string.Empty;
        public int SortIndex { get; set; }
        public int AutoApprove { get; set; } = 1;
        public string PayrollConfigType { get; set; } = string.Empty;
        public string AssignmentType { get; set; } = "weekly_loop";
        public string GenerateTimekeepingType { get; set; } = "generate_from_start_of_month";
        public string Type { get; set; } = "shift_assignment";
    }
}