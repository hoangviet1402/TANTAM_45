/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: CommonLogger
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using System.Globalization;

namespace MyUtility.Extensions
{
    public static class NumberExtension
    {
//        public static string ToCurrencyString(this int number, string unit = "")
//        {
//            return ConvertUtility.FormatCurrency(number, unit);
//        }

        public static string ToCurrencyString(this decimal number, bool enableShorten = false, bool showUnit = true,
            bool enableRound = true)
        {
            var unit = "đ";
            var format = "N00";

            if (enableRound == false)
            {
                if (showUnit) return number.ToString("#,##0.00") + unit;
                return number.ToString("#,##0.00");
            }

            if (enableShorten)
            {
                if (number >= 1000000)
                {
                    number = number / 1000000;
                    unit = "tr";
                    format = "N01";
                }
                else if (number >= 1000)
                {
                    number = number / 1000;
                    unit = "k";
                    format = "N00";
                }
            }

            var currency = number.ToString(format, new CultureInfo("vi-VN"));

            return showUnit == false ? currency : string.Format("{0}{1}", currency, unit);
        }


        public static string ToCurrencyString(this decimal? number, bool enableShorten = false, bool showUnit = true,
            bool enableRound = true)
        {
            return ToCurrencyString(number.GetValueOrDefault(), enableShorten, showUnit, enableRound);
        }

        public static string ToCurrencyString(this int number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyString((decimal)number, enableShorten, showUnit);
        }

        public static string ToCurrencyString(this long number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyString((decimal)number, enableShorten, showUnit);
        }

        public static string ToCurrencyString(this int? number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyString((decimal)number.GetValueOrDefault(0), enableShorten, showUnit);
        }

        public static string ToCurrencyString(this long? number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyString((decimal)number.GetValueOrDefault(0), enableShorten, showUnit);
        }

        /// <summary>
        ///     format tiền tệ xu ThinhQHT
        /// </summary>
        /// <param name="number"></param>
        /// <param name="enableShorten"> </param>
        /// <param name="showUnit">= false để ko có chữ VND</param>
        /// <param name="enableRound"> = true để ko  có .00</param>
        /// <returns></returns>
        public static string ToCurrencyStringXu(this decimal number, bool enableShorten = false, bool showUnit = true,
            bool enableRound = true)
        {
            var unit = "";
            var format = "N00";

            if (enableRound == false)
            {
                if (showUnit) return number.ToString("#,##0.00") + unit;
                return number.ToString("#,##0.00");
            }

            if (enableShorten)
            {
                if (number >= 1000000)
                {
                    number = number / 1000000;
                    unit = "tr";
                    format = "N01";
                }
                else if (number >= 1000)
                {
                    number = number / 1000;
                    unit = "k";
                    format = "N00";
                }
            }

            var currency = number.ToString(format, new CultureInfo("vi-VN"));

            return showUnit == false ? currency : string.Format("{0}{1}", currency, unit);
        }

        public static string ToCurrencyStringXu(this decimal? number, bool enableShorten = false, bool showUnit = true,
            bool enableRound = true)
        {
            return ToCurrencyStringXu(number.GetValueOrDefault(), enableShorten, showUnit, enableRound);
        }

        public static string ToCurrencyStringXu(this int number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyStringXu((decimal)number, enableShorten, showUnit);
        }

        public static string ToCurrencyStringXu(this long number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyStringXu((decimal)number, enableShorten, showUnit);
        }

        public static string ToCurrencyStringXu(this int? number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyStringXu((decimal)number.GetValueOrDefault(0), enableShorten, showUnit);
        }

        public static string ToCurrencyStringXu(this long? number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyStringXu((decimal)number.GetValueOrDefault(0), enableShorten, showUnit);
        }

        #region format theo culture

        public static string FormatCoinCultureUs(this decimal myCoin, int round = -1)
        {
            var format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            //return string.Format(CultureInfo.GetCultureInfo("en-us"), format, myCoin).Replace("$", "");
            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureVN(this int myCoin, int round = -1)
        {
            var format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureVN(this double myCoin, int round = -1)
        {
            var format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureVN(this decimal myCoin, int round = -1)
        {
            var format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureVN(this float myCoin, int round = -1)
        {
            var format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            return string.Format(CultureInfo.GetCultureInfo("vi-vn"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureVN(this long myCoin, int round = -1)
        {
            var format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            return string.Format(CultureInfo.GetCultureInfo("vi-vn"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureUs(this float myCoin, int round = -1)
        {
            var format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            //return string.Format(CultureInfo.GetCultureInfo("en-us"), format, myCoin).Replace("$", "");
            return string.Format(CultureInfo.GetCultureInfo("vi-vn"), format, myCoin).Replace("₫", "").Trim();
        }

        /// <summary>
        ///     <para>-1: sẽ có 2 số sau dấu .</para>
        ///     <para>0: sẽ ko có số sau dấu .</para>
        ///     <para>>0: sẽ có >0 số sau dấu .</para>
        /// </summary>
        /// <param name="myCoin"></param>
        /// <param name="round"></param>
        /// <returns></returns>
        public static string FormatCoinCultureUs(this int myCoin, int round = -1)
        {
            var format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            //return string.Format(CultureInfo.GetCultureInfo("en-us"), format, myCoin).Replace("$", "");
            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureUs(this double myCoin, int round = -1)
        {
            var format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            //return string.Format(CultureInfo.GetCultureInfo("en-us"), format, myCoin).Replace("$", "");
            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureUs(this long myCoin, int round = -1)
        {
            var format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            //return string.Format(CultureInfo.GetCultureInfo("en-us"), format, myCoin).Replace("$", "");
            return string.Format(CultureInfo.GetCultureInfo("vi-vn"), format, myCoin).Replace("₫", "").Trim();
        }

        #endregion
    }
}