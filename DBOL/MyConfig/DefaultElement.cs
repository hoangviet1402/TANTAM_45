/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: Quan ly thong tin cau hinh chung cho project
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using System.Configuration;

namespace MyConfig
{
    public class DefaultElement : ConfigurationElement
    {
        /// <summary>
        ///     Cho biết site có đang ở chế độ Beta không
        ///     Nếu là Beta thì chỉ cho phép một số tính năng hoạt động
        ///     Các tính năng này được lọc trong file Authorize ở Infrastructure
        ///     <para>Author: PhatVT</para>
        ///     <para>Create Date: 05/03/2015</para>
        /// </summary>
        [ConfigurationProperty("IsBeta", DefaultValue = true)]
        public bool IsBeta => (bool)this["IsBeta"];

        [ConfigurationProperty("EnablePass2", DefaultValue = true)]
        public bool EnablePass2 => (bool)this["EnablePass2"];

        [ConfigurationProperty("IsLogLoginInLobby", DefaultValue = true)]
        public bool IsLogLoginInLobby => (bool)this["IsLogLoginInLobby"];

        [ConfigurationProperty("IsAppTest", DefaultValue = false)]
        public bool IsAppTest => (bool)this["IsAppTest"];

        [ConfigurationProperty("EnableDebug", DefaultValue = false)]
        public bool EnableDebug => (bool)this["EnableDebug"];

        [ConfigurationProperty("IsEnableVQMM", DefaultValue = false)]
        public bool IsEnableVQMM => (bool)this["IsEnableVQMM"];

        [ConfigurationProperty("PageSize", DefaultValue = 10)]
        public int PageSize => (int)this["PageSize"];

        [ConfigurationProperty("IsAutoGenDisplayNameInstantFromDb", DefaultValue = false)]
        public bool IsAutoGenDisplayNameInstantFromDb => (bool)this["IsAutoGenDisplayNameInstantFromDb"];


        [ConfigurationProperty("LimitDepositForShowGame", DefaultValue = 10)]
        public int LimitDepositForShowGame => (int)this["LimitDepositForShowGame"];

        [ConfigurationProperty("DefaultLanguage", DefaultValue = "vn")]
        public string DefaultLanguage => (string)this["DefaultLanguage"];

        [ConfigurationProperty("DefaultLanguageCookieName", DefaultValue = "lang")]
        public string DefaultLanguageCookieName => (string)this["DefaultLanguageCookieName"];

        [ConfigurationProperty("AppId", DefaultValue = 0)]
        public int AppId => (int)this["AppId"];

        /// <summary>
        ///     cấu hình nhân 10 (x10,...)
        /// </summary>
        [ConfigurationProperty("MucHienThiTienTheoServer", DefaultValue = 1)]
        public int MucHienThiTienTheoServer => (int)this["MucHienThiTienTheoServer"];

        /// <summary>
        ///     Author: ThongNT
        ///     <para>Ten domain default: http://hoiquan52.com</para>
        /// </summary>
        [ConfigurationProperty("HostName", DefaultValue = "http://localhost:2967/")]
        public string HostName => (string)this["HostName"];


        [ConfigurationProperty("AppName", DefaultValue = "Vina Game")]
        public string AppName => (string)this["AppName"];

        /// <summary>
        ///     <para>0: vgt</para>
        ///     <para>1: vsc</para>
        ///     <para>2: srv</para>
        /// </summary>
        [ConfigurationProperty("DomainID", DefaultValue = 0)]
        public int DomainID => (int)this["DomainID"];

        /// <summary>
        ///     Author: ThongNT
        ///     <para>Domain chua hinh cua user</para>
        /// </summary>
        [ConfigurationProperty("ImageHost", DefaultValue = "http://192.168.5.167:8005/")]
        public string ImageHost => (string)this["ImageHost"];

