/*
 * Author: TrungTT
 * Date: 2015-03-17
 * Description: config key enum dung cho DbConfig
 */

using System.ComponentModel;

namespace DbConfig.Enums
{
    public enum ConfigKeyEnum
    {
        /// <summary>
        /// <para>Author: TrungTT</para>
        /// <para>Date: 2015-04-13</para>
        /// <para>Description: Anh hung bang hoi</para>
        /// </summary>
        [Description("AnhHungBangHoiConfig")]
        AnhHungBangHoi = 1,

        /// <summary>
        /// <para>Author: TrungTT</para>
        /// <para>Date: 2015-04-13</para>
        /// <para>Description: Bao tri nap the cao</para>
        /// </summary>
        [Description("MaintainDeposit")]
        MaintainDeposit = 2,

        [Description("LoginRedirect")]
        LoginRedirect=3,
    }
}
