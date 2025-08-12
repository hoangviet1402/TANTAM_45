using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Employee;
using BussinessObject.Models.Shift;
using BussinessObject.Permission;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Web.Http;
using TanTamApi.JWT.Helper;
using TanTamApi.JWT.Middleware;

namespace TanTamApi.Controllers
{
    [ApiAuthorize]
    [RoutePrefix("api/employee")]
    public class EmployeeController : ApiController
    {
        /// <summary>
        /// Get employee detail by id
        /// </summary>
        [HttpGet]
        [Route("detail")]
        public IHttpActionResult GetEmployeeDetail([FromUri] int employee_id, [FromUri] int company_id)
        {
            var response = new ApiResult<EmployeeDetailResponse>()
            {
                Data = new EmployeeDetailResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Get company and account info from JWT token
                //var tokenCompanyId = JwtHelper.GetCompanyIdFromToken(Request);
                //var accountId = JwtHelper.GetAccountIdFromToken(Request);

                // Validate JWT token info
                //if (tokenCompanyId != company_id)
                //{
                //    response.Code = ResponseResultEnum.InvalidToken.Value();
                //    response.Message = "Thông tin công ty không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}

                var request = new EmployeeDetailRequest
                {
                    EmployeeId = employee_id,
                    CompanyId = company_id
                };

                //if (!ModelState.IsValid)
                //{
                //    response.Code = ResponseResultEnum.InvalidInput.Value();
                //    response.Message = "Thông tin đầu vào không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}
                var employeeId = JwtHelper.GetAccountMapIDFromToken(Request);
                var userRole = JwtHelper.GetRoleFromToken(Request);

                var result = BoFactory.Employee.GetEmployeeDetailAsync(request, employeeId, userRole);

                if (result.Code == ResponseResultEnum.NotFound.Value() || result.Code == ResponseResultEnum.NoData.Value())
                {
                    response.Code = ResponseResultEnum.NotFound.Value();
                    response.Message = "Không tìm thấy nhân viên.";
                    return Content(HttpStatusCode.OK, response);
                }

                if (result.Code == ResponseResultEnum.InvalidInput.Value())
                {
                    return Content(HttpStatusCode.OK, result);
                }

                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("GetEmployeeDetail Exception employee_id {0}, company_id {1}, EX:", employee_id, company_id, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Get employee list with pagination and filtering
        /// </summary>
        [RequiredPermission]
        [HttpGet]
        [Route("get-dynamic-list")]
        public IHttpActionResult GetEmployeeList([FromUri] EmployeeListRequest request)
        {
            var response = new ApiResult<EmployeeListResponse>()
            {
                Data = new EmployeeListResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                if (request == null)
                {
                    request = new EmployeeListRequest();
                }

                // Get company info from JWT token
                //var tokenCompanyId = JwtHelper.GetCompanyIdFromToken(Request);
                //var accountId = JwtHelper.GetAccountIdFromToken(Request);

                // Validate JWT token info
                //if (tokenCompanyId != request.CompanyId)
                //{
                //    response.Code = ResponseResultEnum.InvalidToken.Value();
                //    response.Message = "Thông tin công ty không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}

                //if (!ModelState.IsValid)
                //{
                //    response.Code = ResponseResultEnum.InvalidInput.Value();
                //    response.Message = "Thông tin đầu vào không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}

                var employeeId = JwtHelper.GetAccountMapIDFromToken(Request);
                var userRole = JwtHelper.GetRoleFromToken(Request);

                var result = BoFactory.Employee.GetEmployeeListAsync(request, employeeId, userRole);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("GetEmployeeList Exception company_id {0}, EX:", request?.CompanyId, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Create a new employee
        /// </summary>
        [RequiredPermission]
        [HttpPost]
        [Route("create-employee")]
        public IHttpActionResult CreateEmployee([FromBody] CreateEmployeeRequest request)
        {
            var response = new ApiResult<EmployeeCreateResult>()
            {
                Data = new EmployeeCreateResult(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng nhập đủ thông tin.";
                    return Content(HttpStatusCode.OK, response);
                }

                // Get company info from JWT token
                //var tokenCompanyId = JwtHelper.GetCompanyIdFromToken(Request);
                //var accountId = JwtHelper.GetAccountIdFromToken(Request);
                var role = JwtHelper.GetRoleFromToken(Request);
                // Validate JWT token info
                //if (tokenCompanyId != request.CompanyId)
                //{
                //    response.Code = ResponseResultEnum.InvalidToken.Value();
                //    response.Message = "Thông tin công ty không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}

                // Set additional info
                request.DeviceId = Request.Headers.UserAgent?.ToString() ?? "";

                //if (!ModelState.IsValid)
                //{
                //    response.Code = ResponseResultEnum.InvalidInput.Value();
                //    response.Message = "Thông tin đầu vào không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}

                var result = BoFactory.Employee.CreateEmployeeAsync(request, role);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("CreateEmployee Exception request {0}, EX:", request?.FullName, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Delete an employee
        /// </summary>
        [RequiredPermission]
        [HttpPost]
        [Route("delete-employee")]
        public IHttpActionResult DeleteEmployee([FromUri] int employee_id, [FromUri] int company_id)
        {
            var response = new ApiResult<bool>()
            {
                Data = false,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Get company info from JWT token
                //var tokenCompanyId = JwtHelper.GetCompanyIdFromToken(Request);
                //var accountId = JwtHelper.GetAccountIdFromToken(Request);
                var myEmployeeId = JwtHelper.GetAccountMapIDFromToken(Request);

                // Validate JWT token info
                //if (tokenCompanyId != company_id)
                //{
                //    response.Code = ResponseResultEnum.InvalidToken.Value();
                //    response.Message = "Thông tin công ty không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}

                // Validate input
                if (employee_id <= 0 || company_id <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thông tin nhân viên không hợp lệ.";
                    return Content(HttpStatusCode.OK, response);
                }

                var result = BoFactory.Employee.DeleteEmployeeAsync(employee_id, company_id, myEmployeeId);

                if (result.Code == ResponseResultEnum.NotFound.Value())
                {
                    return Content(HttpStatusCode.OK, result);
                }

                if (result.Code == ResponseResultEnum.InvalidInput.Value())
                {
                    return Content(HttpStatusCode.OK, result);
                }

                if (result.Code == ResponseResultEnum.Failed.Value() || result.Code == ResponseResultEnum.SystemError.Value())
                {
                    return Content(HttpStatusCode.OK, result);
                }

                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("DeleteEmployee Exception employee_id {0}, company_id {1}, EX:", employee_id, company_id, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Delete multiple employees
        /// </summary>
        [RequiredPermission]
        [HttpPost]
        [Route("delete-multi-employee")]
        public IHttpActionResult DeleteMultiEmployee([FromBody] DeleteMultiEmployeeRequest request)
        {
            var response = new ApiResult<bool>()
            {
                Data = false,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng nhập đủ thông tin.";
                    return Content(HttpStatusCode.OK, response);
                }

                // Get company info from JWT token
                //var tokenCompanyId = JwtHelper.GetCompanyIdFromToken(Request);
                //var accountId = JwtHelper.GetAccountIdFromToken(Request);
                var myEmployeeId = JwtHelper.GetAccountMapIDFromToken(Request);

                // Validate JWT token info
                //if (tokenCompanyId != request.CompanyId)
                //{
                //    response.Code = ResponseResultEnum.InvalidToken.Value();
                //    response.Message = "Thông tin công ty không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}

                //if (!ModelState.IsValid)
                //{
                //    response.Code = ResponseResultEnum.InvalidInput.Value();
                //    response.Message = "Thông tin đầu vào không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}

                var result = BoFactory.Employee.DeleteMultiEmployeeAsync(request, myEmployeeId);

                if (result.Code == ResponseResultEnum.NotFound.Value())
                {
                    return Content(HttpStatusCode.OK, result);
                }

                if (result.Code == ResponseResultEnum.InvalidInput.Value())
                {
                    return Content(HttpStatusCode.OK, result);
                }

                if (result.Code == ResponseResultEnum.Failed.Value() || result.Code == ResponseResultEnum.SystemError.Value())
                {
                    return Content(HttpStatusCode.OK, result);
                }

                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("DeleteMultiEmployee Exception company_id {0}, EX:", request?.CompanyId, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Reset employee password
        /// </summary>
        [HttpPost]
        [Route("reset-password")]
        public IHttpActionResult ResetEmployeePassword([FromUri] int employee_id, [FromUri] int company_id, 
            [FromBody] ResetEmployeePasswordRequest request)
        {
            var response = new ApiResult<bool>()
            {
                Data = false,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng nhập đủ thông tin.";
                    return Content(HttpStatusCode.OK, response);
                }

                // Get company info from JWT token
                //var tokenCompanyId = JwtHelper.GetCompanyIdFromToken(Request);
                //var accountId = JwtHelper.GetAccountIdFromToken(Request);

                // Validate JWT token info
                //if (tokenCompanyId != company_id)
                //{
                //    response.Code = ResponseResultEnum.InvalidToken.Value();
                //    response.Message = "Thông tin công ty không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}

                // Validate input
                if (employee_id <= 0 || company_id <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thông tin nhân viên không hợp lệ.";
                    return Content(HttpStatusCode.OK, response);
                }

                //if (!ModelState.IsValid)
                //{
                //    response.Code = ResponseResultEnum.InvalidInput.Value();
                //    response.Message = "Thông tin đầu vào không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}

                var result = BoFactory.Employee.ResetEmployeePasswordAsync(employee_id, company_id, request);

                if (result.Code == ResponseResultEnum.InvalidInput.Value())
                {
                    return Content(HttpStatusCode.OK, result);
                }

                if (result.Code == ResponseResultEnum.Failed.Value() || result.Code == ResponseResultEnum.SystemError.Value())
                {
                    return Content(HttpStatusCode.OK, result);
                }

                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("ResetEmployeePassword Exception employee_id {0}, company_id {1}, EX:", employee_id, company_id, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Update employee details
        /// </summary>
        [RequiredPermission]
        [HttpPost]
        [Route("update-details")]
        public IHttpActionResult UpdateEmployeeDetails([FromUri] int employee_id, [FromUri] int company_id, 
            [FromBody] UpdateEmployeeDetailsRequest request)
        {
            var response = new ApiResult<bool>()
            {
                Data = false,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng nhập đủ thông tin.";
                    return Content(HttpStatusCode.OK, response);
                }

                // Get company info from JWT token
                //var tokenCompanyId = JwtHelper.GetCompanyIdFromToken(Request);
                //var accountId = JwtHelper.GetAccountIdFromToken(Request);

                // Validate JWT token info
                //if (tokenCompanyId != company_id)
                //{
                //    response.Code = ResponseResultEnum.InvalidToken.Value();
                //    response.Message = "Thông tin công ty không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}
                var role = JwtHelper.GetRoleFromToken(Request);

                // Validate input
                if (employee_id <= 0 || company_id <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thông tin nhân viên không hợp lệ.";
                    return Content(HttpStatusCode.OK, response);
                }

                //if (!ModelState.IsValid)
                //{
                //    response.Code = ResponseResultEnum.InvalidInput.Value();
                //    response.Message = "Thông tin đầu vào không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}

                var result = BoFactory.Employee.UpdateEmployeeDetailsAsync(employee_id, company_id, request, role);

                if (result.Code == ResponseResultEnum.InvalidInput.Value())
                {
                    return Content(HttpStatusCode.OK, result);
                }

                if (result.Code == ResponseResultEnum.NotFound.Value())
                {
                    return Content(HttpStatusCode.OK, result);
                }

                if (result.Code == ResponseResultEnum.Failed.Value() || result.Code == ResponseResultEnum.SystemError.Value())
                {
                    return Content(HttpStatusCode.OK, result);
                }

                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("UpdateEmployeeDetails Exception employee_id {0}, company_id {1}, EX:", employee_id, company_id, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Get employee filter list
        /// </summary>
        [RequiredPermission(WebPermissionKeys.EmployeeViewList)]
        [HttpPost]
        [Route("list")]
        public IHttpActionResult GetEmployeeFilterList([FromBody] EmployeeFilterListRequest request)
        {
            var response = new ApiResult<EmployeeFilterListResponse>()
            {
                Data = new EmployeeFilterListResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng nhập đủ thông tin.";
                    return Content(HttpStatusCode.OK, response);
                }

                // Get company info from JWT token
                //var tokenCompanyId = JwtHelper.GetCompanyIdFromToken(Request);
                //var accountId = JwtHelper.GetAccountIdFromToken(Request);

                // Validate JWT token info
                //if (tokenCompanyId != request.CompanyId)
                //{
                //    response.Code = ResponseResultEnum.InvalidToken.Value();
                //    response.Message = "Thông tin công ty không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}

                // if (!ModelState.IsValid)
                // {
                //     response.Code = ResponseResultEnum.InvalidInput.Value();
                //     response.Message = "Thông tin đầu vào không hợp lệ.";
                //     return Content(HttpStatusCode.OK, response);
                // }

                var employeeId = JwtHelper.GetAccountMapIDFromToken(Request);
                var userRole = JwtHelper.GetRoleFromToken(Request);

                var result = BoFactory.Employee.GetEmployeeFilterListAsync(request, employeeId, userRole);

                if (result.Code == ResponseResultEnum.NotFound.Value())
                {
                    return Content(HttpStatusCode.OK, result);
                }

                if (result.Code == ResponseResultEnum.InvalidInput.Value())
                {
                    return Content(HttpStatusCode.OK, result);
                }

                if (result.Code == ResponseResultEnum.Failed.Value() || result.Code == ResponseResultEnum.SystemError.Value())
                {
                    return Content(HttpStatusCode.OK, result);
                }

                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("GetEmployeeFilterList Exception company_id {0}, EX:", request?.CompanyId, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Get next employee code
        /// </summary>
        [HttpGet]
        [Route("get-last-item")]
        public IHttpActionResult GetNextEmployeeCode([FromUri] int company_id)
        {
            var response = new ApiResult<NextEmployeeCodeDto>()
            {
                Data = new NextEmployeeCodeDto(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Get company info from JWT token
                //var tokenCompanyId = JwtHelper.GetCompanyIdFromToken(Request);
                //var accountId = JwtHelper.GetAccountIdFromToken(Request);

                // Validate JWT token info
                //if (tokenCompanyId != company_id)
                //{
                //    response.Code = ResponseResultEnum.InvalidToken.Value();
                //    response.Message = "Thông tin công ty không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}

                var request = new NextEmployeeCodeRequest { CompanyId = company_id };

                //if (!ModelState.IsValid)
                //{
                //    response.Code = ResponseResultEnum.InvalidInput.Value();
                //    response.Message = "Thông tin đầu vào không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}

                var result = BoFactory.Employee.GetNextEmployeeCodeAsync(request);

                if (result.Code == ResponseResultEnum.InvalidInput.Value())
                {
                    return Content(HttpStatusCode.OK, result);
                }

                if (result.Code == ResponseResultEnum.Failed.Value() || result.Code == ResponseResultEnum.SystemError.Value())
                {
                    return Content(HttpStatusCode.OK, result);
                }

                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("GetNextEmployeeCode Exception company_id {0}, EX:", company_id, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Lấy danh sách trạng thái nhân viên (enum) động
        /// </summary>
        [HttpGet]
        [Route("get-employee-status-enum")]
        public IHttpActionResult GetEmployeeStatusEnum()
        {
            var response = new ApiResult<List<EnumToList>>()
            {
                Data = new List<EnumToList>(),
                Code = ResponseResultEnum.Success.Value(),
                Message = "Lấy danh sách trạng thái nhân viên thành công."
            };
            try
            {
                response.Data = typeof(EmployeeStatusEnum).ToList();
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("GetEmployeeStatusEnum Exception: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Lấy danh sách role user (không bao gồm SystemAdmin)
        /// </summary>
        [HttpGet]
        [Route("roles")]
        public IHttpActionResult GetUserRoles()
        {
            var response = new ApiResult<List<EnumToList>>()
            {
                Data = new List<EnumToList>(),
                Code = ResponseResultEnum.Success.Value(),
                Message = "Lấy danh sách vai trò thành công."
            };
            try
            {
                var allRoles = typeof(UserRole).ToList();
                // Loại bỏ SystemAdmin (Key = 1)
                response.Data = allRoles.FindAll(r => r.Key != (int)UserRole.SystemAdmin);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("GetUserRoles Exception: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        [HttpPost]
        [Route("list-by-shift-assignment")]
        public IHttpActionResult ListForAddShiftAssignment([FromBody] EmployeesInfo_ForAddShiftRequest request)
        {
            var response = new ApiResult<List<EmployeesInfo_ForAddShiftResponse>>()
            {
                Data = new List<EmployeesInfo_ForAddShiftResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountId = JwtHelper.GetAccountIdFromToken(Request);
                DateTime dateFrom = DateTime.Now.GetBeginOfDay();
                if (string.IsNullOrEmpty(request.WorkingDay) == false)
                {
                    dateFrom = DateTime.ParseExact(
                        request.WorkingDay,
                        "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture
                    );
                }
                response = BoFactory.ShiftAssignment.EmployeesInfo_GetDetailForAddShift(companyId, accountId, request.ShiftId, request.BranchId, request.IsOnlyBranch, dateFrom);

                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("GetEmployeeStatusEnum Exception: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }
    }
}