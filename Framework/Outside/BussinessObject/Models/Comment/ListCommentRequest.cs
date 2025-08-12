using Newtonsoft.Json;

namespace BussinessObject.Models.Comment
{
    public class ListCommentRequest
    {
        [JsonProperty("task_id")]
        public int TaskId { get; set; }
        
        [JsonProperty("source")]
        public string Source { get; set; }
        
        [JsonProperty("page")]
        public int Page { get; set; } = 1;
        
        [JsonProperty("per_page")]
        public int PerPage { get; set; } = 5;
    }
} 