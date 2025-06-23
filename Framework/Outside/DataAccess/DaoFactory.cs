/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: DaoFactory
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using DataAccess.Dao.Shift;
using DataAccess.Dao.TanTam;
using DataAccess.Dao.TanTamDao;

namespace DataAccess
{
    public class DaoFactory
    {
        public static ITanTamDao TanTam => new TanTamDao();
        public static IAuthDao Auth => new AuthDao();
        public static IUserDao User => new UserDao();
        public static IEmployeeDao Employee => new EmployeeDao();
        public static ITaskDao Task => new TaskDao();

        public static ICompanyDao Company => new CompanyDao();
        public static IDepartmentDao Department => new DepartmentDao();
        public static IBranchesDao Branches => new BranchesDao();
        public static IPositionDao Position => new PositionDao();
        public static IShiftDao Shift => new ShiftDao();
        public static IPayrollDao Payroll => new PayrollDao();
        public static IShiftAssignmentDao ShiftAssignment => new ShiftAssignmentDao();
    }
}