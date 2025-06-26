using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TanTamApi.Models.Response
{
    /// <summary>
    /// Response cho task detail
    /// </summary>
    public class TaskResponse
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
    }

    /// <summary>
    /// Response cho task group
    /// </summary>
    public class TaskGroupResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("bundle_id")]
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

        [JsonProperty("sub_tasks")]
        public List<SubTaskResponse> SubTasks { get; set; }
    }

    /// <summary>
    /// Response cho task user/manager
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
    /// Response cho task list với pagination
    /// </summary>
    public class TaskListResponse
    {
        [JsonProperty("items")]
        public List<TaskResponse> Items { get; set; }

        [JsonProperty("meta")]
        public TaskListMetaResponse Meta { get; set; }
    }

    /// <summary>
    /// Meta information cho task list
    /// </summary>
    public class TaskListMetaResponse
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

    /// <summary>
    /// Response cho sub-task list với pagination (moved from file riêng)
    /// </summary>
    public class SubTaskListResponse
    {
        [JsonProperty("items")]
        public List<SubTaskResponse> Items { get; set; }

        [JsonProperty("meta")]
        public TaskListMetaResponse Meta { get; set; }
    }

    /// <summary>
    /// Response cho sub-task (moved from file riêng)
    /// </summary>
    public class SubTaskResponse
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

        [JsonProperty("completed_at")]
        public DateTime? CompletedAt { get; set; }

        [JsonProperty("is_completed")]
        public bool? IsCompleted { get; set; }

        [JsonProperty("completion_percentage")]
        public int? CompletionPercentage { get; set; }

        [JsonProperty("created_user_obj")]
        public UserSimpleResponse CreatedUserObj { get; set; }

        [JsonProperty("customized_fields")]
        public Dictionary<string, TaskFieldDetailResponse> CustomizedFields { get; set; }
    }

    /// <summary>
    /// Response cho task field (moved from file riêng)
    /// </summary>
    public class TaskFieldResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("field_id")]
        public int FieldId { get; set; }

        [JsonProperty("value_text")]
        public string ValueText { get; set; }

        [JsonProperty("value_option_id")]
        public int? ValueOptionId { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// Response cho task field detail (dùng cho customized_fields)
    /// </summary>
    public class TaskFieldDetailResponse
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("title_nosign")]
        public string TitleNosign { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("sort_index")]
        public int SortIndex { get; set; }

        [JsonProperty("option_action")]
        public string OptionAction { get; set; }
    }

    /// <summary>
    /// Response cho task detail với đầy đủ thông tin
    /// </summary>
    public class TaskDetailResponse : TaskResponse
    {
        [JsonProperty("groups")]
        public List<TaskGroupDetailResponse> Groups { get; set; }

        [JsonProperty("managers")]
        public List<UserSimpleResponse> Managers { get; set; }

        [JsonProperty("users")]
        public List<UserSimpleResponse> Users { get; set; }

        [JsonProperty("creator")]
        public UserSimpleResponse Creator { get; set; }
    }

    /// <summary>
    /// Response cho task group detail với sub tasks
    /// </summary>
    public class TaskGroupDetailResponse : TaskGroupResponse
    {
        [JsonProperty("sub_tasks")]
        public new List<SubTaskDetailResponse> SubTasks { get; set; }
    }

    /// <summary>
    /// Response cho sub task detail với customized fields
    /// </summary>
    public class SubTaskDetailResponse : SubTaskResponse
    {
        [JsonProperty("customized_fields")]
        public new Dictionary<string, TaskFieldDetailResponse> CustomizedFields { get; set; }

        [JsonProperty("created_user_obj")]
        public new UserSimpleResponse CreatedUserObj { get; set; }
    }

    /// <summary>
    /// Response cho user đơn giản
    /// </summary>
    public class UserSimpleResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public int? Username { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("identification")]
        public string Identification { get; set; }

        [JsonProperty("branch_id")]
        public int? BranchId { get; set; }
    }
} 