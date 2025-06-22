using System;
using System.Collections.Generic;
using System.Linq;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Shift;
using DataAccess;
using DataAccess.Model.Shift;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using ResxLanguagesUtility;
using ResxLanguagesUtility.Enums;

namespace BussinessObject.Bo.Shift
{
    public class ShiftBo : BaseBo<DBNull>
    {
        public ShiftBo()
            : base(DaoFactory.Shift)
        {
        }

        public ApiResult<TimesResponse> GetTimes(string lang)
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
                var total = 0;
                var dataSQL = DaoFactory.Shift.GetTimes(lang);
                response.Data.Hours = dataSQL.Where(x => x.IsHour == 1).Select(x => new HourResponse()
                {
                    Id = x.ID,
                    Name = x.Name,
                    Type = x.Type,
                    Value = x.Value ?? 0
                }).ToList();
                response.Data.Minutes = dataSQL.Where(x => x.IsHour == 0).Select(x => new MinuteResponse()
                {
                    Id = x.ID,
                    Name = x.Name,
                    Type = x.Type,
                    Value = x.Value ?? 0
                }).ToList();
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Lấy danh sách thời gian thành công";
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("GetTimes Exception Lang  EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lấy danh sách thời gian thất bại";
            }

            return response;
        }

        public ApiResult<ShiftCreateAndAssignResponse> ShiftCreateAndAssign(ShiftCreateAndAssignRequest request, int companyId)
        {
            var response = new ApiResult<ShiftCreateAndAssignResponse>()
            {
                Data = new ShiftCreateAndAssignResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                if (request == null || request.Shift == null)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Dữ liệu không hợp lệ";
                    return response;
                }

                if (request.Shift.StartHourId <= 0 || request.Shift.StartMinuteId <= 0 ||
                    request.Shift.EndHourId <= 0 || request.Shift.EndMinuteId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Thời gian bắt đầu và kết thúc ca làm việc không hợp lệ";
                    return response;
                }

                if (request.Shift.StartCheckInHourId <= 0 || request.Shift.StartCheckInMinuteId <= 0 ||
                    request.Shift.EndCheckInHourId <= 0 || request.Shift.EndCheckInMinuteId <= 0 ||
                    request.Shift.StartCheckOutHourId <= 0 || request.Shift.StartCheckOutMinuteId <= 0 ||
                    request.Shift.EndCheckOutHourId <= 0 || request.Shift.EndCheckOutMinuteId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Thời gian check-in/check-out không hợp lệ";
                    return response;
                }

                if (request.ShiftAssignment == null)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Dữ liệu phân ca không hợp lệ";
                    return response;
                }


                var shiftParameter = new Ins_Shift_Create_Parameter
                {
                    CompanyID = companyId, // Có thể lấy từ context hoặc config
                    Name = request.Shift.Name,
                    NameNosign = StringCommon.NormalizeText(request.Shift.Name, " "),
                    ShiftKey = StringCommon.NormalizeText(request.Shift.Name, "_").ToUpper(),
                    StartHourId = request.Shift.StartHourId ?? 0,
                    StartMinuteId = request.Shift.StartMinuteId ?? 0,
                    EndHourId = request.Shift.EndHourId ?? 0,
                    EndMinuteId = request.Shift.EndMinuteId ?? 0,
                    Coefficient = request.Shift.Coefficient ?? 0,
                    MinimumWorkingHour = request.Shift.MinimumWorkingHour ?? 8,
                    Note = request.Shift.Note,
                    EarlyCheckOut = request.Shift.EarlyCheckOut ?? 0,
                    LatelyCheckIn = request.Shift.LatelyCheckIn ?? 0,
                    MaxLateCheckInOutMinute = request.Shift.MaxLateCheckInOutMinute,
                    MinSoonCheckInOutMinute = request.Shift.MinSoonCheckInOutMinute,
                    Status = request.Shift.Status ?? 0,
                    Type = request.Shift.Type,
                    SortIndex = request.Shift.SortIndex ?? 0,
                    IsOvertimeShift = request.Shift.IsOvertimeShift ?? 1,
                    MealCoefficient = request.Shift.MealCoefficient ?? 1,
                    Timezone = string.IsNullOrEmpty(request.Shift.Timezone) ? "Asia/Bangkok" : request.Shift.Timezone,
                    StartCheckInMinuteId = request.Shift.StartCheckInMinuteId ?? 0,
                    EndCheckInMinuteId = request.Shift.EndCheckInMinuteId ?? 0,
                    StartCheckOutMinuteId = request.Shift.StartCheckOutMinuteId ?? 0,
                    EndCheckOutMinuteId = request.Shift.EndCheckOutMinuteId ?? 0,
                    StartCheckInHourId = request.Shift.StartCheckInHourId ?? 0,
                    EndCheckInHourId = request.Shift.EndCheckInHourId ?? 0,
                    StartCheckOutHourId = request.Shift.StartCheckOutHourId ?? 0,
                    EndCheckOutHourId = request.Shift.EndCheckOutHourId ?? 0,
                };


