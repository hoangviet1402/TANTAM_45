using System.Collections.Generic;
using System.ComponentModel;
using TanTamApi.Models.Request;
using MyConfig;
using Newtonsoft.Json;

namespace TanTamApi.Models.Version
{
    //public class VersionConfigServerModel
    //{
    //    public ConfigServerModel VersionConfig
    //}
    public class RequestConfigServerModel : ApiBaseRequest
    {
        public RequestConfigServerModel()
        {
            ConfigId = 1;
            GameVersion = "1.0";
        }

        [JsonProperty("ConfigId")] public int ConfigId { get; set; }

        [JsonProperty("GameVersion")] public string GameVersion { get; set; }
    }

    public class ReportDownloadResourceErrorModel : ApiBaseRequest
    {
        [JsonProperty("ex")] public string ex { get; set; }
    }

    public class ConfigServerModel
    {
        // {"GameIpServer":"10.17.16.67"
        //,"versionWp": "1.0",
        //"versionAndroid": "1.0",
        //"versionIOS": "1.0",
        //"forceUpdate":"",
        //"message": "Các bạn hãy cập nhật phiên bản mới nhất để trải nghiệm tốt hơn",
        //"mustUpgrade": false,
        //"IsExchangeCard":true,
        //"ServerPort":"",
        //"androidLink": "https://play.google.com/store/apps/details?id=air.com.p111.mgames",
        //"iOSLink": "https://itunes.apple.com/vn/app/p111-anh-bai-online/id827805393?ls=1&mt=8",
        //"WPLink": "https://itunes.apple.com/vn/app/p111-anh-bai-online/id827805393?ls=1&mt=8",
        //"LinkFanpage":"","ApiUrl":"","PhoneSupport":""}

        public ConfigServerModel()
        {
            GameIpServer = string.Empty;
            GamePortServer = string.Empty;
            VersionWp = string.Empty;
            VersionAndroid = string.Empty;
            VersionIos = string.Empty;
            ForceUpdate = string.Empty;
            Message = string.Empty;
            MustUpgrade = false;
            IsExchangeCard = false;
            IsChargeCard = false;
            ServerPort = string.Empty;
            WpLink = string.Empty;
            IosLink = string.Empty;
            AndroidLink = string.Empty;
            LinkFanpage = string.Empty;
            LinkForum = string.Empty;
            LinkCheckIP = "http://ipinfo.io/json";
            BangHoiLink = string.Empty;
            DienDanLink = string.Empty;
            FanpageLink = string.Empty;
            LinkTransferCoin = string.Empty;
            ApiUrl = string.Empty;
            PhoneSupport = string.Empty;
            AppsflyerId = string.Empty;
            EnableIap = false;
            AllowSignUp = false;
            InviteReward = 0;
            IsIpForeign = false;
            IsReview = false;
            ChanelId = 0;
            ChanelName = string.Empty;
            AppId = 0;
            IsNewFish = false;
            FacebookAppId = "";
            IsOnlyLifeOfPi = false;
            IsEnableSMS = false;
            UrlResourceConfig = string.Empty;
            InGameMiniRes = string.Empty;
            Slots = "";
            IsEnableBangHoi = false;
            IsSandboxIapIos = false;
            EnableEmulator = false;
            PaymentPackets = string.Empty;
            BettingCountry = string.Empty;
            TrackingFB = true;
            TrackingGG = false;
            DefaultLanguage = MyConfiguration.Default.DefaultLanguage;
            ListCoinGunNormal = "10,50,100,500,1000";
            ListCoinGunVip = "1000,2000,5000,8000,10000";
            ListCoinGun7Sea = "";
            LessPointFree = MyConfiguration.Default.LessPointFree;
            LessPointBigGun = MyConfiguration.Default.LessPointBigGun;
            LessPointDaiGiaGun = MyConfiguration.Default.LessPointDaiGiaGun;
            Icons = string.Empty;
            IsShowCard = false;
            IsShowSlot = false;
            RewardFB = 0;
            RewardFBMax = 0;
            RewardRegister = 0;
            UrlPolicy = MyConfiguration.Default.FullDomain;
            IsShowWebView = false;
            IsEnableLockLevel = false;
            IsRegisterCaptchaEnabled = true;
            ImageHost = "";
        }

