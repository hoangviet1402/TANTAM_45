using System.Collections.Generic;
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
        public int TaskId { get; set; }

        [JsonProperty("collaborator_ids")]
        public List<int> UserIds { get; set; }
    }
} 