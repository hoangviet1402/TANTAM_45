using System.ComponentModel;

namespace BussinessObject.Enum
{
    /// <summary>
    ///     quy định code cho hai con BanCaMobileApi và MiniGameApi
    /// </summary>
    public enum ApiStatusCode
    {
        [Description("Success")]
        Success = 1,

        [Description("Failed")]
        Failed = 2,

        [Description("Web view")]
        WebView = 3,

        [Description("Invalid NickName")]
        InvalidDisplayName = 4,
        
        [Description("Existed Account")]
        ExistedAccount = 5,

        [Description("Invalid NickName")]
        InvalidNickName = 6,

        [Description("User not login")]
        UserNotLogin = 999,

        [Description("Server maintenance")]
        ServerMaintenance = 1000,

        [Description("System error")]
        SystemError = 1001,

        [Description("Token Expire")]
        TokenExpire = 1002,

        [Description("Unexisted api")]
        UnexistedApi = 1003,

        [Description("Invalid data input")]
        InvalidDataInput = 1004,

        [Description("Not login or login other")]
        InvalidSign = 1005,

        [Description("Not eunning")]
        NotRunning = 1006,

        [Description("Add failed")]
        AddOrSubtractGoldFailed = 1007,

        [Description("Profile email has exist")]
        ProfileEmailHasExist = 1008,

        [Description("Invalid data")]
        InvalidData = 1009,

        [Description("Invalid captcha")]
        InvalidCaptcha = 1010,

        [Description("Expire captcha")]
        ExpireCaptcha = 1011,

        [Description("AccountLocked")]
        AccountLocked = 1012,

        [Description("Retry")] 
        RetryOrtherArea = 2000,

        [Description("Send request to trace")] 
        SendRequestTraceLog = 2001,

        [Description("No room")] 
        NoRoom = 202,

        [Description("NotEnoughtCoinForJoinroom")]
        NotEnoughtCoinForJoinroom = 203,

        /// <summary>
        ///     Không có cấu hình sản phẩm
        /// </summary>
        NoProductConfig = 105,

        /// <summary>
        ///     Hết hạn mức nạp
        /// </summary>
        ReachLimit = 108,

        /// <summary>
        ///     Mở bằng browser
        /// </summary>
        IsOpenWeb = 101,

        /// <summary>
        ///     Không có cấu hình ví
        /// </summary>
        NoWallet = 102,

        #region guild

        [Description("Bang không tồn tại")] 
        GuildNotExist = 1010,

        [Description("Tài khoản không phải bang chủ")]
        NotGuildOwner = 1011,

        [Description("Yêu cầu rời bang cũ.")] 
        HaveGuild = 1012,

        [Description("Gia nhập bang thất bại.")]
        JoinGuildFail = 1013,

        [Description("Gia nhập bang thành công.")]
        JoinGuildSuccess = 1014,

        [Description("Bạn đang bị cấm thảo luận.")]
        CmtFailBlock = 1015,

        [Description("Số ký tự vượt quá giới hạn")]
        ContentErrorLenght = 1016,

        [Description("Nội dung không phù hợp.")]
        ContentNoAllow = 1017,

        [Description("Rời Bang thành công.")] 
        QuitGuildSuccess = 1018,

        [Description("Bang chủ không được rời bang.")]
        QuitGuildIsOwner = 1019,

        [Description("Bang chủ không được rời bang.")]
        HaveNoGuild = 1020,

        [Description("Slogan không chứa ký tự bị cấm.")]
        InvalidSloganGuild = 1021,

        [Description("Chỉ được kick user vào những ngày quy định.")]
        InvalidDaysAllowKickUser = 1022,

        [Description("User không có trong bang.")]
        UserNotInGuild = 1023,

        [Description("Nội dung không phù hợp.")]
        GiftCodeNoAllow = 1024,

        [Description("Tài khoản FB đã link với account khác")]
        AccountLinked = 9999,

        #endregion

        #region Payment

        MinCoinDeposit = 1201,
        MaxCoinDeposit = 1202,
        ServerDepositError = 1203,

        #endregion

        #region Event

        EventNoRunning = 314,
        UserReceivedAReward = 318,