        public bool IsNewFish { get; set; }

        public int ChanelId { get; set; }

        public int AppId { get; set; }

        public string ChanelName { get; set; }

        public string GameIpServer { get; set; }

        public string GamePortServer { get; set; }

        [Description("versionWp")] public string VersionWp { get; set; }

        [Description("versionAndroid")] public string VersionAndroid { get; set; }

        [Description("versionIOS")] public string VersionIos { get; set; }

        [Description("forceUpdate")] public string ForceUpdate { get; set; }

        [Description("message")] public string Message { get; set; }

        [Description("mustUpgrade")] public bool MustUpgrade { get; set; }

        public bool IsExchangeCard { get; set; }

        public bool IsChargeCard { get; set; }

        public string ServerPort { get; set; }

        [Description("WPLink")] public string WpLink { get; set; }

        [Description("iOSLink")] public string IosLink { get; set; }

        [Description("androidLink")] public string AndroidLink { get; set; }

        public string LinkFanpage { get; set; }

        public string ApiUrl { get; set; }

        public string PhoneSupport { get; set; }

        public string AppsflyerId { get; set; }

        public string LinkForum { get; set; }

        public bool EnableIap { get; set; }

        public bool AllowSignUp { get; set; }

        public int InviteReward { get; set; }

        public bool IsIpForeign { get; set; }

        public bool IsReview { get; set; }

        public bool IsEnableMarket { get; set; }

        public bool IsEnable7Ocean { get; set; }

        public string ConnectionType { get; set; }

        public List<object> ExtendLink { get; set; }

        //public string IosAppsflyerId { get; set; }

        //public bool IosIsExchangeCard { get; set; }

        //public bool IosIsChargeCard { get; set; }

        //public bool IosEnableIap { get; set; }

        [JsonProperty("LessPointFree")] public int LessPointFree { get; set; }

        [JsonProperty("LessPointBigGun")] public int LessPointBigGun { get; set; }

        [JsonProperty("LessPointDaiGiaGun")] public int LessPointDaiGiaGun { get; set; }

        [JsonProperty("LinkCheckIP")] public string LinkCheckIP { get; set; }

        public string BangHoiLink { get; set; }

        public string DienDanLink { get; set; }

        public string FanpageLink { get; set; }

        public string LinkTransferCoin { get; set; }

        [JsonProperty("FacebookAppId")] public string FacebookAppId { get; set; }

        [JsonProperty("IsOnlyLifeOfPi")] public bool IsOnlyLifeOfPi { get; set; }

        [JsonProperty("IsEnableSMS")] public bool IsEnableSMS { get; set; }

        public string UrlResourceConfig { get; set; }

        public string InGameMiniRes { get; set; }

        public string Slots { get; set; }

        public bool IsEnableBangHoi { get; set; }

        [JsonProperty("IsSandboxIapIos")] public bool IsSandboxIapIos { get; set; }

        public bool EnableEmulator { get; set; }

        public string PaymentPackets { get; set; }

        public string BettingCountry { get; set; }

        public bool TrackingFB { get; set; }

        public bool TrackingGG { get; set; }

        public string DefaultLanguage { get; set; }

        public string ListCoinGunNormal { get; set; }

        public string ListCoinGunVip { get; set; }

        public string ListCoinGun7Sea { get; set; }

        public object Icons { get; set; }

        public bool IsShowCard { get; set; }

        public bool IsShowSlot { get; set; }

        public decimal RewardFB { get; set; }

        public decimal RewardFBMax { get; set; }

        public decimal RewardRegister { get; set; }

        public string UrlPolicy { get; set; }

        public bool IsShowWebView { get; set; }

        public bool IsEnableLockLevel { get; set; }

        public bool IsRegisterCaptchaEnabled { get; set; }

        public string ImageHost { get; set; }

        public List<string> AreaApiUrl { get; set; }

        public string csf { get; set; }
    }

