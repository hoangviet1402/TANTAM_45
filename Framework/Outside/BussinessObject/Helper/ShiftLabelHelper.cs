using System;
using System.Collections.Generic;
using BussinessObject.Enum;

namespace BussinessObject.Helper
{
    /// <summary>
    /// Helper class for shift-related labels and text processing
    /// Centralizes all label formatting logic for shift types, assignment types, etc.
    /// </summary>
    public static class ShiftLabelHelper
    {
        /// <summary>
        /// Get label for generate timekeeping type
        /// </summary>
        /// <param name="type">Generate timekeeping type key</param>
        /// <returns>Vietnamese label for the type</returns>
        public static string GetGenerateTimekeepingTypeLabel(string type)
        {
            if (string.IsNullOrEmpty(type))
                return "Tháng này";

            switch (type.ToLower())
            {
                case "generate_from_start_of_month":
                    return "Tháng này";
                case "generate_from_start_of_week":
                    return "Tuần này";
                default:
                    return "Tháng này";
            }
        }

        /// <summary>
        /// Get label for assignment type
        /// </summary>
        /// <param name="type">Assignment type key</param>
        /// <returns>Vietnamese label for the type</returns>
        public static string GetAssignmentTypeLabel(string type)
        {
            if (string.IsNullOrEmpty(type))
                return "Lặp theo tuần";

            switch (type.ToLower())
            {
                case "weekly_loop":
                    return "Lặp theo tuần";
                case "monthly_loop":
                    return "Lặp theo tháng";
                case "daily_loop":
                    return "Lặp theo ngày";
                default:
                    return "Lặp theo tuần";
            }
        }

        /// <summary>
        /// Get label for shift type
        /// </summary>
        /// <param name="type">Shift type key</param>
        /// <returns>Vietnamese label for the type</returns>
        public static string GetShiftTypeLabel(string type)
        {
            if (string.IsNullOrEmpty(type))
                return "Ca làm việc cố định";

            switch (type.ToLower())
            {
                case "standard_working":
                    return "Ca làm việc cố định";
                case "overtime_working":
                    return "Ca làm thêm giờ";
                case "flexible_working":
                    return "Ca làm việc linh hoạt";
                case "part_time_working":
                    return "Ca làm việc bán thời gian";
                default:
                    return "Ca làm việc cố định";
            }
        }

        /// <summary>
        /// Get all generate timekeeping type options
        /// </summary>
        /// <returns>Dictionary of type keys and labels</returns>
        public static Dictionary<string, string> GetAllGenerateTimekeepingTypes()
        {
            return new Dictionary<string, string>
            {
                { "generate_from_start_of_month", "Tháng này" },
                { "generate_from_start_of_week", "Tuần này" }
            };
        }

        /// <summary>
        /// Get all assignment type options
        /// </summary>
        /// <returns>Dictionary of type keys and labels</returns>
        public static Dictionary<string, string> GetAllAssignmentTypes()
        {
            return new Dictionary<string, string>
            {
                { "weekly_loop", "Lặp theo tuần" },
                { "monthly_loop", "Lặp theo tháng" },
                { "daily_loop", "Lặp theo ngày" }
            };
        }

        /// <summary>
        /// Get all shift type options
        /// </summary>
        /// <returns>Dictionary of type keys and labels</returns>
        public static Dictionary<string, string> GetAllShiftTypes()
        {
            return new Dictionary<string, string>
            {
                { "standard_working", "Ca làm việc cố định" },
                { "overtime_working", "Ca làm thêm giờ" },
                { "flexible_working", "Ca làm việc linh hoạt" },
                { "part_time_working", "Ca làm việc bán thời gian" }
            };
        }

        /// <summary>
        /// Get option name based on action type and clock type
        /// </summary>
        /// <param name="actionType">Action type enum value</param>
        /// <param name="clockType">Clock type enum value</param>
        /// <returns>Vietnamese label for the action and clock type combination</returns>
        public static string GetOptionName(int actionType, int clockType)
        {
            if (!System.Enum.IsDefined(typeof(Shift_ActionType_Enum), actionType) || 
                !System.Enum.IsDefined(typeof(Clock_Type_Enum), clockType))
            {
                return "Khác";
            }

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
    }
} 