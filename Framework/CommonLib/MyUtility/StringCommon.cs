﻿using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MyUtility
{
    public class StringCommon
    {
        /// <summary>
        /// Tách và trả về phần số gốc từ số điện thoại nhập vào.
        /// Ví dụ: "0365618300" → "365618300"
        ///        "+84912345678" → "912345678"
        /// </summary>
        public static string ExtractCoreNumber(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
               return "";

            input = Regex.Replace(input, @"[\s\.\-]", "").Trim();

            if (input.StartsWith("+84"))
                return input.Substring(3); // Bỏ +84
            else if (input.StartsWith("84"))
                return input.Substring(2); // Bỏ 84
            else if (input.StartsWith("0"))
                return input.Substring(1); // Bỏ 0

            // Nếu không có prefix thì giữ nguyên
            return input;
        }

        /// <summary>
        /// Loại bỏ dấu tiếng Việt và thay thế khoảng trắng bằng ký tự cho trước
        /// </summary>
        public static string NormalizeText(string input, string separator = "_")
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            // Loại bỏ dấu tiếng Việt
            string text = input.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark).ToArray();
            text = new string(chars).Normalize(NormalizationForm.FormC);
            // Thay thế các ký tự không phải chữ/số bằng separator
            text = Regex.Replace(text, "[^a-zA-Z0-9]+", separator);
            // Loại bỏ separator ở đầu/cuối và thay thế nhiều separator liên tiếp thành 1
            text = Regex.Replace(text, $"{separator}+", separator).Trim(separator.ToCharArray());
            return text;
        }

        /// <summary>
        /// Tạo chuỗi số ngẫu nhiên với độ dài được chỉ định
        /// </summary>
        /// <param name="length">Độ dài mong muốn của chuỗi số</param>
        /// <returns>Chuỗi số có độ dài được chỉ định</returns>
        public static string GenerateUniqueNumber(int length = 11)
        {
            // Lấy timestamp hiện tại (milliseconds) - tương thích với .NET Framework 4.5
            long timestamp = Common.ConvetDateTimeToTotalMilliseconds(DateTime.UtcNow);

            // Tạo số ngẫu nhiên 3 chữ số
            Random random = new Random();
            int randomNum = random.Next(100, 999);

            // Kết hợp timestamp và số ngẫu nhiên
            string result = $"{timestamp}{randomNum}";

            // Đảm bảo độ dài theo yêu cầu bằng cách cắt hoặc thêm số 0
            if (result.Length > length)
            {
                result = result.Substring(0, length);
            }
            else if (result.Length < length)
            {
                result = result.PadLeft(length, '0');
            }

            return result;
        }

        public static string GenerateUniqueString(int length = 11)
        {
            return Guid.NewGuid().ToString("N");
        }
        public static string GenTranId()
        {
            var rd = new Random();
            var transId = DateTime.Now + DateTime.Now.Millisecond.ToString();
            transId = transId.Replace("/", "").Replace(":", "").Replace(" ", "");

            var strRandom = "";
            for (var i = 0; i < 5; i++)
            {
                var randNumber = rd.Next(1, 3) == 1 ? rd.Next(97, 123) : rd.Next(48, 58);
                strRandom = strRandom + (char)randNumber;
            }

            return strRandom + transId;
        }

        /// <summary>
        ///     Tạo chuỗi bảo mật
        ///     <para>Author: PhatVT</para>
        ///     <para>Created Date: 17/12/2014</para>
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateSecureCode(int length)
        {
            var securitycode = "";

            var g = Guid.NewGuid();

            securitycode = g.ToString();

            securitycode = securitycode.Replace("-", "");

            securitycode = securitycode.Substring(0, length);

            return securitycode.ToUpper();
        }

        public static string ToSubstring(int maxLength, string entry)
        {
            if (maxLength == 0) maxLength = 100;
            if (entry.Length + 10 > maxLength)
                entry = entry.Substring(0, maxLength) + "...";
            return entry;
        }

        public static string GetTextID(string title, int maxLength)
        {
            title = UnicodeToASCII(title);
            if (title.Length > maxLength)
                title = title.Substring(0, maxLength).Trim();

            if (title.EndsWith("-"))
                title = title.Substring(0, title.Length - 1);

            return title;
        }

        public static string UnicodeToASCII(string strUnicode)
        {
            var strB = new StringBuilder(strUnicode);

            string[] Unicode_char =
            {
                "\u1EF9", "\u1EF8", "\u1EF7", "\u1EF6", "\u1EF5", "\u1EF4",
                "\u1EF3", "\u1EF2", "\u1EF1", "\u1EF0", "\u1EEF", "\u1EEE", "\u1EED", "\u1EEC", "\u1EEB",
                "\u1EEA", "\u1EE9", "\u1EE8", "\u1EE7", "\u1EE6", "\u1EE5", "\u1EE4", "\u1EE3", "\u1EE2",
                "\u1EE1", "\u1EE0", "\u1EDF", "\u1EDE", "\u1EDD", "\u1EDC", "\u1EDB", "\u1EDA", "\u1ED9",
                "\u1ED8", "\u1ED7", "\u1ED6", "\u1ED5", "\u1ED4", "\u1ED3", "\u1ED2", "\u1ED1", "\u1ED0",
                "\u1ECF", "\u1ECE", "\u1ECD", "\u1ECC", "\u1ECB", "\u1ECA", "\u1EC9", "\u1EC8", "\u1EC7",
                "\u1EC6", "\u1EC5", "\u1EC4", "\u1EC3", "\u1EC2", "\u1EC1", "\u1EC0", "\u1EBF", "\u1EBE",
                "\u1EBD", "\u1EBC", "\u1EBB", "\u1EBA", "\u1EB9", "\u1EB8", "\u1EB7", "\u1EB6", "\u1EB5",
                "\u1EB4", "\u1EB3", "\u1EB2", "\u1EB1", "\u1EB0", "\u1EAF", "\u1EAE", "\u1EAD", "\u1EAC",
                "\u1EAB", "\u1EAA", "\u1EA9", "\u1EA8", "\u1EA7", "\u1EA6", "\u1EA5", "\u1EA4", "\u1EA3",
                "\u1EA2", "\u1EA1", "\u1EA0", "\u01B0", "\u01AF", "\u01A1", "\u01A0", "\u0169", "\u0168",
                "\u0129", "\u0128", "\u0111", "\u0103", "\u0102", "\u00FD", "\u00FA", "\u00F9", "\u00F5",
                "\u00F4", "\u00F3", "\u00F2", "\u00ED", "\u00EC", "\u00EA", "\u00E9", "\u00E8", "\u00E3",
                "\u00E2", "\u00E1", "\u00E0", "\u00DD", "\u00DA", "\u00D9", "\u00D5", "\u00D4", "\u00D3",
                "\u00D2", "\u0110", "\u00CD", "\u00CC", "\u00CA", "\u00C9", "\u00C8", "\u00C3", "\u00C2",
                "\u00C1", "\u00C0"
            };

            string[] Ascii_char =
            {
                "y", "Y", "y", "Y", "y", "Y", "y", "Y", "u", "U", "u",
                "U", "u", "U", "u", "U", "u", "U", "u", "U", "u", "U", "o", "O",
                "o", "O", "o", "O", "o", "O", "o", "O", "o", "O", "o", "O", "o",
                "O", "o", "O", "o", "O", "o", "O", "o", "O", "i", "I", "i", "I", "e",
                "E", "e", "E", "e", "E", "e", "E", "e", "E", "e", "E", "e", "E", "e",
                "E", "a", "A", "a", "A", "a", "A", "a", "A", "a", "A", "a", "A",
                "a", "A", "a", "A", "a", "A", "a", "A", "a", "A", "a", "A", "u", "U",
                "o", "O", "u", "U", "i", "I", "d", "a", "A", "y", "u", "u", "o", "o", "o",
                "o", "i", "i", "e", "e", "e", "a", "a", "a", "a", "Y", "U", "U", "O", "O",
                "O", "O", "D", "I", "I", "E", "E", "E", "A", "A", "A", "A"
            };

            for (var i = 0; i < Ascii_char.Length; i++)
                strB.Replace(Unicode_char[i], Ascii_char[i]);

            var strInput = strB.ToString().ToLower();
            strB = new StringBuilder();

            foreach (var c in strInput)
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '-')
                    strB.Append(c);
                else
                    strB.Append("-");

            return Regex.Replace(strB.ToString(), @"-+", "-");
        }

        public static string RemoveUnicode(string strUnicode)
        {
            var strB = new StringBuilder(strUnicode);

            string[] Unicode_char =
            {
                "\u1EF9", "\u1EF8", "\u1EF7", "\u1EF6", "\u1EF5", "\u1EF4",
                "\u1EF3", "\u1EF2", "\u1EF1", "\u1EF0", "\u1EEF", "\u1EEE", "\u1EED", "\u1EEC", "\u1EEB",
                "\u1EEA", "\u1EE9", "\u1EE8", "\u1EE7", "\u1EE6", "\u1EE5", "\u1EE4", "\u1EE3", "\u1EE2",
                "\u1EE1", "\u1EE0", "\u1EDF", "\u1EDE", "\u1EDD", "\u1EDC", "\u1EDB", "\u1EDA", "\u1ED9",
                "\u1ED8", "\u1ED7", "\u1ED6", "\u1ED5", "\u1ED4", "\u1ED3", "\u1ED2", "\u1ED1", "\u1ED0",
                "\u1ECF", "\u1ECE", "\u1ECD", "\u1ECC", "\u1ECB", "\u1ECA", "\u1EC9", "\u1EC8", "\u1EC7",
                "\u1EC6", "\u1EC5", "\u1EC4", "\u1EC3", "\u1EC2", "\u1EC1", "\u1EC0", "\u1EBF", "\u1EBE",
                "\u1EBD", "\u1EBC", "\u1EBB", "\u1EBA", "\u1EB9", "\u1EB8", "\u1EB7", "\u1EB6", "\u1EB5",
                "\u1EB4", "\u1EB3", "\u1EB2", "\u1EB1", "\u1EB0", "\u1EAF", "\u1EAE", "\u1EAD", "\u1EAC",
                "\u1EAB", "\u1EAA", "\u1EA9", "\u1EA8", "\u1EA7", "\u1EA6", "\u1EA5", "\u1EA4", "\u1EA3",
                "\u1EA2", "\u1EA1", "\u1EA0", "\u01B0", "\u01AF", "\u01A1", "\u01A0", "\u0169", "\u0168",
                "\u0129", "\u0128", "\u0111", "\u0103", "\u0102", "\u00FD", "\u00FA", "\u00F9", "\u00F5",
                "\u00F4", "\u00F3", "\u00F2", "\u00ED", "\u00EC", "\u00EA", "\u00E9", "\u00E8", "\u00E3",
                "\u00E2", "\u00E1", "\u00E0", "\u00DD", "\u00DA", "\u00D9", "\u00D5", "\u00D4", "\u00D3",
                "\u00D2", "\u0110", "\u00CD", "\u00CC", "\u00CA", "\u00C9", "\u00C8", "\u00C3", "\u00C2",
                "\u00C1", "\u00C0"
            };

            string[] Ascii_char =
            {
                "y", "Y", "y", "Y", "y", "Y", "y", "Y", "u", "U", "u",
                "U", "u", "U", "u", "U", "u", "U", "u", "U", "u", "U", "o", "O",
                "o", "O", "o", "O", "o", "O", "o", "O", "o", "O", "o", "O", "o",
                "O", "o", "O", "o", "O", "o", "O", "o", "O", "i", "I", "i", "I", "e",
                "E", "e", "E", "e", "E", "e", "E", "e", "E", "e", "E", "e", "E", "e",
                "E", "a", "A", "a", "A", "a", "A", "a", "A", "a", "A", "a", "A",
                "a", "A", "a", "A", "a", "A", "a", "A", "a", "A", "a", "A", "u", "U",
                "o", "O", "u", "U", "i", "I", "d", "a", "A", "y", "u", "u", "o", "o", "o",
                "o", "i", "i", "e", "e", "e", "a", "a", "a", "a", "Y", "U", "U", "O", "O",
                "O", "O", "D", "I", "I", "E", "E", "E", "A", "A", "A", "A"
            };

            for (var i = 0; i < Ascii_char.Length; i++)
                strB.Replace(Unicode_char[i], Ascii_char[i]);

            var strInput = strB.ToString().ToLower();
            strB = new StringBuilder();

            foreach (var c in strInput)
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '-')
                    strB.Append(c);
                else
                    strB.Append(" ");

            return Regex.Replace(strB.ToString(), @" +", " ");
        }

        public static string RemoveQuote(string strUnicode)
        {
            return string.IsNullOrEmpty(strUnicode)
                ? string.Empty
                : strUnicode.Replace(@"""", string.Empty).Replace(@"'", string.Empty);
        }

        public static bool IsPhoneNumber(string phoneString, int charIndex = 0)
        {
            if (phoneString.Length == 0)
                return false;
            for (int i = charIndex, j = phoneString.Length; i < j; i++)
            {
                var c = phoneString[i];
                try
                {
                    var outInt = 0;
                    if (!int.TryParse(c.ToString(), out outInt))
                        return false;
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }


        #region DecodeFromUtf8

        public static string DecodeFromUtf8(string entry)
        {
            if (string.IsNullOrEmpty(entry))
                return string.Empty;
            var bytes = Encoding.Default.GetBytes(entry);
            return Encoding.UTF8.GetString(bytes);
        }

        #endregion
    }

    #region Remove Html Tag

    /// <summary>
    ///     Methods to remove HTML from strings.
    /// </summary>
    public class HtmlRemoval
    {
        /// <summary>
        ///     Compiled regular expression for performance.
        /// </summary>
        private static readonly Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

        /// <summary>
        ///     Remove HTML from string with Regex.
        /// </summary>
        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        /// <summary>
        ///     Remove HTML from string with compiled Regex.
        /// </summary>
        public static string StripTagsRegexCompiled(string source)
        {
            return _htmlRegex.Replace(source, string.Empty);
        }

        /// <summary>
        ///     Remove HTML from string with compiled Regex.
        /// </summary>
        /// <param name="source">Source to replace</param>
        /// <param name="pattern">Pattern to replace</param>
        /// <param name="rep">String which will replace for pattern</param>
        /// <returns></returns>
        public static string StripTagsRegexCompiled(string source, string pattern, string rep)
        {
            var _regex = new Regex(pattern);
            return StripTagsRegexCompiled(source, _regex, rep);
        }

        /// <summary>
        ///     Remove HTML from string with compiled Regex.
        /// </summary>
        /// <param name="source">Source to replace</param>
        /// <param name="reg">Regex to replace</param>
        /// <param name="rep">String which will replace for Regex</param>
        /// <returns></returns>
        public static string StripTagsRegexCompiled(string source, Regex reg, string rep)
        {
            return reg.Replace(source, rep);
        }

        /// <summary>
        ///     Remove HTML tags from string using char array.
        /// </summary>
        public static string StripTagsCharArray(string source)
        {
            var array = new char[source.Length];
            var arrayIndex = 0;
            var inside = false;

            for (var i = 0; i < source.Length; i++)
            {
                var let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }

                if (let == '>')
                {
                    inside = false;
                    continue;
                }

                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }

            return new string(array, 0, arrayIndex);
        }

        /// <summary>
        ///     Check HTML in data.
        /// </summary>
        public static bool ChecHtmlRegexCompiled(string source)
        {
            return _htmlRegex.IsMatch(source);
        }
    }

    #endregion
}