using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Shift;
using BussinessObject.Permission;
using DataAccess;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using Newtonsoft.Json;
using TanTamApi.JWT.Helper;
using TanTamApi.JWT.Middleware;

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

        [ApiAuthorize]
        [RequiredPermission]
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

        [ApiAuthorize]
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

        [ApiAuthorize]
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

        [ApiAuthorize]
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
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                response = BoFactory.Payroll.Payroll_StatusClockInOutShift(companyId,accountIdMap, DateTime.Now, timekeeper_device, is_show_button, isInitial);
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
            var response = new ApiResult<ClockInOutShiftResponse>()
            {
                Data = new ClockInOutShiftResponse(),
                Code = ResponseResultEnum.Success.Value(),
                Message = ResponseResultEnum.Success.Text()
            };
            try
            {
                CommonLogger.PerformanceLogger.DebugFormat("clock-in-out-shift {0}", JsonConvert.SerializeObject(request));
                var accountIdMap = JwtHelper.GetAccountMapIDFromToken(Request);
                var companyIdMap = JwtHelper.GetCompanyIdFromToken(Request);
                response = BoFactory.Payroll.Payroll_ClockInOutShift(request, accountIdMap, companyIdMap,DateTime.Now);
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
        [ApiAuthorize]
        [RequiredPermission]
        [HttpGet]
        [Route("list-shift-assignment-with-shift")]
        public IHttpActionResult ListShiftAssignmentWithShift(
            int page = 1,
            int page_size = 15,
            string status = "active",
            int? start_hour_value = null,
            int? end_hour_value = null,
            string keyword = null)
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

                // Create request object from query parameters
                var request = new GetListShiftAssignmentWithShiftRequest
                {
                    Page = page,
                    PageSize = page_size,
                    Status = status,
                    StartHourValue = start_hour_value,
                    EndHourValue = end_hour_value,
                    Keyword = keyword
                };

                var result = BoFactory.Shift.GetListShiftAssignmentWithShift(companyId, employeeId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ListShiftAssignmentWithShift Exception.", ex);
                return Content(HttpStatusCode.InternalServerError, new ApiResult<object>
                {
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình xử lý."
                });
            }
        }

        /// <summary>
        /// Get detailed shift assignment with shift information by ID
        /// </summary>
        [ApiAuthorize]
        [HttpGet]
        [Route("detail-shift-assignment-with-shift")]
        public IHttpActionResult DetailShiftAssignmentWithShift(int id)
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

                if (id <= 0)
                {
                    return Content(HttpStatusCode.BadRequest, new ApiResult<object>
                    {
                        Code = ResponseResultEnum.InvalidData.Value(),
                        Message = "ID shift assignment không hợp lệ"
                    });
                }

                var result = BoFactory.Shift.GetShiftAssignmentDetailWithShift(id, companyId, employeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("DetailShiftAssignmentWithShift Exception.", ex);
                return Content(HttpStatusCode.InternalServerError, new ApiResult<object>
                {
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình xử lý."
                });
            }
        }

        /// <summary>
        /// Update shift assignment with shift information
        /// </summary>
        [ApiAuthorize]
        [RequiredPermission]
        [HttpPost]
        [Route("update-shift-assignment-with-shift")]
        public IHttpActionResult UpdateShiftAssignmentWithShift([FromBody] ShiftUpdateAndAssignRequest request)
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

                if (request == null || string.IsNullOrEmpty(request.Id))
                {
                    return Content(HttpStatusCode.BadRequest, new ApiResult<object>
                    {
                        Code = ResponseResultEnum.InvalidData.Value(),
                        Message = "Dữ liệu không hợp lệ"
                    });
                }

                var result = BoFactory.Shift.UpdateShiftAssignmentWithShiftSimplified(request, companyId, employeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("UpdateShiftAssignmentWithShift Exception.", ex);
                return Content(HttpStatusCode.InternalServerError, new ApiResult<object>
                {
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình xử lý."
                });
            }
        }

        /// <summary>
        /// API để xóa shift assignment cùng với shift
        /// </summary>
        [ApiAuthorize]
        [RequiredPermission]
        [HttpPost]
        [Route("delete-shift-assignment-with-shift")]
        public IHttpActionResult DeleteShiftAssignmentWithShift([FromBody] DeleteShiftAssignmentRequest request)
        {
            var response = new ApiResult<DeleteShiftAssignmentResponse>()
            {
                Data = new DeleteShiftAssignmentResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var employeeId = JwtHelper.GetAccountMapIDFromToken(Request);

                if (companyId <= 0 || employeeId <= 0)
                {
                    return Content(HttpStatusCode.Unauthorized, new ApiResult<DeleteShiftAssignmentResponse>
                    {
                        Code = ResponseResultEnum.InvalidToken.Value(),
                        Message = "Phiên đăng nhập không hợp lệ",
                        Data = new DeleteShiftAssignmentResponse()
                    });
                }

                if (request == null || string.IsNullOrEmpty(request.Id))
                {
                    return Content(HttpStatusCode.BadRequest, new ApiResult<DeleteShiftAssignmentResponse>
                    {
                        Code = ResponseResultEnum.InvalidData.Value(),
                        Message = "Dữ liệu không hợp lệ",
                        Data = new DeleteShiftAssignmentResponse()
                    });
                }


                response = BoFactory.Shift.DeleteShiftAssignmentWithShift(int.Parse(request.Id), companyId, employeeId);
                
                if (response.Code == ResponseResultEnum.Success.Value())
                {
                    return Ok(response);
                }
                else if (response.Code == ResponseResultEnum.InvalidData.Value())
                {
                    return Content(HttpStatusCode.BadRequest, response);
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, response);
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftController.DeleteShiftAssignmentWithShift - Error occurred", ex);
                return Content(HttpStatusCode.InternalServerError, new ApiResult<DeleteShiftAssignmentResponse>
                {
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Đã xảy ra lỗi trong quá trình xử lý",
                    Data = new DeleteShiftAssignmentResponse()
                });
            }
        }

        [ApiAuthorize]
        [RequiredPermission]
        [HttpPost]
        [Route("summary-employee-shift")]
        public IHttpActionResult SummaryEmployeeShift([FromBody] EmployeeShiftSummaryRequest request)
        {
            var response = new ApiResult<EmployeeShiftSummaryResponse>()
            {
                Data = new EmployeeShiftSummaryResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {

                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var employeeId = JwtHelper.GetAccountMapIDFromToken(Request);
                var role = JwtHelper.GetRoleFromToken(Request);

                if (companyId <= 0 || employeeId <= 0)
                {
                    return Content(HttpStatusCode.Unauthorized, new ApiResult<EmployeeShiftSummaryResponse>
                    {
                        Code = ResponseResultEnum.InvalidToken.Value(),
                        Message = "Phiên đăng nhập không hợp lệ",
                        Data = new EmployeeShiftSummaryResponse()
                    });
                }

                // Sử dụng PermissionHelper để kiểm tra quyền
                // var permissionKeysToCheck = new List<string> 
                // { 
                //     MobilePermissionKeys.MobileWorkTimekeeping,
                //     WebPermissionKeys.ShiftViewSummaryEmployee,
                //     WebPermissionKeys.EmployeeViewList
                // };
                
                // var validPermissions = PermissionHelper.GetValidPermissions(employeeId, permissionKeysToCheck, role);
                
                // // Kiểm tra logic quyền: có MobileWorkTimekeeping HOẶC (có ShiftViewSummaryEmployee VÀ EmployeeViewList)
                // var hasPermission = validPermissions.Contains(MobilePermissionKeys.MobileWorkTimekeeping) || 
                //                   (validPermissions.Contains(WebPermissionKeys.ShiftViewSummaryEmployee) && 
                //                    validPermissions.Contains(WebPermissionKeys.EmployeeViewList));

                // if (!hasPermission)
                // {
                //     return Content(HttpStatusCode.Unauthorized, new ApiResult<EmployeeShiftSummaryResponse>
                //     {
                //         Code = ResponseResultEnum.InvalidToken.Value(),
                //         Message = "Phiên đăng nhập không hợp lệ",
                //         Data = new EmployeeShiftSummaryResponse()
                //     });
                // }
                
                // Set company_id from token if not provided in request
                if (request == null) request = new EmployeeShiftSummaryRequest();
                if (request.CompanyId <= 0) request.CompanyId = companyId;

                response = BoFactory.ShiftSummary.GetEmployeeShiftSummary(request, employeeId, role);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"SummaryEmployeeShift Exception.", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý.";
            }
            
            return Ok(response);
        }

        /// <summary>
        /// API để từ chối/xóa ca làm việc đã được đăng ký
        /// </summary>
        [ApiAuthorize]
        [RequiredPermission]
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
        [ApiAuthorize]
        [RequiredPermission]
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
        [ApiAuthorize]
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
        [ApiAuthorize(UserRole.SystemAdmin, UserRole.Manager, UserRole.RegionalManager, UserRole.BranchManager)]
        [HttpPost]
        [Route("check-in-out-shift")]
        public IHttpActionResult CheckInOutShift([FromBody] CheckInOutShiftUpdateRequest request)
        {
            try
            {
                // Call business logic to update check-in/out
                var userId = JwtHelper.GetAccountIdFromToken(Request);
                var result = BoFactory.Shift.UpdateCheckInOut(request, userId);
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
        [ApiAuthorize(UserRole.SystemAdmin, UserRole.Manager, UserRole.RegionalManager, UserRole.BranchManager)]
        [HttpPost]
        [Route("uncheckin-uncheckout-shift")]
        public IHttpActionResult UncheckInOutShift([FromBody] UncheckInOutShiftRequest request)
        {
            try
            {
                var userId = JwtHelper.GetAccountIdFromToken(Request);
                // Call business logic to uncheck in/out
                var result = BoFactory.Shift.UncheckInOut(request, userId);
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
        [ApiAuthorize]
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

        #region dùng cho admin add ca của nhân viên
        [ApiAuthorize]
        [HttpGet]
        [Route("for-register")]
        public IHttpActionResult ListShiftForRegister(int week_of_year , int year, int branch_id, string type)
        {
            var response = new ApiResult<List<ShiftLite_ForRegisterResponse>>()
            {
                Data = new List<ShiftLite_ForRegisterResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                ShiftLite_ForRegisterRequest request = new ShiftLite_ForRegisterRequest()
                {
                    WeekOfYear = week_of_year,
                    Year = year,
                    BranchId = branch_id,
                    Type = type
                };
                var companyID = JwtHelper.GetCompanyIdFromToken(Request);
                response = BoFactory.ShiftAssignment.ShiftLite_ForRegister(request, companyID);
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
            return Ok(response);
        }

        [ApiAuthorize]
        [HttpGet]
        [Route("history-employee-shift")]
        public IHttpActionResult HistoryEmployeeShift(int week_of_year, int year, int branch_id, int shift_id)
        {
            var response = new ApiResult<List<HistoryEmployeeShiftResponse>>()
            {
                Data = new List<HistoryEmployeeShiftResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };
            try
            {

                HistoryEmployeeShiftRequest request = new HistoryEmployeeShiftRequest()
                {
                    BranchId = branch_id,
                    WeekOfYear = week_of_year,
                    Year = year,
                    ShiftID = shift_id
                };
                var userId = JwtHelper.GetAccountIdFromToken(Request);
                var companyID = JwtHelper.GetCompanyIdFromToken(Request);
                response = BoFactory.ShiftAssignment.HistoryEmployeeShift(request,companyID);
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

            return Ok(response);
        }

        [ApiAuthorize]
        [HttpPost]
        [Route("register-shift-app")]
        public IHttpActionResult EmployeeRegisterShift([FromBody]  EmployeeRegisterShiftRequest request)
        {
            var response = new ApiResult<List<ShiftLite_ForRegisterResponse>>()
            {
                Data = new List<ShiftLite_ForRegisterResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var userId = JwtHelper.GetAccountIdFromToken(Request);
                var companyID = JwtHelper.GetCompanyIdFromToken(Request);
                var accountIdMap = JwtHelper.GetAccountMapIDFromToken(Request);
                var roleid = JwtHelper.GetRoleFromToken(Request);
                if (roleid == UserRole.SystemAdmin.Value() ) 
                {
                    DateTime dateFrom = DateTime.Now.GetBeginOfDay();
                    if (string.IsNullOrEmpty(request.WorkingDay) == false)
                    {
                        dateFrom = DateTime.ParseExact(
                            request.WorkingDay,
                            "yyyy-MM-dd HH:mm:ss",
                            CultureInfo.InvariantCulture
                        );
                    }
                    else
                    {
                        response.Code = ResponseResultEnum.InvalidData.Value();
                        response.Message = "chưa chọn ngày add ca";
                    }
                    response = BoFactory.ShiftAssignment.EmployeeRegisterShift(request, companyID, dateFrom);
                }
                else
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Chỉ có quản lý mới có quyền này";
                }
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
            return Ok(response);
        }

        [ApiAuthorize]
        [HttpPost]
        [Route("reject-shift-app")]
        public IHttpActionResult EmployeRrejectShift([FromBody]  EmployeeRejectShiftRequest request)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var userId = JwtHelper.GetAccountIdFromToken(Request);
                var companyID = JwtHelper.GetCompanyIdFromToken(Request);
                var roleid = JwtHelper.GetRoleFromToken(Request);
                if (roleid == UserRole.SystemAdmin.Value() && request.UserId > 0)
                {
                    response = BoFactory.ShiftAssignment.EmployeRrejectShift(request.id, request.UserId);
                }
                else
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                }
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
            return Ok(response);
        }
        #endregion 
    }
}