        /// <summary>
        ///     Author: QuangPN
        ///     <para>Đường dẫn lưu avatar. Lưu trong trang web</para>
        /// </summary>
        [ConfigurationProperty("AvatarPath", DefaultValue = "i/")]
        public string AvatarPath => (string)this["AvatarPath"];

        [ConfigurationProperty("DefaultAvatar", DefaultValue = "i/d1.png")]
        public string DefaultAvatar => (string)this["DefaultAvatar"];

        /// <summary>
        ///     Author: TrungTT
        ///     Descriiption: Chanel hien tai - config theo ChannelIdEnum
        /// </summary>
        [ConfigurationProperty("ChannelId", DefaultValue = 1)]
        public int ChannelId => (int)this["ChannelId"];

        /// <summary>
        ///     Author: TrungTT
        ///     UserId mac dinh cua admin
        /// </summary>
        [ConfigurationProperty("UserAdmin", DefaultValue = 11334)]
        public int UserAdmin => (int)this["UserAdmin"];

        /// <summary>
        ///     Version xoa Cache CSS va Javascript
        /// </summary>
        [ConfigurationProperty("ContentVersion", DefaultValue = 1)]
        public int ContentVersion => (int)this["ContentVersion"];

        [ConfigurationProperty("LinkFanPageFaceBookAppReview", DefaultValue = "")]
        public string LinkFanPageFaceBookAppReview => (string)this["LinkFanPageFaceBookAppReview"];

        [ConfigurationProperty("IsUseMessageV3", DefaultValue = "false")]
        public bool IsUseMessageV3 => (bool)this["IsUseMessageV3"];

        [ConfigurationProperty("SecurityKey", DefaultValue = "abc")]
        public string SecurityKey => (string)this["SecurityKey"];

        [ConfigurationProperty("SecurityIV", DefaultValue = "123")]
        public string SecurityIV => (string)this["SecurityIV"];

        [ConfigurationProperty("AuthenticationStatusCodeCheck", DefaultValue = "0")]
        public int AuthenticationStatusCodeCheck => (int)this["AuthenticationStatusCodeCheck"];

        /// <summary>
        ///     bật/tắt cache EventPromotion
        /// </summary>
        [ConfigurationProperty("IsEnableCacheEventPormotion", DefaultValue = false)]
        public bool IsEnableCacheEventPormotion => (bool)this["IsEnableCacheEventPormotion"];

        /// <summary>
        ///     config nội dung thư là html
        /// </summary>
        [ConfigurationProperty("MobileOfflineMessageFormat", DefaultValue = "{0}")]
        public string MobileOfflineMessageFormat => (string)this["MobileOfflineMessageFormat"];

        /// <summary>
        ///     cấu hình update FullName là DisplayName
        /// </summary>
        [ConfigurationProperty("IsConfigUpdateFullName", DefaultValue = false)]
        public bool IsConfigUpdateFullName => (bool)this["IsConfigUpdateFullName"];

        [ConfigurationProperty("NumerDefaultAvatar", DefaultValue = 0)]
        public int NumerDefaultAvatar => (int)this["NumerDefaultAvatar"];

        /// <summary>
        ///     MiniGameApiResponseCode
        /// </summary>
        [ConfigurationProperty("MinigameResponseCode", DefaultValue = 1)]
        public int MinigameResponseCode => (int)this["MinigameResponseCode"];

        [ConfigurationProperty("IsShowMailDefault", DefaultValue = false)]
        public bool IsShowMailDefault => (bool)this["IsShowMailDefault"];

        [ConfigurationProperty("DefaultUtmMobileOrganic", DefaultValue = "")]
        public string DefaultUtmMobileOrganic => (string)this["DefaultUtmMobileOrganic"];

        [ConfigurationProperty("IsUnicodeDisplayName", DefaultValue = false)]
        public bool IsUnicodeDisplayName => (bool)this["IsUnicodeDisplayName"];

