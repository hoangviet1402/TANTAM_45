using Newtonsoft.Json;

namespace TanTamApi.Models.Request
{
    /// <summary>
    ///     Base Request model
    ///     <para>Author: PhatVT</para>
    ///     <para>Created Date: 18/12/2014</para>
    /// </summary>
    public class ApiBaseRequest
    {
        public ApiBaseRequest()
        {
            ChannelId = 0;
            UserAgent = string.Empty;
        }

        public object Data { get; set; }

        [JsonProperty("s")] public string Sign { get; set; }

        [JsonProperty("av")] public string AppVersion { get; set; }

        [JsonProperty("uid")] public int UserId { get; set; }

        [JsonProperty("uidGa")] public int UserIdGame { get; set; }

        [JsonProperty("pw")] public string Password { get; set; }

        [JsonProperty("ps2")] public string Pass2 { get; set; }

        [JsonProperty("ip")] public string IpAddress { get; set; }

        [JsonProperty("ua")] public string UserAgent { get; set; }

        /// <summary>
        ///     2 - Android, 3 - iOS, 4 - web app
        /// </summary>
        [JsonProperty("pi")]
        public int PlatformId { get; set; }

        [JsonProperty("hi")] public string HardwareId { get; set; }

        [JsonProperty("IMEI")] public string Imei { get; set; }

        [JsonProperty("an")] public string MethodName { get; set; }

        [JsonProperty("ci")] public int ChannelId { get; set; }

        [JsonProperty("sci")] public int SubChannelId { get; set; }

        [JsonProperty("ai")] public string AppId { get; set; }

        [JsonProperty("bi")] public string BundleID { get; set; }

        [JsonProperty("l")] public string Lang { get; set; }

        [JsonProperty("pt")] public string PushToken { get; set; }

        /// <summary>
        ///     mã máy
        ///     <para>ví dụ: Samsung S2</para>
        /// </summary>
        [JsonProperty("mn")]
        public string ModelName { get; set; }

        /// <summary>
        ///     version hệ điều hành
        ///     <para>Ví dụ: iOS 12</para>
        /// </summary>
        [JsonProperty("mv")]
        public string ModelVersion { get; set; }

        [JsonProperty("utm_campaign")] public string utm_campaign { get; set; }


        [JsonProperty("utm_source")] public string utm_source { get; set; }

        [JsonProperty("ipl")] public string LocalIP { get; set; }

        [JsonProperty("coi")] public int CountryId { get; set; }

        [JsonProperty("co")] public string Country { get; set; }

        [JsonProperty("cos")] public string CountrySim { get; set; }

        [JsonProperty("coip")] public string CountryIp { get; set; }

        /// <summary>
        ///     mac wifi address (có thể rỗng)
        /// </summary>
        [JsonProperty("MFA")]
        public string MFA { get; set; }

        /// <summary>
        ///     fingerprint
        /// </summary>
        [JsonProperty("FGID")]
        public string FGID { get; set; }
    }
}