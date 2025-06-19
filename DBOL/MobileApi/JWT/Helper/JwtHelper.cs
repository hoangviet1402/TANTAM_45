using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using MyConfig;
using BussinessObject.Enum;
using BussinessObject.Models.ApiResponse;
using BussinessObject.Models.Auth;
using BussinessObject;
using MyUtility.Extensions;
using MyUtility;

namespace TanTamApi.JWT.Helper
{
    public class JwtHelper
    {
        public static string  GenerateAccessToken(int accountId, int employeeAccountMapId, int companyId, int role,out string jwtID)
        {
            jwtID = GenerateRefreshToken();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(MyConfiguration.JWT.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("AccountId", accountId.ToString()),
                new Claim("EmployeeId", employeeAccountMapId.ToString()),
                new Claim("CompanyId", companyId.ToString()),
                new Claim(ClaimTypes.Role, role.ToString()),               
                new Claim("JwtID", jwtID),
                // new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // ID cho token
                //new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.().ToString(), ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: MyConfiguration.JWT.Issuer,
                audience: MyConfiguration.JWT.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(MyConfiguration.JWT.ExpiryInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// Lấy CompanyId từ JWT token trong Web API
        /// </summary>
        public static int GetCompanyIdFromToken(HttpRequestMessage request)
        {
            try
            {
                var token = GetTokenFromRequest(request);
                if (string.IsNullOrEmpty(token))
                    return 0;

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                
                var companyIdClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "CompanyId");
                if (companyIdClaim != null && int.TryParse(companyIdClaim.Value, out int companyId))
                    return companyId;
                
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Lấy AccountId từ JWT token trong Web API
        /// </summary>
        public static int GetAccountIdFromToken(HttpRequestMessage request)
        {
            try
            {
                var token = GetTokenFromRequest(request);
                if (string.IsNullOrEmpty(token))
                    return 0;

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                
                var accountIdClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "AccountId");
                if (accountIdClaim != null && int.TryParse(accountIdClaim.Value, out int accountId))
                    return accountId;
                
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Lấy EmployeeId từ JWT token trong Web API
        /// </summary>
        public static int GetEmployeeIdFromToken(HttpRequestMessage request)
        {
            try
            {
                var token = GetTokenFromRequest(request);
                if (string.IsNullOrEmpty(token))
                    return 0;

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                
                var employeeIdClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "EmployeeId");
                if (employeeIdClaim != null && int.TryParse(employeeIdClaim.Value, out int employeeId))
                    return employeeId;
                
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Lấy Role từ JWT token trong Web API
        /// </summary>
        public static int GetRoleFromToken(HttpRequestMessage request)
        {
            try
            {
                var token = GetTokenFromRequest(request);
                if (string.IsNullOrEmpty(token))
                    return 0;

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                
                var roleClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "role" || c.Type == "Role");
                if (roleClaim != null && int.TryParse(roleClaim.Value, out int role))
                    return role;
                
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Lấy JWT ID từ JWT token trong Web API
        /// </summary>
        public static string GetJwtIdFromToken(HttpRequestMessage request)
        {
            try
            {
                var token = GetTokenFromRequest(request);
                if (string.IsNullOrEmpty(token))
                    return string.Empty;

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                
                var jtiClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "jti" || c.Type == "JwtID");
                return jtiClaim?.Value ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static ApiResult<AuthResponse> GenerateAuthResponse(int accountId, int employeeId, int companyId, int role, string ip)
        {
            var response = new ApiResult<AuthResponse>()
            {
                Data = new AuthResponse(),
                Code = ResponseResultEnum.ServiceUnavailable.Value(),
                Message = ResponseResultEnum.ServiceUnavailable.Text()
            };

            string jwtID;
            var accessToken = GenerateAccessToken(accountId, employeeId, companyId, role, out jwtID);
            var refreshToken = GenerateRefreshToken();

            int lifeTime = MyConfiguration.JWT.LifeTime;
            if (BoFactory.Auth.InsertEmployeeToken(employeeId, SecurityCommon.sha256_hash(jwtID), SecurityCommon.sha256_hash(refreshToken), lifeTime, ip, "") > 0)
            {
                response.Code = ResponseResultEnum.Success.Value();
                response.Message = ResponseResultEnum.Success.Text();
                response.Data = new AuthResponse()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    UserId = accountId,
                    ShopId = companyId,
                };
            }
            else
            {
                response.Code = ResponseResultEnum.Failed.Value();
                response.Message = ResponseResultEnum.Failed.Text();
            }

            return response;
        }

        /// <summary>
        /// Lấy token từ request header
        /// </summary>
        private static string GetTokenFromRequest(HttpRequestMessage request)
        {
            var authHeader = request.Headers.Authorization;
            if (authHeader != null && authHeader.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                return authHeader.Parameter;
            }
            return string.Empty;
        }
    }
}