        [ConfigurationProperty("IsCheckEmoji", DefaultValue = true)]
        public bool IsCheckEmoji => (bool)this["IsCheckEmoji"];

        [ConfigurationProperty("IsCheckSpecialChar", DefaultValue = true)]
        public bool IsCheckSpecialChar => (bool)this["IsCheckSpecialChar"];

        [ConfigurationProperty("IsCheckGroupUnicode", DefaultValue = true)]
        public bool IsCheckGroupUnicode => (bool)this["IsCheckGroupUnicode"];

        [ConfigurationProperty("IsUseOpenProviderCache", DefaultValue = false)]
        public bool IsUseOpenProviderCache => (bool)this["IsUseOpenProviderCache"];

        /// <summary>
        ///     Thoi gian cache cua token cho openid
        /// </summary>
        [ConfigurationProperty("OpenProviderCacheExpireTime", DefaultValue = 30)]
        public int OpenProviderCacheExpireTime => (int)this["OpenProviderCacheExpireTime"];

        /// <summary>
        ///     Bật/Tắt hiển thị giờ theo TimeZone
        /// </summary>
        [ConfigurationProperty("IsEnableDisplayCustomTimeZone", DefaultValue = false)]
        public bool IsEnableDisplayCustomTimeZone => (bool)this["IsEnableDisplayCustomTimeZone"];

        /// <summary>
        ///     Id TimeZone muốn convert datetime sang để hiển thị
        ///     Id lấy từ TimeZoneInfo.GetSystemTimeZones().Select(t => t.Id)
        /// </summary>
        [ConfigurationProperty("DisplayTimeZoneStringId", DefaultValue = "")]
        public string DisplayTimeZoneStringId => (string)this["DisplayTimeZoneStringId"];

        [ConfigurationProperty("IsDuplicateDisplayName", DefaultValue = true)]
        public bool IsDuplicateDisplayName => (bool)this["IsDuplicateDisplayName"];

        [ConfigurationProperty("IsLoginLogSplit", DefaultValue = false)]
        public bool IsLoginLogSplit => (bool)this["IsLoginLogSplit"];

        [ConfigurationProperty("MiniGameUserSource", DefaultValue = 0)]
        public int MiniGameUserSource => (int)this["MiniGameUserSource"];

        /// <summary>
        ///     on/off upload avatar
        /// </summary>
        [ConfigurationProperty("IsUploadAvatar", DefaultValue = false)]
        public bool IsUploadAvatar => (bool)this["IsUploadAvatar"];

        /// <summary>
        ///     on/off upload cover
        /// </summary>
        [ConfigurationProperty("IsUploadCover", DefaultValue = false)]
        public bool IsUploadCover => (bool)this["IsUploadCover"];

        /// <summary>
        ///     Ảnh nền mặc định
        /// </summary>
        [ConfigurationProperty("DefaultCover", DefaultValue = "i/defaultcover.png")]
        public string DefaultCover => (string)this["DefaultCover"];

        /// <summary>
        ///     Máy chủ tĩnh
        /// </summary>
        [ConfigurationProperty("StaticHost", DefaultValue = "http://fn.vinagame.com")]
        public string StaticHost => (string)this["StaticHost"];

        /// <summary>
        ///     Thời gian chờ
        /// </summary>
        [ConfigurationProperty("DelayTime", DefaultValue = 1)]
        public int DelayTime => (int)this["DelayTime"];

        [ConfigurationProperty("IsCheckIsReviewInCode", DefaultValue = false)]
        public bool IsCheckIsReviewInCode => (bool)this["IsCheckIsReviewInCode"];

        [ConfigurationProperty("RouteAuthenApi2", DefaultValue = "http://abc.net/authen2")]
        public string RouteAuthenApi2 => (string)this["RouteAuthenApi2"];

        [ConfigurationProperty("RouteAuthenVQMMIsShowWebView", DefaultValue = false)]
        public bool RouteAuthenVQMMIsShowWebView => (bool)this["RouteAuthenVQMMIsShowWebView"];

