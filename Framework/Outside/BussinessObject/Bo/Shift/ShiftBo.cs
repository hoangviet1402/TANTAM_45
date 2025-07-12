using System;
using System.Collections.Generic;
using System.Linq;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.OpenShift;
using BussinessObject.Models.Shift;

using DataAccess;
using DataAccess.Model.Shift;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using Newtonsoft.Json;
using ResxLanguagesUtility;
using ResxLanguagesUtility.Enums;
using EntitiesObject.Entities.TanTamEntities;

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

        public ApiResult<ShiftCreateAndAssignResponse> ShiftCreateAndAssign(ShiftCreateAndAssignRequest request, int companyId, int accountMapID)
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


                var shiftId = DaoFactory.Shift.Shift_Create_Info(shiftParameter);
                shiftParameter.ShiftId = shiftId;
                if (shiftId <= 0)
                {
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Tạo ca làm việc thất bại";
                    return response;
                }
                response.Data.Shift = new ShiftResponse();
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
                if (request.Shift.BranchIds == null || request.Shift.BranchIds.Count == 0)
                {
                    var totalBranchs = 0;
                    var data_companyBranchs =  DaoFactory.Branches.GetAllBranchs(companyId, out totalBranchs);
                    request.Shift.BranchIds = data_companyBranchs.Select(x => x.BranchId).ToList();
                }

                // tạo Shift_Branch theo ID Branch mà CLient truyền lên
                foreach (var item in request.Shift.BranchIds)
                {
                    var shiftBranchCreateed = DaoFactory.Shift.Shift_Branch_Create(new Ins_Shift_Branch_Create_Parameter()
                    {
                        BranchID = item,
                        CompanyID = companyId,
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

                #endregion

                #region tạo ShiftTimeInOutConfig

                var shiftCreateTimeInOutConfig = DaoFactory.Shift.Shift_Create_TimeInOutConfig(shiftParameter);
                var shiftCreateTimeInOutConfig_One = shiftCreateTimeInOutConfig.FirstOrDefault();

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
                if (request.ShiftAssignment.BranchIds == null || request.ShiftAssignment.BranchIds.Count == 0)
                {
                    request.ShiftAssignment.BranchIds = request.Shift.BranchIds;
                }
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
                    }, out assignmentID);
                    response.Data.Branches.AddRange(
                        shiftAssignmentCreateBranch.Select(x => new BranchInfo()
                        {
                            Label = x.BranchName,
                            Value = x.BranchID
                        })
                    );
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
                    request.ShiftAssignment.Assignments = new List<int>() { 0, 1, 1, 1, 1, 1, 0,};
                }

                for (int i = 0; i < request.ShiftAssignment.Assignments.Count; i++)
                {
                    if (request.ShiftAssignment.Assignments[i] == 1)
                    {
                        var createAssignment = DaoFactory.ShiftAssignment.ShiftAssignment_CreateAssignment(new Ins_ShiftAssignment_CreateAssignment_Parameter()
                        {
                            DateOfWeek = i ,
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
                }
                //foreach (var item in request.ShiftAssignment.Assignments)
                //{
                    
                //}
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

                #region tạo ca làm việc chi nhánh được chọn
                if (request.IsOnboarding == 1 && (request.ShiftAssignment.BranchIds == null || request.ShiftAssignment.BranchIds.Any() == false))
                {
                    var total = 0;
                    request.ShiftAssignment.BranchIds = DaoFactory.Branches.GetAllBranchs(companyId, out total).Select(x => x.BranchId).ToList();
                }

                if (request.ShiftAssignment.BranchIds.Count() > 0)
                {
                    // trường hợp không có DepartmentIds và PositionIds thì insert cho toàn bộ nhân viên của chi nhánh
                    if (
                        (request.ShiftAssignment.DepartmentIds == null || request.ShiftAssignment.DepartmentIds.Any() == false) &&
                        (request.ShiftAssignment.PositionIds == null || request.ShiftAssignment.PositionIds.Any() == false)
                      )
                    {
                        foreach (var item in request.ShiftAssignment.BranchIds)
                        {
                            var data = DaoFactory.Branches.EmployeeBranchMap_GetByBranchId(item, companyId, true);
                            foreach (var item_UserIds in data)
                            {
                                var assignment_user_id = DaoFactory.ShiftAssignment.ShiftAssignment_User_Create(shiftAssignmentId, item_UserIds.EmployeeId);
                                if (assignment_user_id > 0)
                                {
                                    DateTime dateFrom, dateTo;

                                    if (shiftAssignmentParameter.GenerateTimekeepingType == Generate_Timekeeping_Type_Obj_Enum.generate_from_start_of_month.Text())
                                    {
                                        DateTimeExtension.GetRangeByType(DateTime.Now, 1, out dateFrom, out dateTo);
                                    }
                                    else
                                    {
                                        DateTimeExtension.GetRangeByType(DateTime.Now, 2, out dateFrom, out dateTo);
                                    }

                                    dateFrom = DateTime.Now.GetBeginOfDay();

                                    DaoFactory.Payroll.Payroll_User_Create_MultiDay(new Payroll_User_CreateMultiDayParameter()
                                    {
                                        AccountMapID = item_UserIds.EmployeeId,
                                        AssignmentUserID = assignment_user_id,
                                        CheckinType = "",
                                        CheckouType = "",
                                        EndTime = response.Data.Shift.EndTime,
                                        StartTime = response.Data.Shift.StartTime,

                                        RealCoefficient = 0,
                                        RealWorkingHour = 0,
                                        RealWorkingMinute = 0,
                                        RestEndTimeShort = "",
                                        RestStartTimeShort = "",
                                        Status = 0,
                                        WeekOfYear = DateTime.Now.GetWeekNumber()
                                    },
                                        dateFrom, dateTo
                                    );
                                }
                            }
                        }
                    }
                    else
                    {
                        if (request.ShiftAssignment.DepartmentIds.Count() > 0)
                        {

                        }

                        if (request.ShiftAssignment.PositionIds.Count() > 0)
                        {

                        }
                    }
                }
                // danh sách chi nhanh 
                #endregion


                #region tạo ca làm việc cho nhân viên hiện tại

                if (request.ShiftAssignment.UserIds != null && request.ShiftAssignment.UserIds.Any())
                {
                    foreach (var item_UserIds in request.ShiftAssignment.UserIds)
                    {
                        var assignment_user_id = DaoFactory.ShiftAssignment.ShiftAssignment_User_Create(shiftAssignmentId, item_UserIds);
                        if (assignment_user_id > 0)
                        {
                            DateTime dateFrom, dateTo;

                            if (shiftAssignmentParameter.GenerateTimekeepingType == Generate_Timekeeping_Type_Obj_Enum.generate_from_start_of_month.Text())
                            {
                                DateTimeExtension.GetRangeByType(DateTime.Now, 1, out dateFrom, out dateTo);
                            }
                            else
                            {
                                DateTimeExtension.GetRangeByType(DateTime.Now, 2, out dateFrom, out dateTo);
                            }

                            dateFrom = DateTime.Now.GetBeginOfDay();

                            DaoFactory.Payroll.Payroll_User_Create_MultiDay(new Payroll_User_CreateMultiDayParameter()
                            {
                                AccountMapID = item_UserIds,
                                AssignmentUserID = assignment_user_id,
                                CheckinType = "",
                                CheckouType = "",
                                EndTime = response.Data.Shift.EndTime,
                                StartTime = response.Data.Shift.StartTime,

                                RealCoefficient = 0,
                                RealWorkingHour = 0,
                                RealWorkingMinute = 0,
                                RestEndTimeShort = "",
                                RestStartTimeShort = "",
                                Status = 0,
                                WeekOfYear = DateTime.Now.GetWeekNumber()
                            },
                                dateFrom, dateTo
                            );
                        }
                    }
                }
                #endregion

                var result = DaoFactory.Company.UpdateCompanyStep(companyId, SetupStepEnum.ONBOARDING_CREATE_SHIFT.Value());
                
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

        /// <summary>
        /// Get list of shift assignments with shift details
        /// </summary>
        public ApiResult<CheckInOutShiftUpdateResponse> UpdateCheckInOut(CheckInOutShiftUpdateRequest request, int userId_check)
        {
            var response = new ApiResult<CheckInOutShiftUpdateResponse>()
            {
                Data = new CheckInOutShiftUpdateResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate request
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Dữ liệu yêu cầu không hợp lệ.";
                    return response;
                }

                // Parse and validate ID
                if (string.IsNullOrEmpty(request.Id) || !int.TryParse(request.Id, out int workingDayId) || workingDayId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "ID ca làm việc không hợp lệ.";
                    return response;
                }

                if (string.IsNullOrEmpty(request.UserId) || !int.TryParse(request.UserId, out int userId) || userId <= 0 || userId == 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "ID nhân viên không hợp lệ.";
                    return response;
                }

                // Validate at least one action is requested
                if (request.IsCheckin == 0 && request.IsCheckout == 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Phải chỉ định ít nhất một hành động check-in hoặc check-out.";
                    return response;
                }

                // Call DAO to update check-in/out
                var result = DaoFactory.Shift.UpdateCheckInOut(
                    workingDayId, 
                    userId,
                    request.CheckinTime, 
                    request.CheckoutTime, 
                    request.IsCheckin == 1, 
                    request.IsCheckout == 1
                );

                if (result != null && result.Success == 1)
                {
                    // Map result to response using snake_case convention
                    response.Data.success = result.Success;
                    response.Data.suw_id = result.SuwId.GetValueOrDefault(0);
                    response.Data.working_day = result.WorkingDay?.ToString("yyyy-MM-dd") ?? "";
                    response.Data.message = result.Message ?? "Cập nhật chấm công thành công";
                    response.Data.is_check_in = result.IsCheckIn.GetValueOrDefault(0) == 1;
                    response.Data.is_check_out = result.IsCheckOut.GetValueOrDefault(0) == 1;
                    response.Data.start_check_in_time = result.StartCheckInTime?.ToString(@"hh\:mm\:ss");
                    response.Data.start_check_out_time = result.StartCheckOutTime?.ToString(@"hh\:mm\:ss");

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = result.Message ?? "Cập nhật thời gian chấm công thành công";

                    // Check-in log
                    if (request.IsCheckin == 1)
                    {
                        var logResult = DaoFactory.ShiftAssignment.CreateShiftAssignmentUserWorkingDayLog(
                            workingDayId,
                            Shift_ActionType_Enum.checkin.Value(), // ActionType: checkin
                            Clock_Type_Enum.admin.Value(), // ClockType: admin
                            DateTime.Now,
                            request.Reason,
                            userId_check
                        );
                    }
                    // Check-out log
                    if (request.IsCheckout == 1)
                    {
                        var logResult = DaoFactory.ShiftAssignment.CreateShiftAssignmentUserWorkingDayLog(
                            workingDayId,
                            Shift_ActionType_Enum.checkout.Value(), // ActionType: checkout
                            Clock_Type_Enum.admin.Value(), // ClockType: admin
                            DateTime.Now,
                            request.Reason,
                            userId_check
                        );
                    }
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = result?.Message ?? "Không thể cập nhật thời gian chấm công";
                }
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                // Handle Entity Framework specific exceptions (from stored procedure errors)
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("ShiftBo.UpdateCheckInOut - EntityCommandExecutionException", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi hệ thống.";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("ShiftBo.UpdateCheckInOut - Error occurred: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }

            return response;
        }

        public ApiResult<UncheckInOutShiftResponse> UncheckInOut(UncheckInOutShiftRequest request,int userId_check)
        {
            var response = new ApiResult<UncheckInOutShiftResponse>()
            {
                Data = new UncheckInOutShiftResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate request
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Dữ liệu yêu cầu không hợp lệ.";
                    return response;
                }

                // Parse and validate ID
                if (string.IsNullOrEmpty(request.Id) || !int.TryParse(request.Id, out int workingDayId) || workingDayId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "ID ca làm việc không hợp lệ.";
                    return response;
                }

                // Parse and validate user_id from request
                if (string.IsNullOrEmpty(request.UserId) || !int.TryParse(request.UserId, out int userId) || userId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "User ID không hợp lệ.";
                    return response;
                }

                // Validate at least one action is requested
                if (request.IsUncheckin == 0 && request.IsUncheckout == 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Phải chỉ định ít nhất một hành động hủy check-in hoặc check-out.";
                    return response;
                }

                // Call DAO to uncheck in/out
                var result = DaoFactory.Shift.UncheckInOut(
                    workingDayId, 
                    userId, 
                    request.IsUncheckin == 1, 
                    request.IsUncheckout == 1, 
                    request.Reason
                );

                if (result != null && result.Success == 1)
                {
                    response.Data.success = result.Success;
                    response.Data.suw_id = result.SuwId.GetValueOrDefault(0);
                    response.Data.working_day = result.WorkingDay?.ToString("yyyy-MM-dd") ?? "";
                    response.Data.message = result.Message ?? "Hủy chấm công thành công";
                    response.Data.is_check_in = result.IsCheckIn.GetValueOrDefault(0) == 1;
                    response.Data.is_check_out = result.IsCheckOut.GetValueOrDefault(0) == 1;
                    response.Data.start_check_in_time = result.StartCheckInTime?.ToString(@"hh\:mm\:ss");
                    response.Data.start_check_out_time = result.StartCheckOutTime?.ToString(@"hh\:mm\:ss");
                    response.Data.reason = result.Reason ?? "";

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = result.Message ?? "Hủy chấm công thành công";

                    // Trash check-in log if uncheckin
                    if (request.IsUncheckin == 1)
                    {
                        // Find latest non-trashed check-in log for this working day
                        var logs = DaoFactory.ShiftAssignment.GetShiftAssignmentUserWorkingDayLogsByShiftAssignmentUserWorkingDay(workingDayId);
                        var logToTrash = logs?.FirstOrDefault(l => l.ActionType == 1 && !l.is_trashed);
                        if (logToTrash != null)
                        {
                            var trashResult = DaoFactory.ShiftAssignment.TrashShiftAssignmentUserWorkingDayLog(logToTrash.Id, userId, request.Reason);
                        }
                        // Create uncheckin log
                        var logResult = DaoFactory.ShiftAssignment.CreateShiftAssignmentUserWorkingDayLog(
                            workingDayId,
                            Shift_ActionType_Enum.uncheckin.Value(), // ActionType: uncheckin
                            Clock_Type_Enum.admin.Value(), // ClockType: admin
                            DateTime.Now,
                            request.Reason,
                            userId_check
                        );
                    }
                    // Trash checkout log if uncheckout
                    if (request.IsUncheckout == 1)
                    {
                        var logs = DaoFactory.ShiftAssignment.GetShiftAssignmentUserWorkingDayLogsByShiftAssignmentUserWorkingDay(workingDayId);
                        var logToTrash = logs?.FirstOrDefault(l => l.ActionType == 2 && !l.is_trashed);
                        if (logToTrash != null)
                        {
                            var trashResult = DaoFactory.ShiftAssignment.TrashShiftAssignmentUserWorkingDayLog(logToTrash.Id, userId, request.Reason);
                        }
                        // Create uncheckout log
                        var logResult = DaoFactory.ShiftAssignment.CreateShiftAssignmentUserWorkingDayLog(
                            workingDayId,
                            Shift_ActionType_Enum.uncheckout.Value(), // ActionType: uncheckout
                            Clock_Type_Enum.admin.Value(), // ClockType: admin
                            DateTime.Now,
                            request.Reason,
                            userId_check
                        );
                    }
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = result?.Message ?? "Không thể hủy chấm công";
                }
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                // Handle Entity Framework specific exceptions (from stored procedure errors)
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("ShiftBo.UncheckInOut - EntityCommandExecutionException", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi hệ thống.";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("ShiftBo.UncheckInOut - Error occurred: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }

            return response;
        }

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
    }
}
