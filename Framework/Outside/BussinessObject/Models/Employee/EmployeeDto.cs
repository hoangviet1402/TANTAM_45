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
        public int id { get; set; }
        public int companyId { get; set; }
        public int? employeesInfoId { get; set; }
        public int? role { get; set; }
        public bool employeeMapIsActive { get; set; }
        public bool isNewUser { get; set; }
        public bool needSetPassword { get; set; }
        public DateTime? employeeMapCreatedAt { get; set; }
        public string fullName { get; set; }

        // From EmployeesInfo
        public string employeeCode { get; set; }
        public DateTime? birthDate { get; set; }
        public int? gender { get; set; }
        public int? displayOrder { get; set; }
        public string contactAddress { get; set; }
        public string skype { get; set; }
        public string facebook { get; set; }
        public string emergencyName { get; set; }
        public string emergencyMobile { get; set; }
        public string emergencyLandline { get; set; }
        public string emergencyRelation { get; set; }
        public string emergencyAddress { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string district { get; set; }
        public string ward { get; set; }
        public string permanentAddress { get; set; }
        public string hometown { get; set; }
        public string currentAddress { get; set; }
        public string identityCard { get; set; }
        public DateTime? identityCardCreateDate { get; set; }
        public string identityCardPlace { get; set; }
        public string passportId { get; set; }
        public DateTime? passportCreateDate { get; set; }
        public DateTime? passportExp { get; set; }
        public string passportPlace { get; set; }
        public string bankHolder { get; set; }
        public string bankAccount { get; set; }
        public string bankName { get; set; }
        public string bankBranch { get; set; }
        public string taxIdentification { get; set; }
        public DateTime employeesInfoCreatedAt { get; set; }

        // From Account
        public string email { get; set; }
        public string phone { get; set; }
        public string phoneCode { get; set; }
        public bool accountIsActive { get; set; }
        public string deviceId { get; set; }
    }

    /// <summary>
    /// Employee list item DTO
    /// </summary>
    public class EmployeeListDto
    {
        public int employeeId { get; set; }
        public string employeeName { get; set; }
        public string employeeCode { get; set; }
        public string phone { get; set; }
        public int? userRole { get; set; }
        public int? branch { get; set; }
        public int? department { get; set; }
        public int? title { get; set; }
        public bool employeeAccountMapIsActive { get; set; }
        public bool accountIsActive { get; set; }
    }

    /// <summary>
    /// Employee filter list DTO  
    /// </summary>
    public class EmployeeFilterListDto
    {
        public string name { get; set; }
        public string userId { get; set; }
        public string employeeId { get; set; }
        public string username { get; set; }
        public string regionId { get; set; }
        public string branchId { get; set; }
        public string departmentId { get; set; }
        public string positionId { get; set; }
        public string identification { get; set; }
        public bool? isNoNeedTimekeeping { get; set; }
    }

    /// <summary>
    /// Create employee result DTO
    /// </summary>
    public class EmployeeCreateResult
    {
        public int employeeAccountId { get; set; }
        public int isNewUser { get; set; }
        public int needSetPassword { get; set; }
        public int needSetCompany { get; set; }
    }

    /// <summary>
    /// Next employee code DTO
    /// </summary>
    public class NextEmployeeCodeDto
    {
        public string nextCode { get; set; }
    }
} 