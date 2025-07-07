using System;
using System.Collections.Generic;
using System.Linq;
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
            public decimal WorkingHour { get; set; }
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
        private static decimal CalculateWorkingHours(int startHour, int startMinute, int endHour, int endMinute)
        {
            decimal startDecimal = startHour + (startMinute / 60.0m);
            decimal endDecimal = endHour + (endMinute / 60.0m);

            if (endDecimal >= startDecimal)
            {
                // Same day
                return endDecimal - startDecimal;
            }
            else
            {
                // Cross midnight (night shift)
                return (24.0m - startDecimal) + endDecimal;
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

                    return new ShiftTimeConfig
                    {
                        StartTime = FormatTime(startHour, startMinute),
                        EndTime = FormatTime(endHour, endMinute),
                        WorkingHour = CalculateWorkingHours(startHour, startMinute, endHour, endMinute)
                    };
                }
                else
                {
                    // Use default fallback values
                    return new ShiftTimeConfig
                    {
                        StartTime = "08:00:00",
                        EndTime = "17:30:00",
                        WorkingHour = 9.5m
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
                    WorkingHour = 9.5m
                };
            }
        }

        /// <summary>
        /// Clear cached lookup data (useful for testing or when data changes)
        /// </summary>
        public static void ClearCache()
        {
            lock (_lookupLock)
            {
                _hourLookup = null;
                _minuteLookup = null;
            }
        }
    }
} 