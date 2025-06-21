
namespace DataAccess.Model.Shift
{
    public class Ins_ShiftAssignment_CreateAssignment_Parameter
    {
        public int ShiftAssignmentID { get; set; }
        public int ShiftID { get; set; }
        public string Label { get; set; } = string.Empty;
        public int DateOfWeek { get; set; }
    }
}