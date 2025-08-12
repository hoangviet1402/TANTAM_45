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
        [JsonProperty("positions", NullValueHandling = NullValueHandling.Ignore)]
        public List<CreatePosisionRequest> Positions { get; set; }
        [JsonProperty("company_id", NullValueHandling = NullValueHandling.Ignore)]
        public int CompanyId { get; set; }
        [JsonProperty("exp_year", NullValueHandling = NullValueHandling.Ignore)]
        public int ExpYear { get; set; }
        [JsonProperty("is_onboarding", NullValueHandling = NullValueHandling.Ignore)]
        public int IsOnboarding { get; set; }
        [JsonProperty("branch_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> BranchIds { get; set; }
        [JsonProperty("department_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> DepartmentIds { get; set; }
    }

    public class CreatePosisionRequest
    {
        [JsonProperty("names", NullValueHandling = NullValueHandling.Ignore)]
        public string Names { get; set; }
        [JsonProperty("exp_year", NullValueHandling = NullValueHandling.Ignore)]
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
        public List<PositionsBranchsResponseList> Branchs { get; set; }

        [JsonProperty("department_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> DepartmentIds { get; set; }

        [JsonProperty("departments", NullValueHandling = NullValueHandling.Ignore)]
        public List<PositionsDepartmentsResponseList> Departments { get; set; }

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

    public class PositionsBranchsResponseList
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }
    }

    public class PositionByBranchResponse
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int id { get; set; }
        
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }
        
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string code { get; set; }
        
        [JsonProperty("sort_index", NullValueHandling = NullValueHandling.Ignore)]
        public int? sort_index { get; set; }
        
        [JsonProperty("academic_level", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> academic_level { get; set; }
        
        [JsonProperty("exp_year", NullValueHandling = NullValueHandling.Ignore)]
        public int? exp_year { get; set; }
        
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string description { get; set; }
        
        [JsonProperty("alias", NullValueHandling = NullValueHandling.Ignore)]
        public string alias { get; set; }

        [JsonProperty("branch_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> branch_ids { get; set; }

        [JsonProperty("branchs", NullValueHandling = NullValueHandling.Ignore)]
        public List<PositionsBranchsResponseList> branchs { get; set; }

        [JsonProperty("department_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> department_ids { get; set; }

        [JsonProperty("departments", NullValueHandling = NullValueHandling.Ignore)]
        public List<PositionsDepartmentsResponseList> departments { get; set; }
    }

    public class PositionsDepartmentsResponseList
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }
    }

    public class PositionListDetailResponse
    {
        [JsonProperty("meta", NullValueHandling = NullValueHandling.Include)]
        public BranchesMetaReponse Meta { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<PositionByBranchResponse> Items { get; set; }
    }

    // =============================================
    // UPDATE POSITION MODELS
    // =============================================

    public class UpdatePositionRequest
    {
        [JsonProperty("position_id", NullValueHandling = NullValueHandling.Ignore)]
        public int PositionId { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("alias", NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty("exp_year", NullValueHandling = NullValueHandling.Ignore)]
        public int? ExpYear { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("sort_index", NullValueHandling = NullValueHandling.Ignore)]
        public int? SortIndex { get; set; }

        [JsonProperty("branch_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> BranchIds { get; set; }

        [JsonProperty("department_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> DepartmentIds { get; set; }
    }

    public class UpdatePositionResponse
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("alias", NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty("exp_year", NullValueHandling = NullValueHandling.Ignore)]
        public int? ExpYear { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("sort_index", NullValueHandling = NullValueHandling.Ignore)]
        public int? SortIndex { get; set; }

        [JsonProperty("branch_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> BranchIds { get; set; }

        [JsonProperty("branchs", NullValueHandling = NullValueHandling.Ignore)]
        public List<PositionsBranchsResponseList> Branchs { get; set; }

        [JsonProperty("department_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> DepartmentIds { get; set; }

        [JsonProperty("departments", NullValueHandling = NullValueHandling.Ignore)]
        public List<PositionsDepartmentsResponseList> Departments { get; set; }
    }


}