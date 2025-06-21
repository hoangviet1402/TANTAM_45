using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using MyUtility.Extensions;

namespace MyUtility
{
    public static class DateTimeExtension
    {
        public enum DateFormat
        {
            [Description("dd/MM/yyyy")] NgayThangNam = 1,

            [Description("yyyy/MM/dd")] NamThangNgay = 2,

            [Description("MM/dd/yyyy")] ThangNgayNam = 3,

            [Description("yyyy-MM-dd")] NamThangNgaySql = 4,

            [Description("yyyyMMdd")] NamThangNgayExport = 5,

            [Description("MM/yyyy")] ThangNam = 6
        }

        public enum GroupDateTypeEnum
        {
            GroupMonthDate = 1,
            GroupDayDate = 2,
            GroupHourDate = 3
        }

        public enum TimeFormat
        {
            Hmmss,

            HHmmss,

            Hmmssfff,

            HHmmssfff
        }

        public static DateTime GetDate19700101 => DateTime.Parse("1970/01/01");

        public static DateTime ParseExact(string datetimeString, DateFormat format)
        {
            switch (format)
            {
                case DateFormat.ThangNgayNam:
                    return ParseExactThangNgayNam(datetimeString);

                case DateFormat.NgayThangNam:
                    return ParseExactNgayThangNam(datetimeString);

                case DateFormat.NamThangNgay:
                    return ParseExactNamThangNgay(datetimeString);

                default:
                    throw new Exception("Not regconize format");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="datetimeString"></param>
        /// <param name="dateFormat"></param>
        /// <param name="timeFormat"></param>
        /// <param name="isGetDateIfError">Neu convert sang datetime bi loi thi tu dong convert sang date thoi</param>
        /// <returns></returns>
        public static DateTime ParseExact(string datetimeString, DateFormat dateFormat, TimeFormat timeFormat,
            bool isGetDateIfError = false)
        {
            if (isGetDateIfError)
                try
                {
                    return ParseExactGeneral(datetimeString, dateFormat, timeFormat);
                }
                catch (Exception)
                {
                    return ParseExact(datetimeString, dateFormat);
                }

            return ParseExactGeneral(datetimeString, dateFormat, timeFormat);
        }

        private static DateTime ParseExactThangNgayNam(string dateTimeString)
        {
            try
            {
                return DateTime.ParseExact(dateTimeString, "MM/dd/yyyy", null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static DateTime ParseExactNgayThangNam(string dateTimeString)
        {
            try
            {
                return DateTime.ParseExact(dateTimeString, "dd/MM/yyyy", null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static DateTime ParseExactNamThangNgay(string dateTimeString)
        {
            try
            {
                return DateTime.ParseExact(dateTimeString, "yyyy/MM/dd", null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private static DateTime ParseExactGeneral(string dateTimeString, DateFormat dateFormat, TimeFormat timeFormat)
        {
            var formatString = "";

            switch (dateFormat)
            {
                case DateFormat.ThangNgayNam:
                    formatString = "MM/dd/yyyy";
                    break;
                case DateFormat.NgayThangNam:
                    formatString = "dd/MM/yyyy";
                    break;
                case DateFormat.NamThangNgay:
                    formatString = "yyyy/MM/dd";
                    break;
                default:
                    throw new Exception("Not regconize date format");
            }

            switch (timeFormat)
            {
                case TimeFormat.HHmmssfff:
                    formatString += " HH:mm:ss.fff";
                    break;
                case TimeFormat.HHmmss:
                    formatString += " HH:mm:ss";
                    break;
                case TimeFormat.Hmmss:
                    formatString += " H:mm:ss";
                    break;
                case TimeFormat.Hmmssfff:
                    formatString += " H:mm:ss.fff";
                    break;
                default:
                    throw new Exception("Not regconize time format");
            }

            try
            {
                return DateTime.ParseExact(dateTimeString, formatString, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     TanPVD: 2015/01/07
        /// </summary>
        /// <param name="dt"></param>
        /// format datetime
        /// <returns></returns>
        public static string FormatDateTime(object dt)
        {
            if (dt == null)
                return string.Empty;
            var datetime = dt.ToString();
            var dateBetween = DateTime.Now - DateTime.Parse(datetime);
            if (dateBetween.Days < 1 && dateBetween.Hours == 0 && dateBetween.Minutes == 0)
                return "vài giây trước";
            if (dateBetween.Days < 1 && dateBetween.Hours == 0 && dateBetween.Minutes > 0)
                return string.Format("{0} phút trước", dateBetween.Minutes);
            if (dateBetween.Days < 1) return string.Format("{0} giờ trước", dateBetween.Hours);
            return DateTime.Parse(datetime).ToString("dd/MM/yyyy");
        }

        /// <summary>
        ///     <para>Author:TrungLD</para>
        ///     <para>DateCreated: 20/01/2015</para>
        ///     chuyển giờ thành định dạng giờ-phút
        /// </summary>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static string ConvertHouseToHouseMinute(double minute)
        {
            return minute >= 60
                ? string.Format(@"{0} Giờ {1} Phút", Math.Floor(minute / 60), Math.Floor(minute % 60))
                : string.Format(@"{0} Phút", Math.Floor(minute % 60));
        }

        /// <summary>
        ///     <para>Author:TrungLD</para>
        ///     <para>DateCreated:02/02/2015</para>
        ///     <para>Description: lấy totalSeconds tính đến thời điểm hiện tại</para>
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static double ConvertTimesToTotalSeconds(DateTime date)
        {
            var span = date - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            return Math.Round(span.TotalSeconds, 0);
        }

        /// <summary>
        ///     <para>Author:TrungLD</para>
        ///     <para>DateCreated:02/02/2015</para>
        ///     <para>Description: tính độ lệch thời gian so với hiện tại</para>
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="eslapedMinutes"></param>
        /// <returns></returns>
        public static bool GetEslapedMinutes(double stime, ref double eslapedMinutes)
        {
            try
            {
                var timeZone = TimeZone.CurrentTimeZone;
                var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);

                var senderTotalSeconds = stime + timeZone.GetUtcOffset(DateTime.Now).TotalSeconds;
                var senderTotalSecondsTimeSpan = TimeSpan.FromSeconds(senderTotalSeconds);

                var receiverDiffTimeSpan = DateTime.UtcNow - origin;
                var tsResult = receiverDiffTimeSpan - senderTotalSecondsTimeSpan;
                eslapedMinutes = tsResult.TotalMinutes;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string CountDownTime(this DateTime? date, string formatHours = "h", string formatMinute = "")
        {
            if (!date.HasValue)
                return "";
            var remaindate = date.Value - DateTime.Now;
            if (remaindate.TotalHours < 24)
                return string.Format("{0}{1} {2}{3}", remaindate.Hours, formatHours, remaindate.ToString("mm"),
                    formatMinute); // time.ToString(@"hhhmm");
            return Math.Floor(remaindate.TotalDays).ToString(CultureInfo.InvariantCulture) + " ngày";
        }

        public static string GetVnDateTimeFormat(this DateTime? date)
        {
            return date.HasValue ? GetVnDateTimeFormat(date.Value) : string.Empty;
        }

        public static string GetVnDateTimeFormat(this DateTime date)
        {
            var formatString = DateFormat.NgayThangNam.Text() + " " + TimeFormat.HHmmss.Text();
            return date.ToString(formatString);
        }


        /// <summary>
        ///     Lấy format ngày tháng
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetVnDateFormat(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        /// <summary>
        ///     Author: QuocTuan
        ///     CreateDate: 27/03/2019
        ///     Description: Format ngày theo Culture
        /// </summary>
        /// <param name="date"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string FormatDateCulture(this DateTime date, string culture = "")
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            if (!string.IsNullOrEmpty(culture)) currentCulture = new CultureInfo(culture, false);

            return date.ToString(new CultureInfo(currentCulture.Name));
        }

        public static List<DateTime> GetDateRangeSeries(DateTime dateFrom, DateTime dateTo, int groupType)
        {
            var result = new List<DateTime>();
            if (dateFrom == null || dateTo == null) return result;

            dateFrom = dateFrom.GetBeginOfDay();
            dateTo = dateTo.GetEndOfDay();

            while (dateFrom < dateTo)
            {
                result.Add(dateFrom);

                switch (groupType)
                {
                    case (int)GroupDateTypeEnum.GroupMonthDate:
                        dateFrom = dateFrom.AddMonths(1);
                        break;

                    case (int)GroupDateTypeEnum.GroupDayDate:
                    default:
                        dateFrom = dateFrom.AddDays(1);
                        break;
                }
            }

            return result;
        }

        private static List<string> GetRegistryTimeZoneIds()
        {
            var timeZones = TimeZoneInfo.GetSystemTimeZones();
            return timeZones.Select(timeZone => timeZone.Id).ToList();
        }

        public static DateTime ConvertToCustomTimeZone(this DateTime source, string displayTimeZoneName)
        {
            if (string.IsNullOrEmpty(displayTimeZoneName))
                return TimeZoneInfo.ConvertTime(source, TimeZoneInfo.Local, TimeZoneInfo.Local);

            var timezones = GetRegistryTimeZoneIds();
            if (timezones == null || !timezones.Any())
                return TimeZoneInfo.ConvertTime(source, TimeZoneInfo.Local, TimeZoneInfo.Local);

            var timezone = timezones.FirstOrDefault(t => t == displayTimeZoneName);
            if (timezone == null)
                return TimeZoneInfo.ConvertTime(source, TimeZoneInfo.Local, TimeZoneInfo.Local);

            var displayTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            var convertedDate = TimeZoneInfo.ConvertTime(source, TimeZoneInfo.Local, displayTimeZoneInfo);
            return convertedDate;
        }

        #region Extension

        public static DateTime? ConvertSeccondToDateTime(this double data)
        {
            var tempDate = DateTime.Now;
            try
            {
                tempDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(data);
            }
            catch (Exception)
            {
            }

            return tempDate;
        }

        public static DateTime ConvertSeccondToDateTime(this int data)
        {
            var tempDate = DateTime.Now;
            try
            {
                tempDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(data);
            }
            catch (Exception)
            {
            }

            return tempDate;
        }

        public static DateTime ConvertSeccondToDateTime(this long data)
        {
            var tempDate = DateTime.Now;
            try
            {
                tempDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(data);
            }
            catch (Exception)
            {
            }

            return tempDate;
        }

        public static double ConvertDateTimeToSeccond(this DateTime data)
        {
            var seccond = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            try
            {
                seccond = (data - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            }
            catch (Exception)
            {
            }

            return seccond;
        }

        public static double ConvertDateTimeToSeccond(this DateTime? data)
        {
            var datas = data ?? DateTime.Now;
            var seccond = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            try
            {
                seccond = (datas - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            }
            catch (Exception)
            {
            }

            return seccond;
        }

        /// <summary>
        ///     Lay ngay dau tien trong tuan cua mot ngay bat ky
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime FirstDateOfWeek(this DateTime date)
        {
            var info = Thread.CurrentThread.CurrentCulture;
            var dOfWeek = info.Calendar.GetDayOfWeek(date);
            var h = new Hashtable();
            h["Sunday"] = 6;
            h["Monday"] = 0;
            h["Tuesday"] = 1;
            h["Wednesday"] = 2;
            h["Thursday"] = 3;
            h["Friday"] = 4;
            h["Saturday"] = 5;
            var indexOfday = double.Parse(h[dOfWeek.ToString()].ToString());
            var tmpDate = date.AddDays(-indexOfday);
            return new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day);
        }

        /// <summary>
        ///     Lay ngay cuoi cung trong tuan cua mot ngay bat ky
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime LastDateOfWeek(this DateTime date)
        {
            var info = Thread.CurrentThread.CurrentCulture;
            var dOfWeek = info.Calendar.GetDayOfWeek(date);
            var h = new Hashtable();
            h["Sunday"] = 6;
            h["Monday"] = 0;
            h["Tuesday"] = 1;
            h["Wednesday"] = 2;
            h["Thursday"] = 3;
            h["Friday"] = 4;
            h["Saturday"] = 5;
            var indexOfday = double.Parse(h[dOfWeek.ToString()].ToString());
            var tmpDate = date.AddDays(6 - indexOfday);
            return new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day, 23, 59, 59);
        }

        /// <summary>
        ///     Ngay dau thang cua mot ngay bat ky
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return DateTime.Parse(date.Year + "-" + date.Month + "-01");
        }

        public static List<int> MonthOfYear()
        {
            return new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        }

        /// <summary>
        ///     Ngay cuoi thang cua mot ngay bat ky
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this DateTime date)
        {
            var tmpDate =
                DateTime.Parse(date.Year + "-" + date.Month + "-" + DateTime.DaysInMonth(date.Year, date.Month));
            return new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day, 23, 59, 59);
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            var diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0) diff += 7;

            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime StartOfDate(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }

        public static DateTime EndOfDate(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59, 997);
        }

        public static DateTime EndOfHour(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 59, 59, 997);
        }

        public static DateTime EndOfMinute(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 59, 997);
        }

        /// <summary>
        ///     <para>Author:TrungLD</para>
        ///     <para>DateCreated: 21/03/2015</para>
        ///     <para>Lấy số tuần hiện tại theo DateTime</para>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetWeekNumber(this DateTime dt)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(dt);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) dt = dt.AddDays(3);

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
            //return weekNo;
        }

        /// <summary>
        ///     <para>Author:TrungLD</para>
        ///     <para>DateCreated: 21/03/2015</para>
        ///     <para>Lấy danh sách số tuần theo năm</para>
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static List<Week> GetWeeksOfTheYear(this int year)
        {
            var firstDayOfYear = new DateTime(year, 1, 1);
            var beginningDayOfWeek = firstDayOfYear.AddDays(-1 * Convert.ToInt32(firstDayOfYear.DayOfWeek));
            var endingDayOfWeek = beginningDayOfWeek.AddDays(6);
            var weekOfYear = 1;
            var weeksOfTheYear = new List<Week>();

            while (beginningDayOfWeek.Year < year + 1)
            {
                var week = new Week { Number = weekOfYear, BeginningOfWeek = beginningDayOfWeek };
                weeksOfTheYear.Add(week);

                beginningDayOfWeek = beginningDayOfWeek.AddDays(7);
                weekOfYear++;
            }

            return weeksOfTheYear;
        }

        public class Week
        {
            public DateTime BeginningOfWeek { get; set; }
            public DateTime EndOfWeek => BeginningOfWeek.AddDays(6);
            public int Number { get; set; }
            public string Text => ToString();

            public override string ToString()
            {
                return DateTime.Now > BeginningOfWeek && DateTime.Now < EndOfWeek
                    ? string.Format(
                        "Week {0} or current week: {1} - {2}",
                        Number,
                        BeginningOfWeek.ToShortDateString(),
                        EndOfWeek.ToShortDateString())
                    : string.Format(
                        "Week {0}: {1} - {2}",
                        Number,
                        BeginningOfWeek.ToShortDateString(),
                        EndOfWeek.ToShortDateString());
            }
        }


        public static DateTime FirstDateOfWeek(int year, int weekOfYear)
        {
            var jan1 = new DateTime(year, 1, 1);
            if (weekOfYear == 1)
                return jan1;
            var _culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            var _uiculture = (CultureInfo)CultureInfo.CurrentUICulture.Clone();

            _culture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
            _uiculture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;

            Thread.CurrentThread.CurrentCulture = _culture;
            Thread.CurrentThread.CurrentUICulture = _uiculture;
            var daysOffset = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;

            var firstMonday = jan1.AddDays(daysOffset);

            var firstWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(jan1,
                CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule,
                CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);

            if (firstWeek <= 1) weekOfYear -= 1;

            return firstMonday.AddDays(weekOfYear * 7);
        }

        /// <summary>
        ///     <para>Author:TrungLD</para>
        ///     <para>DateCreated: 10/04/2015</para>
        ///     <para>Gets the 12:00:00 instance of a DateTime</para>
        /// </summary>
        public static DateTime GetBeginOfDay(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        ///     <para>Author:TrungLD</para>
        ///     <para>DateCreated: 10/04/2015</para>
        ///     <para>Gets the 11:59:59 instance of a DateTime</para>
        /// </summary>
        public static DateTime GetEndOfDay(this DateTime dateTime)
        {
            return GetBeginOfDay(dateTime).AddDays(1).AddSeconds(-1);
        }

        #endregion

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
        }

        public static DateTime UnixTimeStamp_AddSeconds_ToDateTime(double unixTimeStamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        }

        public static TimeResult FormatDateTimeResx(object dt)
        {
            if (dt == null)
                return new TimeResult { TimeString = string.Empty, KeyString = "" };

            var datetime = dt.ToString();
            var parsedDateTime = DateTime.Parse(datetime);
            var dateBetween = DateTime.Now - parsedDateTime;

            if (dateBetween.Days < 1 && dateBetween.Hours == 0 && dateBetween.Minutes == 0)
                return new TimeResult { TimeString = "", KeyString = "GuildGiayTruoc" };

            if (dateBetween.Days < 1 && dateBetween.Hours == 0 && dateBetween.Minutes > 0)
                return new TimeResult
                {
                    TimeString = dateBetween.Minutes.ToString(),
                    KeyString = "MinuteAgo"
                };

            if (dateBetween.Days < 1)
                return new TimeResult
                {
                    TimeString = dateBetween.Hours.ToString(),
                    KeyString = "GuildGioTruoc"
                };

            return new TimeResult
            {
                TimeString = parsedDateTime.ToString("dd/MM/yyyy HH:mm"),
                KeyString = ""
            };
        }

        // Helper class to hold the two string values
        public class TimeResult
        {
            public string TimeString { get; set; }
            public string KeyString { get; set; }
        }
        /// <summary>
        /// Tính thời gian làm việc dựa trên giờ bắt đầu và kết thúc
        /// </summary>
        /// <param name="startHourId">ID giờ bắt đầu</param>
        /// <param name="startMinuteId">ID phút bắt đầu</param>
        /// <param name="endHourId">ID giờ kết thúc</param>
        /// <param name="endMinuteId">ID phút kết thúc</param>
        /// <returns>Số giờ làm việc (làm tròn 2 chữ số thập phân)</returns>
        public static double CalculateWorkingHour(int startHourId, int startMinuteId, int endHourId, int endMinuteId)
        {
            try
            {
                var startTotalMinutes = (startHourId * 60) + startMinuteId;
                var endTotalMinutes = (endHourId * 60) + endMinuteId;

                // Xử lý ca làm việc qua đêm (end time < start time)
                if (endTotalMinutes < startTotalMinutes)
                {
                    endTotalMinutes += 24 * 60; // Thêm 24 giờ
                }

                var diffMinutes = endTotalMinutes - startTotalMinutes;
                return Math.Round(diffMinutes / 60.0, 2);
            }
            catch
            {
                return 0;
            }
        }
    }
}