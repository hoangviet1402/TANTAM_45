using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace BussinessObject.Models.User
{
    public class CreateUserRequest
    {
        [Required]
        [JsonProperty("username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required]
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [Required]
        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("role_id")]
        public int RoleId { get; set; }

        [JsonProperty("department_id")]
        public int? DepartmentId { get; set; }

        [JsonProperty("position_id")]
        public int? PositionId { get; set; }

        [JsonProperty("branch_id")]
        public int? BranchId { get; set; }

        [JsonProperty("company_id")]
        public int CompanyId { get; set; }
    }

    public class UpdateUserRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("is_active")]
        public bool? IsActive { get; set; }

        [JsonProperty("role_id")]
        public int? RoleId { get; set; }

        [JsonProperty("department_id")]
        public int? DepartmentId { get; set; }

        [JsonProperty("position_id")]
        public int? PositionId { get; set; }

        [JsonProperty("branch_id")]
        public int? BranchId { get; set; }
    }

    public class UpdateUserProfileRequest
    {
        [Required]
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("birth_date")]
        public DateTime? BirthDate { get; set; }

        [JsonProperty("gender")]
        public int? Gender { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("skills")]
        public string Skills { get; set; }

        [JsonProperty("join_date")]
        public DateTime? JoinDate { get; set; }
    }

    public class ChangeUserPasswordRequest
    {
        [Required]
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [Required]
        [JsonProperty("old_password")]
        public string OldPassword { get; set; }

        [Required]
        [JsonProperty("new_password")]
        public string NewPassword { get; set; }

        [Required]
        [JsonProperty("confirm_password")]
        public string ConfirmPassword { get; set; }
    }

    public class UserStatusRequest
    {
        [Required]
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [Required]
        [JsonProperty("is_active")]
        public bool IsActive { get; set; }
    }
} 