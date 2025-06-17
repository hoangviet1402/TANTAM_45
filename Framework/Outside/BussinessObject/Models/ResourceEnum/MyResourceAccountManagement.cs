using BussinessObject.Models.ResourceEnum.Model;

namespace BussinessObject.Models.ResourceEnum
{
    public partial class MyResourceManagement
    {
        private const ResourceTypeEnum ResourceAccountName = ResourceTypeEnum.Account;

        public static MyResourceDetailModel DisplayNameDoDai =>
            new MyResourceDetailModel(ResourceAccountName, "DisplayNameDoDai", "DisplayNameDoDai");

        public static MyResourceDetailModel DisplayNameKhoangTrang => new MyResourceDetailModel(ResourceAccountName,
            "DisplayNameKhoangTrang", "DisplayNameKhoangTrang");

        public static MyResourceDetailModel DisplayNameNotCorrect =>
            new MyResourceDetailModel(ResourceAccountName, "DisplayNameNotCorrect", "DisplayNameNotCorrect");

        public static MyResourceDetailModel DisplayNameNotCorrect1 => new MyResourceDetailModel(ResourceAccountName,
            "DisplayNameNotCorrect1", "DisplayNameNotCorrect1");

        public static MyResourceDetailModel DisplayNameNull =>
            new MyResourceDetailModel(ResourceAccountName, "DisplayNameNull", "DisplayNameNull");

        public static MyResourceDetailModel ExistedAccount =>
            new MyResourceDetailModel(ResourceAccountName, "ExistedAccount", "ExistedAccount");

        public static MyResourceDetailModel Failed =>
            new MyResourceDetailModel(ResourceAccountName, "Failed", "Failed");

        public static MyResourceDetailModel GetAvatarSuccess =>
            new MyResourceDetailModel(ResourceAccountName, "GetAvatarSuccess", "GetAvatarSuccess");

        public static MyResourceDetailModel GetMailSuccess =>
            new MyResourceDetailModel(ResourceAccountName, "GetMailSuccess", "GetMailSuccess");

        public static MyResourceDetailModel GetUserOnlineSuccess =>
            new MyResourceDetailModel(ResourceAccountName, "GetUserOnlineSuccess", "GetUserOnlineSuccess");

        public static MyResourceDetailModel HistoriCoinBanCaSuccess => new MyResourceDetailModel(ResourceAccountName,
            "HistoriCoinBanCaSuccess", "HistoriCoinBanCaSuccess");

        public static MyResourceDetailModel ImageLength =>
            new MyResourceDetailModel(ResourceAccountName, "imageLength", "imageLength");

        public static MyResourceDetailModel InvalidDisplayName =>
            new MyResourceDetailModel(ResourceAccountName, "InvalidDisplayName", "InvalidDisplayName");

        public static MyResourceDetailModel InvalidDisplayName2 =>
            new MyResourceDetailModel(ResourceAccountName, "InvalidDisplayName2", "InvalidDisplayName2");

        public static MyResourceDetailModel InvalidNickName =>
            new MyResourceDetailModel(ResourceAccountName, "InvalidNickName", "InvalidNickName");

        public static MyResourceDetailModel IsValidExtension =>
            new MyResourceDetailModel(ResourceAccountName, "isValidExtension", "isValidExtension");

        public static MyResourceDetailModel LoginFailed =>
            new MyResourceDetailModel(ResourceAccountName, "LoginFailed", "LoginFailed");

        public static MyResourceDetailModel LoginSuccess =>
            new MyResourceDetailModel(ResourceAccountName, "LoginSuccess", "LoginSuccess");

        public static MyResourceDetailModel NonExistsFaceAccount =>
            new MyResourceDetailModel(ResourceAccountName, "NonExistsFaceAccount", "NonExistsFaceAccount");

        public static MyResourceDetailModel NotHaveMail =>
            new MyResourceDetailModel(ResourceAccountName, "NotHaveMail", "NotHaveMail");

        public static MyResourceDetailModel OAuthLoginError =>
            new MyResourceDetailModel(ResourceAccountName, "OAuthLoginError", "OAuthLoginError");