                var shiftId = DaoFactory.Shift.ShiftCreateInfo(shiftParameter);
                if (shiftId <= 0)
                {
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Tạo ca làm việc thất bại";
                    return response;
                }

                response.Data.Shift.Timezone = shiftParameter.Timezone;
                response.Data.Shift.IsOvertimeShift = shiftParameter.IsOvertimeShift;
                response.Data.Shift.MealCoefficient = shiftParameter.MealCoefficient;
                response.Data.Shift.MinimumWorkingHour = shiftParameter.MinimumWorkingHour;
                response.Data.Shift.Id = shiftId;
                response.Data.Shift.Name = shiftParameter.Name;
                response.Data.Shift.NameNoSign = shiftParameter.NameNosign;
                response.Data.Shift.ShiftKey = shiftParameter.ShiftKey;
                response.Data.Shift.ShiftTypeObj = new ShiftTypeObject() {
                    Id = 1,
                    Value = shiftParameter.Type,
                    Name  = ResxLanguages.GetText(shiftParameter.Type, ResxLanguagesEnum.Home),
                    Type  = "shift_type",
                };

                #region tạo Shift_Branch
                response.Data.Shift.BranchIds = new List<BranchDetail>();
                if (request.Shift.BranchIds != null && request.Shift.BranchIds.Count > 0)
                {
                    // tạo Shift_Branch theo ID Branch mà CLient truyền lên
                    foreach (var item in request.Shift.BranchIds)
                    {
                        var shiftBranchCreateed = DaoFactory.Shift.Shift_Branch_Create(new Ins_Shift_Branch_Create_Parameter()
                        {
                            BranchID = item,
                            CompanyID = 0,
                            IsInsertOne = true,
                            ShiftID = shiftId,
                        });
                        if (shiftBranchCreateed != null)
                        {
                            response.Data.Shift.BranchIds.AddRange(
                                shiftBranchCreateed.Select(x => new BranchDetail()
                                {
                                    BranchIdObj = new BranchObject()
                                    {
                                        Color = x.Color,
                                        Id = x.BranchID,
                                        Name = x.BranchName
                                    },
                                    Index = x.SortIndex ?? 0
                                })
                            );
                        }
                    }

                }
                #endregion

                #region tạo ShiftTimeInOutConfig

