using MyUtility.Extensions;

namespace BussinessObject.Models.ResourceEnum.Model
{
    public enum ResourceTypeEnum
    {
        Account,
        Common,
        Event,
        Game,
        Home,
        Item,
        Landing,
        Lather,
        Shared,
        Payment,
        TransferGold
    }

    public class MyResourceDetailModel
    {
        public MyResourceDetailModel(ResourceTypeEnum resourceName, string resourceKeyName,
            string resourceKeyNameMobile)
        {
            ResourceName = resourceName;
            ResourceKeyName = resourceKeyName;
            ResourceKeyNameMobile = resourceKeyNameMobile;
        }

        public string Language { get; set; }
        public ResourceTypeEnum ResourceName { get; set; }
        public string ResourceKeyName { get; set; }
        public string ResourceKeyNameMobile { get; set; }

        public string ToFormatResource(bool isMobile = false)
        {
            return ResourceName.Text() + "|" + (isMobile ? ResourceKeyNameMobile : ResourceKeyName);
        }
    }
}