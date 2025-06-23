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
                        Id = employeeFromDb.Id,
                        CompanyId = employeeFromDb.CompanyId,
                        EmployeesInfoId = employeeFromDb.EmployeesInfoId,
                        Role = employeeFromDb.Role,
                        EmployeeMapIsActive = employeeFromDb.EmployeeMapIsActive,
                        IsNewUser = employeeFromDb.IsNewUser,
                        NeedSetPassword = employeeFromDb.NeedSetPassword,
                        EmployeeMapCreatedAt = employeeFromDb.EmployeeMapCreatedAt,
                        FullName = employeeFromDb.FullName,
                        EmployeeCode = employeeFromDb.EmployeeCode,
                        BirthDate = employeeFromDb.BirthDate,
                        Gender = employeeFromDb.Gender,
                        DisplayOrder = employeeFromDb.DisplayOrder,
                        ContactAddress = employeeFromDb.ContactAddress,
                        Skype = employeeFromDb.Skype,
                        Facebook = employeeFromDb.Facebook,
                        EmergencyName = employeeFromDb.EmergencyName,
                        EmergencyMobile = employeeFromDb.EmergencyMobile,
                        EmergencyLandline = employeeFromDb.EmergencyLandline,
                        EmergencyRelation = employeeFromDb.EmergencyRelation,
                        EmergencyAddress = employeeFromDb.EmergencyAddress,
                        Country = employeeFromDb.Country,
                        Province = employeeFromDb.Province,
                        District = employeeFromDb.District,
                        Ward = employeeFromDb.Ward,
                        PermanentAddress = employeeFromDb.PermanentAddress,
                        Hometown = employeeFromDb.Hometown,
                        CurrentAddress = employeeFromDb.CurrentAddress,
                        IdentityCard = employeeFromDb.IdentityCard,
                        IdentityCardCreateDate = employeeFromDb.IdentityCardCreateDate,
                        IdentityCardPlace = employeeFromDb.IdentityCardPlace,
                        PassportID = employeeFromDb.PassportID,
                        PassporCreateDate = employeeFromDb.PassporCreateDate,
                        PassporExp = employeeFromDb.PassporExp,
                        PassporPlace = employeeFromDb.PassporPlace,
                        BankHolder = employeeFromDb.BankHolder,
                        BankAccount = employeeFromDb.BankAccount,
                        BankName = employeeFromDb.BankName,
                        BankBranch = employeeFromDb.BankBranch,
                        TaxIdentification = employeeFromDb.TaxIdentification,
                        EmployeesInfoCreatedAt = employeeFromDb.EmployeesInfoCreatedAt,
                        Email = employeeFromDb.Email,
                        Phone = employeeFromDb.Phone,
                        PhoneCode = employeeFromDb.PhoneCode,
                        AccountIsActive = employeeFromDb.AccountIsActive,
                        DeviceId = employeeFromDb.DeviceId
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
                    response.Data.Items = employeesFromDb.Select(emp => new EmployeeListDto
                    {
                        EmployeeId = emp.EmployeeId,
                        EmployeeName = emp.EmployeeName,
                        EmployeeCode = emp.EmployeeCode,
                        Phone = emp.Phone,
                        UserRole = emp.UserRole,
                        Branch = emp.Branch,
                        Department = emp.Department,
                        Title = emp.Title,
                        EmployeeAccountMapIsActive = emp.EmployeeAccountMapIsActive,
                        AccountIsActive = emp.AccountIsActive
                    }).ToList();

                    response.Data.Meta = new MetaResponse
                    {
                        Total = employeesFromDb.Count, // This should come from DB
                        Count = response.Data.Items.Count,
                        PerPage = limit,
                        CurrentPage = page,
                        TotalPages = (int)Math.Ceiling((double)employeesFromDb.Count / limit),
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

                response.Data.EmployeeAccountId = employeeAccountId;
                response.Data.IsNewUser = isNewUser;
                response.Data.NeedSetPassword = needSetPassword;
                response.Data.NeedSetCompany = needSetCompany;

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
        /// Delete employee
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

                // Delete employee
                var result = DaoFactory.Employee.DeleteEmployee(employeeId);
                if (result > 0)
                {
                    response.Data = true;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Xóa nhân viên thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Không thể xóa nhân viên";
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
        /// Delete multiple employees
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
                // Validate input
                if (request.CompanyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp ID công ty hợp lệ.";
                    return response;
                }

                if (request.EmployeeIds == null || !request.EmployeeIds.Any())
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp danh sách ID nhân viên.";
                    return response;
                }

                // Convert array to comma-separated string
                string employeeIds = string.Join(",", request.EmployeeIds);

                // Delete multiple employees
                var result = DaoFactory.Employee.DeleteMultiEmployee(employeeIds);
                if (result > 0)
                {
                    response.Data = true;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Xóa nhân viên thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Không thể xóa nhân viên";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("EmployeeBo.DeleteMultiEmployeeAsync - Error occurred", ex);
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
                string hashedPassword = AESHelper.HashPassword(request.NewPassword);

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

                // Update employee details
                var result = DaoFactory.Employee.UpdateEmployeeDetails(
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

                if (result > 0)
                {
                    response.Data = true;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Cập nhật thông tin nhân viên thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Không thể cập nhật thông tin nhân viên";
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
                    response.Data.Items = employeesFromDb.Select(emp => new EmployeeFilterListDto
                    {
                        Name = emp.Name,
                        UserId = emp.UserId,
                        Username = emp.Username,
                        RegionId = emp.RegionId,
                        BranchId = emp.BranchId,
                        DepartmentId = emp.DepartmentId,
                        PositionId = emp.PositionId,
                        Identification = emp.Identification,
                        IsNoNeedTimekeeping = emp.IsNoNeedTimekeeping
                    }).ToList();

                    response.Data.Total = employeesFromDb.Count;
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

                // Get last employee code
                var lastCode = DaoFactory.Employee.GetNextEmployeeCode(request.CompanyId);
                
                if (!string.IsNullOrEmpty(lastCode))
                {
                    response.Data.NextCode = GenerateNextEmployeeCode(lastCode);
                }
                else
                {
                    response.Data.NextCode = "EMP001";
                }

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

                // Extract number from code (e.g., EMP001 -> 001)
                var numberPart = System.Text.RegularExpressions.Regex.Match(lastCode, @"\d+").Value;
                if (int.TryParse(numberPart, out int number))
                {
                    number++;
                    var prefix = lastCode.Substring(0, lastCode.Length - numberPart.Length);
                    return $"{prefix}{number:D3}";
                }

                return "EMP001";
            }
            catch
            {
                return "EMP001";
            }
        }
    }
} 