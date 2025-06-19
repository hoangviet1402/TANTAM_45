/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: Common define common static function
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace MyUtility
{
    public class Common
    {
        public static bool IsAllNumber(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            return value.All(char.IsNumber);
        }

        #region RANDOM_STRING

        /// <summary>
        ///     Trả về chuỗi random
        /// </summary>
        /// <param name="size">độ dài của chuỗi</param>
        /// <param name="lowerCase">viết hoa hay thường.True:Viết hoa,Flase:Viết thường</param>
        /// <returns>Chuỗi sau khi random</returns>
        public static string RandomString(int size, bool lowerCase)
        {
            var builder = new StringBuilder();
            var random = new Random();
            char ch;

            var rndint = new Random();
            var rnd = new Random();
            for (var i = 0; i < size; i++)
            {
                var so = rnd.Next(0, 2);
                if (so == 1)
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                else
                    ch = Convert.ToChar(rndint.Next(0, 9).ToString());

                builder.Append(ch);
            }

            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        #endregion

        /// <summary>
        ///     Giải mã chuỗi Json thành Object và bỏ qua Catch
        /// </summary>
        /// <typeparam name="T">Kiểu đối tượng muốn chuyển đổi thành</typeparam>
        /// <param name="value">Giá trị chuỗi Json</param>
        /// <returns></returns>
        public static T TryDeserializeObject<T>(string value)
        {
            var retValue = default(T);
            try
            {
                // JsonConvert.DefaultSettings = () => new JsonSerializerSettings { MaxDepth = 128 };
                retValue = JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception ex)
            {
                //throw ex;                
            }

            return retValue;
        }

        /// <summary>
        ///     Convert Object thành chuỗi Json
        /// </summary>
        /// <param name="obj">Đối tượng muốn convert</param>
        /// <returns></returns>
        public static string TrySerializeObject(object obj)
        {
            string retValue = null;
            try
            {
                retValue = JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                //throw ex;                
            }

            return retValue;
        }

        #region time valid

        /// <summary>
        ///     kiểm tra thời gian hợp lệ
        /// </summary>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        public static bool CheckTimeString(string timeStart, string timeEnd)
        {
            if (string.IsNullOrEmpty(timeStart) && string.IsNullOrEmpty(timeEnd))
                return true;

            if (string.IsNullOrEmpty(timeStart))
                timeStart = "00:00:00";

            if (string.IsNullOrEmpty(timeEnd))
                timeStart = "23:59:59";

            if (timeStart == "00:00:00" && timeEnd == "23:59:59")
                return true;

            var now = DateTime.Now;
            var dateFrom = DateTime.ParseExact(string.Format("{0} {1}", now.ToString("yyyy-MM-dd"), timeStart),
                "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            var dateTo = DateTime.ParseExact(string.Format("{0} {1}", now.ToString("yyyy-MM-dd"), timeEnd),
                "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            // data đẹp
            if (dateFrom < dateTo)
            {
                if (now >= dateFrom && now < dateTo)
                    return true;

                return false;
            }

            // từ 23h -> 1h
            if (dateFrom > dateTo && now >= dateFrom && now < dateTo.AddDays(1))
                return true;

            // từ 3h -> 1h
            if (now >= dateFrom.AddDays(-1) && now < dateTo)
                return true;

            return false;
        }

        #endregion

        public static T DynamicParser<T>(dynamic value, T defaultValue)
        {
            try
            {
                return (T)value;
            }
            catch
            {
                return defaultValue;
            }
        }


        #region ma hoa

        #region md5

 
        public static string MD5_encode(string str_encode)
        {
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();
            using (var md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str_encode));
                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                foreach (var t in data) sBuilder.Append(t.ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        #endregion

        #region HMAC SHA 256

        /// <summary>
        ///     Author: ThongNT
        ///     <para></para>
        ///     Convert byte array to hexa string
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        private static string ConvertByteToString(IEnumerable<byte> buff)
        {
            return buff.Aggregate("", (current, t) => current + t.ToString("X2"));
        }





        #endregion

        private static readonly byte[] Sbox = new byte[255];
        private static readonly byte[] MyKey = new byte[255];


        private static bool StrToByteArray(string hexString, ref byte[] HexAsBytes)
        {
            try
            {
                HexAsBytes = new byte[hexString.Length / 2];
                for (var index = 0; index < HexAsBytes.Length; index++)
                {
                    var byteValue = hexString.Substring(index * 2, 2);
                    HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        private static string CharsToHex(IList<byte> chars)
        {
            var result = string.Empty;
            var len = chars.Count;
            for (var i = 0; i < len; i++) result += string.Format("{0:x2}", chars[i]);
            return result;
        }

        private static byte[] Calculate(IList<byte> plaintxt, IList<byte> psw)
        {
            Initialize(psw);
            byte i = 0;
            byte j = 0;
            var len = plaintxt.Count;
            var cipher = new byte[len];
            for (var a = 0; a < len; a++)
            {
                i = (byte)((i + 1) % 255);
                j = (byte)((j + Sbox[i]) % 255);

                var temp = Sbox[i];
                Sbox[i] = Sbox[j];
                Sbox[j] = temp;

                var idx = (byte)((Sbox[i] + Sbox[j]) % 255);
                int k = Sbox[idx];

                var cipherby = (byte)(plaintxt[a] ^ k);
                cipher[a] = cipherby;
            }

            return cipher;
        }


        private static void Initialize(IList<byte> pwd)
        {
            byte b = 0;
            var intLength = pwd.Count;

            for (byte a = 0; a < 255; a++)
            {
                MyKey[a] = pwd[a % intLength];
                Sbox[a] = a;
            }

            for (byte a = 0; a < 255; a++)
            {
                b = (byte)((b + Sbox[a] + MyKey[a]) % 255);
                var tempSwap = Sbox[a];
                Sbox[a] = Sbox[b];
                Sbox[b] = tempSwap;
            }
        }

        public static TimeSpan ConvetDateTimeToTimeSpan(DateTime dateTime)
        {
            var dateBetween = DateTime.Now - dateTime;
            return dateBetween;
        }

        public static double ConvetDateTimeToUnixTimeSpan(DateTime value)
        {
            return Convert.ToDouble(value.ToString("yyyyMMddHHmmss"));
        }

        public static long ConvetDateTimeToUnixTimeSpanLong(DateTime value)
        {
            try
            {
                return Convert.ToInt64(value.ToString("yyyyMMddHHmmss"));
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static long ConvetDateTimeToTotalMilliseconds(DateTime value)
        {
            return Convert.ToInt64(value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .TotalMilliseconds);
        }

        #endregion

        #region base64

        public static string ConvertToBase64(string plainText)
        {
            try
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(plainTextBytes);
            }
            catch (Exception)
            {
                return plainText;
            }
        }

        public static string ConvertFromBase64(string base64String)
        {
            try
            {
                var byteArray = Convert.FromBase64String(base64String.Replace(" ", "+"));
                return Encoding.UTF8.GetString(byteArray);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        #endregion
    }
}