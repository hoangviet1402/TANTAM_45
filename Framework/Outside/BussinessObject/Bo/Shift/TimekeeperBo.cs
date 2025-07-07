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
                response.Data.Items = data.Select(x => new TimekeeperLog
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
                    ConnectionInfo = new Timekeeper_Connection_Info() {
                        ConnectionType = x.ConnectionType.ToEnum<Connection_Type_Enum>().Text(),
                        Bssid = x.Bssid,
                        Ssid = x.Ssid
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
                    }
                }).ToList();
            }
            else
            {
                response.Code = ResponseResultEnum.NoData.Value();
                response.Message = ResponseResultEnum.NoData.Text();
            }
            return response;
        }
    }
}
