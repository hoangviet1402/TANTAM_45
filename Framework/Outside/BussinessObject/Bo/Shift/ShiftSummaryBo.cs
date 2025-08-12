using BussinessObject.Models.Shift;
using DataAccess;
using Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using MyUtility.Extensions;
using EntitiesObject.Entities.TanTamEntities;
using BussinessObject.Helper;

namespace BussinessObject.Bo.Shift
{
    /// <summary>
    /// Business Object for handling shift summary operations
    /// </summary>
    public class ShiftSummaryBo : BaseBo<DBNull>
    {
        #region Constants
        private const int MAX_DATE_RANGE_DAYS = 31;
        private const int EXPANDED_DATE_RANGE_YEARS = 1;
        private const string FUTURE_SHIFT_COLOR = "#C4C4C4";
        private const string ON_TIME_COLOR = "#7ED321";
        private const string LATE_CHECKIN_COLOR = "#FFCB76";
        private const string EARLY_CHECKOUT_COLOR = "#FF9500";
        private const string INCOMPLETE_SHIFT_COLOR = "#FF0000";
        private const string NO_CHECKIN_COLOR = "#666666";
        #endregion

        public ShiftSummaryBo()
            : base(DaoFactory.Shift)
        {
        }

        /// <summary>
        /// Set shift status based on checkin/checkout time comparison with shift start/end times
        /// </summary>
        private void SetShiftStatus(ShiftDetailItem shiftDetail, Ins_ShiftAssignment_User_WorkingDay_GetSummary_Single_Result shift)
        {
            var status = new ShiftStatus();
            
            // Check if shift is in future
            var shiftDate = DateTime.Parse(shiftDetail.working_day.Split(' ')[0]);
            var currentDate = DateTime.Now.Date;
            
            if (shiftDate > currentDate)
            {
                SetFutureShiftStatus(status);
            }
            else if (!string.IsNullOrEmpty(shiftDetail.checkin_time) && !string.IsNullOrEmpty(shiftDetail.checkout_time))
            {
                SetCompletedShiftStatus(shiftDetail, shift, status);
            }
            else if (!string.IsNullOrEmpty(shiftDetail.checkin_time) && string.IsNullOrEmpty(shiftDetail.checkout_time))
            {
                SetIncompleteShiftStatus(status);
            }
            else
            {
                SetNoCheckinStatus(status);
            }

            shiftDetail.status = status;
        }

        /// <summary>
        /// Set status for future shifts
        /// </summary>
        private void SetFutureShiftStatus(ShiftStatus status)
        {
            status.color = FUTURE_SHIFT_COLOR;
            status.status_color = new List<string> { "#838BA3", "#EBEBEB" };
            status.name = "Chưa đến ca làm";
            status.not_available = 1;
            status.detail = new List<string>();
        }

        /// <summary>
        /// Set status for completed shifts (both checkin and checkout)
        /// </summary>
        private void SetCompletedShiftStatus(ShiftDetailItem shiftDetail, Ins_ShiftAssignment_User_WorkingDay_GetSummary_Single_Result shift, ShiftStatus status)
        {
            var checkinTime = DateTime.Parse(shiftDetail.checkin_time);
            var checkoutTime = DateTime.Parse(shiftDetail.checkout_time);
            var shiftStartTime = DateTime.Parse(shiftDetail.start_time);
            var shiftEndTime = DateTime.Parse(shiftDetail.end_time);
            
            var latelyCheckinMinutes = shift.LatelyCheckIn;
            var earlyCheckoutMinutes = shift.EarlyCheckOut;
            
            var checkinDiff = checkinTime - shiftStartTime;
            var checkoutDiff = checkoutTime - shiftEndTime;
            
            if (checkinDiff.TotalMinutes <= latelyCheckinMinutes && checkoutDiff.TotalMinutes >= -earlyCheckoutMinutes)
            {
                SetOnTimeStatus(shiftDetail, status);
            }
            else if (checkinDiff.TotalMinutes > latelyCheckinMinutes)
            {
                SetLateCheckinStatus(shiftDetail, shift, status);
            }
            else if (checkoutDiff.TotalMinutes < -earlyCheckoutMinutes)
            {
                SetEarlyCheckoutStatus(shiftDetail, shift, status);
            }
            else
            {
                SetOtherTimeStatus(shiftDetail, status);
            }
        }

