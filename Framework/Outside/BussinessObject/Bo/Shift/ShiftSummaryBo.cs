using BussinessObject.Models.Shift;
using DataAccess;
using Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public void SetShiftStatus(ShiftDetailItem shiftDetail, dynamic shift)
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
                    shiftDetail.real_coefficient = 1;
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
                    shiftDetail.real_coefficient = 1;
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
        /// Create working day data in bulk for employees and shifts
        /// </summary>
        public int CreateWorkingDayDataBulk(int companyId, DateTime startDate, DateTime endDate, string employeeIds)
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
            
            int totalRecordsCreated = 0;
            
            for (DateTime currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
            {
                foreach (var employee in employees)
                {
                    foreach (var shift in shifts)
                    {
                        try
                        {
                            int recordCreated = DaoFactory.Shift.CreateShiftAssignmentUserWorkingDaySingle(
                                employee.EmployeeId, 
                                shift.ShiftId,
                                currentDate
                            );
                            totalRecordsCreated += recordCreated;
                        }
                        catch (Exception ex)
                        {
                            // Log error but continue with other records
                            CommonLogger.DefaultLogger.Error($"Error creating working day for Employee {employee.EmployeeId}, Shift {shift.ShiftId}, Date {currentDate:yyyy-MM-dd}", ex);
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
        public List<EmployeeBranchObject> ParseBranches(string branchesJson)
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
    }
} 