        public static MyResourceDetailModel OAuthLoginError1 =>
            new MyResourceDetailModel(ResourceAccountName, "OAuthLoginError1", "OAuthLoginError1");

        public static MyResourceDetailModel AccountSuccess =>
            new MyResourceDetailModel(ResourceAccountName, "Success", "Success");

        public static MyResourceDetailModel UpdateFail =>
            new MyResourceDetailModel(ResourceAccountName, "UpdateFail", "UpdateFail");

        public static MyResourceDetailModel UpdateSuccessAddCoin =>
            new MyResourceDetailModel(ResourceAccountName, "UpdateSuccessAddCoin", "UpdateSuccessAddCoin");

        public static MyResourceDetailModel UserAndPassError =>
            new MyResourceDetailModel(ResourceAccountName, "UserAndPassError", "UserAndPassError");

        public static MyResourceDetailModel UserInvalit =>
            new MyResourceDetailModel(ResourceAccountName, "UserInvalit", "UserInvalit");

        public static MyResourceDetailModel UserNameHaveEmpty =>
            new MyResourceDetailModel(ResourceAccountName, "UserNameHaveEmpty", "UserNameHaveEmpty");

        public static MyResourceDetailModel UserNameInvalit =>
            new MyResourceDetailModel(ResourceAccountName, "UserNameInvalit", "UserNameInvalit");

        public static MyResourceDetailModel UserNameInvalit2 =>
            new MyResourceDetailModel(ResourceAccountName, "UserNameInvalit2", "UserNameInvalit2");

        public static MyResourceDetailModel UserNameInvalit3 =>
            new MyResourceDetailModel(ResourceAccountName, "UserNameInvalit3", "UserNameInvalit3");

        public static MyResourceDetailModel UserNameInvalit4 =>
            new MyResourceDetailModel(ResourceAccountName, "UserNameInvalit4", "UserNameInvalit4");

        public static MyResourceDetailModel UserNameLenght =>
            new MyResourceDetailModel(ResourceAccountName, "UserNameLenght", "UserNameLenght");

        public static MyResourceDetailModel UserNameNull =>
            new MyResourceDetailModel(ResourceAccountName, "UserNameNull", "UserNameNull");

        public static MyResourceDetailModel AccountNotFound => new MyResourceDetailModel(ResourceAccountName,
            "OpenAuthConfirmLogin_NotExistAccount", "OpenAuthConfirmLogin_NotExistAccount");

        public static MyResourceDetailModel ValidationLogin =>
            new MyResourceDetailModel(ResourceAccountName, "ValidationLogin", "ValidationLogin");

        public static MyResourceDetailModel ValidationLogin15 =>
            new MyResourceDetailModel(ResourceAccountName, "ValidationLogin15", "ValidationLogin15");

        public static MyResourceDetailModel ValidationPassword =>
            new MyResourceDetailModel(ResourceAccountName, "ValidationPassword", "ValidationPassword");

        public static MyResourceDetailModel ValidationPassword2 =>
            new MyResourceDetailModel(ResourceAccountName, "ValidationPassword2", "ValidationPassword2");

        public static MyResourceDetailModel ValidationPassword3 =>
            new MyResourceDetailModel(ResourceAccountName, "ValidationPassword3", "ValidationPassword3");

        public static MyResourceDetailModel ValidationPassword4 =>
            new MyResourceDetailModel(ResourceAccountName, "ValidationPassword4", "ValidationPassword4");

        public static MyResourceDetailModel RegisterCaptchaInvalid => new MyResourceDetailModel(ResourceAccountName,
            "RegisterCaptchaInvalid", "RegisterCaptchaInvalid");

        public static MyResourceDetailModel RegisterCaptchaNotExists => new MyResourceDetailModel(ResourceAccountName,
            "RegisterCaptchaNotExists", "RegisterCaptchaNotExists");

        public static MyResourceDetailModel RegisterFailed =>
            new MyResourceDetailModel(ResourceAccountName, "RegisterFailed", "RegisterFailed");

        public static MyResourceDetailModel RegisterSuccess =>
            new MyResourceDetailModel(ResourceAccountName, "RegisterSuccess", "RegisterSuccess");
    }
}