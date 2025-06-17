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

using DbConfig.EF;

namespace DbConfig.Dao
{
    internal class DaoFactory
    {
        public static IMyConfigDao MyConfig { get { return new MyConfigDao(); } }
    }
}
