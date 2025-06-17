using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using Logger;
using TanTamApi.Models.Captcha;
using MyConfig;
using MyUtility;
using MyUtility.Extensions;

namespace TanTamApi.Helper
{
    [Serializable]
    public class CaptchaHelper
    {
        private static string SessionId = "MyCaptcha";
        private static string SessionLoginId = "MyLoginCaptcha";
        private static string SessionDepositId = "DepositCardCaptcha";

        /// <summary>
        ///     Lưu captcha vào session
        ///     <para>thời hạn 10'</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        public static void SaveCaptcha(string name, string text)
        {
            var model = new CaptchaModel { Text = text, ExpiryDate = DateTime.Now.AddMinutes(10) };
            HttpContext.Current.Session[SessionId] = model;
        }

        public static void SaveCaptchaWithName(string name, string text)
        {
            var model = new CaptchaModel { Text = text, ExpiryDate = DateTime.Now.AddMinutes(10) };
            HttpContext.Current.Session[name] = model;
        }

        public static void SaveLoginCaptcha(string name, string text)
        {
            var model = new CaptchaModel { Text = text, ExpiryDate = DateTime.Now.AddMinutes(10) };
            HttpContext.Current.Session[SessionLoginId] = model;
        }

        /// <summary>
        ///     Check captcha trong session
        ///     <para>return false nếu check ko đúng</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool ValidateCaptcha(string name, string text)
        {
            if (HttpContext.Current.Session[SessionId] == null)
                return false;
            var model = HttpContext.Current.Session[SessionId] as CaptchaModel;
            var rs = model != null &&
                     string.Equals(model.Text, text, StringComparison.CurrentCultureIgnoreCase);
            ClearCaptcha();
            return rs;
        }

        public static bool ValidateCaptchaWithName(string name, string text)
        {
            if (HttpContext.Current.Session[name] == null)
                return false;
            var model = HttpContext.Current.Session[name] as CaptchaModel;
            var rs = model != null &&
                     string.Equals(model.Text, text, StringComparison.CurrentCultureIgnoreCase);
            ClearCaptcha(name);
            return rs;
        }

        public static void ClearCaptcha()
        {
            if (HttpContext.Current.Session[SessionId] != null) HttpContext.Current.Session.Remove(SessionId);
        }


        public static void ClearCaptcha(string name)
        {
            if (HttpContext.Current.Session[name] != null) HttpContext.Current.Session.Remove(name);
        }

        public static string GetCaptchaInSessionWithName(string name)
        {
            CaptchaModel model;
            if (HttpContext.Current.Session[name] == null)
            {
                model = new CaptchaModel
                {
                    Text = NewCaptcha(3)
                };
                HttpContext.Current.Session[name] = model;
            }

            model = HttpContext.Current.Session[name] as CaptchaModel;
            return model != null ? model.Text : null;
        }

        public static string GetCaptchaInSessionWithName(string name, ref int first, ref int second, ref int opera)
        {
            CaptchaModel model;
            if (HttpContext.Current.Session[name] == null)
            {
                var rnd = new Random();
                first = rnd.Next(MyConfiguration.Captcha.MinRandom, MyConfiguration.Captcha.MaxRandom);
                second = rnd.Next(MyConfiguration.Captcha.MinRandom, MyConfiguration.Captcha.MaxRandom);
                opera = rnd.Next(MyConfiguration.Captcha.OperatorRandom);

                var value = opera == 0 ? first + second : first - second;

                if (value < 0)
                {
                    var _first = first;

                    first = second;
                    second = _first;
                    value = value * -1;
                }

                model = new CaptchaModel
                {
                    Text = value.ToString()
                };
                HttpContext.Current.Session[name] = model;
            }

            model = HttpContext.Current.Session[name] as CaptchaModel;
            return model != null ? model.Text : null;
        }

        public static string GetCaptchaInSession(string name)
        {
            CaptchaModel model;
            if (HttpContext.Current.Session[SessionId] == null)
            {
                model = new CaptchaModel
                {
                    Text = NewCaptcha(3)
                };
                HttpContext.Current.Session[SessionId] = model;
            }

            model = HttpContext.Current.Session[SessionId] as CaptchaModel;
            return model != null ? model.Text : null;
        }

