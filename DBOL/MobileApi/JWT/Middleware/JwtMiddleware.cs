using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using Microsoft.IdentityModel.Tokens;

namespace TanTamApi.JWT.Middleware
{
    public class JwtMiddleware
    {
        private readonly IConfiguration _configuration;

        public JwtMiddleware(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ProcessRequest(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"]?.Split(' ').Last();
        }

        private void AttachUserToContext(HttpContext context, string token)
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
                var jti =  jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                var employeeId = principal.Claims.FirstOrDefault(c => c.Type == "EmployeeId")?.Value;
                var companyId = principal.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
                var accountId = principal.Claims.FirstOrDefault(c => c.Type == "AccountId")?.Value;
                var role = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (accountId != null && companyId != null && role != null && !string.IsNullOrEmpty(jti))
                {
                    var storedToken = authRepository.GetTokenInfo(int.Parse(accountId), int.Parse(companyId)).Result;
                    if (storedToken != null && storedToken.IsActive && storedToken.AccountIsActive && storedToken.CompanyIsActive && storedToken.JwtID.Equals(AESHelper.HashPassword(jti)))
                    {
                        context.Items["AccountId"] = int.Parse(accountId);
                        context.Items["EmployeeId"] = int.Parse(employeeId ?? "0");
                        context.Items["CompanyId"] = int.Parse(companyId);
                        context.Items["JwtID"] = jti;
                        context.Items["Role"] = int.Parse(role);
                    }
                }
            }
            catch (Exception ex)
            {               
            }
        }
    }
}