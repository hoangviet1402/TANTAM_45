using BussinessObject.Bo;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Shift;
using DataAccess;
using DataAccess.Entities;
using Logger;
using MyUtility.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BussinessObject.Bo.TanTamBo
{
    public class ShiftBo : BaseBo<DBNull>
    {
        public ShiftBo() : base(DaoFactory.Shift)
        {
        }

        /// <summary>
        /// Get times for shift configuration
        /// </summary>
        public ApiResult<TimesResponse> GetTimes(GetTimesRequest request)
        {
            var response = new ApiResult<TimesResponse>()
            {
                Data = new TimesResponse()
                {
                    Hours = new List<HourResponse>(),
                    Minutes = new List<MinuteResponse>()
                },
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate input
                if (request == null || string.IsNullOrEmpty(request.Lang))
                {
                    request = new GetTimesRequest { Lang = "vi" };
                }

                var timesData = DaoFactory.Shift.GetTimes(request.Lang);
                
                if (timesData != null && timesData.Any())
                {
                    response.Data.Hours = timesData.Where(x => x.IsHour == 1).Select(x => new HourResponse()
                    {
                        Id = x.ID,
                        Name = x.Name,
                        Type = x.Type,
                        Value = x.Value ?? 0
                    }).ToList();

                    response.Data.Minutes = timesData.Where(x => x.IsHour == 0).Select(x => new MinuteResponse()
                    {
                        Id = x.ID,
                        Name = x.Name,
                        Type = x.Type,
                        Value = x.Value ?? 0
                    }).ToList();

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Lấy danh sách thời gian thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.NoData.Value();
                    response.Message = "Không có dữ liệu thời gian";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftBo.GetTimes - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Create shift and assign to employees
        /// </summary>
        public ApiResult<ShiftCreateAndAssignResponse> CreateShiftAndAssign(ShiftCreateAndAssignRequest request, int companyId)
        {
            var response = new ApiResult<ShiftCreateAndAssignResponse>()
            {
                Data = new ShiftCreateAndAssignResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate input
                if (request == null || request.Shift == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Dữ liệu không hợp lệ";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.Shift.Name))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Tên ca làm việc không được để trống";
                    return response;
                }

                if (request.Shift.StartHourId <= 0 || request.Shift.StartMinuteId <= 0 ||
                    request.Shift.EndHourId <= 0 || request.Shift.EndMinuteId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thời gian bắt đầu và kết thúc ca làm việc không hợp lệ";
                    return response;
                }

                if (request.Shift.StartCheckInHourId <= 0 || request.Shift.StartCheckInMinuteId <= 0 ||
                    request.Shift.EndCheckInHourId <= 0 || request.Shift.EndCheckInMinuteId <= 0 ||
                    request.Shift.StartCheckOutHourId <= 0 || request.Shift.StartCheckOutMinuteId <= 0 ||
                    request.Shift.EndCheckOutHourId <= 0 || request.Shift.EndCheckOutMinuteId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thời gian check-in/check-out không hợp lệ";
                    return response;
                }

                if (request.ShiftAssignment == null || string.IsNullOrWhiteSpace(request.ShiftAssignment.Title))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Dữ liệu phân ca không hợp lệ";
                    return response;
                }

                // Create shift parameter
                var shiftParameter = new Ins_Shift_Create_Parameter
                {
                    CompanyID = companyId,
                    Name = request.Shift.Name.Trim(),
                    NameNosign = request.Shift.Name.RemoveSign().Replace(" ", "_").ToUpper(),
                    ShiftKey = request.Shift.Name.RemoveSign().Replace(" ", "_").ToUpper(),
                    StartHourId = request.Shift.StartHourId ?? 0,
                    StartMinuteId = request.Shift.StartMinuteId ?? 0,
                    EndHourId = request.Shift.EndHourId ?? 0,
                    EndMinuteId = request.Shift.EndMinuteId ?? 0,
                    Coefficient = request.Shift.Coefficient ?? 1,
                    MinimumWorkingHour = request.Shift.MinimumWorkingHour ?? 8,
                    Note = request.Shift.Note ?? string.Empty,
                    EarlyCheckOut = request.Shift.EarlyCheckOut ?? 0,
                    LatelyCheckIn = request.Shift.LatelyCheckIn ?? 0,
                    MaxLateCheckInOutMinute = request.Shift.MaxLateCheckInOutMinute,
                    MinSoonCheckInOutMinute = request.Shift.MinSoonCheckInOutMinute,
                    Status = request.Shift.Status ?? 1,
                    Type = request.Shift.Type ?? "shift_working",
                    SortIndex = request.Shift.SortIndex ?? 0,
                    IsOvertimeShift = request.Shift.IsOvertimeShift ?? 0,
                    MealCoefficient = request.Shift.MealCoefficient ?? 0,
                    Timezone = string.IsNullOrEmpty(request.Shift.Timezone) ? "Asia/Bangkok" : request.Shift.Timezone,
                    StartCheckInMinuteId = request.Shift.StartCheckInMinuteId ?? 0,
                    EndCheckInMinuteId = request.Shift.EndCheckInMinuteId ?? 0,
                    StartCheckOutMinuteId = request.Shift.StartCheckOutMinuteId ?? 0,
                    EndCheckOutMinuteId = request.Shift.EndCheckOutMinuteId ?? 0,
                    StartCheckInHourId = request.Shift.StartCheckInHourId ?? 0,
                    EndCheckInHourId = request.Shift.EndCheckInHourId ?? 0,
                    StartCheckOutHourId = request.Shift.StartCheckOutHourId ?? 0,
                    EndCheckOutHourId = request.Shift.EndCheckOutHourId ?? 0
                };

                // Create shift
                var shiftId = DaoFactory.Shift.ShiftCreateInfo(shiftParameter);
                if (shiftId <= 0)
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Tạo ca làm việc thất bại";
                    return response;
                }

                // Set basic shift response data
                response.Data.Shift.Id = shiftId;
                response.Data.Shift.Name = shiftParameter.Name;
                response.Data.Shift.NameNoSign = shiftParameter.NameNosign;
                response.Data.Shift.ShiftKey = shiftParameter.ShiftKey;
                response.Data.Shift.Timezone = shiftParameter.Timezone;
                response.Data.Shift.IsOvertimeShift = shiftParameter.IsOvertimeShift;
                response.Data.Shift.MealCoefficient = (int)shiftParameter.MealCoefficient;
                response.Data.Shift.MinimumWorkingHour = (int)shiftParameter.MinimumWorkingHour ;
                response.Data.Shift.Coefficient = (int)shiftParameter.Coefficient;
                response.Data.Shift.Note = shiftParameter.Note;
                response.Data.Shift.Status = shiftParameter.Status;
                response.Data.Shift.Type = shiftParameter.Type;
                response.Data.Shift.EarlyCheckOut = shiftParameter.EarlyCheckOut;
                response.Data.Shift.LatelyCheckIn = shiftParameter.LatelyCheckIn;
                response.Data.Shift.MaxLateCheckInOutMinute = shiftParameter?.MaxLateCheckInOutMinute ?? 0;
                response.Data.Shift.MinSoonCheckInOutMinute = shiftParameter?.MinSoonCheckInOutMinute ?? 0;
                response.Data.Shift.SortIndex = shiftParameter.SortIndex;

                // Create shift branches
                if (request.Shift.BranchIds != null && request.Shift.BranchIds.Any())
                {
                    foreach (var branchId in request.Shift.BranchIds)
                    {
                        var branchResult = DaoFactory.Shift.Shift_Branch_Create(new Ins_Shift_Branch_Create_Parameter()
                        {
                            BranchID = branchId,
                            CompanyID = companyId,
                            IsInsertOne = true,
                            ShiftID = shiftId
                        });

                        if (branchResult != null && branchResult.Any())
                        {
                            response.Data.Shift.BranchIds.AddRange(
                                branchResult.Select(x => new BranchDetail()
                                {
                                    BranchIdObj = new BranchObject()
                                    {
                                        Color = x.Color,
                                        Id = x.BranchID,
                                        Name = x.BranchName
                                    },
                                    Index = x?.SortIndex ?? 0
                                })
                            );
                        }
                    }
                }

                // Create shift time in/out config
                var timeInOutConfig = DaoFactory.Shift.ShiftCreateTimeInOutConfig(shiftParameter, shiftId);
                if (timeInOutConfig != null && timeInOutConfig.Any())
                {
                    var configData = timeInOutConfig.FirstOrDefault();
                    if (configData != null)
                    {
                        // Set time objects
                        response.Data.Shift.StartHourObj = new TimeObject
                        {
                            Id = configData.StartHourID ?? 0,
                            Name = configData.StartHourName,
                            Type = configData.StartHourType,
                            Value = (configData.StartHourValue ?? 0).ToString()
                        };
                        response.Data.Shift.StartHourId = configData.StartHourID ?? 0;

                        response.Data.Shift.StartMinuteObj = new TimeObject
                        {
                            Id = configData.StartMinuteID ?? 0,
                            Name = configData.StartMinuteName,
                            Type = configData.StartMinuteType,
                            Value = (configData.StartMinuteValue ?? 0).ToString()
                        };
                        response.Data.Shift.StartMinuteId = configData.StartMinuteID ?? 0;

                        response.Data.Shift.EndHourObj = new TimeObject
                        {
                            Id = configData.EndHourID ?? 0,
                            Name = configData.EndHourName,
                            Type = configData.EndHourType,
                            Value = (configData.EndHourValue ?? 0).ToString()
                        };
                        response.Data.Shift.EndHourId = configData.EndHourID ?? 0;

                        response.Data.Shift.EndMinuteObj = new TimeObject
                        {
                            Id = configData.EndMinuteID ?? 0,
                            Name = configData.EndMinuteName,
                            Type = configData.EndMinuteType,
                            Value = (configData.EndMinuteValue ?? 0).ToString()
                        };
                        response.Data.Shift.EndMinuteId = configData.EndMinuteID ?? 0;

                        // Set check in time objects
                        response.Data.Shift.StartCheckInHourObj = new TimeObject
                        {
                            Id = configData.StartCheckInHourID ?? 0,
                            Name = configData.StartCheckInHourName,
                            Type = configData.StartCheckInHourType,
                            Value = (configData.StartCheckInHourValue ?? 0).ToString()
                        };
                        response.Data.Shift.StartCheckInHourId = configData.StartCheckInHourID ?? 0;

                        response.Data.Shift.StartCheckInMinuteObj = new TimeObject
                        {
                            Id = configData.StartCheckInMinuteID ?? 0,
                            Name = configData.StartCheckInMinuteName,
                            Type = configData.StartCheckInMinuteType,
                            Value = (configData.StartCheckInMinuteValue ?? 0).ToString()
                        };
                        response.Data.Shift.StartCheckInMinuteId = configData.StartCheckInMinuteID ?? 0;

                        response.Data.Shift.EndCheckInHourObj = new TimeObject
                        {
                            Id = configData.EndCheckInHourID ?? 0,
                            Name = configData.EndCheckInHourName,
                            Type = configData.EndCheckInHourType,
                            Value = (configData.EndCheckInHourValue ?? 0).ToString()
                        };
                        response.Data.Shift.EndCheckInHourId = configData.EndCheckInHourID ?? 0;

                        response.Data.Shift.EndCheckInMinuteObj = new TimeObject
                        {
                            Id = configData.EndCheckInMinuteID ?? 0,
                            Name = configData.EndCheckInMinuteName,
                            Type = configData.EndCheckInMinuteType,
                            Value = (configData.EndCheckInMinuteValue ?? 0).ToString()
                        };
                        response.Data.Shift.EndCheckInMinuteId = configData.EndCheckInMinuteID ?? 0;

                        // Set check out time objects  
                        response.Data.Shift.StartCheckOutHourObj = new TimeObject
                        {
                            Id = configData.StartCheckOutHourID ?? 0,
                            Name = configData.StartCheckOutHourName,
                            Type = configData.StartCheckOutHourType,
                            Value = (configData.StartCheckOutHourValue ?? 0).ToString()
                        };
                        response.Data.Shift.StartCheckOutHourId = configData.StartCheckOutHourID ?? 0;

                        response.Data.Shift.StartCheckOutMinuteObj = new TimeObject
                        {
                            Id = configData.StartCheckOutMinuteID ?? 0,
                            Name = configData.StartCheckOutMinuteName,
                            Type = configData.StartCheckOutMinuteType,
                            Value = (configData.StartCheckOutMinuteValue ?? 0).ToString()
                        };
                        response.Data.Shift.StartCheckOutMinuteId = configData.StartCheckOutMinuteID ?? 0;

                        response.Data.Shift.EndCheckOutHourObj = new TimeObject
                        {
                            Id = configData.EndCheckOutHourID ?? 0,
                            Name = configData.EndCheckOutHourName,
                            Type = configData.EndCheckOutHourType,
                            Value = (configData.EndCheckOutHourValue ?? 0).ToString()
                        };
                        response.Data.Shift.EndCheckOutHourId = configData.EndCheckOutHourID ?? 0;

                        response.Data.Shift.EndCheckOutMinuteObj = new TimeObject
                        {
                            Id = configData.EndCheckOutMinuteID ?? 0,
                            Name = configData.EndCheckOutMinuteName,
                            Type = configData.EndCheckOutMinuteType,
                            Value = (configData.EndCheckOutMinuteValue ?? 0).ToString()
                        };
                        response.Data.Shift.EndCheckOutMinuteId = configData.EndCheckOutMinuteID ?? 0;

                        // Calculate times
                        var now = DateTime.Now;
                        response.Data.Shift.StartTime = new DateTime(now.Year, now.Month, now.Day,
                            configData.StartHourValue ?? 0, configData.StartMinuteValue ?? 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                        response.Data.Shift.EndTime = new DateTime(now.Year, now.Month, now.Day,
                            configData.EndHourValue ?? 0, configData.EndMinuteValue ?? 0, 0).ToString("yyyy-MM-dd HH:mm:ss");

                        response.Data.Shift.StartCheckInTime = new DateTime(now.Year, now.Month, now.Day,
                            configData.StartCheckInHourValue ?? 0, configData.StartCheckInMinuteValue ?? 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                        response.Data.Shift.EndCheckInTime = new DateTime(now.Year, now.Month, now.Day,
                            configData.EndCheckInHourValue ?? 0, configData.EndCheckInMinuteValue ?? 0, 0).ToString("yyyy-MM-dd HH:mm:ss");

                        response.Data.Shift.StartCheckOutTime = new DateTime(now.Year, now.Month, now.Day,
                            configData.StartCheckOutHourValue ?? 0, configData.StartCheckOutMinuteValue ?? 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                        response.Data.Shift.EndCheckOutTime = new DateTime(now.Year, now.Month, now.Day,
                            configData.EndCheckOutHourValue ?? 0, configData.EndCheckOutMinuteValue ?? 0, 0).ToString("yyyy-MM-dd HH:mm:ss");

                        response.Data.Shift.RestStartTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                        response.Data.Shift.RestEndTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");

                        // Calculate working hours
                        response.Data.Shift.WorkingHour = CalculateWorkingHour(
                            configData.StartHourValue ?? 0,
                            configData.StartMinuteValue ?? 0,
                            configData.EndHourValue ?? 0,
                            configData.EndMinuteValue ?? 0
                        );
                    }
                }

                // Create shift assignment
                var shiftAssignmentParameter = new Ins_ShiftAssignment_Create_Parameter
                {
                    CompanyID = companyId,
                    ShiftID = shiftId,
                    Title = request.ShiftAssignment.Title.Trim(),
                    SortIndex = request.ShiftAssignment.SortIndex,
                    AutoApprove = request.ShiftAssignment.AutoApprove ?? 1,
                    Type = request.ShiftAssignment.Type ?? "shift_assignment",
                    PayrollConfigType = request.ShiftAssignment.PayrollConfigType ?? string.Empty,
                    AssignmentTypeObj = request.ShiftAssignment.AssignmentType ?? "weekly_loop",
                    GenerateTimekeepingTypeObj = request.ShiftAssignment.GenerateTimekeepingType ?? "generate_from_start_of_month"
                };

                var shiftAssignmentId = DaoFactory.Shift.ShiftAssignmentCreate(shiftAssignmentParameter);
                if (shiftAssignmentId <= 0)
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Tạo phân ca thất bại";
                    return response;
                }

                // Set shift assignment response data
                response.Data.Id = shiftAssignmentId;
                response.Data.Title = shiftAssignmentParameter.Title;
                response.Data.Type = shiftAssignmentParameter.Type;
                response.Data.AssignmentType = shiftAssignmentParameter.AssignmentTypeObj;
                response.Data.AutoApprove = shiftAssignmentParameter.AutoApprove;
                response.Data.PayrollConfigType = shiftAssignmentParameter.PayrollConfigType;
                response.Data.SortIndex = shiftAssignmentParameter.SortIndex;
                response.Data.MealCoefficient = 0;

                response.Data.GenerateTimekeepingTypeObj = new TypeObject()
                {
                    Label = "Tháng này",
                    Key = shiftAssignmentParameter.GenerateTimekeepingTypeObj
                };

                response.Data.AssignmentTypeObj = new TypeObject()
                {
                    Label = "Lặp theo tuần",
                    Key = shiftAssignmentParameter.AssignmentTypeObj
                };

                // Create shift assignment branches
                if (request.ShiftAssignment.BranchIds != null && request.ShiftAssignment.BranchIds.Any())
                {
                    foreach (var branchId in request.ShiftAssignment.BranchIds)
                    {
                        var branchResult = DaoFactory.Shift.ShiftAssignment_CreateBranch(new Ins_ShiftAssignment_Branch_Create_Parameter()
                        {
                            BranchID = branchId,
                            CompanyID = companyId,
                            IsInsertOne = true,
                            ShiftAssignmentID = shiftAssignmentId
                        });

                        if (branchResult != null && branchResult.Any())
                        {
                            response.Data.Branches.AddRange(
                                branchResult.Select(x => new BranchInfo()
                                {
                                    Label = x.BranchName,
                                    Value = x.BranchID
                                })
                            );
                        }
                    }
                }

                // Create shift assignment positions
                if (request.ShiftAssignment.PositionIds != null && request.ShiftAssignment.PositionIds.Any())
                {
                    foreach (var positionId in request.ShiftAssignment.PositionIds)
                    {
                        var positionResult = DaoFactory.Shift.ShiftAssignment_CreatePosition(new Ins_ShiftAssignment_Position_Create_Parameter()
                        {
                            PositionID = positionId,
                            CompanyID = companyId,
                            IsInsertOne = true,
                            ShiftAssignmentID = shiftAssignmentId
                        });

                        if (positionResult != null && positionResult.Any())
                        {
                            response.Data.Positions.AddRange(
                                positionResult.Select(x => new PositionInfo()
                                {
                                    Label = x.PositionName,
                                    Value = x.PositionID
                                })
                            );
                        }
                    }
                }

                // Create shift assignment departments
                if (request.ShiftAssignment.DepartmentIds != null && request.ShiftAssignment.DepartmentIds.Any())
                {
                    foreach (var departmentId in request.ShiftAssignment.DepartmentIds)
                    {
                        var departmentResult = DaoFactory.Shift.ShiftAssignment_CreateDepartment(new Ins_ShiftAssignment_Department_Create_Parameter()
                        {
                            DepartmentID = departmentId,
                            CompanyID = companyId,
                            IsInsertOne = true,
                            ShiftAssignmentID = shiftAssignmentId
                        });

                        if (departmentResult != null && departmentResult.Any())
                        {
                            response.Data.Departments.AddRange(
                                departmentResult.Select(x => new DepartmentInfo()
                                {
                                    Label = x.DepartmentName,
                                    Value = x.DepartmentID
                                })
                            );
                        }
                    }
                }

                // Create shift assignment weekly schedule
                if (request.ShiftAssignment.Assignments == null || !request.ShiftAssignment.Assignments.Any())
                {
                    // Default weekly schedule: Mon-Fri work, Sat-Sun off  
                    request.ShiftAssignment.Assignments = new List<int>() { 1, 1, 1, 1, 1, 0, 0 };
                }

                foreach (var dayAssignment in request.ShiftAssignment.Assignments)
                {
                    var assignmentId = DaoFactory.Shift.ShiftAssignment_CreateAssignment(new Ins_ShiftAssignment_CreateAssignment_Parameter()
                    {
                        DateOfWeek = dayAssignment,
                        Label = shiftParameter.Name,
                        ShiftAssignmentID = shiftAssignmentId,
                        ShiftID = shiftId
                    });

                    response.Data.AssignmentObjs.Add(new AssignmentObj()
                    {
                        Key = dayAssignment,
                        Label = dayAssignment == 1 ? shiftParameter.Name : null
                    });
                }

                // Set hard-coded values for demo/testing
                response.Data.Shift.ShiftTypeObj = new ShiftTypeObject
                {
                    Id = 4,
                    Name = "Ca làm việc cố định",
                    Value = "standard_working",
                    Type = "shift_type"
                };
                response.Data.Shift.ShiftTypeId = 4;

                response.Data.Shift.ShopObj = new ShopObject
                {
                    Id = companyId,
                    Name = "Company " + companyId
                };
                response.Data.Shift.ShopId = companyId;

                response.Data.Shift.RestStartHourId = "";
                response.Data.Shift.RestStartMinuteId = "";
                response.Data.Shift.RestEndHourId = "";
                response.Data.Shift.RestEndMinuteId = "";

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Tạo ca làm việc thành công";
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftBo.CreateShiftAndAssign - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get list of employee shifts
        /// </summary>
        public ApiResult<object> GetListEmployeeShift(int companyId, int employeeId)
        {
            var response = new ApiResult<object>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Static JSON exactly like old project
                var jsonString = @"[
                    {
                        ""id"": 6,
                        ""name"": ""Ca hành chính"",
                        ""shift_key"": ""CA_HANH_CHINH"",
                        ""shift_id"": 5,
                        ""shift_type"": ""hard"",
                        ""start_time"": ""2025-06-09 09:00:00"",
                        ""end_time"": ""2025-06-09 17:30:00"",
                        ""working_hour"": 8.5,
                        ""working_day"": ""2025-06-09 00:00:00"",
                        ""week_of_year"": 24,
                        ""branch_id"": 3,
                        ""user_id"": 2,
                        ""checkin_time"": null,
                        ""checkout_time"": null,
                        ""is_confirm"": 1,
                        ""is_overtime_shift"": 0,
                        ""shop_id"": 4,
                        ""meal_coefficient"": 0,
                        ""timezone"": ""Asia/Bangkok"",
                        ""is_open_shift"": 0,
                        ""dynamic_user_id"": null,
                        ""checkin_type"": """",
                        ""checkout_type"": """",
                        ""employees"": [
                            {
                                ""username"": ""+84111111121"",
                                ""name"": ""111111121"",
                                ""user_id"": 2
                            }
                        ],
                        ""clock_status"": ""clock_in"",
                        ""is_active"": true,
                        ""locations"": []
                    }
                ]";

                response.Data = JsonConvert.DeserializeObject(jsonString);
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftBo.GetListEmployeeShift - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get clock in/out status
        /// </summary>
        public ApiResult<object> GetStatusClockInOut(int companyId, int employeeId, int isShowButton)
        {
            var response = new ApiResult<object>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Logic exactly like old project - only this method uses string interpolation
                string clock_type = isShowButton == 0 ? "clock_in" : "clock_out";
                string jsonString;
                
                if (clock_type == "clock_in")
                {
                    jsonString = $@"{{
                        ""clock_setting"": {{
                            ""clock_in_out_requirements"": [""gps""],
                            ""is_location_tracking"": 0,
                            ""distance"": 100,
                            ""debug"": false,
                            ""logLevel"": 5
                        }},
                        ""clock_type"": ""clock_out"",
                        ""current_employee_shift"": {{
                            ""id"": 1,
                            ""name"": ""ca2"",
                            ""shift_key"": ""CA2"",
                            ""shift_id"": 1,
                            ""shift_type"": ""hard"",
                            ""start_time"": ""2025-06-09 08:00:00"",
                            ""end_time"": ""2025-06-09 17:30:00"",
                            ""working_hour"": 9.5,
                            ""working_day"": ""2025-06-09 00:00:00"",
                            ""week_of_year"": 24,
                            ""branch_id"": 1,
                            ""user_id"": 30,
                            ""checkin_time"": null,
                            ""checkout_time"": ""2025-06-09 18:46:00"",
                            ""is_confirm"": 1,
                            ""is_overtime_shift"": 0,
                            ""shop_id"": 1,
                            ""meal_coefficient"": 0,
                            ""timezone"": ""Asia/Saigon"",
                            ""is_open_shift"": 0,
                            ""dynamic_user_id"": null,
                            ""checkin_type"": """",
                            ""checkout_type"": ""mobile"",
                            ""checkout_log_id"": 222,
                            ""checkout_branch_id"": 1,
                            ""is_reason"": 0,
                            ""reason_code"": """"
                        }},
                        ""employee_shifts"": [],
                        ""timekeeper_log"": {{
                            ""id"": 222,
                            ""time"": ""2025-06-09 18:46:41"",
                            ""clock_type"": ""clock_in"",
                            ""employee_shift_id"": 1
                        }}
                    }}";
                }
                else
                {
                    jsonString = $@"{{
                        ""clock_setting"": {{
                            ""clock_in_out_requirements"": [""gps""],
                            ""is_location_tracking"": 0,
                            ""distance"": 100,
                            ""debug"": false,
                            ""logLevel"": 5
                        }},
                        ""clock_type"": ""clock_in"",
                        ""current_employee_shift"": {{
                            ""id"": 1,
                            ""name"": ""Ca Cá Nhân""
                        }},
                        ""employee_shifts"": [
                            {{
                                ""shift"": {{
                                    ""id"": 1,
                                    ""name"": ""ca2"",
                                    ""shift_key"": ""CA2"",
                                    ""shift_id"": 1,
                                    ""shift_type"": ""hard"",
                                    ""start_time"": ""2025-06-09 08:00:00"",
                                    ""end_time"": ""2025-06-09 17:30:00"",
                                    ""working_hour"": 9.5,
                                    ""working_day"": ""2025-06-09 00:00:00"",
                                    ""week_of_year"": 24,
                                    ""branch_id"": 1,
                                    ""user_id"": 30,
                                    ""checkin_time"": null,
                                    ""checkout_time"": null,
                                    ""is_confirm"": 1,
                                    ""is_overtime_shift"": 0,
                                    ""shop_id"": 1,
                                    ""meal_coefficient"": 0,
                                    ""timezone"": ""Asia/Saigon"",
                                    ""is_open_shift"": 0,
                                    ""dynamic_user_id"": null,
                                    ""checkin_type"": """",
                                    ""checkout_type"": """"
                                }},
                                ""is_reason"": 0,
                                ""is_yesterday"": 0,
                                ""is_end_next_day"": 0
                            }}
                        ],
                        ""timekeeper_log"": {{
                            ""id"": 222,
                            ""time"": ""2025-05-29 14:19:08"",
                            ""clock_type"": ""clock_out"",
                            ""employee_shift_id"": 1
                        }}
                    }}";
                }

                response.Data = JsonConvert.DeserializeObject(jsonString);
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftBo.GetStatusClockInOut - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Clock in or out
        /// </summary>
        public ApiResult<object> ClockInOut(ClockInOutShiftRequest request, int companyId)
        {
            var response = new ApiResult<object>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate input
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Dữ liệu không hợp lệ";
                    return response;
                }

                if (request.EmployeeShiftId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Thông tin ca làm việc không hợp lệ";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.ClockType) || 
                    (request.ClockType != "clock_in" && request.ClockType != "clock_out"))
                {
                    response.Code = ResponseResultEnum.InvalidInput.Value();
                    response.Message = "Loại chấm công không hợp lệ";
                    return response;
                }

                // Static JSON exactly like old project 
                string clock_type = request.ClockType;
                string jsonString = "";
                if (clock_type == "clock_out")
                {
                    jsonString = @"{
                    ""next_clock_type"": ""clock_in"",
                    ""current_employee_shift"": {
                        ""id"": ""683412a08fb63932d90cd06b"",
                        ""name"": ""ca2"",
                        ""shift_key"": ""CA2"",
                        ""shift_id"": ""682f0ca6fdeac0f5570d3317"",
                        ""shift_type"": ""hard"",
                        ""start_time"": ""2025-06-09 08:00:00"",
                        ""end_time"": ""2025-06-09 17:30:00"",
                        ""working_hour"": 9.5,
                        ""working_day"": ""2025-06-09 00:00:00"",
                        ""week_of_year"": 24,
                        ""branch_id"": ""682ef049dc534fa14b0dedf4"",
                        ""user_id"": ""682eefe1189d3q7Nz"",
                        ""checkin_time"": null,
                        ""checkout_time"": ""2025-06-09 18:46:00"",
                        ""is_confirm"": 1,
                        ""is_overtime_shift"": 0,
                        ""shop_id"": ""682eefe18cdcf28162021f37"",
                        ""meal_coefficient"": 0,
                        ""timezone"": ""Asia/Saigon"",
                        ""is_open_shift"": 0,
                        ""dynamic_user_id"": null,
                        ""checkin_type"": """",
                        ""checkout_type"": ""mobile"",
                        ""checkout_log_id"": ""6846c9a7fbb1987f350878f3"",
                        ""checkout_branch_id"": ""682ef049dc534fa14b0dedf4""
                    },
                    ""timekeeper_log"": {
                        ""id"": ""6846c9a7fbb1987f350878f3"",
                        ""time"": ""2025-06-09 18:46:47"",
                        ""clock_type"": ""clock_out""
                    }
                    }";
                }
                else
                {
                    jsonString = @"{
                    ""next_clock_type"": ""clock_out"",
                    ""current_employee_shift"": {
                        ""id"": ""683412a08fb63932d90cd06b"",
                        ""name"": ""ca2"",
                        ""shift_key"": ""CA2"",
                        ""shift_id"": ""682f0ca6fdeac0f5570d3317"",
                        ""shift_type"": ""hard"",
                        ""start_time"": ""2025-06-09 08:00:00"",
                        ""end_time"": ""2025-06-09 17:30:00"",
                        ""working_hour"": 9.5,
                        ""working_day"": ""2025-06-09 00:00:00"",
                        ""week_of_year"": 24,
                        ""branch_id"": ""682ef049dc534fa14b0dedf4"",
                        ""user_id"": ""682eefe1189d3q7Nz"",
                        ""checkin_time"": null,
                        ""checkout_time"": ""2025-06-09 18:46:00"",
                        ""is_confirm"": 1,
                        ""is_overtime_shift"": 0,
                        ""shop_id"": ""682eefe18cdcf28162021f37"",
                        ""meal_coefficient"": 0,
                        ""timezone"": ""Asia/Saigon"",
                        ""is_open_shift"": 0,
                        ""dynamic_user_id"": null,
                        ""checkin_type"": """",
                        ""checkout_type"": ""mobile"",
                        ""checkout_log_id"": ""6846c9a7fbb1987f350878f3"",
                        ""checkout_branch_id"": ""682ef049dc534fa14b0dedf4""
                    },
                    ""timekeeper_log"": {
                        ""id"": ""6846c9a7fbb1987f350878f3"",
                        ""time"": ""2025-06-09 18:46:47"",
                        ""clock_type"": ""clock_in""
                    }
                    }";
                }

                response.Data = JsonConvert.DeserializeObject(jsonString);
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftBo.ClockInOut - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get list of shift assignments with shift details
        /// </summary>
        public ApiResult<object> GetListShiftAssignmentWithShift(int companyId, int employeeId)
        {
            var response = new ApiResult<object>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Static JSON exactly like old project
                var jsonString = @"{
                    ""meta"": {
                        ""total"": 2,
                        ""count"": 2,
                        ""per_page"": 15,
                        ""current_page"": 1,
                        ""total_pages"": 1
                    },
                    ""items"": [
                        {
                            ""id"": ""685111f073439356d402af74"",
                            ""title"": ""testea"",
                            ""sort_index"": 0,
                            ""shift"": {
                                ""id"": ""685111f073439356d402af6c"",
                                ""name"": ""testea"",
                                ""shift_key"": ""TESTEA"",
                                ""symbol"": null,
                                ""color"": null,
                                ""sort_index"": 0,
                                ""working_hour"": 9.5,
                                ""start_hour_obj"": {
                                    ""id"": ""5b7e2a2add8e8408c63973a6"",
                                    ""name"": ""08 giờ"",
                                    ""value"": ""8"",
                                    ""type"": ""hour_working""
                                },
                                ""start_minute_obj"": {
                                    ""id"": ""5b7e2eaedd8e8437444429c3"",
                                    ""name"": ""00 phút"",
                                    ""value"": ""0"",
                                    ""type"": ""minute_working""
                                },
                                ""end_hour_obj"": {
                                    ""id"": ""5b7e2a88dd8e840a782810ed"",
                                    ""name"": ""17 giờ"",
                                    ""value"": ""17"",
                                    ""type"": ""hour_working""
                                },
                                ""end_minute_obj"": {
                                    ""id"": ""5b7e2eb9dd8e8408c63973b3"",
                                    ""name"": ""30 phút"",
                                    ""value"": ""30"",
                                    ""type"": ""minute_working""
                                },
                                ""is_overtime_shift"": null,
                                ""timezone"": ""Asia/Saigon""
                            }
                        },
                        {
                            ""id"": ""685112449ce972792b078627"",
                            ""title"": ""newca"",
                            ""sort_index"": 0,
                            ""shift"": {
                                ""id"": ""685112449ce972792b07861f"",
                                ""name"": ""newca"",
                                ""shift_key"": ""NEWCA"",
                                ""symbol"": null,
                                ""color"": null,
                                ""sort_index"": 0,
                                ""working_hour"": 9.5,
                                ""start_hour_obj"": {
                                    ""id"": ""5b7e2a2add8e8408c63973a6"",
                                    ""name"": ""08 giờ"",
                                    ""value"": ""8"",
                                    ""type"": ""hour_working""
                                },
                                ""start_minute_obj"": {
                                    ""id"": ""5b7e2eaedd8e8437444429c3"",
                                    ""name"": ""00 phút"",
                                    ""value"": ""0"",
                                    ""type"": ""minute_working""
                                },
                                ""end_hour_obj"": {
                                    ""id"": ""5b7e2a88dd8e840a782810ed"",
                                    ""name"": ""17 giờ"",
                                    ""value"": ""17"",
                                    ""type"": ""hour_working""
                                },
                                ""end_minute_obj"": {
                                    ""id"": ""5b7e2eb9dd8e8408c63973b3"",
                                    ""name"": ""30 phút"",
                                    ""value"": ""30"",
                                    ""type"": ""minute_working""
                                },
                                ""is_overtime_shift"": null,
                                ""timezone"": ""Asia/Saigon""
                            }
                        }
                    ]
                }";

                response.Data = JsonConvert.DeserializeObject(jsonString);
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftBo.GetListShiftAssignmentWithShift - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get employee shift summary
        /// </summary>
        public ApiResult<object> GetEmployeeShiftSummary(int companyId, int employeeId)
        {
            var response = new ApiResult<object>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Static JSON exactly like old project (truncated version for brevity)
                var jsonString = @"{
                    ""meta"": {
                        ""total"": 7,
                        ""count"": 7,
                        ""per_page"": 10,
                        ""current_page"": 1,
                        ""total_pages"": 1
                    },
                    ""items"": [
                        {
                            ""user_id"": ""6847d5853c45bcBDe"",
                            ""phone"": ""+841231231232"",
                            ""username"": ""+841231231232"",
                            ""name"": ""asdasdsa"",
                            ""shop_id"": ""6847d585ef2efb7c9504f8cf"",
                            ""identification"": ""0001"",
                            ""branch_id"": ""684fc39e16b9f2ea27078acd"",
                            ""branch_obj"": {
                                ""id"": ""684fc39e16b9f2ea27078acd"",
                                ""name"": ""TDT"",
                                ""color"": null
                            },
                            ""payroll"": [],
                            ""shifts"": {
                                ""2025-06-16 00:00:00"": [
                                    {
                                        ""id"": ""684fc8349c887a82af0f4e5c"",
                                        ""name"": ""dasdas"",
                                        ""shift_key"": ""DASDAS"",
                                        ""shift_id"": ""684fc8330f843dbb6a00f2e7"",
                                        ""shift_type"": ""hard"",
                                        ""start_time"": ""2025-06-16 08:00:00"",
                                        ""end_time"": ""2025-06-16 17:30:00"",
                                        ""working_hour"": 9.5,
                                        ""working_day"": ""2025-06-16 00:00:00"",
                                        ""week_of_year"": 25,
                                        ""branch_id"": ""684fc39e16b9f2ea27078acd"",
                                        ""user_id"": ""6847d5853c45bcBDe"",
                                        ""checkin_time"": null,
                                        ""checkout_time"": null,
                                        ""is_confirm"": 1,
                                        ""is_overtime_shift"": 0,
                                        ""shop_id"": ""6847d585ef2efb7c9504f8cf"",
                                        ""meal_coefficient"": 0,
                                        ""timezone"": ""Asia/Saigon"",
                                        ""is_open_shift"": 0,
                                        ""dynamic_user_id"": null,
                                        ""checkin_type"": """",
                                        ""checkout_type"": """",
                                        ""shift_name"": ""dasdas"",
                                        ""display_option"": {
                                            ""shift_name"": ""dasdas"",
                                            ""option_type"": """",
                                            ""option_name"": """"
                                        },
                                        ""real_working_hour"": 0,
                                        ""real_working_minute"": 0,
                                        ""rest_start_time_short"": ""00:00"",
                                        ""rest_end_time_short"": ""00:00"",
                                        ""coefficient"": 1,
                                        ""real_coefficient"": 0,
                                        ""status"": {
                                            ""color"": ""#666666"",
                                            ""status_color"": [
                                                ""#838BA3"",
                                                ""#EBEBEB""
                                            ],
                                            ""name"": ""Chưa vào/ra ca"",
                                            ""detail"": [
                                                ""Thời gian: 0 giờ""
                                            ]
                                        },
                                        ""approved"": false
                                    }
                                ]
                            },
                            ""conflict_shifts"": [],
                            ""total_working_hour"": 57,
                            ""real_working_hour"": 0
                        }
                    ]
                }";

                response.Data = JsonConvert.DeserializeObject(jsonString);
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftBo.GetEmployeeShiftSummary - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get list of open shifts
        /// </summary>
        public ApiResult<object> GetListOpenShift(int companyId, int employeeId, ListOpenShiftRequest request)
        {
            var response = new ApiResult<object>()
            {
                Data = null,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Static JSON exactly like old project
                var jsonString = @"[
                    [
                        {
                            ""id"": ""685112524a36fd9fe7021681"",
                            ""shift_name"": ""newca"",
                            ""total_employees"": 1,
                            ""shift_id"": ""685112449ce972792b07861f"",
                            ""start_time"": ""2025-06-16 08:00"",
                            ""end_time"": ""2025-06-16 17:30"",
                            ""working_day"": ""2025-06-16"",
                            ""timezone"": null,
                            ""is_draft"": false,
                            ""status"": {
                                ""not_available"": 0,
                                ""status_color"": [
                                    ""#838BA3"",
                                    ""#EBEBEB""
                                ]
                            },
                            ""registered_employees"": 0
                        }
                    ],
                    [
                        {
                            ""id"": ""6851121992d25d377b0fa2e7"",
                            ""shift_name"": ""testea"",
                            ""total_employees"": 1,
                            ""shift_id"": ""685111f073439356d402af6c"",
                            ""start_time"": ""2025-06-17 08:00"",
                            ""end_time"": ""2025-06-17 17:30"",
                            ""working_day"": ""2025-06-17"",
                            ""timezone"": null,
                            ""is_draft"": false,
                            ""status"": {
                                ""not_available"": 0,
                                ""status_color"": [
                                    ""#838BA3"",
                                    ""#EBEBEB""
                                ]
                            },
                            ""registered_employees"": 0
                        }
                    ],
                    [],
                    [
                        {
                            ""id"": ""6851125519a953ca2e032231"",
                            ""shift_name"": ""newca"",
                            ""total_employees"": 1,
                            ""shift_id"": ""685112449ce972792b07861f"",
                            ""start_time"": ""2025-06-19 08:00"",
                            ""end_time"": ""2025-06-19 17:30"",
                            ""working_day"": ""2025-06-19"",
                            ""timezone"": null,
                            ""is_draft"": false,
                            ""status"": {
                                ""not_available"": 1,
                                ""status_color"": [
                                    ""#838BA3"",
                                    ""#EBEBEB""
                                ]
                            },
                            ""registered_employees"": 0
                        }
                    ],
                    [
                        {
                            ""id"": ""6851124c097147cb0b09d861"",
                            ""shift_name"": ""newca"",
                            ""total_employees"": 1,
                            ""shift_id"": ""685112449ce972792b07861f"",
                            ""start_time"": ""2025-06-20 08:00"",
                            ""end_time"": ""2025-06-20 17:30"",
                            ""working_day"": ""2025-06-20"",
                            ""timezone"": null,
                            ""is_draft"": false,
                            ""status"": {
                                ""not_available"": 1,
                                ""status_color"": [
                                    ""#838BA3"",
                                    ""#EBEBEB""
                                ]
                            },
                            ""registered_employees"": 0
                        }
                    ],
                    [],
                    []
                ]";

                response.Data = JsonConvert.DeserializeObject(jsonString);
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftBo.GetListOpenShift - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        #region Helper Methods
        private double CalculateWorkingHour(int startHour, int startMinute, int endHour, int endMinute)
        {
            var startTime = new TimeSpan(startHour, startMinute, 0);
            var endTime = new TimeSpan(endHour, endMinute, 0);
            
            if (endTime < startTime)
            {
                // Handle overnight shift
                endTime = endTime.Add(TimeSpan.FromDays(1));
            }
            
            var duration = endTime - startTime;
            return Math.Round(duration.TotalHours, 1);
        }

        private int GetWeekOfYear(DateTime date)
        {
            var culture = CultureInfo.CurrentCulture;
            var calendar = culture.Calendar;
            return calendar.GetWeekOfYear(date, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);
        }
        #endregion
    }
    public static class StringExtensions
    {
        /// <summary>
        /// Removes diacritical marks (accents) from a string.
        /// </summary>
        public static string RemoveSign(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var normalizedString = input.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
} 