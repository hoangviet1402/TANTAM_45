using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Employee;
using BussinessObject.Models.User;
using DataAccess;
using Logger;
using MyUtility.Extensions;
using System;
using System.Linq;
using BussinessObject.Helper;
using MyUtility;
using System.Collections.Generic;
using BussinessObject.Permission;
using EntitiesObject.Entities.TanTamEntities;

namespace BussinessObject.Bo.TanTamBo
{
    public class EmployeeBo : BaseBo<DBNull>
    {
        public EmployeeBo() : base(DaoFactory.Employee)
        {
        }

        /// <summary>
        /// Get employee detail by id
        /// </summary>
        public ApiResult<EmployeeDetailResponse> GetEmployeeDetailAsync(EmployeeDetailRequest request, int employeeId, int userRole)
        {
            var response = new ApiResult<EmployeeDetailResponse>
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Validate input
                if (request.EmployeeId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ID nhân viên hợp lệ.";
                    return response;
                }

                if (request.CompanyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ID công ty hợp lệ.";
                    return response;
                }

                // Call stored procedure to get employee detail
                var employeeFromDb = DaoFactory.Employee.GetEmployeeDetail(request.EmployeeId);

                if (employeeFromDb != null)
                {
                    // Check if employee belongs to the specified company
                    if (employeeFromDb.CompanyId != request.CompanyId)
                    {
                        response.Code = ResponseResultEnum.InvalidInput.Value();
                        response.Message = "Nhân viên không thuộc công ty được chỉ định.";
                        return response;
                    }

                    // Map from DB result to DTO
                    // Get object data using common stored procedure
                    var objectData = DaoFactory.Employee.GetEmployeeObjectData(request.EmployeeId);
                    bool canShowEmailAndPhone = PermissionHelper.HasPermission(employeeId, WebPermissionKeys.EmployeeShowEmailAndPhone, userRole);
                    
                    response.Data = new EmployeeDetailResponse
                    {
                        id = employeeFromDb.Id,
                        companyId = employeeFromDb.CompanyId,
                        employeesInfoId = employeeFromDb.EmployeesInfoId,
                        role = employeeFromDb.Role,
                        roleName = employeeFromDb.Role.GetValueOrDefault(UserRole.Employees.Value()).ToEnum<UserRole>().Text(),
                        employeeMapIsActive = employeeFromDb.EmployeeMapIsActive,
                        isNewUser = employeeFromDb.IsNewUser,
                        needSetPassword = employeeFromDb.NeedSetPassword,
                        employeeMapCreatedAt = employeeFromDb.EmployeeMapCreatedAt,
                        fullName = employeeFromDb.FullName,
                        employeeCode = employeeFromDb.EmployeeCode,
                        birthDate = employeeFromDb.BirthDate,
                        gender = employeeFromDb.Gender,
                        displayOrder = employeeFromDb.DisplayOrder,
                        contactAddress = employeeFromDb.ContactAddress,
                        skype = employeeFromDb.Skype,
                        facebook = employeeFromDb.Facebook,
                        emergencyName = employeeFromDb.EmergencyName,
                        emergencyMobile = employeeFromDb.EmergencyMobile,
                        emergencyLandline = employeeFromDb.EmergencyLandline,
                        emergencyRelation = employeeFromDb.EmergencyRelation,
                        emergencyAddress = employeeFromDb.EmergencyAddress,
                        country = employeeFromDb.Country,
                        province = employeeFromDb.Province,
                        district = employeeFromDb.District,
                        ward = employeeFromDb.Ward,
                        permanentAddress = employeeFromDb.PermanentAddress,
                        hometown = employeeFromDb.Hometown,
                        currentAddress = employeeFromDb.CurrentAddress,
                        identityCard = employeeFromDb.IdentityCard,
                        identityCardCreateDate = employeeFromDb.IdentityCardCreateDate,
                        identityCardPlace = employeeFromDb.IdentityCardPlace,
                        passportId = employeeFromDb.PassportID,
                        passportCreateDate = employeeFromDb.PassporCreateDate,
                        passportExp = employeeFromDb.PassporExp,
                        passportPlace = employeeFromDb.PassporPlace,
                        bankHolder = employeeFromDb.BankHolder,
                        bankAccount = employeeFromDb.BankAccount,
                        bankName = employeeFromDb.BankName,
                        bankBranch = employeeFromDb.BankBranch,
                        taxIdentification = employeeFromDb.TaxIdentification,
                        employeesInfoCreatedAt = employeeFromDb.EmployeesInfoCreatedAt.GetValueOrDefault(),
                        email = employeeFromDb.Email,
                        phone = employeeFromDb.Phone,
                        phoneCode = employeeFromDb.PhoneCode,
                        accountIsActive = employeeFromDb.AccountIsActive,
                        deviceId = employeeFromDb.DeviceId,

                        // Map nested objects using common stored procedure
                        company_obj = objectData?.CompanyObjId.HasValue == true ? new { id = objectData.CompanyObjId.Value, name = objectData.CompanyObjName } : null,
                        branch_obj = objectData?.BranchObjId.HasValue == true ? new { id = objectData.BranchObjId.Value, name = objectData.BranchObjName } : null,
                        department_obj = objectData?.DepartmentObjId.HasValue == true ? new { id = objectData.DepartmentObjId.Value, name = objectData.DepartmentObjName } : null,
                        position_obj = objectData?.PositionObjId.HasValue == true ? new { id = objectData.PositionObjId.Value, name = objectData.PositionObjName } : null,
                        region_obj = objectData?.RegionObjId.HasValue == true ? new { id = objectData.RegionObjId.Value, name = objectData.RegionObjName } : null,

                        is_root = employeeFromDb.Role == UserRole.SystemAdmin.Value() ? 1 : 0,
                        is_hide_email_and_phone = !canShowEmailAndPhone,
                    };

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Lấy thông tin nhân viên thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không tìm thấy thông tin nhân viên";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("EmployeeBo.GetEmployeeDetailAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get employee list with pagination and filtering
        /// </summary>
        public ApiResult<EmployeeListResponse> GetEmployeeListAsync(EmployeeListRequest request, int employeeId, int userRole)
        {
            var response = new ApiResult<EmployeeListResponse>
            {
                Data = new EmployeeListResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Validate input
                if (request.CompanyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ID công ty hợp lệ.";
                    return response;
                }

                int page = request.Page ?? 1;
                int limit = request.Limit ?? 10;

                if (page < 1)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Trang phải lớn hơn hoặc bằng 1.";
                    return response;
                }

                // Sửa logic truyền filter: chỉ truyền true/false nếu filter, còn lại truyền null
                bool? isActive = null;
                bool? isQuit = null;
                bool isAll = false;

                var statusType = request.IsQuit.HasValue ? request.IsQuit.Value.ToEnum<EmployeeStatusEnum>() : EmployeeStatusEnum.All;
                // Nếu request có trường IsActive, có thể bổ sung thêm logic ở đây nếu cần

                switch (statusType)
                {
                    case EmployeeStatusEnum.Active:
                        isActive = true;
                        break;
                    case EmployeeStatusEnum.InActive:
                        isActive = false;
                        break;
                    case EmployeeStatusEnum.IsQuit:
                        isQuit = true;
                        break;
                    case EmployeeStatusEnum.NotWorking:
                        // Hard case: NotWorking không trả về data
                        response.Code = ResponseResultEnum.NoData.Value();
                        response.Message = "Không có dữ liệu cho trạng thái không làm việc";
                        return response;
                    case EmployeeStatusEnum.All:
                        isAll = true;
                        break;
                    default:
                        isActive = null;
                        isQuit = null;
                        isAll = false;
                        break;
                }

                // Validate region_id and branch_id
                // if (request.BranchId.HasValue && !request.RegionId.HasValue)
                // {
                //     response.Code = ResponseResultEnum.InvalidInput.Value();
                //     response.Message = "Khi truyền branch_id thì phải truyền region_id.";
                //     return response;
                // }

                // Gọi DAO với nullable
                var employeesFromDb = DaoFactory.Employee.GetEmployeeList(
                    request.CompanyId, page, limit, request.FullName, isQuit, isActive, isAll,
                    request.RegionId, request.BranchId);

                if (employeesFromDb != null && employeesFromDb.Any())
                {
                    var currentUserObjectData = new Ins_Employee_GetObjectData_Result();
                    
                    if (!request.RegionId.HasValue && !request.BranchId.HasValue)
                    {
                        currentUserObjectData = DaoFactory.Employee.GetEmployeeObjectData(employeeId);

                        if (userRole == UserRole.RegionalManager.Value() && currentUserObjectData.RegionObjId == null)
                        {
                            response.Code = ResponseResultEnum.NoData.Value();
                            response.Message = "Không có quyền xem danh sách nhân viên";
                            return response;
                        }

                        if (userRole == UserRole.BranchManager.Value() && currentUserObjectData.BranchObjId == null)
                        {
                            response.Code = ResponseResultEnum.NoData.Value();
                            response.Message = "Không có quyền xem danh sách nhân viên";
                            return response;
                        }
                    }

                    bool canShowEmailAndPhone = PermissionHelper.HasPermission(employeeId, WebPermissionKeys.EmployeeShowEmailAndPhone, userRole);

                    // Map from DB result to DTO
                    response.Data.items = employeesFromDb.Select(emp => 
                    {
                        // Get object data for each employee using common stored procedure
                        var userObjectData = DaoFactory.Employee.GetEmployeeObjectData(emp.EmployeeId);

                        if (currentUserObjectData.RegionObjId.HasValue && currentUserObjectData.RegionObjId.Value > 0)
                        {
                            
                            if (userRole == UserRole.RegionalManager.Value() && userObjectData.RegionObjId != currentUserObjectData.RegionObjId)
                            {
                                return null;
                            }
                        }

                        if (currentUserObjectData.BranchObjId.HasValue && currentUserObjectData.BranchObjId.Value > 0)
                        {
                            if (userRole == UserRole.BranchManager.Value() && userObjectData.BranchObjId != currentUserObjectData.BranchObjId)
                            {
                                return null;
                            }
                        }

                        return new EmployeeListDto
                        {
                            employeeId = emp.EmployeeId,
                            employeeName = emp.EmployeeName,
                            employeeCode = emp.EmployeeCode,
                            email = canShowEmailAndPhone ? emp.Email : "*********",
                            phone = canShowEmailAndPhone ? emp.Phone : "*********",
                            userRole = emp.UserRole,
                            userRoleName = emp.UserRole.GetValueOrDefault(UserRole.Employees.Value()).ToEnum<UserRole>().Text(),
                            branch = emp.Branch,
                            department = emp.Department,
                            title = emp.Title,
                            employeeAccountMapIsActive = emp.EmployeeAccountMapIsActive,
                            accountIsActive = emp.AccountIsActive,
                            isActive = emp.EmployeeAccountMapIsActive,
                            isQuit = emp.IsQuit,

                            company_obj = userObjectData?.CompanyObjId.HasValue == true ? new { id = userObjectData.CompanyObjId.Value, name = userObjectData.CompanyObjName } : null,
                            branch_obj = userObjectData?.BranchObjId.HasValue == true ? new { id = userObjectData.BranchObjId.Value, name = userObjectData.BranchObjName } : null,
                            department_obj = userObjectData?.DepartmentObjId.HasValue == true ? new { id = userObjectData.DepartmentObjId.Value, name = userObjectData.DepartmentObjName } : null,
                            position_obj = userObjectData?.PositionObjId.HasValue == true ? new { id = userObjectData.PositionObjId.Value, name = userObjectData.PositionObjName } : null,
                            region_obj = userObjectData?.RegionObjId.HasValue == true ? new { id = userObjectData.RegionObjId.Value, name = userObjectData.RegionObjName } : null,

                            is_root = emp.UserRole == UserRole.SystemAdmin.Value() ? 1 : 0,
                        };
                    }).Where(item => item != null).ToList();

                    response.Data.meta = new MetaResponse
                    {
                        total = employeesFromDb.Count, // This should come from DB
                        count = response.Data.items.Count,
                        perPage = limit,
                        currentPage = page,
                        totalPages = (int)Math.Ceiling((double)employeesFromDb.Count / limit),
                    };

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Lấy danh sách nhân viên thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không có dữ liệu nhân viên";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("EmployeeBo.GetEmployeeListAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Create new employee
        /// Note: Không cho phép tạo user có role = 1 (SystemAdmin). 
        /// Nếu role = 1 được gửi lên, sẽ tự động chuyển thành role = 10 (Employees)
        /// </summary>
        public ApiResult<EmployeeCreateResult> CreateEmployeeAsync(CreateEmployeeRequest request, int role)
        {
            var response = new ApiResult<EmployeeCreateResult>
            {
                Data = new EmployeeCreateResult(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Validate input
                if (request.CompanyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ID công ty hợp lệ.";
                    return response;
                }

                // Validate role
                if(request.Role <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp Role hợp lệ.";
                    return response;
                }

                // Validate employee code
                if (!string.IsNullOrEmpty(request.EmployeeCode))
                {
                    var validationResult = EmployeeBoHelper.ValidateEmployeeCode(request.CompanyId, request.EmployeeCode, 0);
                    if (!validationResult.IsValid)
                    {
                        if (validationResult.ShouldAutoGenerate)
                        {
                            // Auto generate next employee code
                            request.EmployeeCode = EmployeeBoHelper.GenerateNextEmployeeCodeForCompany(request.CompanyId);
                        }
                        else
                        {
                            response.Code = ResponseResultEnum.InvalidInput.Value();
                            response.Message = validationResult.ErrorMessage;
                            return response;
                        }
                    }
                }
                else
                {
                    // Auto generate employee code if not provided
                    request.EmployeeCode = EmployeeBoHelper.GenerateNextEmployeeCodeForCompany(request.CompanyId);
                }

                // Generate default values if not provided
                if (string.IsNullOrEmpty(request.Phone))
                {
                    request.Phone = "1117" + AESHelper.GenerateUniqueNumber(14);
                }

                if (string.IsNullOrEmpty(request.PhoneCode))
                {
                    request.PhoneCode = "+84";
                }

                if (string.IsNullOrEmpty(request.Email))
                {
                    request.Email = $"{request.Phone}@mail.com";
                }

                // Kiểm tra trùng lặp email và số điện thoại với nhân viên khác
                if (!string.IsNullOrEmpty(request.Email))
                {
                    var emailCheckResult = DaoFactory.Employee.CheckDuplicateEmail(request.CompanyId, request.Email, 0);
                    if (emailCheckResult != null && emailCheckResult.IsDuplicate.GetValueOrDefault(false))
                    {
                        response.Code = ResponseResultEnum.InvalidInput.Value();
                        response.Message = emailCheckResult.ErrorMessage;
                        return response;
                    }
                }

                if (!string.IsNullOrEmpty(request.Phone))
                {
                    request.Phone = StringCommon.ExtractCoreNumber(request.Phone);
                    var phoneCheckResult = DaoFactory.Employee.CheckDuplicatePhone(request.CompanyId, request.Phone, 0);
                    if (phoneCheckResult != null && phoneCheckResult.IsDuplicate.GetValueOrDefault(false))
                    {
                        response.Code = ResponseResultEnum.InvalidInput.Value();
                        response.Message = phoneCheckResult.ErrorMessage;
                        return response;
                    }
                }

                if (request.Role == 1 && role != UserRole.SystemAdmin.Value())
                {
                    // Nếu role = 1 (SystemAdmin) thì set default thành Employees (role = 10)
                    request.Role = 10;
                }
                else if (request.Role <= 0)
                {
                    request.Role = 10; // Default to Employee role
                }

                // Hash password if provided
                string hashedPassword = string.IsNullOrEmpty(request.Password) ? "" : SecurityCommon.sha256_hash(request.Password);
                
                // Call stored procedure to create employee
                int employeeAccountId, isNewUser, needSetPassword, needSetCompany;
                DaoFactory.Employee.CreateEmployee(
                    request.FullName,
                    request.EmployeeCode,
                    request.Phone,
                    request.PhoneCode,
                    request.Email,
                    hashedPassword,
                    request.CompanyId,
                    request.BranchId,
                    request.DepartmentId,
                    request.PositionId,
                    request.RegionId,
                    request.Role,
                    request.DeviceId,
                    out employeeAccountId,
                    out isNewUser,
                    out needSetPassword,
                    out needSetCompany
                );

                response.Data.employeeAccountId = employeeAccountId;
                response.Data.isNewUser = isNewUser;
                response.Data.needSetPassword = needSetPassword;
                response.Data.needSetCompany = needSetCompany;

                // Add default permissions based on role
                if (employeeAccountId > 0 && request.Role != UserRole.SystemAdmin.Value())
                {
                    DaoFactory.Permission.InsertDefaultPermissionsForEmployee(employeeAccountId, request.Role);
                }

                var result = DaoFactory.Company.UpdateCompanyStep(request.CompanyId, SetupStepEnum.ONBOARDING_CREATE_EMPLOYEE.Value());
                if (result > 0)
                {
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Tạo tài khoản thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Không thể cập nhật bước đặt công ty";
                }
                
                switch (employeeAccountId)
                {
                    case 0:
                        response.Code = ResponseResultEnum.Failed.Value();
                        response.Message = "Chưa tạo được tài khoản do hệ thống bận vui lòng thử lại sau.";
                        break;
                    default:
                        response.Code = ResponseResultEnum.Success.Value();
                        response.Message = $"Tạo tài khoản thành công";
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("EmployeeBo.CreateEmployeeAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Có lỗi khi tạo tài khoản";
            }

            return response;
        }

        /// <summary>
        /// Delete employee - simplified version (validation moved to stored procedure)
        /// </summary>
        public ApiResult<bool> DeleteEmployeeAsync(int employeeId, int companyId, int myEmployeeId)
        {
            var response = new ApiResult<bool>
            {
                Data = false,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Only basic input validation
                if (employeeId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ID nhân viên hợp lệ.";
                    return response;
                }

                if (myEmployeeId == employeeId)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Bạn không thể xóa chính mình.";
                    return response;
                }

                if (PermissionHelper.IsSystemAdmin(employeeId))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Bạn không thể xóa admin.";
                    return response;
                }

                // Call stored procedure - all business validation is done there
                var result = DaoFactory.Employee.DeleteEmployee(employeeId);

                response.Data = true;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Xóa nhân viên thành công";
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("EmployeeBo.DeleteEmployeeAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Delete multiple employees - simplified version (validation moved to stored procedure)
        /// </summary>
        public ApiResult<bool> DeleteMultiEmployeeAsync(DeleteMultiEmployeeRequest request, int myEmployeeId)
        {
            var response = new ApiResult<bool>
            {
                Data = false,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Only basic input validation
                if (request.EmployeeIds == null || !request.EmployeeIds.Any())
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp danh sách ID nhân viên.";
                    return response;
                }

                if (request.EmployeeIds.Contains(myEmployeeId))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Bạn không thể xóa chính mình.";
                    return response;
                }

                // Track results
                int successCount = 0;
                int failedCount = 0;
                var errorMessages = new List<string>();

                // Loop through each employee and delete individually
                // Stored procedure handles all validation
                foreach (var employeeId in request.EmployeeIds)
                {
                    if (PermissionHelper.IsSystemAdmin(employeeId))
                    {
                        failedCount++;
                        errorMessages.Add($"ID {employeeId}: Không thể xóa admin.");
                        continue;
                    }

                    // Call stored procedure - all validation is done there
                    var deleteResult = DaoFactory.Employee.DeleteEmployee(employeeId);
                    if (deleteResult > 0)
                    {
                        successCount++;
                    }
                    else
                    {
                        failedCount++;
                        errorMessages.Add($"ID {employeeId}: Không thể xóa");
                    }
                }

                // Prepare response based on results
                if (successCount > 0 && failedCount == 0)
                {
                    // All successful
                    response.Data = true;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = $"Xóa thành công {successCount} nhân viên";
                }
                else if (successCount > 0 && failedCount > 0)
                {
                    // Partial success
                    response.Data = true;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = $"Xóa thành công {successCount} nhân viên. Thất bại {failedCount} nhân viên: {string.Join("; ", errorMessages.Take(2))}";
                }
                else
                {
                    // All failed
                    response.Data = false;
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = $"Không thể xóa nhân viên. Lỗi: {string.Join("; ", errorMessages.Take(2))}";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("EmployeeBo.DeleteMultiEmployeeAsync - Unexpected error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Reset employee password
        /// </summary>
        public ApiResult<bool> ResetEmployeePasswordAsync(int employeeId, int companyId, ResetEmployeePasswordRequest request)
        {
            var response = new ApiResult<bool>
            {
                Data = false,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Validate input
                if (employeeId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ID nhân viên hợp lệ.";
                    return response;
                }

                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ID công ty hợp lệ.";
                    return response;
                }

                // Check if employee exists and belongs to company
                var employee = DaoFactory.Employee.GetEmployeeDetail(employeeId);
                if (employee == null)
                {
                    response.Code = ResponseResultEnum.NotFound.Value();
                    response.Message = "Không tìm thấy nhân viên.";
                    return response;
                }

                if (employee.CompanyId != companyId)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Nhân viên không thuộc công ty được chỉ định.";
                    return response;
                }

                // Hash the new password
                string hashedPassword = SecurityCommon.sha256_hash(request.NewPassword);

                // Reset password
                var result = DaoFactory.Employee.ResetEmployeePassword(employeeId, hashedPassword);
                if (result > 0)
                {
                    response.Data = true;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Đặt lại mật khẩu thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Không thể đặt lại mật khẩu";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("EmployeeBo.ResetEmployeePasswordAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Update employee details
        /// </summary>
        public ApiResult<bool> UpdateEmployeeDetailsAsync(int employeeId, int companyId, UpdateEmployeeDetailsRequest request, int role)
        {
            var response = new ApiResult<bool>
            {
                Data = false,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Validate input
                if (employeeId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ID nhân viên hợp lệ.";
                    return response;
                }

                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ID công ty hợp lệ.";
                    return response;
                }

                // Check if employee exists and belongs to company
                var employee = DaoFactory.Employee.GetEmployeeDetail(employeeId);
                if (employee == null)
                {
                    response.Code = ResponseResultEnum.NotFound.Value();
                    response.Message = "Không tìm thấy nhân viên.";
                    return response;
                }

                if (employee.CompanyId != companyId)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Nhân viên không thuộc công ty được chỉ định.";
                    return response;
                }

                // Validate employee code for update
                if (!string.IsNullOrEmpty(request.EmployeeCode))
                {
                    var validationResult = EmployeeBoHelper.ValidateEmployeeCode(companyId, request.EmployeeCode, employeeId);
                    if (!validationResult.IsValid)
                    {
                        response.Code = ResponseResultEnum.InvalidInput.Value();
                        response.Message = validationResult.ErrorMessage;
                        return response;
                    }
                }

                // Validate role if provided
                if (request.Role.HasValue)
                {
                    if (request.Role.Value == UserRole.SystemAdmin.Value() && role != UserRole.SystemAdmin.Value())
                    {
                        // Nếu role = SystemAdmin thì set default thành Employees
                        request.Role = UserRole.Employees.Value();
                    }
                    else if (request.Role.Value <= 0)
                    {
                        request.Role = UserRole.Employees.Value();
                    }
                }

                request.Phone = StringCommon.ExtractCoreNumber(request.Phone);

                // Kiểm tra trùng lặp email và số điện thoại với nhân viên khác
                if (!string.IsNullOrEmpty(request.Email))
                {
                    var emailCheckResult = DaoFactory.Employee.CheckDuplicateEmail(employee.CompanyId, request.Email, employeeId);
                    if (emailCheckResult != null && emailCheckResult.IsDuplicate.GetValueOrDefault(false))
                    {
                        response.Code = ResponseResultEnum.InvalidInput.Value();
                        response.Message = emailCheckResult.ErrorMessage;
                        return response;
                    }
                }

                if (!string.IsNullOrEmpty(request.Phone))
                {
                    var phoneCheckResult = DaoFactory.Employee.CheckDuplicatePhone(employee.CompanyId, request.Phone, employeeId);
                    if (phoneCheckResult != null && phoneCheckResult.IsDuplicate.GetValueOrDefault(false))
                    {
                        response.Code = ResponseResultEnum.InvalidInput.Value();
                        response.Message = phoneCheckResult.ErrorMessage;
                        return response;
                    }
                }

                if (request.BirthDate.HasValue && request.BirthDate.Value > DateTime.Now)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Ngày sinh không hợp lệ.";
                    return response;
                }

                // Update employee details
                var result = DaoFactory.Employee.UpdateEmployeeDetails_v2(
                    employeeId,
                    request.FullName,
                    request.BirthDate,
                    request.Gender,
                    request.EmployeeCode,
                    request.DisplayOrder,
                    request.Email,
                    request.Phone,
                    request.PhoneCode,
                    request.DepartmentId,
                    request.RegionId,
                    request.BranchId,
                    request.PositionId,
                    request.IsQuit,
                    request.IsActive,
                    request.Role
                );

                // Update permissions if role changed
                if (request.Role.HasValue && request.Role.Value != employee.Role)
                {
                    if (request.Role.Value == UserRole.Employees.Value()){
                        DaoFactory.Permission.DeleteEmployeePermissionsByType(employeeId, PermissionTypeEnum.Web.Value());
                        DaoFactory.Permission.DeleteEmployeePermissionsByType(employeeId, PermissionTypeEnum.Mobile.Value());
                    }

                    DaoFactory.Permission.InsertDefaultPermissionsForEmployee(employeeId, request.Role.Value);

                } 

                response.Data = true;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Cập nhật thông tin nhân viên thành công";
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("EmployeeBo.UpdateEmployeeDetailsAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get employee filter list
        /// </summary>
        public ApiResult<EmployeeFilterListResponse> GetEmployeeFilterListAsync(EmployeeFilterListRequest request, int employeeId, int userRole)
        {
            var response = new ApiResult<EmployeeFilterListResponse>
            {
                Data = new EmployeeFilterListResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Validate input
                if (request.CompanyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ID công ty hợp lệ.";
                    return response;
                }

                // Parse dates
                DateTime startDate = DateTime.Now.AddDays(-30);
                DateTime endDate = DateTime.Now;

                if (!string.IsNullOrEmpty(request.StartDate))
                {
                    DateTime.TryParse(request.StartDate, out startDate);
                }

                if (!string.IsNullOrEmpty(request.EndDate))
                {
                    DateTime.TryParse(request.EndDate, out endDate);
                }

                // Call stored procedure
                var employeesFromDb = DaoFactory.Employee.GetEmployeeFilterList(
                    request.CompanyId,
                    request.Page,
                    request.Limit,
                    startDate,
                    endDate,
                    request.IsNoNeedTimekeeping == 1,
                    0 // total records - will be set by stored procedure
                );

                if (employeesFromDb != null && employeesFromDb.Any())
                {
                    var myEmployeeData = DaoFactory.Employee.GetEmployeeObjectData(employeeId);

                    employeesFromDb = employeesFromDb.Where(x => {
                        // Nếu là Regional Manager (Quản lý vùng)
                        if (userRole == (int)UserRole.RegionalManager && myEmployeeData.RegionObjId.HasValue && myEmployeeData.RegionObjId.Value > 0)
                        {
                            return x.RegionId.ToString() == myEmployeeData.RegionObjId.Value.ToString(); // Chỉ thấy nhân viên cùng vùng
                        }

                        // Nếu là Branch Manager (Quản lý chi nhánh)  
                        if (userRole == (int)UserRole.BranchManager && myEmployeeData.BranchObjId.HasValue && myEmployeeData.BranchObjId.Value > 0)
                        {
                            return x.BranchId.ToString() == myEmployeeData.BranchObjId.Value.ToString(); // Chỉ thấy nhân viên cùng chi nhánh
                        }
                        
                        return true; // Các role khác thấy tất cả
                    }).ToList();

                    response.Data.items = employeesFromDb.Select(emp => new EmployeeFilterListDto
                    {
                        name = emp.Name,
                        userId = emp.UserId,
                        employeeId = emp.EmployeeId,
                        username = emp.Username,
                        regionId = emp.RegionId,
                        branchId = emp.BranchId,
                        departmentId = emp.DepartmentId,
                        positionId = emp.PositionId,
                        identification = emp.Identification,
                        isNoNeedTimekeeping = emp.IsNoNeedTimekeeping
                    }).ToList();

                    response.Data.total = employeesFromDb.Count;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Lấy danh sách nhân viên thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không có dữ liệu nhân viên";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("EmployeeBo.GetEmployeeFilterListAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get next employee code
        /// </summary>
        public ApiResult<NextEmployeeCodeDto> GetNextEmployeeCodeAsync(NextEmployeeCodeRequest request)
        {
            var response = new ApiResult<NextEmployeeCodeDto>
            {
                Data = new NextEmployeeCodeDto(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Validate input
                if (request.CompanyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ID công ty hợp lệ.";
                    return response;
                }

                response.Data.nextCode = EmployeeBoHelper.GenerateNextEmployeeCodeForCompany(request.CompanyId);

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Lấy mã nhân viên tiếp theo thành công";
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("EmployeeBo.GetNextEmployeeCodeAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }
    }
}