                var shiftCreateTimeInOutConfig = DaoFactory.Shift.ShiftCreateTimeInOutConfig(shiftParameter);
                var shiftCreateTimeInOutConfig_One = shiftCreateTimeInOutConfig.FirstOrDefault();
                response.Data.Shift.Id = shiftId;
                if (shiftCreateTimeInOutConfig_One != null)
                {

                    response.Data.Shift.StartHourObj = new TimeObject
                    {
                        Id = shiftCreateTimeInOutConfig_One.StartHourID ?? 0,
                        Name = shiftCreateTimeInOutConfig_One.StartHourName,
                        Type = shiftCreateTimeInOutConfig_One.StartHourType,
                        Value = shiftCreateTimeInOutConfig_One.StartHourValue ?? 0,
                    };
                    response.Data.Shift.StartMinuteObj = new TimeObject
                    {
                        Id = shiftCreateTimeInOutConfig_One.StartMinuteID ?? 0,
                        Name = shiftCreateTimeInOutConfig_One.StartMinuteName,
                        Type = shiftCreateTimeInOutConfig_One.StartMinuteType,
                        Value = shiftCreateTimeInOutConfig_One.StartMinuteValue ?? 0,
                    };

                    response.Data.Shift.EndHourObj = new TimeObject
                    {
                        Id = shiftCreateTimeInOutConfig_One.EndHourID ?? 0,
                        Name = shiftCreateTimeInOutConfig_One.EndHourName,
                        Type = shiftCreateTimeInOutConfig_One.EndHourType,
                        Value = shiftCreateTimeInOutConfig_One.EndHourValue ?? 0,
                    };
                    response.Data.Shift.EndMinuteObj = new TimeObject
                    {
                        Id = shiftCreateTimeInOutConfig_One.EndMinuteID ?? 0,
                        Name = shiftCreateTimeInOutConfig_One.EndMinuteName,
                        Type = shiftCreateTimeInOutConfig_One.EndMinuteType,
                        Value = shiftCreateTimeInOutConfig_One.EndMinuteValue ?? 0,
                    };

                    response.Data.Shift.StartCheckInHourObj = new TimeObject
                    {
                        Id = shiftCreateTimeInOutConfig_One.StartCheckInHourID ?? 0,
                        Name = shiftCreateTimeInOutConfig_One.StartCheckInHourName,
                        Type = shiftCreateTimeInOutConfig_One.StartCheckInHourType,
                        Value = shiftCreateTimeInOutConfig_One.StartCheckInHourValue ?? 0,
                    };

                    response.Data.Shift.StartCheckInMinuteObj = new TimeObject
                    {
                        Id = shiftCreateTimeInOutConfig_One.StartCheckInMinuteID ?? 0,
                        Name = shiftCreateTimeInOutConfig_One.StartCheckInMinuteName,
                        Type = shiftCreateTimeInOutConfig_One.StartCheckInMinuteType,
                        Value = shiftCreateTimeInOutConfig_One.StartCheckInMinuteValue ?? 0,
                    };

                    response.Data.Shift.EndCheckInHourObj = new TimeObject
                    {
                        Id = shiftCreateTimeInOutConfig_One.EndCheckInHourID ?? 0,
                        Name = shiftCreateTimeInOutConfig_One.EndCheckInHourName,
                        Type = shiftCreateTimeInOutConfig_One.EndCheckInHourType,
                        Value = shiftCreateTimeInOutConfig_One.EndCheckInHourValue ?? 0,
                    };

                    response.Data.Shift.EndCheckInMinuteObj = new TimeObject
                    {
                        Id = shiftCreateTimeInOutConfig_One.EndCheckInMinuteID ?? 0,
                        Name = shiftCreateTimeInOutConfig_One.EndCheckInMinuteName,
                        Type = shiftCreateTimeInOutConfig_One.EndCheckInMinuteType,
                        Value = shiftCreateTimeInOutConfig_One.EndCheckInMinuteValue ?? 0,
                    };

                    response.Data.Shift.StartCheckOutHourObj = new TimeObject
                    {
                        Id = shiftCreateTimeInOutConfig_One.StartCheckOutHourID ?? 0,
                        Name = shiftCreateTimeInOutConfig_One.StartCheckOutHourName,
                        Type = shiftCreateTimeInOutConfig_One.StartCheckOutHourType,
                        Value = shiftCreateTimeInOutConfig_One.StartCheckOutHourValue ?? 0,
                    };

                    response.Data.Shift.StartCheckOutMinuteObj = new TimeObject
                    {
                        Id = shiftCreateTimeInOutConfig_One.StartCheckOutMinuteID ?? 0,
                        Name = shiftCreateTimeInOutConfig_One.StartCheckOutMinuteName,
                        Type = shiftCreateTimeInOutConfig_One.StartCheckOutMinuteType,
                        Value = shiftCreateTimeInOutConfig_One.StartCheckOutMinuteValue ?? 0,
                    };

                    response.Data.Shift.EndCheckOutHourObj = new TimeObject
                    {
                        Id = shiftCreateTimeInOutConfig_One.EndCheckOutHourID ?? 0,
                        Name = shiftCreateTimeInOutConfig_One.EndCheckOutHourName,
                        Type = shiftCreateTimeInOutConfig_One.EndCheckOutHourType,
                        Value = shiftCreateTimeInOutConfig_One.EndCheckOutHourValue ?? 0,
                    };

                    response.Data.Shift.EndCheckOutMinuteObj = new TimeObject
                    {
                        Id = shiftCreateTimeInOutConfig_One.EndCheckOutMinuteID ?? 0,
                        Name = shiftCreateTimeInOutConfig_One.EndCheckOutMinuteName,
                        Type = shiftCreateTimeInOutConfig_One.EndCheckOutMinuteType,
                        Value = shiftCreateTimeInOutConfig_One.EndCheckOutMinuteValue ?? 0,
                    };

                    //StartTime
                    response.Data.Shift.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                    shiftCreateTimeInOutConfig_One.StartHourValue ?? 0, shiftCreateTimeInOutConfig_One.StartMinuteValue ?? 0, 0);
                    //EndTime
                    response.Data.Shift.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                        shiftCreateTimeInOutConfig_One.EndHourValue ?? 0, shiftCreateTimeInOutConfig_One.EndMinuteValue ?? 0, 0);
                    //StartCheckInTime
                    response.Data.Shift.StartCheckInTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                        shiftCreateTimeInOutConfig_One.StartCheckInHourValue ?? 0, 0, 0);
                    response.Data.Shift.EndCheckInTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                        shiftCreateTimeInOutConfig_One.EndCheckInHourValue ?? 0, 0, 0);
                    //StartCheckOutTime
                    response.Data.Shift.StartCheckOutTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                        shiftCreateTimeInOutConfig_One.StartCheckOutHourValue ?? 0, 0, 0);
                    response.Data.Shift.EndCheckOutTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                        shiftCreateTimeInOutConfig_One.EndCheckOutHourValue ?? 0, 0, 0);

                    //StartCheckOutTime
                    response.Data.Shift.RestStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    response.Data.Shift.RestEndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                    response.Data.Shift.WorkingHour = DateTimeExtension.CalculateWorkingHour(
                       shiftCreateTimeInOutConfig_One.StartHourValue ?? 0,
                       shiftCreateTimeInOutConfig_One.StartMinuteValue ?? 0,
                       shiftCreateTimeInOutConfig_One.EndHourValue ?? 0,
                       shiftCreateTimeInOutConfig_One.EndMinuteValue ?? 0
                    );
                }

                #endregion

                var shiftAssignmentParameter = new Ins_ShiftAssignment_Create_Parameter
                {
                    CompanyID = 1, // Có thể lấy từ context hoặc config
                    ShiftID = shiftId,
                    Title = request.ShiftAssignment.Title,
                    SortIndex = request.ShiftAssignment.SortIndex,
                    AutoApprove = request.ShiftAssignment.AutoApprove ?? 1,
                    Type = request.ShiftAssignment.Type ?? "shift_assignment",
                    PayrollConfigType = request.ShiftAssignment.PayrollConfigType ?? "",
                    AssignmentType = request.ShiftAssignment.AssignmentType ?? "weekly_loop",
                    GenerateTimekeepingType = request.ShiftAssignment.GenerateTimekeepingType,
                };
                var shiftAssignmentId = DaoFactory.ShiftAssignment.ShiftAssignmentCreate(shiftAssignmentParameter);
                if (shiftAssignmentId <= 0)
                {
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Tạo ca làm việc thất bại";
                    return response;
                }
                #region tạo ShiftAssignment_Branch
                response.Data.Branches = new List<BranchInfo>();
                if (request.ShiftAssignment.BranchIds != null && request.ShiftAssignment.BranchIds.Count > 0)
                {
                    // tạo ShiftAssignment_Branch theo ID Branch mà CLient truyền lên store [Ins_ShiftAssignment_Branch_Create]
                    int assignmentID = 0;
                    foreach (var item in request.ShiftAssignment.BranchIds)
                    {
                        var shiftAssignmentCreateBranch = DaoFactory.ShiftAssignment.ShiftAssignment_CreateBranch(new Ins_ShiftAssignment_Branch_Create_Parameter()
                        {
                            BranchID = item,
                            CompanyID = 0,
                            IsInsertOne = true,
                            ShiftAssignmentID = shiftAssignmentId
                        },out assignmentID);
                        response.Data.Branches.AddRange(
                            shiftAssignmentCreateBranch.Select(x => new BranchInfo()
                            {
                                Label = x.BranchName,
                                Value = x.BranchID
                            })
                        );
                    }
                }
                #endregion

                #region tạo ShiftAssignment_Position
                response.Data.Positions = new List<PositionInfo>();
                if (request.ShiftAssignment.PositionIds != null && request.ShiftAssignment.PositionIds.Count > 0)
                {
                    // tạo ShiftAssignment_Branch theo ID Branch mà CLient truyền lên store [Ins_ShiftAssignment_Branch_Create]
                    foreach (var item in request.ShiftAssignment.PositionIds)
                    {
                        var shiftAssignmentCreatePosition = DaoFactory.ShiftAssignment.ShiftAssignment_CreatePosition(new Ins_ShiftAssignment_Position_Create_Parameter()
                        {
                            PositionID = item,
                            CompanyID = 0,
                            IsInsertOne = true,
                            ShiftAssignmentID = shiftAssignmentId
                        });
                        response.Data.Positions.AddRange(
                            shiftAssignmentCreatePosition.Select(x => new PositionInfo()
                            {
                                Label = x.PositionName,
                                Value = x.PositionID
                            })
                        );
                    }
                }

                #endregion

                #region tạo ShiftAssignment_Department
                response.Data.Departments = new List<DepartmentInfo>();
                if (request.ShiftAssignment.DepartmentIds != null && request.ShiftAssignment.DepartmentIds.Count > 0)
                {
                    // tạo ShiftAssignment_Branch theo ID Branch mà CLient truyền lên store [Ins_ShiftAssignment_Branch_Create]
                    foreach (var item in request.ShiftAssignment.DepartmentIds)
                    {
                        var shiftAssignmentCreateDepartment = DaoFactory.ShiftAssignment.ShiftAssignment_CreateDepartment(new Ins_ShiftAssignment_Department_Create_Parameter()
                        {
                            DepartmentID = item,
                            CompanyID = 0,
                            IsInsertOne = true,
                            ShiftAssignmentID = shiftAssignmentId
                        });
                        response.Data.Departments.AddRange(
                            shiftAssignmentCreateDepartment.Select(x => new DepartmentInfo()
                            {
                                Label = x.DepartmentName,
                                Value = x.DepartmentID
                            })
                        );
                    }
                }
                #endregion

                #region tạo ShiftAssignment assignments
                response.Data.AssignmentObjs = new List<AssignmentObj>();
                if (request.ShiftAssignment.Assignments == null || request.ShiftAssignment.Assignments.Any() == false)
                {
                    // mặc định client ko truyền thì lấy theo demo 
                    request.ShiftAssignment.Assignments = new List<int>() { 1, 1, 1, 0, 1, 1, 0 };
                }
                foreach (var item in request.ShiftAssignment.Assignments)
                {
                    var createAssignment = DaoFactory.ShiftAssignment.ShiftAssignment_CreateAssignment(new Ins_ShiftAssignment_CreateAssignment_Parameter()
                    {
                        DateOfWeek = item,
                        Label = shiftAssignmentParameter.Title,
                        ShiftAssignmentID = shiftAssignmentId,
                        ShiftID = shiftId
                    });

                    response.Data.AssignmentObjs.Add(new AssignmentObj()
                    {
                        Key = createAssignment,
                        Label = shiftParameter.Name
                    });
                }
                #endregion

                response.Data.Id = shiftAssignmentId;
                response.Data.Title = shiftAssignmentParameter.Title;
                response.Data.Type = shiftAssignmentParameter.Type;
                response.Data.AssignmentType = shiftAssignmentParameter.AssignmentType;
                response.Data.AutoApprove = shiftAssignmentParameter.AutoApprove;
                response.Data.PayrollConfigType = shiftAssignmentParameter.PayrollConfigType;
                response.Data.SortIndex = shiftAssignmentParameter.SortIndex;
                response.Data.MealCoefficient = 0;
                response.Data.GenerateTimekeepingTypeObj = new TypeObject()
                {
                    Label = "Tháng này",
                    Key = shiftAssignmentParameter.GenerateTimekeepingType
                };
                response.Data.AssignmentTypeObj = new TypeObject()
                {
                    Label = "Lặp theo tuần",
                    Key = shiftAssignmentParameter.AssignmentType
                };

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Tạo ca làm việc thành công";
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("shift ShiftCreate EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Tạo ca làm việc thất bại";
            }

            return response;
        }

        public ApiResult<TimesResponse> ListEmployeeShift(string lang)
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

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Lấy danh sách chi nhánh thành công";
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("shift ListEmployeeShift EX:", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lấy danh sách phòng ban thất bại";
            }

            return response;
        }
    }
}
