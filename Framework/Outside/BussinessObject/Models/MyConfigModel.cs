using System.Collections.Generic;
using System.ComponentModel;
using MyConfig;

namespace BussinessObject.Models
{
    public class MyConfigModel
    {
        public MyConfigModel()
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
            ConnectionType = "rs";
            AppId = 0;
            IsNewFish = false;
            FacebookAppId = string.Empty;
            IsOnlyLifeOfPi = false;
            IsEnableSMS = false;
            UrlResourceConfig = string.Empty;
            InGameMiniRes = string.Empty;
            Slots = string.Empty;
            IsEnableBangHoi = false;
            IsSandboxIapIos = false;
            EnableEmulator = false;
            PaymentPackets = string.Empty;
            BettingCountry = string.Empty;
            TrackingFB = true;
            TrackingGG = false;
            ListCoinGunNormal = "10,50,100,500,1000";
            ListCoinGunVip = "1000,2000,5000,8000,10000";
            ListCoinGun7Sea = string.Empty;
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
            StaticInGameUrl = string.Empty;
        }

        public bool IsNewFish { get; set; }

        public int AppId { get; set; }

        public int ChanelId { get; set; }

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

        public string FlatformName { get; set; }

        public string ConnectionType { get; set; }

        public string GameVersion { get; set; }

        public string LinkDownload { get; set; }

        public string Port { get; set; }

        public bool IsEnableMarket { get; set; }

        public bool IsEnable7Ocean { get; set; }

        public string FacebookAppId { get; set; }

        public bool IsOnlyLifeOfPi { get; set; }

        public bool IsEnableSMS { get; set; }

        public string UrlResourceConfig { get; set; }

        public string InGameMiniRes { get; set; }

        public string Slots { get; set; }

        public string BangHoiLink { get; set; }

        public string DienDanLink { get; set; }

        public string FanpageLink { get; set; }

        public string LinkTransferCoin { get; set; }

        public bool IsEnableBangHoi { get; set; }

        public bool IsSandboxIapIos { get; set; }

        public bool EnableEmulator { get; set; }

        public string PaymentPackets { get; set; }

        public string BettingCountry { get; set; }

        public bool TrackingFB { get; set; }

        public bool TrackingGG { get; set; }

        public string ListCoinGunNormal { get; set; }

        public string ListCoinGunVip { get; set; }

        public string ListCoinGun7Sea { get; set; }

        public int LessPointFree { get; set; }

        public int LessPointBigGun { get; set; }

        public int LessPointDaiGiaGun { get; set; }

        public string Icons { get; set; }

        public bool IsShowCard { get; set; }

        public bool IsShowSlot { get; set; }

        public decimal RewardFB { get; set; }

        public decimal RewardFBMax { get; set; }

        public decimal RewardRegister { get; set; }

        public string UrlPolicy { get; set; }

        public bool IsShowWebView { get; set; }

        public bool IsEnableLockLevel { get; set; }

        public string StaticInGameUrl { get; set; }

        /// <summary>
        ///     <para>null/"" -> ko show room free</para>
        ///     <para>"abc"/"abc,xyz" -> show room free cho 2 country này</para>
        ///     <para>"all" -> show free cho all country</para>
        /// </summary>
        public string CountryShowFree { get; set; }
    }

    public class ConfigSoSValue
    {
        public string Message { get; set; }
        public bool IsEnable { get; set; }
    }

    public class ChanelApiModel
    {
        public int AppId { get; set; }
        public int ChanelId { get; set; }
        public string ConnectionType { get; set; }
        public string ChanelName { get; set; }
        public List<ConfigApiModel> ChanelData { get; set; }
    }

    public class ConfigApiModel
    {
        public string FlatFormId { get; set; }

        public string LinkForum { get; set; }

        public string LinkFanpage { get; set; }

        public string AppsflyerId { get; set; }

        public string ApiUrl { get; set; }

        public string PhoneSupport { get; set; }

        public List<Versions> Versions { get; set; }

        public string FlatformName { get; set; }
    }

    public class Versions
    {
        public string GameVersion { get; set; }

        public string Version { get; set; }

        public string ForceUpdate { get; set; }

        public string Message { get; set; }

        public bool MustUpgrade { get; set; }

        public bool IsExchangeCard { get; set; }

        public string LinkDownload { get; set; }

        public bool EnableIap { get; set; }

        public bool AllowSignUp { get; set; }

        public int InviteReward { get; set; }

        public bool IsIpForeign { get; set; }

        public bool IsChargeCard { get; set; }

        public bool IsReview { get; set; }

        public bool IsEnableMarket { get; set; }

        public bool IsEnable7Ocean { get; set; }

        public bool IsOnlyLifeOfPi { get; set; }

        public bool IsEnableSMS { get; set; }

        public bool IsNewFish { get; set; }

        public List<GameServers> GameServers { get; set; }

        public string UrlResourceConfig { get; set; }

        public string ApiUrl { get; set; }

        public string InGameMiniRes { get; set; }

        public string Slots { get; set; }

        public string BangHoiLink { get; set; }

        public string DienDanLink { get; set; }

        public string LinkFanpage { get; set; }

        public string LinkTransferCoin { get; set; }

        public bool IsEnableBangHoi { get; set; }

        public bool IsSandboxIapIos { get; set; }

        public bool EnableEmulator { get; set; }

        public string PaymentPackets { get; set; }

        public string BettingCountry { get; set; }

        public bool TrackingFB { get; set; }

        public bool TrackingGG { get; set; }

        public string ListCoinGunNormal { get; set; }

        public string ListCoinGunVip { get; set; }

        public string ListCoinGun7Sea { get; set; }

        public int LessPointFree { get; set; }

        public int LessPointBigGun { get; set; }

        public int LessPointDaiGiaGun { get; set; }

        public string Icons { get; set; }

        public bool IsShowCard { get; set; }

        public bool IsShowSlot { get; set; }

        public decimal RewardFB { get; set; }

        public decimal RewardFBMax { get; set; }

        public decimal RewardRegister { get; set; }

        public string UrlPolicy { get; set; }

        public bool IsShowWebView { get; set; }

        public bool IsEnableLockLevel { get; set; }

        public string StaticInGameUrl { get; set; }

        /// <summary>
        ///     <para>null/"" -> ko show room free</para>
        ///     <para>"abc"/"abc,xyz" -> show room free cho 2 country này</para>
        ///     <para>"all" -> show free cho all country</para>
        /// </summary>
        public string CountryShowFree { get; set; }
    }

    public class GameServers
    {
        public string Ip { get; set; }

        public string Port { get; set; }
    }
}