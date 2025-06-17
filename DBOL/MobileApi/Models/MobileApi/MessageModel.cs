using System.Collections.Generic;
using Newtonsoft.Json;

namespace TanTamApi.Models.MobileApi
{
    public class MessageSummaryModel
    {
        [JsonProperty(PropertyName = "ttr", NullValueHandling = NullValueHandling.Ignore)]
        public long? TotalRows { get; set; }

        [JsonProperty(PropertyName = "tm", NullValueHandling = NullValueHandling.Ignore)]
        public int TotalMail { get; set; }

        [JsonProperty(PropertyName = "mld", NullValueHandling = NullValueHandling.Ignore)]
        public List<ListMessageModel> MessageListData { get; set; }
    }

    public class ListMessageModel
    {
        [JsonProperty(PropertyName = "mid", NullValueHandling = NullValueHandling.Ignore)]
        public int MessageId { get; set; }

        [JsonProperty(PropertyName = "sid", NullValueHandling = NullValueHandling.Ignore)]
        public int? SenderId { get; set; }

        [JsonProperty(PropertyName = "sna", NullValueHandling = NullValueHandling.Ignore)]
        public string SenderName { get; set; }

        [JsonProperty(PropertyName = "tit", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "sde", NullValueHandling = NullValueHandling.Ignore)]
        public string SendDate { get; set; }

        [JsonProperty(PropertyName = "isr", NullValueHandling = NullValueHandling.Ignore)]
        public int IsRead { get; set; }

        [JsonProperty(PropertyName = "scnt", NullValueHandling = NullValueHandling.Ignore)]
        public string SummaryContent { get; set; }
    }

    public class MessageDetailModel
    {
        [JsonProperty(PropertyName = "mid", NullValueHandling = NullValueHandling.Ignore)]
        public int MessageId { get; set; }

        [JsonProperty(PropertyName = "sid", NullValueHandling = NullValueHandling.Ignore)]
        public int? SenderId { get; set; }

        [JsonProperty(PropertyName = "sna", NullValueHandling = NullValueHandling.Ignore)]
        public string SenderName { get; set; }

        [JsonProperty(PropertyName = "tit", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "mec", NullValueHandling = NullValueHandling.Ignore)]
        public string MessageContent { get; set; }

        [JsonProperty(PropertyName = "sd", NullValueHandling = NullValueHandling.Ignore)]
        public string SendDate { get; set; }
    }
}