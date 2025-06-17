using Newtonsoft.Json;

namespace TanTamApi.Models
{
    /// <summary>
    ///     Định nghĩa đối tượng user lấy từ Open Auth
    ///     <para>Author: PhatVT</para>
    ///     <para>Created Date: 23/12/2014</para>
    /// </summary>
    public class GoogleUserDto
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("email")] public string Email { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("given_name")] public string GivenName { get; set; }

        [JsonProperty("family_name")] public string FamilyName { get; set; }

        [JsonProperty("link")] public string Link { get; set; }

        [JsonProperty("picture")] public string Picture { get; set; }

        [JsonProperty("gender")] public string Gender { get; set; }
    }

    public class GoogleUser2Dto
    {
        /// <summary>
        ///     Giá trị nhận dạng cho người dùng, duy nhất trong tất cả Tài khoản Google và không bao giờ bị sử dụng lại. Một Tài
        ///     khoản Google có thể có nhiều địa chỉ email tại những thời điểm khác nhau, nhưng giá trị sub không bao giờ thay đổi.
        ///     Hãy dùng sub trong ứng dụng của bạn làm khoá nhận dạng duy nhất cho người dùng. Độ dài tối đa là 255 ký tự ASCII
        ///     phân biệt chữ hoa chữ thường.
        /// </summary>
        [JsonProperty("sub")]
        public string Id { get; set; }

        [JsonProperty("email")] public string Email { get; set; }

        [JsonProperty("given_name")] public string GivenName { get; set; }

        [JsonProperty("family_name")] public string FamilyName { get; set; }
    }
}