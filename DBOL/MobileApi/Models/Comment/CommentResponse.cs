using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TanTamApi.Models.Comment
{
    /// <summary>
    /// Response model cho comment
    /// </summary>
    public class CommentResponse
    {
        /// <summary>
        /// ID comment
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nội dung comment
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// ID người tạo comment
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Tên người tạo comment
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Avatar người tạo comment
        /// </summary>
        public string UserAvatar { get; set; }

        /// <summary>
        /// ID đối tượng được comment
        /// </summary>
        public int? TargetId { get; set; }

        /// <summary>
        /// Loại đối tượng được comment
        /// </summary>
        public string TargetType { get; set; }

        /// <summary>
        /// ID comment cha (nếu là reply)
        /// </summary>
        public int? ParentCommentId { get; set; }

        /// <summary>
        /// Thời gian tạo comment
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian cập nhật comment
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Trạng thái comment (active, deleted, etc.)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Danh sách reply comments
        /// </summary>
        public List<CommentResponse> Replies { get; set; } = new List<CommentResponse>();

        /// <summary>
        /// Số lượng likes
        /// </summary>
        public int LikeCount { get; set; }

        /// <summary>
        /// Người dùng hiện tại đã like comment này chưa
        /// </summary>
        public bool IsLiked { get; set; }
    }

    /// <summary>
    /// Response model cho việc tạo comment
    /// </summary>
    public class CreateCommentResponse
    {
        /// <summary>
        /// ID comment đã tạo
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nội dung comment
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thông báo kết quả
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// Response model cho danh sách comments
    /// </summary>
    public class GetCommentsResponse
    {
        /// <summary>
        /// Danh sách comments
        /// </summary>
        public List<CommentResponse> Comments { get; set; } = new List<CommentResponse>();

        /// <summary>
        /// Tổng số comments
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Số lượng item trên trang
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Có trang tiếp theo không
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Có trang trước không
        /// </summary>
        public bool HasPreviousPage { get; set; }
    }

    /// <summary>
    /// Response model cho việc cập nhật comment
    /// </summary>
    public class UpdateCommentResponse
    {
        /// <summary>
        /// ID comment đã cập nhật
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nội dung comment mới
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Thông báo kết quả
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// Response model cho việc xóa comment
    /// </summary>
    public class DeleteCommentResponse
    {
        /// <summary>
        /// ID comment đã xóa
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Thông báo kết quả
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Thời gian xóa
        /// </summary>
        public DateTime DeletedAt { get; set; }
    }
}