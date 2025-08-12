using System;
using System.Collections.Generic;
using System.Linq;
using BussinessObject.Enum;
using DataAccess;
using Logger;

namespace BussinessObject.Helper
{


    /// <summary>
    /// Helper class for shift time configuration processing
    /// Shared across multiple Business Objects to avoid code duplication
    /// </summary>
    public class ShiftTimeConfigHelper
    {
        // ✅ Cache for hour/minute data (static to share across all instances)
        private static Dictionary<int, int> _hourLookup = null;
        private static Dictionary<int, int> _minuteLookup = null;
        private static readonly object _lookupLock = new object();

        /// <summary>
        /// Helper class for shift time configuration result
        /// </summary>
        public class ShiftTimeConfig
        {
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public double WorkingHour { get; set; }
            // ✅ Thêm property mới
            public string StartCheckinOut { get; set; }
            // Thêm 4 property mới
            public string StartCheckin { get; set; }
            public string EndCheckin { get; set; }
            public string StartCheckout { get; set; }
            public string EndCheckout { get; set; }
        }

        /// <summary>
        /// Initialize hour and minute lookup dictionaries
        /// </summary>
        private static void InitializeLookupData()
        {
            lock (_lookupLock)
            {
                if (_hourLookup == null || _minuteLookup == null)
                {
                    try
                    {
                        // Load hour data
                        var hours = DaoFactory.Shift.GetAllHours();
                        _hourLookup = hours?.ToDictionary(h => h.ID, h => h.HourValue ?? 0) ?? new Dictionary<int, int>();

                        // Load minute data
                        var minutes = DaoFactory.Shift.GetAllMinutes();
                        _minuteLookup = minutes?.ToDictionary(m => m.ID, m => m.MinuteValue ?? 0) ?? new Dictionary<int, int>();
                    }
                    catch (Exception ex)
                    {
                        CommonLogger.DefaultLogger.Error("ShiftTimeConfigHelper: Error initializing lookup data", ex);
                        _hourLookup = new Dictionary<int, int>();
                        _minuteLookup = new Dictionary<int, int>();
                    }
                }
            }
        }

        /// <summary>
        /// Get hour value by ID with fallback
        /// </summary>
        private static int GetHourValue(int? hourId, int defaultHour = 8)
        {
            if (hourId == null) return defaultHour;
            return _hourLookup.ContainsKey(hourId.Value) ? _hourLookup[hourId.Value] : defaultHour;
        }

        /// <summary>
        /// Get minute value by ID with fallback
        /// </summary>
        private static int GetMinuteValue(int? minuteId, int defaultMinute = 0)
        {
            if (minuteId == null) return defaultMinute;
            return _minuteLookup.ContainsKey(minuteId.Value) ? _minuteLookup[minuteId.Value] : defaultMinute;
        }

        /// <summary>
        /// Format time from hour and minute values
        /// </summary>
        private static string FormatTime(int hour, int minute)
        {
            return $"{hour:D2}:{minute:D2}:00";
        }

        /// <summary>
        /// Calculate working hours between start and end time
        /// </summary>
        private static double CalculateWorkingHours(int startHour, int startMinute, int endHour, int endMinute)
        {
            double startDecimal = startHour + (startMinute / 60.0);
            double endDecimal = endHour + (endMinute / 60.0);

            if (endDecimal >= startDecimal)
            {
                // Same day
                return endDecimal - startDecimal;
            }
            else
            {
                // Cross midnight (night shift)
                return (24.0 - startDecimal) + endDecimal;
            }
        }

