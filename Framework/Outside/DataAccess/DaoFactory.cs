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

using DataAccess.Dao.TanTamDao;

namespace DataAccess
{
    public class DaoFactory
    {
        public static ITanTamDao TanTam => new TanTamDao();
        public static IAuthDao Auth => new AuthDao();
    }
}