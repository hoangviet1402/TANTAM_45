using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Enum;
using BussinessObject.Helper;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Shift;
using DataAccess;
using DataAccess.Model.Shift;
using EntitiesObject.Entities.TanTamEntities;
using MyUtility;
using MyUtility.Extensions;

namespace BussinessObject.Bo.Shift
{
    public class TimekeeperBo : BaseBo<DBNull>
    {
        public TimekeeperBo()
            : base(DaoFactory.Timekeeper)
        {

        }


        public ApiResult<ListTimekeeperLogReponse> Timekeeper_log_GetListByAccountMapID(int companyID,int accountMapID, DateTime dateFrom, DateTime dateTo)
        {
            var response = new ApiResult<ListTimekeeperLogReponse>()
            {
                Data = new ListTimekeeperLogReponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };
            var data = DaoFactory.Timekeeper.Timekeeper_log_GetListByAccountMapID(companyID,accountMapID, dateFrom, dateTo);
            var data_logChamCongHo = DaoFactory.ShiftAssignment.ShiftAssignment_User_WorkingDay_Log_FromDayToDay(companyID, accountMapID, dateFrom, dateTo);
            var dataName = DaoFactory.Employee.GetEmployeeAccountMapByCompanyId(companyID);
            if (data != null && data.Any())
            {
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();

                response.Data.Meta = new TimekeeperLogMeta()
                {
                    count = data.Count,
                    current_page = 1,
                    per_page = 1,
                    total = data.Count,
                    total_pages = 10000
                };
                response.Data.Items = new List<TimekeeperLog>();
                string clockType = "";
                string optionName = "";
                string optionTypeName = "";
                foreach (var x in data)
                {
                    var log = data_logChamCongHo.FirstOrDefault(c => c.PayrollID == x.PayrollUserID);
                    // Parse ActionType enum
                    //var actionType = log.ActionType.ToEnum<Shift_ActionType_Enum>().Text();
                    if (log != null)
                    {
                        clockType = log.ClockType.ToEnum<Clock_Type_Enum>().Text();
                        optionName = ShiftTimeConfigHelper.GetCheckinActionDescription(log.ActionType, log.ClockType);
                        optionTypeName = clockType.Length > 0 ? clockType[0].ToString().ToUpper() + clockType.Substring(1) : clockType;
                    }

                    var item_asds = new TimekeeperLog()
                    {
                        Id = x.TimekeeperID,
                        ClockType = x.ClockType.ToEnum<Clock_Type_Enum>().Text(),
                        CreatedAt = x.TimekeeperCreateDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                        Time = x.LogTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                        Device = x.TimeKeeperDevice.ToEnum<TimeKeeper_Device_Enum>().Text(),
                        IsCheck = x.IsCheck ?? 1,
                        AccountMapId = accountMapID,
                        Timezone = x.Timezone,
                        PayrollUserID = x.TimekeeperPayrollUserID ?? 0,
                        ConnectionInfo = new Timekeeper_Connection_Info()
                        {
                            ConnectionType = x.ConnectionType.ToEnum<Connection_Type_Enum>().Text(),
                            Bssid = x.Bssid,
                            Ssid = x.Ssid
                        },
                        Location = new Timekeeper_Location_Info()
                        {
                            Latitude = x.Latitude ?? 0,
                            Longitude = x.Longitude,
                            Accuracy = x.Accuracy,
                            Altitude = x.Altitude,
                            AltitudeAccuracy = x.AltitudeAccuracy,
                            Speed = x.Speed,
                            SpeedAccuracy = x.SpeedAccuracy,
                            Course = x.Course,
                            CourseAccuracy = x.CourseAccuracy,
                            Mocked = false
                        },
                        EmployeeShift = new ClockInOut_Shift()
                        {
                            Id = x.PayrollUserID,
                            Name = x.ShiftName,
                            ShiftKey = x.ShiftKey,
                            ShiftId = x.ShiftId,
                            ShiftType = x.ShiftType,
                            StartTime = x.StartTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                            EndTime = x.EndTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                            WorkingHour = x.WorkingHour,
                            WorkingDay = x.WorkingDay.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                            WeekOfYear = x.WeekOfYear,
                            BranchId = x.ShiftAssignmentBranchID,
                            UserId = accountMapID,
                            IsConfirm = x.IsConfirm,
                            IsOvertimeShift = x.IsOvertimeShift,
                            ShopId = x.CompanyID ?? 0,
                            MealCoefficient = x.ShiftAssignmentMealCoefficient,
                            Timezone = x.Timezone,
                            IsOpenShift = x.IsOpenShift,
                            CheckinType = "",
                            CheckoutType = "",
                            CheckoutLogId = x.CheckoutLogId,
                            CheckoutBranchId = x.CheckoutBranchId,
                            CheckinLogId = x.CheckinLogId,
                            CheckinBranchId = x.CheckinBranchId,
                            CheckinBranchObj = new CheckinBranchObj()
                            {
                                Id = x.CheckinBranchId ?? 0,
                                Name = x.CheckinBranchName
                            }
                        },
                        Option = log != null ? new TimekeeperLogOption
                        {
                                Type = clockType,
                                Name = optionName,
                                TypeName = optionTypeName
                        } : null,
                        Name = log != null && string.IsNullOrEmpty(log.created_user_name) == false ?
                            log.created_user_name : 
                            dataName.FirstOrDefault(e => e.Id == x.AccountMapID) != null ? dataName.FirstOrDefault(e => e.Id == x.AccountMapID).FullName : "",
                    };

                    response.Data.Items.Add(item_asds);
                }
                
            }
            else
            {
                response.Code = ResponseResultEnum.NoData.Value();
                response.Message = ResponseResultEnum.NoData.Text();
            }
            return response;
        }

