using System;
using System.Collections.Generic;

namespace BussinessObject.Models.Employee
{
    /// <summary>
    /// Employee detail response DTO
    /// </summary>
    public class EmployeeDetailDto
    {
        // From EmployeeAccountMap
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int? EmployeesInfoId { get; set; }
        public int? Role { get; set; }
        public bool EmployeeMapIsActive { get; set; }
        public bool IsNewUser { get; set; }
        public bool NeedSetPassword { get; set; }
        public DateTime? EmployeeMapCreatedAt { get; set; }
        public string FullName { get; set; }

        // From EmployeesInfo
        public string EmployeeCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Gender { get; set; }
        public int? DisplayOrder { get; set; }
        public string ContactAddress { get; set; }
        public string Skype { get; set; }
        public string Facebook { get; set; }
        public string EmergencyName { get; set; }
        public string EmergencyMobile { get; set; }
        public string EmergencyLandline { get; set; }
        public string EmergencyRelation { get; set; }
        public string EmergencyAddress { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string PermanentAddress { get; set; }
        public string Hometown { get; set; }
        public string CurrentAddress { get; set; }
        public string IdentityCard { get; set; }
        public DateTime? IdentityCardCreateDate { get; set; }
        public string IdentityCardPlace { get; set; }
        public string PassportID { get; set; }
        public DateTime? PassporCreateDate { get; set; }
        public DateTime? PassporExp { get; set; }
        public string PassporPlace { get; set; }
        public string BankHolder { get; set; }
        public string BankAccount { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string TaxIdentification { get; set; }
        public DateTime EmployeesInfoCreatedAt { get; set; }

        // From Account
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhoneCode { get; set; }
        public bool AccountIsActive { get; set; }
        public string DeviceId { get; set; }
    }

    /// <summary>
    /// Employee list item DTO
    /// </summary>
    public class EmployeeListDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string Phone { get; set; }
        public int? UserRole { get; set; }
        public int? Branch { get; set; }
        public int? Department { get; set; }
        public int? Title { get; set; }
        public bool EmployeeAccountMapIsActive { get; set; }
        public bool AccountIsActive { get; set; }
    }

    /// <summary>
    /// Employee filter list DTO  
    /// </summary>
    public class EmployeeFilterListDto
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string RegionId { get; set; }
        public string BranchId { get; set; }
        public string DepartmentId { get; set; }
        public string PositionId { get; set; }
        public string Identification { get; set; }
        public bool? IsNoNeedTimekeeping { get; set; }
    }

    /// <summary>
    /// Create employee result DTO
    /// </summary>
    public class EmployeeCreateResult
    {
        public int EmployeeAccountId { get; set; }
        public int IsNewUser { get; set; }
        public int NeedSetPassword { get; set; }
        public int NeedSetCompany { get; set; }
    }

    /// <summary>
    /// Next employee code DTO
    /// </summary>
    public class NextEmployeeCodeDto
    {
        public string NextCode { get; set; }
    }
} 