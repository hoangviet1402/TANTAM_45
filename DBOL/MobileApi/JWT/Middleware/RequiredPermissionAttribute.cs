using BussinessObject;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using DataAccess;
using Logger;
using MyUtility.Extensions;
using System;
using System.Collections.Generic; // Added for List
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using TanTamApi.JWT.Helper;

namespace TanTamApi.JWT.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequiredPermissionAttribute : AuthorizationFilterAttribute
    {
        private readonly string[] _requiredPermissionKeys;

        public RequiredPermissionAttribute(params string[] requiredPermissionKeys)
        {
            _requiredPermissionKeys = requiredPermissionKeys;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                var employeeId = JwtHelper.GetAccountMapIDFromToken(actionContext.Request);

                if (employeeId <= 0)
                {
                    actionContext.Response = CreateUnauthorizedResponse(actionContext.Request, "Không có quyền truy cập");
                    return;
                }

                // Kiểm tra nếu user có role SystemAdmin = 1 thì luôn có quyền truy cập
                var userRole = JwtHelper.GetRoleFromToken(actionContext.Request);
                
                if (userRole == (int)UserRole.SystemAdmin)
                {
                    // SystemAdmin có quyền truy cập tất cả API, không cần kiểm tra permission
                    // Gán quyền đặc biệt cho controller biết
                    actionContext.Request.Properties["CurrentUserPermissions"] = "SystemAdmin";
                    return;
                }

                bool hasPermission = false;
                object userPermissionKeysObj = null;

                // Lấy danh sách quyền của user
                var userPermissionKeys = DaoFactory.Permission.GetEmployeePermissions(employeeId);
                userPermissionKeysObj = userPermissionKeys;
                
                // Lấy RouteName từ permissions thay vì Key
                var userRouteNames = userPermissionKeys
                    .Select(p => p.RouteName)
                    .Where(rn => !string.IsNullOrEmpty(rn)) // Chỉ lấy những route có giá trị
                    .ToList();
                
                // Lấy Key từ permissions - sử dụng HashSet để tối ưu performance
                var userPermissionKeysSet = new HashSet<string>(
                    userPermissionKeys
                        .Select(p => p.Key)
                        .Where(k => !string.IsNullOrEmpty(k)),
                    StringComparer.OrdinalIgnoreCase);
                
                // Kiểm tra theo key trước (nếu có truyền key)
                if (_requiredPermissionKeys != null && _requiredPermissionKeys.Length > 0)
                {
                    // Check theo key - sử dụng HashSet.Contains() cho performance O(1)
                    hasPermission = _requiredPermissionKeys.All(requiredKey => userPermissionKeysSet.Contains(requiredKey));
                    
                    // Nếu check theo key thành công thì return
                    if (hasPermission)
                    {
                        actionContext.Request.Properties["CurrentUserPermissions"] = userPermissionKeysObj;
                        return;
                    }
                }
                
                // Nếu không có key hoặc check key thất bại, check theo route
                var currentRoute = GetCurrentApiRoute(actionContext);
                hasPermission = CheckPermissionByRoute(currentRoute, userRouteNames);

                // Gắn danh sách quyền vào request để controller lấy
                actionContext.Request.Properties["CurrentUserPermissions"] = userPermissionKeysObj;

                if (!hasPermission)
                {
                    actionContext.Response = CreateUnauthorizedResponse(actionContext.Request, "Không có quyền truy cập");
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResult<object>
                {
                    Code = ResponseResultEnum.SystemError.Value(),
                    Message = "Lỗi hệ thống: " + ex.Message,
                    Data = null
                };
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, errorResponse);
            }
        }

        /// <summary>
        /// Lấy API route hiện tại từ action context
        /// </summary>
        private string GetCurrentApiRoute(HttpActionContext actionContext)
        {
            try
            {
                // Method 1: Lấy từ route data
                var routeData = actionContext.RequestContext.RouteData;
                
                // Lấy controller và action name một cách an toàn
                string controllerName = null;
                string actionName = null;
                
                if (routeData.Values.ContainsKey("controller"))
                {
                    controllerName = routeData.Values["controller"]?.ToString()?.ToLower();
                }
                
                if (routeData.Values.ContainsKey("action"))
                {
                    actionName = routeData.Values["action"]?.ToString()?.ToLower();
                }
                
                // Method 2: Nếu không lấy được từ route data, thử lấy từ action descriptor
                if (string.IsNullOrEmpty(controllerName) || string.IsNullOrEmpty(actionName))
                {
                    var routeFromActionContext = GetRouteFromActionContext(actionContext);
                    if (!string.IsNullOrEmpty(routeFromActionContext))
                    {
                        return routeFromActionContext;
                    }
                }
                else
                {
                    // Tạo route pattern: controller/action
                    var currentRoute = $"{controllerName}/{actionName}";
                    return currentRoute;
                }
                
                // Method 3: Lấy từ request URI nếu vẫn không được
                var routeFromUri = GetRouteFromRequestUri(actionContext.Request);
                return routeFromUri;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"Error getting current route: {ex.Message}", ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Lấy route từ request URI
        /// </summary>
        private string GetRouteFromRequestUri(HttpRequestMessage request)
        {
            try
            {
                var uri = request.RequestUri;
                var path = uri.AbsolutePath;
                
                // Loại bỏ "api/" prefix nếu có
                if (path.StartsWith("/api/", StringComparison.OrdinalIgnoreCase))
                {
                    path = path.Substring(5); // Bỏ "/api/"
                }
                else if (path.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    path = path.Substring(1); // Bỏ "/" đầu
                }
                
                // Loại bỏ "/" cuối nếu có
                if (path.EndsWith("/"))
                {
                    path = path.Substring(0, path.Length - 1);
                }
                
                return path.ToLower();
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"Error getting route from URI: {ex.Message}", ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Fallback method để lấy route từ action context
        /// </summary>
        private string GetRouteFromActionContext(HttpActionContext actionContext)
        {
            try
            {
                var actionDescriptor = actionContext.ActionDescriptor;
                var controllerDescriptor = actionDescriptor.ControllerDescriptor;
                
                var controllerName = controllerDescriptor.ControllerName?.ToLower();
                
                // Lấy route từ Route attribute thay vì tên method
                var routeAttribute = actionDescriptor.GetCustomAttributes<RouteAttribute>().FirstOrDefault();
                string actionRoute = null;
                
                if (routeAttribute != null)
                {
                    actionRoute = routeAttribute.Template?.ToLower();
                }
                
                // Kết hợp controller name với route
                if (!string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(actionRoute))
                {
                    var fullRoute = $"{controllerName}/{actionRoute}";
                    return fullRoute;
                }
                
                // Nếu không có Route attribute, thử lấy từ RoutePrefix + tên method
                if (string.IsNullOrEmpty(actionRoute))
                {
                    var routePrefixAttribute = controllerDescriptor.GetCustomAttributes<RoutePrefixAttribute>().FirstOrDefault();
                    if (routePrefixAttribute != null)
                    {
                        var prefix = routePrefixAttribute.Prefix?.Replace("api/", "").ToLower();
                        var methodName = actionDescriptor.ActionName?.ToLower();
                        actionRoute = $"{prefix}/{methodName}";
                        return actionRoute;
                    }
                }
                
                // Fallback cuối cùng: controller/action
                var actionName = actionDescriptor.ActionName?.ToLower();
                if (!string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(actionName))
                {
                    var fallbackRoute = $"{controllerName}/{actionName}";
                    return fallbackRoute;
                }
                
                return string.Empty;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error($"Error in fallback method: {ex.Message}", ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Tạo response cho trường hợp không có quyền truy cập
        /// </summary>
        private HttpResponseMessage CreateUnauthorizedResponse(HttpRequestMessage request, string message)
        {
            var unauthorizedResponse = new ApiResult<object>
            {
                Code = ResponseResultEnum.Unauthorized.Value(),
                Message = message,
                Data = null
            };
            return request.CreateResponse(HttpStatusCode.OK, unauthorizedResponse);
        }

        /// <summary>
        /// Kiểm tra permission theo route
        /// </summary>
        private bool CheckPermissionByRoute(string currentRoute, List<string> userPermissions)
        {
            if (string.IsNullOrEmpty(currentRoute) || userPermissions == null || !userPermissions.Any())
                return false;

            // Chuyển đổi sang HashSet để tối ưu performance
            var permissionSet = new HashSet<string>(userPermissions, StringComparer.OrdinalIgnoreCase);

            // Kiểm tra exact match trước
            if (permissionSet.Contains(currentRoute))
                return true;

            // Kiểm tra wildcard pattern (ví dụ: employee/*)
            foreach (var permission in userPermissions)
            {
                if (permission.EndsWith("/*"))
                {
                    var permissionPrefix = permission.Substring(0, permission.Length - 2);
                    if (currentRoute.StartsWith(permissionPrefix, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }

            return false;
        }
    }
}