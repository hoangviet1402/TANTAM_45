/*
 * Author: TrungTT
 * Date: 2015-03-18
 * Description: MyConfigBo
 */
using DbConfig.Dao;

namespace DbConfig.Bo
{
    public interface IMyConfigBo
    {
        /// <summary>
        /// <para>Author: TrungTT</para>
        /// <para>Date: 2015-03-18</para>
        /// <para>Lay gia tri config trong MyConfig</para>
        /// </summary>
        /// <returns></returns>
        string GetConfig(string configKey);
    }

    internal class MyConfigBo : IMyConfigBo
    {
        /// <summary>
        /// <para>Author: TrungTT</para>
        /// <para>Date: 2015-03-18</para>
        /// <para>Lay gia tri config trong MyConfig</para>
        /// </summary>
        /// <returns></returns>
        public string GetConfig(string configKey)
        {
            return DaoFactory.MyConfig.GetValue<string>(configKey);
        }
    }
}
