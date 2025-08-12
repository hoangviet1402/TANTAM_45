using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TanTamApi.Helper;
using TanTamApi.Models;
using System.Linq;
using System.Collections.Generic;

namespace TanTamApi.Controllers
{
    /// <summary>
    /// ESP32-CAM Streaming Controller - Simple Version
    /// TODO: Enable CORS after installing Microsoft.AspNet.WebApi.Cors package
    /// </summary>
    [RoutePrefix("api/esp32")]
    public class ESP32Controller_Simple : ApiController
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
        /// Health check
        /// </summary>
        [HttpGet]
        [Route("health")]
        public IHttpActionResult GetHealth()
        {
            try
            {
                var health = ESP32StreamingService.GetHealthStatus();
                health.Clients = ESP32StreamingHub_Simple.GetActiveStreams().Count;
                return Ok(health);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting health: {ex.Message}");
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Test endpoint
        /// </summary>
        [HttpGet]
        [Route("test")]
        public IHttpActionResult Test()
        {
            return Ok(new { message = "ESP32 Controller is working!", timestamp = DateTime.UtcNow });
        }
    }
} 