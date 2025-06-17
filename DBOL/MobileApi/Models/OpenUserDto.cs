using System;
using BussinessObject.Enum.Info;

namespace TanTamApi.Models
{
    /// <summary>
    ///     Định nghĩa đối tượng user lấy từ Open Auth
    ///     <para>Author: PhatVT</para>
    ///     <para>Created Date: 23/12/2014</para>
    /// </summary>
    [Serializable]
    public class OpenUserDto
    {
        public string Email { get; set; }

        public string EmailId { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public OpenProviderIdEnum OpenProvider { get; set; }
        public string UserProviderId { get; set; }
    }

    [Serializable]
    public class FacebookOpenUser
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }
}