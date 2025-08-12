using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Report;
using DataAccess;
using EntitiesObject.Entities.TanTamEntities;
using Logger;
using MyUtility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using MyUtility;
using BussinessObject.Helper;

namespace BussinessObject.Bo.TanTamBo
{
    public class ReportBo : BaseBo<DBNull>
    {
        public ReportBo() : base(DaoFactory.Report)
        {
        }


        /// <summary>
        /// Get employees growth statistics for dashboard
        /// </summary>
        public ApiResult<DashboardEmployeesGrowthResponse> GetDashboardEmployeesGrowthAsync(DashboardEmployeesGrowthRequest request, int employeeId, int role)
        {
            var response = new ApiResult<DashboardEmployeesGrowthResponse>
            {
                Data = new DashboardEmployeesGrowthResponse(),
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

                if (request.DaysAgo < 0)
                {
                    request.DaysAgo = 30; // Default to 30 days
                }

                var myEmployeeData = DaoFactory.Employee.GetEmployeeObjectData(employeeId);
                if (myEmployeeData == null)
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không tìm thấy thông tin nhân viên.";
                    return response;
                }

                int? regionId = null;
                int? branchId = null;

                if (role == UserRole.RegionalManager.Value())
                {
                    regionId = myEmployeeData.RegionObjId;
                }

                if (role == UserRole.BranchManager.Value())
                {
                    branchId = myEmployeeData.BranchObjId;
                }

                // Call stored procedure to get growth statistics
                var resultFromDb = DaoFactory.Report.GetEmployeesGrowth(request.CompanyId, request.DaysAgo, regionId, branchId);

                if (resultFromDb != null)
                {
                    // Map from DB result (PascalCase) to response DTO (snake_case for API)
                    response.Data = new DashboardEmployeesGrowthResponse
                    {
                        current_employees_count = resultFromDb.CurrentEmployeesCount,
                        old_employees_count = resultFromDb.OldEmployeesCount,
                        diff_employees_count = resultFromDb.DiffEmployeesCount,
                        diff_employees_percent = resultFromDb.DiffEmployeesPercent
                    };

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Lấy thống kê tăng trưởng nhân viên thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không có dữ liệu thống kê";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ReportBo.GetDashboardEmployeesGrowthAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get working status statistics for dashboard reporting
        /// </summary>
        public ApiResult<WorkingTotalResponse> GetWorkingTotalAsync(WorkingTotalRequest request, int companyId, int employeeId, int role)
        {
            var response = new ApiResult<WorkingTotalResponse>()
            {
                Data = new WorkingTotalResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            // Always use current date
            DateTime workingDay = DateTime.Now.Date;

            try
            {
                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID công ty không hợp lệ.";
                    return response;
                }

                if (employeeId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID nhân viên không hợp lệ.";
                    return response;
                }

                if (!System.Enum.IsDefined(typeof(UserRole), role))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vai trò không hợp lệ.";
                    return response;
                }

                var myEmployeeData = DaoFactory.Employee.GetEmployeeObjectData(employeeId);
                if (myEmployeeData == null)
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không tìm thấy thông tin nhân viên.";
                    return response;
                }

                int? regionId = null;
                int? branchId = null;

                if (role == UserRole.RegionalManager.Value())
                {
                    regionId = myEmployeeData.RegionObjId;
                }
                else if (role == UserRole.BranchManager.Value())
                {
                    branchId = myEmployeeData.BranchObjId;
                }

                var result = DaoFactory.Shift.GetWorkingTotal(companyId, workingDay, regionId, branchId);

                if (result != null)
                {
                    response.Data = new WorkingTotalResponse
                    {
                        on_working = result.on_working,
                        late_working = result.late_working,
                        not_working = result.not_working,
                        onleave = result.onleave,
                        on_time = result.on_time,
                        share_location = result.share_location
                    };

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Lấy thống kê trạng thái làm việc thành công.";
                }
                else
                {
                    response.Data = new WorkingTotalResponse
                    {
                        on_working = 0,
                        late_working = 0,
                        not_working = 0,
                        onleave = 0,
                        on_time = 0,
                        share_location = 0
                    };

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Không có dữ liệu chấm công cho ngày được chọn.";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat(
                    "ReportBo.GetWorkingTotal - Unexpected error. CompanyId: {0}, EmployeeId: {1}, Role: {2}, WorkingDay: {3}, Ex: {4}",
                    companyId, employeeId, role, workingDay.ToString("yyyy-MM-dd"), ex);
                
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý.";
            }

            return response;
        }

        /// <summary>
        /// Get detailed working status list for dashboard reporting with pagination
        /// </summary>
        public ApiResult<WorkingDetailResponse> GetWorkingDetailAsync(WorkingTotalRequest request, int companyId, int employeeId, int role)
        {
            var response = new ApiResult<WorkingDetailResponse>()
            {
                Data = new WorkingDetailResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            // Always use current date
            DateTime workingDay = DateTime.Now.Date;

            try
            {
                // Validate company ID
                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "ID công ty không hợp lệ.";
                    return response;
                }

                // Validate required parameters for detail mode
                if (!request.page.HasValue || string.IsNullOrEmpty(request.type))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Vui lòng cung cấp tham số page và type cho chế độ chi tiết.";
                    return response;
                }

                // Validate type parameter
                var validTypes = new[] { "on_working", "on_time", "late_working", "not_working", "onleave" };
                if (!validTypes.Contains(request.type))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Loại trạng thái không hợp lệ. Vui lòng sử dụng: " + string.Join(", ", validTypes);
                    return response;
                }

                // Set defaults
                int page = request.page.Value;
                int perPage = 15; // Fixed 15 items per page

                if (page < 1) page = 1;


                var myEmployeeData = DaoFactory.Employee.GetEmployeeObjectData(employeeId);
                if (myEmployeeData == null)
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không tìm thấy thông tin nhân viên.";
                    return response;
                }

                int? regionId = null;
                int? branchId = null;

                if (role == UserRole.RegionalManager.Value())
                {
                    regionId = myEmployeeData.RegionObjId;
                }
                else if (role == UserRole.BranchManager.Value())
                {
                    branchId = myEmployeeData.BranchObjId;
                }

                var allResults = DaoFactory.Shift.GetWorkingDetail(companyId, workingDay, regionId, branchId);

                if (allResults != null && allResults.Any())
                {
                    // Filter data in C# based on type
                    var filteredResults = FilterWorkingDataByType(allResults, request.type);

                    // Handle pagination in C#
                    int totalCount = filteredResults.Count;
                    int totalPages = totalCount > 0 ? (int)Math.Ceiling((double)totalCount / perPage) : 0;

                    var pagedResults = filteredResults
                        .Skip((page - 1) * perPage)
                        .Take(perPage)
                        .ToList();

                    // Map results to response model
                    response.Data.items = pagedResults.Select(r => new WorkingDetailItem
                    {
                        id = r.payroll_user_id.ToString(),
                        name = r.employee_name ?? "",
                        checkin_time = r.CheckinTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                        checkout_time = r.CheckoutTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                        employee = new WorkingDetailEmployee
                        {
                            id = r.employee_id.ToString(),
                            name = r.employee_name ?? ""
                        },
                        shift = new WorkingDetailShift
                        {
                            id = r.shift_id.ToString(),
                            name = r.shift_name ?? ""
                        },
                        start_time = r.StartTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                        end_time = r.EndTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                        request = new WorkingDetailRequest(), // Empty for now
                        workingday_config = new { }, // Empty object
                        branch_obj = new WorkingDetailBranch
                        {
                            id = r.branch_id.ToString(),
                            name = r.branch_name ?? ""
                        }
                    }).ToList();

                    // Set pagination meta
                    response.Data.meta = new WorkingDetailMeta
                    {
                        total = totalCount,
                        count = pagedResults.Count,
                        per_page = perPage,
                        current_page = page,
                        total_pages = totalPages
                    };

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Lấy danh sách chi tiết trạng thái làm việc thành công.";
                }
                else
                {
                    // No data but still success
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Không có dữ liệu cho loại trạng thái được chọn.";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("ReportBo.GetWorkingDetailAsync - Unexpected error. CompanyId: {0}, Type: {1}, Page: {2}, Ex: {3}",
                    companyId, request?.type, request?.page, ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý.";
            }