        /// <summary>
        /// Set status for on-time shifts
        /// </summary>
        private void SetOnTimeStatus(ShiftDetailItem shiftDetail, ShiftStatus status)
        {
            status.color = ON_TIME_COLOR;
            status.status_color = new List<string> { "#1ECC78", "#D2F5E4" };
            status.name = "Đúng giờ";
            status.detail = new List<string> { $"Thời gian: {shiftDetail.real_working_hour} giờ" };
        }

        /// <summary>
        /// Set status for late checkin shifts
        /// </summary>
        private void SetLateCheckinStatus(ShiftDetailItem shiftDetail, Ins_ShiftAssignment_User_WorkingDay_GetSummary_Single_Result shift, ShiftStatus status)
        {
            var checkinTime = DateTime.Parse(shiftDetail.checkin_time);
            var checkoutTime = DateTime.Parse(shiftDetail.checkout_time);
            var shiftStartTime = DateTime.Parse(shiftDetail.start_time);
            var shiftEndTime = DateTime.Parse(shiftDetail.end_time);
            var latelyCheckinMinutes = shift.LatelyCheckIn;
            
            status.color = LATE_CHECKIN_COLOR;
            status.status_color = new List<string> { "#FFC888", "#FFF4E7" };
            status.name = "Trễ giờ vào ca";
            
            var checkinTimeStr = checkinTime.ToString("HH:mm");
            var expectedStartStr = shiftStartTime.ToString("HH:mm");
            var checkoutTimeStr = checkoutTime.ToString("HH:mm");
            var expectedEndStr = shiftEndTime.ToString("HH:mm");
            
            status.detail = new List<string> { 
                $"Vào ca trễ: {checkinTimeStr} (HS: {expectedStartStr}, cho phép trễ {latelyCheckinMinutes} phút). Ra ca: {checkoutTimeStr} (HS: {expectedEndStr})" 
            };
        }

        /// <summary>
        /// Set status for early checkout shifts
        /// </summary>
        private void SetEarlyCheckoutStatus(ShiftDetailItem shiftDetail, Ins_ShiftAssignment_User_WorkingDay_GetSummary_Single_Result shift, ShiftStatus status)
        {
            var checkinTime = DateTime.Parse(shiftDetail.checkin_time);
            var checkoutTime = DateTime.Parse(shiftDetail.checkout_time);
            var shiftStartTime = DateTime.Parse(shiftDetail.start_time);
            var shiftEndTime = DateTime.Parse(shiftDetail.end_time);
            var earlyCheckoutMinutes = shift.EarlyCheckOut;
            
            status.color = EARLY_CHECKOUT_COLOR;
            status.status_color = new List<string> { "#FF9500", "#FFF4E7" };
            status.name = "Ra ca sớm";
            
            var checkinTimeStr = checkinTime.ToString("HH:mm");
            var expectedStartStr = shiftStartTime.ToString("HH:mm");
            var checkoutTimeStr = checkoutTime.ToString("HH:mm");
            var expectedEndStr = shiftEndTime.ToString("HH:mm");
            
            status.detail = new List<string> { 
                $"Vào ca: {checkinTimeStr} (HS: {expectedStartStr}). Ra ca sớm: {checkoutTimeStr} (HS: {expectedEndStr}, cho phép sớm {earlyCheckoutMinutes} phút)" 
            };
        }

        /// <summary>
        /// Set status for other time issues
        /// </summary>
        private void SetOtherTimeStatus(ShiftDetailItem shiftDetail, ShiftStatus status)
        {
            var checkinTime = DateTime.Parse(shiftDetail.checkin_time);
            var checkoutTime = DateTime.Parse(shiftDetail.checkout_time);
            var shiftStartTime = DateTime.Parse(shiftDetail.start_time);
            var shiftEndTime = DateTime.Parse(shiftDetail.end_time);
            
            status.color = LATE_CHECKIN_COLOR; // Default to late checkin color for other issues
            status.status_color = new List<string> { "#FFC888", "#FFF4E7" };
            status.name = "Không đúng giờ";
            
            var checkinTimeStr = checkinTime.ToString("HH:mm");
            var expectedStartStr = shiftStartTime.ToString("HH:mm");
            var checkoutTimeStr = checkoutTime.ToString("HH:mm");
            var expectedEndStr = shiftEndTime.ToString("HH:mm");
            
            status.detail = new List<string> { 
                $"Vào ca: {checkinTimeStr} (HS: {expectedStartStr}). Ra ca: {checkoutTimeStr} (HS: {expectedEndStr})" 
            };
        }

