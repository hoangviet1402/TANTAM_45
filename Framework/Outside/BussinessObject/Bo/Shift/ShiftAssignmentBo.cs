using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Shift;
using Logger;
using MyUtility.Extensions;
using BussinessObject.Helper;
using MyUtility;
using DataAccess.Model.Shift;

namespace BussinessObject.Bo.Shift
{
    public class ShiftAssignmentBo : BaseBo<DBNull>
    {
        public ShiftAssignmentBo()
            : base(DaoFactory.ShiftAssignment)
        {
        }

        /// <summary>
        /// Reject/delete a shift assignment for a working day
        /// </summary>
        public ApiResult<RejectShiftResponse> RejectShift(RejectShiftRequest request)
        {
            var response = new ApiResult<RejectShiftResponse>()
            {
                Data = new RejectShiftResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate input
                if (request == null || request.Id <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "ID ca làm việc không hợp lệ.";
                    return response;
                }

                if (request.UserId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Mã người dùng không hợp lệ.";
                    return response;
                }

                // Call DAO to reject shift
                var result = DaoFactory.Shift.RejectShift(request.Id, request.UserId);

                if (result != null)
                {
                    response.Data.id = result.SuwId.GetValueOrDefault(0);

                    // Handle WorkingDay formatting
                    if (result.WorkingDay != null)
                    {
                        response.Data.working_day = ((DateTime)result.WorkingDay).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        response.Data.working_day = "";
                    }

                    response.Data.shift_name = result.ShiftName ?? "";

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = result.Message ?? "Từ chối ca làm việc thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Không thể từ chối ca làm việc";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftAssignmentBo.RejectShift - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Register shift for single user with shift_id, working_day, user_id
        /// </summary>
        public ApiResult<RegisterShiftResponse> RegisterShift(RegisterShiftRequest request)
        {
            var response = new ApiResult<RegisterShiftResponse>()
            {
                Data = new RegisterShiftResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate input
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Request không hợp lệ.";
                    return response;
                }

                if (request.shift_id <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Shift ID không hợp lệ.";
                    return response;
                }

                if (request.user_id <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "User ID không hợp lệ.";
                    return response;
                }

                // Parse working day
                DateTime workingDay;
                if (string.IsNullOrEmpty(request.working_day) || !DateTime.TryParse(request.working_day, out workingDay))
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Ngày làm việc không hợp lệ. Định dạng yêu cầu: yyyy-MM-dd";
                    return response;
                }

                // Call simplified RegisterShift stored procedure (which uses CreateSingle internally)
                var result = DaoFactory.Shift.RegisterShift(request.shift_id, workingDay, request.user_id);

                if (result != null)
                {
                    response.Data.total_updated = result.TotalUpdated.GetValueOrDefault();

                    // Handle WorkingDay formatting
                    if (result.WorkingDay != null)
                    {
                        response.Data.working_day = ((DateTime)result.WorkingDay).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        response.Data.working_day = "";
                    }

                    response.Data.shift_name = result.ShiftName ?? "";

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = result.Message ?? "Đăng ký ca làm việc thành công.";
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = "Không thể đăng ký ca làm việc";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftAssignmentBo.RegisterShift - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get list of shifts for user's company on specific working day
        /// Uses new stored procedure that returns shift info with SuwId = NULL for later updates
        /// </summary>
        public ApiResult<ListShiftResponse> GetShiftList(ListShiftRequest request)
        {
            if (request == null)
            {
                return new ApiResult<ListShiftResponse>()
                {
                    Data = new ListShiftResponse(),
                    Code = ResponseResultEnum.InvalidData.Value(),
                    Message = "Request không hợp lệ."
                };
            }

            int userId;
            var userValidationResult = ShiftListHelper.ValidateUserIdInput(request.UserId, out userId);
            if (userValidationResult != null)
            {
                return userValidationResult;
            }

            DateTime workingDay;
            var workingDayValidationResult = ShiftListHelper.ValidateWorkingDayInput(request.WorkingDay, out workingDay);
            if (workingDayValidationResult != null)
            {
                workingDayValidationResult.Message = "Ngày làm việc là bắt buộc và phải hợp lệ (định dạng: yyyy-MM-dd).";
                return workingDayValidationResult;
            }

            return ShiftListHelper.GetShiftListByWorkingDayCommon(userId, workingDay, false);
        }

        /// <summary>
        /// Get shift assignment user working day logs for a specific ShiftAssignment_User_WorkingDay
        /// </summary>
        public ApiResult<object> GetShiftAssignmentUserWorkingDayLogsByEmployeeShift(int employeeShiftId)
        {
            var response = new ApiResult<object>()
            {
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                // Validate input
                if (employeeShiftId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "ID ca làm việc nhân viên không hợp lệ.";
                    return response;
                }

                // Get logs from DAO (no longer pass companyId)
                var logs = DaoFactory.ShiftAssignment.GetShiftAssignmentUserWorkingDayLogsByShiftAssignmentUserWorkingDay(employeeShiftId);

                if (logs != null)
                {
                    // Transform to required format with C# logic
                    var responseData = logs.Select(log =>
                    {
                        // Parse ActionType enum
                        var actionType = ((Shift_ActionType_Enum)log.ActionType).Text();

                        // Parse ClockType enum
                        var clockType = ((Clock_Type_Enum)log.ClockType).Text();

                        // Generate option name using C# logic
                        string optionName = ShiftLabelHelper.GetOptionName(log.ActionType, log.ClockType);

                        // Generate option type name
                        string optionTypeName = clockType.Length > 0 ? clockType[0].ToString().ToUpper() + clockType.Substring(1) : clockType;

                        // Created user info (always present as requested)
                        var createdUser = new
                        {
                            name = log.created_user_name ?? "",
                            username = log.created_user_username ?? "",
                            user_id = log.created_user_id ?? ""
                        };

                        return new
                        {
                            log.time,
                            log_id = log.Id,
                            is_trashed = log.is_trashed ? 1 : 0,
                            log.created_at,
                            log.trashed_at,
                            created_user = createdUser,
                            trashed_user = (object)null, // Always null as requested
                            clock_type = clockType,
                            reason = log.reason ?? "",
                            option = new
                            {
                                type = clockType,
                                name = optionName,
                                type_name = optionTypeName
                            }
                        };
                    }).ToArray();

                    response.Data = responseData;
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Lấy thông tin chấm công thành công";
                }
                else
                {
                    response.Data = new object[0];
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Không có log chấm công nào";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftAssignmentBo.GetShiftAssignmentUserWorkingDayLogsByEmployeeShift - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        public ApiResult<List<ShiftLite_ForRegisterResponse>> ShiftLite_ForRegister(ShiftLite_ForRegisterRequest request, int companyId)
        {
            var response = new ApiResult<List<ShiftLite_ForRegisterResponse>>()
            {
                Data = new List<ShiftLite_ForRegisterResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var currentdate = DateTime.Now;
                // Validate input
                if (request == null)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Request không hợp lệ.";
                    return response;
                }
                var dataHour = DaoFactory.Shift.GetTimes("vn");
                var data = DaoFactory.ShiftAssignment.ShiftAssignment_GetByBranchSimple(request.BranchId).Where(x => x.CompanyID == companyId).ToList();
                if (data == null || data.Any() == false)
                {
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = ResponseResultEnum.NoData.Text();
                    return response;
                }
                response.Data = data.Select(x => new ShiftLite_ForRegisterResponse()
                {
                    Name = x.ShiftName,
                    ShiftKey = x.ShiftKey,
                    ShiftId = x.ShiftAssignmentId,
                    StartTime = new DateTime(
                                              currentdate.Year,
                                              currentdate.Month,
                                              currentdate.Day,
                                              dataHour.FirstOrDefault(z => z.ID == x.StartHourId && z.IsHour == 1).Value ?? 0,
                                              dataHour.FirstOrDefault(z => z.ID == x.StartMinuteId && z.IsHour == 0).Value ?? 0,
                                              0
                                              ).ToString("yyyy-MM-dd HH:mm:ss"),
                    EndTime = new DateTime(
                                              currentdate.Year,
                                              currentdate.Month,
                                              currentdate.Day,
                                              dataHour.FirstOrDefault(z => z.ID == x.EndHourId && z.IsHour == 1).Value ?? 0,
                                              dataHour.FirstOrDefault(z => z.ID == x.EndMinuteId && z.IsHour == 0).Value ?? 0,
                                              0
                                              ).ToString("yyyy-MM-dd HH:mm:ss"),
                    WorkingDay = currentdate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Timezone = x.Timezone
                }).OrderByDescending(x => x.WorkingDay).ToList();

                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftAssignmentBo.ShiftLite_ForRegister - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        public ApiResult<List<HistoryEmployeeShiftResponse>> HistoryEmployeeShift(HistoryEmployeeShiftRequest request, int companyID)
        {
            var response = new ApiResult<List<HistoryEmployeeShiftResponse>>()
            {
                Data = new List<HistoryEmployeeShiftResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };
            try
            {
                var currentdate = DateTime.Now;
                var dateFrom = DateTime.Now;
                var dateTo = DateTime.Now;

                DateTimeExtension.GetStartAndEndDateOfWeek(request.Year, request.WeekOfYear, out dateFrom, out dateTo);

                var dataShift = DaoFactory.ShiftAssignment.ShiftAssignment_GetByBranchSimple(request.BranchId).Where(x => x.CompanyID == companyID).ToList();
                if (dataShift == null || dataShift.Any() == false)
                {
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = ResponseResultEnum.NoData.Text();
                    return response;
                }
                var checkShift = dataShift.FirstOrDefault(x => x.ShiftAssignmentId == request.ShiftID);
                var shiftDateOfWeek = DaoFactory.Shift.GetAssignmentDateOfWeekByShiftId(checkShift.ShiftId);
                if (checkShift == null || checkShift.ShiftId <= 0)
                {
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = ResponseResultEnum.NoData.Text();
                    return response;
                }
                var listEmployerInShift = DaoFactory.ShiftAssignment.ShiftAssignment_GetAllEmployerInShift(request.ShiftID, request.WeekOfYear, dateFrom, dateTo, companyID);
                var dataHour = DaoFactory.Shift.GetTimes("vn");
                var data_date = new HistoryEmployeeShiftResponse();
                for (DateTime date = dateTo; date >= dateFrom; date = date.AddDays(-1))
                {
                    if (shiftDateOfWeek.Any(x => x.DateOfWeek.GetValueOrDefault(0) == date.DayOfWeek.Value()) == true)
                    {
                        data_date = new HistoryEmployeeShiftResponse()
                        {
                            Id = Guid.NewGuid().ToString("N"),
                            Name = checkShift.ShiftName,
                            ShiftKey = checkShift.ShiftKey,
                            ShiftId = checkShift.ShiftId,
                            StartTime = new DateTime(
                                                      date.Year,
                                                      date.Month,
                                                      date.Day,
                                                      dataHour.FirstOrDefault(z => z.ID == checkShift.StartHourId && z.IsHour == 1).Value ?? 0,
                                                      dataHour.FirstOrDefault(z => z.ID == checkShift.StartMinuteId && z.IsHour == 0).Value ?? 0,
                                                      0
                                                      ).ToString("yyyy-MM-dd HH:mm:ss"),
                            EndTime = new DateTime(
                                                      date.Year,
                                                      date.Month,
                                                      date.Day,
                                                      dataHour.FirstOrDefault(z => z.ID == checkShift.EndHourId && z.IsHour == 1).Value ?? 0,
                                                      dataHour.FirstOrDefault(z => z.ID == checkShift.EndMinuteId && z.IsHour == 0).Value ?? 0,
                                                      0
                                                      ).ToString("yyyy-MM-dd HH:mm:ss"),
                            WorkingDay = date.ToString("yyyy-MM-dd HH:mm:ss"),
                            WeekOfYear = request.WeekOfYear,
                            BranchId = checkShift.ShiftAssignmentBranchID,
                            TotalRegister = 0,
                            Timezone = checkShift.Timezone,
                            Employees = listEmployerInShift.Where(x => x.WorkingDay.GetValueOrDefault().GetBeginOfDay() == date)
                                        .Select(x => new HistoryEmployeeShiftResponse_EmployeeInfo()
                                        {
                                            Id = x.PayrollUserID.ToString(),
                                            Name = x.FullName,
                                            Username = string.Format("{0}{1}", x.PhoneCode, x.Phone),
                                            UserId = x.AccountMapID
                                        }).ToList()
                        };
                        response.Data.Add(data_date);
                    }
                }
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftAssignmentBo.HistoryEmployeeShift - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }
            return response;
        }

        public ApiResult<List<ListForAddShiftAssignmentResponse>> ListForAddShiftAssignment(HistoryEmployeeShiftRequest request)
        {


            //?branch_id = 682ef049dc534fa14b0dedf4
            //& is_only_branch = 1
            //& shift_id = 685b9a707b0a6ac42e0894f4
            // & is_quit = 0
            // & working_day = 2025 - 06 - 23
            // & keyword =
            // &filter % 5Bname % 5D =
            //&page = 1
            var response = new ApiResult<List<ListForAddShiftAssignmentResponse>>()
            {
                Data = new List<ListForAddShiftAssignmentResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };
            try
            {
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftAssignmentBo.HistoryEmployeeShift - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }
            return response;
        }

        //companyId, accountId, shift_id, branch_id , is_only_branch, dateFrom
        public ApiResult<List<EmployeesInfo_ForAddShiftResponse>> EmployeesInfo_GetDetailForAddShift(int companyId, int accountId, int shiftID, int branchId, int is_only_branch, DateTime dateFrom)
        {
            var response = new ApiResult<List<EmployeesInfo_ForAddShiftResponse>>()
            {
                Data = new List<EmployeesInfo_ForAddShiftResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                DateTime currentDate = DateTime.Now;
                //if()
                var data = DaoFactory.ShiftAssignment.EmployeesInfo_GetDetailForAddShift(companyId, shiftID, branchId, dateFrom);
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
                if (data != null && data.Any())
                {
                    
                    var shopInfor = DaoFactory.Company.GetCompanyInfo(companyId);
                    foreach (var item in data)
                    {
                        var aa = new EmployeesInfo_ForAddShiftResponse();
                        aa.Id = item.AccountMapID;
                        aa.Name = item.FullName;
                        aa.Phone = item.PhoneFull;
                        aa.Email = item.Email;
                        aa.ShopId = companyId;
                        aa.Shop = shopInfor.FullName;
                        aa.Identification = item.Identification;
                        //aa.UpdatedAt = "";
                        aa.IsTancaPhone = 0;
                        aa.IsTancaEmail = 0;
                        aa.SortIndex = 0;
                        aa.LastActivity = currentDate.ToString("yyyy-MM-dd HH:mm:ss");
                        aa.RegionId = item.RegionID;
                        aa.RegionObj = new RegionObj()
                        {
                            Id = item.RegionID,
                            Name = item.Region
                        };
                        aa.BranchId = item.BranchId;
                        aa.BranchObj = new BranchObj()
                        {
                            Id = item.BranchId,
                            Name = item.BranchName
                        };
                        aa.Position = item.PositionId;
                        aa.PositionObj = new PositionObj()
                        {
                            Id = item.PositionId,
                            Name = item.PositionName
                        };
                        aa.Department = item.DepartmentID;
                        aa.Region = item.Region;
                        aa.Branch = item.BranchName;
                        aa.PayrollConfig = "";
                        aa.Group = item.Role.ToEnum<UserRole>().Text();
                        aa.GroupObj = new GroupObj()
                        {
                            Id = item.Role.GetValueOrDefault(UserRole.Employees.Value()),
                            Name = item.Role.ToEnum<UserRole>().Text()
                        };

                        response.Data.Add(aa);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftAssignmentBo.EmployeesInfo_GetDetailForAddShift - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        public ApiResult<List<ShiftLite_ForRegisterResponse>> EmployeeRegisterShift(EmployeeRegisterShiftRequest request, int companyId, DateTime dateFrom)
        {
            var response = new ApiResult<List<ShiftLite_ForRegisterResponse>>()
            {
                Data = new List<ShiftLite_ForRegisterResponse>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var data = DaoFactory.Shift.Shift_GetSimple(companyId, 0 ).
                            Where(x =>x.CompanyID == companyId && ( x.ShiftAssignmentID == request.shiftAssignmentId || x.ShiftName == request.Shift || x.ShiftKey == request.Shift )).FirstOrDefault();
                if (data != null && data.ShiftId >= 0)
                {
                    var data_hour = DaoFactory.Shift.GetTimes("vn");
                    foreach (var item_UserIds in request.UserIds)
                    {
                        var assignment_user_id = DaoFactory.ShiftAssignment.ShiftAssignment_User_Create(request.shiftAssignmentId, item_UserIds, ShiftAssignment_User_type_Enum.manual.Value());
                        if (assignment_user_id > 0)
                        {
                            DaoFactory.Payroll.Payroll_User_Create_MultiDay(new Payroll_User_CreateMultiDayParameter()
                            {
                                AccountMapID = item_UserIds,
                                AssignmentUserID = assignment_user_id,
                                CheckinType = "",
                                CheckouType = "",
                                StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                    data_hour.FirstOrDefault( x=> x.ID == data.StartHourId && x.IsHour == 1).Value ?? 0, 
                                                    data_hour.FirstOrDefault(x => x.ID == data.StartMinuteId && x.IsHour == 0).Value ?? 0, 0),
                                //EndTime
                                EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                                data_hour.FirstOrDefault(x => x.ID == data.EndHourId && x.IsHour == 1).Value ?? 0,
                                                                data_hour.FirstOrDefault(x => x.ID == data.EndMinuteId && x.IsHour == 0).Value ?? 0, 0),

                                RealCoefficient = 0,
                                RealWorkingHour = 0,
                                RealWorkingMinute = 0,
                                RestEndTimeShort = "",
                                RestStartTimeShort = "",
                                Status = 1,
                                WeekOfYear = dateFrom.GetBeginOfDay().GetWeekNumber(),
                                IsAddPayRollManual = 1
                            },
                                dateFrom.GetBeginOfDay(), dateFrom.EndOfDate()
                            );
                        }

                    }
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = ResponseResultEnum.Success.Text();
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftAssignmentBo.ShiftLite_ForRegister - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        public ApiResult<int> EmployeRrejectShift(int payrollID, int accountMapID)
        {
            var response = new ApiResult<int>()
            {
                Data = 0,
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                response.Data = DaoFactory.Payroll.Payroll_User_UpdateStatus(payrollID, accountMapID,0);
                if (response.Data > 0)
                {
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = ResponseResultEnum.Success.Text();
                }
                else
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = ResponseResultEnum.Failed.Text();
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftAssignmentBo.ShiftLite_ForRegister - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }
    }
}
