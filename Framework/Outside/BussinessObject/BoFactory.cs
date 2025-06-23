/*********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: BoFactory
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using BussinessObject.Bo.Shift;
using BussinessObject.Bo.TanTamBo;

namespace BussinessObject
{
    public class BoFactory
    {
        public static AuthBo Auth => new AuthBo();
        public static TaskBo Task => new TaskBo();
        public static UserBo User => new UserBo();
        public static EmployeeBo Employee => new EmployeeBo();
        public static CompanyBo Company => new CompanyBo();
        public static BranchesBo Branches => new BranchesBo();
        public static DepartmentBo Department => new DepartmentBo();
        public static PositionBo Position => new PositionBo();
        public static ShiftBo Shift => new ShiftBo();
        public static PayrollBo Payroll => new PayrollBo();
        public static ShiftAssignmentBo ShiftAssignment => new ShiftAssignmentBo();
    }
}