        /// <summary>
        /// Set status for incomplete shifts (only checkin, no checkout)
        /// </summary>
        private void SetIncompleteShiftStatus(ShiftStatus status)
        {
            status.color = INCOMPLETE_SHIFT_COLOR;
            status.status_color = new List<string> { "#FF0E39", "#FFCFD7" };
            status.name = "Chưa ra ca";
            status.detail = new List<string> { "Thời gian: 0 giờ" };
        }

        /// <summary>
        /// Set status for shifts with no checkin/checkout
        /// </summary>
        private void SetNoCheckinStatus(ShiftStatus status)
        {
            status.color = NO_CHECKIN_COLOR;
            status.status_color = new List<string> { "#838BA3", "#EBEBEB" };
            status.name = "Chưa vào/ra ca";
            status.detail = new List<string> { "Thời gian: 0 giờ" };
        }

        /// <summary>
        /// Get day of week assignments for shifts
        /// </summary>
        private Dictionary<int, List<int>> GetShiftDayOfWeekAssignments(List<Ins_ShiftAssignment_User_WorkingDay_GetShifts_Result> shifts)
        {
            var shiftAssignments = new Dictionary<int, List<int>>();
            
            if (!shifts.Any())
                return shiftAssignments;
            
            try
            {
                // Query Assignment table for DateOfWeek for each shift individually
                foreach (var shift in shifts)
                {
                    var assignments = DaoFactory.Shift.GetAssignmentDateOfWeekByShiftId(shift.ShiftId);
                    
                    if (!shiftAssignments.ContainsKey(shift.ShiftId))
                    {
                        shiftAssignments[shift.ShiftId] = new List<int>();
                    }
                    
                    foreach (var assignment in assignments)
                    {
                        shiftAssignments[shift.ShiftId].Add(assignment.DateOfWeek ?? 0);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"Error getting assignment day of week for shifts", ex);
                // If error, allow all days (fallback to old behavior)
                foreach (var shift in shifts)
                {
                    shiftAssignments[shift.ShiftId] = new List<int> { 0, 1, 2, 3, 4, 5, 6 }; // All days
                }
            }
            
            return shiftAssignments;
        }

        /// <summary>
        /// Check if date matches day of week assignments
        /// </summary>
        private bool ShouldCreateShiftForDate(DateTime date, List<int> dayOfWeekAssignments)
        {
            if (!dayOfWeekAssignments.Any())
                return true; // If no assignments, create for all days (fallback)
            
            // DayOfWeek: Sunday=0, Monday=1, Tuesday=2, Wednesday=3, Thursday=4, Friday=5, Saturday=6
            int dayOfWeek = (int)date.DayOfWeek;
            return dayOfWeekAssignments.Contains(dayOfWeek);
        }

        /// <summary>
        /// Create working day data in bulk for employees and shifts  
        /// </summary>
        private int CreateWorkingDayDataBulk(int companyId, DateTime startDate, DateTime endDate, List<string> employeeIds)
        {
            if (employeeIds == null || !employeeIds.Any())
            {
                return CreateWorkingDayDataForAllEmployees(companyId, startDate, endDate);
            }
            
            return CreateWorkingDayDataForSpecificEmployees(companyId, startDate, endDate, employeeIds);
        }

        /// <summary>
        /// Create working day data for all employees in company
        /// </summary>
        private int CreateWorkingDayDataForAllEmployees(int companyId, DateTime startDate, DateTime endDate)
        {
            var employees = DaoFactory.Shift.GetEmployeeSingle(companyId, null);
            var shifts = DaoFactory.Shift.GetShifts(companyId);
            
            ValidateEmployeesAndShifts(employees, shifts);
            
            var shiftDayOfWeekAssignments = GetShiftDayOfWeekAssignments(shifts);
            var dateRange = GetEffectiveDateRange(startDate, endDate);
            
            if (dateRange.StartDate > dateRange.EndDate)
            {
                return 0;
            }
            
            return CreateWorkingDayRecordsForDateRange(employees, shifts, shiftDayOfWeekAssignments, dateRange.StartDate, dateRange.EndDate);
        }

        /// <summary>
        /// Create working day data for specific employees
        /// </summary>
        private int CreateWorkingDayDataForSpecificEmployees(int companyId, DateTime startDate, DateTime endDate, List<string> employeeIds)
        {
            var totalRecordsCreated = 0;
            
            foreach (var empIdStr in employeeIds)
            {
                if (string.IsNullOrWhiteSpace(empIdStr)) continue;
                
                int empId;
                if (!int.TryParse(empIdStr, out empId)) continue;
                
                var employees = DaoFactory.Shift.GetEmployeeSingle(companyId, empId);
                var shifts = DaoFactory.Shift.GetShifts(companyId);
                
                if (!employees.Any() || !shifts.Any())
                {
                    continue; // Skip if no employees or shifts found
                }
                
                var shiftDayOfWeekAssignments = GetShiftDayOfWeekAssignments(shifts);
                var dateRange = GetEffectiveDateRange(startDate, endDate);
                
                if (dateRange.StartDate > dateRange.EndDate)
                {
                    continue;
                }
                
                totalRecordsCreated += CreateWorkingDayRecordsForDateRange(employees, shifts, shiftDayOfWeekAssignments, dateRange.StartDate, dateRange.EndDate);
            }
            
            return totalRecordsCreated;
        }

        /// <summary>
        /// Validate that employees and shifts exist
        /// </summary>
        private void ValidateEmployeesAndShifts(List<Ins_ShiftAssignment_User_WorkingDay_GetEmployee_Single_Result> employees, List<Ins_ShiftAssignment_User_WorkingDay_GetShifts_Result> shifts)
        {
            if (!employees.Any())
            {
                throw new InvalidOperationException("Công ty này chưa có nhân viên nào hoạt động.");
            }
            if (!shifts.Any())
            {
                throw new InvalidOperationException("Công ty này chưa có ca làm việc nào được kích hoạt.");
            }
        }

        /// <summary>
        /// Date range helper class
        /// </summary>
        private class DateRange
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }

