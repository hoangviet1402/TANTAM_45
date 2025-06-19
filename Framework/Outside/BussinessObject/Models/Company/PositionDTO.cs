using System.Collections.Generic;
using Newtonsoft.Json;

namespace BussinessObject.Models.Company
{
    public class CreatePositionRequest
    {
        public string Name { get; set; }
        public int PosisionId { get; set; }
        public int BrandId { get; set; }
        public int CompanyId { get; set; }
    }

    public class CreatePosisionInAllBranchesRequest
    {
        public List<CreatePosisionRequest> Posisions { get; set; }
        public int CompanyId { get; set; }
        public int ExpYear { get; set; }
    }

    public class CreatePosisionRequest
    {
        public string Names { get; set; }
        public int ExpYear { get; set; }
    }


    public class CreatePosisionResponse
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }

        [JsonProperty("shop_id", NullValueHandling = NullValueHandling.Ignore)]
        public int ShopId { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("academic_level", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> AcademicLevel { get; set; }

        [JsonProperty("exp_year", NullValueHandling = NullValueHandling.Ignore)]
        public int ExpYear { get; set; }

        [JsonProperty("branch_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> BranchIds { get; set; }

        [JsonProperty("branchs", NullValueHandling = NullValueHandling.Ignore)]
        public List<PosisionsBranchsResponseList> Branchs { get; set; }

        [JsonProperty("parent_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? ParentId { get; set; }

        [JsonProperty("sort_index", NullValueHandling = NullValueHandling.Ignore)]
        public int SortIndex { get; set; }

        [JsonProperty("parent", NullValueHandling = NullValueHandling.Ignore)]
        public object Parent { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty("alias", NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }

        [JsonProperty("is_head", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsHead { get; set; }

        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
    }

    public class PosisionsBranchsResponseList
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }
    }
}