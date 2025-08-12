# ESP32-CAM Streaming Integration với TANTAM_45

## Tổng quan

Project này tích hợp ESP32-CAM streaming server vào hệ thống TANTAM_45 sử dụng ASP.NET .NET Framework 4.5. Hệ thống hỗ trợ real-time video streaming, lưu trữ ảnh, và quản lý streams.

## Tính năng chính

### 1. Real-time Streaming
- **SignalR Hub**: Giao tiếp real-time giữa ESP32-CAM và web clients
- **Frame Broadcasting**: Tự động broadcast frames đến tất cả viewers
- **Multi-stream Support**: Hỗ trợ nhiều streams đồng thời

### 2. Image Management
- **Automatic Storage**: Tự động lưu frames vào disk
- **Smart Cleanup**: Tự động xóa ảnh cũ (giữ 50 ảnh gần nhất)
- **Snapshot System**: Lưu ảnh mới nhất của mỗi stream

### 3. API Endpoints
- **Streaming**: Nhận và xử lý frames từ ESP32-CAM
- **Status**: Theo dõi trạng thái streams
- **Media**: Quản lý ảnh và snapshots
- **Health**: Monitoring hệ thống

## Cấu trúc Project

### Models
```
Models/
├── ESP32StreamModel.cs          # Stream information
├── ESP32FrameModel.cs           # Frame data
├── ESP32StreamRequest.cs        # Stream request
├── ESP32StreamResponse.cs       # Stream response
├── ESP32ImageInfo.cs            # Image information
├── ESP32ImagesResponse.cs       # Images list response
└── ESP32HealthResponse.cs       # Health status
```

### Controllers
```
Controllers/
├── ESP32Controller.cs           # Web API controller
└── ESP32MvcController.cs        # MVC controller
```

### Helpers
```
Helper/
├── ESP32StreamingHub.cs         # SignalR hub
└── ESP32StreamingService.cs     # Business logic service
```

### Views
```
Views/ESP32/
└── Index.cshtml                 # Streaming test interface
```

## Cài đặt và Cấu hình

### 1. Build Strategy (Step-by-Step)
**Phase 1: Build với CORS từ Web.config (không cần NuGet package)**
1. CORS đã được cấu hình trong `Web.config` và `Global.asax.cs`
2. Sử dụng `ESP32Controller` (đã có CORS support)
3. Sử dụng `ESP32StreamingHub_Simple` (không có SignalR)
4. Build project để verify CORS functionality

**Phase 2: Install và Enable SignalR**
```bash
Install-Package Microsoft.AspNet.SignalR -Version 2.2.3
```
1. Thay thế `ESP32StreamingHub_Simple` bằng `ESP32StreamingHub`
2. Uncomment SignalR code trong `Global.asax.cs`
3. Build để verify full functionality

### 2. NuGet Packages (Step-by-Step)
**Bước 1**: Restore existing packages
```bash
Update-Package -reinstall
```

**Bước 2**: Install CORS package trước
```bash
Install-Package Microsoft.AspNet.WebApi.Cors -Version 5.2.3
```

**Bước 3**: Install SignalR package sau
```bash
Install-Package Microsoft.AspNet.SignalR -Version 2.2.3
```

**Bước 4**: Verify packages.config có:
```xml
<package id="Microsoft.AspNet.WebApi.Cors" version="5.2.3" targetFramework="net45" />
<package id="Microsoft.AspNet.SignalR" version="2.2.3" targetFramework="net45" />
```

**Lưu ý**: 
- Sử dụng SignalR version 2.2.3 để tương thích với .NET Framework 4.5
- `System.Drawing` là assembly built-in của .NET Framework, không cần install qua NuGet

### 2. Web.config
Đã cấu hình:
- **MIME types** cho HLS streaming (.m3u8, .ts)
- **CORS support** (không cần NuGet package):
  ```xml
  <customHeaders>
    <add name="Access-Control-Allow-Origin" value="*" />
    <add name="Access-Control-Allow-Headers" value="Content-Type, Accept, X-Requested-With, X-Stream-Id" />
    <add name="Access-Control-Allow-Methods" value="GET, POST, PUT, DELETE, OPTIONS" />
    <add name="Access-Control-Allow-Credentials" value="true" />
  </customHeaders>
  ```
