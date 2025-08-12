using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TanTamApi.Models;
using MyUtility;
namespace TanTamApi.Helper
{
    /// <summary>
    /// Service xử lý ESP32-CAM streaming
    /// </summary>
    public class ESP32StreamingService
    {
        private static readonly ConcurrentDictionary<string, ESP32StreamModel> _activeStreams = new ConcurrentDictionary<string, ESP32StreamModel>();
        private static readonly string _imagesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "ESP32Images");
        private static readonly string _snapshotsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "ESP32Snapshots");
        private static readonly string _hlsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "ESP32HLS");
        
        private static readonly object _lockObject = new object();
        private static readonly int _maxImagesPerStream = 50;
        private static readonly long _maxStorageBytes = 50L * 1024 * 1024 * 1024; // 50GB

        static ESP32StreamingService()
        {
            // Tạo thư mục cần thiết
            EnsureDirectoriesExist();
            
            // Cleanup timer
            StartCleanupTimer();
        }

        /// <summary>
        /// Đảm bảo các thư mục cần thiết tồn tại
        /// </summary>
        private static void EnsureDirectoriesExist()
        {
            try
            {
                if (!Directory.Exists(_imagesDirectory))
                    Directory.CreateDirectory(_imagesDirectory);
                
                if (!Directory.Exists(_snapshotsDirectory))
                    Directory.CreateDirectory(_snapshotsDirectory);
                
                if (!Directory.Exists(_hlsDirectory))
                    Directory.CreateDirectory(_hlsDirectory);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating directories: {ex.Message}");
            }
        }

        /// <summary>
        /// Bắt đầu timer cleanup
        /// </summary>
        private static void StartCleanupTimer()
        {
            var timer = new System.Threading.Timer(CleanupOldStreams, null, 
                TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
        }

        /// <summary>
        /// Xử lý frame từ ESP32-CAM
        /// </summary>
        public static async Task<ESP32StreamResponse> ProcessFrameAsync(string streamId, byte[] frameData)
        {
            try
            {
                // Tạo hoặc cập nhật stream
                var stream = GetOrCreateStream(streamId);
                stream.FrameCount++;
                stream.TotalBytes += frameData.Length;
                stream.LastFrameTime = DateTime.UtcNow;

                // Log frame info
                System.Diagnostics.Debug.WriteLine($"Processing frame {stream.FrameCount}: {frameData.Length} bytes");
                
                // Lưu frame vào disk
                var imageInfo = await SaveFrameToDiskAsync(streamId, frameData, stream.FrameCount);
                stream.LastFrameUrl = imageInfo.Url;
                System.Diagnostics.Debug.WriteLine($"Frame saved: {imageInfo.Filename}");

                // Broadcast frame qua SignalR
                var frameModel = new ESP32FrameModel
                {
                    StreamId = streamId,
                    Frame = Convert.ToBase64String(frameData),
                    Timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds,
                    FrameNumber = stream.FrameCount,
                    FrameSize = frameData.Length
                };

                ESP32StreamingHub_Simple.BroadcastFrame(streamId, frameModel);

                // Cleanup nếu cần
                await CleanupOldImagesAsync(streamId);

                return new ESP32StreamResponse
                {
                    Status = "success",
                    Message = "Frame processed successfully",
                    StreamId = streamId,
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error processing frame: {ex.Message}");
                return new ESP32StreamResponse
                {
                    Status = "error",
                    Message = ex.Message,
                    StreamId = streamId,
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Lấy hoặc tạo stream mới
        /// </summary>
        private static ESP32StreamModel GetOrCreateStream(string streamId)
        {
            return _activeStreams.GetOrAdd(streamId, id => new ESP32StreamModel
            {
                StreamId = id,
                Status = "active",
                StartTime = DateTime.UtcNow,
                FrameCount = 0,
                TotalBytes = 0,
                Viewers = 0
            });
        }

        /// <summary>
        /// Lưu frame vào disk
        /// </summary>
        private static async Task<ESP32ImageInfo> SaveFrameToDiskAsync(string streamId, byte[] frameData, int frameNumber)
        {
            return await Task.Run(() =>
            {
                try
                {
                    // Tạo filename với timestamp
                    var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH-mm-ss-fffZ");
                    var filename = $"{streamId}_frame_{frameNumber}_{timestamp}.jpg";
                    var imagePath = Path.Combine(_imagesDirectory, filename);

                    // Lưu frame
                    File.WriteAllBytes(imagePath, frameData);

                    // Lưu snapshot mới nhất
                    var snapshotPath = Path.Combine(_snapshotsDirectory, $"{streamId}_latest.jpg");
                    File.WriteAllBytes(snapshotPath, frameData);

                    return new ESP32ImageInfo
                    {
                        Filename = filename,
                        Url = $"/App_Data/ESP32Images/{filename}",
                        Size = frameData.Length,
                        Created = DateTime.UtcNow,
                        StreamId = streamId
                    };
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error saving frame: {ex.Message}");
                    throw;
                }
            });
        }

        /// <summary>
        /// Cleanup ảnh cũ
        /// </summary>
        private static async Task CleanupOldImagesAsync(string streamId)
        {
            await Task.Run(() =>
            {
                try
                {
                    var files = Directory.GetFiles(_imagesDirectory, $"{streamId}_frame_*")
                        .Select(f => new FileInfo(f))
                        .OrderByDescending(f => f.CreationTime)
                        .ToList();

                    // Xóa ảnh cũ, chỉ giữ maxImagesPerStream
                    if (files.Count > _maxImagesPerStream)
                    {
                        var filesToDelete = files.Skip(_maxImagesPerStream);
                        foreach (var file in filesToDelete)
                        {
                            try
                            {
                                file.Delete();
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error deleting file {file.Name}: {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error cleaning up images: {ex.Message}");
                }
            });
        }

        /// <summary>
        /// Cleanup streams cũ
        /// </summary>
        private static void CleanupOldStreams(object state)
        {
            try
            {
                var now = DateTime.UtcNow;
                var oldStreams = new List<string>();

                foreach (var kvp in _activeStreams)
                {
                    var age = now - kvp.Value.StartTime;
                    if (age.TotalMinutes > 30) // 30 phút
                    {
                        oldStreams.Add(kvp.Key);
                    }
                }

                foreach (var streamId in oldStreams)
                {
                    ESP32StreamModel removed;
                    _activeStreams.TryRemove(streamId, out removed);
                    System.Diagnostics.Debug.WriteLine($"Cleaned up old stream: {streamId}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cleaning up streams: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách streams đang hoạt động
        /// </summary>
        public static List<ESP32StreamModel> GetActiveStreams()
        {
            return _activeStreams.Values.ToList();
        }

        /// <summary>
        /// Lấy thông tin stream
        /// </summary>
        public static ESP32StreamModel GetStream(string streamId)
        {
            ESP32StreamModel stream;
            if (_activeStreams.TryGetValue(streamId, out stream))
            {
                return stream;
            }
            return null;
        }

        /// <summary>
        /// Lấy danh sách ảnh của stream
        /// </summary>
        public static async Task<ESP32ImagesResponse> GetStreamImagesAsync(string streamId = null)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var files = Directory.GetFiles(_imagesDirectory, "*.jpg")
                        .Select(f => new FileInfo(f))
                        .Where(f => streamId == null || f.Name.StartsWith($"{streamId}_frame_"))
                        .OrderByDescending(f => f.CreationTime)
                        .Select(f => new ESP32ImageInfo
                        {
                            Filename = f.Name,
                            Url = $"/App_Data/ESP32Images/{f.Name}",
                            Size = f.Length,
                            Created = f.CreationTime,
                            StreamId = f.Name.Split('_')[0]
                        })
                        .ToList();

                    return new ESP32ImagesResponse
                    {
                        Total = files.Count,
                        Images = files
                    };
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error getting images: {ex.Message}");
                    return new ESP32ImagesResponse
                    {
                        Total = 0,
                        Images = new List<ESP32ImageInfo>()
                    };
                }
            });
        }

        /// <summary>
        /// Lấy snapshot mới nhất
        /// </summary>
        public static string GetLatestSnapshotPath(string streamId)
        {
            try
            {
                var snapshotPath = Path.Combine(_snapshotsDirectory, $"{streamId}_latest.jpg");
                if (File.Exists(snapshotPath))
                {
                    return snapshotPath;
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting snapshot: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Lấy health status
        /// </summary>
        public static ESP32HealthResponse GetHealthStatus()
        {
            try
            {
                var totalSize = Directory.GetFiles(_imagesDirectory, "*.jpg")
                    .Sum(f => new FileInfo(f).Length);

                return new ESP32HealthResponse
                {
                    Status = "ok",
                    Uptime = Environment.TickCount / 1000.0,
                    Streams = _activeStreams.Count,
                    Clients = 0, // Sẽ được cập nhật từ SignalR Hub
                    ServerTime = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting health status: {ex.Message}");
                return new ESP32HealthResponse
                {
                    Status = "error",
                    Uptime = 0,
                    Streams = 0,
                    Clients = 0,
                    ServerTime = DateTime.UtcNow
                };
            }
        }
    }
} 