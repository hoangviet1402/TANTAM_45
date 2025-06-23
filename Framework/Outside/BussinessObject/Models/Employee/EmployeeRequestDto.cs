using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.User;
using MyUtility.Extensions;

namespace BussinessObject.Models.Employee
{
    /// <summary>
    /// Employee detail request DTO
    /// </summary>
    public class EmployeeDetailRequest
    {
        [Required]
        public int EmployeeId { get; set; }
        
        [Required]
        public int CompanyId { get; set; }
    }

    /// <summary>
    /// Employee list request DTO
    /// </summary>
    public class EmployeeListRequest
    {
        [Required]
        public int CompanyId { get; set; }
        
        public int? Page { get; set; } = 1;
        
        public int? Limit { get; set; } = 10;
        
        public string FullName { get; set; }
        
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// Create employee request DTO
    /// </summary>
    public class CreateEmployeeRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập mã nhân viên.")]
        public string EmployeeCode { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn công ty.")]
        public int CompanyId { get; set; }
        
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhoneCode { get; set; } = "+84";
        public string Password { get; set; }
        public int Role { get; set; } = UserRole.Employees.Value();
        public string DeviceId { get; set; }
    }

    /// <summary>
    /// Update employee details request DTO
    /// </summary>
    public class UpdateEmployeeDetailsRequest
    {
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public string EmployeeCode { get; set; }
        public int? DisplayOrder { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhoneCode { get; set; }
    }

    /// <summary>
    /// Delete multiple employees request DTO
    /// </summary>
    public class DeleteMultiEmployeeRequest
    {
        [Required]
        public int CompanyId { get; set; }
        
        [Required]
        public int[] EmployeeIds { get; set; }
    }

    /// <summary>
    /// Reset employee password request DTO
    /// </summary>
    public class ResetEmployeePasswordRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string NewPassword { get; set; }
    }

    /// <summary>
    /// Employee filter list request DTO
    /// </summary>
    public class EmployeeFilterListRequest
    {
        [Required]
        public int CompanyId { get; set; }
        
        public int Page { get; set; } = 1;
        
        public string StartDate { get; set; }
        
        public string EndDate { get; set; }
        
        public int IsNoNeedTimekeeping { get; set; } = 0;
        
        public int TaskStatusFilter { get; set; } = 0;
        
        public int IsInactive { get; set; } = 0;
        
        public int Limit { get; set; } = 10;
        
        public int TransType { get; set; } = 0;
    }

    /// <summary>
    /// Next employee code request DTO
    /// </summary>
    public class NextEmployeeCodeRequest
    {
        [Required]
        public int CompanyId { get; set; }
    }

    /// <summary>
    /// Employee list response DTO
    /// </summary>
    public class EmployeeListResponse
    {
        public List<EmployeeListDto> Items { get; set; } = new List<EmployeeListDto>();
        public MetaResponse Meta { get; set; }
    }

    /// <summary>
    /// Employee detail response DTO
    /// </summary>
    public class EmployeeDetailResponse : EmployeeDetailDto
    {
    }

    /// <summary>
    /// Employee filter list response DTO
    /// </summary>
    public class EmployeeFilterListResponse
    {
        public List<EmployeeFilterListDto> Items { get; set; } = new List<EmployeeFilterListDto>();
        public int Total { get; set; }
    }
} 