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

using DataAccess.Dao.TanTam;

namespace DataAccess
{
    public class DaoFactory
    {
       
        public static IAuthDao Auth => new AuthDao();
        public static ICompanyDao Company => new CompanyDao();
        public static IDepartmentDao Department => new DepartmentDao();
        public static IBranchesDao Branches => new BranchesDao();
    }
}