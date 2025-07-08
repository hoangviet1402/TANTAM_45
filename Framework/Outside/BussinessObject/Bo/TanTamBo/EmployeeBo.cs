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
        public ApiResult<EmployeeDetailResponse> GetEmployeeDetailAsync(EmployeeDetailRequest request)
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
                    response.Data = new EmployeeDetailResponse
                    {
                        id = employeeFromDb.Id,
                        companyId = employeeFromDb.CompanyId,
                        employeesInfoId = employeeFromDb.EmployeesInfoId,
                        role = employeeFromDb.Role,
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
                        employeesInfoCreatedAt = employeeFromDb.EmployeesInfoCreatedAt,
                        email = employeeFromDb.Email,
                        phone = employeeFromDb.Phone,
                        phoneCode = employeeFromDb.PhoneCode,
                        accountIsActive = employeeFromDb.AccountIsActive,
                        deviceId = employeeFromDb.DeviceId
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
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("EmployeeBo.GetEmployeeDetailAsync - Entity Exception", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi hệ thống.";
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
        public ApiResult<EmployeeListResponse> GetEmployeeListAsync(EmployeeListRequest request)
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

                // Call stored procedure to get employee list
                var employeesFromDb = DaoFactory.Employee.GetEmployeeList(
                    request.CompanyId, page, limit, request.FullName, request.IsActive);

                if (employeesFromDb != null && employeesFromDb.Any())
                {
                    // Map from DB result to DTO
                    response.Data.items = employeesFromDb.Select(emp => new EmployeeListDto
                    {
                        employeeId = emp.EmployeeId,
                        employeeName = emp.EmployeeName,
                        employeeCode = emp.EmployeeCode,
                        phone = emp.Phone,
                        userRole = emp.UserRole,
                        branch = emp.Branch,
                        department = emp.Department,
                        title = emp.Title,
                        employeeAccountMapIsActive = emp.EmployeeAccountMapIsActive,
                        accountIsActive = emp.AccountIsActive
                    }).ToList();

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
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("EmployeeBo.GetEmployeeListAsync - Entity Exception", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi hệ thống.";
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
        /// </summary>
        public ApiResult<EmployeeCreateResult> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            var response = new ApiResult<EmployeeCreateResult>
            {
                Data = new EmployeeCreateResult(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
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

                if (request.Role <= 0)
                {
                    request.Role = 3; // Default to Employee role
                }

                // Handle BranchId: if not provided, get the first branch of the company
                if (request.BranchId <= 0)
                {
                    try
                    {
                        int total = 0;
                        var branches = DaoFactory.Branches.GetAllBranchs(request.CompanyId, out total);
                        
                        if (branches != null && branches.Any())
                        {
                            request.BranchId = branches.First().BranchId;
                        }
                        else
                        {
                            response.Code = ResponseResultEnum.InvalidInput.Value();
                            response.Message = "Công ty chưa có chi nhánh nào. Vui lòng tạo chi nhánh trước khi thêm nhân viên.";
                            return response;
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonLogger.DefaultLogger.Error("EmployeeBo.CreateEmployeeAsync - Error getting branches", ex);
                        response.Code = ResponseResultEnum.InvalidInput.Value();
                        response.Message = "Vui lòng cung cấp BranchId hợp lệ.";
                        return response;
                    }
                }

                // Hash password if provided
                string hashedPassword = string.IsNullOrEmpty(request.Password) ? "" : AESHelper.HashPassword(request.Password);

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
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("EmployeeBo.CreateEmployeeAsync - Entity Exception", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi hệ thống.";
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
        public ApiResult<bool> DeleteEmployeeAsync(int employeeId, int companyId)
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

                // Call stored procedure - all business validation is done there
                var result = DaoFactory.Employee.DeleteEmployee(employeeId);

                response.Data = true;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Xóa nhân viên thành công";
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = sqlEx.Message; // SQL error message from stored procedure
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("EmployeeBo.DeleteEmployeeAsync - Entity Exception", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi hệ thống.";
                }
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
        public ApiResult<bool> DeleteMultiEmployeeAsync(DeleteMultiEmployeeRequest request)
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

                // Track results
                int successCount = 0;
                int failedCount = 0;
                var errorMessages = new List<string>();

                // Loop through each employee and delete individually
                // Stored procedure handles all validation
                foreach (var employeeId in request.EmployeeIds)
                {
                    try
                    {
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
                    catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
                    {
                        if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                        {
                            failedCount++;
                            errorMessages.Add($"ID {employeeId}: {sqlEx.Message}");
                        }
                        else
                        {
                            failedCount++;
                            errorMessages.Add($"ID {employeeId}: Lỗi hệ thống");
                            CommonLogger.DefaultLogger.Error($"EmployeeBo.DeleteMultiEmployeeAsync - Entity Exception for ID {employeeId}", entityEx);
                        }
                    }
                    catch (Exception ex)
                    {
                        failedCount++;
                        errorMessages.Add($"ID {employeeId}: {ex.Message}");
                        CommonLogger.DefaultLogger.Error($"EmployeeBo.DeleteMultiEmployeeAsync - Error for ID {employeeId}", ex);
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
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("EmployeeBo.ResetEmployeePasswordAsync - Entity Exception", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi hệ thống.";
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
        public ApiResult<bool> UpdateEmployeeDetailsAsync(int employeeId, int companyId, UpdateEmployeeDetailsRequest request)
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

                var all_employee_account_map = GetEmployeeAccountMapForValidation(employee.CompanyId);

                request.Phone = StringCommon.ExtractCoreNumber(request.Phone);

                // Kiểm tra trùng lặp email và số điện thoại với nhân viên khác
                string errorMessage;
                if (CheckDuplicateContactInfo(all_employee_account_map, request.Email, request.Phone, employeeId, out errorMessage))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = errorMessage;
                    return response;
                }

                // Kiểm tra trùng lặp employee code
                if (CheckDuplicateEmployeeCode(all_employee_account_map, request.EmployeeCode, employeeId, out errorMessage))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = errorMessage;
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
                    request.PhoneCode
                );

                response.Data = true;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Cập nhật thông tin nhân viên thành công";
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("EmployeeBo.UpdateEmployeeDetailsAsync - Entity Exception", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi hệ thống.";
                }
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
        public ApiResult<EmployeeFilterListResponse> GetEmployeeFilterListAsync(EmployeeFilterListRequest request)
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
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("EmployeeBo.GetEmployeeFilterListAsync - Entity Exception", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi hệ thống.";
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

                // Get all employee codes from database
                var allEmployeeCodes = DaoFactory.Employee.GetAllEmployeeCodes(request.CompanyId);
                
                // Find the highest/last employee code using C# logic
                var lastCode = FindHighestEmployeeCode(allEmployeeCodes);
                
                if (!string.IsNullOrEmpty(lastCode))
                {
                    response.Data.nextCode = GenerateNextEmployeeCode(lastCode);
                }
                else
                {
                    response.Data.nextCode = "EMP001";
                }

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Lấy mã nhân viên tiếp theo thành công";
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("EmployeeBo.GetNextEmployeeCodeAsync - Entity Exception", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi hệ thống.";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("EmployeeBo.GetNextEmployeeCodeAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Find the highest employee code from the list using complex logic
        /// </summary>
        private string FindHighestEmployeeCode(List<string> employeeCodes)
        {
            if (employeeCodes == null || !employeeCodes.Any())
            {
                return null;
            }

            // Filter out empty codes
            var validCodes = employeeCodes.Where(code => !string.IsNullOrWhiteSpace(code)).ToList();
            if (!validCodes.Any())
            {
                return null;
            }

            // Sort using custom logic that handles different formats
            var sortedCodes = validCodes
                .Select(code => new
                {
                    Code = code,
                    // Extract numeric part for proper sorting
                    NumericPart = ExtractNumericPart(code),
                    CodeLength = code.Length
                })
                .OrderByDescending(item => item.NumericPart)    // First sort by numeric value
                .ThenByDescending(item => item.CodeLength)      // Then by code length
                .ThenByDescending(item => item.Code)            // Finally alphabetically
                .Select(item => item.Code)
                .ToList();

            return sortedCodes.FirstOrDefault();
        }

        /// <summary>
        /// Extract numeric part from employee code for sorting
        /// </summary>
        private long ExtractNumericPart(string employeeCode)
        {
            try
            {
                if (string.IsNullOrEmpty(employeeCode))
                {
                    return 0;
                }

                // Check if code contains any numbers
                if (!System.Text.RegularExpressions.Regex.IsMatch(employeeCode, @"\d"))
                {
                    return 0;
                }

                // If entire code is numeric
                if (long.TryParse(employeeCode, out long fullNumeric))
                {
                    return fullNumeric;
                }

                // Extract the last continuous numeric part
                var matches = System.Text.RegularExpressions.Regex.Matches(employeeCode, @"\d+");
                if (matches.Count > 0)
                {
                    // Get the last numeric match
                    var lastMatch = matches[matches.Count - 1];
                    if (long.TryParse(lastMatch.Value, out long numericPart))
                    {
                        return numericPart;
                    }
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Generate next employee code based on last code
        /// </summary>
        private string GenerateNextEmployeeCode(string lastCode)
        {
            try
            {
                if (string.IsNullOrEmpty(lastCode))
                {
                    return "EMP001";
                }

                // Extract all numeric parts
                var matches = System.Text.RegularExpressions.Regex.Matches(lastCode, @"\d+");
                if (matches.Count > 0)
                {
                    // Get the last numeric match
                    var lastMatch = matches[matches.Count - 1];
                    var numberPart = lastMatch.Value;
                    var startIndex = lastMatch.Index;
                    
                    if (int.TryParse(numberPart, out int number))
                    {
                        number++;
                        
                        // Preserve the original format with same number of digits
                        var newNumberPart = number.ToString().PadLeft(numberPart.Length, '0');
                        
                        // Replace the numeric part in the original string
                        var prefix = lastCode.Substring(0, startIndex);
                        var suffix = lastCode.Substring(startIndex + numberPart.Length);
                        
                        return $"{prefix}{newNumberPart}{suffix}";
                    }
                }

                // If no numeric part found, append 001
                return $"{lastCode}001";
            }
            catch
            {
                return "EMP001";
            }
        }

        /// <summary>
        /// Kiểm tra trùng lặp email và số điện thoại (kiểu cũ, dùng out string)
        /// </summary>
        private bool CheckDuplicateContactInfo(
            List<EmployeeAccountMapValidationDto> allEmployees,
            string email,
            string phone,
            int excludeEmployeeId,
            out string errorMessage)
        {
            errorMessage = string.Empty;
            if (!string.IsNullOrEmpty(email))
            {
                var duplicateEmail = allEmployees
                    .FirstOrDefault(map => map.Email != null
                        && map.Email.ToLower().Trim() == email.ToLower().Trim()
                        && map.EmployeeAccountMapId != excludeEmployeeId);
                if (duplicateEmail != null)
                {
                    errorMessage = $"Email '{email}' đã được sử dụng bởi nhân viên khác.";
                    return true;
                }
            }
            if (!string.IsNullOrEmpty(phone))
            {
                var duplicatePhone = allEmployees
                    .FirstOrDefault(map => map.Phone != null
                        && map.Phone.Trim() == phone.Trim()
                        && map.EmployeeAccountMapId != excludeEmployeeId);
                if (duplicatePhone != null)
                {
                    errorMessage = $"Số điện thoại '{phone}' đã được sử dụng bởi nhân viên khác.";
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Kiểm tra trùng lặp employee code (kiểu cũ, dùng out string)
        /// </summary>
        private bool CheckDuplicateEmployeeCode(
            List<EmployeeAccountMapValidationDto> allEmployees,
            string employeeCode,
            int excludeEmployeeId,
            out string errorMessage)
        {
            errorMessage = string.Empty;
            if (!string.IsNullOrEmpty(employeeCode))
            {
                var duplicateCode = allEmployees
                    .FirstOrDefault(map => map.EmployeeCode != null
                        && map.EmployeeCode.Trim() == employeeCode.Trim()
                        && map.EmployeeAccountMapId != excludeEmployeeId);
                if (duplicateCode != null)
                {
                    errorMessage = $"Mã nhân viên '{employeeCode}' đã được sử dụng bởi nhân viên khác.";
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Lấy dữ liệu employee account map cho validation (không dùng Entity result)
        /// </summary>
        private List<EmployeeAccountMapValidationDto> GetEmployeeAccountMapForValidation(int companyId)
        {
            try
            {
                // Gọi DAO để lấy dữ liệu
                var entityResults = DaoFactory.Employee.GetEmployeeAccountMapByCompanyId(companyId);
                
                if (entityResults == null || !entityResults.Any())
                    return new List<EmployeeAccountMapValidationDto>();

                // Map sang DTO validation
                var validationList = new List<EmployeeAccountMapValidationDto>();
                
                foreach (var entity in entityResults)
                {
                    // Sử dụng reflection để lấy properties an toàn
                    var validationDto = new EmployeeAccountMapValidationDto();
                    
                    // Map EmployeeAccountMapId (Id property)
                    validationDto.EmployeeAccountMapId = entity.Id;
                    
                    // Map Email và Phone từ Account (cần thêm properties vào Entity result)
                    // Tạm thời để null cho đến khi Entity result được update
                    validationDto.Email = entity.Email; // entity.Email
                    validationDto.Phone = entity.Phone; // entity.Phone
                    validationDto.EmployeeCode = entity.AccountId.ToString(); // entity.EmployeeCode
                    
                    validationList.Add(validationDto);
                }
                
                return validationList;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("EmployeeBo.GetEmployeeAccountMapForValidation - Error", ex);
                return new List<EmployeeAccountMapValidationDto>();
            }
        }
    }
} 