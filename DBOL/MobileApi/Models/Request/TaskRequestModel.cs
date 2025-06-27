using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TanTamApi.Models.Request;

namespace TanTamApi.Models.Request
{
    /// <summary>
    /// Request tạo task mới
    /// </summary>
    public class CreateTaskRequestModel
    {
        [Required(ErrorMessage = "Tiêu đề task không được để trống")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "ID người tạo không được để trống")]
        [JsonProperty("created_user_obj")]
        public int CreatedUserObj { get; set; }

        [JsonProperty("default_view")]
        public string DefaultView { get; set; } = "list";

        [JsonProperty("color")]
        public string Color { get; set; } = "#cccccc";

        [JsonProperty("department_ids")]
        public string DepartmentIds { get; set; }

        [JsonProperty("position_ids")]
        public string PositionIds { get; set; }

        [JsonProperty("branch_ids")]
        public string BranchIds { get; set; }

        [JsonProperty("user_ids")]
        public string UserIds { get; set; }
    }

    /// <summary>
    /// Request lấy chi tiết task
    /// </summary>
    public class TaskDetailRequestModel
    {
        [Required(ErrorMessage = "ID task không được để trống")]
        [JsonProperty("task_id")]
        public int TaskId { get; set; }
    }

    /// <summary>
    /// Request lấy danh sách task
    /// </summary>
    public class TaskListRequestModel : ApiBaseRequest
    {
        [JsonProperty("task_id")]
        public int? TaskId { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; } = 1;

        [JsonProperty("limit")]
        public int Limit { get; set; } = 10;
    }

    /// <summary>
    /// Request tạo task group
    /// </summary>
    public class CreateTaskGroupRequestModel
    {
        [Required(ErrorMessage = "ID bundle không được để trống")]
        [JsonProperty("bundle_id")]
        public int BundleId { get; set; }

        [Required(ErrorMessage = "Tên nhóm không được để trống")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; } = "#cccccc";

        [JsonProperty("position")]
        public string Position { get; set; }
    }

    /// <summary>
    /// Request cập nhật tên task group
    /// </summary>
    public class UpdateTaskGroupNameRequestModel
    {
        [Required(ErrorMessage = "ID group không được để trống")]
        [JsonProperty("group_id")]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Tên group không được để trống")]
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Request tạo sub-task
    /// </summary>
    public class CreateTaskSubRequestModel : ApiBaseRequest
    {
        [Required(ErrorMessage = "Tiêu đề sub-task không được để trống")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("bundle_id")]
        public int? BundleId { get; set; }

        [JsonProperty("created_user_id")]
        public int? CreatedUserId { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }
    }

    /// <summary>
    /// Request lấy sub-tasks theo bundle
    /// </summary>
    public class TaskSubListRequestModel : ApiBaseRequest
    {
        [Required(ErrorMessage = "ID bundle không được để trống")]
        [JsonProperty("bundle_id")]
        public string BundleId { get; set; }
    }

    /// <summary>
    /// Request tạo task field với options
    /// </summary>
    public class CreateTaskFieldRequestModel : ApiBaseRequest
    {
        [Required(ErrorMessage = "ID đối tượng không được để trống")]
        [JsonProperty("object_id")]
        public int ObjectId { get; set; }

        [Required(ErrorMessage = "Tiêu đề trường không được để trống")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("title_nosign")]
        public string TitleNosign { get; set; }

        [JsonProperty("add_to_lib")]
        public bool? AddToLib { get; set; }

        [JsonProperty("notify_when_value_changed")]
        public bool? NotifyWhenValueChanged { get; set; }

        [JsonProperty("field_key")]
        public string FieldKey { get; set; }

        [JsonProperty("is_default")]
        public bool? IsDefault { get; set; }

        [JsonProperty("created_user_id")]
        public int? CreatedUserId { get; set; }

        [JsonProperty("sort_index")]
        public int? SortIndex { get; set; }

        [JsonProperty("active")]
        public bool? Active { get; set; }

        [JsonProperty("object_sort_index")]
        public int? ObjectSortIndex { get; set; }

        [JsonProperty("object_active")]
        public bool? ObjectActive { get; set; }
    }

    /// <summary>
    /// Request lấy task groups theo task ID
    /// </summary>
    public class TaskGroupsRequestModel : ApiBaseRequest
    {
        [Required(ErrorMessage = "ID task không được để trống")]
        [JsonProperty("task_id")]
        public int TaskId { get; set; }
    }

    /// <summary>
    /// Request lấy task managers theo task ID
    /// </summary>
    public class TaskManagersRequestModel : ApiBaseRequest
    {
        [Required(ErrorMessage = "ID task không được để trống")]
        [JsonProperty("task_id")]
        public int TaskId { get; set; }
    }

    /// <summary>
    /// Request lấy task users theo task ID
    /// </summary>
    public class TaskUsersRequestModel : ApiBaseRequest
    {
        [Required(ErrorMessage = "ID task không được để trống")]
        [JsonProperty("task_id")]
        public int TaskId { get; set; }
    }

    /// <summary>
    /// Request lấy tasks theo user ID
    /// </summary>
    public class UserTasksRequestModel : ApiBaseRequest
    {
        [Required(ErrorMessage = "ID người dùng không được để trống")]
        [JsonProperty("user_id")]
        public int UserId { get; set; }
    }

    /// <summary>
    /// Request lấy thông tin người tạo task
    /// </summary>
    public class TaskCreatorInfoRequestModel : ApiBaseRequest
    {
        [Required(ErrorMessage = "ID task không được để trống")]
        [JsonProperty("task_id")]
        public int TaskId { get; set; }
    }

    /// <summary>
    /// Request cập nhật task
    /// </summary>
    public class UpdateTaskRequestModel : ApiBaseRequest
    {
        [Required(ErrorMessage = "ID task không được để trống")]
        [JsonProperty("task_id")]
        public int TaskId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("view_id")]
        public string ViewId { get; set; }

        [JsonProperty("department_ids")]
        public string DepartmentIds { get; set; }

        [JsonProperty("position_ids")]
        public string PositionIds { get; set; }

        [JsonProperty("branch_ids")]
        public string BranchIds { get; set; }

        [JsonProperty("user_ids")]
        public string UserIds { get; set; }
    }

    /// <summary>
    /// Request xóa task
    /// </summary>
    public class DeleteTaskRequestModel : ApiBaseRequest
    {
        [Required(ErrorMessage = "ID task không được để trống")]
        [JsonProperty("task_id")]
        public int TaskId { get; set; }
    }

    /// <summary>
    /// Request lưu trữ/khôi phục task
    /// </summary>
    public class ArchiveTaskRequestModel : ApiBaseRequest
    {
        [Required(ErrorMessage = "ID task không được để trống")]
        [JsonProperty("task_id")]
        public int TaskId { get; set; }

        [JsonProperty("is_archived")]
        public bool IsArchived { get; set; } = true;
    }

    /// <summary>
    /// Request thêm/xóa task yêu thích
    /// </summary>
    public class FavoriteTaskRequestModel : ApiBaseRequest
    {
        [Required(ErrorMessage = "ID task không được để trống")]
        [JsonProperty("task_id")]
        public int TaskId { get; set; }

        [JsonProperty("is_favored")]
        public bool IsFavored { get; set; } = true;
    }

    /// <summary>
    /// Request tìm kiếm task
    /// </summary>
    public class SearchTaskRequestModel : ApiBaseRequest
    {
        [JsonProperty("keyword")]
        public string Keyword { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; } = 1;

        [JsonProperty("limit")]
        public int Limit { get; set; } = 10;

        [JsonProperty("status")]
        public string Status { get; set; } // "all", "active", "archived"

        [JsonProperty("sort_by")]
        public string SortBy { get; set; } = "created_at"; // "created_at", "updated_at", "title"

        [JsonProperty("sort_order")]
        public string SortOrder { get; set; } = "desc"; // "asc", "desc"
    }

    /// <summary>
    /// Request thống kê task
    /// </summary>
    public class TaskStatsRequestModel : ApiBaseRequest
    {
        [JsonProperty("user_id")]
        public int? UserId { get; set; }

        [JsonProperty("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("end_date")]
        public DateTime? EndDate { get; set; }
    }

    /// <summary>
    /// Request import task từ file
    /// </summary>
    public class ImportTaskRequestModel : ApiBaseRequest
    {
        [Required(ErrorMessage = "File không được để trống")]
        [JsonProperty("file_content")]
        public string FileContent { get; set; }

        [JsonProperty("file_type")]
        public string FileType { get; set; } = "csv"; // "csv", "excel"

        [JsonProperty("created_user_obj")]
        public int CreatedUserObj { get; set; }
    }

    /// <summary>
    /// Request export task
    /// </summary>
    public class ExportTaskRequestModel : ApiBaseRequest
    {
        [JsonProperty("task_ids")]
        public List<int> TaskIds { get; set; }

        [JsonProperty("export_type")]
        public string ExportType { get; set; } = "csv"; // "csv", "excel", "pdf"

        [JsonProperty("include_details")]
        public bool IncludeDetails { get; set; } = true;
    }

    /// <summary>
    /// Request bulk operations cho task
    /// </summary>
    public class BulkTaskOperationRequestModel : ApiBaseRequest
    {
        [Required(ErrorMessage = "Danh sách ID task không được để trống")]
        [JsonProperty("task_ids")]
        public List<int> TaskIds { get; set; }

        [Required(ErrorMessage = "Loại thao tác không được để trống")]
        [JsonProperty("operation")]
        public string Operation { get; set; } // "archive", "unarchive", "delete", "assign_users", "change_color"

        [JsonProperty("parameters")]
        public Dictionary<string, object> Parameters { get; set; }
    }

    /// <summary>
    /// Request gán người dùng cho task
    /// </summary>
    public class AssignUsersToTaskRequestModel : ApiBaseRequest
    {
        [Required(ErrorMessage = "ID task không được để trống")]
        [JsonProperty("task_id")]
        public int TaskId { get; set; }

        [JsonProperty("user_ids")]
        public List<int> UserIds { get; set; }

        [JsonProperty("remove_existing")]
        public bool RemoveExisting { get; set; } = false;
    }

    /// <summary>
    /// Request thay đổi quyền quản lý task
    /// </summary>
    public class ChangeTaskManagerRequestModel : ApiBaseRequest
    {
        [Required(ErrorMessage = "ID task không được để trống")]
        [JsonProperty("task_id")]
        public int TaskId { get; set; }

        [JsonProperty("manager_ids")]
        public List<int> ManagerIds { get; set; }

        [JsonProperty("remove_existing")]
        public bool RemoveExisting { get; set; } = false;
    }

    /// <summary>
    /// Request check done sub-task
    /// </summary>
    public class CheckDoneSubTaskRequestModel : ApiBaseRequest
    {
        [Required(ErrorMessage = "ID sub-task không được để trống")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Trạng thái hoàn thành không được để trống")]
        [JsonProperty("is_completed")]
        public bool IsCompleted { get; set; }
    }

    /// <summary>
    /// Request tạo sub-task (moved from file riêng)
    /// </summary>
    public class CreateSubTaskRequestModel
    {
        [JsonProperty("bundle_id")]
        public int BundleId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("created_user_id")]
        public int? CreatedUserId { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }
    }

    /// <summary>
    /// Request cập nhật deadline của sub-task (moved from file riêng)
    /// </summary>
    public class UpdateSubTaskDeadlineRequestModel
    {
        [Required(ErrorMessage = "ID sub-task không được để trống")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("deadline")]
        public DateTime? Deadline { get; set; }

        [JsonProperty("start_date")]
        public DateTime? StartDate { get; set; }
    }

    /// <summary>
    /// Request cập nhật assigned_user cho task (moved from file riêng)
    /// </summary>
    public class UpdateTaskAssignedUserRequestModel
    {
        [Required(ErrorMessage = "ID task không được để trống")]
        [JsonProperty("task_id")]
        public int TaskId { get; set; }

        [JsonProperty("assigned_user")]
        public int? AssignedUser { get; set; }
    }

    /// <summary>
    /// Request tạo task field với options
    /// </summary>
    public class TaskFieldOptionRequest
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("color")]
        public string Color { get; set; }
        [JsonProperty("sort_index")]
        public int SortIndex { get; set; }
    }

    public class CreateTaskFieldRequest
    {
        [Required(ErrorMessage = "ID task không được để trống")]
        [JsonProperty("task_id")]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Tên thuộc tính không được để trống")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Loại thuộc tính không được để trống")]
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("add_to_library")]
        public bool? AddToLibrary { get; set; }

        [JsonProperty("notify_on_change")]
        public bool? NotifyOnChange { get; set; }

        [JsonProperty("options")]
        public List<TaskFieldOptionRequest> Options { get; set; }
    }

    /// <summary>
    /// Request cập nhật tiêu đề sub-task
    /// </summary>
    public class UpdateSubTaskTitleRequestModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
} 