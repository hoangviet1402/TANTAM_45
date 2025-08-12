using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TanTamApi.Models;

namespace TanTamApi.Helper
{
    /// <summary>
    /// Simple version của ESP32StreamingHub để test build
    /// TODO: Thay thế bằng SignalR implementation
    /// </summary>
    public class ESP32StreamingHub_Simple
    {
        private static readonly Dictionary<string, HashSet<string>> _streamGroups = new Dictionary<string, HashSet<string>>();
        private static readonly Dictionary<string, string> _clientStreams = new Dictionary<string, string>();

        /// <summary>
        /// Get current stream viewers count
        /// </summary>
        public static int GetStreamViewers(string streamId)
        {
            if (_streamGroups.ContainsKey(streamId))
            {
                return _streamGroups[streamId].Count;
            }
            return 0;
        }

        /// <summary>
        /// Get all active streams
        /// </summary>
        public static List<string> GetActiveStreams()
        {
            return new List<string>(_streamGroups.Keys);
        }

        /// <summary>
        /// Broadcast frame to stream viewers (placeholder)
        /// </summary>
        public static void BroadcastFrame(string streamId, ESP32FrameModel frame)
        {
            try
            {
                // TODO: Implement SignalR broadcasting
                System.Diagnostics.Debug.WriteLine($"Frame broadcasted to stream {streamId}: {frame.FrameNumber}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error broadcasting frame: {ex.Message}");
            }
        }

        /// <summary>
        /// Broadcast stream status update (placeholder)
        /// </summary>
        public static void BroadcastStreamUpdate(string streamId, ESP32StreamModel stream)
        {
            try
            {
                // TODO: Implement SignalR broadcasting
                System.Diagnostics.Debug.WriteLine($"Stream update broadcasted: {streamId}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error broadcasting stream update: {ex.Message}");
            }
        }
    }
} 