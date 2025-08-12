using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TanTamApi.Models;

namespace TanTamApi.Helper
{
    /// <summary>
    /// SignalR Hub cho ESP32-CAM streaming
    /// </summary>
    public class ESP32StreamingHub : Hub
    {
        private static readonly Dictionary<string, HashSet<string>> _streamGroups = new Dictionary<string, HashSet<string>>();
        private static readonly Dictionary<string, string> _clientStreams = new Dictionary<string, string>();

        /// <summary>
        /// Client join stream
        /// </summary>
        public async Task JoinStream(string streamId)
        {
            try
            {
                // Join SignalR group
                await Groups.Add(Context.ConnectionId, streamId);
                
                // Track client in stream
                if (!_streamGroups.ContainsKey(streamId))
                {
                    _streamGroups[streamId] = new HashSet<string>();
                }
                _streamGroups[streamId].Add(Context.ConnectionId);
                
                // Track which stream client is watching
                _clientStreams[Context.ConnectionId] = streamId;
                
                // Log
                System.Diagnostics.Debug.WriteLine($"Client {Context.ConnectionId} joined stream: {streamId}");
                
                // Notify other clients
                Clients.OthersInGroup(streamId).clientJoined(Context.ConnectionId, streamId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error joining stream: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Client leave stream
        /// </summary>
        public async Task LeaveStream(string streamId)
        {
            try
            {
                // Leave SignalR group
                await Groups.Remove(Context.ConnectionId, streamId);
                
                // Remove from tracking
                if (_streamGroups.ContainsKey(streamId))
                {
                    _streamGroups[streamId].Remove(Context.ConnectionId);
                    
                    // Clean up empty groups
                    if (_streamGroups[streamId].Count == 0)
                    {
                        _streamGroups.Remove(streamId);
                    }
                }
                
                // Remove client tracking
                _clientStreams.Remove(Context.ConnectionId);
                
                // Log
                System.Diagnostics.Debug.WriteLine($"Client {Context.ConnectionId} left stream: {streamId}");
                
                // Notify other clients
                Clients.OthersInGroup(streamId).clientLeft(Context.ConnectionId, streamId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error leaving stream: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get current stream viewers count
        /// </summary>
        public int GetStreamViewers(string streamId)
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
        public List<string> GetActiveStreams()
        {
            return new List<string>(_streamGroups.Keys);
        }

        /// <summary>
        /// Client connected
        /// </summary>
        public override Task OnConnected()
        {
            System.Diagnostics.Debug.WriteLine($"Client connected: {Context.ConnectionId}");
            return base.OnConnected();
        }

        /// <summary>
        /// Client disconnected
        /// </summary>
        public override Task OnDisconnected(bool stopCalled)
        {
            try
            {
                string streamId = null;
                if (_clientStreams.TryGetValue(Context.ConnectionId, out streamId))
                {
                    // Remove from stream tracking
                    if (_streamGroups.ContainsKey(streamId))
                    {
                        _streamGroups[streamId].Remove(Context.ConnectionId);
                        
                        if (_streamGroups[streamId].Count == 0)
                        {
                            _streamGroups.Remove(streamId);
                        }
                    }
                    
                    _clientStreams.Remove(Context.ConnectionId);
                    
                    // Notify other clients
                    Clients.OthersInGroup(streamId).clientLeft(Context.ConnectionId, streamId);
                }
                
                System.Diagnostics.Debug.WriteLine($"Client disconnected: {Context.ConnectionId}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in OnDisconnected: {ex.Message}");
            }
            
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// Client reconnected
        /// </summary>
        public override Task OnReconnected()
        {
            System.Diagnostics.Debug.WriteLine($"Client reconnected: {Context.ConnectionId}");
            return base.OnReconnected();
        }

        /// <summary>
        /// Broadcast frame to stream viewers
        /// </summary>
        public static void BroadcastFrame(string streamId, ESP32FrameModel frame)
        {
            try
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ESP32StreamingHub>();
                hubContext.Clients.Group(streamId).onFrameReceived(frame);
                
                System.Diagnostics.Debug.WriteLine($"Frame broadcasted to stream {streamId}: {frame.FrameNumber}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error broadcasting frame: {ex.Message}");
            }
        }

        /// <summary>
        /// Broadcast stream status update
        /// </summary>
        public static void BroadcastStreamUpdate(string streamId, ESP32StreamModel stream)
        {
            try
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ESP32StreamingHub>();
                hubContext.Clients.Group(streamId).onStreamUpdate(stream);
                
                System.Diagnostics.Debug.WriteLine($"Stream update broadcasted: {streamId}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error broadcasting stream update: {ex.Message}");
            }
        }
    }
} 