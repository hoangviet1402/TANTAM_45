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
            DaoFactory.Payroll.ShiftAssignment_User_Create(parameter, dateFrom, dateTo);
        }

        public List<Ins_Payroll_User_GetList_Result> Payroll_User_GetList(int assignmentUserID, int accountMapID, int brandId, DateTime dateFrom, DateTime dateTo)
        {
            var data = DaoFactory.Payroll.Payroll_User_GetList(assignmentUserID, accountMapID, brandId, dateFrom, dateTo);
            if (assignmentUserID == 0)
            {
                return data.Where(x => x.ShiftType == Shift_Type_Enum.standard_working.Text()).ToList();
            }
            else
            {
                return data;
            }
        }

        public ApiResult<StatusClockInOutShiftResponse> StatusClockInOutShift(int accountMapID, DateTime dateFrom, string timekeeper_device = "", int is_show_button = 0, bool isInitial = false)
        {
            var response = new ApiResult<StatusClockInOutShiftResponse>()
            {
                Data = new StatusClockInOutShiftResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            var currentDate = DateTime.Now;
            var dataShift = DaoFactory.Payroll.Shift_User_GetStatus_clock_in_out(accountMapID, dateFrom);
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

            response.Data.CurrentEmployeeShift = new CurrentEmployeeShift()
            {
                Id = 999999999,
                Name = "Ca Cá Nhân"
            };

            var dataTimekeeper = DaoFactory.Payroll.Timekeeper_log_User_GetLog_OneDay(accountMapID, dateFrom);
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
                if (response.Data.TimekeeperLog.ClockType == Clock_Type_Enum.clock_in.Text())
                {
                    response.Data.CurrentEmployeeShift = new CurrentEmployeeShift()
                    {
                        Id = response.Data.TimekeeperLog.PayrollUserID,
                        Name = dataShift.Any(x => x.AssignmentUserID == response.Data.TimekeeperLog.PayrollUserID) ?
                                dataShift.FirstOrDefault(x => x.AssignmentUserID == response.Data.TimekeeperLog.PayrollUserID).ShiftName : "Ca cá nhân"
                    };
                }
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
                    //if (dataShift.Any(x => SetTime(dataTimes, dateFrom, x.chec ?? 0, x.EndCheckOutMinuteId ?? 0) < currentDate))
                    //{

                    //}
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

            response.Data.ClockType = Clock_Type_Enum.clock_in.Text();

            if(isInitial == true && accountMapID > 0)
            {
                #region tạo ca làm việc cho nhân viên hiện tại
                foreach (var dataShiftItem in dataShift.Where(x => string.IsNullOrEmpty(x.GenerateTimekeepingType) == false && x.ShiftType == Shift_Type_Enum.standard_working.Text()).ToList())
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

                        dateFrom = DateTime.Now.GetBeginOfDay();

                        DaoFactory.Payroll.ShiftAssignment_User_Create(new Payroll_User_CreateMultiDayParameter()
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

            response.Data.EmployeeShifts = dataShift.Select(x => new EmployeeShift()
            {
                IsYesterday = x.WorkingDay.GetValueOrDefault().GetBeginOfDay() < dateFrom.GetBeginOfDay() ? 1 : 0,
                IsEndNextDay = x.WorkingDay.GetValueOrDefault().GetBeginOfDay() <= dateFrom.GetBeginOfDay() ? 0 : 1,
                IsReason = 1,
                ClockInOut_Shift_Info = new Models.Shift.ClockInOut_Shift()
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
                    BranchId = x.BranchID,
                    UserId = accountMapID,
                    IsConfirm = 1,
                    IsOvertimeShift = x.IsOvertimeShift,
                    ShopId = x.CompanyID ?? 0,
                    MealCoefficient = x.ShiftAssignmentMealCoefficient,
                    Timezone = x.Timezone,
                    IsOpenShift = x.IsOpenShift,
                    CheckinType = x.CheckinType != null && x.CheckinType  > 0? x.CheckinType.Value.ToEnum<TimeKeeper_Device_Enum>().Text() : null,
                    CheckoutType = x.CheckoutType != null && x.CheckoutType > 0  ? x.CheckoutType.Value.ToEnum<TimeKeeper_Device_Enum>().Text() : null
                }
            }).ToList();
            response.Code = ResponseResultEnum.Success.Value();
            response.Message = ResponseResultEnum.Success.Text();
            return response;
        }

        public ApiResult<ClockInOutShiftResponse> ClockInOutShift(ClockInOutShiftRequest request, int accountMapID, int companyIdMap, DateTime dateFrom)
        {
            var response = new ApiResult<ClockInOutShiftResponse>()
            {
                Data = new ClockInOutShiftResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            var dataShift = DaoFactory.Payroll.Shift_User_GetStatus_clock_in_out(accountMapID, dateFrom);
            var clock_shift = new Ins_Shift_User_GetStatus_clock_in_out_Result();
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

            var dataTimekeeper = DaoFactory.Payroll.Timekeeper_log_User_GetLog_OneDay(accountMapID, dateFrom);
            var logID = DaoFactory.Payroll.Timekeeper_log_User_Insert(new Timekeeper_log_User_Insert_parameter()
            {
                AccountMapID = accountMapID,
                EmployeeShiftID = request.EmployeeShiftId ?? 0,
                LogTime = dateFrom,
                ClockType = request.ClockType.ToEnum<Clock_Type_Enum>().Value(),
                CurrentBranchId = request.BranchId ?? 0,
                ConnectionType = request.ConnectionType.ToEnum<Connection_Type_Enum>().Value(),
                TimeKeeperDevice = request.TimekeeperDevice.ToEnum<TimeKeeper_Device_Enum>().Value(),
                Bssid = request.Bssid,
                Ssid = request.Ssid,
                Latitude = request.Latitude ?? 0,
                Longitude = request.Longitude ?? 0
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
