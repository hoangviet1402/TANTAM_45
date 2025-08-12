using System;
using System.Linq;

namespace TanTamApi.Helper
{
    /// <summary>
    /// Helper class để validate JPEG frames từ ESP32-CAM
    /// </summary>
    public static class ESP32FrameValidator
    {
        /// <summary>
        /// Validate JPEG frame data
        /// </summary>
        public static bool IsValidJPEG(byte[] frameData)
        {
            if (frameData == null || frameData.Length < 4)
                return false;

            // JPEG SOI marker: FF D8
            bool hasSOI = frameData[0] == 0xFF && frameData[1] == 0xD8;
            
            // JPEG EOI marker: FF D9 (tìm ở cuối)
            bool hasEOI = false;
            for (int i = frameData.Length - 2; i >= 0; i--)
            {
                if (frameData[i] == 0xFF && frameData[i + 1] == 0xD9)
                {
                    hasEOI = true;
                    break;
                }
            }

            return hasSOI && hasEOI;
        }

        /// <summary>
        /// Get frame info for debugging
        /// </summary>
        public static string GetFrameInfo(byte[] frameData)
        {
            if (frameData == null)
                return "Frame data is null";

            var info = $"Size: {frameData.Length} bytes, ";
            
            if (frameData.Length >= 4)
            {
                var header = BitConverter.ToString(frameData.Take(4).ToArray());
                info += $"Header: {header}, ";
                
                bool hasSOI = frameData[0] == 0xFF && frameData[1] == 0xD8;
                info += $"SOI: {(hasSOI ? "✓" : "✗")}, ";
                
                bool hasEOI = false;
                for (int i = frameData.Length - 2; i >= 0; i--)
                {
                    if (frameData[i] == 0xFF && frameData[i + 1] == 0xD9)
                    {
                        hasEOI = true;
                        break;
                    }
                }
                info += $"EOI: {(hasEOI ? "✓" : "✗")}";
            }
            else
            {
                info += "Too small to analyze";
            }

            return info;
        }

        /// <summary>
        /// Check if frame data looks like valid image
        /// </summary>
        public static bool LooksLikeImage(byte[] frameData)
        {
            if (frameData == null || frameData.Length < 100)
                return false;

            // Check for common image patterns
            bool hasJPEGHeader = frameData[0] == 0xFF && frameData[1] == 0xD8;
            bool hasReasonableSize = frameData.Length > 1000; // At least 1KB
            bool hasVariedContent = frameData.Distinct().Count() > 10; // Not all same bytes

            return hasJPEGHeader && hasReasonableSize && hasVariedContent;
        }
    }
} 