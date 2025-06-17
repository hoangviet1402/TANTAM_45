using BussinessObject.Models.ResourceEnum.Model;

namespace BussinessObject.Models.ResourceEnum
{
    public partial class MyResourceManagement
    {
        private const ResourceTypeEnum ResourceCommonName = ResourceTypeEnum.Common;

        public static MyResourceDetailModel ApiStatusInvalidDataInput => new MyResourceDetailModel(ResourceCommonName,
            "ApiStatusInvalidDataInput", "ApiStatusInvalidDataInput");

        public static MyResourceDetailModel ApiStatusProfileEmailHasExist =>
            new MyResourceDetailModel(ResourceCommonName, "ApiStatusProfileEmailHasExist",
                "ApiStatusProfileEmailHasExist");

        public static MyResourceDetailModel TurnsIsExpired =>
            new MyResourceDetailModel(ResourceCommonName, "TurnsIsExpired", "TurnsIsExpired");

        public static MyResourceDetailModel TurnsInvalid =>
            new MyResourceDetailModel(ResourceCommonName, "TurnsInvalid", "TurnsInvalid");

        public static MyResourceDetailModel SystemMaintaining =>
            new MyResourceDetailModel(ResourceCommonName, "SystemMaintaining", "SystemMaintaining");

        public static MyResourceDetailModel SystemErrorContactAdmin => new MyResourceDetailModel(ResourceCommonName,
            "SystemErrorContactAdmin", "SystemErrorContactAdmin");

        public static MyResourceDetailModel SystemError =>
            new MyResourceDetailModel(ResourceCommonName, "SystemError", "SystemError");

        public static MyResourceDetailModel NotLoginOrBeAnother =>
            new MyResourceDetailModel(ResourceCommonName, "NotLoginOrBeAnother", "NotLoginOrBeAnother");

        public static MyResourceDetailModel MaxImageValidate =>
            new MyResourceDetailModel(ResourceCommonName, "MaxImageValidate", "MaxImageValidate");

        public static MyResourceDetailModel InputMoneyFail =>
            new MyResourceDetailModel(ResourceCommonName, "InputMoneyFail", "InputMoneyFail");

        public static MyResourceDetailModel InformationInvalid =>
            new MyResourceDetailModel(ResourceCommonName, "InformationInvalid", "InformationInvalid");

        public static MyResourceDetailModel ImageValidate =>
            new MyResourceDetailModel(ResourceCommonName, "ImageValidate", "ImageValidate");

        public static MyResourceDetailModel GetDataSuccess =>
            new MyResourceDetailModel(ResourceCommonName, "GetDataSuccess", "GetDataSuccess");

        public static MyResourceDetailModel GetDataFailNoDataRequest => new MyResourceDetailModel(ResourceCommonName,
            "GetDataFailNoDataRequest", "GetDataFailNoDataRequest");

        public static MyResourceDetailModel CreatedSuccess =>
            new MyResourceDetailModel(ResourceCommonName, "CreatedSuccess", "CreatedSuccess");

        public static MyResourceDetailModel ConfigNotExists =>
            new MyResourceDetailModel(ResourceCommonName, "ConfigNotExists", "ConfigNotExists");
    }
}