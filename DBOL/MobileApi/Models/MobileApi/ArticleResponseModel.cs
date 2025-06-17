using Newtonsoft.Json;

namespace TanTamApi.Models.MobileApi
{
    public class ArticleResponseModel
    {
        [JsonProperty("ari", NullValueHandling = NullValueHandling.Ignore)]
        public int ArticleID { get; set; }

        [JsonProperty("ob", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsOpenBrowser { get; set; }

        [JsonProperty("ti", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("li", NullValueHandling = NullValueHandling.Ignore)]
        public string Link { get; set; }

        [JsonProperty("lii", NullValueHandling = NullValueHandling.Ignore)]
        public string LinkImage { get; set; }

        [JsonProperty("bo", NullValueHandling = NullValueHandling.Ignore)]
        public string Body { get; set; }

        [JsonProperty("cs", NullValueHandling = NullValueHandling.Ignore)]
        public string CheckSum { get; set; }
    }
}