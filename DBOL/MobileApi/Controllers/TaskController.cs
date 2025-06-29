using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using Logger;
using MyUtility.Extensions;
using TanTamApi.JWT.Helper;
using TanTamApi.Models.Request;
using TanTamApi.Models.Response;

namespace TanTamApi.Controllers
{
    [RoutePrefix("api/task")]
    public class TaskController : ApiController
    {
        #region Task CRUD Operations

        /// <summary>
        /// Tạo task mới
        /// </summary>
        [HttpPost]
        [Route("create")]
        public IHttpActionResult CreateTask([FromBody] CreateTaskRequestModel request)
        {
            var accountId = JwtHelper.GetAccountIdFromToken(Request);

            var response = new ApiResult<TaskResponse>()
            {
                Data = new TaskResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            if (request == null)
            {
                response.Code = ResponseResultEnum.InvalidInput.Value();
                response.Message = "Vui lòng nhập đủ thông tin.";
                return Content(HttpStatusCode.OK, response);
            }

            if (string.IsNullOrEmpty(request.Title))
            {
                response.Code = ResponseResultEnum.InvalidInput.Value();
                response.Message = "Tiêu đề task không được để trống.";
                return Content(HttpStatusCode.OK, response);
            }

            var result = BoFactory.Task.CreateTask(
                request.Title,
                accountId,
                request.DefaultView,
                request.Color,
                request.DepartmentIds,
                request.PositionIds,
                request.BranchIds,
                request.UserIds
            );

            if (result != null)
            {
                // Map từ entity sang response object
                response.Data = new TaskResponse
                {
                    Id = result.id,
                    Name = result.name,
                    Icon = result.icon,
                    Favored = result.favored,
                    ViewId = result.view_id,
                    Color = result.color,
                    UpdatedAt = result.updated_at,
                    IsArchived = result.is_archived,
                    CreatedAt = result.created_at,
                    DefaultDeadlineTime = result.default_deadline_time,
                    DefaultStartTime = result.default_start_time,
                    TaskDone = result.task_done,
                    TaskCount = result.task_count,
                    TaskOverdue = result.task_overdue,
                };
            }

            response.Code = ResponseResultEnum.Success.Value();
            response.Message = "Tạo task thành công.";
            return Content(HttpStatusCode.OK, response);
        }

        /// <summary>
        /// Lấy thông tin chi tiết của task bao gồm groups và sub tasks
        /// </summary>
        [HttpGet]
        [Route("detail")]
        public IHttpActionResult GetTaskDetail([FromUri] int? task_id = null)
        {
            var response = new ApiResult<TaskDetailResponse>()
            {
                Data = new TaskDetailResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                if (!task_id.HasValue || task_id.Value <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Task ID không hợp lệ.";
                    return Content(HttpStatusCode.OK, response);
                }

                var taskDetail = BoFactory.Task.GetTaskDetail(task_id.Value);

                if (taskDetail == null)
                {
                    response.Code = ResponseResultEnum.NotFound.Value();
                    response.Message = "Không tìm thấy task.";
                    return Content(HttpStatusCode.OK, response);
                }

                // Lấy thông tin groups của task
                var taskGroups = BoFactory.Task.GetTaskGroupsByTaskId(task_id.Value);

                // Debug logging
                CommonLogger.DefaultLogger.DebugFormat(
                    "GetTaskDetail - Task ID: {0}, Task Groups Count: {1}",
                    task_id.Value,
                    taskGroups?.Count ?? 0
                );

                if (taskGroups != null && taskGroups.Any())
                {
                    foreach (var group in taskGroups)
                    {
                        CommonLogger.DefaultLogger.DebugFormat(
                            "GetTaskDetail - Group ID: {0}, Name: {1}, Task ID: {2}",
                            group.id,
                            group.name,
                            group.task_id
                        );
                    }
                }
                else
                {
                    CommonLogger.DefaultLogger.DebugFormat(
                        "GetTaskDetail - No groups found for Task ID: {0}",
                        task_id.Value
                    );
                }

                // Lấy thông tin managers của task
                var taskManagers = BoFactory.Task.GetTaskManagersByTask(task_id.Value);

                // Lấy thông tin users của task
                var taskUsers = BoFactory.Task.GetTaskUsersByTask(task_id.Value);

                // Lấy thông tin creator của task
                var taskCreator = BoFactory.Task.GetTaskCreatorInfo(task_id.Value);

                // Map từ entity sang response object với đầy đủ thông tin
                response.Data = new TaskDetailResponse
                {
                    // Thông tin cơ bản của task
                    Id = taskDetail.id,
                    Name = taskDetail.name,
                    Icon = taskDetail.icon,
                    Favored = taskDetail.favored,
                    ViewId = taskDetail.view_id,
                    Color = taskDetail.color,
                    UpdatedAt = taskDetail.updated_at,
                    IsArchived = taskDetail.is_archived,
                    CreatedAt = taskDetail.created_at,
                    DefaultDeadlineTime = taskDetail.default_deadline_time,
                    DefaultStartTime = taskDetail.default_start_time,
                    TaskDone = taskDetail.task_done,
                    TaskCount = taskDetail.task_count,
                    TaskOverdue = taskDetail.task_overdue,

                    // Thông tin groups và sub tasks
                    Groups =
                        taskGroups
                            ?.Select(g => new TaskGroupDetailResponse
                            {
                                Id = g.id,
                                Name = g.name,
                                TaskId = g.task_id,
                                Color = g.color,
                                Index = g.index,
                                CreatedAt = g.created_at,
                                UpdatedAt = g.updated_at,
                                SubTasks = BoFactory
                                    .Task.GetTaskSubListByBundle(g.id.ToString())
                                    .Select(s =>
                                    {
                                        // Debug logging cho sub task
                                        CommonLogger.DefaultLogger.DebugFormat(
                                            "GetTaskDetail - Sub Task ID: {0}, Title: {1}, Bundle ID: {2}",
                                            s.id,
                                            s.title,
                                            s.bundle_id
                                        );

                                        // Lấy thông tin user tạo sub task
                                        var createdUser = BoFactory
                                            .Task.GetTaskUsersByUser(s.created_user_id ?? 0)
                                            .FirstOrDefault();

                                        // Lấy customized fields cho sub task
                                        var customizedFieldRows =
                                            BoFactory.Task.GetSubFieldValueByTitle(s.id);
                                        var customizedFields = customizedFieldRows
                                            .GroupBy(f => f.field_id)
                                            .ToDictionary(
                                                group =>
                                                    group.First().key?.ToString()
                                                    ?? group.Key.ToString(),
                                                group => new TaskFieldDetailResponse
                                                {
                                                    Key = group.First().title,
                                                    Value = group.First().value_text,
                                                    Title = group.First().title,
                                                    TitleNosign = null,
                                                    Color = group.First().color,
                                                    SortIndex = 0,
                                                    OptionAction = group.First().option_action,
                                                }
                                            );

                                        return new SubTaskDetailResponse
                                        {
                                            Id = s.id,
                                            OrdinalNumber = s.ordinal_number,
                                            BundleId = s.bundle_id,
                                            SortIndex = s.sort_index,
                                            PrivateSortIndex = s.private_sort_index,
                                            Title = s.title,
                                            TitleNosign = s.title_nosign,
                                            Description = s.description,
                                            Alias = s.alias,
                                            ShopId = s.shop_id,
                                            CreatedUserId = s.created_user_id,
                                            Deadline = s.deadline,
                                            StartDate = s.start_date,
                                            Duration = s.duration,
                                            CreatedAt = s.created_at,
                                            UpdatedAt = s.updated_at,
                                            CompletedAt = s.completed_at,
                                            IsCompleted = s.is_completed,
                                            CompletionPercentage = s.completion_percentage,
                                            CustomizedFields =
                                                customizedFields
                                                ?? new Dictionary<
                                                    string,
                                                    TaskFieldDetailResponse
                                                >(),
                                            CreatedUserObj =
                                                createdUser != null
                                                    ? new UserSimpleResponse
                                                    {
                                                        Id = GetPropertyValue<int>(
                                                            createdUser,
                                                            "id"
                                                        ),
                                                        Username = GetPropertyValue<int?>(
                                                            createdUser,
                                                            "username"
                                                        ),
                                                        Name = GetPropertyValue<string>(
                                                            createdUser,
                                                            "name"
                                                        ),
                                                        Identification = GetPropertyValue<string>(
                                                            createdUser,
                                                            "identification"
                                                        ),
                                                        BranchId = GetPropertyValue<int?>(
                                                            createdUser,
                                                            "branch_id"
                                                        ),
                                                    }
                                                    : null,
                                        };
                                    })
                                    .ToList(),
                            })
                            .ToList() ?? new List<TaskGroupDetailResponse>(),

                    // Thông tin managers
                    Managers = null,

                    // Thông tin users
                    Users = null,

                    // Thông tin creator
                    Creator =
                        taskCreator != null
                            ? new UserSimpleResponse
                            {
                                Id = taskCreator.id,
                                Username = taskCreator.username,
                                Name = taskCreator.name,
                                Identification = taskCreator.identification,
                                BranchId = taskCreator.branch_id,
                            }
                            : null,
                };

                // Debug logging cho response
                CommonLogger.DefaultLogger.DebugFormat(
                    "GetTaskDetail - Response Groups Count: {0}, Managers Count: {1}, Users Count: {2}",
                    response.Data.Groups?.Count ?? 0,
                    response.Data.Managers?.Count ?? 0,
                    response.Data.Users?.Count ?? 0
                );

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Lấy thông tin chi tiết task thành công.";
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "GetTaskDetail Exception task_id {0}, EX: {1}",
                    task_id,
                    ex.ToString()
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Lấy danh sách tasks
        /// </summary>
        [HttpGet]
        [Route("list")]
        public IHttpActionResult GetTaskList(
            [FromUri] int? task_id = null,
            [FromUri] int page = 1,
            [FromUri] int limit = 10
        )
        {
            var response = new ApiResult<TaskListResponse>()
            {
                Data = new TaskListResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                var taskList = BoFactory.Task.GetTaskList(task_id);

                // Map từ entity sang response
                var taskResponses = taskList
                    .Select(task => new TaskResponse
                    {
                        Id = task.id,
                        Name = task.name,
                        Icon = task.icon,
                        Favored = task.favored,
                        ViewId = task.view_id,
                        Color = task.color,
                        UpdatedAt = task.updated_at,
                        IsArchived = task.is_archived,
                        CreatedAt = task.created_at,
                        DefaultDeadlineTime = task.default_deadline_time,
                        DefaultStartTime = task.default_start_time,
                        TaskDone = task.task_done,
                        TaskCount = task.task_count,
                        TaskOverdue = task.task_overdue,
                    })
                    .ToList();

                response.Data = new TaskListResponse
                {
                    Items = taskResponses,
                    Meta = new TaskListMetaResponse
                    {
                        Total = taskResponses.Count(),
                        Count = taskResponses.Count(),
                        PerPage = limit,
                        CurrentPage = page,
                        TotalPages = (int)Math.Ceiling((double)taskResponses.Count() / limit),
                    },
                };

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Lấy danh sách task thành công.";
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("GetTaskList Exception EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        #endregion

        #region Task Group Operations

        /// <summary>
        /// Lấy danh sách nhóm task theo task ID
        /// </summary>
        [HttpGet]
        [Route("groups")]
        public IHttpActionResult GetTaskGroups([FromUri] int? task_id = null)
        {
            var response = new ApiResult<List<TaskGroupResponse>>()
            {
                Data = new List<TaskGroupResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                if (!task_id.HasValue || task_id.Value <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Task ID không hợp lệ.";
                    return Content(HttpStatusCode.OK, response);
                }

                var groupList = BoFactory.Task.GetTaskGroupsByTaskId(task_id.Value);

                if (groupList == null)
                {
                    response.Code = ResponseResultEnum.NotFound.Value();
                    response.Message = "Không tìm thấy nhóm task.";
                    return Content(HttpStatusCode.OK, response);
                }

                response.Data = groupList
                    .Select(g => new TaskGroupResponse
                    {
                        Id = g.id,
                        Name = g.name,
                        TaskId = g.task_id,
                        Color = g.color,
                        Index = g.index,
                        CreatedAt = g.created_at,
                        UpdatedAt = g.updated_at,
                        SubTasks = BoFactory
                            .Task.GetTaskSubListByBundle(g.id.ToString())
                            .Select(s =>
                            {
                                var createdUser = BoFactory
                                    .Task.GetTaskUsersByUser(s.created_user_id ?? 0)
                                    .FirstOrDefault();
                                return new SubTaskResponse
                                {
                                    Id = s.id,
                                    OrdinalNumber = s.ordinal_number,
                                    BundleId = s.bundle_id,
                                    SortIndex = s.sort_index,
                                    PrivateSortIndex = s.private_sort_index,
                                    Title = s.title,
                                    TitleNosign = s.title_nosign,
                                    Description = s.description,
                                    Alias = s.alias,
                                    ShopId = s.shop_id,
                                    CreatedUserId = s.created_user_id,
                                    Deadline = s.deadline,
                                    StartDate = s.start_date,
                                    Duration = s.duration,
                                    CreatedAt = s.created_at,
                                    UpdatedAt = s.updated_at,
                                    CompletedAt = s.completed_at,
                                    IsCompleted = s.is_completed,
                                    CompletionPercentage = s.completion_percentage,
                                    CustomizedFields =
                                        new Dictionary<string, TaskFieldDetailResponse>(),
                                    CreatedUserObj = null,
                                };
                            })
                            .ToList(),
                    })
                    .ToList();

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Lấy danh sách nhóm task thành công.";
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "GetTaskGroups Exception task_id {0}, EX:",
                    task_id,
                    ex
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Tạo nhóm task mới
        /// </summary>
        [HttpPost]
        [Route("create-group")]
        public IHttpActionResult CreateTaskGroup([FromBody] CreateTaskGroupRequestModel request)
        {
            var response = new ApiResult<TaskGroupResponse>()
            {
                Data = new TaskGroupResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng nhập đủ thông tin.";
                    return Content(HttpStatusCode.OK, response);
                }

                if (request.BundleId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Bundle ID không hợp lệ.";
                    return Content(HttpStatusCode.OK, response);
                }

                var result = BoFactory.Task.CreateTaskGroup(
                    request.BundleId,
                    request.Name,
                    request.Color,
                    request.Position
                );

                if (result != null)
                {
                    response.Data = new TaskGroupResponse
                    {
                        Id = result.id,
                        Name = result.name,
                        Color = result.color,
                        Index = result.index,
                        CreatedAt = result.created_at,
                        UpdatedAt = result.updated_at,
                    };
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Tạo group thành công.";
                }
                else
                {
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Không thể tạo group.";
                }

                return Content(HttpStatusCode.OK, response);
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ExtractSqlErrorMessage(entityEx.Message);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "CreateTaskGroup Exception request {0}, EX: {1}",
                    request?.ToString(),
                    ex.ToString()
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Xóa group và tất cả subtasks trong group
        /// </summary>
        [HttpPost]
        [Route("delete-group")]
        public IHttpActionResult DeleteTaskGroup([FromUri] int groupId)
        {
            var response = new ApiResult<List<TaskGroupResponse>>()
            {
                Data = new List<TaskGroupResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                if (groupId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID group không hợp lệ.";
                    return Content(HttpStatusCode.OK, response);
                }

                var result = BoFactory.Task.DeleteTaskGroup(groupId);

                if (result != null && result.Any())
                {
                    // Map từ entity sang response object
                    response.Data = result
                        .Select(g => new TaskGroupResponse
                        {
                            Id = g.id,
                            TaskId = g.task_id,
                            Name = g.name,
                            Color = g.color,
                            Index = g.index,
                            CreatedAt = g.created_at,
                            UpdatedAt = g.updated_at,
                            SubTasks = BoFactory
                                .Task.GetTaskSubListByBundle(g.id.ToString())
                                .Select(s =>
                                {
                                    var createdUser = BoFactory
                                        .Task.GetTaskUsersByUser(s.created_user_id ?? 0)
                                        .FirstOrDefault();
                                    return new SubTaskResponse
                                    {
                                        Id = s.id,
                                        OrdinalNumber = s.ordinal_number,
                                        BundleId = s.bundle_id,
                                        SortIndex = s.sort_index,
                                        PrivateSortIndex = s.private_sort_index,
                                        Title = s.title,
                                        TitleNosign = s.title_nosign,
                                        Description = s.description,
                                        Alias = s.alias,
                                        ShopId = s.shop_id,
                                        CreatedUserId = s.created_user_id,
                                        Deadline = s.deadline,
                                        StartDate = s.start_date,
                                        Duration = s.duration,
                                        CreatedAt = s.created_at,
                                        UpdatedAt = s.updated_at,
                                        CompletedAt = s.completed_at,
                                        IsCompleted = s.is_completed,
                                        CompletionPercentage = s.completion_percentage,
                                        CustomizedFields =
                                            new Dictionary<string, TaskFieldDetailResponse>(),
                                        CreatedUserObj = null,
                                    };
                                })
                                .ToList(),
                        })
                        .ToList();

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Xóa group thành công.";
                }
                else
                {
                    response.Code = ResponseResultEnum.NotFound.Value();
                    response.Message = "Group không tồn tại hoặc đã được xóa.";
                }

                return Content(HttpStatusCode.OK, response);
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (
                    entityEx.InnerException != null
                    && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx
                )
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("BO Error", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi hệ thống.";
                }
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "DeleteTaskGroup Exception groupId {0}, EX: {1}",
                    groupId,
                    ex.ToString()
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Cập nhật tên của task group
        /// </summary>
        [HttpPost]
        [Route("update-group-name")]
        public IHttpActionResult UpdateTaskGroupName([FromBody] UpdateTaskGroupNameRequestModel request)
        {
            var response = new ApiResult<TaskGroupResponse>()
            {
                Data = new TaskGroupResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng nhập đủ thông tin.";
                    return Content(HttpStatusCode.OK, response);
                }

                if (request.GroupId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID group không hợp lệ.";
                    return Content(HttpStatusCode.OK, response);
                }

                if (string.IsNullOrEmpty(request.Name))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Tên group không được để trống.";
                    return Content(HttpStatusCode.OK, response);
                }

                var result = BoFactory.Task.UpdateTaskGroupName(request.GroupId, request.Name);

                if (result != null)
                {
                    response.Data = new TaskGroupResponse
                    {
                        Id = result.id,
                        TaskId = result.task_id,
                        Name = result.name,
                        Color = result.color,
                        Index = result.index,
                        CreatedAt = result.created_at,
                        UpdatedAt = result.updated_at
                    };
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Cập nhật tên group thành công.";
                }
                else
                {
                    response.Code = ResponseResultEnum.NotFound.Value();
                    response.Message = "Group không tồn tại.";
                }

                return Content(HttpStatusCode.OK, response);
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (
                    entityEx.InnerException != null
                    && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx
                )
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("BO Error", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi hệ thống.";
                }
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "UpdateTaskGroupName Exception request {0}, EX: {1}",
                    request?.ToString(),
                    ex.ToString()
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        #endregion

        #region SubTask CRUD Operations

        /// <summary>
        /// Tạo sub-task mới
        /// </summary>
        [HttpPost]
        [Route("create-subtask")]
        public IHttpActionResult CreateSubTask([FromBody] CreateSubTaskRequestModel request)
        {
            // var accountId = JwtHelper.GetAccountIdFromToken(Request);
            var accountId = 166;

            var response = new ApiResult<SubTaskResponse>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng nhập đủ thông tin.";
                    return Content(HttpStatusCode.OK, response);
                }

                if (request.BundleId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Bundle ID không hợp lệ.";
                    return Content(HttpStatusCode.OK, response);
                }

                if (string.IsNullOrEmpty(request.Title))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Tiêu đề không được để trống.";
                    return Content(HttpStatusCode.OK, response);
                }

                var alias = MyUtility
                    .Extensions.StringExtension.ConvertToUnSign(request.Title)
                    .Replace("-", "_");
                var result = BoFactory.Task.CreateTaskSub(
                    request.Title,
                    alias,
                    request.BundleId,
                    accountId,
                    request.Position
                );

                if (result != null)
                {
                    var createdUser = BoFactory
                        .Task.GetTaskUsersByUser(result.created_user_id ?? 0)
                        .FirstOrDefault();

                    response.Data = new SubTaskResponse
                    {
                        Id = result.id,
                        OrdinalNumber = result.ordinal_number,
                        BundleId = result.bundle_id,
                        SortIndex = result.sort_index,
                        PrivateSortIndex = result.private_sort_index,
                        Title = result.title,
                        TitleNosign = result.title_nosign,
                        Description = result.description,
                        Alias = result.alias,
                        ShopId = result.shop_id,
                        CreatedUserId = result.created_user_id,
                        Deadline = result.deadline,
                        StartDate = result.start_date,
                        Duration = result.duration,
                        CreatedAt = result.created_at,
                        UpdatedAt = result.updated_at,
                        CompletedAt = null,
                        IsCompleted = false,
                        CompletionPercentage = 0,
                        CustomizedFields = null,
                        CreatedUserObj = null,
                    };
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Tạo sub-task thành công.";
                }
                else
                {
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Tạo sub-task thất bại.";
                }
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.DebugFormat(
                    "CreateSubTask Exception request {0}, EX:",
                    request != null ? request.Title : "",
                    ex
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Lấy danh sách sub-tasks theo bundle
        /// </summary>
        /// <param name="bundleId">ID của bundle</param>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="limit">Số lượng item mỗi trang</param>
        /// <returns>Danh sách sub-tasks</returns>
        [HttpGet]
        [Route("subtasks")]
        public IHttpActionResult GetSubTaskList(
            [FromUri] string bundleId,
            [FromUri] int page = 1,
            [FromUri] int limit = 10
        )
        {
            var response = new ApiResult<SubTaskListResponse>()
            {
                Data = new SubTaskListResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Validation
                if (string.IsNullOrEmpty(bundleId))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Bundle ID không được để trống.";
                    return Content(HttpStatusCode.OK, response);
                }

                if (page <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Số trang phải lớn hơn 0.";
                    return Content(HttpStatusCode.OK, response);
                }

                if (limit <= 0 || limit > 100)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Số lượng item mỗi trang phải từ 1 đến 100.";
                    return Content(HttpStatusCode.OK, response);
                }

                // Lấy danh sách sub-tasks
                var subTaskList = BoFactory.Task.GetTaskSubListByBundle(bundleId);

                if (subTaskList == null || !subTaskList.Any())
                {
                    response.Data = new SubTaskListResponse
                    {
                        Items = new List<SubTaskResponse>(),
                        Meta = new TaskListMetaResponse
                        {
                            Total = 0,
                            Count = 0,
                            PerPage = limit,
                            CurrentPage = page,
                            TotalPages = 0,
                        },
                    };
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Không có sub-task nào.";
                    return Content(HttpStatusCode.OK, response);
                }

                // Pagination
                var totalCount = subTaskList.Count();
                var pagedSubTasks = subTaskList.Skip((page - 1) * limit).Take(limit).ToList();

                // Lấy danh sách user IDs để query một lần
                var userIds = pagedSubTasks
                    .Where(s => s.created_user_id.HasValue)
                    .Select(s => s.created_user_id.Value)
                    .Distinct()
                    .ToList();

                // Lấy thông tin users (sử dụng method hiện có)
                var users = new Dictionary<int, object>();
                foreach (var userId in userIds)
                {
                    var userTasks = BoFactory.Task.GetTaskUsersByUser(userId);
                    if (userTasks != null && userTasks.Any())
                    {
                        // Lấy thông tin user từ task đầu tiên (giả sử user có ít nhất 1 task)
                        var userInfo = userTasks.First();
                        users[userId] = userInfo;
                    }
                }

                var subTaskResponses = pagedSubTasks
                    .Select(s =>
                    {
                        // Lấy thông tin user tạo task
                        object createdUser = null;
                        if (
                            s.created_user_id.HasValue && users.ContainsKey(s.created_user_id.Value)
                        )
                        {
                            createdUser = users[s.created_user_id.Value];
                        }

                        // Lấy customized fields cho subtask
                        var customizedFieldRows = BoFactory.Task.GetSubFieldValueByTitle(s.id);
                        var customizedFields = customizedFieldRows
                            .GroupBy(f => f.field_id)
                            .ToDictionary(
                                group => group.First().key?.ToString() ?? group.Key.ToString(),
                                group => new TaskFieldDetailResponse
                                {
                                    Key = group.First().title,
                                    Value = group.First().value_text,
                                    Title = group.First().title,
                                    TitleNosign = null,
                                    Color = group.First().color,
                                    SortIndex = 0,
                                    OptionAction = group.First().option_action,
                                }
                            );

                        return new SubTaskResponse
                        {
                            Id = s.id,
                            OrdinalNumber = s.ordinal_number,
                            BundleId = s.bundle_id,
                            SortIndex = s.sort_index,
                            PrivateSortIndex = s.private_sort_index,
                            Title = s.title,
                            TitleNosign = s.title_nosign,
                            Description = s.description,
                            Alias = s.alias,
                            ShopId = s.shop_id,
                            CreatedUserId = s.created_user_id,
                            Deadline = s.deadline,
                            StartDate = s.start_date,
                            Duration = s.duration,
                            CreatedAt = s.created_at,
                            UpdatedAt = s.updated_at,
                            CompletedAt = s.completed_at,
                            IsCompleted = s.is_completed,
                            CompletionPercentage = s.completion_percentage,
                            CustomizedFields =
                                customizedFields
                                ?? new Dictionary<string, TaskFieldDetailResponse>(),
                            CreatedUserObj =
                                createdUser != null
                                    ? new UserSimpleResponse
                                    {
                                        Id = GetPropertyValue<int>(createdUser, "id"),
                                        Username = GetPropertyValue<int?>(createdUser, "username"),
                                        Name = GetPropertyValue<string>(createdUser, "name"),
                                        Identification = GetPropertyValue<string>(
                                            createdUser,
                                            "identification"
                                        ),
                                        BranchId = GetPropertyValue<int?>(createdUser, "branch_id"),
                                    }
                                    : null,
                        };
                    })
                    .ToList();

                response.Data = new SubTaskListResponse
                {
                    Items = subTaskResponses,
                    Meta = new TaskListMetaResponse
                    {
                        Total = totalCount,
                        Count = subTaskResponses.Count(),
                        PerPage = limit,
                        CurrentPage = page,
                        TotalPages = (int)Math.Ceiling((double)totalCount / limit),
                    },
                };

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Lấy danh sách sub-task thành công.";
                return Content(HttpStatusCode.OK, response);
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                // Extract SQL RAISERROR message
                if (
                    entityEx.InnerException != null
                    && entityEx.InnerException.Message.Contains("RAISERROR")
                )
                {
                    var errorMessage = ExtractSqlErrorMessage(entityEx.InnerException.Message);
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = errorMessage;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error($"GetSubTaskList EntityException", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi khi xử lý.";
                }
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "GetSubTaskList Exception bundleId {0}, page {1}, limit {2}, EX: {3}",
                    bundleId,
                    page,
                    limit,
                    ex
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý.";
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Lấy danh sách sub-tasks theo bundle
        /// </summary>
        /// <param name="bundleId">ID của bundle</param>
        /// <returns>Danh sách sub-tasks</returns>
        [HttpGet]
        [Route("subtasks-by-bundle")]
        public IHttpActionResult GetTaskSubListByBundle([FromUri] string bundleId)
        {
            var response = new ApiResult<List<SubTaskResponse>>()
            {
                Data = new List<SubTaskResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                if (string.IsNullOrEmpty(bundleId))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Bundle ID không được để trống.";
                    return Content(HttpStatusCode.OK, response);
                }

                var subTaskList = BoFactory.Task.GetTaskSubListByBundle(bundleId);

                var subTaskResponses = subTaskList
                    .Select(s =>
                    {
                        var createdUser = BoFactory
                            .Task.GetTaskUsersByUser(s.created_user_id ?? 0)
                            .FirstOrDefault();

                        // Lấy customized fields cho subtask
                        var customizedFieldRows = BoFactory.Task.GetSubFieldValueByTitle(s.id);
                        var customizedFields = customizedFieldRows
                            .GroupBy(f => f.field_id)
                            .ToDictionary(
                                group => group.First().key?.ToString() ?? group.Key.ToString(),
                                group => new TaskFieldDetailResponse
                                {
                                    Key = group.First().title,
                                    Value = group.First().value_text,
                                    Title = group.First().title,
                                    TitleNosign = null,
                                    Color = group.First().color,
                                    SortIndex = 0,
                                    OptionAction = group.First().option_action,
                                }
                            );

                        return new SubTaskResponse
                        {
                            Id = s.id,
                            OrdinalNumber = s.ordinal_number,
                            BundleId = s.bundle_id,
                            SortIndex = s.sort_index,
                            PrivateSortIndex = s.private_sort_index,
                            Title = s.title,
                            TitleNosign = s.title_nosign,
                            Description = s.description,
                            Alias = s.alias,
                            ShopId = s.shop_id,
                            CreatedUserId = s.created_user_id,
                            Deadline = s.deadline,
                            StartDate = s.start_date,
                            Duration = s.duration,
                            CreatedAt = s.created_at,
                            UpdatedAt = s.updated_at,
                            CompletedAt = s.completed_at,
                            IsCompleted = s.is_completed,
                            CompletionPercentage = s.completion_percentage,
                            CustomizedFields =
                                customizedFields
                                ?? new Dictionary<string, TaskFieldDetailResponse>(),
                            CreatedUserObj =
                                createdUser != null
                                    ? new UserSimpleResponse
                                    {
                                        Id = GetPropertyValue<int>(createdUser, "id"),
                                        Username = GetPropertyValue<int?>(createdUser, "username"),
                                        Name = GetPropertyValue<string>(createdUser, "name"),
                                        Identification = GetPropertyValue<string>(
                                            createdUser,
                                            "identification"
                                        ),
                                        BranchId = GetPropertyValue<int?>(createdUser, "branch_id"),
                                    }
                                    : null,
                        };
                    })
                    .ToList();

                response.Data = subTaskResponses;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Lấy danh sách sub-task thành công.";
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "GetTaskSubListByBundle Exception bundle_id {0}, EX:",
                    bundleId,
                    ex
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái hoàn thành của sub-task
        /// </summary>
        /// <param name="id">ID của sub-task</param>
        /// <param name="isCompleted">Trạng thái hoàn thành</param>
        /// <returns>Thông tin sub-task đã cập nhật</returns>
        [HttpPost]
        [Route("check-done")]
        public IHttpActionResult UpdateTaskSubCompleted(
            [FromUri] int id,
            [FromBody] bool isCompleted
        )
        {
            var response = new ApiResult<SubTaskResponse>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                if (id <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID của sub-task không hợp lệ.";
                    return Content(HttpStatusCode.OK, response);
                }

                var result = BoFactory.Task.UpdateTaskSubCompleted(id, isCompleted);

                if (result != null)
                {
                    var createdUser = BoFactory
                        .Task.GetTaskUsersByUser(result.created_user_id ?? 0)
                        .FirstOrDefault();
                    response.Data = new SubTaskResponse
                    {
                        Id = result.id,
                        OrdinalNumber = result.ordinal_number,
                        BundleId = result.bundle_id,
                        SortIndex = result.sort_index,
                        PrivateSortIndex = result.private_sort_index,
                        Title = result.title,
                        TitleNosign = result.title_nosign,
                        Description = result.description,
                        Alias = result.alias,
                        ShopId = result.shop_id,
                        CreatedUserId = result.created_user_id,
                        Deadline = result.deadline,
                        StartDate = result.start_date,
                        Duration = result.duration,
                        CreatedAt = result.created_at,
                        UpdatedAt = result.updated_at,
                        CompletedAt = result.completed_at,
                        IsCompleted = result.is_completed,
                        CompletionPercentage = result.completion_percentage,
                        CustomizedFields = BoFactory
                            .Task.GetSubFieldValueByTitle(result.id)
                            .ToDictionary(
                                f => f.key?.ToString() ?? f.field_id.ToString(),
                                f => new TaskFieldDetailResponse
                                {
                                    Key = f.title,
                                    Value = f.value_text,
                                    Title = f.title,
                                    TitleNosign = null,
                                    Color = f.color,
                                    SortIndex = 0, // Không có sort_index trong result này
                                    OptionAction = f.option_action,
                                }
                            ),
                        CreatedUserObj =
                            createdUser != null
                                ? new UserSimpleResponse
                                {
                                    Id = createdUser.id,
                                    Username = GetPropertyValue<int?>(createdUser, "username"),
                                    Name = GetPropertyValue<string>(createdUser, "name"),
                                    Identification = GetPropertyValue<string>(
                                        createdUser,
                                        "identification"
                                    ),
                                    BranchId = GetPropertyValue<int?>(createdUser, "branch_id"),
                                }
                                : null,
                    };
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Cập nhật trạng thái hoàn thành sub-task thành công.";
                }
                else
                {
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Cập nhật trạng thái hoàn thành sub-task thất bại.";
                }
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "UpdateTaskSubCompleted Exception id {0}, isCompleted {1}, EX:",
                    id,
                    isCompleted,
                    ex
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Cập nhật deadline của sub-task
        /// </summary>
        /// <param name="request">Thông tin cập nhật deadline</param>
        /// <returns>Thông tin sub-task đã cập nhật</returns>
        [HttpPost]
        [Route("update-deadline")]
        public IHttpActionResult UpdateSubTaskDeadline(
            [FromBody] UpdateSubTaskDeadlineRequestModel request
        )
        {
            var response = new ApiResult<SubTaskResponse>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng nhập đủ thông tin.";
                    return Content(HttpStatusCode.OK, response);
                }

                if (request.Id <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID sub-task không hợp lệ.";
                    return Content(HttpStatusCode.OK, response);
                }

                var result = BoFactory.Task.UpdateTaskSubDeadline(
                    request.Id,
                    request.Deadline,
                    request.StartDate
                );

                if (result != null)
                {
                    var createdUser = BoFactory
                        .Task.GetTaskUsersByUser(result.created_user_id ?? 0)
                        .FirstOrDefault();
                    response.Data = new SubTaskResponse
                    {
                        Id = result.id,
                        OrdinalNumber = result.ordinal_number,
                        BundleId = result.bundle_id,
                        SortIndex = result.sort_index,
                        PrivateSortIndex = result.private_sort_index,
                        Title = result.title,
                        TitleNosign = result.title_nosign,
                        Description = result.description,
                        Alias = result.alias,
                        ShopId = result.shop_id,
                        CreatedUserId = result.created_user_id,
                        Deadline = result.deadline,
                        StartDate = result.start_date,
                        Duration = result.duration,
                        CreatedAt = result.created_at,
                        UpdatedAt = result.updated_at,
                        CompletedAt = result.completed_at,
                        IsCompleted = result.is_completed,
                        CompletionPercentage = result.completion_percentage,
                        CustomizedFields = BoFactory
                            .Task.GetSubFieldValueByTitle(result.id)
                            .ToDictionary(
                                f => f.key?.ToString() ?? f.field_id.ToString(),
                                f => new TaskFieldDetailResponse
                                {
                                    Key = f.title,
                                    Value = f.value_text,
                                    Title = f.title,
                                    TitleNosign = null,
                                    Color = f.color,
                                    SortIndex = 0, // Không có sort_index trong result này
                                    OptionAction = f.option_action,
                                }
                            ),
                        CreatedUserObj =
                            createdUser != null
                                ? new UserSimpleResponse
                                {
                                    Id = createdUser.id,
                                    Username = GetPropertyValue<int?>(createdUser, "username"),
                                    Name = GetPropertyValue<string>(createdUser, "name"),
                                    Identification = GetPropertyValue<string>(
                                        createdUser,
                                        "identification"
                                    ),
                                    BranchId = GetPropertyValue<int?>(createdUser, "branch_id"),
                                }
                                : null,
                    };
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Cập nhật deadline thành công.";
                }
                else
                {
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Cập nhật deadline thất bại.";
                }
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "UpdateSubTaskDeadline Exception request {0}, EX:",
                    request != null ? request.Id.ToString() : "",
                    ex
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Xóa sub-task và trả về danh sách sub-task còn lại trong group
        /// </summary>
        /// <param name="subtaskId">ID của sub-task cần xóa</param>
        /// <returns>Danh sách sub-task còn lại trong group và thông tin group</returns>
        [HttpPost]
        [Route("delete-subtask")]
        public IHttpActionResult DeleteSubTask([FromUri] int subtaskId)
        {
            var response = new ApiResult<object>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                if (subtaskId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID sub-task không hợp lệ.";
                    return Content(HttpStatusCode.OK, response);
                }

                var result = BoFactory.Task.DeleteTaskSub(subtaskId);

                if (result != null && result.Any())
                {
                    // Lấy thông tin group từ sub task đầu tiên
                    var firstSubTask = result.First();
                    var groupId = firstSubTask.bundle_id;

                    // Lấy thông tin chi tiết của group
                    var groupInfo = BoFactory
                        .Task.GetTaskGroupsByTaskId(groupId ?? 0)
                        .FirstOrDefault(g => g.id == groupId);

                    // Map từ entity sang response
                    var subTaskResponses = result
                        .Select(s =>
                        {
                            var createdUser = BoFactory
                                .Task.GetTaskUsersByUser(s.created_user_id ?? 0)
                                .FirstOrDefault();

                            // Lấy customized fields cho subtask
                            var customizedFieldRows = BoFactory.Task.GetSubFieldValueByTitle(s.id);
                            var customizedFields = customizedFieldRows
                                .GroupBy(f => f.field_id)
                                .ToDictionary(
                                    group => group.First().key?.ToString() ?? group.Key.ToString(),
                                    group => new TaskFieldDetailResponse
                                    {
                                        Key = group.First().title,
                                        Value = group.First().value_text,
                                        Title = group.First().title,
                                        TitleNosign = null,
                                        Color = group.First().color,
                                        SortIndex = 0,
                                        OptionAction = group.First().option_action,
                                    }
                                );

                            return new SubTaskResponse
                            {
                                Id = s.id,
                                OrdinalNumber = s.ordinal_number,
                                BundleId = s.bundle_id,
                                SortIndex = s.sort_index,
                                PrivateSortIndex = s.private_sort_index,
                                Title = s.title,
                                TitleNosign = s.title_nosign,
                                Description = s.description,
                                Alias = s.alias,
                                ShopId = s.shop_id,
                                CreatedUserId = s.created_user_id,
                                Deadline = s.deadline,
                                StartDate = s.start_date,
                                Duration = s.duration,
                                CreatedAt = s.created_at,
                                UpdatedAt = s.updated_at,
                                CompletedAt = s.completed_at,
                                IsCompleted = s.is_completed,
                                CompletionPercentage = s.completion_percentage,
                                CustomizedFields =
                                    customizedFields
                                    ?? new Dictionary<string, TaskFieldDetailResponse>(),
                                CreatedUserObj =
                                    createdUser != null
                                        ? new UserSimpleResponse
                                        {
                                            Id = GetPropertyValue<int>(createdUser, "id"),
                                            Username = GetPropertyValue<int?>(
                                                createdUser,
                                                "username"
                                            ),
                                            Name = GetPropertyValue<string>(createdUser, "name"),
                                            Identification = GetPropertyValue<string>(
                                                createdUser,
                                                "identification"
                                            ),
                                            BranchId = GetPropertyValue<int?>(
                                                createdUser,
                                                "branch_id"
                                            ),
                                        }
                                        : null,
                            };
                        })
                        .ToList();

                    // Tạo response object bao gồm cả group info và sub tasks
                    var responseData = new
                    {
                        Group = groupInfo != null
                            ? new
                            {
                                Id = groupInfo.id,
                                Name = groupInfo.name,
                                TaskId = groupInfo.task_id,
                                Color = groupInfo.color,
                                Index = groupInfo.index,
                                CreatedAt = groupInfo.created_at,
                                UpdatedAt = groupInfo.updated_at,
                            }
                            : null,
                        SubTasks = subTaskResponses,
                        DeletedSubTaskId = subtaskId,
                        Message = "Xóa sub-task thành công.",
                    };

                    response.Data = responseData;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Xóa sub-task thành công.";
                }
                else
                {
                    response.Code = ResponseResultEnum.NotFound.Value();
                    response.Message = "Sub-task không tồn tại hoặc đã được xóa.";
                }

                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "DeleteSubTask Exception subtaskId {0}, EX: {1}",
                    subtaskId,
                    ex.ToString()
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Cập nhật tiêu đề sub-task
        /// </summary>
        [HttpPost]
        [Route("update-subtask-title")]
        public IHttpActionResult UpdateSubTaskTitle(
            [FromBody] UpdateSubTaskTitleRequestModel request
        )
        {
            var response = new ApiResult<SubTaskResponse>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                if (request == null || request.Id <= 0 || string.IsNullOrEmpty(request.Title))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID và tiêu đề không được để trống.";
                    return Content(HttpStatusCode.OK, response);
                }

                // Tự động sinh title_nosign và alias từ title
                string titleNosign = MyUtility.Extensions.StringExtension.ConvertToUnSign(
                    request.Title
                );
                string alias = titleNosign.Replace("-", "_");

                var result = BoFactory.Task.UpdateSubTaskTitle(
                    request.Id,
                    request.Title,
                    titleNosign,
                    alias
                );

                if (result != null)
                {
                    response.Data = new SubTaskResponse
                    {
                        Id = result.id,
                        OrdinalNumber = result.ordinal_number,
                        BundleId = result.bundle_id,
                        SortIndex = result.sort_index,
                        PrivateSortIndex = result.private_sort_index,
                        Title = result.title,
                        TitleNosign = result.title_nosign,
                        Description = result.description,
                        Alias = result.alias,
                        ShopId = result.shop_id,
                        CreatedUserId = result.created_user_id,
                        Deadline = result.deadline,
                        StartDate = result.start_date,
                        Duration = result.duration,
                        CreatedAt = result.created_at,
                        UpdatedAt = result.updated_at,
                        CompletedAt = result.completed_at,
                        IsCompleted = result.is_completed,
                        CompletionPercentage = result.completion_percentage,
                    };
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Cập nhật tiêu đề sub-task thành công.";
                }
                else
                {
                    response.Code = ResponseResultEnum.NotFound.Value();
                    response.Message = "Không tìm thấy sub-task.";
                }
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "UpdateSubTaskTitle Exception id {0}, EX: {1}",
                    request != null ? request.Id.ToString() : "",
                    ex.ToString()
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        #endregion

        #region Task User & Manager Operations

        /// <summary>
        /// Lấy danh sách quản lý của task
        /// </summary>
        [HttpGet]
        [Route("managers")]
        public IHttpActionResult GetTaskManagers([FromUri] int? task_id = null)
        {
            var response = new ApiResult<List<UserSimpleResponse>>()
            {
                Data = new List<UserSimpleResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            return Content(HttpStatusCode.OK, response);
        }

        /// <summary>
        /// Lấy danh sách người dùng được gán cho task
        /// </summary>
        [HttpGet]
        [Route("users")]
        public IHttpActionResult GetTaskUsers([FromUri] int? task_id = null)
        {
            var response = new ApiResult<List<UserSimpleResponse>>()
            {
                Data = new List<UserSimpleResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            return Content(HttpStatusCode.OK, response);
        }

        /// <summary>
        /// Thêm collaborators cho task
        /// </summary>
        /// <param name="request">Thông tin thêm collaborators</param>
        /// <returns>Danh sách collaborators đã thêm</returns>
        [HttpPost]
        [Route("add-collaborators")]
        public IHttpActionResult AddTaskCollaborators(
            [FromBody] AddTaskCollaboratorsRequestModel request
        )
        {
            var response = new ApiResult<List<TaskUserResponse>>()
            {
                Data = new List<TaskUserResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng nhập đủ thông tin.";
                    return Content(HttpStatusCode.OK, response);
                }

                if (string.IsNullOrEmpty(request.TaskId))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID task không được để trống.";
                    return Content(HttpStatusCode.OK, response);
                }

                var result = BoFactory.Task.AddTaskCollaborators(request.TaskId, request.UserIds);

                if (result > 0)
                {
                    // AddTaskCollaborators trả về số lượng collaborators đã thêm
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = $"Thêm {result} collaborators thành công.";
                }
                else
                {
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Không có collaborators mới được thêm.";
                }
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "AddTaskCollaborators Exception request {0}, EX:",
                    request != null ? request.TaskId : "",
                    ex
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Cập nhật assigned_user cho task
        /// </summary>
        [HttpPost]
        [Route("update-assigned-user")]
        public IHttpActionResult UpdateTaskAssignedUser(
            [FromBody] UpdateTaskAssignedUserRequestModel request
        )
        {
            var response = new ApiResult<UserSimpleResponse>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                if (request == null || request.TaskId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng nhập đủ thông tin.";
                    return Content(HttpStatusCode.OK, response);
                }

                var result = BoFactory.Task.UpdateTaskAssignedUser(
                    request.TaskId,
                    request.AssignedUser
                );

                if (result != null && result.Count > 0)
                {
                    var first = result.First();
                    // Nếu store trả về user thì map sang UserSimpleResponse
                    response.Data = null;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Cập nhật assigned_user thành công.";
                }
                else
                {
                    response.Code = ResponseResultEnum.NotFound.Value();
                    response.Message = "Task không tồn tại hoặc cập nhật thất bại.";
                }
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "UpdateTaskAssignedUser Exception request {0}, EX:",
                    request != null ? request.TaskId.ToString() : "",
                    ex
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        #endregion

        #region Task Field Operations

        /// <summary>
        /// Tạo task field mới
        /// </summary>
        [HttpPost]
        [Route("create-field")]
        public IHttpActionResult CreateTaskField([FromBody] CreateTaskFieldRequest request)
        {
            var response = new ApiResult<TaskFieldResponse>
            {
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            if (
                request == null
                || string.IsNullOrEmpty(request.Title)
                || string.IsNullOrEmpty(request.Type)
            )
            {
                response.Code = ResponseResultEnum.InvalidInput.Value();
                response.Message = "Vui lòng nhập đủ thông tin.";
                return Content(HttpStatusCode.OK, response);
            }

            try
            {
                var result = BoFactory.Task.CreateTaskField(
                    request.TaskId,
                    request.Title,
                    request.Type,
                    request.Description,
                    request.AddToLibrary,
                    request.NotifyOnChange
                );

                // Nếu có options và type là dropdown thì thêm options
                if (
                    result != null
                    && result.id > 0
                    && request.Options != null
                    && request.Options.Any()
                    && request.Type == "dropdown"
                )
                {
                    var optionsString = string.Join(
                        ";",
                        request.Options.Select(o => $"{o.Title}|{o.Color}|{o.SortIndex}")
                    );
                    BoFactory.Task.InsertTaskFieldOptionsBulk(result.id, optionsString);
                }

                // Map trực tiếp sang TaskFieldResponse
                response.Data =
                    result == null
                        ? null
                        : new TaskFieldResponse { Id = result.id, CreatedAt = result.created_at };
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Tạo thuộc tính thành công.";
            }
            catch (Exception ex)
            {
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ex.Message;
            }

            return Content(HttpStatusCode.OK, response);
        }

        #endregion

        #region Test & Utility Endpoints

        /// <summary>
        /// Test endpoint
        /// </summary>
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok("Task API is working!");
        }

        /// <summary>
        /// Test endpoint để kiểm tra groups của task
        /// </summary>
        [HttpGet]
        [Route("test-groups")]
        public IHttpActionResult TestTaskGroups([FromUri] int? task_id = null)
        {
            var response = new ApiResult<object>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                if (!task_id.HasValue || task_id.Value <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Task ID không hợp lệ.";
                    return Content(HttpStatusCode.OK, response);
                }

                var taskGroups = BoFactory.Task.GetTaskGroupsByTaskId(task_id.Value);
                var taskDetail = BoFactory.Task.GetTaskDetail(task_id.Value);

                // Tạo danh sách groups
                List<object> groupsList;
                if (taskGroups != null && taskGroups.Any())
                {
                    groupsList = taskGroups
                        .Select(g => new
                        {
                            Id = g.id,
                            Name = g.name,
                            TaskId = g.task_id,
                            Color = g.color,
                            Index = g.index,
                        })
                        .Cast<object>()
                        .ToList();
                }
                else
                {
                    groupsList = new List<object>();
                }

                response.Data = new
                {
                    TaskId = task_id.Value,
                    TaskDetail = taskDetail != null
                        ? new
                        {
                            Id = taskDetail.id,
                            Name = taskDetail.name,
                            Color = taskDetail.color,
                        }
                        : null,
                    GroupsCount = taskGroups?.Count ?? 0,
                    Groups = groupsList,
                };

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Test groups thành công.";
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "TestTaskGroups Exception task_id {0}, EX:",
                    task_id,
                    ex
                );
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Extract SQL error message từ EntityCommandExecutionException
        /// </summary>
        /// <param name="errorMessage">Error message từ exception</param>
        /// <returns>SQL error message đã được extract</returns>
        private string ExtractSqlErrorMessage(string errorMessage)
        {
            try
            {
                // Tìm message trong RAISERROR
                if (errorMessage.Contains("RAISERROR"))
                {
                    // Pattern: RAISERROR (N'Message tiếng Việt', 16, 1)
                    var startIndex = errorMessage.IndexOf("N'");
                    if (startIndex >= 0)
                    {
                        startIndex += 2; // Bỏ qua N'
                        var endIndex = errorMessage.IndexOf("'", startIndex);
                        if (endIndex > startIndex)
                        {
                            return errorMessage.Substring(startIndex, endIndex - startIndex);
                        }
                    }
                }

                // Fallback: trả về message gốc nếu không extract được
                return errorMessage;
            }
            catch
            {
                return errorMessage;
            }
        }

        /// <summary>
        /// Lấy giá trị property từ object một cách an toàn
        /// </summary>
        /// <param name="obj">Object cần lấy property</param>
        /// <param name="propertyName">Tên property</param>
        /// <returns>Giá trị property hoặc default value</returns>
        private T GetPropertyValue<T>(object obj, string propertyName)
        {
            try
            {
                if (obj == null)
                    return default(T);

                var property = obj.GetType().GetProperty(propertyName);
                if (property != null)
                {
                    var value = property.GetValue(obj);
                    if (value != null && value is T)
                    {
                        return (T)value;
                    }
                }
                return default(T);
            }
            catch
            {
                return default(T);
            }
        }

        #endregion
    }
}
