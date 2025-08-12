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
        public static PermissionBo Permission => new PermissionBo();
        public static MenuBo Menu => new MenuBo();
        public static DepartmentBo Department => new DepartmentBo();
        public static PositionBo Position => new PositionBo();
        public static ShiftBo Shift => new ShiftBo();
        public static ShiftSummaryBo ShiftSummary => new ShiftSummaryBo();
        public static PayrollBo Payroll => new PayrollBo();
        public static ShiftAssignmentBo ShiftAssignment => new ShiftAssignmentBo();
        public static OpenShiftBo OpenShift => new OpenShiftBo();
        public static ReportBo Report => new ReportBo();
        public static TimekeeperBo Timekeeper => new TimekeeperBo();
        public static WifiBo Wifi => new WifiBo();
        public static CommentBo Comment => new CommentBo();
        public static TutorialsBo Tutorials => new TutorialsBo();
        public static RequestForBo RequestFor => new RequestForBo();
        public static BuildingBo Building => new BuildingBo();
    }
}