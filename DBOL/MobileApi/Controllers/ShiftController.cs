using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Shift;
using Logger;
using MyUtility.Extensions;
using Newtonsoft.Json;
using TanTamApi.JWT.Helper;

namespace TanTamApi.Controllers
{
    [RoutePrefix("api/shift")]
    public class ShiftController : ApiController
    {
        [HttpPost, Route("times-get")]
        public HttpResponseMessage GetTimes([FromBody] GetTimesRequest request)
        {
            var response = new ApiResult<TimesResponse>()
            {
                Data = new TimesResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };
            try
            {
                CommonLogger.PerformanceLogger.DebugFormat("times-get {0}", JsonConvert.SerializeObject(request));
                response = BoFactory.Shift.GetTimes(request.Lang ?? "vi");
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("shift GetTimes EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        [JWT.Middleware.Authorize]
        [HttpPost, Route("create-shift-and-assign-shift")]
        public HttpResponseMessage CreateShiftAndAssignShift([FromBody] ShiftCreateAndAssignRequest request)
        {
            var response = new ApiResult<ShiftCreateAndAssignResponse>()
            {
                Data = new ShiftCreateAndAssignResponse(),
                Code = ResponseResultEnum.Success.Value(),
                Message = ResponseResultEnum.Success.Text()
            };
            try
            {
                CommonLogger.PerformanceLogger.DebugFormat("create-shift-and-assign-shift request {0}", JsonConvert.SerializeObject(request));
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountMapID = JwtHelper.GetAccountMapIDFromToken(Request);
                response = BoFactory.Shift.ShiftCreateAndAssign(request, companyId , accountMapID);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("shift CreateShiftAndAssignShift EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [JWT.Middleware.Authorize]
        [HttpPost, Route("create-shift")]
        public HttpResponseMessage CreateShift([FromBody] ShiftCreateAndAssignRequest request)
        {
            var response = new ApiResult<ShiftCreateAndAssignResponse>()
            {
                Data = new ShiftCreateAndAssignResponse(),
                Code = ResponseResultEnum.Success.Value(),
                Message = ResponseResultEnum.Success.Text()
            };
            try
            {
                CommonLogger.PerformanceLogger.DebugFormat("create-shift request {0}", JsonConvert.SerializeObject(request));
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                response = BoFactory.Shift.ShiftCreateAndAssign(request, companyId, 0);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("shift CreateShiftAndAssignShift EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [JWT.Middleware.Authorize]
        [HttpGet, Route("list-employee-shift")]
        public HttpResponseMessage ListEmployeeShift(string working_day = "today,tomorrow")
        {
            var response = new ApiResult<List<ClockInOut_Shift>>()
            {
                Data = new List<ClockInOut_Shift>(),
                Code = ResponseResultEnum.Success.Value(),
                Message = ResponseResultEnum.Success.Text()
            };
            try
            {
                CommonLogger.PerformanceLogger.DebugFormat("list-employee-shift {0}", working_day);
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountId = JwtHelper.GetAccountIdFromToken(Request);
                var accountMapID = JwtHelper.GetAccountMapIDFromToken(Request);
                #region demo
                CommonLogger.PerformanceLogger.DebugFormat("list-employee-shift companyId {0}, accountId {1}, accountMapID {2}", companyId, accountId , accountMapID);
                #endregion
                response = BoFactory.Payroll.Payroll_User_GetList(companyId, accountMapID, working_day);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("shift ListEmployeeShift EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }
            CommonLogger.PerformanceLogger.DebugFormat("list-employee-shift response {0}", JsonConvert.SerializeObject(response));
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        [JWT.Middleware.Authorize]
        [HttpGet, Route("status-clock-in-out-shift")]
        public HttpResponseMessage StatusClockInOutShift(string timekeeper_device = "", int is_show_button = 0,bool isInitial = false)
        {
            var response = new ApiResult<StatusClockInOutShiftResponse>()
            {
                Data = new StatusClockInOutShiftResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };
            try
            {
                CommonLogger.PerformanceLogger.DebugFormat("status-clock-in-out-shift timekeeper_device {0},is_show_button {1},isInitial {2}", timekeeper_device, is_show_button, isInitial);
                var accountIdMap = JwtHelper.GetAccountMapIDFromToken(Request);
                response = BoFactory.Payroll.Payroll_StatusClockInOutShift(accountIdMap, DateTime.Now, timekeeper_device, is_show_button, isInitial);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("shift ListEmployeeShift EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }
            CommonLogger.PerformanceLogger.DebugFormat("status-clock-in-out-shift response  {0}", JsonConvert.SerializeObject(response));
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        [HttpPost, Route("clock-in-out-shift")]
        public HttpResponseMessage ClockInOutShift([FromBody] ClockInOutShiftRequest request)
        {
            var response = new ApiResult<object>()
            {
                Code = ResponseResultEnum.Success.Value(),
                Message = ResponseResultEnum.Success.Text()
            };
            try
            {
                CommonLogger.PerformanceLogger.DebugFormat("clock-in-out-shift {0}", JsonConvert.SerializeObject(request));
                var accountIdMap = JwtHelper.GetAccountMapIDFromToken(Request);
                var companyIdMap = JwtHelper.GetCompanyIdFromToken(Request);
                response.Data = BoFactory.Payroll.Payroll_ClockInOutShift(request, accountIdMap, companyIdMap,DateTime.Now);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("shift ListEmployeeShift EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }
            CommonLogger.PerformanceLogger.DebugFormat("clock-in-out-shift response  {0}", JsonConvert.SerializeObject(response));
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        /// <summary>
        /// Get list of shift assignments with shift details
        /// </summary>
        [TanTamApi.JWT.Middleware.Authorize]
        [HttpGet]
        [Route("list-shift-assignment-with-shift-v2")]
        public IHttpActionResult ListShiftAssignmentWithShiftV2()
        {
            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var employeeId = JwtHelper.GetAccountMapIDFromToken(Request);

                if (companyId <= 0 || employeeId <= 0)
                {
                    return Content(HttpStatusCode.Unauthorized, new ApiResult<object>
                    {
                        Code = ResponseResultEnum.InvalidToken.Value(),
                        Message = "Phiên đăng nhập không hợp lệ"
                    });
                }

                var result = BoFactory.Shift.GetListShiftAssignmentWithShift(companyId, employeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ListShiftAssignmentWithShiftV2 Exception.", ex);
                return Content(HttpStatusCode.InternalServerError, new ApiResult<object>
                {
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình xử lý."
                });
            }
        }

        [TanTamApi.JWT.Middleware.Authorize]
        [HttpPost]
        [Route("summary-employee-shift")]
        public IHttpActionResult SummaryEmployeeShift([FromBody] EmployeeShiftSummaryRequest request)
        {
            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var employeeId = JwtHelper.GetAccountMapIDFromToken(Request);

                if (companyId <= 0 || employeeId <= 0)
                {
                    return Content(HttpStatusCode.Unauthorized, new ApiResult<EmployeeShiftSummaryResponse>
                    {
                        Code = ResponseResultEnum.InvalidToken.Value(),
                        Message = "Phiên đăng nhập không hợp lệ",
                        Data = new EmployeeShiftSummaryResponse()
                    });
                }

                // Set company_id from token if not provided in request
                if (request == null) request = new EmployeeShiftSummaryRequest();
                if (request.CompanyId <= 0) request.CompanyId = companyId;

                var result = BoFactory.ShiftSummary.GetEmployeeShiftSummary(request, employeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"SummaryEmployeeShift Exception.", ex);
                return Content(HttpStatusCode.InternalServerError, new ApiResult<EmployeeShiftSummaryResponse>
                {
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình xử lý.",
                    Data = new EmployeeShiftSummaryResponse()
                });
            }
        }

        /// <summary>
        /// API để từ chối/xóa ca làm việc đã được đăng ký
        /// </summary>
        [TanTamApi.JWT.Middleware.Authorize]
        [HttpPost]
        [Route("reject-shift")]
        public IHttpActionResult RejectShift([FromBody] RejectShiftRequest request)
        {
            var response = new ApiResult<RejectShiftResponse>()
            {
                Data = new RejectShiftResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate request has user_id
                if (request != null && request.UserId <= 0)
                {
                    // If user_id not provided, get from token
                    request.UserId = JwtHelper.GetAccountIdFromToken(Request);
                }

                response = BoFactory.ShiftAssignment.RejectShift(request);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("ShiftController.RejectShift - Error occurred: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }

            return Ok(response);
        }

        /// <summary>
        /// API để đăng ký ca làm việc với shift_id, working_day, user_id
        /// </summary>
        [TanTamApi.JWT.Middleware.Authorize]
        [HttpPost]
        [Route("register-shift")]
        public IHttpActionResult RegisterShift([FromBody] RegisterShiftRequest request)
        {
            var response = new ApiResult<RegisterShiftResponse>()
            {
                Data = new RegisterShiftResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Auto-fill user_id from JWT token if not provided
                if (request != null && request.user_id <= 0)
                {
                    var userIdFromToken = JwtHelper.GetAccountIdFromToken(Request);
                    request.user_id = userIdFromToken;
                }

                response = BoFactory.ShiftAssignment.RegisterShift(request);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("ShiftController.RegisterShift - Error occurred: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }

            return Ok(response);
        }

        /// <summary>
        /// API để lấy danh sách ca làm việc theo company của user
        /// </summary>
        [TanTamApi.JWT.Middleware.Authorize]
        [HttpPost]
        [Route("list-shift")]
        public IHttpActionResult ListShift([FromBody] ListShiftRequest request)
        {
            var response = new ApiResult<ListShiftResponse>()
            {
                Data = new ListShiftResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Auto-fill user_id from JWT token if not provided
                if (request != null && string.IsNullOrEmpty(request.UserId))
                {
                    var userIdFromToken = JwtHelper.GetAccountIdFromToken(Request);
                    request.UserId = userIdFromToken.ToString();
                }

                response = BoFactory.ShiftAssignment.GetShiftList(request);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("ShiftController.ListShift - Error occurred: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }

            return Ok(response);
        }

        /// <summary>
        /// API để check in/out shift với thông tin chi tiết
        /// Format request: {"reason":"","id":"6862564289d492ce0d0f9c18","branch_id":"685e12a14c2104da69073d96","user_id":"685e123922a34XN0l","checkin_time":"2025-07-03 08:00:00","checkout_time":"2025-07-03 17:30:00","is_checkin":1,"is_checkout":1,"working_day":"2025-07-02 00:00:00"}
        /// </summary>
        [TanTamApi.JWT.Middleware.Authorize]
        [HttpPost]
        [Route("check-in-out-shift")]
        public IHttpActionResult CheckInOutShift([FromBody] CheckInOutShiftUpdateRequest request)
        {
            try
            {
                // Call business logic to update check-in/out
                var result = BoFactory.Shift.UpdateCheckInOut(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("ShiftController.CheckInOutShift - Error occurred: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, new ApiResult<CheckInOutShiftUpdateResponse>
                {
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình xử lý",
                    Data = new CheckInOutShiftUpdateResponse()
                });
            }
        }

        /// <summary>
        /// API để hủy check in/out shift
        /// Format request: {"id":"6862564289d492ce0d0f9c18","branch_id":"685e12a14c2104da69073d96","user_id":"685e123922a34XN0l","is_uncheckin":1,"reason":""}
        /// </summary>
        [TanTamApi.JWT.Middleware.Authorize]
        [HttpPost]
        [Route("uncheckin-uncheckout-shift")]
        public IHttpActionResult UncheckInOutShift([FromBody] UncheckInOutShiftRequest request)
        {
            try
            {
                // Call business logic to uncheck in/out
                var result = BoFactory.Shift.UncheckInOut(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("ShiftController.UncheckInOutShift - Error occurred: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, new ApiResult<UncheckInOutShiftResponse>
                {
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình xử lý",
                    Data = new UncheckInOutShiftResponse()
                });
            }
        }

        /// <summary>
        /// API để lấy thời gian chấm công của nhân viên theo ca làm việc
        /// </summary>
        [TanTamApi.JWT.Middleware.Authorize]
        [HttpGet]
        [Route("get-checked-time-employee-shift")]
        public IHttpActionResult GetCheckedTimeEmployeeShift(int employee_shift_id = 0)
        {
            if (employee_shift_id <= 0)
            {
                return Ok(new ApiResult<object>
                {
                    Code = 400,
                    Message = "employee_shift_id là bắt buộc và phải lớn hơn 0",
                    Data = null
                });
            }

            try
            {
                // Call business logic to get shift assignment user working day logs
                var result = BoFactory.ShiftAssignment.GetShiftAssignmentUserWorkingDayLogsByEmployeeShift(employee_shift_id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("ShiftController.GetCheckedTimeEmployeeShift - Error occurred: {0}", ex);
                return Ok(new ApiResult<object>
                {
                    Code = 500,
                    Message = "Đã xảy ra lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
    }
}