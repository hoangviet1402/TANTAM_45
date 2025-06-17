using System.ComponentModel;

namespace BussinessObject.Enum.Info
{
    /// <summary>
    ///     Định nghĩa Referer Id
    ///     <para>Author: PhatVT</para>
    ///     <para>Create Date: 23/12/2014</para>
    /// </summary>
    public enum RefererIdEnum
    {
        [Description("Web")] Web = 0,

        [Description("Yahoo")] Yahoo = 1,

        [Description("Google")] Google = 2,

        [Description("Facebook")] Facebook = 3,

        [Description("Bing")] Bing = 4,

        [Description("GoogleAdword")] GoogleAdword = 5
    }
}