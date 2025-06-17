using BussinessObject.Models.ResourceEnum.Model;

namespace BussinessObject.Models.ResourceEnum
{
    public partial class MyResourceManagement
    {
        private const ResourceTypeEnum ResourceGameName = ResourceTypeEnum.Game;

        public static MyResourceDetailModel VipLevelBlockRoomTextSilver => new MyResourceDetailModel(ResourceGameName,
            "VipLevelBlockRoomTextSilver", "VipLevelBlockRoomTextSilver");

        public static MyResourceDetailModel ListImageEmpty =>
            new MyResourceDetailModel(ResourceGameName, "ListImageEmpty", "ListImageEmpty");

        public static MyResourceDetailModel GuideNotExist =>
            new MyResourceDetailModel(ResourceGameName, "GuideNotExist", "GuideNotExist");

        public static MyResourceDetailModel GuideNoNew =>
            new MyResourceDetailModel(ResourceGameName, "GuideNoNew", "GuideNoNew");

        public static MyResourceDetailModel GiftIsWrong =>
            new MyResourceDetailModel(ResourceGameName, "GiftIsWrong", "GiftIsWrong");

        public static MyResourceDetailModel GetSKIpAndPortFail =>
            new MyResourceDetailModel(ResourceGameName, "GetSKIpAndPortFail", "GetSKIpAndPortFail");

        public static MyResourceDetailModel GetListSuccess =>
            new MyResourceDetailModel(ResourceGameName, "GetListSuccess", "GetListSuccess");

        public static MyResourceDetailModel GetLinkSuccess =>
            new MyResourceDetailModel(ResourceGameName, "GetLinkSuccess", "GetLinkSuccess");

        public static MyResourceDetailModel GetLinkFail =>
            new MyResourceDetailModel(ResourceGameName, "GetLinkFail", "GetLinkFail");

        public static MyResourceDetailModel GetGuideListSuccess =>
            new MyResourceDetailModel(ResourceGameName, "GetGuideListSuccess", "GetGuideListSuccess");

        public static MyResourceDetailModel GetConfigsSuccess =>
            new MyResourceDetailModel(ResourceGameName, "GetConfigsSuccess", "GetConfigsSuccess");

        public static MyResourceDetailModel GetBossAndGiftSuccess =>
            new MyResourceDetailModel(ResourceGameName, "GetBossAndGiftSuccess", "GetBossAndGiftSuccess");

        public static MyResourceDetailModel GameNotExistOrNotHasHtml5 => new MyResourceDetailModel(ResourceGameName,
            "GameNotExistOrNotHasHtml5", "GameNotExistOrNotHasHtml5");

        public static MyResourceDetailModel GameLevelBlockRoomTextLevel => new MyResourceDetailModel(ResourceGameName,
            "GameLevelBlockRoomTextLevel", "GameLevelBlockRoomTextLevel");
    }
}