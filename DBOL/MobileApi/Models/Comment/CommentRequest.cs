using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TanTamApi.Models.Comment
{
    /// <summary>
    /// Request model cho việc tạo comment mới
    /// </summary>
    public class CreateCommentRequest
    {
        /// <summary>
        /// Nội dung comment
        /// </summary>
        [Required(ErrorMessage = "Nội dung comment không được để trống")]
        [StringLength(1000, ErrorMessage = "Nội dung comment không được vượt quá 1000 ký tự")]
        public string Content { get; set; }

        /// <summary>
        /// ID đối tượng được comment (task, employee, etc.)
        /// </summary>
        public int? TargetId { get; set; }

        /// <summary>
        /// Loại đối tượng được comment (task, employee, etc.)
        /// </summary>
        public string TargetType { get; set; }

        /// <summary>
        /// ID comment cha (nếu là reply)
        /// </summary>
        public int? ParentCommentId { get; set; }
    }

    /// <summary>
    /// Request model cho việc cập nhật comment
    /// </summary>
    public class UpdateCommentRequest
    {
        /// <summary>
        /// ID comment cần cập nhật
        /// </summary>
        [Required(ErrorMessage = "ID comment không được để trống")]
        public int CommentId { get; set; }

        /// <summary>
        /// Nội dung comment mới
        /// </summary>
        [Required(ErrorMessage = "Nội dung comment không được để trống")]
        [StringLength(1000, ErrorMessage = "Nội dung comment không được vượt quá 1000 ký tự")]
        public string Content { get; set; }
    }

    /// <summary>
    /// Request model cho việc lấy danh sách comments
    /// </summary>
    public class GetCommentsRequest
    {
        /// <summary>
        /// Trang hiện tại (mặc định là 1)
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Số lượng item trên trang (mặc định là 20)
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// ID đối tượng được comment
        /// </summary>
        public int? TargetId { get; set; }

        /// <summary>
        /// Loại đối tượng được comment
        /// </summary>
        public string TargetType { get; set; }

        /// <summary>
        /// Từ khóa tìm kiếm
        /// </summary>
        public string Keyword { get; set; }
    }

    /// <summary>
    /// Request model cho việc xóa comment
    /// </summary>
    public class DeleteCommentRequest
    {
        /// <summary>
        /// ID comment cần xóa
        /// </summary>
        [Required(ErrorMessage = "ID comment không được để trống")]
        public int CommentId { get; set; }
    }
}