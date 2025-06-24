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
            DaoFactory.Payroll.ShiftAssignment_User_Create(parameter, dateFrom , dateTo);
        }

        public List<Ins_Payroll_User_GetList_Result> Payroll_User_GetList(int assignmentUserID, int accountMapID, int brandId, DateTime dateFrom, DateTime dateTo)
        {
            var data  = DaoFactory.Payroll.Payroll_User_GetList(assignmentUserID, accountMapID , brandId, dateFrom , dateTo);
            if(assignmentUserID == 0)
            {
                return data.Where(x => x.ShiftType == shift_type_enum.standard_working.Text()).ToList();
            }
            else
            {
                return data;
            }
        }

        public ApiResult<StatusClockInOutShiftResponse> StatusClockInOutShift(int accountMapID, DateTime dateFrom)
        {
            var response = new ApiResult<StatusClockInOutShiftResponse>()
            {
                Data = new StatusClockInOutShiftResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };


            var dataShift = DaoFactory.Payroll.Shift_User_GetStatus_clock_in_out(accountMapID, dateFrom);
            var dataTimes = DaoFactory.Shift.GetTimes("");
            //if(dataShift == null || dataShift.Any() == false)
            //{
            //    response.Code = ResponseResultEnum.NoData.Value();
            //    response.Message = "Không có data ca";
            //    return response;
            //}
            response.Data.ClockSetting = new ClockSetting()
            {
                ClockInOutRequirements = new List<string>() { "gps" },
                Debug = false,
                Distance = 100,
                IsLocationTracking = 0,
                LogLevel = 5
            };

            // kiểm tra ca hợp lệ


            var dataTimekeeper = DaoFactory.Payroll.Timekeeper_log_User_GetLog_OneDay(accountMapID, dateFrom);
            if (dataTimekeeper != null && dataTimekeeper.Any())
            {
                var currentTimekeeper = dataTimekeeper.OrderByDescending(c => c.LogTime).FirstOrDefault();
                response.Data.TimekeeperLog = new TimekeeperLog()
                {
                    Id = currentTimekeeper.ID,
                    Time = currentTimekeeper.LogTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                    ClockType = currentTimekeeper.ClockType.ToEnum<clock_type_enum>().Text(),
                    EmployeeShiftId = currentTimekeeper.EmployeeShiftID ?? 0
                };
            }

            if (response.Data.TimekeeperLog != null 
                && response.Data.TimekeeperLog.Id > 0 
                && response.Data.TimekeeperLog.ClockType == clock_type_enum.clock_in.Text())
            {
                response.Data.ClockType = clock_type_enum.clock_out.Text();
            }


            switch (response.Data.ClockType.ToEnum<clock_type_enum>())
            {
                case clock_type_enum.clock_out: // nó muốn check out
                    break;
                case clock_type_enum.clock_in: // nó muốn check in
                    SetTime(dataTimes, dateFrom, x.EndCheckOutHourId, x.EndCheckOutMinuteId)
                        if (dataShift.Any(x=> new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, x.EndCheckOutHourId ?? 0 ,x.EndCheckOutMinuteId ?? 0,59) > dateFrom))
                         {

                        }
                    break;
                default:
                    break;
            }

            response.Data.ClockType = clock_type_enum.clock_in.Text();
            response.Data.CurrentEmployeeShift = new CurrentEmployeeShift()
            {
                Id = "on_call",
                Name = "Ca Cá Nhân"
            };

            response.Data.EmployeeShifts = new List<EmployeeShift>() { };

            response.Data.EmployeeShifts = dataShift.Select(x => new EmployeeShift()
            {
                IsYesterday = x.WorkingDay.GetValueOrDefault().GetBeginOfDay() < dateFrom.GetBeginOfDay() ? 1 : 0,
                IsEndNextDay = x.WorkingDay.GetValueOrDefault().GetBeginOfDay() <= dateFrom.GetBeginOfDay() ? 0 : 1,
                IsReason = 1 ,
                ClockInOut_Shift_Info = new Models.Shift.Shift() {
                    Id = x.AssignmentUserID,
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
                    CheckinType = x.CheckinType,
                    CheckoutType = x.CheckoutType
                }
            }).ToList();
            response.Code = ResponseResultEnum.Success.Value();
            response.Message = ResponseResultEnum.Success.Text();
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
            return new DateTime(
                date.Year, 
                date.Month, 
                date.Day, 
                listTimes.FirstOrDefault(x => x.ID == hourid && x.IsHour == 1).Value ?? 0 , 
                listTimes.FirstOrDefault(x => x.ID == minuteid && x.IsHour == 0).Value ?? 0, 
                0
            );
        }
    }
}
