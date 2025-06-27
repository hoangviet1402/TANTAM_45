using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BussinessObject.Models.Task
{
    /// <summary>
    /// Request tạo task mới
    /// </summary>
    public class CreateTaskRequest
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
    public class TaskDetailRequest
    {
        [Required(ErrorMessage = "ID task không được để trống")]
        [JsonProperty("task_id")]
        public int TaskId { get; set; }
    }

    /// <summary>
    /// Request lấy danh sách task
    /// </summary>
    public class TaskListRequest
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
    public class CreateTaskGroupRequest
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
    /// Request tạo sub-task
    /// </summary>
    public class CreateTaskSubRequest
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
    public class TaskSubListRequest
    {
        [Required(ErrorMessage = "ID bundle không được để trống")]
        [JsonProperty("bundle_id")]
        public string BundleId { get; set; }
    }

    /// <summary>
    /// Request tạo task field với options
    /// </summary>
    public class CreateTaskFieldRequest
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
    /// Response cho task detail
    /// </summary>
    public class TaskDetailResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("favored")]
        public bool? Favored { get; set; }

        [JsonProperty("view_id")]
        public string ViewId { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("is_archived")]
        public bool? IsArchived { get; set; }

        [JsonProperty("created_user_id")]
        public int? CreatedUserId { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("default_deadline_time")]
        public string DefaultDeadlineTime { get; set; }

        [JsonProperty("default_start_time")]
        public string DefaultStartTime { get; set; }

        [JsonProperty("task_done")]
        public int TaskDone { get; set; }

        [JsonProperty("task_count")]
        public int TaskCount { get; set; }

        [JsonProperty("task_overdue")]
        public int TaskOverdue { get; set; }

        [JsonProperty("user_count")]
        public int UserCount { get; set; }
    }

    /// <summary>
    /// Response cho task list
    /// </summary>
    public class TaskListResponse
    {
        [JsonProperty("items")]
        public List<TaskDetailResponse> Items { get; set; } = new List<TaskDetailResponse>();

        [JsonProperty("meta")]
        public MetaResponse Meta { get; set; } = new MetaResponse();
    }

    /// <summary>
    /// Response cho task group
    /// </summary>
    public class TaskGroupResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("task_id")]
        public int TaskId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("index")]
        public int? Index { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// Response cho task manager/user
    /// </summary>
    public class TaskUserResponse
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("department_id")]
        public int? DepartmentId { get; set; }

        [JsonProperty("branch_id")]
        public int? BranchId { get; set; }
    }

    /// <summary>
    /// Response cho sub-task
    /// </summary>
    public class TaskSubResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("ordinal_number")]
        public int? OrdinalNumber { get; set; }

        [JsonProperty("bundle_id")]
        public int? BundleId { get; set; }

        [JsonProperty("sort_index")]
        public int? SortIndex { get; set; }

        [JsonProperty("private_sort_index")]
        public int? PrivateSortIndex { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("title_nosign")]
        public string TitleNosign { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("shop_id")]
        public int? ShopId { get; set; }

        [JsonProperty("created_user_id")]
        public int? CreatedUserId { get; set; }

        [JsonProperty("deadline")]
        public DateTime? Deadline { get; set; }

        [JsonProperty("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("duration")]
        public int? Duration { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// Response cho task creator info
    /// </summary>
    public class TaskCreatorResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public int? Username { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("identification")]
        public int? Identification { get; set; }

        [JsonProperty("branch_id")]
        public int? BranchId { get; set; }
    }

    /// <summary>
    /// Meta response cho pagination
    /// </summary>
    public class MetaResponse
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