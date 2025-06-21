
namespace DataAccess.Model.Shift
{
    public class Ins_ShiftAssignment_Department_Create_Parameter
    {
        public int ShiftAssignmentID { get; set; }
        public int CompanyID { get; set; }
        public int DepartmentID { get; set; }
        public bool IsInsertOne { get; set; }
    }
}
