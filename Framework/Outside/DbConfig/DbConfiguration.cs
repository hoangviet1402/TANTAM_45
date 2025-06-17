using System.Reflection;
using DbConfig.Bo;
using DbConfig.Dao;

namespace DbConfig
{
    public class DbConfiguration
    {
        public static int Default
        {
            get
            {
                return DaoFactory.MyConfig.GetValue<int>(MethodBase.GetCurrentMethod().Name, 0);
            }
            set { DaoFactory.MyConfig.Save(MethodBase.GetCurrentMethod().Name, value); }
        }

        public static IMyConfigBo Config
        {
            get { return new MyConfigBo(); }
        }
    }
}
