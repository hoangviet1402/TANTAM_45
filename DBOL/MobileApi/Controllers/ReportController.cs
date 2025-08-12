using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Report;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Http;
using TanTamApi.JWT.Helper;
using TanTamApi.JWT.Middleware;

namespace TanTamApi.Controllers
{
    [RoutePrefix("api/report")]
    public class ReportController : ApiController
    {
        /// <summary>
        /// Get dashboard employees growth statistics
        /// Company ID is automatically extracted from JWT token
        /// </summary>
        [ApiAuthorize]
        [HttpGet]
        [Route("dashboard-employees-growth")]
        public IHttpActionResult GetDashboardEmployeesGrowth([FromUri] int days_ago = 30)
        {
            var response = new ApiResult<DashboardEmployeesGrowthResponse>()
            {
                Data = new DashboardEmployeesGrowthResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Get company info from JWT token
                var tokenCompanyId = JwtHelper.GetCompanyIdFromToken(Request);
                var employeeId = JwtHelper.GetAccountMapIDFromToken(Request);
                var role = JwtHelper.GetRoleFromToken(Request);

                // Validate JWT token info
                if (tokenCompanyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidToken.Value();
                    response.Message = "Thông tin công ty trong token không hợp lệ.";
                    return Content(HttpStatusCode.OK, response);
                }

                var request = new DashboardEmployeesGrowthRequest 
                { 
                    CompanyId = tokenCompanyId,
                    DaysAgo = days_ago
                };

                // Basic validation is now done above with JWT token

                var result = BoFactory.Report.GetDashboardEmployeesGrowthAsync(request, employeeId, role);

                if (result.Code == ResponseResultEnum.NoData.Value())
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không có dữ liệu thống kê.";
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
                CommonLogger.DefaultLogger.ErrorFormat("GetDashboardEmployeesGrowth Exception days_ago {0}, EX:", days_ago, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Get working status statistics for today
        /// Company ID is automatically extracted from JWT token
        /// Working day is automatically set to current date
        /// 
        /// Two modes:
        /// 1. Summary mode (no parameters): Returns statistics totals
        /// 2. Detail mode (with page + type): Returns detailed list with pagination (15 items per page)
        /// </summary>
        [ApiAuthorize]
        [HttpGet]
        [Route("who-is-working-total")]
        public IHttpActionResult GetWorkingTotal([FromUri] int? page = null, [FromUri] string type = null)
        {
            var response = new ApiResult<WorkingTotalResponse>()
            {
                Data = new WorkingTotalResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Get company info from JWT token
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var employeeId = JwtHelper.GetAccountMapIDFromToken(Request);
                var role = JwtHelper.GetRoleFromToken(Request);

                // Validate JWT token info
                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidToken.Value();
                    response.Message = "Thông tin công ty trong token không hợp lệ.";
                    return Content(HttpStatusCode.OK, response);
                }

                var request = new WorkingTotalRequest 
                { 
                    page = page,
                    type = type
                };

                // Check if this is detail mode (page + type provided) or summary mode
                if (page.HasValue && !string.IsNullOrEmpty(type))
                {
                    // Detail mode: Return paginated list
                    var detailResult = BoFactory.Report.GetWorkingDetailAsync(request, companyId, employeeId, role);
                    return Content(HttpStatusCode.OK, detailResult);
                }
                else
                {
                    // Summary mode: Return statistics totals
                    var summaryResult = BoFactory.Report.GetWorkingTotalAsync(request, companyId, employeeId, role);
                    return Content(HttpStatusCode.OK, summaryResult);
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("GetWorkingTotal Exception Page: {0}, Type: {1}, EX: {2}", 
                    page, type, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Get working time statistics for dashboard
        /// Company ID is automatically extracted from JWT token
        /// Returns percentage statistics for different working time categories over specified period
        /// 
        /// Supported time periods: 1, 3, 7, 30, 90 days
        /// - Values will be automatically normalized to the nearest supported period
        /// - Example: days_ago=5 will be normalized to 7 days
        /// 
        /// Categories:
        /// - no_timekeeping: Chưa chấm công - Hoàn toàn không có check in và check out (%)
        /// - no_clock_in_or_out: Thiếu chấm công - Chỉ có 1 trong 2 (hoặc check in hoặc check out) (%)
        /// - good_timekeeping: Chấm công tốt - Có đủ cả check in và check out đúng giờ (%)
        /// - in_late_out_soon: Trễ giờ công - Check in muộn hoặc check out sớm (%)
        /// 
        /// Example usage:
        /// - GET /api/report/dashboard-working-time?days_ago=1  (Today only)
        /// - GET /api/report/dashboard-working-time?days_ago=7  (Last 7 days)
        /// - GET /api/report/dashboard-working-time?days_ago=30 (Last 30 days)
        /// </summary>
        [ApiAuthorize]
        [HttpGet]
        [Route("dashboard-working-time")]
        public IHttpActionResult GetDashboardWorkingTime([FromUri] int days_ago = 7)
        {
            var response = new ApiResult<DashboardWorkingTimeResponse>()
            {
                Data = new DashboardWorkingTimeResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Get company info from JWT token
                var tokenCompanyId = JwtHelper.GetCompanyIdFromToken(Request);
                var employeeId = JwtHelper.GetAccountMapIDFromToken(Request);
                var role = JwtHelper.GetRoleFromToken(Request);

                // Validate JWT token info
                if (tokenCompanyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidToken.Value();
                    response.Message = "Thông tin công ty trong token không hợp lệ.";
                    return Content(HttpStatusCode.OK, response);
                }

                // Input validation and normalization
                if (days_ago < 0)
                {
                    days_ago = 7; // Default to 7 days for negative values
                }

                var request = new DashboardWorkingTimeRequest 
                { 
                    CompanyId = tokenCompanyId,
                    DaysAgo = days_ago
                };

                // Call business logic (will auto-normalize to supported values: 1, 3, 7, 30, 90)
                var result = BoFactory.Report.GetDashboardWorkingTimeAsync(request, employeeId, role);

                if (result.Code == ResponseResultEnum.NoData.Value())
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không có dữ liệu thống kê thời gian làm việc.";
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
                CommonLogger.DefaultLogger.ErrorFormat("GetDashboardWorkingTime Exception days_ago {0}, EX:", days_ago, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        /// <summary>
        /// Get dashboard devices statistics  
        /// Company ID is automatically extracted from JWT token
        /// Working day is automatically set to current date
        /// 
        /// Returns statistics about employees who have checked in or out:
        /// - count: Number of employees who have at least one check-in or check-out
        /// - percent: Percentage of employees with check-in/out data relative to total active employees
        /// 
        /// Response format: {"admin": {"percent": 66.7, "count": 2}}
        /// </summary>
        [ApiAuthorize]
        [HttpGet]
        [Route("dashboard-devices")]
        public IHttpActionResult GetDashboardDevices()
        {
            var response = new ApiResult<DashboardDevicesResponse>()
            {
                Data = new DashboardDevicesResponse
                {
                    admin = new DashboardDevicesAdmin
                    {
                        count = 0,
                        percent = 0.0m
                    }
                },
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Get company info from JWT token
                var tokenCompanyId = JwtHelper.GetCompanyIdFromToken(Request);

                // Validate JWT token info
                if (tokenCompanyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidToken.Value();
                    response.Message = "Thông tin công ty trong token không hợp lệ.";
                    return Content(HttpStatusCode.OK, response);
                }

                var request = new DashboardDevicesRequest 
                { 
                    CompanyId = tokenCompanyId,
                    WorkingDay = DateTime.Now.Date // Use current date
                };

                // Call business logic
                var result = BoFactory.Report.GetDashboardDevicesAsync(request);

                if (result.Code == ResponseResultEnum.NoData.Value())
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không có dữ liệu thống kê thiết bị chấm công.";
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
                CommonLogger.DefaultLogger.ErrorFormat("GetDashboardDevices Exception CompanyId: {0}, EX: {1}", 
                    JwtHelper.GetCompanyIdFromToken(Request), ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        [ApiAuthorize]
        [HttpGet]
        [Route("get-employee-report")]
        public IHttpActionResult GetEmployeeReport(string from_date, string to_date)
        {
            //{"from_date": "2025-07-23", "to_date": "2025-07-23"}
            var response = new ApiResult<EmployeePayrollReportResponse>()
            {
                Data = new EmployeePayrollReportResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Get company info from JWT token
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountMapID = JwtHelper.GetAccountMapIDFromToken(Request);
                DateTime dateFrom = DateTime.Now.GetBeginOfDay();
                DateTime dateTo = DateTime.Now.GetBeginOfDay();
                if (string.IsNullOrEmpty(from_date) == false)
                {
                    dateFrom = DateTime.ParseExact(
                        from_date,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture
                    );
                    
                }
                else
                {
                    // đầu tháng
                    dateFrom = dateFrom.FirstDayOfMonth();
                }
                if (string.IsNullOrEmpty(to_date) == false)
                {
                    dateTo = DateTime.ParseExact(
                        to_date,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture
                    );
                }
                else
                {
                    // cuối tháng
                    dateTo = dateFrom.FirstDayOfMonth();
                }
                response = BoFactory.Report.GetEmployeeReport(accountMapID, companyId, dateFrom, dateTo);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("GetDashboardDevices Exception CompanyId: {0}, EX: {1}",
                    JwtHelper.GetCompanyIdFromToken(Request), ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        [ApiAuthorize]
        [HttpGet]
        [Route("get-employee-report-detail")]
        public IHttpActionResult GetEmployeeReportDetail(string from_date, string to_date,string detail_type = "SOLAN_QUENCHECKINOUT")
        {
            //{"from_date": "2025-07-23", "to_date": "2025-07-23"}
            var response = new ApiResult<List<EmployeePayrollReportDetailResponse>>()
            {
                Data = new List<EmployeePayrollReportDetailResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Get company info from JWT token
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                var accountMapID = JwtHelper.GetAccountMapIDFromToken(Request);
                DateTime dateFrom = DateTime.Now.GetBeginOfDay();
                DateTime dateTo = DateTime.Now.GetBeginOfDay();
                if (string.IsNullOrEmpty(from_date) == false)
                {
                    dateFrom = DateTime.ParseExact(
                        from_date,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture
                    );

                }
                else
                {
                    // đầu tháng
                    dateFrom = dateFrom.FirstDayOfMonth();
                }
                if (string.IsNullOrEmpty(to_date) == false)
                {
                    dateTo = DateTime.ParseExact(
                        to_date,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture
                    );
                }
                else
                {
                    // cuối tháng
                    dateTo = dateFrom.FirstDayOfMonth();
                }
                switch (detail_type.ToUpper())
                {
                    case "NGAYCONG_THUCTE":
                        response = BoFactory.Report.GetEmployeeReport_NGAYCONG_THUCTE(accountMapID, companyId, dateFrom, dateTo);
                        break;
                    case "GIOCONG_THUCTE":
                        response = BoFactory.Report.GetEmployeeReport_GIOCONG_THUCTE(accountMapID, companyId, dateFrom, dateTo);
                        break;
                    case "SOGIO_VESOM":
                        response = BoFactory.Report.GetEmployeeReport_SOGIO_VESOM(accountMapID, companyId, dateFrom, dateTo);
                        break;
                    case "SOGIO_DIMUON":
                        response = BoFactory.Report.GetEmployeeReport_SOGIO_DIMUON(accountMapID, companyId, dateFrom, dateTo);
                        break;
                    case "SOLAN_QUENCHECKIN":
                        response = BoFactory.Report.GetEmployeeReport_SOLAN_QUENCHECKIN(accountMapID, companyId, dateFrom, dateTo);
                        break;
                    case "SOLAN_QUENCHECKOUT":
                        response = BoFactory.Report.GetEmployeeReport_SOLAN_QUENCHECKOUT(accountMapID, companyId, dateFrom, dateTo);
                        break;
                    case "SOLAN_QUENCHECKINOUT":
                        response = BoFactory.Report.GetEmployeeReport_SOLAN_QUENCHECKINOUT(accountMapID, companyId, dateFrom, dateTo);
                        break;
                    default:
                        response = BoFactory.Report.GetEmployeeReport_NGAYCONG_THUCTE(accountMapID, companyId, dateFrom, dateTo);
                        break;
                }
                response.Data = response.Data.OrderByDescending(x => x.Title).ToList();
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("GetDashboardDevices Exception CompanyId: {0}, EX: {1}",
                    JwtHelper.GetCompanyIdFromToken(Request), ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        [ApiAuthorize(UserRole.SystemAdmin)]
        [HttpGet]
        [Route("clock-tab-employee-clock")]
        public IHttpActionResult ClockTabEmployeeClock(string from_date, string to_date)
        {
            //{"from_date": "2025-07-23", "to_date": "2025-07-23"}
            var response = new ApiResult<ClockTabEmployeeClockResponse>()
            {
                Data = new ClockTabEmployeeClockResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Get company info from JWT token
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                DateTime dateFrom = DateTime.Now.GetBeginOfDay();
                DateTime dateTo = DateTime.Now.EndOfDate();
                if (string.IsNullOrEmpty(from_date) == false)
                {
                    dateFrom = DateTime.ParseExact(
                        from_date,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture
                    );

                }
                else
                {
                    // đầu tháng
                    dateFrom = dateFrom.FirstDayOfMonth();
                }
                if (string.IsNullOrEmpty(to_date) == false)
                {
                    dateTo = DateTime.ParseExact(
                        to_date,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture
                    );
                }
                else
                {
                    // cuối tháng
                    dateTo = dateFrom.FirstDayOfMonth();
                }
                response = BoFactory.Report.ClockTabEmployeeClock(companyId, dateFrom.GetBeginOfDay(), dateTo.EndOfDate());
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("GetDashboardDevices Exception CompanyId: {0}, EX: {1}",
                    JwtHelper.GetCompanyIdFromToken(Request), ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        [ApiAuthorize(UserRole.SystemAdmin)]
        [HttpGet]
        [Route("clock-tab-not-clock-in-out")]
        public IHttpActionResult ClockTabNotClockInOut(string from_date, string to_date)
        {
            //{"from_date": "2025-07-23", "to_date": "2025-07-23"}
            var response = new ApiResult<ClockTabNotClockInOutResponse>()
            {
                Data = new ClockTabNotClockInOutResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Get company info from JWT token
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                DateTime dateFrom = DateTime.Now.GetBeginOfDay();
                DateTime dateTo = DateTime.Now.EndOfDate();
                if (string.IsNullOrEmpty(from_date) == false)
                {
                    dateFrom = DateTime.ParseExact(
                        from_date,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture
                    );

                }
                else
                {
                    // đầu tháng
                    dateFrom = dateFrom.FirstDayOfMonth();
                }
                if (string.IsNullOrEmpty(to_date) == false)
                {
                    dateTo = DateTime.ParseExact(
                        to_date,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture
                    );
                }
                else
                {
                    // cuối tháng
                    dateTo = dateFrom.FirstDayOfMonth();
                }
                response = BoFactory.Report.ClockTabNotClockInOut(companyId, dateFrom.GetBeginOfDay(), dateTo.EndOfDate());
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("GetDashboardDevices Exception CompanyId: {0}, EX: {1}",
                    JwtHelper.GetCompanyIdFromToken(Request), ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        [ApiAuthorize(UserRole.SystemAdmin)]
        [HttpGet]
        [Route("clock-tab-clock-late-soon")]
        public IHttpActionResult ClockTabClockLateSoon(string from_date, string to_date)
        {
            //{"from_date": "2025-07-23", "to_date": "2025-07-23"}
            var response = new ApiResult<ClockTabClockLateSoonResponse>()
            {
                Data = new ClockTabClockLateSoonResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Get company info from JWT token
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                DateTime dateFrom = DateTime.Now.GetBeginOfDay();
                DateTime dateTo = DateTime.Now.EndOfDate();
                if (string.IsNullOrEmpty(from_date) == false)
                {
                    dateFrom = DateTime.ParseExact(
                        from_date,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture
                    );

                }
                else
                {
                    // đầu tháng
                    dateFrom = dateFrom.FirstDayOfMonth();
                }
                if (string.IsNullOrEmpty(to_date) == false)
                {
                    dateTo = DateTime.ParseExact(
                        to_date,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture
                    );
                }
                else
                {
                    // cuối tháng
                    dateTo = dateFrom.FirstDayOfMonth();
                }
                response = BoFactory.Report.ClockTabClockLateSoon(companyId, dateFrom.GetBeginOfDay(), dateTo.EndOfDate());
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("GetDashboardDevices Exception CompanyId: {0}, EX: {1}",
                    JwtHelper.GetCompanyIdFromToken(Request), ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        [ApiAuthorize(UserRole.SystemAdmin)]
        [HttpGet]
        [Route("timesheet-tab-working-hours")]
        public IHttpActionResult TimesheetTabWorkingHours(string from_date, string to_date)
        {
            //{"from_date": "2025-07-23", "to_date": "2025-07-23"}
            var response = new ApiResult<TimesheetTabWorkingHoursResponse>
            {
                Data = new TimesheetTabWorkingHoursResponse()
                {
                    GIOCONG_THUCTE = new List<List<double>>(),
                    GIOCONG_TIEUCHUAN = new List<List<double>>(),
                    Info = new TimesheetTabWorkingHours_Info()
                    {
                        data2 = "Giờ công thực tế",
                        data1 = "Giờ công tiêu chuẩn",
                    }
                },
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Get company info from JWT token
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                DateTime dateFrom = DateTime.Now.GetBeginOfDay();
                DateTime dateTo = DateTime.Now.EndOfDate();
                if (string.IsNullOrEmpty(from_date) == false)
                {
                    dateFrom = DateTime.ParseExact(
                        from_date,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture
                    );

                }
                else
                {
                    // đầu tháng
                    dateFrom = dateFrom.FirstDayOfMonth();
                }
                if (string.IsNullOrEmpty(to_date) == false)
                {
                    dateTo = DateTime.ParseExact(
                        to_date,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture
                    );
                }
                else
                {
                    // cuối tháng
                    dateTo = dateFrom.FirstDayOfMonth();
                }
                response = BoFactory.Report.TimesheetTabWorkingHours(companyId, dateFrom.GetBeginOfDay(), dateTo.EndOfDate());
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("TimesheetTabWorkingHours Exception CompanyId: {0}, EX: {1}",
                    JwtHelper.GetCompanyIdFromToken(Request), ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }

        [ApiAuthorize(UserRole.SystemAdmin)]
        [HttpGet]
        [Route("timesheet-tab-working-day")]
        public IHttpActionResult TimesheetTabWorkingDay(string from_date, string to_date)
        {
            //{"from_date": "2025-07-23", "to_date": "2025-07-23"}
            var response = new ApiResult<TimesheetTabWorkingDayResponse>
            {
                Data = new TimesheetTabWorkingDayResponse()
                {
                    NGAYCONG_THUCTE = new List<List<decimal>>(),
                    NGAYCONG_TIEUCHUAN = new List<List<decimal>>(),
                    Info = new TimesheetTabWorkingDay_Info()
                    {
                        data1 = "Ngày công tiêu chuẩn",
                        data2 = "Ngày công thực tế",
                    }
                },
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                // Get company info from JWT token
                var companyId = JwtHelper.GetCompanyIdFromToken(Request);
                DateTime dateFrom = DateTime.Now.GetBeginOfDay();
                DateTime dateTo = DateTime.Now.EndOfDate();
                if (string.IsNullOrEmpty(from_date) == false)
                {
                    dateFrom = DateTime.ParseExact(
                        from_date,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture
                    );

                }
                else
                {
                    // đầu tháng
                    dateFrom = dateFrom.FirstDayOfMonth();
                }
                if (string.IsNullOrEmpty(to_date) == false)
                {
                    dateTo = DateTime.ParseExact(
                        to_date,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture
                    );
                }
                else
                {
                    // cuối tháng
                    dateTo = dateFrom.FirstDayOfMonth();
                }
                response = BoFactory.Report.TimesheetTabWorkingDate(companyId, dateFrom.GetBeginOfDay(), dateTo.EndOfDate());
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("TimesheetTabWorkingHours Exception CompanyId: {0}, EX: {1}",
                    JwtHelper.GetCompanyIdFromToken(Request), ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = ResponseResultEnum.SystemError.Text();
                return Content(HttpStatusCode.OK, response);
            }
        }
    }
} 