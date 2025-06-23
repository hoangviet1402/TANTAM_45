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

        [JsonProperty("groups")]
        public List<TaskGroupResponse> Groups { get; set; }
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
} 