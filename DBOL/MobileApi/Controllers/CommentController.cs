using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Comment;
using BussinessObject.Permission;
using DataAccess;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using Newtonsoft.Json;
using TanTamApi.JWT.Helper;
using TanTamApi.JWT.Middleware;

namespace TanTamApi.Controllers
{
    [RoutePrefix("api/comment")]
    public class CommentController : ApiController
    {
        [ApiAuthorize]
        [HttpPost, Route("add")]
        public HttpResponseMessage AddComment()
        {
            var response = new ApiResult<AddCommentResponse>()
            {
                Data = new AddCommentResponse(),
                Code = ResponseResultEnum.Success.Value(),
                Message = ResponseResultEnum.Success.Text()
            };
            
            try
            {
                // Check if request contains multipart content
                if (!Request.Content.IsMimeMultipartContent())
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Request phải là multipart/form-data";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }

                // Get form data from HttpContext
                var httpContext = System.Web.HttpContext.Current;
                var form = httpContext.Request.Form;
                var files = httpContext.Request.Files;

                // Create request object from form data
                var request = new AddCommentRequest();
                
                // Parse task_id
                if (int.TryParse(form["task_id"], out int taskId))
                {
                    request.TaskId = taskId;
                }
                else
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "task_id không hợp lệ";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }

                // Get content
                request.Content = form["content"];
                if (string.IsNullOrEmpty(request.Content))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Nội dung comment không được để trống";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }

                // Get source (optional)
                request.Source = form["source"] ?? "task";

                // Get mention_ids (optional)
                request.MentionIds = new List<int>();
                
                // Try to get mention_ids[] as array first
                var mentionIdsArray = form.GetValues("mention_ids[]");
                if (mentionIdsArray != null)
                {
                    foreach (var mentionIdStr in mentionIdsArray)
                    {
                        if (int.TryParse(mentionIdStr, out int mentionId))
                        {
                            request.MentionIds.Add(mentionId);
                        }
                    }
                }
                else
                {
                    // Try to get mention_ids as JSON string
                    var mentionIdsJson = form["mention_ids"];
                    if (!string.IsNullOrEmpty(mentionIdsJson))
                    {
                        try
                        {
                            var mentionIdsList = JsonConvert.DeserializeObject<List<int>>(mentionIdsJson);
                            if (mentionIdsList != null)
                            {
                                request.MentionIds.AddRange(mentionIdsList);
                            }
                        }
                        catch (JsonException)
                        {
                            // If JSON parsing fails, try to parse as single integer
                            if (int.TryParse(mentionIdsJson, out int singleMentionId))
                            {
                                request.MentionIds.Add(singleMentionId);
                            }
                        }
                    }
                }

                // Get files (optional)
                request.Files = new List<System.Web.HttpPostedFileBase>();
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        request.Files.Add(new System.Web.HttpPostedFileWrapper(file));
                    }
                }

                // Get user info from token
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountId = JwtHelper.GetAccountIdFromToken(Request);

                // Call business logic
                response.Data = BoFactory.Comment.CreateTaskComment(request, accountId);

                response.Message = "Thêm comment thành công";
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CommentController AddComment EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }
            
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost, Route("list")]
        public HttpResponseMessage GetComments([FromBody] ListCommentRequest request)
        {
            var response = new ApiResult<ListCommentResponse>()
            {
                Data = new ListCommentResponse(),
                Code = ResponseResultEnum.Success.Value(),
                Message = ResponseResultEnum.Success.Text()
            };
            
            try
            {
                // Validate request
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Dữ liệu đầu vào không hợp lệ";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }

                if (request.TaskId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "TaskId phải lớn hơn 0";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }

                // Call business logic
                response.Data = BoFactory.Comment.GetTaskComments(request);

                response.Message = "Lấy danh sách comments thành công";
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CommentController GetComments EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }
            
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost, Route("delete/{commentId}")]
        public HttpResponseMessage DeleteComment(int commentId)
        {
            var response = new ApiResult<DeleteCommentResponse>()
            {
                Data = new DeleteCommentResponse(),
                Code = ResponseResultEnum.Success.Value(),
                Message = ResponseResultEnum.Success.Text()
            };
            
            try
            {
                // Validate input
                if (commentId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "CommentId phải lớn hơn 0";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }

                // Get user info from token
                var accountId = JwtHelper.GetAccountIdFromToken(Request);

                // Call business logic
                response.Data = BoFactory.Comment.DeleteTaskComment(commentId, accountId);

                response.Message = "Xóa comment thành công";
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CommentController DeleteComment EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }
            
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ApiAuthorize]
        [HttpPost, Route("update")]
        public HttpResponseMessage UpdateComment([FromBody] UpdateCommentRequest request)
        {
            var response = new ApiResult<UpdateCommentResponse>()
            {
                Data = new UpdateCommentResponse(),
                Code = ResponseResultEnum.Success.Value(),
                Message = ResponseResultEnum.Success.Text()
            };
            
            try
            {
                // Validate request
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Dữ liệu đầu vào không hợp lệ";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }

                if (request.CommentId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "CommentId phải lớn hơn 0";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }

                if (string.IsNullOrEmpty(request.Content))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Nội dung comment không được để trống";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }

                // Get user info from token
                var accountId = JwtHelper.GetAccountIdFromToken(Request);

                // Call business logic
                response.Data = BoFactory.Comment.UpdateTaskComment(request.CommentId, accountId, request.Content);

                response.Message = "Cập nhật comment thành công";
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CommentController UpdateComment EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }
            
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        // [ApiAuthorize]
        [HttpGet, Route("download/{fileId}")]
        public HttpResponseMessage DownloadFile(int fileId)
        {
            try
            {
                // Get file information from database
                var fileInfo = DaoFactory.Comment.GetFileUploadById(fileId);
                if (fileInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { message = "File không tồn tại" });
                }

                // Check if file exists on disk
                var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/"), fileInfo.Path.Replace("/", "\\"));
                if (!File.Exists(filePath))
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { message = "File không tồn tại trên server" });
                }

                // Read file content
                var fileBytes = File.ReadAllBytes(filePath);
                
                // Create response with file content
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(fileBytes)
                };

                // Set headers for file download
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileInfo.Name
                };
                response.Content.Headers.ContentLength = fileBytes.Length;

                return response;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("CommentController DownloadFile EX:", ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Đã xảy ra lỗi trong quá trình tải file" });
            }
        }
    }
}