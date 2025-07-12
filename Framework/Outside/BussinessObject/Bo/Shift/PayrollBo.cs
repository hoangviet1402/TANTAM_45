using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Shift;
using DataAccess;
using DataAccess.Model.Shift;
using EntitiesObject.Entities.TanTamEntities;
using MyUtility;
using MyUtility.Extensions;

namespace BussinessObject.Bo.Shift
{
    public class PayrollBo : BaseBo<DBNull>
    {
        public PayrollBo()
            : base(DaoFactory.Payroll)
        {

        }
        public void ShiftAssignment_User_Create(Payroll_User_CreateMultiDayParameter parameter, DateTime dateFrom, DateTime dateTo)
        {
            DaoFactory.Payroll.Payroll_User_Create_MultiDay(parameter, dateFrom, dateTo);
        }

        public ApiResult<List<ClockInOut_Shift>> Payroll_User_GetList(int companyId, int accountMapID, string working_day)
        {
            var response = new ApiResult<List<ClockInOut_Shift>>()
            {
                Data = new List<ClockInOut_Shift>(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            DateTime dateFrom = DateTime.Now;
            DateTime dateTo = DateTime.Now;
            if(string.IsNullOrEmpty(working_day))
            {
                working_day = "today,tomorrow";
            }
            List<string> getDatePayroll = working_day.ToLower().Split(',').ToList();
            var user_Branches = DaoFactory.Branches.AccountGetAllBranchs(accountMapID);
            List<Ins_Payroll_User_GetList_Result> clock_shift = null;
            foreach (var item in user_Branches)
            {
                var data = DaoFactory.Payroll.Payroll_User_GetList(0, accountMapID, item.BranchId, dateFrom.GetBeginOfDay(), dateTo.LastDayOfMonth());
                foreach (var item_getDatePayroll in getDatePayroll)
                {
                    clock_shift = null;
                    switch (item_getDatePayroll)
                    {
                        case "today":
                            clock_shift = data.Where(x => x.WorkingDay.GetValueOrDefault().GetBeginOfDay() == dateFrom.GetBeginOfDay()).ToList();
                            response.Data.AddRange(clock_shift.Select(aa => new ClockInOut_Shift()
                            {
                                Id = aa.PayrollUserID,
                                Name = aa.ShiftName,
                                ShiftKey = aa.ShiftKey,
                                ShiftId = aa.ShiftId,
                                ShiftType = aa.ShiftType,
                                StartTime = aa.StartTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                                EndTime = aa.EndTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                                WorkingHour = aa.WorkingHour,
                                WorkingDay = aa.WorkingDay.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                                WeekOfYear = aa.WeekOfYear,
                                BranchId = item.BranchId,
                                UserId = accountMapID,
                                IsConfirm = 1,
                                IsOvertimeShift = aa.IsOvertimeShift,
                                ShopId = aa.CompanyID ?? 0,
                                MealCoefficient = aa.ShiftAssignmentMealCoefficient,
                                Timezone = aa.Timezone,
                                IsOpenShift = aa.IsOpenShift,
                                ClockStatus = "clock_in",
                                Is_Active = dateFrom >= aa.StartTime.GetValueOrDefault() && dateFrom <= aa.EndTime.GetValueOrDefault()
                            }));
                            break;
                        case "tomorrow":
                            clock_shift = data.Where(x => x.WorkingDay.GetValueOrDefault().GetBeginOfDay() > dateFrom.GetBeginOfDay()).OrderBy(x => x.WorkingDay.GetValueOrDefault()).Take(1).ToList();
                            response.Data.AddRange(clock_shift.Select( aa => new ClockInOut_Shift()
                            {
                                Id = aa.PayrollUserID,
                                Name = aa.ShiftName,
                                ShiftKey = aa.ShiftKey,
                                ShiftId = aa.ShiftId,
                                ShiftType = aa.ShiftType,
                                StartTime = aa.StartTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                                EndTime = aa.EndTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                                WorkingHour = aa.WorkingHour,
                                WorkingDay = aa.WorkingDay.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                                WeekOfYear = aa.WeekOfYear,
                                BranchId = item.BranchId,
                                UserId = accountMapID,
                                IsConfirm = 1,
                                IsOvertimeShift = aa.IsOvertimeShift,
                                ShopId = aa.CompanyID ?? 0,
                                MealCoefficient = aa.ShiftAssignmentMealCoefficient,
                                Timezone = aa.Timezone,
                                IsOpenShift = aa.IsOpenShift,
                                ClockStatus = "clock_in",
                                Is_Active = dateFrom >= aa.StartTime.GetValueOrDefault() && dateFrom <= aa.EndTime.GetValueOrDefault()
                            }));
                            break;
                        case "week":
                            clock_shift = data.Where(x => x.WorkingDay.GetValueOrDefault().GetBeginOfDay() > dateFrom.AddDays(1).GetBeginOfDay()).OrderBy(x => x.WorkingDay.GetValueOrDefault()).Take(7).ToList();
                            response.Data.AddRange(clock_shift.Select(aa => new ClockInOut_Shift()
                            {
                                Id = aa.AssignmentUserID,
                                Name = aa.ShiftName,
                                ShiftKey = aa.ShiftKey,
                                ShiftId = aa.ShiftId,
                                ShiftType = aa.ShiftType,
                                StartTime = aa.StartTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                                EndTime = aa.EndTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                                WorkingHour = aa.WorkingHour,
                                WorkingDay = aa.WorkingDay.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                                WeekOfYear = aa.WeekOfYear,
                                BranchId = item.BranchId,
                                UserId = accountMapID,
                                IsConfirm = 1,
                                IsOvertimeShift = aa.IsOvertimeShift,
                                ShopId = aa.CompanyID ?? 0,
                                MealCoefficient = aa.ShiftAssignmentMealCoefficient,
                                Timezone = aa.Timezone,
                                IsOpenShift = aa.IsOpenShift,
                                ClockStatus = "clock_in",
                                Is_Active = dateFrom >= aa.StartTime.GetValueOrDefault() && dateFrom <= aa.EndTime.GetValueOrDefault()
                            }));
                            break;
                        default:
                            break;
                    }
                }        
            }
            response.Code = ResponseResultEnum.Success.Value();
            response.Message = ResponseResultEnum.Success.Text();
            return response;
        }

        public ApiResult<StatusClockInOutShiftResponse> Payroll_StatusClockInOutShift(int accountMapID, DateTime dateFrom, string timekeeper_device = "", int is_show_button = 0, bool isInitial = false)
        {
            var response = new ApiResult<StatusClockInOutShiftResponse>()
            {
                Data = new StatusClockInOutShiftResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            var currentDate = DateTime.Now;
            var dataShift = DaoFactory.Payroll.Payroll_User_GetStatus_clock_in_out(accountMapID, dateFrom);
            var dataTimes = DaoFactory.Shift.GetTimes("");
            response.Data.ClockType = Clock_Type_Enum.clock_in.Text();
            response.Data.ClockSetting = new ClockSetting()
            {
                ClockInOutRequirements = new List<string>() { "gps" },
                Debug = false,
                Distance = 100,
                IsLocationTracking = 0,
                LogLevel = 5
            };

            // kiểm tra ca hợp lệ
            response.Data.ClockType = Clock_Type_Enum.clock_in.Text();
            response.Data.CurrentEmployeeShift = new ClockInOut_Shift()
            {
                Id = 999999999,
                Name = "Ca Cá Nhân"
            };

            var dataTimekeeper = DaoFactory.Timekeeper.Timekeeper_log_User_GetLog_OneDay(accountMapID, dateFrom);
            response.Data.TimekeeperLog = new TimekeeperLog()
            {
                Id = 0,                
                ClockType = Clock_Type_Enum.clock_in.Text()
            };
            if (dataTimekeeper != null && dataTimekeeper.Any())
            {
                var currentTimekeeper = dataTimekeeper.OrderByDescending(c => c.LogTime).FirstOrDefault();
                response.Data.TimekeeperLog = new TimekeeperLog()
                {
                    Id = currentTimekeeper.ID,
                    Time = currentTimekeeper.LogTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                    ClockType = currentTimekeeper.ClockType.ToEnum<Clock_Type_Enum>().Text(),
                    PayrollUserID = currentTimekeeper.PayrollUserID ?? 0
                };                
            }

            if (response.Data.TimekeeperLog != null
                && response.Data.TimekeeperLog.Id > 0
                && response.Data.TimekeeperLog.ClockType == Clock_Type_Enum.clock_in.Text())
            {
                response.Data.ClockType = Clock_Type_Enum.clock_out.Text();
            }


            switch (response.Data.ClockType.ToEnum<Clock_Type_Enum>())
            {
                case Clock_Type_Enum.clock_out: // nó muốn check out đã qua ca mới và chưa tới giờ vô // autocheck out
                    // cho check out thoải mái
                    break;
                case Clock_Type_Enum.clock_in: // nó muốn check in -> thời gian hiện tại quá thời gian checkout -> ko hiện ca 
                    if (dataShift.Any(x => SetTime(dataTimes, dateFrom, x.EndCheckOutHourId ?? 0, x.EndCheckOutMinuteId ?? 0) >= currentDate) == false)
                    {
                        response.Code = ResponseResultEnum.NoData.Value();
                        response.Message = "Không có ca nào hợp lệ vui lòng chuyển qua ca cá nhân";
                        return response;
                    }
                    break;
                default:
                    break;
            }

            

            if(isInitial == true && accountMapID > 0)
            {
                #region tạo ca làm việc cho nhân viên hiện tại
                foreach (var dataShiftItem in dataShift.Where(x => string.IsNullOrEmpty(x.GenerateTimekeepingType) == false && x.ShiftStatus == Shift_status.active.Value()).ToList())
                {
                    //var assignment_user_id = DaoFactory.ShiftAssignment.ShiftAssignment_User_Create(dataShiftItem.ShiftAssignmentId, accountMapID);
                    if (dataShiftItem.AssignmentUserID > 0)
                    {
                        DateTime dateTo;

                        if (dataShiftItem.GenerateTimekeepingType == Generate_Timekeeping_Type_Obj_Enum.generate_from_start_of_month.Text())
                        {
                            DateTimeExtension.GetRangeByType(DateTime.Now, 1, out dateFrom, out dateTo);
                        }
                        else
                        {
                            DateTimeExtension.GetRangeByType(DateTime.Now, 2, out dateFrom, out dateTo);
                        }

                        dateFrom = dateFrom.GetBeginOfDay();

                        DaoFactory.Payroll.Payroll_User_Create_MultiDay(new Payroll_User_CreateMultiDayParameter()
                        {
                            AccountMapID = accountMapID,
                            AssignmentUserID = dataShiftItem.AssignmentUserID,
                            CheckinType = "",
                            CheckouType = "",
                            EndTime = dataShiftItem.EndTime,
                            StartTime = dataShiftItem.StartTime,

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

                #endregion
            }

            response.Data.EmployeeShifts = new List<EmployeeShift>() { };
            if (response.Data.TimekeeperLog.ClockType == Clock_Type_Enum.clock_in.Text())
            {
                response.Data.CurrentEmployeeShift = dataShift.Select(x => new ClockInOut_Shift()
                {
                    Is_Active = x.ShiftStatus == 1,
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
                    BranchId = x.BranchID,
                    UserId = accountMapID,
                    IsConfirm = 1,
                    IsOvertimeShift = x.IsOvertimeShift,
                    ShopId = x.CompanyID ?? 0,
                    MealCoefficient = x.ShiftAssignmentMealCoefficient,
                    Timezone = x.Timezone,
                    IsOpenShift = x.IsOpenShift,
                    CheckinType = x.CheckinType != null && x.CheckinType > 0 ? x.CheckinType.Value.ToEnum<TimeKeeper_Device_Enum>().Text() : null,
                    CheckoutType = x.CheckoutType != null && x.CheckoutType > 0 ? x.CheckoutType.Value.ToEnum<TimeKeeper_Device_Enum>().Text() : null

                }).FirstOrDefault();
            }
            else
            {
                response.Data.EmployeeShifts = dataShift.Select(x => new EmployeeShift()
                {
                    IsYesterday = x.WorkingDay.GetValueOrDefault().GetBeginOfDay() < dateFrom.GetBeginOfDay() ? 1 : 0,
                    IsEndNextDay = x.WorkingDay.GetValueOrDefault().GetBeginOfDay() <= dateFrom.GetBeginOfDay() ? 0 : 1,
                    IsReason = 1,
                    ClockInOut_Shift_Info = new Models.Shift.ClockInOut_Shift()
                    {
                        Is_Active = x.ShiftStatus == 1,
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
                        BranchId = x.BranchID,
                        UserId = accountMapID,
                        IsConfirm = 1,
                        IsOvertimeShift = x.IsOvertimeShift,
                        ShopId = x.CompanyID ?? 0,
                        MealCoefficient = x.ShiftAssignmentMealCoefficient,
                        Timezone = x.Timezone,
                        IsOpenShift = x.IsOpenShift,
                        CheckinType = x.CheckinType != null && x.CheckinType > 0 ? x.CheckinType.Value.ToEnum<TimeKeeper_Device_Enum>().Text() : null,
                        CheckoutType = x.CheckoutType != null && x.CheckoutType > 0 ? x.CheckoutType.Value.ToEnum<TimeKeeper_Device_Enum>().Text() : null
                    }
                }).ToList();
            }
            response.Code = ResponseResultEnum.Success.Value();
            response.Message = ResponseResultEnum.Success.Text();
            return response;
        }

        public ApiResult<ClockInOutShiftResponse> Payroll_ClockInOutShift(ClockInOutShiftRequest request, int accountMapID, int companyIdMap, DateTime dateFrom)
        {
            var response = new ApiResult<ClockInOutShiftResponse>()
            {
                Data = new ClockInOutShiftResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            var dataShift = DaoFactory.Payroll.Payroll_User_GetStatus_clock_in_out(accountMapID, dateFrom);
            var clock_shift = new Ins_Shift_User_GetStatus_clock_in_out_Result();

            if(request.EmployeeShiftId == null || request.EmployeeShiftId <= 1)
            {
                request.EmployeeShiftId = request.Id;
            }
            if (dataShift != null && dataShift.Any())
            {
                clock_shift = dataShift.FirstOrDefault(x => x.PayrollUserID == request.EmployeeShiftId);
            }

            if (clock_shift == null || clock_shift.AssignmentUserID == 0)
            {
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = "Ca bạn chọn chưa vào ca hoặc không tồn tại";
                return response;
            }

            #region  kiểm tra wifi ở đây 

            if (clock_shift.AutoApprove == 0)
            {
                List<Ins_Wifi_Get_Result> wifi_account = DaoFactory.Wifi.WifiGet(0, request.BranchId ?? clock_shift.BranchID, 0, accountMapID);
                var check_wifi = true;
                var check_gps = true;
                if (wifi_account != null && wifi_account.Any())
                {
                    if(string.IsNullOrEmpty(request.Bssid) == false && wifi_account.Any( x => x.Bssid == request.Bssid) == true)
                    {
                        //check wifi
                        check_wifi = true;
                    }
                    if (wifi_account.Any(x => GeoHelper.IsInCoverage(
                                request.Latitude ?? 0, request.Longitude ?? 0, x.Accuracy ?? 0,
                                x.Latitude ?? 0, x.Longitude ?? 0, x.Radius ?? 0
                                ) == true) == true
                        )
                    {
                        //check  tọa độ 
                        check_gps = true;
                    }
                }

                if(check_wifi == false && check_gps == false)
                {
                    response.Code = ResponseResultEnum.Success.Value();
                    response.Message = "vị trí bạn vào / ra ca không hợp lệ";
                    return response;
                }
            }
            #endregion
            var dataTimekeeper = DaoFactory.Timekeeper.Timekeeper_log_User_GetLog_OneDay(accountMapID, dateFrom);
            var logID = DaoFactory.Timekeeper.Timekeeper_log_User_Insert(new Timekeeper_log_User_Insert_parameter()
            {
                AccountMapID = accountMapID,
                EmployeeShiftID = request.EmployeeShiftId ?? 0,
                LogTime = dateFrom,
                ClockType = request.ClockType.ToEnum<Clock_Type_Enum>().Value(),
                CurrentBranchId = request.BranchId ?? clock_shift.BranchID,
                ConnectionType = request.ConnectionType.ToEnum<Connection_Type_Enum>().Value(),
                TimeKeeperDevice = request.TimekeeperDevice.ToEnum<TimeKeeper_Device_Enum>().Value(),
                Bssid = request.Bssid,
                Ssid = request.Ssid,
                Latitude = request.Latitude ?? 0,
                Longitude = request.Longitude ?? 0,
                Accuracy = request.Accuracy ?? 0,
                Altitude = request.Altitude ?? 0,
                AltitudeAccuracy = request.AltitudeAccuracy ?? 0,
                Speed = request.Speed ?? 0,
                SpeedAccuracy = request.SpeedAccuracy ?? 0,
                Course = request.Course ?? 0,
                CourseAccuracy = request.CourseAccuracy ?? 0,
                Mocked = request.Mocked ?? false,
            });

            if (logID > 0)
            {
                if (request.ClockType.ToEnum<Clock_Type_Enum>() == Clock_Type_Enum.clock_in)
                {
                    response.Data.NextClockType = Clock_Type_Enum.clock_out.Text();
                }
                else //(request.ClockType.ToEnum<Clock_Type_Enum>() == Clock_Type_Enum.clock_in)
                {
                    response.Data.NextClockType = Clock_Type_Enum.clock_out.Text();
                }

                response.Data.CurrentEmployeeShift = new ClockInOut_Shift()
                {
                    Id = clock_shift.AssignmentUserID,
                    Name = clock_shift.ShiftName,
                    ShiftKey = clock_shift.ShiftKey,
                    ShiftId = clock_shift.ShiftId,
                    ShiftType = clock_shift.ShiftType,
                    StartTime = clock_shift.StartTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                    EndTime = clock_shift.EndTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                    WorkingHour = clock_shift.WorkingHour,
                    WorkingDay = clock_shift.WorkingDay.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                    WeekOfYear = clock_shift.WeekOfYear,
                    BranchId = clock_shift.BranchID,
                    UserId = accountMapID,
                    IsConfirm = 1,
                    IsOvertimeShift = clock_shift.IsOvertimeShift,
                    ShopId = clock_shift.CompanyID ?? 0,
                    MealCoefficient = clock_shift.ShiftAssignmentMealCoefficient,
                    Timezone = clock_shift.Timezone,
                    IsOpenShift = clock_shift.IsOpenShift,

                    CheckinType = clock_shift.CheckinType != null && clock_shift.CheckinType > 0 ? clock_shift.CheckinType.Value.ToEnum<TimeKeeper_Device_Enum>().Text() : null,
                    CheckoutType = clock_shift.CheckoutType != null && clock_shift.CheckoutType > 0 ? clock_shift.CheckoutType.Value.ToEnum<TimeKeeper_Device_Enum>().Text() : null,


                    CheckoutLogId = request.ClockType.ToEnum<Clock_Type_Enum>() == Clock_Type_Enum.clock_in ? null : logID,
                    CheckoutBranchId = request.ClockType.ToEnum<Clock_Type_Enum>() == Clock_Type_Enum.clock_in ? null : request.BranchId,
                    CheckinLogId = request.ClockType.ToEnum<Clock_Type_Enum>() == Clock_Type_Enum.clock_out ? null : logID,
                    CheckinBranchId = request.ClockType.ToEnum<Clock_Type_Enum>() == Clock_Type_Enum.clock_out ? null : request.BranchId
                };

                response.Data.TimekeeperLog = new TimekeeperLog()
                {
                    Id = logID.GetValueOrDefault(0),
                    Time = dateFrom.ToString("yyyy-MM-dd HH:mm:ss"),
                    ClockType = request.ClockType.ToEnum<Clock_Type_Enum>().Text(),                   
                };
            }

            return response;
        }

        /// <summary>
        /// Tạo một đối tượng DateTime mới từ ngày của đối tượng hiện tại và giờ, phút được chỉ định.
        /// </summary>
        /// <param name="date">Ngày hiện tại.</param>
        /// <param name="hour">Giờ muốn gán.</param>
        /// <param name="minute">Phút muốn gán.</param>
        /// <returns>Một đối tượng DateTime mới với ngày được giữ nguyên và thời gian được cập nhật.</returns>
        public DateTime SetTime(List<Ins_Time_GetList_Result> listTimes, DateTime date, int hourid, int minuteid)
        {
            var data = new DateTime(
                date.Year,
                date.Month,
                date.Day,
                listTimes.FirstOrDefault(x => x.ID == hourid && x.IsHour == 1).Value ?? 0,
                listTimes.FirstOrDefault(x => x.ID == minuteid && x.IsHour == 0).Value ?? 0,
                0
            );
            return data;
        }
    }
}
