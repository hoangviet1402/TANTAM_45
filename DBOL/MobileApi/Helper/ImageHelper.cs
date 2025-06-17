using System;
using System.Text.RegularExpressions;
using Logger;
using TanTamApi.Models.Response;
using TanTamApi.UploadFileService;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;

namespace TanTamApi.Helper
{
    public static class ImageHelper
    {
        /// <summary>
        ///     QuangPN
        ///     Đổi base 64 thành byte
        ///     <para>data:image/png;base64,iVBORw0KGgoAAAANSUhEUg</para>
        ///     <para>ex png|jpg|gif...</para>
        /// </summary>
        /// <returns></returns>
        public static byte[] Base64ToByte(string base64String, out string ex)
        {
            try
            {
                #region Get string

                var base64 = "";
                ex = "";

                if (base64String.IndexOf("base64,", StringComparison.Ordinal) > 0)
                {
                    var pos = base64String.IndexOf("base64,", StringComparison.Ordinal) + 7;
                    base64 = base64String.Substring(pos);
                    var regex = new Regex(@"data:image\/(.*?);base64");
                    var v = regex.Match(base64String);
                    ex = v.Groups[1].ToString().ToLower();
                }

                #endregion

                var bytes = Convert.FromBase64String(base64);
                return bytes;
            }
            catch (Exception e)
            {
                CommonLogger.DefaultLogger.Error("Base64ToByte", e);
                ex = null;
                return null;
            }
        }

        public static UploadFileResponse SaveAvatar(byte[] bytes, int pubUserId, string filename)
        {
            return SaveImageToService(TypeImageUploadEnum.Avatar, bytes, filename);
        }

        public static UploadFileResponse SaveCover(byte[] bytes, int pubUserId, string filename)
        {
            return SaveImageUnResizeToService(TypeImageUploadEnum.Avatar, bytes, filename);
        }

        public static UploadFileResponse SaveImageToService(TypeImageUploadEnum type, byte[] bytes, string path,
            int width = 100, int height = 100)
        {
            try
            {
                var upload = new FileUploadSvc();
                var rs = upload.UploadImageResize(type, path, bytes, width, height);
                return rs.IsSuccess
                    ? new UploadFileResponse { Code = UploadFileResponseCode.Success, Path = rs.ImagePath }
                    : new UploadFileResponse { Code = UploadFileResponseCode.Fail, Message = rs.Message };
            }
            catch (Exception e)
            {
                CommonLogger.DefaultLogger.Error("SaveImageToService", e);
                return new UploadFileResponse { Code = UploadFileResponseCode.Fail, Message = e.Message };
            }
        }

        public static UploadFileResponse SaveImageUnResizeToService(TypeImageUploadEnum type, byte[] bytes, string path)
        {
            try
            {
                var upload = new FileUploadSvc();
                var rs = upload.UploadImage(type, path, bytes);
                return rs.IsSuccess
                    ? new UploadFileResponse { Code = UploadFileResponseCode.Success, Path = rs.ImagePath }
                    : new UploadFileResponse { Code = UploadFileResponseCode.Fail, Message = rs.Message };
            }
            catch (Exception e)
            {
                CommonLogger.DefaultLogger.Error("SaveImageUnResizeToService", e);
                return new UploadFileResponse { Code = UploadFileResponseCode.Fail, Message = e.Message };
            }
        }

        public static async Task<string> ImageToBase64Async(string imagePath) // Make the method async
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                throw new ArgumentNullException(nameof(imagePath), "The image path cannot be null or empty.");
            }

            byte[] imageBytes;

            if (imagePath.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || imagePath.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                // Handle URL path (Download image from URL)
                using (HttpClient httpClient = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await httpClient.GetAsync(imagePath);
                        response.EnsureSuccessStatusCode(); // Ensure we got a successful response (2xx status codes)
                        imageBytes = await response.Content.ReadAsByteArrayAsync();
                    }
                    catch (HttpRequestException ex)
                    {
                        CommonLogger.DefaultLogger.ErrorFormat("Error downloading image from URL: {0} - {1}", ex.Message, imagePath);
                        return null;
                    }
                }
            }
            else
            {
                // Handle local file path
                if (!File.Exists(imagePath))
                {
                    CommonLogger.DefaultLogger.ErrorFormat("The specified image file was not found. {0}", imagePath);
                }
                try
                {
                    imageBytes = File.ReadAllBytes(imagePath);
                }
                catch (Exception ex)
                {
                    CommonLogger.DefaultLogger.ErrorFormat("Error reading local image file: {0}", ex.Message);
                    return null;
                }
            }

            try
            {
                // Convert the byte array to a Base64 string.
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("Error converting image to Base64: {0}", ex);
                return null;
            }
        }

        public static string ImageToBase64(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                CommonLogger.DefaultLogger.ErrorFormat("The image path cannot be null or empty. {0}", imagePath);
                return null;
            }

            byte[] imageBytes;

            if (imagePath.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || imagePath.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = httpClient.GetAsync(imagePath).Result;
                        response.EnsureSuccessStatusCode();
                        imageBytes = response.Content.ReadAsByteArrayAsync().Result;
                    }
                    catch (HttpRequestException ex)
                    {
                        CommonLogger.DefaultLogger.ErrorFormat("Error downloading image from URL: {0} - {1}", ex.Message, imagePath);
                        return null;
                    }
                }
            }
            else
            {
                string directoryName =
                   HttpContext.Current.Server.MapPath(imagePath);
                if (string.IsNullOrEmpty(directoryName))
                {
                    return null;
                }

                if (!File.Exists(directoryName))
                {
                    CommonLogger.DefaultLogger.ErrorFormat("The specified image file was not found. {0}", directoryName);
                }
                try
                {
                    imageBytes = File.ReadAllBytes(directoryName);
                }
                catch (Exception ex)
                {
                    CommonLogger.DefaultLogger.ErrorFormat("Error reading local image file: {0}", ex);
                    return null;
                }
            }

            try
            {
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.ErrorFormat("Error converting image to Base64: {0}", ex);
                return null;
            }
        }

        public static string CleanBase64String(string base64)
        {
            if (string.IsNullOrEmpty(base64))
                return base64;

            // Remove data URL prefix if present
            if (base64.Contains(","))
            {
                base64 = base64.Substring(base64.IndexOf(",") + 1);
            }

            // Remove any whitespace
            base64 = base64.Trim();

            // Validate base64 string
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                // If we got here, it's valid base64
                return base64;
            }
            catch
            {
                throw new ArgumentException("Invalid base64 string");
            }
        }
    }
}