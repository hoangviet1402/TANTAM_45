using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BussinessObject.Models.Comment
{
    /// <summary>
    /// Request model cho việc update comment
    /// </summary>
    public class UpdateCommentRequest
    {
        [JsonProperty("comment_id")]
        [Required(ErrorMessage = "CommentId không được để trống")]
        public int CommentId { get; set; }
        
        [JsonProperty("content")]
        [Required(ErrorMessage = "Nội dung comment không được để trống")]
        [StringLength(1000, ErrorMessage = "Nội dung comment không được vượt quá 1000 ký tự")]
        public string Content { get; set; }
    }
} 