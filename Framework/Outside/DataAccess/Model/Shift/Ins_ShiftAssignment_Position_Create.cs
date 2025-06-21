
namespace DataAccess.Model.Shift
{
    public class Ins_ShiftAssignment_Position_Create_Parameter
    {
        public int ShiftAssignmentID { get; set; }
        public int CompanyID { get; set; }
        public int PositionID { get; set; }
        public bool IsInsertOne { get; set; }
    }
}
