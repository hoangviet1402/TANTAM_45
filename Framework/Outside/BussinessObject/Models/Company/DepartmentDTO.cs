using System.Collections.Generic;
using Newtonsoft.Json;

namespace BussinessObject.Models.Company
{
    public class CreateDepartmentInAllBranchesRequest
    {
        public List<string> Names { get; set; }
        public int CompanyId { get; set; }
    }


    public class CreateDepartmentRequest
    {
        [JsonProperty("names", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Name { get; set; }

        [JsonProperty("branchid", NullValueHandling = NullValueHandling.Ignore)]
        public int BranchId { get; set; }

        [JsonProperty("is_onboarding", NullValueHandling = NullValueHandling.Ignore)]
        public int IsOnboarding { get; set; }
    }

    public class CreateDepartmentResponse
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

        [JsonProperty("branch_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> BranchIds { get; set; }

        [JsonProperty("branchs", NullValueHandling = NullValueHandling.Ignore)]
        public List<DepartmentsBranchsResponseList> Branchs { get; set; }

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

    public class DepartmentsBranchsResponseList
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }
    }
}