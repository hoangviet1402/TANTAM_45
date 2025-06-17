using BussinessObject.Models.ResourceEnum.Model;

namespace BussinessObject.Models.ResourceEnum
{
    public partial class MyResourceManagement
    {
        private const ResourceTypeEnum ResourceItemName = ResourceTypeEnum.Item;

        public static MyResourceDetailModel ItemGroupNameOther =>
            new MyResourceDetailModel(ResourceItemName, "ItemGroupNameOther", "ItemGroupNameOther");

        public static MyResourceDetailModel ItemGroupNameBullet =>
            new MyResourceDetailModel(ResourceItemName, "ItemGroupNameBullet", "ItemGroupNameBullet");

        public static MyResourceDetailModel ItemGroupNameSkill =>
            new MyResourceDetailModel(ResourceItemName, "ItemGroupNameSkill", "ItemGroupNameSkill");

        public static MyResourceDetailModel ItemGroupNameGun =>
            new MyResourceDetailModel(ResourceItemName, "ItemGroupNameGun", "ItemGroupNameGun");

        public static MyResourceDetailModel ItemGroupNameEffect =>
            new MyResourceDetailModel(ResourceItemName, "ItemGroupNameEffect", "ItemGroupNameEffect");

        public static MyResourceDetailModel ItemCannotActive =>
            new MyResourceDetailModel(ResourceItemName, "ItemCannotActive", "ItemCannotActive");

        public static MyResourceDetailModel ItemNoData =>
            new MyResourceDetailModel(ResourceItemName, "ItemNoData", "ItemNoData");
    }
}