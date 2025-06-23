using Newtonsoft.Json;
using System;

namespace TanTamApi.Models.Response
{
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

        [JsonProperty("created_user_obj")]
        public UserSimpleResponse CreatedUserObj { get; set; }
    }
} 