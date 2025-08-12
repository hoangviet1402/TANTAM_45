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
    //public class CreateTaskRequestModel
    //{
    //    [Required(ErrorMessage = "Tiêu đề task không được để trống")]
    //    [JsonProperty("title")]
    //    public string Title { get; set; }

    //    [JsonProperty("default_view")]
    //    public string DefaultView { get; set; } = "list";

    //    [JsonProperty("color")]
    //    public string Color { get; set; } = "#cccccc";

    //    [JsonProperty("department_ids")]
    //    public string DepartmentIds { get; set; }

    //    [JsonProperty("position_ids")]
    //    public string PositionIds { get; set; }

    //    [JsonProperty("branch_ids")]
    //    public string BranchIds { get; set; }

    //    [JsonProperty("user_ids")]
    //    public string UserIds { get; set; }
    //}

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
    /// Request cập nhật màu sắc task group
    /// </summary>
    public class UpdateTaskGroupColorRequestModel
    {
        [Required(ErrorMessage = "ID group không được để trống")]
        [JsonProperty("group_id")]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Màu sắc không được để trống")]
        [JsonProperty("color")]
        public string Color { get; set; }
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
    //public class CreateTaskFieldRequestModel : ApiBaseRequest
    //{
    //    [Required(ErrorMessage = "ID đối tượng không được để trống")]
    //    [JsonProperty("object_id")]
    //    public int ObjectId { get; set; }

    //    [Required(ErrorMessage = "Tiêu đề trường không được để trống")]
    //    [JsonProperty("title")]
    //    public string Title { get; set; }

    //    [JsonProperty("title_nosign")]
    //    public string TitleNosign { get; set; }

    //    [JsonProperty("add_to_lib")]
    //    public bool? AddToLib { get; set; }

    //    [JsonProperty("notify_when_value_changed")]
    //    public bool? NotifyWhenValueChanged { get; set; }

    //    [JsonProperty("field_key")]
    //    public string FieldKey { get; set; }

    //    [JsonProperty("is_default")]
    //    public bool? IsDefault { get; set; }

    //    [JsonProperty("created_user_id")]
    //    public int? CreatedUserId { get; set; }

    //    [JsonProperty("sort_index")]
    //    public int? SortIndex { get; set; }

    //    [JsonProperty("active")]
    //    public bool? Active { get; set; }

    //    [JsonProperty("object_sort_index")]
    //    public int? ObjectSortIndex { get; set; }

    //    [JsonProperty("object_active")]
    //    public bool? ObjectActive { get; set; }
    //}

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

        [JsonProperty("assigned_id")]
        public int? AssignedId { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("deadline_date")]
        public string DeadlineDate { get; set; }
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

        [JsonProperty("deadline_time")]
        public string DeadlineTime { get; set; }

        [JsonProperty("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("looping")]
        public bool Looping { get; set; }
    }

    /// <summary>
    /// Request cập nhật assigned_user cho task (moved from file riêng)
    /// </summary>
    public class UpdateTaskAssignedUserRequestModel
    {
        [Required(ErrorMessage = "ID task không được để trống")]
        [JsonProperty("task_id")]
        public int TaskId { get; set; }

        [JsonProperty("assigned_id")]
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

    /// <summary>
    /// Request lấy danh sách sub-task theo subtask_id
    /// </summary>
    public class TaskSubListBySubTaskIdRequestModel
    {
        [JsonProperty("subtask_id")]
        public int SubTaskId { get; set; }
    }

    /// <summary>
    /// Request cập nhật mô tả sub-task
    /// </summary>
    public class UpdateSubTaskDescriptionRequestModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Request cho TaskBundle list
    /// </summary>
    public class TaskBundleListRequestModel
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "project";

        [JsonProperty("is_all")]
        public bool IsAll { get; set; } = true;
    }

    /// <summary>
    /// Request cho TaskBundle list-group-task
    /// </summary>
    public class TaskBundleGroupTaskRequestModel
    {
        [JsonProperty("bundle_id")]
        public int BundleId { get; set; }

        [JsonProperty("task_level")]
        public string[] TaskLevel { get; set; }
    }

    /// <summary>
    /// Request cho TaskBundle update-status
    /// </summary>
    public class TaskBundleUpdateStatusRequestModel
    {
        [JsonProperty("status_id")]
        public int StatusId { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Request cho UpdateTaskSubCompleted
    /// </summary>
    public class UpdateTaskSubCompletedRequestModel
    {
        [JsonProperty("task_id")]
        public int TaskId { get; set; }

        [JsonProperty("is_done")]
        public bool IsDone { get; set; }
    }

    /// <summary>
    /// Request cho API lấy danh sách user theo bundle_id
    /// </summary>
    public class TaskBundleListUserRequestModel
    {
        [JsonProperty("bundle_id")]
        public int BundleId { get; set; }

        [JsonProperty("is_all")]
        public bool IsAll { get; set; } = true;
    }

    public class TaskBundleCreateRequestModel
    {
        public string Name { get; set; }
        public List<string> DepartmentIds { get; set; }
        public List<string> PositionIds { get; set; }
        public List<string> BranchIds { get; set; }
        public List<string> UserIds { get; set; }
        public string DefaultView { get; set; }
    }

    public class SetTaskBundleFavoriteRequestModel
    {
        public int BundleId { get; set; }
        public bool IsFavorite { get; set; }
    }

    /// <summary>
    /// Request tạo task bundle status (group) mới
    /// </summary>
    public class CreateTaskBundleStatusRequestModel
    {
        [Required(ErrorMessage = "Bundle ID không được để trống")]
        [JsonProperty("bundle_id")]
        public int BundleId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = "Không tiêu đề";

        [JsonProperty("color")]
        public string Color { get; set; } = "#cccccc";

        [JsonProperty("position")]
        public string Position { get; set; } = "last";
    }

    /// <summary>
    /// Request model cho API xóa task bundle status
    /// </summary>
    public class DeleteTaskBundleStatusRequestModel
    {
        [Required(ErrorMessage = "Status ID không được để trống")]
        [JsonProperty("status_id")]
        public int StatusId { get; set; }
    }

    /// <summary>
    /// Request model cho API tạo task
    /// </summary>
    public class CreateTaskRequestModel
    {
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string DeadlineDate { get; set; }

        [Required(ErrorMessage = "Bundle ID không được để trống")]
        public int BundleId { get; set; }

        [Required(ErrorMessage = "Status ID không được để trống")]
        public int StatusId { get; set; }

        public string Position { get; set; }
    }

    /// <summary>
    /// Request model cho API cập nhật task
    /// </summary>
    public class UpdateTaskRequestModel
    {
        [Required(ErrorMessage = "ID task không được để trống")]
        public string Id { get; set; }

        public string Title { get; set; }

        public string UnsignedTitle { get; set; }

        public string Description { get; set; }

        public string BundleId { get; set; }

        public string StatusId { get; set; }

        public string SectionId { get; set; }

        public DateTime? Deadline { get; set; }

        public DateTime? StartDate { get; set; }

        public int? Duration { get; set; }

        public int? Percent { get; set; }

        public int? SortIndex { get; set; }

        public int? OrdinalNumber { get; set; }

        public bool? IsDone { get; set; }

        public bool? IsMilestone { get; set; }

        public bool? IsForm { get; set; }

        public bool? IsTaskApproval { get; set; }

        public bool? IsLooping { get; set; }

        public string LoopType { get; set; }

        public int? LoopInterval { get; set; }

        public string RecurringType { get; set; }

        public string DoneNote { get; set; }

        public string Location { get; set; }

        public int? FileCount { get; set; }

        public int? SubTaskDone { get; set; }

        public int? CommentCount { get; set; }

        public DateTime? CreatedAt { get; set; }

        public List<string> Collaborators { get; set; }

        public List<string> Permission { get; set; }

        public List<object> Forms { get; set; }

        public List<object> FormInfo { get; set; }

        public object RecurringConfig { get; set; }

        public object CoverImageObj { get; set; }

        public string TaskId { get; set; }
    }

    public class UpdateSubTaskPriorityRequestModel
    {
        public int task_id { get; set; }
        public int? priority_id { get; set; }
    }

    public class UpdateTaskCustomFieldsRequestModel
    {
        public int task_id { get; set; }
        public int customized_field_id { get; set; }
        public string value { get; set; }
    }

    public class TaskBundleUpdateRequestModel
    {
        public string bundle_id { get; set; }
        public string name { get; set; }
        public string default_view { get; set; }
        public List<string> department_ids { get; set; }
        public List<string> position_ids { get; set; }
        public List<string> branch_ids { get; set; }
        public List<string> user_ids { get; set; }
    }

    /// <summary>
    /// Request cập nhật status cho subtask (update-status)
    /// </summary>
    public class UpdateTaskSubStatusRequestModel
    {
        [JsonProperty("status_id")]
        public int StatusId { get; set; }

        [JsonProperty("task_id")]
        public int TaskId { get; set; }

        [JsonProperty("sort_index")]
        public int SortIndex { get; set; }

        [JsonProperty("sort_task_id")]
        public int SortTaskId { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }
    }

    public class SetTaskBundleIconRequestModel
    {
        public int bundle_id { get; set; }
        public string color { get; set; }
        public string icon { get; set; }
    }

    public class CreateTaskLabelRequestModel
    {
        public string name { get; set; }
        public string color { get; set; }
        public int bundle_id { get; set; }
        public int? user_id { get; set; }
        public int? sort_index { get; set; }
        public string title { get; set; }
    }

    public class SetTaskLabelRequestModel
    {
        public int task_id { get; set; }
        public List<int> label_ids { get; set; }
    }

    public class OptionDto
    {
        public string title { get; set; }
        public string color { get; set; }
        public int sort_index { get; set; }
        public string title_nosign { get; set; }
    }

    public class CreateTaskFieldRequestModel
    {
        public string key { get; set; }
        public List<OptionDto> options { get; set; }
        public bool add_to_lib { get; set; }
        public bool notify_when_value_changed { get; set; }
        public bool only_created_user_edit { get; set; }
        public string title { get; set; }
        public int object_id { get; set; }
        public bool active { get; set; }
        public string source { get; set; }
    }

    /// <summary>
    /// Request xóa task label
    /// </summary>
    public class DeleteTaskLabelRequestModel
    {
        [Required(ErrorMessage = "ID label không được để trống")]
        [JsonProperty("label_id")]
        public int LabelId { get; set; }
    }

    public class UpdateTaskLabelRequestModel
    {
        [Required(ErrorMessage = "ID label không được để trống")]
        [JsonProperty("label_id")]
        public int LabelId { get; set; }

        [Required(ErrorMessage = "Tên label không được để trống")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Màu sắc không được để trống")]
        [JsonProperty("color")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Bundle ID không được để trống")]
        [JsonProperty("bundle_id")]
        public int BundleId { get; set; }
    }

    /// <summary>
    /// Request model cho cập nhật task field
    /// </summary>
    public class UpdateTaskFieldRequestModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string title_nosign { get; set; }
        public int add_to_lib { get; set; }
        public int notify_when_value_changed { get; set; }
        public string key { get; set; }
        public bool? is_default { get; set; }
        public int used_objects { get; set; }
        public int sort_index { get; set; }
        public Dictionary<string, object> objects { get; set; }
        public int object_id { get; set; }
        public int active { get; set; }
        public List<UpdateTaskFieldOptionModel> options { get; set; }
        public Dictionary<string, UpdateTaskFieldOptionModel> option_by_id { get; set; }
        public Dictionary<string, UpdateTaskFieldOptionModel> option_by_title_nosign { get; set; }
        public int only_created_user_edit { get; set; }
        public string source { get; set; }
        public int update_option { get; set; }
    }

    /// <summary>
    /// Request model cho task field option
    /// </summary>
    public class UpdateTaskFieldOptionModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string color { get; set; }
        public int sort_index { get; set; }
        public string title_nosign { get; set; }
        public string action_option { get; set; }
    }

    /// <summary>
    /// Request model cho xóa task field
    /// </summary>
    public class DeleteTaskFieldRequestModel
    {
        public int id { get; set; }
        public string source { get; set; }
        public int object_id { get; set; }
    }

    /// <summary>
    /// Request bật/tắt field
    /// </summary>
    public class TurnOnOffTaskFieldRequestModel
    {
        [Required(ErrorMessage = "Trạng thái active không được để trống")]
        [JsonProperty("active")]
        public bool active { get; set; }

        [Required(ErrorMessage = "ID field không được để trống")]
        [JsonProperty("id")]
        public int id { get; set; }

        [Required(ErrorMessage = "Source không được để trống")]
        [JsonProperty("source")]
        public string source { get; set; }

        [Required(ErrorMessage = "Object ID không được để trống")]
        [JsonProperty("object_id")]
        public int object_id { get; set; }
    }
}