        public ApiResult<ListTimekeeperLogReponse> Timekeeper_log_GetListByAccountMapID_v2(
            int companyID,
            int accountMapID,
            int employeeShiftId
        )
        {
            var response = new ApiResult<ListTimekeeperLogReponse>()
            {
                Data = new ListTimekeeperLogReponse()
                {
                    Items = new List<TimekeeperLog>(),
                    Meta = new TimekeeperLogMeta(),
                },
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text(),
            };

            var userInfor = DaoFactory.User.GetUserDetail(accountMapID, companyID);
            CreatedUser createdUser1 = new CreatedUser
            {
                Name = userInfor?.FullName ?? "",
                UserId = userInfor?.UserId ?? 0,
                Username = userInfor?.PhoneFull ?? ""
            };
            var data = DaoFactory.Timekeeper.Timekeeper_log_GetListByAccountMapID_ByPayrollUserID(companyID, userInfor.EmployeeAccountMapId, employeeShiftId);
            var hasTimekeeperData = data != null && data.Any();

            if (hasTimekeeperData)
            {
                response.Data.Items = data.Where(x => x.TimekeeperPayrollUserID == employeeShiftId)
                    .Select(x => new TimekeeperLog
                    {
                        Id = x?.TimekeeperID ?? 0,
                        ClockType = "mobile",
                        CreatedAt = x?.TimekeeperCreateDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                        CreatedUser = createdUser1,
                        Time = x?.LogTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                        Device = x?.TimeKeeperDevice != null ? x.TimeKeeperDevice.ToEnum<TimeKeeper_Device_Enum>().Text() : "",
                        IsCheck = x?.IsCheck ?? 1
                    })
                    .ToList();
            }
            var logs =
                DaoFactory.ShiftAssignment.GetShiftAssignmentUserWorkingDayLogsByShiftAssignmentUserWorkingDay(
                    employeeShiftId
                );
            var hasShiftLogs = logs != null && logs.Any();

            if (logs != null)
            {
                // Transform to required format with C# logic
                var responseData = logs.Select(log =>
                    {
                        // Parse ActionType enum
                        var actionType = log?.ActionType != null ? ((Shift_ActionType_Enum)log.ActionType).Text() : "";

                        // Parse ClockType enum
                        var clockType = log?.ClockType != null ? ((Clock_Type_Enum)log.ClockType).Text() : "";

                        // Generate option name using C# logic
                        string optionName = log?.ActionType != null && log?.ClockType != null 
                            ? ShiftTimeConfigHelper.GetCheckinActionDescription(log.ActionType, log.ClockType)
                            : "";

                        // Generate option type name
                        string optionTypeName =
                            !string.IsNullOrEmpty(clockType)
                                ? clockType[0].ToString().ToUpper() + clockType.Substring(1)
                                : clockType;

                        // Created user info (always present as requested)
                        var createdUser = new CreatedUser
                        {
                            Name = log.created_user_name ?? "",
                            Username = log.created_user_username ?? "",
                            UserId = int.TryParse(log.created_user_id?.ToString(), out int userId)
                                ? userId
                                : 0,
                        };
                        return new
                        {
                            time = log?.time,
                            log_id = log?.Id ?? 0,
                            is_trashed = log?.is_trashed == true ? 1 : 0,
                            created_at = log?.created_at,
                            trashed_at = log?.trashed_at,
                            created_user = log?.created_user_id,
                            trashed_user = (object)null, // Always null as requested
                            clock_type = clockType,
                            option = new
                            {
                                type = clockType,
                                name = optionName,
                                type_name = optionTypeName,
                            },
                            reason = log?.reason,

                        };
                    })
                    .ToArray();

                if (responseData != null && responseData.Any())
                {
                    // Convert responseData to TimekeeperLog objects and add to existing Items
                    var additionalItems = responseData
                        .Select(
                            (item, index) =>
                            {
                                // Find corresponding log to get user info
                                var correspondingLog = logs.ElementAtOrDefault(index);
                                return new TimekeeperLog
                                {
                                    // Map the dynamic object properties to TimekeeperLog properties
                                    Id = item.log_id,
                                    ClockType = item.clock_type,
                                    CreatedAt = item.created_at?.ToString() ?? "",
                                    Time = item.time?.ToString() ?? "",
                                    Device = "", // Default value since not in responseData
                                    IsCheck = 1, // Default value
                                    isTrashed = item.is_trashed, // Default value
                                    Reason = item.reason,
                                    CreatedUser =
                                        correspondingLog != null
                                            ? new CreatedUser
                                            {
                                                Name = correspondingLog?.created_user_name ?? "",
                                                Username =
                                                    correspondingLog?.created_user_username ?? "",
                                                UserId = int.TryParse(
                                                    correspondingLog?.created_user_id?.ToString(),
                                                    out int createdUserId
                                                )
                                                    ? createdUserId
                                                    : 0,
                                            }
                                            : null,
                                    Option =
                                        correspondingLog != null && correspondingLog?.ClockType != null && correspondingLog?.ActionType != null
                                            ? new TimekeeperLogOption
                                            {
                                                Type = ((Clock_Type_Enum)correspondingLog.ClockType).Text(),
                                                Name = ShiftTimeConfigHelper.GetCheckinActionDescription(
                                                    correspondingLog.ActionType,
                                                    correspondingLog.ClockType
                                                ),
                                                TypeName =
                                                    ((Clock_Type_Enum)correspondingLog.ClockType)
                                                        .Text()
                                                        .Length > 0
                                                        ? (
                                                            (Clock_Type_Enum)
                                                                correspondingLog.ClockType
                                                        )
                                                            .Text()[0]
                                                            .ToString()
                                                            .ToUpper()
                                                            + (
                                                                (Clock_Type_Enum)
                                                                    correspondingLog.ClockType
                                                            )
                                                                .Text()
                                                                .Substring(1)
                                                        : (
                                                            (Clock_Type_Enum)
                                                                correspondingLog.ClockType
                                                        ).Text(),
                                            }
                                            : null,
                                };
                            }
                        )
                        .ToList();

                    // Ensure Items list is initialized
                    if (response.Data.Items == null)
                    {
                        response.Data.Items = new List<TimekeeperLog>();
                    }

                    // Add additional items to existing Items
                    response.Data.Items.AddRange(additionalItems);
                }
            }

            // Sort Items by CreatedAt from newest to oldest
            if (response.Data.Items != null && response.Data.Items.Any())
            {
                response.Data.Items = response.Data.Items
                    .OrderByDescending(x => DateTime.TryParse(x.CreatedAt, out DateTime createdAt) ? createdAt : DateTime.MinValue)
                    .ToList();
            }

            // Set response code based on whether we have any data from either source
            if (hasTimekeeperData || hasShiftLogs)
            {
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
                response.Data.Meta = new TimekeeperLogMeta()
                {
                    count = response.Data.Items?.Count ?? 0,
                    current_page = 1,
                    per_page = 1,
                    total = response.Data.Items?.Count ?? 0,
                    total_pages = 10000,
                };
            }
            
            response.Code = ResponseResultEnum.Success.Value();
            response.Message = ResponseResultEnum.Success.Text();
            return response;
        }

    }
}
