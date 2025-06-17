using System.ComponentModel;

namespace DataAccessRedis.Model
{
    public enum RankingBoxEnum
    {
        /// <summary>
        ///     Top dong gop cho bang
        /// </summary>
        [Description("AssMemberTopContribute")]
        AssMemberTopContribute = 1,

        /// <summary>
        ///     Top DKN thang
        /// </summary>
        [Description("AssMemberTopExpMonth")] AssMemberTopExpMonth = 2,

        /// <summary>
        ///     Top dong gop thang
        /// </summary>
        [Description("AssMemberTopContributeMonth")]
        AssMemberTopContributeMonth = 3,

        /// <summary>
        ///     Top DKN
        /// </summary>
        [Description("AssMemberTopExp")] AssMemberTopExp = 4,

        /// <summary>
        ///     Cap nhat
        /// </summary>
        [Description("AssUpdateNews")] AssUpdateNews = 5,

        /// <summary>
        ///     Dang nhap sau cung
        /// </summary>
        [Description("AssMemberLastLogin")] AssMemberLastLogin = 6,

        /// <summary>
        ///     Khong dang nhap
        /// </summary>
        [Description("AssMemberNotLogin")] AssMemberNotLogin = 7,

        /// <summary>
        ///     Bai viet moi nhat cho bang
        /// </summary>
        [Description("AssNewestArticle")] AssNewestArticle = 8,

        /// <summary>
        ///     Top dai gia
        /// </summary>
        [Description("TopRichUser")] TopRichUser = 9,

        /// <summary>
        ///     Top dai gia
        /// </summary>
        [Description("DongGopBangHoiMonth")] DongGopBangHoiMonth = 10
    }
}