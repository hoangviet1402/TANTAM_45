
namespace DataAccess.Model.Shift
{
    public class Ins_Shift_Branch_Create_Parameter
    {
        public int ShiftID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public bool IsInsertOne { get; set; }
    }
}
