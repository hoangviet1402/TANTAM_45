using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Newtonsoft.Json;

namespace BussinessObject.Models.Comment
{
    public class AddCommentRequest
    {
        [JsonProperty("mention_ids")]
        public List<int> MentionIds { get; set; }
        
        [JsonProperty("source")]
        public string Source { get; set; }
        
        [JsonProperty("task_id")]
        public int TaskId { get; set; }
        
        [JsonProperty("content")]
        public string Content { get; set; }
        
        [JsonProperty("files")]
        public List<HttpPostedFileBase> Files { get; set; }
        
        /// <summary>
        /// Allowed file extensions: .doc, .docx, .xls, .xlsx, .pdf, .png, .jpeg, .jpg
        /// Maximum file size: 10MB per file
        /// Maximum files: 10 files per request
        /// </summary>
        public AddCommentRequest()
        {
            MentionIds = new List<int>();
            Files = new List<HttpPostedFileBase>();
        }
    }
}