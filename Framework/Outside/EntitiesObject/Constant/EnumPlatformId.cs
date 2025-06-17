/*
 * Author: TrungTT
 * Date: 2015-03-16
 * Description: PlatformId cho biet user dang ky tu platform nao - trong table Account
 */

using System.ComponentModel;

namespace EntitiesObject.Constant
{
    public enum EnumPlatformId
    {
        [Description("Không xác định")] Unknown = 0,

        [Description("Web - P1 Platform")] Web = 1,

        [Description("Mobile - HĐH Android")] Android = 2,

        [Description("Mobile - HĐH IOs")] Ios = 3,

        [Description("Mobile - HĐH Window phone")]
        WindowPhone = 4,

        [Description("Web - Facebook Platform")]
        Facebook = 5
    }
}