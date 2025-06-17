using System.ComponentModel;

namespace TanTamApi.Enum
{
    /// <summary>
    ///     Định nghĩa mã đăng nhập bằng OAuth
    ///     <para>Author: PhatVT</para>
    ///     <para>Create Date: 23/12/2014</para>
    /// </summary>
    public enum OAuthCode
    {
        [Description("Thành công")] Success = 0,

        [Description("Thất bại")] Failed = 1,

        [Description("Tài khoản mới")] NewAccount = 2,

        [Description("Mã bảo vệ bằng SMS")] SMS = 3,

        [Description("Mã bảo vệ")] SecureCode = 4,

        [Description("User không active")] InactiveUser = 5
    }

    public static class LoginErrorEnum
    {
        public enum OAuthLoginError
        {
            [Description("Thành công")] SUCCESS,

            [Description("Thất bại")] ERROR,

            [Description("Không thể lấy thông tin tài khoản")]
            PARSE_USER_FAIL,

            [Description("Đăng nhập Facebook đã bị khóa")]
            FACEBOOK_DISABLE,

            [Description("Không thể lấy thông tin tài khoản Facebook")]
            FACEBOOK_ACESSTOKEN_EXPIRE,

            [Description("Không thể lấy thông tin tài khoản Google")]
            GOOGLE_ACESSTOKEN_EXPIRE,

            [Description("Không thể lấy thông tin tài khoản Yahoo")]
            YAHOO_ACESSTOKEN_EXPIRE,

            [Description("Không thể lấy thông tin tài khoản")]
            TOKEN_EMPTY
        }
    }
}