        /// <summary>
        /// Get shift time configuration from database with C# processing
        /// PUBLIC method that can be used by any Business Object
        /// </summary>
        public static ShiftTimeConfig GetShiftTimeConfiguration(int shiftId)
        {
            try
            {
                // ✅ Initialize lookup data first
                InitializeLookupData();

                // Get raw time config from database
                var timeConfigList = DaoFactory.Shift.GetShiftTimeConfig(shiftId);
                var timeConfig = timeConfigList?.FirstOrDefault();

                if (timeConfig != null)
                {
                    // ✅ Process raw data in C# with lookups
                    var startHour = GetHourValue(timeConfig.StartHourId, 8);
                    var startMinute = GetMinuteValue(timeConfig.StartMinuteId, 0);
                    var endHour = GetHourValue(timeConfig.EndHourId, 17);
                    var endMinute = GetMinuteValue(timeConfig.EndMinuteId, 30);

                    // Lấy giờ/phút checkin/check-out nếu có
                    string startCheckin = null, endCheckin = null, startCheckout = null, endCheckout = null;
                    if (timeConfig.StartCheckInHourId != null && timeConfig.StartCheckInMinuteId != null)
                    {
                        var h = GetHourValue(timeConfig.StartCheckInHourId, startHour);
                        var m = GetMinuteValue(timeConfig.StartCheckInMinuteId, startMinute);
                        startCheckin = FormatTime(h, m);
                    }
                    if (timeConfig.EndCheckInHourId != null && timeConfig.EndCheckInMinuteId != null)
                    {
                        var h = GetHourValue(timeConfig.EndCheckInHourId, endHour);
                        var m = GetMinuteValue(timeConfig.EndCheckInMinuteId, endMinute);
                        endCheckin = FormatTime(h, m);
                    }
                    if (timeConfig.StartCheckOutHourId != null && timeConfig.StartCheckOutMinuteId != null)
                    {
                        var h = GetHourValue(timeConfig.StartCheckOutHourId, startHour);
                        var m = GetMinuteValue(timeConfig.StartCheckOutMinuteId, startMinute);
                        startCheckout = FormatTime(h, m);
                    }
                    if (timeConfig.EndCheckOutHourId != null && timeConfig.EndCheckOutMinuteId != null)
                    {
                        var h = GetHourValue(timeConfig.EndCheckOutHourId, endHour);
                        var m = GetMinuteValue(timeConfig.EndCheckOutMinuteId, endMinute);
                        endCheckout = FormatTime(h, m);
                    }

                    return new ShiftTimeConfig
                    {
                        StartTime = FormatTime(startHour, startMinute),
                        EndTime = FormatTime(endHour, endMinute),
                        WorkingHour = CalculateWorkingHours(startHour, startMinute, endHour, endMinute),
                        StartCheckinOut = startCheckin, // Giữ lại property cũ cho tương thích
                        StartCheckin = startCheckin,
                        EndCheckin = endCheckin,
                        StartCheckout = startCheckout,
                        EndCheckout = endCheckout
                    };
                }
                else
                {
                    // Use default fallback values
                    return new ShiftTimeConfig
                    {
                        StartTime = "08:00:00",
                        EndTime = "17:30:00",
                        WorkingHour = 9.5,
                        StartCheckinOut = null,
                        StartCheckin = null,
                        EndCheckin = null,
                        StartCheckout = null,
                        EndCheckout = null
                    };
                }
            }
            catch (Exception ex)
            {
                // Log error and return fallback
                CommonLogger.DefaultLogger.Error($"ShiftTimeConfigHelper: Error getting time config for shift {shiftId}", ex);
                
                return new ShiftTimeConfig
                {
                    StartTime = "08:00:00",
                    EndTime = "17:30:00", 
                    WorkingHour = 9.5,
                    StartCheckinOut = null,
                    StartCheckin = null,
                    EndCheckin = null,
                    StartCheckout = null,
                    EndCheckout = null
                };
            }
        }

        /// <summary>
        /// ✅ NEW: Process raw hour/minute values directly (for use with stored procedures that return raw values)
        /// </summary>
        /// <param name="startHourValue">Start hour value (0-23)</param>
        /// <param name="startMinuteValue">Start minute value (0-59)</param>
        /// <param name="endHourValue">End hour value (0-23)</param>
        /// <param name="endMinuteValue">End minute value (0-59)</param>
        /// <param name="workingDay">Working day for time calculation</param>
        /// <returns>ShiftTimeConfig with calculated values</returns>
        public static ShiftTimeConfig ProcessRawTimeValues(int? startHourValue, int? startMinuteValue, int? endHourValue, int? endMinuteValue, DateTime workingDay)
        {
            try
            {
                // Use defaults if values are null or invalid
                var startHour = ValidateHour(startHourValue) ? startHourValue.Value : 8;
                var startMinute = ValidateMinute(startMinuteValue) ? startMinuteValue.Value : 0;
                var endHour = ValidateHour(endHourValue) ? endHourValue.Value : 17;
                var endMinute = ValidateMinute(endMinuteValue) ? endMinuteValue.Value : 30;

                // Calculate working hours
                var workingHours = CalculateWorkingHours(startHour, startMinute, endHour, endMinute);

                // Format times with working day
                var startTime = workingDay.Date.AddHours(startHour).AddMinutes(startMinute);
                var endTime = workingDay.Date.AddHours(endHour).AddMinutes(endMinute);

                return new ShiftTimeConfig
                {
                    StartTime = startTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndTime = endTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    WorkingHour = workingHours,
                    StartCheckinOut = null,
                    StartCheckin = null,
                    EndCheckin = null,
                    StartCheckout = null,
                    EndCheckout = null
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftTimeConfigHelper: Error processing raw time values", ex);
                
                // Return safe defaults
                return new ShiftTimeConfig
                {
                    StartTime = workingDay.Date.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss"),
                    EndTime = workingDay.Date.AddHours(17).AddMinutes(30).ToString("yyyy-MM-dd HH:mm:ss"),
                    WorkingHour = 9.5,
                    StartCheckinOut = null,
                    StartCheckin = null,
                    EndCheckin = null,
                    StartCheckout = null,
                    EndCheckout = null
                };
            }
        }

        /// <param name="workingDay">Working day</param>
        /// <returns>Week of year (1-53)</returns>
        public static int CalculateWeekOfYear(DateTime workingDay)
        {
            if (workingDay == DateTime.MinValue)
                return 1;

            try
            {
                return System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                    workingDay, 
                    System.Globalization.CalendarWeekRule.FirstDay, 
                    DayOfWeek.Monday);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("ShiftTimeConfigHelper: Error calculating week of year", ex);
                return 1;
            }
        }

        /// <summary>
        /// Validate hour value (0-23)
        /// </summary>
        private static bool ValidateHour(int? hour)
        {
            return hour.HasValue && hour.Value >= 0 && hour.Value <= 23;
        }

        /// <summary>
        /// Validate minute value (0-59)
        /// </summary>
        private static bool ValidateMinute(int? minute)
        {
            return minute.HasValue && minute.Value >= 0 && minute.Value <= 59;
        }

        /// <summary>
        /// Get option name based on action type and clock type
        /// </summary>
        public static string GetCheckinActionDescription(int actionType, int clockType)
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


    }
} 