using Newtonsoft.Json;

namespace BussinessObject.Models.Comment
{
    /// <summary>
    /// Response model cho việc xóa comment
    /// </summary>
    public class DeleteCommentResponse
    {
        [JsonProperty("deleted_comment_id")]
        public int DeletedCommentId { get; set; }
        [JsonProperty("deleted_user_id")]
        public int DeletedUserId { get; set; }
        
        [JsonProperty("task_id")]
        public int TaskId { get; set; }
        
        [JsonProperty("deleted_by")]
        public int DeletedBy { get; set; }
        
        [JsonProperty("deleted_at")]
        public string DeletedAt { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
    }
} 