        [ConfigurationProperty("RouteAuthenVQMM", DefaultValue = "http://abc.net/authen2")]
        public string RouteAuthenVQMM => (string)this["RouteAuthenVQMM"];

        [ConfigurationProperty("MaxCheckinDate", DefaultValue = 30)]
        public int MaxCheckinDate => (int)this["MaxCheckinDate"];

        [ConfigurationProperty("AwardCheckin", DefaultValue = 300)]
        public int AwardCheckin => (int)this["AwardCheckin"];

        [ConfigurationProperty("EventPromotionAllDeposit", DefaultValue = 34)]
        public int EventPromotionAllDeposit => (int)this["EventPromotionAllDeposit"];

        [ConfigurationProperty("CCURandomMin", DefaultValue = 200)]
        public int CCURandomMin => (int)this["CCURandomMin"];

        [ConfigurationProperty("CCURandomMax", DefaultValue = 371)]
        public int CCURandomMax => (int)this["CCURandomMax"];

        [ConfigurationProperty("DefaultAvatarHostCookieName", DefaultValue = "avatar-host")]
        public string DefaultAvatarHostCookieName => (string)this["DefaultAvatarHostCookieName"];

        [ConfigurationProperty("MinLengthId", DefaultValue = 5)]
        public int MinLengthId => (int)this["MinLengthId"];

        [ConfigurationProperty("MaxLengthId", DefaultValue = 100000000)]
        public int MaxLengthId => (int)this["MaxLengthId"];

        #region Domain

        /// <summary>
        ///     Url full domain
        ///     <para>Author: PhatVT</para>
        ///     <para>Date Created: 12/22/2014</para>
        /// </summary>
        [ConfigurationProperty("FullDomain", DefaultValue = "")]
        public string FullDomain => (string)this["FullDomain"];

        /// <summary>
        ///     Url soft domain
        ///     <para>Author: PhatVT</para>
        ///     <para>Date Created: 12/22/2014</para>
        /// </summary>
        [ConfigurationProperty("ShortDomain", DefaultValue = "")]
        public string ShortDomain => (string)this["ShortDomain"];

        #endregion

        #region key

        [ConfigurationProperty("ConfigSecretKey", DefaultValue = "71vw35@4sd5")]
        public string ConfigSecretKey => (string)this["ConfigSecretKey"];

        [ConfigurationProperty("IsEncryptConfig", DefaultValue = false)]
        public bool IsEncryptConfig => (bool)this["IsEncryptConfig"];

        [ConfigurationProperty("LessPointFree", DefaultValue = 10)]
        public int LessPointFree => (int)this["LessPointFree"];

        [ConfigurationProperty("LessPointBigGun", DefaultValue = 100000)]
        public int LessPointBigGun => (int)this["LessPointBigGun"];

        [ConfigurationProperty("LessPointDaiGiaGun", DefaultValue = 1000000)]
        public int LessPointDaiGiaGun => (int)this["LessPointDaiGiaGun"];

        #endregion

        #region Event

        // mobile
        [ConfigurationProperty("EventCategoryMobile", DefaultValue = 7)]
        public int EventCategoryMobile => (int)this["EventCategoryMobile"];

        [ConfigurationProperty("EventCategoryMobileLimit", DefaultValue = 17)]
        public int EventCategoryMobileLimit => (int)this["EventCategoryMobileLimit"];

        [ConfigurationProperty("EventPageSizeMobile", DefaultValue = 15)]
        public int EventPageSizeMobile => (int)this["EventPageSizeMobile"];

        [ConfigurationProperty("UrlEventPage", DefaultValue = "/a/{0}")]
        //[ConfigurationProperty("UrlEventPage", DefaultValue = "/Event/EventDetailMobile?idEvent={0}&lang={1}")]
        public string UrlEventPage => (string)this["UrlEventPage"];

