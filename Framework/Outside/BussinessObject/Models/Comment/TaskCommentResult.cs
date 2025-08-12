using System;

namespace BussinessObject.Models.Comment
{
    /// <summary>
    /// Result class cho Ins_TaskComment_Create
    /// </summary>
    public class Ins_TaskComment_Create_Result
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public string Source { get; set; }
        public bool IsSystem { get; set; }
        public bool IsComment { get; set; }
        public string Type { get; set; }
        public string Attribute { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// Result class cho Ins_TaskComment_GetByTaskId
    /// </summary>
    public class Ins_TaskComment_GetByTaskId_Result
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string Source { get; set; }
        public string ObjectId { get; set; }
        public string UserId { get; set; }
        public bool? IsSystem { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int IsComment { get; set; }
    }

    /// <summary>
    /// Result class cho TotalCount của Ins_TaskComment_GetByTaskId
    /// </summary>
    public class Ins_TaskComment_GetByTaskId_TotalCount_Result
    {
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// Result class cho Mentions của Ins_TaskComment_GetByTaskId
    /// </summary>
    public class Ins_TaskComment_GetByTaskId_Mentions_Result
    {
        public int TaskCommentId { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Result class cho Ins_TaskComment_Delete
    /// </summary>
    public class Ins_TaskComment_Delete_Result
    {
        public int DeletedCommentId { get; set; }
        public int TaskId { get; set; }
        public string DeletedBy { get; set; }
        public DateTime DeletedAt { get; set; }
    }

    /// <summary>
    /// Result class cho Ins_TaskComment_Update
    /// </summary>
    public class Ins_TaskComment_Update_Result
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public string Source { get; set; }
        public bool IsSystem { get; set; }
        public bool IsComment { get; set; }
        public string Type { get; set; }
        public string Attribute { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsEdited { get; set; }
    }
} 