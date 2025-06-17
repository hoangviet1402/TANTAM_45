using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using TanTamApi.Helper;

namespace TanTamApi.JWT.Helper
{
    public interface IConfiguration
    {
        string this[string key] { get; }
    }

    public class AppSettingsConfiguration : IConfiguration
    {
        public string this[string key]
        {
            get => ConfigurationManager.AppSettings[key];
        }
    }

    public class JwtTokenHelper
    {
        private readonly IConfiguration _configuration;

        public JwtTokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? ""));

                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.FromMinutes(5)
                };

                var principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                var employeeId = principal.Claims.FirstOrDefault(c => c.Type == "EmployeeId")?.Value;
                var companyId = principal.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
                var accountId = principal.Claims.FirstOrDefault(c => c.Type == "AccountId")?.Value;
                var role = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (accountId != null && companyId != null && role != null && !string.IsNullOrEmpty(jti))
                {
                    // .Result is used here because we cannot use async/await in HttpModule directly for BeginRequest
                    //var storedToken = authRepository.GetTokenInfo(int.Parse(accountId), int.Parse(companyId)).Result;
                    //if (storedToken != null && storedToken.IsActive && storedToken.AccountIsActive && storedToken.CompanyIsActive && storedToken.JwtID.Equals(AESHelper.HashPassword(jti)))
                    //{
                    //    context.Items["AccountId"] = int.Parse(accountId);
                    //    context.Items["EmployeeId"] = int.Parse(employeeId ?? "0");
                    //    context.Items["CompanyId"] = int.Parse(companyId);
                    //    context.Items["JwtID"] = jti;
                    //    context.Items["Role"] = int.Parse(role);
                    //}
                    context.Items["AccountId"] = int.Parse(accountId);
                    context.Items["EmployeeId"] = int.Parse(employeeId ?? "0");
                    context.Items["CompanyId"] = int.Parse(companyId);
                    context.Items["JwtID"] = jti;
                    context.Items["Role"] = int.Parse(role);
                }
            }
            catch (Exception ex)
            {
                // Log the error using your LoggerHelper
                // Replace with your actual logging mechanism if LoggerHelper is not available or suitable
                System.Diagnostics.Trace.TraceWarning($"[JWT ERROR] : {ex.Message}");
            }
        }
    }
} 