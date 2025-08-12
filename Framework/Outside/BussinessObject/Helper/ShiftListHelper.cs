using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Shift;
using DataAccess;
using Logger;
using MyUtility.Extensions;
using System;
using System.Linq;

namespace BussinessObject.Helper
{
    public static class ShiftListHelper
    {
        public static ApiResult<ListShiftResponse> GetShiftListByWorkingDayCommon(int userId, DateTime workingDay, bool isAll = true)
        {
            var response = new ApiResult<ListShiftResponse>()
            {
                Data = new ListShiftResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            try
            {
                var shifts = DaoFactory.Shift.GetShiftListByUser(userId, workingDay, isAll);

                if (shifts != null && shifts.Any())
                {
                    foreach (var shift in shifts)
                    {
                        var timeConfig = ShiftTimeConfigHelper.GetShiftTimeConfiguration(shift.ShiftId);

                        var uniqueKey = shift.ShiftKey;
                        var counter = 0;
                        while (response.Data.ContainsKey(uniqueKey))
                        {
                            uniqueKey = $"{shift.ShiftKey}_{counter}";
                            counter++;
                        }

                        var shiftItem = new ShiftListItem
                        {
                            id = shift.ShiftId.ToString() ?? "",
                            name = counter > 0 ? shift.ShiftName + "_" + counter : shift.ShiftName,
                            shift_key = uniqueKey,
                            shift_id = shift.ShiftId.ToString(),
                            working_hour = Math.Round((double)timeConfig.WorkingHour, 2),
                            week_of_year = ShiftTimeConfigHelper.CalculateWeekOfYear(workingDay),
                            branch_id = "",
                            total_register = shift.TotalRegister,
                            is_confirm = null,
                            sort_index = shift.SortIndex,
                            end_working_date = null,
                            timezone = string.IsNullOrEmpty(shift.Timezone) ? "Asia/Saigon" : shift.Timezone
                        };

                        shiftItem.working_day = workingDay.ToString("yyyy-MM-dd HH:mm:ss");
                        shiftItem.start_time = $"{workingDay:yyyy-MM-dd} {timeConfig.StartTime:HH:mm:ss}";
                        shiftItem.end_time = $"{workingDay:yyyy-MM-dd} {timeConfig.EndTime:HH:mm:ss}";

                        response.Data[uniqueKey] = shiftItem;
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
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"GetShiftListByWorkingDayCommon - Error occurred", ex);
                response.Code = ResponseResultEnum.SystemError.Value();
                response.Message = "Lỗi hệ thống: " + ex.Message;
            }

            return response;
        }

        public static ApiResult<ListShiftResponse> ValidateWorkingDayInput(string workingDayString, out DateTime workingDay)
        {
            workingDay = default(DateTime);

            if (string.IsNullOrWhiteSpace(workingDayString))
            {
                return new ApiResult<ListShiftResponse>()
                {
                    Data = new ListShiftResponse(),
                    Code = ResponseResultEnum.InvalidInput.Value(),
                    Message = "Vui lòng cung cấp ngày làm việc."
                };
            }

            if (!DateTime.TryParse(workingDayString, out workingDay))
            {
                return new ApiResult<ListShiftResponse>()
                {
                    Data = new ListShiftResponse(),
                    Code = ResponseResultEnum.InvalidInput.Value(),
                    Message = "Ngày làm việc không hợp lệ."
                };
            }

            return null; // Valid input
        }

        public static ApiResult<ListShiftResponse> ValidateUserIdInput(string userIdString, out int userId)
        {
            userId = 0;

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out userId) || userId <= 0)
            {
                return new ApiResult<ListShiftResponse>()
                {
                    Data = new ListShiftResponse(),
                    Code = ResponseResultEnum.InvalidData.Value(),
                    Message = "User ID là bắt buộc và phải hợp lệ."
                };
            }

            return null; // Valid input
        }
    }
} 