- **SignalR routing** (sau khi install package)

### 3. Global.asax.cs
Đã thêm:
- **CORS configuration** trong `Application_BeginRequest`:
  ```csharp
  // Handle CORS preflight requests
  if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
  {
      HttpContext.Current.Response.StatusCode = 200;
      HttpContext.Current.Response.End();
  }
  ```
- **SignalR configuration** (sau khi install package):
  ```csharp
  RouteTable.Routes.MapHubs();
  ```

## Sử dụng

### 1. ESP32-CAM Setup
ESP32-CAM cần gửi frames qua HTTP POST đến endpoint:
```
POST /api/esp32/stream
Headers:
  X-Stream-Id: {stream_id}
  Content-Type: application/octet-stream
Body: JPEG frame data
```

### 2. Web Client
Truy cập streaming interface:
```
http://localhost:port/esp32
```

### 3. API Testing
Test ESP32 connection:
```javascript
// Test với JavaScript
var testData = new ArrayBuffer(1024);
$.ajax({
    url: '/api/esp32/stream',
    method: 'POST',
    data: testData,
    headers: {
        'X-Stream-Id': 'test_stream',
        'Content-Type': 'application/octet-stream'
    },
    processData: false
});
```

## API Reference

### Streaming Endpoints

#### POST /api/esp32/stream
Nhận frame từ ESP32-CAM
- **Headers**: `X-Stream-Id` (optional, default: "default")
- **Body**: Raw JPEG data
- **Response**: Stream processing status

#### GET /api/esp32/streams
Lấy danh sách streams đang hoạt động
- **Response**: Array of active streams

#### GET /api/esp32/stream/{id}/status
Lấy trạng thái stream cụ thể
- **Parameters**: `id` - Stream ID
- **Response**: Stream status information

### Media Endpoints

#### GET /api/esp32/images/{streamId?}
Lấy danh sách ảnh đã lưu
- **Parameters**: `streamId` (optional) - Filter by stream
- **Response**: Images list with metadata

#### GET /api/esp32/snapshot/{streamId}
Lấy snapshot mới nhất của stream
- **Parameters**: `streamId` - Stream ID
- **Response**: JPEG image

#### GET /api/esp32/image/{filename}
Lấy ảnh cụ thể
- **Parameters**: `filename` - Image filename
- **Response**: JPEG image

### System Endpoints

#### GET /api/esp32/health
Health check
- **Response**: System status and metrics

#### DELETE /api/esp32/stream/{id}
Xóa stream (TODO: chưa implement)

#### POST /api/esp32/cleanup
Manual cleanup (TODO: chưa implement)

## SignalR Events

### Client Events
```javascript
hub.client.onFrameReceived = function(frame) {
    // Frame received from ESP32-CAM
    console.log('Frame:', frame.frameNumber);
};

hub.client.onStreamUpdate = function(stream) {
    // Stream status updated
    console.log('Stream update:', stream);
};

hub.client.clientJoined = function(clientId, streamId) {
    // New client joined stream
    console.log('Client joined:', clientId);
};

hub.client.clientLeft = function(clientId, streamId) {
    // Client left stream
    console.log('Client left:', clientId);
};
```

### Server Methods
```javascript
// Join stream
hub.server.joinStream(streamId).done(function() {
    console.log('Joined stream');
});

// Leave stream
hub.server.leaveStream(streamId).done(function() {
    console.log('Left stream');
});
```

## Cấu hình Storage

### Thư mục tự động tạo:
- `App_Data/ESP32Images/` - Lưu tất cả frames
- `App_Data/ESP32Snapshots/` - Lưu snapshots mới nhất
- `App_Data/ESP32HLS/` - HLS streams (future use)

### Cleanup Rules:
- **Images**: Giữ 50 ảnh gần nhất mỗi stream
- **Streams**: Tự động cleanup sau 30 phút không hoạt động
- **Timer**: Cleanup chạy mỗi 5 phút

