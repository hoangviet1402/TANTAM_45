using BussinessObject.Models.Report;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Helper
{
    public static class PayrollHelper
    {
        // Tính công
        public static decimal CalculateTotalPenalty(
            DateTime shiftStart, DateTime shiftEnd, DateTime checkIn, DateTime checkOut,
            List<Ins_Shift_TimePenaltyRule_SelectByShiftId_Result> rules,
            int latelyCheckIn, int earlyCheckOut)
        {
            decimal totalPenalty = 0.00m;

            // --- Tính công đi muộn ---
            if (checkIn > shiftStart)
            {
                int lateMinutes = (int)(checkIn - shiftStart).TotalMinutes;
                // Nếu có cấu hình cho phép đi trễ, và số phút đi trễ <= cấu hình thì bỏ qua
                if (latelyCheckIn > 0 && lateMinutes <= latelyCheckIn)
                {
                    lateMinutes = 0; // Không tính penalty
                }

                if (lateMinutes > 0)
                {

                    foreach (var rule in rules)
                    {
                        if (rule.Type == 1 && lateMinutes >= rule.MinMinute && lateMinutes <= rule.MaxMinute)
                        {
                            totalPenalty += rule.PenaltyValue.GetValueOrDefault(0);
                            break;
                        }
                    }
                }
            }

            if (totalPenalty >= 1)
            {
                return 0;
            }

            // --- Tính công về sớm ---
            if (checkOut < shiftEnd)
            {
                int earlyMinutes = (int)(shiftEnd - checkOut).TotalMinutes;

                // Nếu có cấu hình cho phép về sớm, và số phút về sớm <= cấu hình thì bỏ qua
                if (earlyCheckOut > 0 && earlyMinutes <= earlyCheckOut)
                {
                    earlyMinutes = 0; // Không tính penalty
                }

                if (earlyMinutes > 0)
                {
                    foreach (var rule in rules)
                    {
                        if (rule.Type == 2 && earlyMinutes >= rule.MinMinute && earlyMinutes <= rule.MaxMinute)
                        {
                            totalPenalty += rule.PenaltyValue.GetValueOrDefault(0);
                            break;
                        }
                    }
                }
            }

            if (totalPenalty >= 1)
            {
                return 0;
            }

            return 1 - totalPenalty;
        }

        // Tính giờ thực
        public static double CalculateWorkHours(DateTime checkIn, DateTime checkOut, DateTime shiftStart, DateTime shiftEnd, DateTime breakStart, DateTime breakEnd)
        {
            if (checkOut <= checkIn)
                return 0;

            // Giới hạn thời gian check-in/check-out trong khung ca
            DateTime actualCheckIn = checkIn < shiftStart ? shiftStart : checkIn;
            DateTime actualCheckOut = checkOut > shiftEnd ? shiftEnd : checkOut;

            if (actualCheckOut <= actualCheckIn)
                return 0;

            TimeSpan totalWorked = actualCheckOut - actualCheckIn;

            TimeSpan breakOverlap = TimeSpan.Zero;

            // Nếu ca làm hoàn toàn nằm ngoài giờ nghỉ thì không trừ giờ nghỉ
            bool endsBeforeBreak = actualCheckOut <= breakStart;
            bool startsAfterBreak = actualCheckIn >= breakEnd;

            if (!endsBeforeBreak && !startsAfterBreak)
            {
                // Có giao nhau thì tính phần giao giữa thời gian làm và thời gian nghỉ
                DateTime actualBreakStart = actualCheckIn > breakStart ? actualCheckIn : breakStart;
                DateTime actualBreakEnd = actualCheckOut < breakEnd ? actualCheckOut : breakEnd;

                if (actualBreakEnd > actualBreakStart)
                    breakOverlap = actualBreakEnd - actualBreakStart;
            }

            TimeSpan actualWork = totalWorked - breakOverlap;

            return Math.Round(actualWork.TotalHours, 2);
        }

        // Làm thêm giờ ( ko tính ot)
        public static int CalculateExtraWorkMinutes(DateTime checkOut, DateTime shiftEnd)
        {
            if (checkOut > shiftEnd)
            {
                int minutesLate = (int)(checkOut - shiftEnd).TotalMinutes;

                if (minutesLate >= 10)
                    return minutesLate;
            }

            return 0;
        }

        // Số phút đi làm sớm
        public static int CalculateEarlyCheckInMinutes(DateTime checkIn, DateTime shiftStart)
        {
            if (checkIn < shiftStart)
            {
                int earlyMinutes = (int)(shiftStart - checkIn).TotalMinutes;

                if (earlyMinutes >= 5)
                    return earlyMinutes;
            }

            return 0;
        }

        // Số phút đi về sớm
        public static int CalculateEarlyLeaveMinutes(DateTime checkOut, DateTime shiftEnd)
        {
            if (checkOut < shiftEnd)
            {
                int earlyMinutes = (int)(shiftEnd - checkOut).TotalMinutes;

                if (earlyMinutes >= 0)
                    return earlyMinutes;
            }

            return 0;
        }

        // Số phút muộn
        public static int CalculateLateMinutes(DateTime checkIn, DateTime shiftStart)
        {
            if (checkIn > shiftStart)
                return (int)(checkIn - shiftStart).TotalMinutes;

            return 0;
        }

        // --- Helper methods to reduce code duplication and improve performance ---
        public static Dictionary<int, List<Ins_Timekeeper_log_GetListByAccountMapID_Simple_Result>> BuildTimekeeperLogDict(List<Ins_Timekeeper_log_GetListByAccountMapID_Simple_Result> logs)
        {
            return logs.GroupBy(x => x.PayrollUserID).ToDictionary(g => g.Key ?? 0, g => g.ToList());
        }
        public static Dictionary<int, List<Ins_ShiftAssignment_User_WorkingDay_GetDateToDate_Result>> BuildChamCongHoDict(List<Ins_ShiftAssignment_User_WorkingDay_GetDateToDate_Result> logs)
        {
            return logs.GroupBy(x => x.PayrollID).ToDictionary(g => g.Key, g => g.ToList());
        }

        public static List<Ins_Timekeeper_log_GetListByAccountMapID_Simple_Result> GetAllLogsForPayrollUser(int payrollUserId, Dictionary<int, List<Ins_Timekeeper_log_GetListByAccountMapID_Simple_Result>> timekeeperDict, Dictionary<int, List<Ins_ShiftAssignment_User_WorkingDay_GetDateToDate_Result>> chamCongHoDict)
        {
            var result = new List<Ins_Timekeeper_log_GetListByAccountMapID_Simple_Result>();
            if (timekeeperDict.ContainsKey(payrollUserId))
                result.AddRange(timekeeperDict[payrollUserId]);
            if (chamCongHoDict.ContainsKey(payrollUserId))
            {
                result.AddRange(chamCongHoDict[payrollUserId].Select(x => new Ins_Timekeeper_log_GetListByAccountMapID_Simple_Result
                {
                    ClockType = x.ActionType,
                    LogTime = x.ActionTime
                }));
            }
            return result;
        }
        public static PayrollReportDetail_ShiftStatus BuildStatus(string color, List<string> statusColor, string name)
        {
            return new PayrollReportDetail_ShiftStatus { Color = color, StatusColor = statusColor, Name = name };
        }
    }
}
