namespace TanTamApi.Models
{
    public class ApiMethodName
    {
        #region Link

        public const string GetLinkGuild = "api/glh";

        #endregion

        #region Config Server

        public const string GetConfigServer = "api/gc";
        public const string GetCaptcha = "api/gcpt";
        public const string CheckCaptcha = "api/ccpt";

        #endregion

        #region Account

        public const string Login = "api/lf";
        public const string PlayNow = "api/pn";
        public const string Register = "api/rf";
        public const string GetUserInfo = "api/gu";
        public const string GetUserInfoLobby = "api/gulb";
        public const string GetUserInfo2 = "api/gu2";
        public const string GetListGameInfo = "api/lgi";
        public const string PlayFacebook = "api/pf";
        public const string UpdateProfile = "api/uprl";
        public const string ChangePassword = "api/cp";
        public const string ReportReviewer = "api/rrwer";
        public const string UpdateEmailPhone = "api/uep";
        public const string GetOfflineMessage = "api/gom";
        public const string DeleteFacebookAccount = "api/da";
        public const string AppleDeleteAccount = "api/dlta";
        public const string GetOfflineMessageDetails = "api/gomd";
        public const string ReadAllMessage = "api/ram";
        public const string DeleteAllMessage = "api/dam";
        public const string DeleteOneMessage = "api/dom";
        public const string UploadAvatarMobile = "api/uam";
        public const string UploadCoverImage = "api/uci";
        public const string ChoiseCoverImage = "api/cci";
        public const string TrackingAds = "api/trad";

        #region Update

        public const string UpdateNickName = "api/unn";
        public const string UpdateDisplayName = "api/udn";
        public const string UpdatePhone = "api/uph";
        public const string UpdateEmail = "api/uem";
        public const string UpdatePass = "api/upa";
        public const string UpdateAvatar = "api/uav";
        public const string UpdateGender = "api/ugd";
        public const string UpdateDateOfBirth = "api/udob";

        #endregion

        #region Game User Account

        public const string GetGameUserInfo = "api/ggui";

        #endregion

        #endregion

        #region Profile

        #endregion

        #region Activity

        public const string GetTreasureHunter = "api/gth";
        public const string GetListUserOnline = "api/gluo";
        public const string GetListUserOnline2 = "api/gluo2";

        #endregion

        #region Lobby

        public const string GetLobby = "api/lb";
        public const string GetBroadcast = "api/gbc";
        public const string EventsLobbyBonus = "api/elpb";
        public const string EventsLobby = "api/evlb";
        public const string KickUser = "api/kur";
        public const string GetUserCoin = "api/guc";
        public const string LevelUserReward = "api/lur";

        #endregion

        #region Payment

        public const string GetGoogleIAP = "api/gi";
        public const string GetListHistoryCoin = "api/hcl";
        public const string GetWallet = "api/gw";
        public const string CreateTransaction = "api/ct";
        public const string SubmitTransaction = "api/sti";
        public const string InitCreditTrans = "api/ict";
        public const string CreditGoogleV2 = "api/cg2";
        public const string GameCreditListGame = "api/gclg";
        public const string GameInitCreditTrans = "api/gict";
        public const string GameCreditGoogleV2 = "api/gcg2";

        public const string GetWithDraw = "api/gwd";
        public const string GetLastWithDraw = "api/lwd";
        public const string SubmitWithDraw = "api/swd";

        public const string GetPaymentVN = "api/gpv";
        public const string ChargeCardVN = "api/ccv";
        public const string ChargeMomoVN = "api/cmv";

        public const string CreateWalletDeposit = "api/cwd";
        public const string SubmitDepositWallet = "api/sdw";

        #endregion
        
        #region Game

        public const string GetServer = "api/gs";
        public const string GetServerV2 = "api/gs2";
        public const string GetServerLobby = "api/gsly";
        public const string GetLevelGeneralGame = "api/glgg";
        public const string GetLevelGeneralGameUpReward = "api/glur";
        public const string GetListLevelsGame = "api/gllg";

        #endregion

        #region Version Game

        #endregion

        #region ToolChanneling
        
        public const string GenerateSubChannel = "api/gsc";
        public const string GetApkLink = "api/gal";
        public const string GetIconsAvailable = "api/gia";
        public const string GetAppBuildHistory = "api/gabh";
        public const string GetAppNamesAvailable = "api/gana";
        public const string GetPackageNamesAvailable = "api/gpna";
        public const string GetApkBuildInfo = "api/gabi";
        public const string SubmitBuildApk = "api/sba";

        public const string GetReportNRU = "api/gnru";
        public const string GetReportAU = "api/grau";
        public const string GetReportBalance = "api/grbl";
        public const string GetReportRevenue = "api/grr";
        public const string GetReportDepositWithdraw = "api/grdw";

        public const string TrackingApkDownload = "api/tad";

        public const string GetFeatureConfig = "api/gfc";

        public const string GetInfoAppBySubChannel = "api/giabs";

        #endregion


        #region Member Admin

        public const string GetLevelConfig = "api/glc";
        public const string RegisterAdmin = "api/ra";
        public const string UpdateAdminProfile = "api/uap";

        #endregion

        #region Log
        public const string LogDeposit = "api/lgdeposit";
        public const string LogCashOut = "api/lgcasho";
        public const string ConfigCashOut = "api/cofcasho";
        public const string CreateCashOut = "api/crecasho";
        public const string GetLogTransferGold = "api/gltg";
        public const string GetLogNewRegisterUser = "api/glnru";
        public const string GetLogUserLogin = "api/glul";
        public const string GetLogUserPlayGame = "api/glpg";
        public const string GetTopNewRegisterUser = "api/gtnru";
        public const string LogProfit = "api/lgprofit";
        #endregion

        #region TransferGold

        public const string GetUserInfoTransferTo = "api/guitt";
        public const string GetTransferGoldConfig = "api/gtgc";
        public const string TransferUserGold = "api/tfug";

        #endregion

        #region event
        public const string TopEvent = "api/topevent";
        public const string TopEventLast = "api/topelast";
        #endregion
    }

    public class ApiClientApkMethodName
    {
        public const string BuildApk = "capi/bapk";
    }
}