using System.ComponentModel;

namespace BussinessObject.Enum.Info
{
    /// <summary>
    ///     Định nghĩa OpenProvider Id
    ///     <para>Author: PhatVT</para>
    ///     <para>Create Date: 23/12/2014</para>
    /// </summary>
    public enum OpenProviderIdEnum
    {
        [Description("Web")] Web = 0,

        [Description("Yahoo")] Yahoo = 1,

        [Description("Google")] Google = 2,

        [Description("Facebook")] Facebook = 3,

        [Description("Mobile")] Mobile = 4,

        [Description("IDS")] IDS = 5,

        [Description("Apple")] Apple = 6,

        [Description("TikTok")] TikTok = 7
    }
}