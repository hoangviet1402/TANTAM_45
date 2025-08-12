using System.Collections.Generic;
using Newtonsoft.Json;

namespace BussinessObject.Models.Company

{
    public class CreateRegionRequest
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    }

    public class CreateBranchesRequest
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        [JsonProperty("region_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? RegionId { get; set; }

        [JsonProperty("is_onboarding", NullValueHandling = NullValueHandling.Ignore)]
        public int? IsOnboarding { get; set; }

        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public float? Latitude { get; set; }

        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public float? Longitude { get; set; }
    }

    public class UpdateBranchRequest
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        [JsonProperty("region_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? RegionId { get; set; }

        [JsonProperty("is_onboarding", NullValueHandling = NullValueHandling.Ignore)]
        public int? IsOnboarding { get; set; }

        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public float? Latitude { get; set; }

        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public float? Longitude { get; set; }
    }

    public class ListBranchesByCompanyRequest
    {
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int? Page { get; set; }

    }

    public class ListBranchesReponse
    {
        [JsonProperty("meta", NullValueHandling = NullValueHandling.Ignore)]
        public BranchesMetaReponse Meta { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<CreateBranchesResponse> Items { get; set; }
    }

    public class BranchesMetaReponse
    {
        [JsonProperty("total", NullValueHandling = NullValueHandling.Ignore)]
        public int Total { get; set; }

        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int Count { get; set; }

        [JsonProperty("per_page", NullValueHandling = NullValueHandling.Ignore)]
        public int PerPage { get; set; }

        [JsonProperty("current_page", NullValueHandling = NullValueHandling.Ignore)]
        public int CurrentPage { get; set; }

        [JsonProperty("total_pages", NullValueHandling = NullValueHandling.Ignore)]
        public string TotalPages { get; set; }
    }

    public class CreateBranchesResponse
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("region", NullValueHandling = NullValueHandling.Ignore)]
        public RegionInfo Region { get; set; }

        [JsonProperty("address_lat", NullValueHandling = NullValueHandling.Ignore)]
        public double AddressLat { get; set; }

        [JsonProperty("address_lng", NullValueHandling = NullValueHandling.Ignore)]
        public double AddressLng { get; set; }

        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        [JsonProperty("province", NullValueHandling = NullValueHandling.Ignore)]
        public string Province { get; set; }

        [JsonProperty("district", NullValueHandling = NullValueHandling.Ignore)]
        public string District { get; set; }

        [JsonProperty("tel", NullValueHandling = NullValueHandling.Ignore)]
        public string Tel { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("is_headquarter", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsHeadquarter { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }

        [JsonProperty("alias", NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty("sort_index", NullValueHandling = NullValueHandling.Ignore)]
        public int SortIndex { get; set; }

        [JsonProperty("phone_code", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneCode { get; set; }
    }

    public class RegionInfo
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty("sort_index", NullValueHandling = NullValueHandling.Ignore)]
        public int SortIndex { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }

        [JsonProperty("alias", NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }
    }

    public class BranchResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("region")]
        public RegionInfo Region { get; set; }
    }

    public class BranchListRequest
    {
        [JsonProperty("region_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? RegionId { get; set; }

        [JsonProperty("is_all", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsAll { get; set; }

        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int? Page { get; set; }
    }

    public class ListDepartmentsByBranchRequest
    {
        [JsonProperty("branch_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> BranchIds { get; set; }

        [JsonProperty("is_all", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsAll { get; set; }
    }

    public class ListPositionByBranchRequest
    {
        [JsonProperty("branch_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> BranchIds { get; set; }

        [JsonProperty("department_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> DepartmentIds { get; set; }

        [JsonProperty("is_all", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsAll { get; set; }

        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int? Page { get; set; }
    }

    public class RegionListRequest
    {
        [JsonProperty("is_all", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsAll { get; set; }

        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int? Page { get; set; }
    }

    public class DeleteRegionRequest
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
    }

    public class DeleteBranchRequest
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
    }

    public class BranchDetailResponse
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("region", NullValueHandling = NullValueHandling.Ignore)]
        public RegionInfo Region { get; set; }

        [JsonProperty("address_lat", NullValueHandling = NullValueHandling.Ignore)]
        public double? AddressLat { get; set; }

        [JsonProperty("address_lng", NullValueHandling = NullValueHandling.Ignore)]
        public double? AddressLng { get; set; }

        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        [JsonProperty("province", NullValueHandling = NullValueHandling.Ignore)]
        public string Province { get; set; }

        [JsonProperty("district", NullValueHandling = NullValueHandling.Ignore)]
        public string District { get; set; }

        [JsonProperty("tel", NullValueHandling = NullValueHandling.Ignore)]
        public string Tel { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("is_headquarter", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsHeadquarter { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }

        [JsonProperty("alias", NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty("sort_index", NullValueHandling = NullValueHandling.Ignore)]
        public int SortIndex { get; set; }

        [JsonProperty("phone_code", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneCode { get; set; }
    }

    public class BranchCountryInfo
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("iso", NullValueHandling = NullValueHandling.Ignore)]
        public string Iso { get; set; }

        [JsonProperty("show_province", NullValueHandling = NullValueHandling.Ignore)]
        public int ShowProvince { get; set; }

        [JsonProperty("show_district", NullValueHandling = NullValueHandling.Ignore)]
        public int ShowDistrict { get; set; }
    }

    public class BranchListDetailResponse
    {
        [JsonProperty("meta", NullValueHandling = NullValueHandling.Include)]
        public BranchesMetaReponse Meta { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<BranchDetailResponse> Items { get; set; }
    }

    public class RegionDetailResponse
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string code { get; set; }

        [JsonProperty("sort_index", NullValueHandling = NullValueHandling.Ignore)]
        public int sort_index { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string description { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string created_at { get; set; }

        [JsonProperty("alias", NullValueHandling = NullValueHandling.Ignore)]
        public string alias { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string color { get; set; }
    }

    public class RegionListDetailResponse
    {
        [JsonProperty("meta", NullValueHandling = NullValueHandling.Include)]
        public BranchesMetaReponse Meta { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<RegionDetailResponse> Items { get; set; }
    }
}