        public static string NewCaptcha(int lenght)
        {
            var arrChar = MyConfiguration.Captcha.ArrCharacterCaptcha.Split(',');
            //{
            //    'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r',
            //    's', 't', 'v', 'w', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
            //    'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'Y', 'Z', '0', '1', '2', '3', '4', '5',
            //    '6', '7', '8', '9'
            //};
            //char[] arrChar =
            //{
            //    'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'm', 'n', 'o', 'p', 'q', 'r',
            //    's', 't', 'v', 'w', 'y', 'z'
            //};
            //char[] arrChar = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            var rnd = new Random();
            var result = string.Empty;
            for (var i = 0; i < lenght; i++)
            {
                var t = rnd.Next(0, arrChar.Length);
                result += arrChar[t];
            }

            return result;
        }

        public static Bitmap GenerateImage(string text, string fontName, int w = 110, int h = 39)
        {
            // Create a new 32-bit bitmap image.Century Schoolbook

            var bitmap = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            var random = new Random();
            // Create a graphics object for drawing.
            var g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(0, 0, w, h);

            // Fill in the background.
            var hatchBrush = new HatchBrush(
                HatchStyle.SmallConfetti,
                Color.LightGray,
                Color.White);
            g.FillRectangle(hatchBrush, rect);

            // Set up the text font.
            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;
            // Adjust the font size until the text fits within the image.
            do
            {
                fontSize--;
                font = new Font(fontName, fontSize, FontStyle.Bold);
                size = g.MeasureString(text, font);
            } while (size.Width > rect.Width);

            // Set up the text format.
            var format = new StringFormat
                { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            if (font == null) font = new Font(fontName, fontSize, FontStyle.Bold);

            // Create a path using the text and warp it randomly.
            var path = new GraphicsPath();
            path.AddString(text ?? string.Empty, font.FontFamily, font.Style.Value(), font.Size, rect, format);
            const float v = 4F;
            PointF[] points =
            {
                new PointF(
                    random.Next(rect.Width) / v,
                    random.Next(rect.Height) / v),
                new PointF(
                    rect.Width - random.Next(rect.Width) / v,
                    random.Next(rect.Height) / v),
                new PointF(
                    random.Next(rect.Width) / v,
                    rect.Height - random.Next(rect.Height) / v),
                new PointF(
                    rect.Width - random.Next(rect.Width) / v,
                    rect.Height - random.Next(rect.Height) / v)
            };
            var matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

            // Draw the text.
            hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.Black, Color.Black);
            g.FillPath(hatchBrush, path);

            // Add some random noise.
            var m = Math.Max(rect.Width, rect.Height);
            for (var i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                var x = random.Next(rect.Width);
                var y = random.Next(rect.Height);
                w = random.Next(m / 50);
                h = random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }

            // Clean up.
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();

            // Set the image.
            return bitmap;
        }

        public static byte[] BitmapToBytes(Bitmap img)
        {
            var byteArray = new byte[0];
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                stream.Close();
                byteArray = stream.ToArray();
            }

            return byteArray;
        }

        #region using Cache

        public static void ClearCaptcha_Cache(string name)
        {
            if (HttpRuntime.Cache[name] != null) HttpRuntime.Cache.Remove(name);
        }

        public static string GetCaptchaInSessionWithName_Cache(string name)
        {
            CaptchaModel model;
            if (HttpRuntime.Cache[name] == null)
            {
                model = new CaptchaModel
                {
                    Text = NewCaptcha(MyConfiguration.Captcha.NumCharRandom),
                    ExpiryDate = DateTime.Now.AddMinutes(5)
                };
                HttpRuntime.Cache[name] = model;
            }

            model = HttpRuntime.Cache[name] as CaptchaModel;
            return model != null ? model.Text : null;
        }

        public static int ValidateCaptchaWithName_Cache(string name, string text)
        {
            if (HttpRuntime.Cache[name] == null)
                return -1;
            var model = HttpRuntime.Cache[name] as CaptchaModel;
            var rs = model != null && string.Equals(model.Text, text, StringComparison.CurrentCultureIgnoreCase) &&
                     model.ExpiryDate > DateTime.Now;
            ClearCaptcha_Cache(name);
            return rs ? 1 : 0;
        }

        public static byte[] GenerateCaptchar_Cache(string HardwareId, string IpAddress, int w, int h)
        {
            var cacheName = string.Format("{0}{1}", HardwareId, IpAddress);
            cacheName = MaHoa.Encrypt(MyConfiguration.Default.ConfigSecretKey, cacheName);
            ClearCaptcha_Cache(cacheName);
            var captcha = GetCaptchaInSessionWithName_Cache(cacheName);

            //var bitmap = ConvertTextToImage(captcha.ToUpper(), "Arial", 18, bgcolor, fcolor, w, h);
            var bitmap = GenerateImage(captcha, "Century Schoolbook", w, h);
            return BitmapToBytes(bitmap); //Convert bitmap into a byte array
        }

        #endregion


    }
}