            return response;
        }

        /// <summary>
        /// Filter working data by type in C# - simple business logic
        /// </summary>
        private List<Ins_Report_GetWorkingDetail_V2_Result> FilterWorkingDataByType(List<Ins_Report_GetWorkingDetail_V2_Result> allData, string type)
        {
            return allData.Where(r =>
            {
                switch (type)
                {
                    case "on_working":
                        // Đã check in nhưng chưa check out
                        return r.CheckinTime != null && r.CheckoutTime == null && r.payroll_status == 1;

                    case "on_time":
                        // Đã check in và check out đúng thời gian
                        if (r.CheckinTime == null || r.CheckoutTime == null || r.payroll_status != 1)
                            return false;

                        var checkinTime = (DateTime)r.CheckinTime;
                        var checkoutTime = (DateTime)r.CheckoutTime;
                        var startTime = (DateTime)r.StartTime;
                        var endTime = (DateTime)r.EndTime;
                        var latelyCheckIn = r.LatelyCheckIn;
                        var earlyCheckOut = r.EarlyCheckOut;

                        var allowedCheckinTime = startTime.AddMinutes(latelyCheckIn);
                        var allowedCheckoutTime = endTime.AddMinutes(-earlyCheckOut);

                        return checkinTime <= allowedCheckinTime && checkoutTime >= allowedCheckoutTime;

                    case "late_working":
                        // Check in/out không đúng thời gian (trễ hoặc sớm)
                        if (r.payroll_status != 1 || (r.CheckinTime == null && r.CheckoutTime == null))
                            return false;

                        var isLateCheckin = false;
                        var isEarlyCheckout = false;

                        if (r.CheckinTime != null)
                        {
                            var checkin = (DateTime)r.CheckinTime;
                            var startAllowed = ((DateTime)r.StartTime).AddMinutes(r.LatelyCheckIn);
                            isLateCheckin = checkin > startAllowed;
                        }

                        if (r.CheckoutTime != null)
                        {
                            var checkout = (DateTime)r.CheckoutTime;
                            var endAllowed = ((DateTime)r.EndTime).AddMinutes(-(r.EarlyCheckOut));
                            isEarlyCheckout = checkout < endAllowed;
                        }

                        return isLateCheckin || isEarlyCheckout;

                    case "not_working":
                        // Ca đã đăng ký nhưng chưa check in/out
                        return r.CheckinTime == null && r.CheckoutTime == null && r.payroll_status == 1;

                    case "onleave":
                        // Ca bị từ chối hoặc không active
                        return r.payroll_status != 1;

                    default:
                        return false;
                }
            }).ToList();
        }

        /// <summary>
        /// Get working time statistics for dashboard
        /// </summary>
        public ApiResult<DashboardWorkingTimeResponse> GetDashboardWorkingTimeAsync(DashboardWorkingTimeRequest request, int employeeId, int role)
        {
            var response = new ApiResult<DashboardWorkingTimeResponse>
            {
                Data = new DashboardWorkingTimeResponse(),
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

                // Normalize days_ago to supported values (1, 3, 7, 30, 90)
                var originalDaysAgo = request.DaysAgo;
                request.IsValidPeriod();

                // Log if value was normalized
                if (originalDaysAgo != request.DaysAgo)
                {
                    CommonLogger.DefaultLogger.InfoFormat(
                        "DashboardWorkingTime: Normalized days_ago from {0} to {1} for company {2}",
                        originalDaysAgo, request.DaysAgo, request.CompanyId);
                }

                var myEmployeeData = DaoFactory.Employee.GetEmployeeObjectData(employeeId);
                if (myEmployeeData == null)
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không tìm thấy thông tin nhân viên.";
                    return response;
                }

                int? regionId = null;
                int? branchId = null;

                if (role == UserRole.RegionalManager.Value())
                {
                    regionId = myEmployeeData.RegionObjId;
                }
                else if (role == UserRole.BranchManager.Value())
                {
                    branchId = myEmployeeData.BranchObjId;
                }

                // Call stored procedure to get working time statistics
                var resultFromDb = DaoFactory.Report.GetWorkingTimeStatistics(request.CompanyId, request.DaysAgo, regionId, branchId);

                if (resultFromDb != null)
                {
                    // Map from DB result to response DTO (snake_case for API)
                    response.Data = new DashboardWorkingTimeResponse
                    {
                        no_timekeeping = resultFromDb.no_timekeeping,
                        no_clock_in_or_out = resultFromDb.no_clock_in_or_out,
                        good_timekeeping = resultFromDb.good_timekeeping,
                        in_late_out_soon = resultFromDb.in_late_out_soon
                    };

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = string.Format("Lấy thống kê thời gian làm việc {0} ngày gần đây thành công.", request.DaysAgo);
                }
                else
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = string.Format("Không có dữ liệu thống kê thời gian làm việc trong {0} ngày gần đây.", request.DaysAgo);
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ReportBo.GetDashboardWorkingTimeAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get dashboard devices statistics
        /// Returns count and percentage of employees who have checked in or out
        /// </summary>
        public ApiResult<DashboardDevicesResponse> GetDashboardDevicesAsync(DashboardDevicesRequest request)
        {
            var response = new ApiResult<DashboardDevicesResponse>
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

                // Default to current date if not provided
                if (!request.WorkingDay.HasValue)
                {
                    request.WorkingDay = DateTime.Now.Date;
                }

                // Call stored procedure to get dashboard devices statistics
                var resultFromDb = DaoFactory.Report.GetDashboardDevices(request.CompanyId, request.WorkingDay);

                if (resultFromDb != null)
                {
                    // Map from DB result to response DTO (following requested format)
                    response.Data.admin.count = resultFromDb.count;
                    response.Data.admin.percent = resultFromDb.percent;

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Lấy thống kê thiết bị chấm công thành công.";
                }
                else
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không có dữ liệu thống kê thiết bị chấm công.";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ReportBo.GetDashboardDevicesAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        #region GetEmployeeReport
        public ApiResult<EmployeePayrollReportResponse> GetEmployeeReport(int accountMapID, int companyId, DateTime dateFrom, DateTime dateto)
        {
            var response = new ApiResult<EmployeePayrollReportResponse>()
            {
                Data = new EmployeePayrollReportResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                //var user_Timekeeper_log = DaoFactory.Timekeeper.GetListByAccountMapID_Simple(accountMapID, dateFrom, dateto);
                var user_Payroll = DaoFactory.Payroll.GetListByAccountMapID(accountMapID, dateFrom, dateto).Where(x => x.PayrollStatus == 1).ToList();
                //var user_ChamCongHo = DaoFactory.ShiftAssignment.LogChamCongHo_GetDateToDate(accountMapID, dateFrom, dateto).Where(x => x.IsTrashed == false).ToList();
                var company_Shift = DaoFactory.Shift.Shift_GetSimple(companyId, -1);
                var dataHour = DaoFactory.Shift.GetTimes("vn");
                // này để dành lưu khỏi gọi lại
                List<Ins_Shift_TimePenaltyRule_SelectByShiftId_Result> timePenaltyRule = new List<Ins_Shift_TimePenaltyRule_SelectByShiftId_Result>();
                List<Ins_Shift_TimePenaltyRule_SelectByShiftId_Result> shift_TimePenaltyRule = new List<Ins_Shift_TimePenaltyRule_SelectByShiftId_Result>();

                List<Ins_ShiftTimeInOutConfig_GetByShiftId_Result> timeInOutConfig = new List<Ins_ShiftTimeInOutConfig_GetByShiftId_Result>();
                Ins_ShiftTimeInOutConfig_GetByShiftId_Result shift_TimeInOutConfig = new Ins_ShiftTimeInOutConfig_GetByShiftId_Result();

                EmployeePayrollReportInfo infor = new EmployeePayrollReportInfo();
                #region WorkDay
                infor.WorkDay = new WorkDay();
                WorkDay infor_WorkDay = new WorkDay();

                #region NGAYCONG_THUCTE
                decimal NGAYCONG_THUCTE = 0;
                foreach (var item in user_Payroll)
                {
                    // rule đã lấy trươc đó thì khỏi gọi lấy lại
                    if (timePenaltyRule.Any(x => x.ShiftID == item.ShiftId))
                    {
                        shift_TimePenaltyRule = timePenaltyRule.Where(x => x.ShiftID == item.ShiftId).ToList();
                    }
                    else // chưa lấy thì gọi db 
                    {
                        shift_TimePenaltyRule = DaoFactory.Shift.Shift_TimePenaltyRule_SelectByShiftId(item.ShiftId);
                        // add rule vô log để dành xài
                        if (shift_TimePenaltyRule != null && shift_TimePenaltyRule.Any())
                        {
                            timePenaltyRule.AddRange(shift_TimePenaltyRule);
                        }
                    }

                    if (item.CheckinTime == null || item.CheckoutTime == null)
                    {
                        continue;
                    }
                    NGAYCONG_THUCTE = NGAYCONG_THUCTE +
                                          PayrollHelper.CalculateTotalPenalty(
                                          item.StartTime.GetValueOrDefault(),
                                          item.EndTime.GetValueOrDefault(),
                                          item.CheckinTime.GetValueOrDefault(),
                                          item.CheckoutTime.GetValueOrDefault(),
                                          shift_TimePenaltyRule,
                                          company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId) != null ? company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId).LatelyCheckIn : 0,
                                          company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId) != null ? company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId).EarlyCheckOut : 0
                                          );
                }

                infor_WorkDay.NGAYCONG_THUCTE = new Item()
                {
                    Name = "Ngày công thực tế",
                    Value = string.Format("{0} Công", NGAYCONG_THUCTE.FormatCoinCultureTanTam()),
                    IsDetail = 1
                };
                #endregion NGAYCONG_THUCTE

                #region GIOCONG_THUCTE
                double GIOCONG_THUCTE = 0;
                foreach (var item in user_Payroll)
                {
                    // rule đã lấy trươc đó thì khỏi gọi lấy lại
                    if (timeInOutConfig.Any(x => x.ShiftID == item.ShiftId))
                    {
                        shift_TimeInOutConfig = timeInOutConfig.Where(x => x.ShiftID == item.ShiftId).FirstOrDefault();
                    }
                    else // chưa lấy thì gọi db 
                    {
                        shift_TimeInOutConfig = DaoFactory.Shift.GetShiftTimeConfig(item.ShiftId).FirstOrDefault();
                        if (shift_TimeInOutConfig != null && shift_TimeInOutConfig.ShiftID != 0)
                        {
                            timeInOutConfig.Add(shift_TimeInOutConfig);
                        }
                    }

                    if (item.CheckinTime == null && item.CheckoutTime == null)
                    {
                        continue;
                    }
                    //chưa cấu hình giờ nghỉ trưa  tính mặc định 12:00 - 13:30
                    if (shift_TimeInOutConfig == null
                            || (shift_TimeInOutConfig.RestStartHourId ?? 0) == 0
                            || (shift_TimeInOutConfig.RestStartMinuteId ?? 0) == 0
                            || (shift_TimeInOutConfig.RestEndHourId ?? 0) == 0
                            || (shift_TimeInOutConfig.RestEndMinuteId ?? 0) == 0
                       )
                    {
                        GIOCONG_THUCTE = GIOCONG_THUCTE + PayrollHelper.CalculateWorkHours(
                                item.CheckinTime.GetValueOrDefault(),
                                item.CheckoutTime.GetValueOrDefault(),
                                item.StartTime.GetValueOrDefault(),
                                item.EndTime.GetValueOrDefault(),
                                new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day, 12, 0, 0),
                                new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day, 13, 30, 0)
                            );
                    }
                    else
                    {
                        GIOCONG_THUCTE = GIOCONG_THUCTE + PayrollHelper.CalculateWorkHours(
                                item.CheckinTime.GetValueOrDefault(),
                                item.CheckoutTime.GetValueOrDefault(),
                                item.StartTime.GetValueOrDefault(),
                                item.EndTime.GetValueOrDefault(),
                                new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day,
                                dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestStartHourId ?? 0) && z.IsHour == 1).Value ?? 0,
                                dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestStartMinuteId ?? 0) && z.IsHour == 0).Value ?? 0,
                                0
                                ),
                                new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day,
                                dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestEndHourId ?? 0) && z.IsHour == 1).Value ?? 0,
                                dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestEndMinuteId ?? 0) && z.IsHour == 0).Value ?? 0,
                                0
                                )
                            );
                    }
                }

                infor_WorkDay.GIOCONG_THUCTE = new Item()
                {
                    Name = "Giờ công thực tế",
                    Value = string.Format("{0} Giờ {1} Phút", (int)GIOCONG_THUCTE, GIOCONG_THUCTE > 0 ? (int)Math.Round((GIOCONG_THUCTE - (int)GIOCONG_THUCTE) * 60) : 0),
                    IsDetail = 1
                };

                #endregion GIOCONG_THUCTE

                infor_WorkDay.SOGIO_LAMTHEM = new Item()
                {
                    Name = "Số giờ làm thêm",
                    Value = string.Format("{0} giờ {1} phút", 0, 0)
                };

                #region SOPHUT_DILAMSOM
                int SOPHUT_DILAMSOM = 0;
                foreach (var item in user_Payroll)
                {
                    if (item.CheckinTime != null)
                    {
                        SOPHUT_DILAMSOM = SOPHUT_DILAMSOM + PayrollHelper.CalculateEarlyCheckInMinutes(item.CheckinTime.GetValueOrDefault(), item.StartTime.GetValueOrDefault());
                    }
                }
                infor_WorkDay.SOPHUT_DILAMSOM = new Item()
                {
                    Name = "Số phút đi làm sớm",
                    Value = string.Format("{0} phút", SOPHUT_DILAMSOM)
                };
                #endregion

                #region GIOCONG_TIEUCHUAN
                double GIOCONG_TIEUCHUAN = 0;
                foreach (var item in user_Payroll)
                {
                    // rule đã lấy trươc đó thì khỏi gọi lấy lại
                    shift_TimeInOutConfig = timeInOutConfig.Where(x => x.ShiftID == item.ShiftId).FirstOrDefault();
                    //chưa cấu hình giờ nghỉ trưa  tính mặc định 12:00 - 13:30
                    if (shift_TimeInOutConfig == null
                            || (shift_TimeInOutConfig.RestStartHourId ?? 0) == 0
                            || (shift_TimeInOutConfig.RestStartMinuteId ?? 0) == 0
                            || (shift_TimeInOutConfig.RestEndHourId ?? 0) == 0
                            || (shift_TimeInOutConfig.RestEndMinuteId ?? 0) == 0
                       )
                    {
                        GIOCONG_TIEUCHUAN = GIOCONG_TIEUCHUAN + PayrollHelper.CalculateWorkHours(
                                item.StartTime.GetValueOrDefault(),
                                item.EndTime.GetValueOrDefault(),
                                item.StartTime.GetValueOrDefault(),
                                item.EndTime.GetValueOrDefault(),
                                new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day, 12, 0, 0),
                                new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day, 13, 30, 0)
                            );
                    }
                    else
                    {
                        GIOCONG_TIEUCHUAN = GIOCONG_TIEUCHUAN + PayrollHelper.CalculateWorkHours(
                                item.StartTime.GetValueOrDefault(),
                                item.EndTime.GetValueOrDefault(),
                                item.StartTime.GetValueOrDefault(),
                                item.EndTime.GetValueOrDefault(),
                                new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day,
                                dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestStartHourId ?? 0) && z.IsHour == 1).Value ?? 0,
                                dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestStartMinuteId ?? 0) && z.IsHour == 0).Value ?? 0,
                                0
                                ),
                                new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day,
                                dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestEndHourId ?? 0) && z.IsHour == 1).Value ?? 0,
                                dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestEndMinuteId ?? 0) && z.IsHour == 0).Value ?? 0,
                                0
                                )
                            );
                    }

                    timeInOutConfig.Add(shift_TimeInOutConfig);

                }
                infor_WorkDay.GIOCONG_TIEUCHUAN = new Item()
                {
                    Name = "Giờ công tiêu chuẩn",
                    Value = string.Format("{0} Giờ {1} Phút", (int)GIOCONG_TIEUCHUAN, GIOCONG_TIEUCHUAN > 0 ? (int)Math.Round((GIOCONG_TIEUCHUAN - (int)GIOCONG_TIEUCHUAN) * 60) : 0),
                };
                #endregion

                infor_WorkDay.SONGAYNGHI_TIEUCHUAN = new Item()
                {
                    Name = "Số ngày nghỉ tiêu chuẩn",
                    Value = string.Format("{0} ngày", 0)
                };

                decimal SONGAYNGHI_KHONGLUONG = 0;
                infor_WorkDay.SONGAYNGHI_KHONGLUONG = new Item()
                {
                    Name = "Số ngày nghỉ không lương (chính thức)",
                    Value = string.Format("{0} ngày", 0)
                };

                #region CONG_CHUAN
                decimal CONG_CHUAN = 0;
                foreach (var item in user_Payroll)
                {
                    CONG_CHUAN = CONG_CHUAN +
                                          PayrollHelper.CalculateTotalPenalty(
                                          item.StartTime.GetValueOrDefault(),
                                          item.EndTime.GetValueOrDefault(),
                                          item.StartTime.GetValueOrDefault(),
                                          item.EndTime.GetValueOrDefault(),
                                          shift_TimePenaltyRule,
                                          company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId) != null ? company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId).LatelyCheckIn : 0,
                                          company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId) != null ? company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId).EarlyCheckOut : 0
                                          );
                }
                infor_WorkDay.CONG_CHUAN = new Item()
                {
                    Name = "Công chuẩn",
                    Value = string.Format("{0} ngày", CONG_CHUAN)
                };
                #endregion

                infor_WorkDay.SONGAYNGHI_LE = new Item()
                {
                    Name = "Số ngày công nghỉ lễ",
                    Value = string.Format("{0} ngày", 0)
                };

                decimal PayableCoefficient = NGAYCONG_THUCTE - SONGAYNGHI_KHONGLUONG;
                infor_WorkDay.PayableCoefficient = new Item()
                {
                    Name = "Tổng công tính lương",
                    Value = string.Format("{0} ngày", PayableCoefficient.FormatCoinCultureTanTam())
                };
                infor.WorkDay = infor_WorkDay;
                #endregion 

                #region WorkHour
                infor.WorkHour = new WorkHour();
                WorkHour infor_WorkHour = new WorkHour();

                #region SOGIO_VESOM 
                int SOGIO_VESOM = 0;
                foreach (var item in user_Payroll)
                {
                    // chỉ tính giờ out
                    if (item.CheckoutTime != null)
                    {
                        SOGIO_VESOM = SOGIO_VESOM + PayrollHelper.CalculateEarlyLeaveMinutes(item.CheckoutTime.GetValueOrDefault(), item.EndTime.GetValueOrDefault());
                    }
                }
                infor_WorkHour.SOGIO_VESOM = new Item()
                {
                    Name = "Số giờ về sớm",
                    Value = string.Format("{0} Giờ {1} Phút", SOGIO_VESOM > 0 ? (int)(SOGIO_VESOM / 60) : 0, SOGIO_VESOM > 0 ? SOGIO_VESOM % 60 : 0),
                    IsDetail = 1
                };
                #endregion

                #region  SOGIO_DIMUON
                int SOGIO_DIMUON = 0;
                foreach (var item in user_Payroll)
                {
                    if (item.CheckinTime != null)
                    {
                        SOGIO_DIMUON = SOGIO_DIMUON + PayrollHelper.CalculateLateMinutes(item.CheckinTime.GetValueOrDefault(), item.StartTime.GetValueOrDefault());
                    }
                }
                infor_WorkHour.SOGIO_DIMUON = new Item()
                {
                    Name = "Số giờ đi muộn",
                    Value = string.Format("{0} Giờ {1} Phút", SOGIO_DIMUON > 0 ? (int)(SOGIO_DIMUON / 60) : 0, SOGIO_DIMUON > 0 ? SOGIO_DIMUON % 60 : 0),
                    IsDetail = 1
                };
                #endregion

                #region SOGIO_DIMUON_VESOM
                int SOGIO_DIMUON_VESOM = SOGIO_VESOM + SOGIO_DIMUON;
                infor_WorkHour.SOGIO_DIMUON_VESOM = new Item()
                {
                    Name = "Số giờ đi muộn, về sớm",
                    Value = string.Format("{0} Giờ {1} Phút", SOGIO_DIMUON_VESOM > 0 ? (int)(SOGIO_DIMUON_VESOM / 60) : 0, SOGIO_DIMUON_VESOM > 0 ? SOGIO_DIMUON_VESOM % 60 : 0),
                };
                infor.WorkHour = infor_WorkHour;
                #endregion

                #endregion
                #region OtherField               
                infor.OtherField = new OtherField();
                OtherField infor_OtherField = new OtherField();

                #region SOLAN_QUENCHECKIN
                int SOLAN_QUENCHECKIN = 0;
                foreach (var item in user_Payroll)
                {
                    if (item.CheckinTime == null && item.CheckoutTime != null)
                    {
                        SOLAN_QUENCHECKIN++;
                        continue;
                    }
                }
                infor_OtherField.SOLAN_QUENCHECKIN = new Item()
                {
                    Name = "Số lần quên checkin",
                    Value = SOLAN_QUENCHECKIN,
                    IsDetail = 1
                };
                #endregion

                #region SOLAN_QUENCHECKOUT
                int SOLAN_QUENCHECKOUT = 0;
                foreach (var item in user_Payroll)
                {
                    if (item.CheckinTime != null && item.CheckoutTime == null)
                    {
                        SOLAN_QUENCHECKOUT++;
                        continue;
                    }
                }
                infor_OtherField.SOLAN_QUENCHECKOUT = new Item()
                {
                    Name = "Số lần quên checkout",
                    Value = SOLAN_QUENCHECKOUT,
                    IsDetail = 1
                };
                #endregion

                #region SOLAN_QUENCHECKINOUT   
                int SOLAN_QUENCHECKINOUT = 0;
                foreach (var item in user_Payroll)
                {
                    if (item.CheckinTime == null && item.CheckoutTime == null)
                    {
                        SOLAN_QUENCHECKINOUT++;
                        continue;
                    }
                }
                infor_OtherField.SOLAN_QUENCHECKINOUT = new Item()
                {
                    Name = "Số lần quên checkin và checkout",
                    Value = SOLAN_QUENCHECKINOUT,
                    IsDetail = 1
                };
                #endregion

                infor.OtherField = infor_OtherField;
                #endregion
                response.Data.Info = infor;
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ReportBo.GetDashboardDevicesAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }
        public ApiResult<List<EmployeePayrollReportDetailResponse>> GetEmployeeReport_NGAYCONG_THUCTE(int accountMapID, int companyId, DateTime dateFrom, DateTime dateto)
        {
            var response = new ApiResult<List<EmployeePayrollReportDetailResponse>>()
            {
                Data = new List<EmployeePayrollReportDetailResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {               
                var user_Payroll = DaoFactory.Payroll.GetListByAccountMapID(accountMapID, dateFrom, dateto).Where(x => x.PayrollStatus == 1).ToList();
                var dataHour = DaoFactory.Shift.GetTimes("vn");
                var shifts = DaoFactory.Shift.Shift_GetSimple(companyId, -1);
                var company_Shift = DaoFactory.Shift.Shift_GetSimple(companyId, -1);
                // này để dành lưu khỏi gọi lại
                List<Ins_Shift_TimePenaltyRule_SelectByShiftId_Result> timePenaltyRule = new List<Ins_Shift_TimePenaltyRule_SelectByShiftId_Result>();
                List<Ins_Shift_TimePenaltyRule_SelectByShiftId_Result> shift_TimePenaltyRule = new List<Ins_Shift_TimePenaltyRule_SelectByShiftId_Result>();

                List<Ins_ShiftTimeInOutConfig_GetByShiftId_Result> timeInOutConfig = new List<Ins_ShiftTimeInOutConfig_GetByShiftId_Result>();
                Ins_ShiftTimeInOutConfig_GetByShiftId_Result shift_TimeInOutConfig = new Ins_ShiftTimeInOutConfig_GetByShiftId_Result();

                for (DateTime loopDate = dateFrom; loopDate <= dateto; loopDate = loopDate.AddDays(1))
                {
                    var day_item = new EmployeePayrollReportDetailResponse();
                    day_item.Title = loopDate.ToString("yyyy-MM-dd HH:mm:ss");
                    day_item.Data = new List<PayrollReportDetail_ShiftData>() { };
                    foreach (var item in user_Payroll.Where(x => x.WorkingDay.GetValueOrDefault().Date == loopDate.Date))
                    {
                        var Ngay_Cong = new PayrollReportDetail_ShiftData();
                        Ngay_Cong.Name = shifts.FirstOrDefault(x => x.ShiftId == item.ShiftId)?.ShiftName;
                        Ngay_Cong.ShiftId = item.PayrollUserID;
                        Ngay_Cong.Timezone = shifts.FirstOrDefault(x => x.ShiftId == item.ShiftId)?.Timezone;
                        Ngay_Cong.EmployeeShiftId = item.AssignmentUserID;

                        // rule đã lấy trươc đó thì khỏi gọi lấy lại
                        if (timePenaltyRule.Any(x => x.ShiftID == item.ShiftId))
                        {
                            shift_TimePenaltyRule = timePenaltyRule.Where(x => x.ShiftID == item.ShiftId).ToList();
                        }
                        else // chưa lấy thì gọi db 
                        {
                            shift_TimePenaltyRule = DaoFactory.Shift.Shift_TimePenaltyRule_SelectByShiftId(item.ShiftId);
                            // add rule vô log để dành xài
                            if (shift_TimePenaltyRule != null)
                            {
                                timePenaltyRule.AddRange(shift_TimePenaltyRule);
                            }
                        }

                        decimal NGAYCONG_THUCTE = 0;                       
                        if (item.CheckinTime == null || item.CheckoutTime == null)
                        {
                            continue;
                        }
                        else if (PayrollHelper.CalculateLateMinutes(item.CheckinTime.GetValueOrDefault(), item.StartTime.GetValueOrDefault()) > 0) //Trễ giờ
                        {

                            Ngay_Cong.Status = new PayrollReportDetail_ShiftStatus()
                            {
                                Color = "#FFCB76",
                                StatusColor = new List<string> { "#FFC888", "#FFF4E7" },
                                Name = "Trễ giờ"
                            };

                            NGAYCONG_THUCTE = NGAYCONG_THUCTE +
                                           PayrollHelper.CalculateTotalPenalty(
                                           item.StartTime.GetValueOrDefault(),
                                           item.EndTime.GetValueOrDefault(),
                                           item.CheckinTime.GetValueOrDefault(),
                                           item.CheckoutTime.GetValueOrDefault(),
                                           shift_TimePenaltyRule,
                                           company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId) != null ? company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId).LatelyCheckIn : 0,
                                           company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId) != null ? company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId).EarlyCheckOut : 0
                                           );

                        }
                        else if (PayrollHelper.CalculateEarlyLeaveMinutes(item.CheckoutTime.GetValueOrDefault(), item.EndTime.GetValueOrDefault()) > 0) //về sớm
                        {
                            Ngay_Cong.Status = new PayrollReportDetail_ShiftStatus()
                            {
                                Color = "#FFCB76",
                                StatusColor = new List<string> { "#FFC888", "#FFF4E7" },
                                Name = "Về sớm"
                            };

                            NGAYCONG_THUCTE = NGAYCONG_THUCTE +
                                           PayrollHelper.CalculateTotalPenalty(
                                           item.StartTime.GetValueOrDefault(),
                                           item.EndTime.GetValueOrDefault(),
                                           item.CheckinTime.GetValueOrDefault(),
                                           item.CheckoutTime.GetValueOrDefault(),
                                           shift_TimePenaltyRule,
                                           company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId) != null ? company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId).LatelyCheckIn : 0,
                                           company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId) != null ? company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId).EarlyCheckOut : 0
                                           );

                        }
                        else  //Đúng giờ
                        {

                            Ngay_Cong.Status = new PayrollReportDetail_ShiftStatus()
                            {
                                Color = "#7ED321",
                                StatusColor = new List<string> { "#1ECC78", "#D2F5E4" },
                                Name = "Đúng giờ"
                            };

                            NGAYCONG_THUCTE = NGAYCONG_THUCTE +
                                           PayrollHelper.CalculateTotalPenalty(
                                           item.StartTime.GetValueOrDefault(),
                                           item.EndTime.GetValueOrDefault(),
                                           item.CheckinTime.GetValueOrDefault(),
                                           item.CheckoutTime.GetValueOrDefault(),
                                           shift_TimePenaltyRule,
                                           company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId) != null ? company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId).LatelyCheckIn : 0,
                                           company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId) != null ? company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId).EarlyCheckOut : 0
                                           );

                        }

                        Ngay_Cong.CheckInTime = item.CheckinTime != null ? item.CheckinTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") : "";
                        Ngay_Cong.CheckOutTime = item.CheckoutTime != null ? item.CheckoutTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") : "";
                        Ngay_Cong.Approved = true;
                        Ngay_Cong.Value = string.Format("{0} công", NGAYCONG_THUCTE.FormatCoinCultureTanTam());
                        if (NGAYCONG_THUCTE <= 0)
                        {
                            continue;
                        }

                        day_item.Data.Add(Ngay_Cong);
                    }
                    if (day_item.Data != null && day_item.Data.Any())
                    {
                        response.Data.Add(day_item);
                    }
                }

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
                return response;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("GetEmployeeReport_NGAYCONG_THUCTE - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }
        public ApiResult<List<EmployeePayrollReportDetailResponse>> GetEmployeeReport_GIOCONG_THUCTE(int accountMapID, int companyId, DateTime dateFrom, DateTime dateto)
        {
            var response = new ApiResult<List<EmployeePayrollReportDetailResponse>>()
            {
                Data = new List<EmployeePayrollReportDetailResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                
                var user_Payroll = DaoFactory.Payroll.GetListByAccountMapID(accountMapID, dateFrom, dateto).Where(x => x.PayrollStatus == 1).ToList();
                var dataHour = DaoFactory.Shift.GetTimes("vn");
                var shifts = DaoFactory.Shift.Shift_GetSimple(companyId, -1);

                // này để dành lưu khỏi gọi lại
                List<Ins_ShiftTimeInOutConfig_GetByShiftId_Result> timeInOutConfig = new List<Ins_ShiftTimeInOutConfig_GetByShiftId_Result>();
                Ins_ShiftTimeInOutConfig_GetByShiftId_Result shift_TimeInOutConfig = new Ins_ShiftTimeInOutConfig_GetByShiftId_Result();

                for (DateTime loopDate = dateFrom; loopDate <= dateto; loopDate = loopDate.AddDays(1))
                {
                    var day_item = new EmployeePayrollReportDetailResponse();
                    day_item.Title = loopDate.ToString("yyyy-MM-dd HH:mm:ss");
                    day_item.Data = new List<PayrollReportDetail_ShiftData>() { };
                    foreach (var item in user_Payroll.Where(x => x.WorkingDay.GetValueOrDefault().Date == loopDate.Date))
                    {
                        var Ngay_Cong = new PayrollReportDetail_ShiftData();
                        Ngay_Cong.Name = shifts.FirstOrDefault(x => x.ShiftId == item.ShiftId)?.ShiftName;
                        Ngay_Cong.ShiftId = item.PayrollUserID;
                        Ngay_Cong.Timezone = shifts.FirstOrDefault(x => x.ShiftId == item.ShiftId)?.Timezone;
                        Ngay_Cong.EmployeeShiftId = item.AssignmentUserID;

                        // rule đã lấy trươc đó thì khỏi gọi lấy lại
                        if (timeInOutConfig.Any(x => x.ShiftID == item.ShiftId))
                        {
                            shift_TimeInOutConfig = timeInOutConfig.Where(x => x.ShiftID == item.ShiftId).FirstOrDefault();
                        }
                        else // chưa lấy thì gọi db 
                        {
                            shift_TimeInOutConfig = DaoFactory.Shift.GetShiftTimeConfig(item.ShiftId).FirstOrDefault();
                            if (shift_TimeInOutConfig != null && shift_TimeInOutConfig.ShiftID != 0)
                            {
                                timeInOutConfig.Add(shift_TimeInOutConfig);
                            }
                        }

                        if (item.CheckinTime == null || item.CheckoutTime == null)
                        {
                            continue;
                        }

                        #region GIOCONG_THUCTE
                        double GIOCONG_THUCTE = 0;
                        //chưa cấu hình giờ nghỉ trưa  tính mặc định 12:00 - 13:30
                        if (shift_TimeInOutConfig == null
                                || (shift_TimeInOutConfig.RestStartHourId ?? 0) == 0
                                || (shift_TimeInOutConfig.RestStartMinuteId ?? 0) == 0
                                || (shift_TimeInOutConfig.RestEndHourId ?? 0) == 0
                                || (shift_TimeInOutConfig.RestEndMinuteId ?? 0) == 0
                           )
                        {
                            GIOCONG_THUCTE = GIOCONG_THUCTE + PayrollHelper.CalculateWorkHours(
                                    item.CheckinTime.GetValueOrDefault(),
                                    item.CheckoutTime.GetValueOrDefault(),
                                    item.StartTime.GetValueOrDefault(),
                                    item.EndTime.GetValueOrDefault(),
                                    new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day, 12, 0, 0),
                                    new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day, 13, 30, 0)
                                );
                        }
                        else
                        {
                            GIOCONG_THUCTE = GIOCONG_THUCTE + PayrollHelper.CalculateWorkHours(
                                    item.CheckinTime.GetValueOrDefault(),
                                    item.CheckoutTime.GetValueOrDefault(),
                                    item.StartTime.GetValueOrDefault(),
                                    item.EndTime.GetValueOrDefault(),
                                    new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day,
                                    dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestStartHourId ?? 0) && z.IsHour == 1).Value ?? 0,
                                    dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestStartMinuteId ?? 0) && z.IsHour == 0).Value ?? 0,
                                    0
                                    ),
                                    new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day,
                                    dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestEndHourId ?? 0) && z.IsHour == 1).Value ?? 0,
                                    dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestEndMinuteId ?? 0) && z.IsHour == 0).Value ?? 0,
                                    0
                                    )
                                );
                        }
                        #endregion

                        if (item.CheckoutTime == null && item.CheckinTime != null) //Only checkin, no checkout
                        {
                            Ngay_Cong.Status = new PayrollReportDetail_ShiftStatus()
                            {
                                Color = "#FF0000",
                                StatusColor = new List<string> { "#FF0E39", "#FFCFD7" },
                                Name = "Quên check out",
                                //Detail = new List<string> { "0 giờ" },
                            };
                        }
                        else if (item.CheckinTime == null && item.CheckoutTime != null) //Only checkout, no checkin
                        {

                            Ngay_Cong.Status = new PayrollReportDetail_ShiftStatus()
                            {
                                Color = "#666666",
                                StatusColor = new List<string> { "#838BA3", "#EBEBEB" },
                                Name = "Quên check in",
                                //Detail = new List<string> { "0 giờ" },
                            };
                        }
                        else if (PayrollHelper.CalculateEarlyCheckInMinutes(item.CheckoutTime.GetValueOrDefault(), item.EndTime.GetValueOrDefault()) > 0) //về sớm
                        {

                            Ngay_Cong.Status = new PayrollReportDetail_ShiftStatus()
                            {
                                Color = "#FFCB76",
                                StatusColor = new List<string> { "#FFC888", "#FFF4E7" },
                                Name = "Về sớm",
                                //Detail = new List<string> { string.Format("{0} giờ", GIOCONG_THUCTE.FormatCoinCultureTanTam()) }
                            };

                        }
                        else if (PayrollHelper.CalculateLateMinutes(item.CheckinTime.GetValueOrDefault(), item.StartTime.GetValueOrDefault()) > 0) //Trễ giờ
                        {

                            Ngay_Cong.Status = new PayrollReportDetail_ShiftStatus()
                            {
                                Color = "#FFCB76",
                                StatusColor = new List<string> { "#FFC888", "#FFF4E7" },
                                Name = "Trễ giờ",
                                //Detail = new List<string> { string.Format("{0} giờ", GIOCONG_THUCTE.FormatCoinCultureTanTam()) }
                            };

                        }
                        else  //Đúng giờ
                        {
                            Ngay_Cong.Status = new PayrollReportDetail_ShiftStatus()
                            {
                                Color = "#7ED321",
                                StatusColor = new List<string> { "#1ECC78", "#D2F5E4" },
                                Name = "Đúng giờ",
                                //Detail = new List<string> { string.Format("{0} giờ", GIOCONG_THUCTE.FormatCoinCultureTanTam()) }
                            };
                        }
                        Ngay_Cong.CheckInTime = item.CheckinTime != null ? item.CheckinTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") : "";
                        Ngay_Cong.CheckOutTime = item.CheckoutTime != null ? item.CheckoutTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") : "";
                        Ngay_Cong.Approved = true;
                        Ngay_Cong.Value = string.Format("{0} Giờ {1} Phút", (int)GIOCONG_THUCTE, GIOCONG_THUCTE > 0 ? (int)Math.Round((GIOCONG_THUCTE - (int)GIOCONG_THUCTE) * 60) : 0);
                        day_item.Data.Add(Ngay_Cong);
                    }
                    if (day_item.Data != null && day_item.Data.Any())
                    {
                        response.Data.Add(day_item);
                    }
                }
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
                return response;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("GetEmployeeReport_GIOCONG_THUCTE - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }
        public ApiResult<List<EmployeePayrollReportDetailResponse>> GetEmployeeReport_SOGIO_VESOM(int accountMapID, int companyId, DateTime dateFrom, DateTime dateto)
        {
            var response = new ApiResult<List<EmployeePayrollReportDetailResponse>>()
            {
                Data = new List<EmployeePayrollReportDetailResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {               
                var user_Payroll = DaoFactory.Payroll.GetListByAccountMapID(accountMapID, dateFrom, dateto).Where(x => x.PayrollStatus == 1).ToList();
                var shifts = DaoFactory.Shift.Shift_GetSimple(companyId, -1);
                for (DateTime loopDate = dateFrom; loopDate <= dateto; loopDate = loopDate.AddDays(1))
                {
                    var day_item = new EmployeePayrollReportDetailResponse();
                    day_item.Title = loopDate.ToString("yyyy-MM-dd HH:mm:ss");
                    day_item.Data = new List<PayrollReportDetail_ShiftData>() { };
                    foreach (var item in user_Payroll.Where(x => x.WorkingDay.GetValueOrDefault().Date == loopDate.Date))
                    {
                        var Ngay_Cong = new PayrollReportDetail_ShiftData();
                        Ngay_Cong.Name = shifts.FirstOrDefault(x => x.ShiftId == item.ShiftId)?.ShiftName;
                        Ngay_Cong.ShiftId = item.PayrollUserID;
                        Ngay_Cong.Timezone = shifts.FirstOrDefault(x => x.ShiftId == item.ShiftId)?.Timezone;
                        Ngay_Cong.EmployeeShiftId = item.AssignmentUserID;                       
                        double SOGIO_VESOM = 0;
                        if (item.CheckoutTime != null) //có check out 
                        {
                            SOGIO_VESOM = PayrollHelper.CalculateEarlyLeaveMinutes(item.CheckoutTime.GetValueOrDefault(), item.EndTime.GetValueOrDefault());
                            if (SOGIO_VESOM > 0) //về sớm
                            {
                                Ngay_Cong.Status = new PayrollReportDetail_ShiftStatus()
                                {
                                    Color = "#FF0000",
                                    StatusColor = new List<string> { "#FF0E39", "#FFCFD7" },
                                    Name = "Về sớm"                                   
                                };
                            }
                            else //0 về sớm
                            {
                                continue; //
                            }
                        }
                        else  //Đúng giờ
                        {
                            continue;
                        }
                        Ngay_Cong.CheckInTime = item.CheckinTime != null ? item.CheckinTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") : "";
                        Ngay_Cong.CheckOutTime = item.CheckoutTime != null ? item.CheckoutTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") : "";
                        Ngay_Cong.Approved = true;
                        Ngay_Cong.Value = string.Format("{0} Giờ {1} Phút", (int)(SOGIO_VESOM / 60), SOGIO_VESOM % 60);
                        day_item.Data.Add(Ngay_Cong);
                    }
                    if (day_item.Data != null && day_item.Data.Any())
                    {
                        response.Data.Add(day_item);
                    }
                }
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
                return response;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("GetEmployeeReport_SOGIO_VESOM - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }
        public ApiResult<List<EmployeePayrollReportDetailResponse>> GetEmployeeReport_SOGIO_DIMUON(int accountMapID, int companyId, DateTime dateFrom, DateTime dateto)
        {
            var response = new ApiResult<List<EmployeePayrollReportDetailResponse>>()
            {
                Data = new List<EmployeePayrollReportDetailResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {               
                var user_Payroll = DaoFactory.Payroll.GetListByAccountMapID(accountMapID, dateFrom, dateto).Where(x => x.PayrollStatus == 1).ToList();               
                var shifts = DaoFactory.Shift.Shift_GetSimple(companyId, -1);
                for (DateTime loopDate = dateFrom; loopDate <= dateto; loopDate = loopDate.AddDays(1))
                {
                    var day_item = new EmployeePayrollReportDetailResponse();
                    day_item.Title = loopDate.ToString("yyyy-MM-dd HH:mm:ss");
                    day_item.Data = new List<PayrollReportDetail_ShiftData>() { };
                    foreach (var item in user_Payroll.Where(x => x.WorkingDay.GetValueOrDefault().Date == loopDate.Date))
                    {
                        var Ngay_Cong = new PayrollReportDetail_ShiftData();
                        Ngay_Cong.Name = shifts.FirstOrDefault(x => x.ShiftId == item.ShiftId)?.ShiftName;
                        Ngay_Cong.ShiftId = item.PayrollUserID;
                        Ngay_Cong.Timezone = shifts.FirstOrDefault(x => x.ShiftId == item.ShiftId)?.Timezone;
                        Ngay_Cong.EmployeeShiftId = item.AssignmentUserID;                       
                        double SOGIO_DIMUON = 0;
                        if (item.CheckinTime != null) //có check out 
                        {
                            SOGIO_DIMUON = PayrollHelper.CalculateLateMinutes(item.CheckinTime.GetValueOrDefault(), item.StartTime.GetValueOrDefault());
                            if (SOGIO_DIMUON > 0)
                            {
                                Ngay_Cong.Status = new PayrollReportDetail_ShiftStatus()
                                {
                                    Color = "#FF0000",
                                    StatusColor = new List<string> { "#FF0E39", "#FFCFD7" },
                                    Name = "Đi muộn",                                   
                                };
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else  //Đúng giờ
                        {
                            continue;
                        }
                        Ngay_Cong.CheckInTime = item.CheckinTime != null ? item.CheckinTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") : "";
                        Ngay_Cong.CheckOutTime = item.CheckoutTime != null ? item.CheckoutTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") : "";
                        Ngay_Cong.Approved = true;
                        Ngay_Cong.Value = string.Format("{0} Giờ {1} Phút", (int)(SOGIO_DIMUON / 60), SOGIO_DIMUON % 60);
                        day_item.Data.Add(Ngay_Cong);
                    }
                    if (day_item.Data != null && day_item.Data.Any())
                    {
                        response.Data.Add(day_item);
                    }
                }
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
                return response;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("GetEmployeeReport_SOGIO_DIMUON - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }
        public ApiResult<List<EmployeePayrollReportDetailResponse>> GetEmployeeReport_SOLAN_QUENCHECKIN(int accountMapID, int companyId, DateTime dateFrom, DateTime dateto)
        {
            var response = new ApiResult<List<EmployeePayrollReportDetailResponse>>()
            {
                Data = new List<EmployeePayrollReportDetailResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var user_Payroll = DaoFactory.Payroll.GetListByAccountMapID(accountMapID, dateFrom, dateto).Where(x => x.PayrollStatus == 1).ToList();               
                var shifts = DaoFactory.Shift.Shift_GetSimple(companyId, -1);
                for (DateTime loopDate = dateFrom; loopDate <= dateto; loopDate = loopDate.AddDays(1))
                {
                    var day_item = new EmployeePayrollReportDetailResponse();
                    day_item.Title = loopDate.ToString("yyyy-MM-dd HH:mm:ss");
                    day_item.Data = new List<PayrollReportDetail_ShiftData>() { };
                    foreach (var item in user_Payroll.Where(x => x.WorkingDay.GetValueOrDefault().Date == loopDate.Date))
                    {
                        var Ngay_Cong = new PayrollReportDetail_ShiftData();
                        Ngay_Cong.Name = shifts.FirstOrDefault(x => x.ShiftId == item.ShiftId)?.ShiftName;
                        Ngay_Cong.ShiftId = item.PayrollUserID;
                        Ngay_Cong.Timezone = shifts.FirstOrDefault(x => x.ShiftId == item.ShiftId)?.Timezone;
                        Ngay_Cong.EmployeeShiftId = item.AssignmentUserID;                        
                        if (item.CheckinTime == null && item.CheckoutTime != null) //quên check in
                        {
                            Ngay_Cong.Status = new PayrollReportDetail_ShiftStatus()
                            {
                                Color = "#FF0000",
                                StatusColor = new List<string> { "#FF0E39", "#FFCFD7" },
                                Name = "Quên check in",

                            };
                        }
                        else
                        {
                            continue;
                        }
                        Ngay_Cong.CheckInTime = "";
                        Ngay_Cong.CheckOutTime = item.CheckoutTime != null ? item.CheckoutTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") : "";
                        Ngay_Cong.Approved = true;
                        Ngay_Cong.Value = "1.00";
                        day_item.Data.Add(Ngay_Cong);
                    }
                    if (day_item.Data != null && day_item.Data.Any())
                    {
                        response.Data.Add(day_item);
                    }
                }
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
                return response;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("GetEmployeeReport_SOLAN_QUENCHECKIN - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }
            return response;
        }
        public ApiResult<List<EmployeePayrollReportDetailResponse>> GetEmployeeReport_SOLAN_QUENCHECKOUT(int accountMapID, int companyId, DateTime dateFrom, DateTime dateto)
        {
            var response = new ApiResult<List<EmployeePayrollReportDetailResponse>>()
            {
                Data = new List<EmployeePayrollReportDetailResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var user_Payroll = DaoFactory.Payroll.GetListByAccountMapID(accountMapID, dateFrom, dateto).Where(x => x.PayrollStatus == 1).ToList();               
                var shifts = DaoFactory.Shift.Shift_GetSimple(companyId, -1);
                for (DateTime loopDate = dateFrom; loopDate <= dateto; loopDate = loopDate.AddDays(1))
                {
                    var day_item = new EmployeePayrollReportDetailResponse();
                    day_item.Title = loopDate.ToString("yyyy-MM-dd HH:mm:ss");
                    day_item.Data = new List<PayrollReportDetail_ShiftData>() { };
                    foreach (var item in user_Payroll.Where(x => x.WorkingDay.GetValueOrDefault().Date == loopDate.Date))
                    {
                        var Ngay_Cong = new PayrollReportDetail_ShiftData();
                        Ngay_Cong.Name = shifts.FirstOrDefault(x => x.ShiftId == item.ShiftId)?.ShiftName;
                        Ngay_Cong.ShiftId = item.PayrollUserID;
                        Ngay_Cong.Timezone = shifts.FirstOrDefault(x => x.ShiftId == item.ShiftId)?.Timezone;
                        Ngay_Cong.EmployeeShiftId = item.AssignmentUserID;

                        if (item.CheckoutTime == null && item.CheckinTime != null) //quên check out
                        {
                            Ngay_Cong.Status = new PayrollReportDetail_ShiftStatus()
                            {
                                Color = "#FF0000",
                                StatusColor = new List<string> { "#FF0E39", "#FFCFD7" },
                                Name = "Quên check out",
                            };
                        }
                        else
                        {
                            continue;
                        }
                        Ngay_Cong.CheckInTime = item.CheckinTime != null ? item.CheckinTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") : "";
                        Ngay_Cong.CheckOutTime = "";
                        Ngay_Cong.Approved = true;
                        Ngay_Cong.Value = "1.00";
                        day_item.Data.Add(Ngay_Cong);
                    }
                    if (day_item.Data != null && day_item.Data.Any())
                    {
                        response.Data.Add(day_item);
                    }
                }
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
                return response;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("GetEmployeeReport_SOLAN_QUENCHECKOUT - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }
            return response;
        }
        public ApiResult<List<EmployeePayrollReportDetailResponse>> GetEmployeeReport_SOLAN_QUENCHECKINOUT(int accountMapID, int companyId, DateTime dateFrom, DateTime dateto)
        {
            var response = new ApiResult<List<EmployeePayrollReportDetailResponse>>()
            {
                Data = new List<EmployeePayrollReportDetailResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var user_Payroll = DaoFactory.Payroll.GetListByAccountMapID(accountMapID, dateFrom, dateto).Where(x => x.PayrollStatus == 1).ToList();
                var shifts = DaoFactory.Shift.Shift_GetSimple(companyId, -1);
                for (DateTime loopDate = dateFrom; loopDate <= dateto; loopDate = loopDate.AddDays(1))
                {
                    var day_item = new EmployeePayrollReportDetailResponse();
                    day_item.Title = loopDate.ToString("yyyy-MM-dd HH:mm:ss");
                    day_item.Data = new List<PayrollReportDetail_ShiftData>() { };
                    foreach (var item in user_Payroll.Where(x => x.WorkingDay.GetValueOrDefault().Date == loopDate.Date))
                    {
                        var Ngay_Cong = new PayrollReportDetail_ShiftData();
                        Ngay_Cong.Name = shifts.FirstOrDefault(x => x.ShiftId == item.ShiftId)?.ShiftName;
                        Ngay_Cong.ShiftId = item.PayrollUserID;
                        Ngay_Cong.Timezone = shifts.FirstOrDefault(x => x.ShiftId == item.ShiftId)?.Timezone;
                        Ngay_Cong.EmployeeShiftId = item.AssignmentUserID;

                        if (item.CheckinTime == null && item.CheckoutTime == null)
                        {
                            Ngay_Cong.Status = new PayrollReportDetail_ShiftStatus()
                            {
                                Color = "#FF0000",
                                StatusColor = new List<string> { "#FF0E39", "#FFCFD7" },
                                Name = "Quên Chấm công",
                            };
                        }
                        else
                        {
                            continue;
                        }
                        Ngay_Cong.CheckInTime = "";
                        Ngay_Cong.CheckOutTime = "";
                        Ngay_Cong.Approved = true;
                        Ngay_Cong.Value = "1.00";

                        day_item.Data.Add(Ngay_Cong);
                    }
                    if (day_item.Data != null && day_item.Data.Any())
                    {
                        response.Data.Add(day_item);
                    }
                }
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
                return response;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("GetEmployeeReport_SOLAN_QUENCHECKINOUT - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }
            return response;
        }
        #endregion GetEmployeeReport

        public ApiResult<ClockTabEmployeeClockResponse> ClockTabEmployeeClock(int companyId, DateTime dateFrom, DateTime dateto)
        {
            var response = new ApiResult<ClockTabEmployeeClockResponse>
            {
                Data = new ClockTabEmployeeClockResponse()
                {
                    EmployeeAccountMap = new List<List<long>>(),
                    Payroll = new List<List<long>>(),
                    info = new ClockTabEmployeeClock_Info()
                    {
                        data1 = "Tổng số NV",
                        data2 = "NV chấm công"
                    }
                },
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                int totalEmployee = 0;
                int totalPayroll = 0;
                var data_EmployeeAccountMap = DaoFactory.EmployeeReport.EmployeeAccountMap_ReportByCompanyID(companyId, dateFrom, dateto, out totalEmployee);
                var data_Payroll = DaoFactory.PayrollReport.Payroll_ReportByCompanyID(companyId, dateFrom, dateto, out totalPayroll);

                for (DateTime loopDate = dateFrom.Date; loopDate <= dateto; loopDate = loopDate.AddDays(1))
                {

                    if (data_EmployeeAccountMap.Any(x => x.ReportDate.GetValueOrDefault().Date == loopDate))
                    {
                        totalEmployee = totalEmployee + data_EmployeeAccountMap.FirstOrDefault(x => x.ReportDate.GetValueOrDefault().Date == loopDate).TotalEmployee.GetValueOrDefault(0);
                        response.Data.EmployeeAccountMap.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            totalEmployee
                        });
                    }
                    else
                    {
                        response.Data.EmployeeAccountMap.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            totalEmployee
                        });
                    }

                    if (data_Payroll.Any(x => x.ReportDate.GetValueOrDefault().Date == loopDate))
                    {
                        response.Data.Payroll.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            data_Payroll.FirstOrDefault(x => x.ReportDate.GetValueOrDefault().Date == loopDate).TotalEmployee.GetValueOrDefault(0)
                        });
                    }
                    else
                    {
                        response.Data.Payroll.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            0
                        });
                    }
                }

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ReportBo.GetDashboardDevicesAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        public ApiResult<ClockTabClockLateSoonResponse> ClockTabClockLateSoon(int companyId, DateTime dateFrom, DateTime dateto)
        {
            var response = new ApiResult<ClockTabClockLateSoonResponse>
            {
                Data = new ClockTabClockLateSoonResponse()
                {
                    EmployeeAccountMap = new List<List<long>>(),
                    Late = new List<List<long>>(),
                    Soon = new List<List<long>>(),
                    Info = new ClockTabClockLateSoon_Info()
                    {
                        data1 = "Tổng số NV",
                        data2 = "Đi trễ",
                        data3 = "Về sớm",
                    }
                },
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                int totalEmployee = 0;
                int totalPayroll = 0;
                var data_EmployeeAccountMap = DaoFactory.EmployeeReport.EmployeeAccountMap_ReportByCompanyID(companyId, dateFrom, dateto, out totalEmployee);
                var data_Payroll_Late = DaoFactory.PayrollReport.Payroll_ReportSoonLateByCompanyID(companyId, dateFrom, dateto, 1, out totalPayroll);
                var data_Payroll_Soon = DaoFactory.PayrollReport.Payroll_ReportSoonLateByCompanyID(companyId, dateFrom, dateto, 2, out totalPayroll);

                for (DateTime loopDate = dateFrom.Date; loopDate <= dateto; loopDate = loopDate.AddDays(1))
                {

                    if (data_EmployeeAccountMap.Any(x => x.ReportDate.GetValueOrDefault().Date == loopDate))
                    {
                        totalEmployee = totalEmployee + data_EmployeeAccountMap.FirstOrDefault(x => x.ReportDate.GetValueOrDefault().Date == loopDate).TotalEmployee.GetValueOrDefault(0);
                        response.Data.EmployeeAccountMap.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            totalEmployee
                        });
                    }
                    else
                    {
                        response.Data.EmployeeAccountMap.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            totalEmployee
                        });
                    }

                    if (data_Payroll_Late.Any(x => x.ReportDate.GetValueOrDefault().Date == loopDate))
                    {
                        response.Data.Late.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            data_Payroll_Late.FirstOrDefault(x => x.ReportDate.GetValueOrDefault().Date == loopDate).TotalEmployee.GetValueOrDefault(0)
                        });
                    }
                    else
                    {
                        response.Data.Late.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            0
                        });
                    }

                    if (data_Payroll_Soon.Any(x => x.ReportDate.GetValueOrDefault().Date == loopDate))
                    {
                        response.Data.Soon.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            data_Payroll_Soon.FirstOrDefault(x => x.ReportDate.GetValueOrDefault().Date == loopDate).TotalEmployee.GetValueOrDefault(0)
                        });
                    }
                    else
                    {
                        response.Data.Soon.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            0
                        });
                    }
                }

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ReportBo.GetDashboardDevicesAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        public ApiResult<ClockTabNotClockInOutResponse> ClockTabNotClockInOut(int companyId, DateTime dateFrom, DateTime dateto)
        {
            var response = new ApiResult<ClockTabNotClockInOutResponse>
            {
                Data = new ClockTabNotClockInOutResponse()
                {
                    EmployeeAccountMap = new List<List<long>>(),
                    NotCheckIn = new List<List<long>>(),
                    NotCheckOut = new List<List<long>>(),
                    NotCheckInAndOut = new List<List<long>>(),
                    Info = new ClockTabNotClockInOut_Info()
                    {
                        data1 = "Tổng số NV",
                        data2 = "Không vào ca",
                        data3 = "Không ra ca",
                        data4 = "Không chấm công",
                    }
                },
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                int totalEmployee = 0;
                int totalPayroll = 0;
                var data_EmployeeAccountMap = DaoFactory.EmployeeReport.EmployeeAccountMap_ReportByCompanyID(companyId, dateFrom, dateto, out totalEmployee);
                var data_Payroll_NotCheckIn = DaoFactory.PayrollReport.Payroll_ReportNotInOutByCompanyID(companyId, dateFrom, dateto, 1, out totalPayroll);
                var data_Payroll_NotCheckOut = DaoFactory.PayrollReport.Payroll_ReportNotInOutByCompanyID(companyId, dateFrom, dateto, 2, out totalPayroll);
                var data_Payroll_NotCheckInAndOut = DaoFactory.PayrollReport.Payroll_ReportNotInOutByCompanyID(companyId, dateFrom, dateto, 3, out totalPayroll);

                for (DateTime loopDate = dateFrom.Date; loopDate <= dateto; loopDate = loopDate.AddDays(1))
                {
                    #region Employee
                    if (data_EmployeeAccountMap.Any(x => x.ReportDate.GetValueOrDefault().Date == loopDate))
                    {
                        totalEmployee = totalEmployee + data_EmployeeAccountMap.FirstOrDefault(x => x.ReportDate.GetValueOrDefault().Date == loopDate).TotalEmployee.GetValueOrDefault(0);
                        response.Data.EmployeeAccountMap.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            totalEmployee
                        });
                    }
                    else
                    {
                        response.Data.EmployeeAccountMap.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            totalEmployee
                        });
                    }
                    #endregion

                    if (data_Payroll_NotCheckIn.Any(x => x.ReportDate.GetValueOrDefault().Date == loopDate))
                    {
                        response.Data.NotCheckIn.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            data_Payroll_NotCheckIn.FirstOrDefault(x => x.ReportDate.GetValueOrDefault().Date == loopDate).TotalEmployee.GetValueOrDefault(0)
                        });
                    }
                    else
                    {
                        response.Data.NotCheckIn.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            0
                        });
                    }

                    if (data_Payroll_NotCheckOut.Any(x => x.ReportDate.GetValueOrDefault().Date == loopDate))
                    {
                        response.Data.NotCheckOut.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            data_Payroll_NotCheckOut.FirstOrDefault(x => x.ReportDate.GetValueOrDefault().Date == loopDate).TotalEmployee.GetValueOrDefault(0)
                        });
                    }
                    else
                    {
                        response.Data.NotCheckOut.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            0
                        });
                    }

                    if (data_Payroll_NotCheckInAndOut.Any(x => x.ReportDate.GetValueOrDefault().Date == loopDate))
                    {
                        response.Data.NotCheckInAndOut.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            data_Payroll_NotCheckInAndOut.FirstOrDefault(x => x.ReportDate.GetValueOrDefault().Date == loopDate).TotalEmployee.GetValueOrDefault(0)
                        });
                    }
                    else
                    {
                        response.Data.NotCheckInAndOut.Add(new List<long> {
                            loopDate.ToUnixTimestamp(),
                            0
                        });
                    }
                }

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ReportBo.GetDashboardDevicesAsync - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        public ApiResult<TimesheetTabWorkingHoursResponse> TimesheetTabWorkingHours(int companyId, DateTime dateFrom, DateTime dateto)
        {
            var response = new ApiResult<TimesheetTabWorkingHoursResponse>
            {
                Data = new TimesheetTabWorkingHoursResponse()
                {
                    GIOCONG_THUCTE = new List<List<double>>(),
                    GIOCONG_TIEUCHUAN = new List<List<double>>(),
                    Info = new TimesheetTabWorkingHours_Info()
                    {
                        data1 = "Giờ công thực tế",
                        data2 = "Giờ công tiêu chuẩn",
                    }
                },
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            try
            {
                var data_Payroll = DaoFactory.PayrollReport.Payroll_User_GetListByCompanyID(companyId, dateFrom, dateto);
                var dataHour = DaoFactory.Shift.GetTimes("vn");
                // này để dành lưu khỏi gọi lại
                List<Ins_ShiftTimeInOutConfig_GetByShiftId_Result> timeInOutConfig = new List<Ins_ShiftTimeInOutConfig_GetByShiftId_Result>();
                Ins_ShiftTimeInOutConfig_GetByShiftId_Result shift_TimeInOutConfig = new Ins_ShiftTimeInOutConfig_GetByShiftId_Result();
                double GIOCONG_THUCTE = 0;
                double GIOCONG_TIEUCHUAN = 0;
                for (DateTime loopDate = dateFrom.Date; loopDate <= dateto; loopDate = loopDate.AddDays(1))
                {
                    GIOCONG_THUCTE = 0;
                    GIOCONG_TIEUCHUAN = 0;
                    var user_Payroll = data_Payroll.Where(x => x.WorkingDay.GetValueOrDefault().Date == loopDate).ToList();
                    foreach (var item in user_Payroll.Where(x => x.WorkingDay.GetValueOrDefault().Date == loopDate.Date))
                    {
                        // rule đã lấy trươc đó thì khỏi gọi lấy lại
                        if (timeInOutConfig.Any(x => x.ShiftID == item.ShiftId))
                        {
                            shift_TimeInOutConfig = timeInOutConfig.Where(x => x.ShiftID == item.ShiftId).FirstOrDefault();
                        }
                        else // chưa lấy thì gọi db 
                        {
                            shift_TimeInOutConfig = DaoFactory.Shift.GetShiftTimeConfig(item.ShiftId).FirstOrDefault();
                            if (shift_TimeInOutConfig != null && shift_TimeInOutConfig.ShiftID != 0)
                            {
                                timeInOutConfig.Add(shift_TimeInOutConfig);
                            }
                        }

                        #region GIOCONG_TIEUCHUAN                       
                        //chưa cấu hình giờ nghỉ trưa  tính mặc định 12:00 - 13:30
                        if (shift_TimeInOutConfig == null
                                || (shift_TimeInOutConfig.RestStartHourId ?? 0) == 0
                                || (shift_TimeInOutConfig.RestStartMinuteId ?? 0) == 0
                                || (shift_TimeInOutConfig.RestEndHourId ?? 0) == 0
                                || (shift_TimeInOutConfig.RestEndMinuteId ?? 0) == 0
                           )
                        {
                            GIOCONG_TIEUCHUAN = GIOCONG_TIEUCHUAN + PayrollHelper.CalculateWorkHours(
                                    item.StartTime.GetValueOrDefault(),
                                    item.EndTime.GetValueOrDefault(),
                                    item.StartTime.GetValueOrDefault(),
                                    item.EndTime.GetValueOrDefault(),
                                    new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day, 12, 0, 0),
                                    new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day, 13, 30, 0)
                                );
                        }
                        else
                        {
                            GIOCONG_TIEUCHUAN = GIOCONG_TIEUCHUAN + PayrollHelper.CalculateWorkHours(
                                    item.StartTime.GetValueOrDefault(),
                                    item.EndTime.GetValueOrDefault(),
                                    item.StartTime.GetValueOrDefault(),
                                    item.EndTime.GetValueOrDefault(),
                                    new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day,
                                    dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestStartHourId ?? 0) && z.IsHour == 1).Value ?? 0,
                                    dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestStartMinuteId ?? 0) && z.IsHour == 0).Value ?? 0,
                                    0
                                    ),
                                    new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day,
                                    dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestEndHourId ?? 0) && z.IsHour == 1).Value ?? 0,
                                    dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestEndMinuteId ?? 0) && z.IsHour == 0).Value ?? 0,
                                    0
                                    )
                                );
                        }
                        #endregion

                        if (item.CheckinTime == null || item.CheckoutTime == null)
                        {
                            continue;
                        }

                        #region GIOCONG_THUCTE                       
                        //chưa cấu hình giờ nghỉ trưa  tính mặc định 12:00 - 13:30
                        if (shift_TimeInOutConfig == null
                                || (shift_TimeInOutConfig.RestStartHourId ?? 0) == 0
                                || (shift_TimeInOutConfig.RestStartMinuteId ?? 0) == 0
                                || (shift_TimeInOutConfig.RestEndHourId ?? 0) == 0
                                || (shift_TimeInOutConfig.RestEndMinuteId ?? 0) == 0
                           )
                        {
                            GIOCONG_THUCTE = GIOCONG_THUCTE + PayrollHelper.CalculateWorkHours(
                                    item.CheckinTime.GetValueOrDefault(),
                                    item.CheckoutTime.GetValueOrDefault(),
                                    item.StartTime.GetValueOrDefault(),
                                    item.EndTime.GetValueOrDefault(),
                                    new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day, 12, 0, 0),
                                    new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day, 13, 30, 0)
                                );
                        }
                        else
                        {
                            GIOCONG_THUCTE = GIOCONG_THUCTE + PayrollHelper.CalculateWorkHours(
                                    item.CheckinTime.GetValueOrDefault(),
                                    item.CheckoutTime.GetValueOrDefault(),
                                    item.StartTime.GetValueOrDefault(),
                                    item.EndTime.GetValueOrDefault(),
                                    new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day,
                                    dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestStartHourId ?? 0) && z.IsHour == 1).Value ?? 0,
                                    dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestStartMinuteId ?? 0) && z.IsHour == 0).Value ?? 0,
                                    0
                                    ),
                                    new DateTime(item.StartTime.GetValueOrDefault().Year, item.StartTime.GetValueOrDefault().Month, item.StartTime.GetValueOrDefault().Day,
                                    dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestEndHourId ?? 0) && z.IsHour == 1).Value ?? 0,
                                    dataHour.FirstOrDefault(z => z.ID == (shift_TimeInOutConfig.RestEndMinuteId ?? 0) && z.IsHour == 0).Value ?? 0,
                                    0
                                    )
                                );
                        }
                        #endregion
                    }

                    response.Data.GIOCONG_THUCTE.Add(new List<double> {
                            loopDate.ToUnixTimestamp(),
                            GIOCONG_THUCTE
                        });
                    response.Data.GIOCONG_TIEUCHUAN.Add(new List<double> {
                            loopDate.ToUnixTimestamp(),
                            GIOCONG_TIEUCHUAN
                        });
                }

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ReportBo.TimesheetTabWorkingHours - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        public ApiResult<TimesheetTabWorkingDayResponse> TimesheetTabWorkingDate(int companyId, DateTime dateFrom, DateTime dateto)
        {
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
                var data_Payroll = DaoFactory.PayrollReport.Payroll_User_GetListByCompanyID(companyId, dateFrom, dateto);
                var company_Shift = DaoFactory.Shift.Shift_GetSimple(companyId, -1);
                // này để dành lưu khỏi gọi lại
                List<Ins_Shift_TimePenaltyRule_SelectByShiftId_Result> timePenaltyRule = new List<Ins_Shift_TimePenaltyRule_SelectByShiftId_Result>();
                List<Ins_Shift_TimePenaltyRule_SelectByShiftId_Result> shift_TimePenaltyRule = new List<Ins_Shift_TimePenaltyRule_SelectByShiftId_Result>();
                decimal NGAYCONG_THUCTE = 0;
                decimal NGAYCONG_TIEUCHUAN = 0;
                for (DateTime loopDate = dateFrom.Date; loopDate <= dateto; loopDate = loopDate.AddDays(1))
                {
                    NGAYCONG_THUCTE = 0;
                    NGAYCONG_TIEUCHUAN = 0;
                    var user_Payroll = data_Payroll.Where(x => x.WorkingDay.GetValueOrDefault().Date == loopDate).ToList();
                    foreach (var item in user_Payroll.Where(x => x.WorkingDay.GetValueOrDefault().Date == loopDate.Date))
                    {
                        // rule đã lấy trươc đó thì khỏi gọi lấy lại
                        if (timePenaltyRule.Any(x => x.ShiftID == item.ShiftId))
                        {
                            shift_TimePenaltyRule = timePenaltyRule.Where(x => x.ShiftID == item.ShiftId).ToList();
                        }
                        else // chưa lấy thì gọi db 
                        {
                            shift_TimePenaltyRule = DaoFactory.Shift.Shift_TimePenaltyRule_SelectByShiftId(item.ShiftId);
                            // add rule vô log để dành xài
                            if (shift_TimePenaltyRule != null && shift_TimePenaltyRule.Any())
                            {
                                timePenaltyRule.AddRange(shift_TimePenaltyRule);
                            }
                        }

                        NGAYCONG_TIEUCHUAN = NGAYCONG_TIEUCHUAN +
                                              PayrollHelper.CalculateTotalPenalty(
                                              item.StartTime.GetValueOrDefault(),
                                              item.EndTime.GetValueOrDefault(),
                                              item.StartTime.GetValueOrDefault(),
                                              item.EndTime.GetValueOrDefault(),
                                              shift_TimePenaltyRule,
                                              company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId) != null ? company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId).LatelyCheckIn : 0,
                                              company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId) != null ? company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId).EarlyCheckOut : 0
                                              );

                        if (item.CheckinTime == null || item.CheckoutTime == null)
                        {
                            continue;
                        }

                        NGAYCONG_THUCTE = NGAYCONG_THUCTE +
                                              PayrollHelper.CalculateTotalPenalty(
                                              item.StartTime.GetValueOrDefault(),
                                              item.EndTime.GetValueOrDefault(),
                                              item.CheckinTime.GetValueOrDefault(),
                                              item.CheckoutTime.GetValueOrDefault(),
                                              shift_TimePenaltyRule,
                                              company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId) != null ? company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId).LatelyCheckIn : 0,
                                              company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId) != null ? company_Shift.FirstOrDefault(x => x.ShiftId == item.ShiftId).EarlyCheckOut : 0
                                              );
                    }

                    response.Data.NGAYCONG_THUCTE.Add(new List<decimal> {
                            loopDate.ToUnixTimestamp(),
                             Math.Round(NGAYCONG_THUCTE)
                        });
                    response.Data.NGAYCONG_TIEUCHUAN.Add(new List<decimal> {
                            loopDate.ToUnixTimestamp(),
                             Math.Round(NGAYCONG_TIEUCHUAN)
                        });
                }

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ReportBo.TimesheetTabWorkingHours - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }
    }
}