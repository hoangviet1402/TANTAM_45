using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BussinessObject.Enum;
using BussinessObject.Helper;
using BussinessObject.Models.Comment;
using DataAccess;
using EntitiesObject.Entities.TanTamEntities;
using Logger;
using MyUtility.Extensions;
using Newtonsoft.Json;

namespace BussinessObject.Bo.TanTamBo
{
    /// <summary>
    /// Business Object cho quản lý Comment
    /// </summary>
    public class CommentBo : BaseBo<DBNull>
    {
        public CommentBo()
            : base(DaoFactory.Comment) { }

        #region Comment Management

        /// <summary>
        /// Tạo TaskComment mới với file attachments
        /// </summary>
        /// <param name="request">Request chứa thông tin comment</param>
        /// <param name="userId">ID người tạo comment</param>
        /// <returns>Response chứa thông tin comment đã tạo</returns>
        public AddCommentResponse CreateTaskComment(AddCommentRequest request, int userId)
        {
            try
            {
                // Validate files if any
                var uploadedFiles = new List<string>();
                if (request.Files != null && request.Files.Any())
                {
                    var validationResult = FileUploadHelper.ValidateCommentFiles(request.Files);
                    if (!validationResult.IsValid)
                    {
                        throw new ArgumentException(string.Join("; ", validationResult.ErrorMessages));
                    }
                    
                    // Process file uploads
                    uploadedFiles = ProcessFileUploads(request.Files, request.TaskId, userId);
                }

                // Create comment in database
                var result = DaoFactory.Comment.CreateTaskComment(
                    request.TaskId, 
                    userId, 
                    request.Content, 
                    request.Source ?? "task"
                );

                // Add file attachments to database if any
                if (request.Files != null && request.Files.Any())
                {
                    for (int i = 0; i < request.Files.Count && i < uploadedFiles.Count; i++)
                    {
                        var file = request.Files[i];
                        var filePath = uploadedFiles[i];
                        var fileName = Path.GetFileName(file.FileName);
                        var extension = Path.GetExtension(file.FileName).Replace(".", "");
                        var pathDate = DateTime.Now.ToString("yyyy-MM-dd");
                        
                        DaoFactory.Comment.AddTaskCommentAttachment(
                            result.Id.Value, 
                            fileName, 
                            filePath, 
                            pathDate, 
                            userId, 
                            extension, 
                            file.ContentLength
                        );
                    }
                }

                // Add mentions if any
                if (request.MentionIds != null && request.MentionIds.Any())
                {
                    foreach (var mentionId in request.MentionIds)
                    {
                        DaoFactory.Comment.AddTaskCommentMention(result.Id.Value, mentionId, null);
                    }
                }

                return new AddCommentResponse
                {
                    CommentId = result.Id.Value,
                    Content = request.Content,
                    CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    CreatedBy = userId,
                    MentionIds = request.MentionIds ?? new List<int>(),
                    Source = request.Source ?? "task",
                    TaskId = request.TaskId,
                    AttachedFiles = uploadedFiles
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CommentBo.CreateTaskComment - Error: {0}", ex);
                throw new Exception($"Failed to create comment: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// Xử lý upload files và trả về danh sách đường dẫn file
        /// </summary>
        /// <param name="files">Danh sách files cần upload</param>
        /// <param name="taskId">ID của task</param>
        /// <param name="userId">ID của user</param>
        /// <returns>Danh sách đường dẫn file đã upload</returns>
        private List<string> ProcessFileUploads(List<HttpPostedFileBase> files, int taskId, int userId)
        {
            var uploadedFiles = new List<string>();
            var uploadDirectory = GetUploadDirectory(taskId);
            
            // Ensure upload directory exists
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }
            
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    // Generate unique filename with random string to avoid conflicts
                    var randomString = Guid.NewGuid().ToString("N").Substring(0, 8);
                    var fileName = $"{DateTime.Now:yyyyMMddHHmmss}_{randomString}_{userId}_{Path.GetFileName(file.FileName)}";
                    var filePath = Path.Combine(uploadDirectory, fileName);
                    
                    // Save file to disk
                    file.SaveAs(filePath);
                    
                    // Store relative path for database
                    var relativePath = Path.Combine("uploads", "comments", taskId.ToString(), fileName);
                    uploadedFiles.Add(relativePath);
                }
            }
            
            return uploadedFiles;
        }
        
        /// <summary>
        /// Lấy thư mục upload cho task
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <returns>Đường dẫn thư mục upload</returns>
        private string GetUploadDirectory(int taskId)
        {
            var baseUploadPath = HttpContext.Current?.Server?.MapPath("~/uploads/comments") ?? 
                                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads", "comments");
            return Path.Combine(baseUploadPath, taskId.ToString());
        }

        /// <summary>
        /// Lấy danh sách comments theo task_id và source
        /// </summary>
        /// <param name="request">Request chứa task_id và source</param>
        /// <returns>Response chứa danh sách comments</returns>
        public ListCommentResponse GetTaskComments(ListCommentRequest request)
        {
            var result = new ListCommentResponse();
            try
            {
                // Lấy comments từ database
                var comments = DaoFactory.Comment.GetByTaskId(request.TaskId, request.Source ?? "task", request.Page, request.PerPage);
                
                // Lấy mentions và file uploads cho tất cả comments bằng batch methods (tối ưu N+1 query)
                var commentIds = comments.Select(c => c.Id).ToList();
                var mentionsByCommentId = DaoFactory.Comment.GetMentionsByCommentIds(commentIds);
                var fileUploadsByCommentId = DaoFactory.Comment.GetFileUploadsByCommentIds(commentIds);

                // Map data từ database sang response models
                var commentItems = new List<CommentItem>();
                
                foreach (var comment in comments)
                {
                    var commentItem = new CommentItem
                    {
                        Id = comment.Id.ToString(),
                        Content = comment.Content,
                        Source = comment.Source,
                        TaskId = comment.TaskId.ToString(),
                        UserObj = new UserObject
                        {
                            Id = comment.UserId,
                            Username = comment.PhoneFull,
                            Name = comment.FullName
                        },
                        IsSystem = comment.IsSystem,
                        Type = comment.Type,
                        CreatedAt = comment.CreatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        UpdatedAt = comment.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        IsComment = comment.IsComment == true ? 1 : 0,
                        IsEdited = comment.IsEdited ?? false,
                        MentionUsers = new List<MentionUser>(),
                        FileUploads = new List<FileUpload>()
                    };

                    // Lấy mentions cho comment này từ dictionary
                    if (mentionsByCommentId.ContainsKey(comment.Id))
                    {
                        var commentMentions = mentionsByCommentId[comment.Id];
                        foreach (var mention in commentMentions)
                        {
                            commentItem.MentionUsers.Add(new MentionUser
                            {
                                Id = mention.UserId?.ToString(),
                                Username = mention.PhoneFull,
                                Name = mention.Name,
                                EmployeeCode = mention.EmployeeCode
                            });
                        }
                    }

                    // Lấy file uploads cho comment này từ dictionary
                    if (fileUploadsByCommentId.ContainsKey(comment.Id))
                    {
                        var commentFileUploads = fileUploadsByCommentId[comment.Id];
                        foreach (var fileUpload in commentFileUploads)
                        {
                            // Tạo URL download cho file
                            var baseUrl = HttpContext.Current?.Request?.Url?.GetLeftPart(UriPartial.Authority) ?? "";
                            var downloadUrl = $"{baseUrl}/{fileUpload.Path?.Replace("\\", "/")}";
                            
                            commentItem.FileUploads.Add(new FileUpload
                            {
                                Id = fileUpload.Id.ToString(),
                                Name = fileUpload.Name,
                                Path = fileUpload.Path,
                                PathDate = fileUpload.PathDate,
                                UserId = fileUpload.UserId.ToString(),
                                Extension = fileUpload.Extension,
                                Size = fileUpload.Size,
                                CreatedAt = fileUpload.CreatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                UserObj = new UserObject
                                {
                                    Id = fileUpload.UserId.ToString(),
                                    Username = fileUpload.Username,
                                    Name = fileUpload.FullName
                                },
                                Url = $"{baseUrl}/api/comment/download/{fileUpload.Id}"
                            });
                        }
                    }

                    commentItems.Add(commentItem);
                }

                // Tính toán meta information - Lấy TotalCount từ stored procedure result
                var totalCount = comments.Count > 0 ? comments.FirstOrDefault()?.TotalCount ?? 0 : 0;
                var totalPages = (int)Math.Ceiling((double)totalCount / request.PerPage);

                return new ListCommentResponse
                {
                    Items = commentItems,
                    Meta = new MetaInfo
                    {
                        Total = totalCount,
                        Count = commentItems.Count,
                        PerPage = request.PerPage,
                        CurrentPage = request.Page,
                        TotalPages = totalPages
                    }
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CommentBo.GetTaskComments - Error: {0}", ex);
                return new ListCommentResponse
                {
                    Items = new List<CommentItem>(),
                    Meta = new MetaInfo
                    {
                        Total = 0,
                        Count = 0,
                        PerPage = request.PerPage,
                        CurrentPage = request.Page,
                        TotalPages = 0
                    }
                };
            }
        }

        /// <summary>
        /// Xóa comment theo ID
        /// </summary>
        /// <param name="commentId">ID của comment cần xóa</param>
        /// <param name="userId">ID của user thực hiện xóa</param>
        /// <returns>Response chứa thông tin comment đã xóa</returns>
        public DeleteCommentResponse DeleteTaskComment(int commentId, int userId)
        {
            try
            {
                // Validate input
                if (commentId <= 0)
                {
                    throw new ArgumentException("CommentId phải lớn hơn 0");
                }

                if (userId <= 0)
                {
                    throw new ArgumentException("UserId phải lớn hơn 0");
                }

                // Call DAO to delete comment
                var result = DaoFactory.Comment.DeleteTaskComment(commentId, userId);

                if (result == null)
                {
                    throw new Exception("Comment không tồn tại hoặc bạn không có quyền xóa");
                }

                var taskCommentResult = DaoFactory.Comment.CreateTaskComment(
                    result.TaskId.GetValueOrDefault(), 
                    userId, 
                    "{0} đã xóa bình luận của {1}", 
                    "task",
                    false,
                    false,
                    "remove_comment"
                );

                if (taskCommentResult == null)
                {
                    throw new Exception("Lỗi khi tạo comment");
                }

                DaoFactory.Comment.AddTaskCommentMention(taskCommentResult.Id.GetValueOrDefault(0), userId, null);
                DaoFactory.Comment.AddTaskCommentMention(taskCommentResult.Id.GetValueOrDefault(0), result.DeletedUserId.GetValueOrDefault(0), null);

                return new DeleteCommentResponse
                {
                    DeletedCommentId = result.DeletedCommentId.GetValueOrDefault(0),
                    DeletedUserId = result.DeletedUserId.GetValueOrDefault(0),
                    TaskId = result.TaskId.GetValueOrDefault(),
                    DeletedBy = result.DeletedBy.GetValueOrDefault(),
                    DeletedAt = result.DeletedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    Message = "Xóa comment thành công"
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CommentBo.DeleteTaskComment - Error: {0}", ex);
                throw new Exception($"Failed to delete comment: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Update comment theo ID
        /// </summary>
        /// <param name="commentId">ID của comment cần update</param>
        /// <param name="userId">ID của user thực hiện update</param>
        /// <param name="content">Nội dung mới</param>
        /// <returns>Response chứa thông tin comment đã update</returns>
        public UpdateCommentResponse UpdateTaskComment(int commentId, int userId, string content)
        {
            try
            {
                // Validate input
                if (commentId <= 0)
                {
                    throw new ArgumentException("CommentId phải lớn hơn 0");
                }

                if (userId <= 0)
                {
                    throw new ArgumentException("UserId phải lớn hơn 0");
                }

                if (string.IsNullOrEmpty(content))
                {
                    throw new ArgumentException("Nội dung comment không được để trống");
                }

                // Call DAO to update comment
                var result = DaoFactory.Comment.UpdateTaskComment(commentId, userId, content);

                if (result == null)
                {
                    throw new Exception("Comment không tồn tại hoặc bạn không có quyền chỉnh sửa");
                }

                return new UpdateCommentResponse
                {
                    CommentId = result.Id,
                    TaskId = result.TaskId,
                    Content = result.Content,
                    UpdatedBy = result.UserId,
                    UpdatedAt = result.UpdatedAt.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                    IsEdited = result.IsEdited.GetValueOrDefault(false),
                    Message = "Cập nhật comment thành công"
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CommentBo.UpdateTaskComment - Error: {0}", ex);
                throw new Exception($"Failed to update comment: {ex.Message}", ex);
            }
        }

        #endregion
    }
}