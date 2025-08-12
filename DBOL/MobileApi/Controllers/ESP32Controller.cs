using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
//using System.Web.Http.Cors;
using TanTamApi.Helper;
using TanTamApi.Models;
using System.Linq; // Added for .FirstOrDefault()
using System.Collections.Generic; // Added for .ToList()

namespace TanTamApi.Controllers
{
    /// <summary>
    /// ESP32-CAM Streaming Controller
    /// </summary>
    [RoutePrefix("api/esp32")]
    public class ESP32Controller : ApiController
    {
        /// <summary>
        /// Nhận frame từ ESP32-CAM
        /// </summary>
        [HttpPost]
        [Route("stream")]
        public async Task<ESP32StreamResponse> ReceiveFrame()
        {
            try
            {
                // Lấy stream ID từ header
                string streamId = "default";
                if (Request.Headers.Contains("X-Stream-Id"))
                {
                    streamId = Request.Headers.GetValues("X-Stream-Id").FirstOrDefault();
                }

                System.Diagnostics.Debug.WriteLine($"Received POST from ESP32-CAM: {streamId}");
                System.Diagnostics.Debug.WriteLine($"Content-Type: {Request.Content.Headers.ContentType}");
                System.Diagnostics.Debug.WriteLine($"Content-Length: {Request.Content.Headers.ContentLength}");

                // Đọc raw body data
                var frameData = await Request.Content.ReadAsByteArrayAsync();
                
                if (frameData != null && frameData.Length > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Received frame data: {frameData.Length} bytes");
                    
                    // Validate frame data
                    var frameInfo = ESP32FrameValidator.GetFrameInfo(frameData);
                    System.Diagnostics.Debug.WriteLine($"Frame analysis: {frameInfo}");
                    
                    if (!ESP32FrameValidator.IsValidJPEG(frameData))
                    {
                        System.Diagnostics.Debug.WriteLine("⚠ Warning: Frame data may not be valid JPEG");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("✓ Valid JPEG frame detected");
                    }
                    
                    // Xử lý frame
                    var response = await ESP32StreamingService.ProcessFrameAsync(streamId, frameData);
                    return response;
                }
                else
                {
                    return new ESP32StreamResponse
                    {
                        Status = "error",
                        Message = "No frame data received",
                        StreamId = streamId,
                        Timestamp = DateTime.UtcNow
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ReceiveFrame: {ex.Message}");
                return new ESP32StreamResponse
                {
                    Status = "error",
                    Message = ex.Message,
                    StreamId = "unknown",
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Lấy danh sách streams đang hoạt động
        /// </summary>
        [HttpGet]
        [Route("streams")]
        public IHttpActionResult GetStreams()
        {
            try
            {
                var streams = ESP32StreamingService.GetActiveStreams();
                var streamList = streams.Select(s => new
                {
                    s.StreamId,
                    s.Status,
                    Viewers = ESP32StreamingHub_Simple.GetStreamViewers(s.StreamId),
                    s.FrameCount,
                    Uptime = (DateTime.UtcNow - s.StartTime).TotalMilliseconds
                }).ToList();

                return Ok(streamList);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting streams: {ex.Message}");
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Lấy trạng thái stream cụ thể
        /// </summary>
        [HttpGet]
        [Route("stream/{id}/status")]
        public IHttpActionResult GetStreamStatus(string id)
        {
            try
            {
                var stream = ESP32StreamingService.GetStream(id);
                if (stream != null)
                {
                    var status = new
                    {
                        stream.StreamId,
                        stream.Status,
                        Viewers = ESP32StreamingHub_Simple.GetStreamViewers(id),
                        Uptime = (DateTime.UtcNow - stream.StartTime).TotalMilliseconds,
                        stream.FrameCount,
                        stream.LastFrameUrl
                    };
                    return Ok(status);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting stream status: {ex.Message}");
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách ảnh đã lưu
        /// </summary>
        [HttpGet]
        [Route("images/{streamId?}")]
        public async Task<IHttpActionResult> GetImages(string streamId = null)
        {
            try
            {
                var images = await ESP32StreamingService.GetStreamImagesAsync(streamId);
                return Ok(images);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting images: {ex.Message}");
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Lấy snapshot mới nhất
        /// </summary>
        [HttpGet]
        [Route("snapshot/{streamId}")]
        public IHttpActionResult GetSnapshot(string streamId)
        {
            try
            {
                var snapshotPath = ESP32StreamingService.GetLatestSnapshotPath(streamId);
                if (!string.IsNullOrEmpty(snapshotPath) && File.Exists(snapshotPath))
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    var fileStream = new FileStream(snapshotPath, FileMode.Open, FileAccess.Read);
                    response.Content = new StreamContent(fileStream);
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("inline")
                    {
                        FileName = $"{streamId}_latest.jpg"
                    };
                    return ResponseMessage(response);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting snapshot: {ex.Message}");
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Lấy ảnh cụ thể
        /// </summary>
        [HttpGet]
        [Route("image/{filename}")]
        public IHttpActionResult GetImage(string filename)
        {
            try
            {
                var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "ESP32Images", filename);
                if (File.Exists(imagePath))
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                    response.Content = new StreamContent(fileStream);
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("inline")
                    {
                        FileName = filename
                    };
                    return ResponseMessage(response);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting image: {ex.Message}");
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Health check
        /// </summary>
        [HttpGet]
        [Route("health")]
        public IHttpActionResult GetHealth()
        {
            try
            {
                var health = ESP32StreamingService.GetHealthStatus();
                health.Clients = ESP32StreamingHub_Simple.GetActiveStreams().Count; // Tạm thời
                return Ok(health);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting health: {ex.Message}");
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Xóa stream và ảnh liên quan
        /// </summary>
        [HttpDelete]
        [Route("stream/{id}")]
        public IHttpActionResult DeleteStream(string id)
        {
            try
            {
                // TODO: Implement stream deletion
                return Ok(new { message = "Stream deletion not implemented yet" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting stream: {ex.Message}");
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Xóa ảnh cũ
        /// </summary>
        [HttpPost]
        [Route("cleanup")]
        public IHttpActionResult CleanupOldImages()
        {
            try
            {
                // TODO: Implement manual cleanup
                return Ok(new { message = "Manual cleanup not implemented yet" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in cleanup: {ex.Message}");
                return InternalServerError(ex);
            }
        }
    }
} 