        /// <summary>
        /// Get effective date range within current month
        /// </summary>
        private DateRange GetEffectiveDateRange(DateTime startDate, DateTime endDate)
        {
            var currentDate = DateTime.Now.Date;
            var currentMonthStart = new DateTime(currentDate.Year, currentDate.Month, 1);
            var currentMonthEnd = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
            
            var effectiveStartDate = startDate < currentMonthStart ? currentMonthStart : startDate;
            var effectiveEndDate = endDate > currentMonthEnd ? currentMonthEnd : endDate;
            
            return new DateRange 
            { 
                StartDate = effectiveStartDate, 
                EndDate = effectiveEndDate 
            };
        }

        /// <summary>
        /// Create working day records for date range
        /// </summary>
        private int CreateWorkingDayRecordsForDateRange(
            List<Ins_ShiftAssignment_User_WorkingDay_GetEmployee_Single_Result> employees,
            List<Ins_ShiftAssignment_User_WorkingDay_GetShifts_Result> shifts,
            Dictionary<int, List<int>> shiftDayOfWeekAssignments,
            DateTime effectiveStartDate,
            DateTime effectiveEndDate)
        {
            var totalRecordsCreated = 0;
            var currentDate = DateTime.Now.Date;
            var currentMonthStart = new DateTime(currentDate.Year, currentDate.Month, 1);
            var currentMonthEnd = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
            
            for (DateTime loopDate = effectiveStartDate; loopDate <= effectiveEndDate; loopDate = loopDate.AddDays(1))
            {
                if (loopDate < currentMonthStart || loopDate > currentMonthEnd)
                {
                    continue;
                }
                
                foreach (var employee in employees)
                {
                    foreach (var shift in shifts)
                    {
                        var dayOfWeekAssignments = shiftDayOfWeekAssignments.ContainsKey(shift.ShiftId)
                            ? shiftDayOfWeekAssignments[shift.ShiftId]
                            : new List<int> { 0, 1, 2, 3, 4, 5, 6 };
                        
                        if (!ShouldCreateShiftForDate(loopDate, dayOfWeekAssignments))
                        {
                            continue;
                        }
                        
                        try
                        {
                            int recordCreated = DaoFactory.Shift.CreateShiftAssignmentUserWorkingDaySingle(
                                employee.EmployeeId,
                                shift.ShiftId,
                                loopDate,
                                false
                            );
                            totalRecordsCreated += recordCreated;
                        }
                        catch (Exception ex)
                        {
                            CommonLogger.DefaultLogger.Error($"Error creating working day for Employee {employee.EmployeeId}, Shift {shift.ShiftId}, Date {loopDate:yyyy-MM-dd}", ex);
                            continue;
                        }
                    }
                }
            }
            
            return totalRecordsCreated;
        }

