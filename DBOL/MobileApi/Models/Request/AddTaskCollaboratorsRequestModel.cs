using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TanTamApi.Models.Request
{
    /// <summary>
    /// Request thêm collaborators cho task
    /// </summary>
    public class AddTaskCollaboratorsRequestModel
    {
        [Required(ErrorMessage = "ID task không được để trống")]
        [JsonProperty("task_id")]
        public string TaskId { get; set; }

        [JsonProperty("user_ids")]
        public string UserIds { get; set; }
    }
} 