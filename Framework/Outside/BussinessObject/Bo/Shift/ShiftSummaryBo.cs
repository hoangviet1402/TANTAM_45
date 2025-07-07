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
        public ShiftSummaryBo()
            : base(DaoFactory.Shift)
        {
        }

        /// <summary>
        /// Set shift status based on checkin/checkout information
        /// </summary>
        private void SetShiftStatus(ShiftDetailItem shiftDetail, dynamic shift)
        {
            var status = new ShiftStatus();
            
            // Check if shift is in future
            var shiftDate = DateTime.Parse(shiftDetail.working_day.Split(' ')[0]);
            var currentDate = DateTime.Now.Date;
            
            if (shiftDate > currentDate)
            {
                // Future shift - not available yet
                status.color = "#C4C4C4";
                status.status_color = new List<string> { "#838BA3", "#EBEBEB" };
                status.name = "Chưa đến ca làm";
                status.not_available = 1;
                status.detail = new List<string>();
            }
            else if (!string.IsNullOrEmpty(shiftDetail.checkin_time) && !string.IsNullOrEmpty(shiftDetail.checkout_time))
            {
                // Both checkin and checkout available
                if (shiftDetail.real_working_hour >= shiftDetail.working_hour)
                {
                    // On time or overtime
                    status.color = "#7ED321";
                    status.status_color = new List<string> { "#1ECC78", "#D2F5E4" };
                    status.name = "Đúng giờ";
                    status.detail = new List<string> { $"Thời gian: {shiftDetail.real_working_hour} giờ" };
                    // ✅ real_coefficient already set from database in constructor
                }
                else
                {
                    // Late or early leave
                    status.color = "#FFCB76";
                    status.status_color = new List<string> { "#FFC888", "#FFF4E7" };
                    status.name = "Trễ giờ";
                    
                    var checkinTime = DateTime.Parse(shiftDetail.checkin_time).ToString("HH:mm");
                    var checkoutTime = DateTime.Parse(shiftDetail.checkout_time).ToString("HH:mm");
                    var expectedStart = DateTime.Parse(shiftDetail.start_time).ToString("HH:mm");
                    var expectedEnd = DateTime.Parse(shiftDetail.end_time).ToString("HH:mm");
                    
                    status.detail = new List<string> { 
                        $"Thời gian: Vào ca/Ra ca lúc: {checkinTime}:{checkoutTime} (HS). Ca làm: {expectedStart}:{expectedEnd}" 
                    };
                    // ✅ real_coefficient already set from database in constructor
                }
            }
            else if (!string.IsNullOrEmpty(shiftDetail.checkin_time) && string.IsNullOrEmpty(shiftDetail.checkout_time))
            {
                // Only checkin, no checkout - problematic
                status.color = "#FF0000";
                status.status_color = new List<string> { "#FF0E39", "#FFCFD7" };
                status.name = "";
                status.detail = new List<string> { "Thời gian: 0 giờ" };
            }
            else
            {
                // No checkin/checkout
                status.color = "#666666";
                status.status_color = new List<string> { "#838BA3", "#EBEBEB" };
                status.name = "Chưa vào/ra ca";
                status.detail = new List<string> { "Thời gian: 0 giờ" };
            }

            shiftDetail.status = status;
        }

                /// <summary>
        /// Get day of week assignments for shifts
        /// </summary>
        private Dictionary<int, List<int>> GetShiftDayOfWeekAssignments(List<Ins_ShiftAssignment_User_WorkingDay_GetShifts_Result> shifts)
        {
            var shiftAssignments = new Dictionary<int, List<int>>();
            
            if (!shifts.Any())
                return shiftAssignments;
            
            // Get shift IDs
            var shiftIds = string.Join(",", shifts.Select(s => s.ShiftId).Distinct());
            
            try
            {
                // Query Assignment table for DateOfWeek
                var assignments = DaoFactory.Shift.GetAssignmentDateOfWeekByShiftIds(shiftIds);
                
                foreach (var assignment in assignments)
                {
                    if (!shiftAssignments.ContainsKey(assignment.ShiftID ?? 0))
                    {
                        shiftAssignments[assignment.ShiftID ?? 0] = new List<int>();
                    }
                    shiftAssignments[assignment.ShiftID ?? 0].Add(assignment.DateOfWeek ?? 0);
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"Error getting assignment day of week for shifts: {shiftIds}", ex);
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
            
            // .NET DayOfWeek: Sunday=0, Monday=1, Tuesday=2, Wednesday=3, Thursday=4, Friday=5, Saturday=6
            int dayOfWeek = (int)date.DayOfWeek;
            return dayOfWeekAssignments.Contains(dayOfWeek);
        }

        /// <summary>
        /// Create working day data in bulk for employees and shifts  
        /// ✅ UPDATED: Sử dụng Assignment table để chỉ tạo ca cho những ngày được định nghĩa
        /// </summary>
        private int CreateWorkingDayDataBulk(int companyId, DateTime startDate, DateTime endDate, string employeeIds)
        {
            var employees = DaoFactory.Shift.GetEmployees(companyId, employeeIds);
            var shifts = DaoFactory.Shift.GetShifts(companyId);
            
            if (!employees.Any())
            {
                throw new InvalidOperationException(
                    string.IsNullOrEmpty(employeeIds) 
                        ? "Công ty này chưa có nhân viên nào hoạt động." 
                        : "Không tìm thấy nhân viên nào với ID được chỉ định thuộc công ty này."
                );
            }
            
            if (!shifts.Any())
            {
                throw new InvalidOperationException("Công ty này chưa có ca làm việc nào được kích hoạt.");
            }
            
            // ✅ NEW: Get day of week assignments for each shift
            var shiftDayOfWeekAssignments = GetShiftDayOfWeekAssignments(shifts);

            // ✅ LOGIC ĐÚNG: Chỉ tạo ca cho tháng hiện tại
            var currentDate = DateTime.Now.Date;
            var currentMonthStart = new DateTime(currentDate.Year, currentDate.Month, 1);
            var currentMonthEnd = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
            
            // Tính toán phạm vi effective chỉ trong tháng hiện tại
            var effectiveStartDate = startDate < currentMonthStart ? currentMonthStart : startDate;
            var effectiveEndDate = endDate > currentMonthEnd ? currentMonthEnd : endDate;
            
            // Nếu không có ngày nào trong tháng hiện tại thì không tạo ca
            if (effectiveStartDate > effectiveEndDate)
            {
                return 0;
            }
            
            int totalRecordsCreated = 0;
            
            for (DateTime loopDate = effectiveStartDate; loopDate <= effectiveEndDate; loopDate = loopDate.AddDays(1))
            {
                // ✅ Double-check: Chỉ tạo ca cho ngày thuộc tháng hiện tại
                if (loopDate < currentMonthStart || loopDate > currentMonthEnd)
                {
                    continue;
                }
                
                foreach (var employee in employees)
                {
                    foreach (var shift in shifts)
                    {
                        // ✅ NEW: Check if shift should be created for this day of week
                        var dayOfWeekAssignments = shiftDayOfWeekAssignments.ContainsKey(shift.ShiftId) 
                            ? shiftDayOfWeekAssignments[shift.ShiftId] 
                            : new List<int> { 0, 1, 2, 3, 4, 5, 6 }; // Default: all days
                        
                        if (!ShouldCreateShiftForDate(loopDate, dayOfWeekAssignments))
                        {
                            // Skip this shift for this date - not in Assignment table
                            continue;
                        }
                        
                        try
                        {
                            int recordCreated = DaoFactory.Shift.CreateShiftAssignmentUserWorkingDaySingle(
                                employee.EmployeeId, 
                                shift.ShiftId,
                                loopDate,
                                false  // ✅ CRITICAL: Do NOT reactivate rejected records in Summary API
                            );
                            totalRecordsCreated += recordCreated;
                        }
                        catch (Exception ex)
                        {
                            // Log error but continue with other records
                            CommonLogger.DefaultLogger.Error($"Error creating working day for Employee {employee.EmployeeId}, Shift {shift.ShiftId}, Date {loopDate:yyyy-MM-dd}", ex);
                            continue;
                        }
                    }
                }
            }
            
            return totalRecordsCreated;
        }

        /// <summary>
        /// Parse branches JSON string to list of branch objects
        /// </summary>
        private List<EmployeeBranchObject> ParseBranches(string branchesJson)
        {
            try
            {
                if (!string.IsNullOrEmpty(branchesJson) && branchesJson != "[]")
                {
                    // Parse as JSON array
                    var branches = JsonConvert.DeserializeObject<List<EmployeeBranchObject>>(branchesJson);
                    
                    // Return the parsed branches if valid, otherwise null
                    return (branches != null && branches.Any()) ? branches : null;
                }

                // Return null if no valid branch data
                return null;
            }
            catch (Exception ex)
            {
                // Log error and return null instead of default branch
                CommonLogger.DefaultLogger.Error($"Error parsing branches JSON: {branchesJson}", ex);
                return null;
            }
        }

        /// <summary>
        /// Get employee shift summary with filtering options
        /// </summary>
        public ApiResult<EmployeeShiftSummaryResponse> GetEmployeeShiftSummary(EmployeeShiftSummaryRequest request, int employeeId)
        {
            var response = new ApiResult<EmployeeShiftSummaryResponse>()
            {
                Data = new EmployeeShiftSummaryResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Handle both string and integer employee_shift_id formats
                int? employeeShiftIdInt = null;
                if (!string.IsNullOrEmpty(request.EmployeeShiftId))
                {
                    // Try to parse as integer first (SuwId)
                    if (int.TryParse(request.EmployeeShiftId, out int parsedId))
                    {
                        employeeShiftIdInt = parsedId;
                    }
                }

                DateTime? startDate = null;
                DateTime? endDate = null;

                // When filtering by employee_shift_id, expand date range to find the record
                if (employeeShiftIdInt.HasValue)
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

                string employeeIdsString = null;
                if (request.EmployeeIds != null && request.EmployeeIds.Any())
                {
                    employeeIdsString = string.Join(",", request.EmployeeIds);
                }

                if (startDate > endDate)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.";
                    return response;
                }
                
                // Skip date range validation when filtering by employee_shift_id
                if (!employeeShiftIdInt.HasValue && (endDate.Value - startDate.Value).TotalDays > 7)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Khoảng thời gian không được vượt quá 7 ngày (1 tuần) để tránh ảnh hưởng đến hiệu suất hệ thống.";
                    return response;
                }

                // Skip bulk creation when filtering specific employee_shift_id or is_shift_only mode
                var recordsCreated = 0;
                if (request.IsShiftOnly != 1 && employeeShiftIdInt == null)
                {
                    recordsCreated = CreateWorkingDayDataBulk(request.CompanyId, startDate.Value, endDate.Value, employeeIdsString);
                }

                var summaryData = DaoFactory.Shift.GetShiftAssignmentUserWorkingDaySummary(
                    request.CompanyId,
                    startDate,
                    endDate,
                    employeeIdsString,
                    request.Month > 0 ? request.Month : (int?)null,
                    request.Year > 0 ? request.Year : (int?)null
                );

                
                // Apply employee_shift_id filter if provided (filter by SuwId - user working day ID)
                if (employeeShiftIdInt.HasValue)
                {
                    summaryData = summaryData.Where(x => x.SuwId == employeeShiftIdInt.Value).ToList();
                    
                    if (!summaryData.Any())
                    {
                        response.Code = ResponseResultEnum.NotFound.Value();
                        response.Message = $"Không tìm thấy ca làm việc với ID: {request.EmployeeShiftId}";
                        return response;
                    }
                }

                // Group data by employees  
                var employeeGroups = summaryData
                    .GroupBy(x => new { x.EmployeeId, x.UserId, x.FullName, x.EmployeeCode, x.Phone })
                    .ToList();

                var items = new List<EmployeeShiftItem>();

                foreach (var empGroup in employeeGroups)
                {
                    var employeeItem = new EmployeeShiftItem
                    {
                        user_id = empGroup.Key.UserId.ToString(),
                        employee_id = empGroup.Key.EmployeeId.ToString(),
                        phone = empGroup.Key.Phone ?? "",
                        username = empGroup.Key.Phone ?? "",
                        name = empGroup.Key.FullName ?? "",
                        company_id = request.CompanyId.ToString(),
                        identification = empGroup.Key.EmployeeCode ?? ""
                     };

                     // Group shifts by date
                     var shiftsByDate = empGroup
                         .GroupBy(d => d.WorkingDay.ToString("yyyy-MM-dd HH:mm:ss"))
                         .ToList();

                     foreach (var dateGroup in shiftsByDate)
                     {
                         var dateKey = dateGroup.Key;
                         var shiftsForDate = new List<ShiftDetailItem>();

                         foreach (var shift in dateGroup)
                         {
                            // ✅ NEW: Get time configuration using shared helper
                            var timeConfig = ShiftTimeConfigHelper.GetShiftTimeConfiguration(shift.ShiftId);

                            var shiftDetail = new ShiftDetailItem
                            {
                                id = shift.SuwId.ToString(),
                                name = shift.ShiftName ?? "",
                                shift_key = shift.ShiftKey ?? "",
                                shift_id = shift.ShiftId.ToString() ?? "",  // This is the actual ShiftId from Shift table
                                // ✅ FIXED: Use time config from shared helper instead of hardcode
                                start_time = shift.WorkingDay.ToString("yyyy-MM-dd") + " " + timeConfig.StartTime,
                                end_time = shift.WorkingDay.ToString("yyyy-MM-dd") + " " + timeConfig.EndTime,
                                working_hour = timeConfig.WorkingHour,
                                working_day = shift.WorkingDay.ToString("yyyy-MM-dd HH:mm:ss"),
                                week_of_year = shift.WeekOfYear.GetValueOrDefault() > 0 ? shift.WeekOfYear.Value : 1,
                                company_id = request.CompanyId.ToString(),
                                // ✅ UPDATED: Format checkin/checkout time properly - combine working day with actual time
                                checkin_time = shift.StartCheckInTime.HasValue ? shift.StartCheckInTime.Value.ToString(@"yyyy-MM-dd HH\:mm\:ss") : null,
                                checkout_time = shift.StartCheckOutTime.HasValue ? shift.StartCheckOutTime.Value.ToString(@"yyyy-MM-dd HH\:mm\:ss") : null,
                                shift_name = shift.ShiftName ?? "",
                                real_working_hour = shift.RealWorkingHour.GetValueOrDefault(),
                                real_working_minute = (int)shift.RealWorkingMinute,
                                // ✅ IMPLEMENTED: Set coefficients from database instead of hardcode
                                coefficient = shift.Coefficient,
                                real_coefficient = shift.Coefficient,
                                meal_coefficient = shift.MealCoefficient
                            };

                            // Apply with_branch filter - only include branch info if requested
                            if (request.WithBranch == 1)
                            {
                                shiftDetail.branch_obj = ParseBranches(shift.BranchesJson);
                            }
                            else
                            {
                                shiftDetail.branch_obj = null; // Exclude branch info
                            }

                            // Set display option based on view_mode
                            shiftDetail.display_option = new DisplayOption
                            {
                                shift_name = shift.ShiftName ?? ""
                            };

                            // Apply is_shift_only filter - if set, only include basic shift info
                            if (request.IsShiftOnly != 1)
                            {
                                // Set status based on checkin/checkout
                                SetShiftStatus(shiftDetail, shift);

                                // Set checkin/checkout options if available
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
                            else
                            {
                                // For shift-only mode, set minimal status
                                shiftDetail.status = new ShiftStatus
                                {
                                    color = "#C4C4C4",
                                    status_color = new List<string> { "#838BA3", "#EBEBEB" },
                                    name = "Ca làm việc",
                                    detail = new List<string>()
                                };
                            }

                            // TODO: Add with_project logic when project data is available in database
                            // This would require extending the stored procedure to include project information
                            
                            shiftsForDate.Add(shiftDetail);
                        }

                        employeeItem.shifts[dateKey] = shiftsForDate;
                    }

                    // Calculate totals
                    var allShifts = employeeItem.shifts.SelectMany(x => x.Value);
                    employeeItem.total_working_hour = allShifts.Sum(x => x.working_hour);
                    employeeItem.real_working_hour = allShifts.Sum(x => x.real_working_hour);

                    items.Add(employeeItem);
                }

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
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi hệ thống.";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftSummaryBo.GetEmployeeShiftSummary - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }
    }
} 