using System;
using TanTamApi.Models.Request;
using Newtonsoft.Json;

namespace TanTamApi.Models.MobileApi
{
    public class EventRequestModel : ApiBaseRequest
    {
        [JsonProperty("ari")] public int? ArticleId { get; set; }

        [JsonProperty("pn")] public int? PageNumber { get; set; }

        [JsonProperty("art")] public int? ArticleType { get; set; }
    }

    public class LoginMobileApiModel : ApiBaseRequest
    {
        [JsonProperty("nn")] public string UserName { get; set; }

        [JsonProperty("at")] public string AccessToken { get; set; }

        [JsonProperty("prn")] public string ProviderName { get; set; }
    }

    public class PlayBySocialModel : ApiBaseRequest
    {
        [JsonProperty("nn")] public string NickName { get; set; }

        [JsonProperty("dn")] public string DisplayName { get; set; }

        [JsonProperty("pi")] public string ProviderID { get; set; }

        [JsonProperty("ma")] public string Email { get; set; }

        [JsonProperty("at")] public string AccessToken { get; set; }

        [JsonProperty("fui")] public string FacebookUserID { get; set; }

        [JsonProperty("oid")] public int? OpenProviderID { get; set; }
    }

    public class ChangePassMobileApiModel : ApiBaseRequest
    {
        [JsonProperty("op")] public string OldPass { get; set; }

        [JsonProperty("np")] public string NewPass { get; set; }
    }

    public class RegisterApiModel : ApiBaseRequest
    {
        [JsonProperty("un")] public string UserName { get; set; }

        [JsonProperty("dn")] public string DisplayName { get; set; }

        [JsonProperty("cc")] public string Captchar { get; set; }

        [JsonProperty("adsid")] public string AdsID { get; set; }

        [JsonProperty("lkp")] public string LinkPersonal { get; set; }

        [JsonProperty("ph")] public string Phone { get; set; }

        [JsonProperty("em")] public string Email { get; set; }
    }

    public class GetUserInfoModel : ApiBaseRequest
    {
        [JsonProperty("suid")] public int SearchUserID { get; set; }

        [JsonProperty("uname")] public string SearchUserName { get; set; }
    }

    public class UpdateAdminProfileModel : ApiBaseRequest
    {
        [JsonProperty("un")] public string UserName { get; set; }

        [JsonProperty("dn")] public string DisplayName { get; set; }

        [JsonProperty("ph")] public string Phone { get; set; }

        [JsonProperty("em")] public string Email { get; set; }

        [JsonProperty("cc")] public string Captcha { get; set; }

        [JsonProperty("np")] public string NewPassword { get; set; }

        [JsonProperty("cp")] public string ConfirmPassword { get; set; }

        [JsonProperty("gd")] public bool Gender { get; set; }

        [JsonProperty("dob")] public DateTime DateOfBirth { get; set; }

        [JsonProperty("lkp")] public string LinkPersonal { get; set; }
    }

    public class CategorySearchModel : ApiBaseRequest
    {
        [JsonProperty("ari")] public int ArticleId { get; set; }

        [JsonProperty("ps")] public int PageSize { get; set; }

        [JsonProperty("pn")] public int PageNumber { get; set; }

        [JsonProperty("cti")] public int CategoryId { get; set; }

        [JsonProperty("art")] public int ArticleType { get; set; }
    }

    public class UploadImageModel : ApiBaseRequest
    {
        [JsonProperty("fn")] public string FileName { get; set; }

        [JsonProperty("atd")] public string AvatarData { get; set; }

        [JsonProperty("cim")] public string CoverImage { get; set; }

        [JsonProperty("url")] public string UlrCoverImage { get; set; }
    }

    public class AccountValidationResult
    {
        public int? KeyType { get; set; }
        public bool IsValid { get; set; }
    }
}