        #region Task

        /// <summary>
        ///     Chưa hoàn thành nhiệm vụ
        /// </summary>
        FinishTaskMissionNotCompleted = 40,

        /// <summary>
        ///     Hết thời gian thực hiện nhiệm vụ
        /// </summary>
        FinishTaskMissionExpired = 41,

        #endregion

        #endregion

        #region tool 
        AdminNotHaveChannelID = 4001,
        
        [Description("No user game")] 
        NoUserIdGame = 4002,
        #endregion

        /// <summary>
        /// Không có data
        /// </summary>
        [Description("No data")]  
        NoData = 4,

        [Description("Processing")]  
        Processing = 2000,
    }

    public enum MobileMiniGameApiStatusCode
    {
        [Description("Thành công")] Success = 0,

        [Description("Thất bại")] Failed = 2,

        [Description("Web view")] WebView = 3,

        [Description("Tài khoản chưa đăng nhập")]
        UserNotLogin = 999,

        [Description("Chức năng đang bảo trì")]
        ServerMaintenance = 1000,

        [Description("Lỗi hệ thống")] SystemError = 1001,

        [Description("Token đã hết hạn")] TokenExpire = 1002,

        [Description("API không tồn tại")] UnexistedApi = 1003,

        [Description("Dữ liệu không hợp lệ")] InvalidDataInput = 1004,

        [Description("Chưa đăng nhập hoặc đăng nhập tại máy khác.")]
        InvalidSign = 1005,

        #region VQMM

        [Description("Hợp lệ.")] LuckySpinPass = 1006,

        [Description("Chức năng tạm khóa! Vui lòng liên hệ CSKH.")]
        OutOfLimitAllUserInDay = 1007,

        [Description("Tài khoản của bạn đang bị khóa! Vui lòng liên hệ CSKH.")]
        CheckBanByUserId = 1008,

        [Description("Địa chỉ Ip đang bị khóa! Vui lòng liên hệ CSKH.")]
        CheckBanByIP = 1009,

        [Description("Chức năng tạm khóa! Vui lòng liên hệ CSKH.")]
        OutOfDayOfWeek = 1010,

        [Description("Chức năng tạm khóa! Vui lòng liên hệ CSKH.")]
        OutOfTimeOfDay = 1011,

        #endregion
    }

    public enum ServerApiCode
    {
        [Description("Request param invalid")]
        RequestParamInvalid = -2, // Param request null or empty

        [Description("Disable server API")]
        DisableServerApi = -1, // Tắt gọi api

        [Description("Success")]
        Success = 1, // Thành công

        [Description("Get_apk_iconApp_only_support_png_base64")]
        IconAppInvalid = 2, // iconApp rỗng hoặc null

        [Description("Get_apk_appName_empty")]
        AppNameInvalid = 3,  // appName rỗng hoặc null

        [Description("Get_apk_packageName_empty")]
        PackageNameInvalid = 4,  // packageName rỗng hoặc null

        [Description("Get_apk_channelId_null")]
        ChannelIdNull = 5, // channelId null

        [Description("Get_apk_subChannelId_null")]
        SubChannelIdNull = 6, // subChannelId null

        [Description("Get_apk_tool_error")]
        ApkToolError = 7, // tool build bị lỗi

        [Description("Get_apk_get_build_info_error")]
        GetBuildInfoError = 8, // lấy build info bị lỗi

        [Description("Get_apk_another_process_is_building")]
        AnotherProcessIsBuilding = 9, // có 1 tiến trình khác đang được xử lý

        [Description("Get_apk_api_url_empty")]
        GetApkApiUrlEmpty = 11, // api url rỗng hoặc empty

        [Description("Get_apk_api_fallback_url_empty")]
        GetApkApiFallbackUrlEmpty = 12, // api fallback url rỗng hoặc null

        [Description("Get_apk_api_appName_invalid")]
        GetApkApiAppNameInvalid = 13,

        [Description("Get_apk_api_packageName_invalid")]
        GetApkApiPackageNameInvalid = 14,
    }

    public class InvalidSign
    {
        public string BeforeCrypt { get; set; }
        public string AfterCrypt { get; set; }
    }
}