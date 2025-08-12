using BussinessObject.Enum;
using BussinessObject.Helper;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Shift;
using DataAccess;
using DataAccess.Model.Shift;
using EntitiesObject.Entities.TanTamEntities;
using Logger;
using MyUtility;
using MyUtility.Extensions;
using ResxLanguagesUtility;
using ResxLanguagesUtility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

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
                var currentDate = DateTime.Now;
                var dataHour = DaoFactory.Shift.GetTimes("vn");
                if (new DateTime(
                                    currentDate.Year,
                                    currentDate.Month,
                                    currentDate.Day,
                                    dataHour.FirstOrDefault(z => z.ID == (request.Shift.StartHourId ?? 0) && z.IsHour == 1).Value ?? 0,
                                    dataHour.FirstOrDefault(z => z.ID == (request.Shift.StartMinuteId ?? 0) && z.IsHour == 0).Value ?? 0,
                                    0
                                    )  >= 
                    new DateTime(
                        currentDate.Year,
                        currentDate.Month,
                        currentDate.Day,
                        dataHour.FirstOrDefault(z => z.ID == (request.Shift.EndHourId ?? 0) && z.IsHour == 1).Value ?? 0,
                        dataHour.FirstOrDefault(z => z.ID == (request.Shift.EndMinuteId ?? 0) && z.IsHour == 0).Value ?? 0,
                        0
                        ))
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Thời gian bắt đầu và kết thúc ca làm việc không hợp lệ";
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

                    StartCheckInHourId = request.Shift.StartCheckInHourId ?? 0,
                    StartCheckInMinuteId = request.Shift.StartCheckInMinuteId ?? 0,
                                        
                    EndCheckInHourId = request.Shift.EndCheckInHourId ?? 0,
                    EndCheckInMinuteId = request.Shift.EndCheckInMinuteId ?? 0,

                    StartCheckOutHourId = request.Shift.StartCheckOutHourId ?? 0,
                    StartCheckOutMinuteId = request.Shift.StartCheckOutMinuteId ?? 0,

                    EndCheckOutMinuteId = request.Shift.EndCheckOutMinuteId ?? 0,                    
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

                // tạo rule 
                DaoFactory.Shift.Shift_TimePenaltyRule_Createdefault(shiftId);

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

                if (request.ShiftAssignment.BranchIds.Count() > 0 && (request.SkipAutoRegisterShift == 0 || request.SkipAutoRegisterShift == null))
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
                                var assignment_user_id = DaoFactory.ShiftAssignment.ShiftAssignment_User_Create(shiftAssignmentId, item_UserIds.EmployeeId, ShiftAssignment_User_type_Enum.auto.Value());
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
                                        Status = 1,
                                        WeekOfYear = DateTime.Now.GetWeekNumber(),
                                        IsAddPayRollManual = 0
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
                        var assignment_user_id = DaoFactory.ShiftAssignment.ShiftAssignment_User_Create(shiftAssignmentId, item_UserIds, ShiftAssignment_User_type_Enum.auto.Value());
                        if (assignment_user_id > 0)
                        {
                            DateTime dateFrom, dateTo;
                            DateTimeExtension.GetRangeByType(DateTime.Now, 2, out dateFrom, out dateTo);
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
                                Status = 1,
                                WeekOfYear = dateFrom.GetWeekNumber(),
                                IsAddPayRollManual = 0
                            },
                                dateFrom.GetBeginOfDay(), dateTo.EndOfDate()
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
                CommonLogger.DefaultLogger.ErrorFormat("shift ShiftCreate EX: {0}", ex);
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
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("ShiftBo.UncheckInOut - Error occurred: {0}", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi trong quá trình xử lý";
            }

            return response;
        }

        public ApiResult<ShiftAssignmentWithShiftResponse> GetListShiftAssignmentWithShift(int companyId, int employeeId, GetListShiftAssignmentWithShiftRequest request)
        {
            var response = new ApiResult<ShiftAssignmentWithShiftResponse>()
            {
                Data = new ShiftAssignmentWithShiftResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate input parameters
                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Company ID không hợp lệ.";
                    return response;
                }

                if (employeeId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Employee ID không hợp lệ.";
                    return response;
                }

                // Use default request if null
                if (request == null)
                {
                    request = new GetListShiftAssignmentWithShiftRequest();
                }

                // Validate page parameters
                if (request.Page <= 0) request.Page = 1;
                if (request.PageSize <= 0 || request.PageSize > 100) request.PageSize = 15;

                // Get shift assignments with shift details from DAO WITHOUT keyword filter first
                var allShiftAssignments = DaoFactory.Shift.GetShiftAssignmentListWithShiftByEmployee(
                    companyId,
                    1, // Get all data first
                    int.MaxValue, // Maximum page size
                    request.Status ?? "active",
                    request.StartHourValue,
                    request.EndHourValue,
                    null // Don't filter by keyword in SQL
                );

                // ✅ VIETNAMESE KEYWORD SEARCH LOGIC using Vietnamese Search Helper
                IEnumerable<Ins_ShiftAssignment_GetListWithShift_Result> filteredShiftAssignments = allShiftAssignments;

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    // Use VietnameseSearchHelper for consistent Vietnamese text search
                    filteredShiftAssignments = VietnameseSearchHelper.FilterByKeyword(
                        allShiftAssignments,
                        request.Keyword,
                        sa => sa.title,           // Search in title (shift assignment title)
                        sa => sa.shift_name,      // Search in shift name
                        sa => sa.shift_key        // Search in shift key
                    );
                }

                // ✅ APPLY PAGINATION AFTER FILTERING
                var totalRecords = filteredShiftAssignments.Count();
                var pagedShiftAssignments = filteredShiftAssignments
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                // Create response structure matching the expected JSON format
                var responseData = new ShiftAssignmentWithShiftResponse();

                if (pagedShiftAssignments != null && pagedShiftAssignments.Any())
                {
                    // Transform data to response format
                    responseData.items = pagedShiftAssignments.Select(sa => new ShiftAssignmentWithShiftItem
                    {
                        id = sa.shift_assignment_id.ToString(),
                        title = sa.title ?? "",
                        sort_index = sa.sort_index,
                        shift = new ShiftDetailForAssignment
                        {
                            id = sa.shift_id.ToString(),
                            name = sa.shift_name ?? "",
                            shift_key = sa.shift_key ?? "",
                            symbol = sa.symbol,
                            color = sa.color,
                            sort_index = sa.shift_sort_index,
                            working_hour = DateTimeExtension.CalculateWorkingHour(sa.start_hour_value, sa.start_minute_value, sa.end_hour_value, sa.end_minute_value),
                            timezone = sa.timezone ?? "Asia/Saigon",
                            is_overtime_shift = sa.is_overtime_shift,
                            start_hour_obj = new TimeObjectInfo
                            {
                                id = sa.start_hour_id.ToString() ?? "",
                                name = sa.start_hour_name ?? "",
                                value = sa.start_hour_value.ToString() ?? "0",
                                type = sa.start_hour_type ?? "hour_working"
                            },
                            start_minute_obj = new TimeObjectInfo
                            {
                                id = sa.start_minute_id.ToString() ?? "",
                                name = sa.start_minute_name ?? "",
                                value = sa.start_minute_value.ToString() ?? "0",
                                type = sa.start_minute_type ?? "minute_working"
                            },
                            end_hour_obj = new TimeObjectInfo
                            {
                                id = sa.end_hour_id.ToString() ?? "",
                                name = sa.end_hour_name ?? "",
                                value = sa.end_hour_value.ToString() ?? "0",
                                type = sa.end_hour_type ?? "hour_working"
                            },
                            end_minute_obj = new TimeObjectInfo
                            {
                                id = sa.end_minute_id.ToString() ?? "",
                                name = sa.end_minute_name ?? "",
                                value = sa.end_minute_value.ToString() ?? "0",
                                type = sa.end_minute_type ?? "minute_working"
                            }
                        }
                    }).ToList();

                    // ✅ PAGINATION INFO with correct total after filtering
                    responseData.meta = new ShiftAssignmentMeta
                    {
                        total = totalRecords,
                        count = responseData.items.Count,
                        per_page = request.PageSize,
                        current_page = request.Page,
                        total_pages = (int)Math.Ceiling((double)totalRecords / request.PageSize)
                    };

                    response.Data = responseData;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = ResponseResultEnum.Success.Text();
                }
                else
                {
                    // Return empty result with proper pagination
                    responseData.items = new List<ShiftAssignmentWithShiftItem>();
                    responseData.meta = new ShiftAssignmentMeta
                    {
                        total = 0,
                        count = 0,
                        per_page = request.PageSize,
                        current_page = request.Page,
                        total_pages = 0
                    };

                    response.Data = responseData;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Không tìm thấy ca làm việc nào.";
                }
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
        /// Get detailed shift assignment with shift information by ID
        /// </summary>
        public ApiResult<ShiftAssignmentDetailResponse> GetShiftAssignmentDetailWithShift(int shiftAssignmentId, int companyId, int employeeId)
        {
            var response = new ApiResult<ShiftAssignmentDetailResponse>()
            {
                Data = new ShiftAssignmentDetailResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate input parameters
                if (shiftAssignmentId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Shift Assignment ID không hợp lệ.";
                    return response;
                }

                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Company ID không hợp lệ.";
                    return response;
                }

                if (employeeId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Employee ID không hợp lệ.";
                    return response;
                }

                // Get shift assignment detail from DAO
                var shiftAssignmentDetail = DaoFactory.Shift.GetShiftAssignmentDetailWithShiftByEmployee(shiftAssignmentId, companyId);

                // Get assignments (7 days of week)
                var assignmentsData = DaoFactory.Shift.GetAssignmentsByShiftAssignmentId(shiftAssignmentId, companyId);

                // Get branches
                var branchesData = DaoFactory.Shift.GetBranchesByShiftAssignmentId(shiftAssignmentId, companyId);

                // Get departments
                var departmentsData = DaoFactory.Shift.GetDepartmentsByShiftAssignmentId(shiftAssignmentId, companyId);

                // Get positions
                var positionsData = DaoFactory.Shift.GetPositionsByShiftAssignmentId(shiftAssignmentId, companyId);

                if (shiftAssignmentDetail != null && shiftAssignmentDetail.Any())
                {
                    var detail = shiftAssignmentDetail.First();

                    // Create assignments array (7 elements: Sunday=0 to Saturday=6)
                    var assignments = new object[7];
                    var assignmentObjs = new List<AssignmentObjectDetail>();

                    if (assignmentsData != null && assignmentsData.Any())
                    {
                        foreach (var assignment in assignmentsData)
                        {
                            var dayOfWeek = assignment.date_of_week;
                            if (dayOfWeek >= 0 && dayOfWeek <= 6)
                            {
                                assignments[dayOfWeek] = assignment.assignment_value;

                                // Add to assignment_objs if has value
                                if (!string.IsNullOrEmpty(assignment.assignment_value))
                                {
                                    assignmentObjs.Add(new AssignmentObjectDetail
                                    {
                                        key = assignment.assignment_value,
                                        label = assignment.assignment_label ?? ""
                                    });
                                }
                            }
                        }
                    }

                    // Create branches array
                    var branches = new List<BranchInfoDetail>();
                    var branchIds = new List<string>();

                    if (branchesData != null && branchesData.Any())
                    {
                        foreach (var branch in branchesData)
                        {
                            branches.Add(new BranchInfoDetail
                            {
                                value = branch.value ?? "",
                                label = branch.label ?? ""
                            });
                            branchIds.Add(branch.value ?? "");
                        }
                    }

                    // Create departments array
                    var departments = new List<DepartmentInfoDetail>();
                    var departmentIds = new List<string>();

                    if (departmentsData != null && departmentsData.Any())
                    {
                        foreach (var department in departmentsData)
                        {
                            departments.Add(new DepartmentInfoDetail
                            {
                                value = department.value ?? "",
                                label = department.label ?? ""
                            });
                            departmentIds.Add(department.value ?? "");
                        }
                    }

                    // Create positions array
                    var positions = new List<PositionInfoDetail>();
                    var positionIds = new List<string>();

                    if (positionsData != null && positionsData.Any())
                    {
                        foreach (var position in positionsData)
                        {
                            positions.Add(new PositionInfoDetail
                            {
                                value = position.value ?? "",
                                label = position.label ?? ""
                            });
                            positionIds.Add(position.value ?? "");
                        }
                    }

                    // Map to response format
                    response.Data = new ShiftAssignmentDetailResponse
                    {
                        id = detail.id.ToString(),
                        title = detail.title ?? "",
                        type = detail.type ?? "shift_assignment",
                        assignment_type = detail.assignment_type ?? "weekly_loop",
                        auto_approve = detail.auto_approve,
                        approver_id = detail.approver_id,
                        user_ids = detail.user_ids,
                        assignments = assignments,
                        assignment_objs = assignmentObjs,
                        branch_ids = branchIds,
                        branches = branches,
                        department_ids = departmentIds,
                        departments = departments,
                        position_ids = positionIds,
                        positions = positions,
                        payroll_config_type = detail.payroll_config_type,
                        sort_index = detail.sort_index,
                        meal_coefficient = detail.meal_coefficient ?? 0,

                        // Generate type objects
                        generate_timekeeping_type_obj = new TypeObjectDetail
                        {
                            label = ShiftLabelHelper.GetGenerateTimekeepingTypeLabel(detail.generate_timekeeping_type),
                            key = detail.generate_timekeeping_type ?? "generate_from_start_of_month"
                        },

                        assignment_type_obj = new TypeObjectDetail
                        {
                            label = ShiftLabelHelper.GetAssignmentTypeLabel(detail.assignment_type),
                            key = detail.assignment_type ?? "weekly_loop"
                        },

                        // Map shift details
                        shift = new ShiftDetailForDetail
                        {
                            id = detail.shift_id.ToString(),
                            name = detail.shift_name ?? "",
                            name_nosign = detail.shift_name_nosign ?? "",
                            shift_key = detail.shift_key ?? "",
                            coefficient = detail.coefficient,
                            note = detail.shift_note ?? "",
                            working_hour = (double)detail.working_hour,
                            status = detail.status,
                            type = detail.shift_working_type ?? "shift_working",
                            sort_index = detail.shift_sort_index,
                            timezone = detail.timezone ?? "Asia/Saigon",
                            is_overtime_shift = detail.is_overtime_shift,
                            color = detail.color,
                            symbol = detail.symbol,
                            meal_coefficient = detail.shift_meal_coefficient,
                            minimum_workinghour = detail.minimum_workinghour,
                            early_check_out = detail.early_check_out,
                            lately_check_in = detail.lately_check_in,
                            max_late_check_in_out_minute = detail.max_late_check_in_out_minute,
                            min_soon_check_in_out_minute = detail.min_soon_check_in_out_minute,

                            // Map time objects
                            start_hour_obj = new TimeObjectDetail
                            {
                                id = detail.start_hour_id.ToString(),
                                name = detail.start_hour_name ?? "",
                                value = detail.start_hour_value.ToString(),
                                type = detail.start_hour_type ?? "hour_working"
                            },

                            start_minute_obj = new TimeObjectDetail
                            {
                                id = detail.start_minute_id.ToString(),
                                name = detail.start_minute_name ?? "",
                                value = detail.start_minute_value.ToString(),
                                type = detail.start_minute_type ?? "minute_working"
                            },

                            end_hour_obj = new TimeObjectDetail
                            {
                                id = detail.end_hour_id.ToString(),
                                name = detail.end_hour_name ?? "",
                                value = detail.end_hour_value.ToString(),
                                type = detail.end_hour_type ?? "hour_working"
                            },

                            end_minute_obj = new TimeObjectDetail
                            {
                                id = detail.end_minute_id.ToString(),
                                name = detail.end_minute_name ?? "",
                                value = detail.end_minute_value.ToString(),
                                type = detail.end_minute_type ?? "minute_working"
                            },

                            // Map check-in time objects
                            start_check_in_hour_obj = new TimeObjectDetail
                            {
                                id = detail.start_check_in_hour_id.ToString(),
                                name = detail.start_check_in_hour_name ?? "",
                                value = detail.start_check_in_hour_value.ToString(),
                                type = detail.start_check_in_hour_type ?? "hour_working"
                            },

                            start_check_in_minute_obj = new TimeObjectDetail
                            {
                                id = detail.start_check_in_minute_id.ToString(),
                                name = detail.start_check_in_minute_name ?? "",
                                value = detail.start_check_in_minute_value.ToString(),
                                type = detail.start_check_in_minute_type ?? "minute_working"
                            },

                            end_check_in_hour_obj = new TimeObjectDetail
                            {
                                id = detail.end_check_in_hour_id.ToString(),
                                name = detail.end_check_in_hour_name ?? "",
                                value = detail.end_check_in_hour_value.ToString(),
                                type = detail.end_check_in_hour_type ?? "hour_working"
                            },

                            end_check_in_minute_obj = new TimeObjectDetail
                            {
                                id = detail.end_check_in_minute_id.ToString(),
                                name = detail.end_check_in_minute_name ?? "",
                                value = detail.end_check_in_minute_value.ToString(),
                                type = detail.end_check_in_minute_type ?? "minute_working"
                            },

                            // Map check-out time objects
                            start_check_out_hour_obj = new TimeObjectDetail
                            {
                                id = detail.start_check_out_hour_id.ToString(),
                                name = detail.start_check_out_hour_name ?? "",
                                value = detail.start_check_out_hour_value.ToString(),
                                type = detail.start_check_out_hour_type ?? "hour_working"
                            },

                            start_check_out_minute_obj = new TimeObjectDetail
                            {
                                id = detail.start_check_out_minute_id.ToString(),
                                name = detail.start_check_out_minute_name ?? "",
                                value = detail.start_check_out_minute_value.ToString(),
                                type = detail.start_check_out_minute_type ?? "minute_working"
                            },

                            end_check_out_hour_obj = new TimeObjectDetail
                            {
                                id = detail.end_check_out_hour_id.ToString(),
                                name = detail.end_check_out_hour_name ?? "",
                                value = detail.end_check_out_hour_value.ToString(),
                                type = detail.end_check_out_hour_type ?? "hour_working"
                            },

                            end_check_out_minute_obj = new TimeObjectDetail
                            {
                                id = detail.end_check_out_minute_id.ToString(),
                                name = detail.end_check_out_minute_name ?? "",
                                value = detail.end_check_out_minute_value.ToString(),
                                type = detail.end_check_out_minute_type ?? "minute_working"
                            },

                            // Map shift type object
                            shift_type_obj = new ShiftTypeObjectDetail
                            {
                                id = "5b7e3224dd8e840a782810fa", // Default ID
                                name = ShiftLabelHelper.GetShiftTypeLabel(detail.shift_type),
                                value = detail.shift_type ?? "standard_working",
                                type = "shift_type"
                            },

                            // Map company object
                            company_obj = new CompanyObjectDetail
                            {
                                id = detail.company_id.ToString(),
                                name = "" // Will be populated if needed
                            },

                            // Set IDs
                            shift_type_id = "5b7e3224dd8e840a782810fa",
                            company_id = detail.company_id.ToString(),
                            start_hour_id = detail.start_hour_id.ToString(),
                            start_minute_id = detail.start_minute_id.ToString(),
                            end_hour_id = detail.end_hour_id.ToString(),
                            end_minute_id = detail.end_minute_id.ToString(),
                            start_check_in_hour_id = detail.start_check_in_hour_id.ToString(),
                            start_check_in_minute_id = detail.start_check_in_minute_id.ToString(),
                            end_check_in_hour_id = detail.end_check_in_hour_id.ToString(),
                            end_check_in_minute_id = detail.end_check_in_minute_id.ToString(),
                            start_check_out_hour_id = detail.start_check_out_hour_id.ToString(),
                            start_check_out_minute_id = detail.start_check_out_minute_id.ToString(),
                            end_check_out_hour_id = detail.end_check_out_hour_id.ToString(),
                            end_check_out_minute_id = detail.end_check_out_minute_id.ToString(),

                            // Set time strings - will be calculated based on working_hour
                            start_time = DateTime.Today.AddHours(detail.start_hour_value).AddMinutes(detail.start_minute_value).ToString("yyyy-MM-dd HH:mm:ss"),
                            end_time = DateTime.Today.AddHours(detail.end_hour_value).AddMinutes(detail.end_minute_value).ToString("yyyy-MM-dd HH:mm:ss"),
                            start_check_in_time = DateTime.Today.AddHours(detail.start_check_in_hour_value).AddMinutes(detail.start_check_in_minute_value).ToString("yyyy-MM-dd HH:mm:ss"),
                            end_check_in_time = DateTime.Today.AddHours(detail.end_check_in_hour_value).AddMinutes(detail.end_check_in_minute_value).ToString("yyyy-MM-dd HH:mm:ss"),
                            start_check_out_time = DateTime.Today.AddHours(detail.start_check_out_hour_value).AddMinutes(detail.start_check_out_minute_value).ToString("yyyy-MM-dd HH:mm:ss"),
                            end_check_out_time = DateTime.Today.AddHours(detail.end_check_out_hour_value).AddMinutes(detail.end_check_out_minute_value).ToString("yyyy-MM-dd HH:mm:ss"),
                            rest_start_time = DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss"),
                            rest_end_time = DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss")
                        }
                    };

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = ResponseResultEnum.Success.Text();
                }
                else
                {
                    response.Code = ResponseResultEnum.NotFound.Value();
                    response.Message = "Không tìm thấy shift assignment hoặc bạn không có quyền truy cập.";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftBo.GetShiftAssignmentDetailWithShift - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }



        public ApiResult<ShiftAssignmentDetailResponse> UpdateShiftAssignmentWithShiftSimplified(ShiftUpdateAndAssignRequest request, int companyId, int employeeId)
        {
            var response = new ApiResult<ShiftAssignmentDetailResponse>
            {
                Data = new ShiftAssignmentDetailResponse(),
                Code = ResponseResultEnum.Success.Value(),
                Message = ResponseResultEnum.Success.Text()
            };

            try
            {
                int shiftAssignmentId = 0;
                int.TryParse(request.Id, out shiftAssignmentId);

                // Helper method to parse IDs from comma-separated string
                var parseIds = new Func<string, List<int>>((ids) =>
                {
                    if (string.IsNullOrEmpty(ids)) return new List<int>();
                    return ids.Split(',').Where(id => !string.IsNullOrEmpty(id)).Select(int.Parse).ToList();
                });

                // Get shift assignment to get shift id
                var shiftAssignmentDetail = DaoFactory.Shift.GetShiftAssignmentDetailWithShiftByEmployee(shiftAssignmentId, companyId);
                if (shiftAssignmentDetail == null || !shiftAssignmentDetail.Any())
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Shift assignment không tồn tại";
                    return response;
                }

                var shiftId = shiftAssignmentDetail.FirstOrDefault()?.shift_id;
                if (shiftId == null)
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Không thể lấy thông tin shift";
                    return response;
                }

                // 1. Update main ShiftAssignment record
                var updateAssignmentResult = DaoFactory.Shift.UpdateShiftAssignmentMain(
                    shiftAssignmentId,
                    companyId,
                    employeeId,
                    request.ShiftAssignment?.Title,
                    request.ShiftAssignment?.AutoApprove,
                    null,
                    request.ShiftAssignment?.PayrollConfigType,
                    request.ShiftAssignment?.AssignmentType,
                    request.ShiftAssignment?.GenerateTimekeepingType,
                    request.ShiftAssignment?.SortIndex,
                    null
                );

                if (updateAssignmentResult == null || !updateAssignmentResult.Any())
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Không thể cập nhật shift assignment";
                    return response;
                }

                // 2. Update main Shift record
                var updateShiftResult = DaoFactory.Shift.UpdateShiftMain(
                    shiftId.Value,
                    companyId,
                    employeeId,
                    request.Shift?.Name,
                    request.Shift?.ShiftKey,
                    request.Shift?.Coefficient,
                    request.Shift?.MinimumWorkingHour,
                    request.Shift?.Note,
                    request.Shift?.EarlyCheckOut,
                    request.Shift?.LatelyCheckIn,
                    request.Shift?.MaxLateCheckInOutMinute,
                    request.Shift?.MinSoonCheckInOutMinute,
                    request.Shift?.Status,
                    request.Shift?.Type,
                    request.Shift?.SortIndex,
                    request.Shift?.IsOvertimeShift,
                    request.Shift?.MealCoefficient,
                    request.Shift?.Timezone
                );

                if (updateShiftResult == null || !updateShiftResult.Any())
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Không thể cập nhật shift";
                    return response;
                }

                // 3. UPDATE SHIFTTIMEINOUTCONFIG
                if (request.Shift != null)
                {
                    var shiftTimeInOutConfigParameter = new Ins_Shift_Create_Parameter
                    {
                        ShiftId = shiftId.Value,
                        StartHourId = request.Shift.StartHourId ?? 0,
                        StartMinuteId = request.Shift.StartMinuteId ?? 0,
                        EndHourId = request.Shift.EndHourId ?? 0,
                        EndMinuteId = request.Shift.EndMinuteId ?? 0,
                        StartCheckInHourId = request.Shift.StartCheckInHourId ?? 0,
                        StartCheckInMinuteId = request.Shift.StartCheckInMinuteId ?? 0,
                        EndCheckInHourId = request.Shift.EndCheckInHourId ?? 0,
                        EndCheckInMinuteId = request.Shift.EndCheckInMinuteId ?? 0,
                        StartCheckOutHourId = request.Shift.StartCheckOutHourId ?? 0,
                        StartCheckOutMinuteId = request.Shift.StartCheckOutMinuteId ?? 0,
                        EndCheckOutHourId = request.Shift.EndCheckOutHourId ?? 0,
                        EndCheckOutMinuteId = request.Shift.EndCheckOutMinuteId ?? 0,
                        MaxLateCheckInOutMinute = request.Shift.MaxLateCheckInOutMinute,
                        MinSoonCheckInOutMinute = request.Shift.MinSoonCheckInOutMinute
                    };

                    // Gọi stored procedure để UPDATE ShiftTimeInOutConfig
                    var shiftUpdateTimeInOutConfig = DaoFactory.Shift.Shift_Create_TimeInOutConfig(shiftTimeInOutConfigParameter);
                    
                    if (shiftUpdateTimeInOutConfig == null || !shiftUpdateTimeInOutConfig.Any())
                    {
                        response.Code = ResponseResultEnum.Failed.Value();
                        response.Message = "Không thể cập nhật cấu hình thời gian ca làm việc";
                        return response;
                    }
                }

                // 4. Update Shift Branches - tái sử dụng stored procedure có sẵn
                if (request.Shift?.BranchIds != null)
                {
                    // Xóa tất cả branches hiện tại
                    DaoFactory.Shift.ClearShiftBranches(shiftId.Value);

                    // Thêm từng branch một sử dụng Shift_Branch_Create
                    foreach (var branchId in request.Shift.BranchIds)
                    {
                        DaoFactory.Shift.Shift_Branch_Create(new Ins_Shift_Branch_Create_Parameter()
                        {
                            ShiftID = shiftId.Value,
                            BranchID = branchId,
                            CompanyID = companyId,
                            IsInsertOne = true
                        });
                    }
                }

                // 5. Update ShiftAssignment Branches
                if (request.ShiftAssignment?.BranchIds != null)
                {
                    // Xóa tất cả branches hiện tại
                    DaoFactory.ShiftAssignment.ClearShiftAssignmentBranches(shiftAssignmentId);

                    // Thêm từng branch một
                    foreach (var branchId in request.ShiftAssignment.BranchIds)
                    {
                        int assignmentId = 0;
                        DaoFactory.ShiftAssignment.ShiftAssignment_CreateBranch(new Ins_ShiftAssignment_Branch_Create_Parameter
                        {
                            ShiftAssignmentID = shiftAssignmentId,
                            BranchID = branchId,
                            CompanyID = companyId,
                            IsInsertOne = true
                        }, out assignmentId);
                    }
                }

                // 6. Update ShiftAssignment Departments
                if (request.ShiftAssignment?.DepartmentIds != null)
                {
                    // Xóa tất cả departments hiện tại
                    DaoFactory.ShiftAssignment.ClearShiftAssignmentDepartments(shiftAssignmentId);

                    // Thêm từng department một
                    foreach (var departmentId in request.ShiftAssignment.DepartmentIds)
                    {
                        DaoFactory.ShiftAssignment.ShiftAssignment_CreateDepartment(new Ins_ShiftAssignment_Department_Create_Parameter
                        {
                            ShiftAssignmentID = shiftAssignmentId,
                            DepartmentID = departmentId,
                            CompanyID = companyId,
                            IsInsertOne = true
                        });
                    }
                }

                // 7. Update ShiftAssignment Positions
                if (request.ShiftAssignment?.PositionIds != null)
                {
                    // Xóa tất cả positions hiện tại
                    DaoFactory.ShiftAssignment.ClearShiftAssignmentPositions(shiftAssignmentId);

                    // Thêm từng position một
                    foreach (var positionId in request.ShiftAssignment.PositionIds)
                    {
                        DaoFactory.ShiftAssignment.ShiftAssignment_CreatePosition(new Ins_ShiftAssignment_Position_Create_Parameter
                        {
                            ShiftAssignmentID = shiftAssignmentId,
                            PositionID = positionId,
                            CompanyID = companyId,
                            IsInsertOne = true
                        });
                    }
                }

                // 8. Update Assignments (7 days of week)
                if (request.ShiftAssignment?.Assignments != null && request.ShiftAssignment.Assignments.Count == 7)
                {
                    // Xóa tất cả assignments hiện tại
                    DaoFactory.ShiftAssignment.ClearAllAssignments(shiftAssignmentId);

                    // Thêm từng assignment một
                    for (int i = 0; i < 7; i++)
                    {
                        if (request.ShiftAssignment.Assignments[i] == 1)
                        {
                            DaoFactory.ShiftAssignment.ShiftAssignment_CreateAssignment(new Ins_ShiftAssignment_CreateAssignment_Parameter
                            {
                                ShiftAssignmentID = shiftAssignmentId,
                                ShiftID = shiftId.Value,
                                DateOfWeek = i,
                                Label = request.ShiftAssignment?.Title
                            });
                        }
                    }
                }

                // Get updated detail
                var detailResult = GetShiftAssignmentDetailWithShift(shiftAssignmentId, companyId, employeeId);
                if (detailResult.Code == ResponseResultEnum.Success.Value())
                {
                    response.Data = detailResult.Data;
                    response.Message = "Cập nhật ca làm việc thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Cập nhật thành công nhưng không thể lấy thông tin chi tiết";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("UpdateShiftAssignmentWithShiftSimplified BO Error", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Đã xảy ra lỗi hệ thống.";
            }

            return response;
        }

        /// <summary>
        /// Delete shift assignment with shift
        /// </summary>
        public ApiResult<DeleteShiftAssignmentResponse> DeleteShiftAssignmentWithShift(int shiftAssignmentId, int companyId, int deletedBy)
        {
            var response = new ApiResult<DeleteShiftAssignmentResponse>()
            {
                Data = new DeleteShiftAssignmentResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate input parameters
                if (shiftAssignmentId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Shift Assignment ID không hợp lệ.";
                    return response;
                }

                if (companyId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Company ID không hợp lệ.";
                    return response;
                }

                if (deletedBy <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Deleted By ID không hợp lệ.";
                    return response;
                }

                // Call DAO to delete shift assignment and shift
                var result = DaoFactory.Shift.DeleteShiftAssignment(shiftAssignmentId, companyId);

                if (result > 0)
                {
                    response.Data.success = 1;
                    response.Data.shift_assignment_id = shiftAssignmentId;
                    response.Data.shift_id = 0; // Not available from stored procedure
                    response.Data.shift_assignment_title = "";
                    response.Data.shift_name = "";
                    response.Data.deleted_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    response.Data.deleted_by = deletedBy;
                    response.Data.message = "Xóa ca làm việc thành công";

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Xóa ca làm việc thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Không thể xóa ca làm việc";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftBo.DeleteShiftAssignmentWithShift - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }
    }
}
