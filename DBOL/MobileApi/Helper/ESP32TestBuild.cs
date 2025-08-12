using System;
using MyUtility;
using TanTamApi.Models;

namespace TanTamApi.Helper
{
    /// <summary>
    /// Test class để verify build hoạt động
    /// </summary>
    public static class ESP32TestBuild
    {
        public static void TestBuild()
        {
            try
            {
                // Test ESP32FrameModel
                var frame = new ESP32FrameModel
                {
                    StreamId = "test",
                    Frame = "test_frame",
                    Timestamp = DateTime.UtcNow.ToUnixTimestamp(),
                    FrameNumber = 1,
                    FrameSize = 1024
                };

                // Test ESP32StreamModel
                var stream = new ESP32StreamModel
                {
                    StreamId = "test",
                    Status = "active",
                    FrameCount = 1,
                    StartTime = DateTime.UtcNow
                };

                System.Diagnostics.Debug.WriteLine($"Test build successful: Frame {frame.FrameNumber}, Stream {stream.FrameCount}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Test build failed: {ex.Message}");
            }
        }
    }
} 