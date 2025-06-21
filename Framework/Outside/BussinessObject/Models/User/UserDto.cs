using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BussinessObject.Models.User
{
    public class UserDto
    {
        [JsonProperty("id")]
        public int UserId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("role")]
        public int? Role { get; set; }

        [JsonProperty("shop_id")]
        public int CompanyId { get; set; }

        [JsonProperty("shop_name")]
        public string CompanyFullName { get; set; }

        [JsonProperty("employee_code")]
        public string EmployeeCode { get; set; }

        [JsonProperty("birth_date")]
        public DateTime? BirthDate { get; set; }

        [JsonProperty("gender")]
        public int? Gender { get; set; }
    }

    public class UserListRequest
    {
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int? Page { get; set; }

        [JsonProperty("limit", NullValueHandling = NullValueHandling.Ignore)]
        public int? Limit { get; set; }

        [JsonProperty("search", NullValueHandling = NullValueHandling.Ignore)]
        public string Search { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int? Status { get; set; }

        [JsonProperty("department_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? DepartmentId { get; set; }

        [JsonProperty("role_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? RoleId { get; set; }

        public UserListRequest()
        {
            Page = 1;
            Limit = 10;
            Search = string.Empty;
        }
    }

    public class UserListResponse
    {
        [JsonProperty("meta", NullValueHandling = NullValueHandling.Ignore)]
        public MetaResponse Meta { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<UserDto> Items { get; set; }

        public UserListResponse()
        {
            Meta = new MetaResponse();
            Items = new List<UserDto>();
        }
    }

    public class MetaResponse
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
        public int TotalPages { get; set; }
    }

    public class UserDetailRequest
    {
        [Required]
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [Required]
        [JsonProperty("company_id")]
        public int CompanyId { get; set; }
    }

    public class UserDetailResponse
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("phone_code")]
        public string PhoneCode { get; set; }

        [JsonProperty("phone_full")]
        public string PhoneFull { get; set; }

        [JsonProperty("account_is_active")]
        public bool AccountIsActive { get; set; }

        [JsonProperty("account_created_at")]
        public DateTime AccountCreatedAt { get; set; }

        [JsonProperty("employee_account_map_id")]
        public int EmployeeAccountMapId { get; set; }

        [JsonProperty("shop_id")]
        public int CompanyId { get; set; }

        [JsonProperty("shop_name")]
        public string CompanyFullName { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("is_active")]
        public bool UserIsActive { get; set; }

        [JsonProperty("is_new_user")]
        public bool IsNewUser { get; set; }

        [JsonProperty("need_set_password")]
        public bool NeedSetPassword { get; set; }

        [JsonProperty("user_created_at")]
        public DateTime? UserCreatedAt { get; set; }

        [JsonProperty("role")]
        public int? Role { get; set; }

        [JsonProperty("employee_info_id")]
        public int? EmployeeInfoId { get; set; }

        [JsonProperty("employee_code")]
        public string EmployeeCode { get; set; }

        [JsonProperty("birth_date")]
        public DateTime? BirthDate { get; set; }

        [JsonProperty("gender")]
        public int? Gender { get; set; }

        [JsonProperty("contact_address")]
        public string ContactAddress { get; set; }
    }
} 