        /// <summary>
        /// Get branches for shift assignment from database
        /// </summary>
        private List<EmployeeBranchObject> GetBranchesForShiftAssignment(int shiftAssignmentId, int companyId)
        {
            try
            {
                var branches = DaoFactory.Shift.GetBranchesByShiftAssignmentId(shiftAssignmentId, companyId);
                
                if (branches != null && branches.Any())
                {
                    return branches.Select(b => new EmployeeBranchObject
                    {
                        id = b.value,
                        name = b.label,
                        color = "" // Color not available in this result, can be enhanced later
                    }).ToList();
                }
                
                return null;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"Error getting branches for shift assignment {shiftAssignmentId}: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// Get employee shift summary with filtering options
        /// </summary>
        public ApiResult<EmployeeShiftSummaryResponse> GetEmployeeShiftSummary(EmployeeShiftSummaryRequest request, int employeeId, int role)
        {
            var response = new ApiResult<EmployeeShiftSummaryResponse>()
            {
                Data = new EmployeeShiftSummaryResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var dateRange = ParseAndValidateDateRange(request);
                if (dateRange == null)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.";
                    return response;
                }

                var employeeShiftId = ParseEmployeeShiftId(request);
                
                // Create working day data if needed
                if (request.IsShiftOnly != 1 && employeeShiftId == null)
                {
                    CreateWorkingDayDataBulk(request.CompanyId, dateRange.StartDate, dateRange.EndDate, request.EmployeeIds);
                }

                // Get summary data
                var summaryData = GetSummaryData(request, dateRange, employeeShiftId);
                if (employeeShiftId.HasValue && !summaryData.Any())
                {
                    response.Code = ResponseResultEnum.NotFound.Value();
                    response.Message = $"Không tìm thấy ca làm việc với ID: {request.EmployeeShiftId}";
                    return response;
                }

                // Filter by permissions
                // summaryData = FilterByPermissions(summaryData, employeeId, role);

                // Build response
                var items = BuildEmployeeShiftItems(summaryData, request);
                
                // Set response data
                response.Data.items = items;
                response.Data.meta.total = items.Count;
                response.Data.meta.count = items.Count;
                response.Data.meta.total_pages = (int)Math.Ceiling((double)items.Count / response.Data.meta.per_page);

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Lấy dữ liệu thành công";
            }
            catch (InvalidOperationException invalidEx)
            {
                response.Code = ResponseResultEnum.InvalidData.Value();
                response.Message = invalidEx.Message;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftSummaryBo.GetEmployeeShiftSummary - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Parse and validate date range from request
        /// </summary>
        private DateRange ParseAndValidateDateRange(EmployeeShiftSummaryRequest request)
        {
            DateTime? startDate = null;
            DateTime? endDate = null;

            // Handle employee_shift_id filtering
            if (!string.IsNullOrEmpty(request.EmployeeShiftId) && int.TryParse(request.EmployeeShiftId, out int _))
            {
                // Expand date range to 1 year to ensure we find the specific employee_shift_id
                var now = DateTime.Now;
                startDate = now.AddYears(-1);
                endDate = now.AddYears(1);
            }
            else
            {
                // Normal date range logic for summary requests
                if (!string.IsNullOrEmpty(request.StartDate))
                {
                    if (DateTime.TryParse(request.StartDate, out DateTime parsedStart))
                        startDate = parsedStart;
                }

                if (!string.IsNullOrEmpty(request.EndDate))
                {
                    if (DateTime.TryParse(request.EndDate, out DateTime parsedEnd))
                        endDate = parsedEnd;
                }

                if (!startDate.HasValue && request.Month > 0 && request.Year > 0)
                {
                    startDate = new DateTime(request.Year, request.Month, 1);
                    endDate = startDate.Value.AddMonths(1).AddDays(-1);
                }

                if (!startDate.HasValue)
                {
                    var now = DateTime.Now;
                    startDate = new DateTime(now.Year, now.Month, 1);
                    endDate = startDate.Value.AddMonths(1).AddDays(-1);
                }
            }

            if (startDate > endDate)
            {
                return null;
            }
            
            if (string.IsNullOrEmpty(request.EmployeeShiftId) && (endDate.Value - startDate.Value).TotalDays > MAX_DATE_RANGE_DAYS)
            {
                throw new InvalidOperationException($"Khoảng thời gian không được vượt quá {MAX_DATE_RANGE_DAYS} ngày để tránh ảnh hưởng đến hiệu suất hệ thống.");
            }

            return new DateRange { StartDate = startDate.Value, EndDate = endDate.Value };
        }

        /// <summary>
        /// Parse employee shift ID from request
        /// </summary>
        private int? ParseEmployeeShiftId(EmployeeShiftSummaryRequest request)
        {
            if (!string.IsNullOrEmpty(request.EmployeeShiftId))
            {
                if (int.TryParse(request.EmployeeShiftId, out int parsedId))
                {
                    return parsedId;
                }
            }
            return null;
        }

        /// <summary>
        /// Get summary data from database
        /// </summary>
        private List<Ins_ShiftAssignment_User_WorkingDay_GetSummary_Single_Result> GetSummaryData(
            EmployeeShiftSummaryRequest request, 
            DateRange dateRange, 
            int? employeeShiftId)
        {
            var summaryData = new List<Ins_ShiftAssignment_User_WorkingDay_GetSummary_Single_Result>();
            
            if (request.EmployeeIds == null || !request.EmployeeIds.Any())
            {
                // Get all employees (pass 0)
                summaryData = DaoFactory.Shift.GetShiftAssignmentUserWorkingDaySummary(
                    request.CompanyId,
                    dateRange.StartDate,
                    dateRange.EndDate,
                    0,
                    request.Month > 0 ? request.Month : (int?)null,
                    request.Year > 0 ? request.Year : (int?)null
                );
            }
            else
            {
                // Loop through each employeeId
                foreach (var empIdStr in request.EmployeeIds)
                {
                    if (string.IsNullOrWhiteSpace(empIdStr)) continue;
                    int empId;
                    if (!int.TryParse(empIdStr, out empId)) continue;
                    
                    var data = DaoFactory.Shift.GetShiftAssignmentUserWorkingDaySummary(
                        request.CompanyId,
                        dateRange.StartDate,
                        dateRange.EndDate,
                        empId,
                        request.Month > 0 ? request.Month : (int?)null,
                        request.Year > 0 ? request.Year : (int?)null
                    );
                    if (data != null && data.Any())
                    {
                        summaryData.AddRange(data);
                    }
                }
            }
            
            if (employeeShiftId.HasValue)
            {
                summaryData = summaryData.Where(x => x.SuwId == employeeShiftId.Value).ToList();
            }

            return summaryData;
        }

        /// <summary>
        /// Filter summary data by user permissions
        /// </summary>
        private List<Ins_ShiftAssignment_User_WorkingDay_GetSummary_Single_Result> FilterByPermissions(
            List<Ins_ShiftAssignment_User_WorkingDay_GetSummary_Single_Result> summaryData,
            int employeeId,
            int role)
        {
            var myEmployeeData = DaoFactory.Employee.GetEmployeeObjectData(employeeId);

            // Cache employee data to avoid repeated database calls
            var employeeDataCache = new Dictionary<int, Ins_Employee_GetObjectData_Result>();
            var uniqueEmployeeIds = summaryData.Select(x => x.EmployeeId).Distinct().ToList();
            
            foreach (var empId in uniqueEmployeeIds)
            {
                employeeDataCache[empId] = DaoFactory.Employee.GetEmployeeObjectData(empId);
            }

            return summaryData.Where(x => {
                var employeeData = employeeDataCache[x.EmployeeId];
                if (role == (int)UserRole.RegionalManager)
                {
                    if (myEmployeeData.RegionObjId > 0)
                    {
                        return employeeData.RegionObjId == myEmployeeData.RegionObjId;
                    }
                }

                if (role == (int)UserRole.BranchManager)
                {
                    if (myEmployeeData.BranchObjId > 0)
                    {
                        return employeeData.BranchObjId == myEmployeeData.BranchObjId;
                    }
                }

                return true;
            }).ToList();
        }

        /// <summary>
        /// Build employee shift items from summary data
        /// </summary>
        private List<EmployeeShiftItem> BuildEmployeeShiftItems(
            List<Ins_ShiftAssignment_User_WorkingDay_GetSummary_Single_Result> summaryData,
            EmployeeShiftSummaryRequest request)
        {
            var employeeGroups = summaryData
                .GroupBy(x => new { x.EmployeeId, x.UserId, x.FullName, x.EmployeeCode, x.Phone })
                .ToList();

            var items = new List<EmployeeShiftItem>();

            foreach (var empGroup in employeeGroups)
            {
                var employeeItem = CreateEmployeeShiftItem(empGroup.Key, request.CompanyId);
                var shiftsByDate = GroupShiftsByDate(empGroup);
                
                foreach (var dateGroup in shiftsByDate)
                {
                    var dateKey = GetUniqueDateKey(dateGroup.Key, employeeItem.shifts);
                    var shiftsForDate = CreateShiftsForDate(dateGroup, request);
                    employeeItem.shifts[dateKey] = shiftsForDate;
                }

                // Calculate totals
                var allShifts = employeeItem.shifts.SelectMany(x => x.Value);
                employeeItem.total_working_hour = Math.Round(allShifts.Sum(x => x.working_hour), 2);
                employeeItem.real_working_hour = Math.Round(allShifts.Sum(x => x.real_working_hour), 2);

                items.Add(employeeItem);
            }

            return items;
        }

        /// <summary>
        /// Create employee shift item
        /// </summary>
        private EmployeeShiftItem CreateEmployeeShiftItem(dynamic empKey, int companyId)
        {
            return new EmployeeShiftItem
            {
                user_id = empKey.UserId.ToString(),
                employee_id = empKey.EmployeeId.ToString(),
                phone = empKey.Phone ?? "",
                username = empKey.Phone ?? "",
                name = empKey.FullName ?? "",
                company_id = companyId.ToString(),
                identification = empKey.EmployeeCode ?? ""
            };
        }

        /// <summary>
        /// Group shifts by date
        /// </summary>
        private List<IGrouping<string, Ins_ShiftAssignment_User_WorkingDay_GetSummary_Single_Result>> GroupShiftsByDate(
            IGrouping<dynamic, Ins_ShiftAssignment_User_WorkingDay_GetSummary_Single_Result> empGroup)
        {
            return empGroup
                .GroupBy(d => d.WorkingDay.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"))
                .ToList();
        }

        /// <summary>
        /// Get unique date key for employee shifts
        /// </summary>
        private string GetUniqueDateKey(string originalDateKey, Dictionary<string, List<ShiftDetailItem>> shifts)
        {
            var dateKey = originalDateKey;
            var dateCounter = 0;
            while (shifts.ContainsKey(dateKey))
            {
                dateCounter++;
                dateKey = $"{originalDateKey}_{dateCounter}";
            }
            return dateKey;
        }

        /// <summary>
        /// Create shifts for specific date
        /// </summary>
        private List<ShiftDetailItem> CreateShiftsForDate(
            IGrouping<string, Ins_ShiftAssignment_User_WorkingDay_GetSummary_Single_Result> dateGroup,
            EmployeeShiftSummaryRequest request)
        {
            var shiftsForDate = new List<ShiftDetailItem>();

            foreach (var shift in dateGroup)
            {
                var originalShiftKey = shift.ShiftKey ?? "";
                var uniqueShiftKey = GetUniqueShiftKey(originalShiftKey, shiftsForDate);
                var shiftCounter = GetShiftCounter(originalShiftKey, shiftsForDate);

                var timeConfig = ShiftTimeConfigHelper.GetShiftTimeConfiguration(shift.ShiftId);
                
                var shiftDetail = CreateShiftDetailItem(shift, request, timeConfig, shiftCounter);

                // Apply with_branch filter
                if (request.WithBranch == 1)
                {
                    shiftDetail.branch_obj = GetBranchesForShiftAssignment(shift.ShiftAssignmentId, request.CompanyId);
                }
                else
                {
                    shiftDetail.branch_obj = null;
                }

                // Set display option
                shiftDetail.display_option = new DisplayOption
                {
                    shift_name = shiftCounter > 0 ? $"{shift.ShiftName}_{shiftCounter}" : shift.ShiftName ?? ""
                };

                // Apply is_shift_only filter
                if (request.IsShiftOnly != 1)
                {
                    SetShiftStatus(shiftDetail, shift);
                    SetCheckinCheckoutOptions(shiftDetail);
                }
                else
                {
                    SetMinimalShiftStatus(shiftDetail);
                }

                shiftsForDate.Add(shiftDetail);
            }

            return shiftsForDate;
        }

        /// <summary>
        /// Get unique shift key
        /// </summary>
        private string GetUniqueShiftKey(string originalShiftKey, List<ShiftDetailItem> shiftsForDate)
        {
            var uniqueShiftKey = originalShiftKey;
            var shiftCounter = 0;
            
            while (shiftsForDate.Any(s => s.shift_key == uniqueShiftKey))
            {
                shiftCounter++;
                uniqueShiftKey = $"{originalShiftKey}_{shiftCounter}";
            }
            
            return uniqueShiftKey;
        }

        /// <summary>
        /// Get shift counter for naming
        /// </summary>
        private int GetShiftCounter(string originalShiftKey, List<ShiftDetailItem> shiftsForDate)
        {
            var shiftCounter = 0;
            var testKey = originalShiftKey;
            
            while (shiftsForDate.Any(s => s.shift_key == testKey))
            {
                shiftCounter++;
                testKey = $"{originalShiftKey}_{shiftCounter}";
            }
            
            return shiftCounter;
        }

        /// <summary>
        /// Create shift detail item
        /// </summary>
        private ShiftDetailItem CreateShiftDetailItem(
            Ins_ShiftAssignment_User_WorkingDay_GetSummary_Single_Result shift,
            EmployeeShiftSummaryRequest request,
            dynamic timeConfig,
            int shiftCounter)
        {
            return new ShiftDetailItem
            {
                id = shift.SuwId.ToString(),
                name = shiftCounter > 0 ? $"{shift.ShiftName}_{shiftCounter}" : shift.ShiftName ?? "",
                shift_key = GetUniqueShiftKey(shift.ShiftKey ?? "", new List<ShiftDetailItem>()),
                shift_id = shift.ShiftId.ToString() ?? "",
                start_time = $"{shift.StartTime.GetValueOrDefault():yyyy-MM-dd} {timeConfig.StartTime}",
                end_time = $"{shift.EndTime.GetValueOrDefault():yyyy-MM-dd} {timeConfig.EndTime}",
                working_hour = timeConfig.WorkingHour,
                working_day = shift.WorkingDay.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                week_of_year = shift.WeekOfYear.GetValueOrDefault() > 0 ? shift.WeekOfYear.Value : 1,
                company_id = request.CompanyId.ToString(),
                checkin_time = (!request.IsWebView || (shift.CheckInByProxy ?? false)) && shift.StartCheckInTime.HasValue
                    ? shift.StartCheckInTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                    : null,
                checkout_time = (!request.IsWebView || (shift.CheckOutByProxy ?? false)) && shift.StartCheckOutTime.HasValue
                    ? shift.StartCheckOutTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                    : null,
                early_checkout_time = shift.EarlyCheckOut,
                lately_checkin_time = shift.LatelyCheckIn,
                start_checkin_time = $"{timeConfig.StartCheckin ?? ""}",
                end_checkin_time = $"{timeConfig.EndCheckin ?? ""}",
                start_checkout_time = $"{timeConfig.StartCheckout ?? ""}",
                end_checkout_time = $"{timeConfig.EndCheckout ?? ""}",
                shift_name = shiftCounter > 0 ? $"{shift.ShiftName}_{shiftCounter}" : shift.ShiftName ?? "",
                real_working_hour = Math.Round((double)(shift.RealWorkingHour ?? 0), 2),
                real_working_minute = shift.RealWorkingMinute.GetValueOrDefault(),
                coefficient = shift.Coefficient,
                real_coefficient = shift.Coefficient,
                meal_coefficient = shift.MealCoefficient
            };
        }

        /// <summary>
        /// Set checkin/checkout options
        /// </summary>
        private void SetCheckinCheckoutOptions(ShiftDetailItem shiftDetail)
        {
            if (!string.IsNullOrEmpty(shiftDetail.checkin_time))
            {
                shiftDetail.checkin_option = new CheckinOption
                {
                    type = "admin",
                    name = "Vào ca qua chấm công hộ",
                    type_name = "Admin"
                };
            }

            if (!string.IsNullOrEmpty(shiftDetail.checkout_time))
            {
                shiftDetail.checkout_option = new CheckoutOption
                {
                    type = "admin", 
                    name = "Ra ca qua chấm công hộ",
                    type_name = "Admin"
                };
            }
        }

        /// <summary>
        /// Set minimal shift status for shift-only mode
        /// </summary>
        private void SetMinimalShiftStatus(ShiftDetailItem shiftDetail)
        {
            shiftDetail.status = new ShiftStatus
            {
                color = FUTURE_SHIFT_COLOR,
                status_color = new List<string> { "#838BA3", "#EBEBEB" },
                name = "Ca làm việc",
                detail = new List<string>()
            };
        }
    }
} 