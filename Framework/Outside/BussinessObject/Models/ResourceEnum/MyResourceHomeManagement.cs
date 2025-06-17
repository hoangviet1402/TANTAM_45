using BussinessObject.Models.ResourceEnum.Model;

namespace BussinessObject.Models.ResourceEnum
{
    public partial class MyResourceManagement
    {
        private const ResourceTypeEnum ResourceHomeName = ResourceTypeEnum.Home;

        public static MyResourceDetailModel HomeServerMaintainance =>
            new MyResourceDetailModel(ResourceHomeName, "HomeServerMaintainance", "HomeServerMaintainance");

        public static MyResourceDetailModel OfflineMessageTitleMessage => new MyResourceDetailModel(ResourceHomeName,
            "OfflineMessageTitleMessage", "OfflineMessageTitleMessage");
    }
}