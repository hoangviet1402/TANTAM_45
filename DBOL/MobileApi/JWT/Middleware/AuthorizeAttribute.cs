using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net;
using System.Net.Http;
using TanTamApi.JWT.Helper;
using MyConfig;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Enum;
using BussinessObject;
using MyUtility;

namespace TanTamApi.JWT.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : System.Web.Mvc.FilterAttribute, System.Web.Mvc.IAuthorizationFilter
    {
        private readonly UserRole[] _roles;

        public AuthorizeAttribute(params UserRole[] roles)
        {
            _roles = roles;
        }
        //Lưu ý:
        //[Authorize] ở controller level là cấu hình mặc định cho tất cả actions 
        //Có thể override bằng cách thêm[Authorize] với tham số khác ở action level vd [Authorize((int)UserRole.SystemAdmin, (int)UserRole.BranchManager)] or [Authorize(1,2)] or [Authorize()]
        //Có thể sử dụng[AllowAnonymous] để cho phép truy cập công khai
        //Authorization ở action level sẽ override authorization ở controller level
        // **** Request -> Authentication Middleware -> Authorization Pipeline -> OnAuthorization -> Controller Action
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var context = filterContext.HttpContext;
            var accountId = TryConvertToInt(context.Items["AccountId"]);
            var companyId = TryConvertToInt(context.Items["CompanyId"]);
            var userRole = TryConvertToInt(context.Items["Role"]);
           
            var result = AuthorizeHelper.CheckAuthorization(accountId, companyId, userRole, _roles);
            if (!result.IsAuthorized)
            {
                if (result.ErrorCode == 401)
                    filterContext.Result = new HttpStatusCodeResult(401, "Unauthorized");
                else if (result.ErrorCode == 403)
                    filterContext.Result = new HttpStatusCodeResult(403, "Forbidden");
                return;
            }
        }

        private int TryConvertToInt(object value)
        {
            if (value == null)
                return 0;

            return int.TryParse(value.ToString(), out int result) ? result : 0;
        }
    }

    // Helper chứa logic dùng chung cho cả MVC và Web API
    public static class AuthorizeHelper
    {
        public class AuthorizationResult
        {
            public bool IsAuthorized { get; set; }
            public int ErrorCode { get; set; } // 0=OK, 401=Unauthorized, 403=Forbidden
        }

        public static AuthorizationResult CheckAuthorization(int? accountId, int? companyId, int? userRole, UserRole[] roles)
        {
            if (accountId == null || companyId == null || userRole == null || accountId <= 0 || companyId <= 0 || userRole <= 0)
            {
                return new AuthorizationResult { IsAuthorized = false, ErrorCode = 401 };
            }
            if (roles != null && roles.Length > 0 && !roles.Contains((UserRole)userRole.Value))
            {
                return new AuthorizationResult { IsAuthorized = false, ErrorCode = 403 };
            }
            return new AuthorizationResult { IsAuthorized = true, ErrorCode = 0 };
        }
    }

    // Filter cho Web API
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private readonly UserRole[] _roles;

        public ApiAuthorizeAttribute(params UserRole[] roles)
        {
            _roles = roles;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            // Lấy token từ header
            var authHeader = actionContext.Request.Headers.Authorization;
            if (authHeader == null || !authHeader.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(authHeader.Parameter))
            {
                var apiResult = new ApiResult<object>
                {
                    Code = (int)ResponseResultEnum.Unauthorized,
                    Data = null,
                    Message = "Missing or invalid Authorization header"
                };
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, apiResult);
                return;
            }

            var token = authHeader.Parameter;
            System.Security.Claims.ClaimsPrincipal principal = null;
            try
            {
                // Validate token (bao gồm kiểm tra hết hạn)
                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var key = System.Text.Encoding.UTF8.GetBytes(MyConfiguration.JWT.SecretKey);
                var parameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = MyConfiguration.JWT.Issuer,
                    ValidateAudience = true,
                    ValidAudience = MyConfiguration.JWT.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(MyConfiguration.JWT.ExpiryInMinutes)
                };
                Microsoft.IdentityModel.Tokens.SecurityToken validatedToken;
                principal = tokenHandler.ValidateToken(token, parameters, out validatedToken);
            }
            catch (Exception ex)
            {
                var apiResult = new ApiResult<object>
                {
                    Code = (int)ResponseResultEnum.Unauthorized,
                    Data = null,
                    Message = "Unauthorized or token expired"
                };
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, apiResult);
                return;
            }

            // Lấy claim từ token đã validate
            int accountId = 0, companyId = 0, userRole = 0;
            string jwtID ="";
            try
            {
                var accountIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "AccountId");
                var companyIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "CompanyId");
                var jwtIDClaim = principal.Claims.FirstOrDefault(c => c.Type == "JwtID");
                var roleClaim = principal.Claims.FirstOrDefault(c => c.Type == "role" || c.Type == "Role" || c.Type == System.Security.Claims.ClaimTypes.Role);
                if (accountIdClaim != null) int.TryParse(accountIdClaim.Value, out accountId);
                if (companyIdClaim != null) int.TryParse(companyIdClaim.Value, out companyId);
                if (roleClaim != null) int.TryParse(roleClaim.Value, out userRole);
                if (jwtIDClaim != null) jwtID = jwtIDClaim.Value;
            }
            catch
            {
                var apiResult = new ApiResult<object>
                {
                    Code = (int)ResponseResultEnum.Unauthorized,
                    Data = null,
                    Message = "Unauthorized"
                };
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, apiResult);
                return;
            }
            var result = AuthorizeHelper.CheckAuthorization(accountId, companyId, userRole, _roles);
            if (!result.IsAuthorized)
            {
                if (result.ErrorCode == 401)
                {
                    var apiResult = new ApiResult<object>
                    {
                        Code = (int)ResponseResultEnum.Unauthorized,
                        Message = "Unauthorized"
                    };
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, apiResult);
                }
                else if (result.ErrorCode == 403)
                {
                    var apiResult = new ApiResult<object>
                    {
                        Code = (int)ResponseResultEnum.Forbidden,
                        Data = null,
                        Message = "Forbidden"
                    };
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, apiResult);
                }
            }
            //else
            //{
            //    var checktoken = BoFactory.Auth.GetTokenInfo(accountId, companyId);
            //    if (checktoken != null && checktoken.JwtID != SecurityCommon.sha256_hash(jwtID))
            //    {
            //        var apiResult = new ApiResult<object>
            //        {
            //            Code = (int)ResponseResultEnum.Unauthorized,
            //            Data = null,
            //            Message = "Đăng nhập tại máy khác"
            //        };
            //        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, apiResult);
            //        return;
            //    }
            //}
        }
    }
}