## Monitoring và Logging

### Debug Output
Tất cả operations được log qua `System.Diagnostics.Debug.WriteLine`:
- Frame processing
- Stream management
- Error handling
- Client connections

### Health Metrics
- Active streams count
- Total viewers
- Server uptime
- Storage usage

## Troubleshooting

### Common Issues

#### 1. Build Error: "The type or namespace name 'AspNet' does not exist"
**Nguyên nhân**: Package Microsoft.AspNet.SignalR chưa được install hoặc version không tương thích

**Giải pháp**:
1. **Restore NuGet packages**:
   ```bash
   Update-Package -reinstall
   ```
   
2. **Install SignalR package manually**:
   ```bash
   Install-Package Microsoft.AspNet.SignalR -Version 2.2.3
   ```
   
3. **Verify package references** trong .csproj file
   
4. **Clean và Rebuild** solution

#### 2. Build Error: "Unable to find version '4.0.0.0' of package 'System.Drawing'"
**Nguyên nhân**: `System.Drawing` là assembly built-in của .NET Framework, không phải NuGet package

**Giải pháp**:
1. **Loại bỏ System.Drawing khỏi packages.config** (đã được sửa)
2. **System.Drawing sẽ tự động available** khi project reference .NET Framework 4.5
3. **Verify project target framework** là .NET Framework 4.5

#### 3. Build Error: "The type or namespace name 'EnableCorsAttribute' could not be found"
**Nguyên nhân**: Package `Microsoft.AspNet.WebApi.Cors` chưa được install

**Giải pháp**: 
- **CORS đã được cấu hình trong Web.config** (không cần NuGet package)
- **Sử dụng ESP32Controller** (đã có CORS support từ Web.config)

#### 4. Build Error: "'ESP32FrameModel' does not contain a definition for 'frameNumber'"
**Nguyên nhân**: Property name mismatch (camel case vs Pascal case)

**Giải pháp**:
1. **Verify property names** trong Models:
   - `ESP32FrameModel.FrameNumber` (Pascal case)
   - `ESP32StreamModel.FrameCount` (Pascal case)
2. **Clean và Rebuild** solution
3. **Check build cache** - có thể cần restart Visual Studio

#### 5. SignalR Connection Failed
- Kiểm tra SignalR package version
- Đảm bảo `RouteTable.Routes.MapHubs()` được gọi
- Check browser console cho JavaScript errors

#### 2. Frame Processing Errors
- Kiểm tra ESP32-CAM data format
- Verify Content-Type header
- Check server logs cho error details

#### 3. Image Storage Issues
- Đảm bảo thư mục `App_Data` có write permissions
- Check disk space
- Verify file path configuration

### Debug Steps
1. Enable browser developer tools
2. Check Network tab cho API calls
3. Monitor Console cho JavaScript errors
4. Verify server-side logging
5. Test với Postman hoặc curl

## Performance Considerations

### Memory Management
- Frames được xử lý async để tránh blocking
- Automatic cleanup prevents memory leaks
- ConcurrentDictionary cho thread-safe stream tracking

### Storage Optimization
- JPEG compression (ESP32-CAM side)
- Automatic cleanup old images
- Configurable retention policies

### Network Efficiency
- SignalR optimizations
- Base64 encoding cho frame transmission
- Efficient group management

## Future Enhancements

### Planned Features
1. **HLS Streaming**: Full HLS support với FFmpeg
2. **RTMP Support**: RTMP server integration
3. **Authentication**: JWT-based stream access control
4. **Recording**: Video recording capabilities
5. **Analytics**: Stream analytics và metrics

### Scalability
- Redis clustering cho multiple servers
- Load balancing support
- Database integration cho stream metadata

## Support và Contact

Để được hỗ trợ hoặc báo cáo issues:
1. Check project documentation
2. Review server logs
3. Test với provided test interface
4. Verify ESP32-CAM configuration

---

**Version**: 1.0.0  
**Last Updated**: 2024  
**Framework**: ASP.NET .NET Framework 4.5  
**Architecture**: MVC + Web API + SignalR 