using System;
using System.Collections.Generic;

namespace TanTamApi.Models
{
    /// <summary>
    /// Model cho ESP32-CAM stream
    /// </summary>
    public class ESP32StreamModel
    {
        public string StreamId { get; set; }
        public string Status { get; set; }
        public int Viewers { get; set; }
        public int FrameCount { get; set; }
        public long Uptime { get; set; }
        public DateTime StartTime { get; set; }
        public long TotalBytes { get; set; }
        public string LastFrameUrl { get; set; }
        public DateTime LastFrameTime { get; set; }
    }

    /// <summary>
    /// Model cho frame data từ ESP32-CAM
    /// </summary>
    public class ESP32FrameModel
    {
        public string StreamId { get; set; }
        public string Frame { get; set; } // Base64 encoded JPEG
        public long Timestamp { get; set; }
        public int FrameNumber { get; set; }
        public int FrameSize { get; set; }
    }

    /// <summary>
    /// Model cho stream request
    /// </summary>
    public class ESP32StreamRequest
    {
        public string StreamId { get; set; }
        public string Quality { get; set; }
        public string Resolution { get; set; }
        public int FrameRate { get; set; }
    }

    /// <summary>
    /// Model cho stream response
    /// </summary>
    public class ESP32StreamResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string StreamId { get; set; }
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// Model cho image info
    /// </summary>
    public class ESP32ImageInfo
    {
        public string Filename { get; set; }
        public string Url { get; set; }
        public long Size { get; set; }
        public DateTime Created { get; set; }
        public string StreamId { get; set; }
    }

    /// <summary>
    /// Model cho images list response
    /// </summary>
    public class ESP32ImagesResponse
    {
        public int Total { get; set; }
        public List<ESP32ImageInfo> Images { get; set; }
    }

    /// <summary>
    /// Model cho health check
    /// </summary>
    public class ESP32HealthResponse
    {
        public string Status { get; set; }
        public double Uptime { get; set; }
        public int Streams { get; set; }
        public int Clients { get; set; }
        public DateTime ServerTime { get; set; }
    }
} 