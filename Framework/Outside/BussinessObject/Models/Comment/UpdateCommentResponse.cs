using Newtonsoft.Json;

namespace BussinessObject.Models.Comment
{
    /// <summary>
    /// Response model cho việc update comment
    /// </summary>
    public class UpdateCommentResponse
    {
        [JsonProperty("comment_id")]
        public int CommentId { get; set; }
        
        [JsonProperty("task_id")]
        public int TaskId { get; set; }
        
        [JsonProperty("content")]
        public string Content { get; set; }
        
        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }
        
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
        
        [JsonProperty("is_edited")]
        public bool IsEdited { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
    }
} 