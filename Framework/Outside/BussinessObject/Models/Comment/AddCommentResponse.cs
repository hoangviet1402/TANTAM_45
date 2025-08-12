using System.Collections.Generic;
using Newtonsoft.Json;

namespace BussinessObject.Models.Comment
{
    public class AddCommentResponse
    {
        [JsonProperty("comment_id")]
        public int CommentId { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty("created_by")]
        public int CreatedBy { get; set; }
        [JsonProperty("mention_ids")]
        public List<int> MentionIds { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("task_id")]
        public int TaskId { get; set; }
        [JsonProperty("attached_files")]
        public List<string> AttachedFiles { get; set; }
    }
}