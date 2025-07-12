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
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("ShiftAssignmentBo.RejectShift - EntityCommandExecutionException", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi hệ thống.";
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
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("ShiftAssignmentBo.RegisterShift - EntityCommandExecutionException", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi hệ thống.";
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
            var response = new ApiResult<ListShiftResponse>()
            {
                Data = new ListShiftResponse(),
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

                // Parse user ID - REQUIRED
                int userId = 0;
                if (string.IsNullOrEmpty(request.UserId) || !int.TryParse(request.UserId, out userId) || userId <= 0)
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "User ID là bắt buộc và phải hợp lệ.";
                    return response;
                }

                // Parse working day - REQUIRED
                DateTime workingDay;
                if (string.IsNullOrEmpty(request.WorkingDay) || !DateTime.TryParse(request.WorkingDay, out workingDay))
                {
                    response.Code = ResponseResultEnum.InvalidData.Value();
                    response.Message = "Ngày làm việc là bắt buộc và phải hợp lệ (định dạng: yyyy-MM-dd).";
                    return response;
                }

                // Get shifts from new DAO method using Ins_Shift_GetListByUser stored procedure
                var shifts = DaoFactory.Shift.GetShiftListByUser(userId, workingDay);

                if (shifts != null && shifts.Any())
                {
                    foreach (var shift in shifts)
                    {
                        //var shiftKey = string.IsNullOrEmpty(shift.ShiftKey) ? $"SHIFT_{shift.ShiftId}" : shift.ShiftKey;

                        // ✅ NEW: Get time configuration using shared helper
                        //var timeConfig = ShiftTimeConfigHelper.GetShiftTimeConfiguration(shift.ShiftId);

                        var shiftItem = new ShiftListItem
                        {
                            id = shift.SuwId.ToString() ?? "", // SuwId will be NULL from new stored procedure
                            name = shift.ShiftName ?? "",
                            shift_key = shift.ShiftKey,
                            shift_id = shift.ShiftId.ToString(),
                            // ✅ FIXED: Use time config working hour instead of hardcode
                            working_hour = shift.WorkingHour.GetValueOrDefault(),
                            week_of_year = shift.WeekOfYear.GetValueOrDefault(0) > 0 ? shift.WeekOfYear.Value : request.WeekOfYear,
                            branch_id = "",
                            total_register = shift.TotalRegister,
                            is_confirm = null,
                            sort_index = shift.SortIndex,
                            end_working_date = null,
                            timezone = string.IsNullOrEmpty(shift.Timezone) ? "Asia/Saigon" : shift.Timezone
                        };

                        // Format working day - use exact date provided
                        shiftItem.working_day = shift.WorkingDay.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss");

                        // ✅ FIXED: Use time config from database instead of hardcode
                        shiftItem.start_time = shift.StartTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss");
                        shiftItem.end_time = shift.EndTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss");

                        // Use shift_key as dictionary key
                        response.Data[shift.ShiftKey] = shiftItem;
                    }

                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Lấy danh sách ca làm việc thành công";
                }
                else
                {
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "Không có ca làm việc nào cho ngày này";
                }
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("ShiftAssignmentBo.GetShiftList - EntityCommandExecutionException", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi hệ thống.";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftAssignmentBo.GetShiftList - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
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
                        string optionName = GetOptionName(log.ActionType, log.ClockType);
                        
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
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (entityEx.InnerException != null && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx)
                {
                    response.Code = ResponseResultEnum.Failed.Value();
                    response.Message = sqlEx.Message;
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("ShiftAssignmentBo.GetShiftAssignmentUserWorkingDayLogsByEmployeeShift - EntityCommandExecutionException", entityEx);
                    response.Code = ResponseResultEnum.SystemError.Value();
                    response.Message = "Đã xảy ra lỗi hệ thống.";
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

        /// <summary>
        /// Get option name based on action type and clock type
        /// </summary>
        private string GetOptionName(int actionType, int clockType)
        {
            var action = (Shift_ActionType_Enum)actionType;
            var clock = (Clock_Type_Enum)clockType;

            switch (action)
            {
                case Shift_ActionType_Enum.checkin:
                    switch (clock)
                    {
                        case Clock_Type_Enum.admin:
                            return "Vào ca qua chấm công hộ";
                        default:
                            return "Vào ca";
                    }
                case Shift_ActionType_Enum.checkout:
                    switch (clock)
                    {
                        case Clock_Type_Enum.admin:
                            return "Ra ca qua chấm công hộ";
                        default:
                            return "Ra ca";
                    }
                case Shift_ActionType_Enum.uncheckin:
                    return "Hủy vào ca";
                case Shift_ActionType_Enum.uncheckout:
                    return "Hủy ra ca";
                default:
                    return "Khác";
            }
        }

        public ApiResult<List<ShiftLite_ForRegisterResponse>> ShiftLite_ForRegister(ShiftLite_ForRegisterRequest request)
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
                var data = DaoFactory.ShiftAssignment.ShiftAssignment_GetByBranchSimple(request.BranchId);
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
                    ShiftId = x.ShiftId,
                    StartTime  = new DateTime(
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

        public ApiResult<List<HistoryEmployeeShiftResponse>> HistoryEmployeeShift(HistoryEmployeeShiftRequest request)
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

                var dataShift = DaoFactory.ShiftAssignment.ShiftAssignment_GetByBranchSimple(request.BranchId);
                if (dataShift == null || dataShift.Any() == false)
                {
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = ResponseResultEnum.NoData.Text();
                    return response;
                }
                var checkShift = dataShift.FirstOrDefault(x => x.ShiftId == request.ShiftID);
                if (checkShift == null || checkShift.ShiftId <= 0)
                {
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = ResponseResultEnum.NoData.Text();
                    return response;
                }
                var listEmployerInShift = DaoFactory.ShiftAssignment.ShiftAssignment_GetAllEmployerInShift(request.ShiftID, request.WeekOfYear, dateFrom, dateTo);
                var dataHour = DaoFactory.Shift.GetTimes("vn");
                var data_date = new HistoryEmployeeShiftResponse();
                for (DateTime date = dateTo; date > dateFrom; date = date.AddDays(-1))
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
                                        Id = Guid.NewGuid().ToString("N"),
                                        Name = x.FullName,
                                        Username = string.Format("{0}{1}", x.PhoneCode, x.Phone),
                                        UserId = x.AccountMapID
                                    }).ToList()
                    };
                    response.Data.Add(data_date);
                }
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
            }
            catch (Exception ex )
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
    }
}