    public class ConfigServerModelV2
    {
        public ConfigServerModelV2()
        {
            VersionAndroid = string.Empty;
            VersionIos = string.Empty;
            ForceUpdate = string.Empty;
            Message = string.Empty;
            ServerPort = string.Empty;
            IosLink = string.Empty;
            AndroidLink = string.Empty;
            LinkFanpage = string.Empty;
            FanpageLink = string.Empty;
            ApiUrl = string.Empty;
            IsReview = false;
            IsOnlyLifeOfPi = false;
            BettingCountry = string.Empty;
            TrackingFacebook = true;
            TrackingGoogle = false;
            ListCoinGunNormal = "10,50,100,500,1000";
            ListCoinGunVip = "1000,2000,5000,8000,10000";
            ListCoinGun7Sea = "";
            LessPointFree = MyConfiguration.Default.LessPointFree;
            LessPointBigGun = MyConfiguration.Default.LessPointBigGun;
            LessPointDaiGiaGun = MyConfiguration.Default.LessPointDaiGiaGun;
            Icons = string.Empty;
            RewardFacebook = 0;
            RewardFacebookMax = 0;
            RewardRegister = 0;
            UrlPolicy = MyConfiguration.Default.FullDomain;
            IsRegisterCaptchaEnabled = true;
        }

        [JsonProperty("anv")] public string VersionAndroid { get; set; }

        [JsonProperty("apv")] public string VersionIos { get; set; }

        [JsonProperty("fupd")] public string ForceUpdate { get; set; }

        [JsonProperty("ms")] public string Message { get; set; }

        [JsonProperty("sp")] public string ServerPort { get; set; }

        [JsonProperty("aplk")] public string IosLink { get; set; }

        [JsonProperty("anlk")] public string AndroidLink { get; set; }

        [JsonProperty("flk")] public string LinkFanpage { get; set; }

        [JsonProperty("aurl")] public string ApiUrl { get; set; }

        [JsonProperty("hrev")] public bool IsReview { get; set; }

        [JsonProperty("tc")] public string ConnectionType { get; set; }

        [JsonProperty("lpf")] public int LessPointFree { get; set; }

        [JsonProperty("lpbwp")] public int LessPointBigGun { get; set; }

        [JsonProperty("lprwp")] public int LessPointDaiGiaGun { get; set; }

        [JsonProperty("slk")] public string FanpageLink { get; set; }

        [JsonProperty("hpi")] public bool IsOnlyLifeOfPi { get; set; }

        [JsonProperty("betcn")] public string BettingCountry { get; set; }

        [JsonProperty("htfb")] public bool TrackingFacebook { get; set; }

        [JsonProperty("htgg")] public bool TrackingGoogle { get; set; }

        [JsonProperty("lcwpn")] public string ListCoinGunNormal { get; set; }

        [JsonProperty("lcwpv")] public string ListCoinGunVip { get; set; }

        [JsonProperty("lcwp7")] public string ListCoinGun7Sea { get; set; }

        [JsonProperty("ic")] public object Icons { get; set; }

        [JsonProperty("rewfb")] public decimal RewardFacebook { get; set; }

        [JsonProperty("rewfbm")] public decimal RewardFacebookMax { get; set; }

        [JsonProperty("rewsu")] public decimal RewardRegister { get; set; }

        [JsonProperty("purl")] public string UrlPolicy { get; set; }

        [JsonProperty("hregc")] public bool IsRegisterCaptchaEnabled { get; set; }

        [JsonProperty("imhst")] public string ImageHost { get; set; }

        [JsonProperty("minirss")] public string InGameMiniRes { get; set; }

        [JsonProperty("csf")] public string CountryShowFree { get; set; }
    }

    public class ConfigServerClusterModel
    {
        public string ApiUrl { get; set; }
        public string FanpageLink { get; set; }
        public string LinkFanpage { get; set; }
        public string ImageHost { get; set; }
        public List<string> AreaApiUrl { get; set; }
    }

    public class StaticInGameUrlModel
    {
        [JsonProperty("c")] public string Country { get; set; }

        [JsonProperty("v")] public dynamic Value { get; set; }
    }
}