        #endregion

        #region Guide

        // mobile
        [ConfigurationProperty("GuideCategoryMobile", DefaultValue = 6)]
        public int GuideCategoryMobile => (int)this["GuideCategoryMobile"];

        [ConfigurationProperty("GuideCategoryMobileLimit", DefaultValue = 16)]
        public int GuideCategoryMobileLimit => (int)this["GuideCategoryMobileLimit"];

        [ConfigurationProperty("GuidePageSizeMobile", DefaultValue = 5)]
        public int GuidePageSizeMobile => (int)this["GuidePageSizeMobile"];

        #endregion

        #region register

        [ConfigurationProperty("IsOpenFacebookRegister", DefaultValue = true)]
        public bool IsOpenFacebookRegister => (bool)this["IsOpenFacebookRegister"];

        [ConfigurationProperty("IsForceEnableEventRegister", DefaultValue = false)]
        public bool IsForceEnableEventRegister => (bool)this["IsForceEnableEventRegister"];

        [ConfigurationProperty("CoinBonusRegister", DefaultValue = 0)]
        public int CoinBonusRegister => (int)this["CoinBonusRegister"];

        /// <summary>
        ///     hiển thị số xu tặng user đăng ký mới trên form
        /// </summary>
        [ConfigurationProperty("RewardRegister", DefaultValue = 0)]
        public int RewardRegister => (int)this["RewardRegister"];

        [ConfigurationProperty("IsEventGameGetByCacheServer", DefaultValue = false)]
        public bool IsEventGameGetByCacheServer => (bool)this["IsEventGameGetByCacheServer"];

        #endregion

        #region Tool App Info

        /// <summary>
        /// Số item muốn lấy ra
        /// </summary>
        [ConfigurationProperty("NumberOfItemsToTake", DefaultValue = 6)]
        public int NumberOfItemsToTake => (int)this["NumberOfItemsToTake"];

        /// <summary>
        ///     Author: TuanNN
        ///     <para>Folder chua hinh tiny</para>
        /// </summary>
        [ConfigurationProperty("ImageTinyFolder", DefaultValue = "icTiny")]
        public string ImageTinyFolder => (string)this["ImageTinyFolder"];

        /// <summary>
        ///     Author: TuanNN
        ///     <para>Giới hạn ký tự tối đa của app name</para>
        /// </summary>
        [ConfigurationProperty("MaxLengthAppName", DefaultValue = 30)]
        public int MaxLengthAppName => (int)this["MaxLengthAppName"];

        /// <summary>
        ///     Author: TuanNN
        ///     <para>Giới hạn ký tự tối đa của package name</para>
        /// </summary>
        [ConfigurationProperty("MaxLengthPackageName", DefaultValue = 60)]
        public int MaxLengthPackageName => (int)this["MaxLengthPackageName"];

        [ConfigurationProperty("DomainApiApk", DefaultValue = "http://192.168.1.163:8066")]
        public string DomainApiApk => (string)this["DomainApiApk"];

        [ConfigurationProperty("EnableRegisterAccountUser", DefaultValue = true)]
        public bool EnableRegisterAccountUser => (bool)this["EnableRegisterAccountUser"];

        /// <summary>
        /// Bật/tắt ghi log thao tác của Admin
        /// </summary>
        [ConfigurationProperty("IsEnableLogActionAdmin", DefaultValue = true)]
        public bool IsEnableLogActionAdmin => (bool)this["IsEnableLogActionAdmin"];

        [ConfigurationProperty("CountriesWhiteList", DefaultValue = "")]
        public string CountriesWhiteList => (string)this["CountriesWhiteList"];

        [ConfigurationProperty("DefaultIpUserGame", DefaultValue = "127.0.0.1")]
        public string DefaultIpUserGame => (string)this["DefaultIpUserGame"];

        #endregion
    }
}