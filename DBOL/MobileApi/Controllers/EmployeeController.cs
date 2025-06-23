using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Employee;
using Logger;
using MyUtility.Extensions;
using System;
using System.Net;
using System.Web.Http;
using TanTamApi.JWT.Helper;
using TanTamApi.Enum;

namespace TanTamApi.Controllers
{
    [RoutePrefix("api/employee")]
    public class EmployeeController : ApiController
    {
        /// <summary>
        /// Get employee detail by id
        /// </summary>
        //[Authorize]
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

                var result = BoFactory.Employee.GetEmployeeDetailAsync(request);

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
        //[Authorize]
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

                var result = BoFactory.Employee.GetEmployeeListAsync(request);
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
        //[Authorize]
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

                // Validate JWT token info
                //if (tokenCompanyId != request.CompanyId)
                //{
                //    response.Code = ResponseResultEnum.InvalidToken.Value();
                //    response.Message = "Thông tin công ty không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}

                // Set additional info
                request.Role = (int)UserRole.Employees;
                request.DeviceId = Request.Headers.UserAgent?.ToString() ?? "";

                //if (!ModelState.IsValid)
                //{
                //    response.Code = ResponseResultEnum.InvalidInput.Value();
                //    response.Message = "Thông tin đầu vào không hợp lệ.";
                //    return Content(HttpStatusCode.OK, response);
                //}

                var result = BoFactory.Employee.CreateEmployeeAsync(request);
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
        //[Authorize]
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

                var result = BoFactory.Employee.DeleteEmployeeAsync(employee_id, company_id);

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
        //[Authorize]
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

                var result = BoFactory.Employee.DeleteMultiEmployeeAsync(request);

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
        //[Authorize]
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
        //[Authorize]
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

                var result = BoFactory.Employee.UpdateEmployeeDetailsAsync(employee_id, company_id, request);

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
        //[Authorize]
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

                var result = BoFactory.Employee.GetEmployeeFilterListAsync(request);

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
        //[Authorize]
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

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok("Employee Controller");
        }
    }
} 