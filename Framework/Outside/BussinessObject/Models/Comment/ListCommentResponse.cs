using System.Collections.Generic;
using Newtonsoft.Json;

namespace BussinessObject.Models.Comment
{
    public class ListCommentResponse
    {
        [JsonProperty("items")]
        public List<CommentItem> Items { get; set; }
        
        [JsonProperty("meta")]
        public MetaInfo Meta { get; set; }
    }

    public class CommentItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("content")]
        public string Content { get; set; }
        
        [JsonProperty("source")]
        public string Source { get; set; }
        
        [JsonProperty("task_id")]
        public string TaskId { get; set; }
        
        [JsonProperty("user_obj")]
        public UserObject UserObj { get; set; }
        
        [JsonProperty("is_system")]
        public bool? IsSystem { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
        
        [JsonProperty("is_comment")]
        public int IsComment { get; set; }
        [JsonProperty("is_edited")]
        public bool IsEdited { get; set; }
        
        [JsonProperty("mention_users")]
        public List<MentionUser> MentionUsers { get; set; } = new List<MentionUser>();
        
        [JsonProperty("file_uploads")]
        public List<FileUpload> FileUploads { get; set; } = new List<FileUpload>();
    }

    public class UserObject
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("username")]
        public string Username { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class MentionUser
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        
        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }
        
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("identification", NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeCode { get; set; }
    }

    public class FileUpload
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("path_date")]
        public string PathDate { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("extension")]
        public string Extension { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("user_obj")]
        public UserObject UserObj { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class MetaInfo
    {
        [JsonProperty("total")]
        public int Total { get; set; }
        
        [JsonProperty("count")]
        public int Count { get; set; }
        
        [JsonProperty("per_page")]
        public int PerPage { get; set; }
        
        [JsonProperty("current_page")]
        public int CurrentPage { get; set; }
        
        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
    }
}