using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TanTamApi.JWT.Helper;

namespace TanTamApi.Helper
{
    public class JwtHelper
    {
        public static string  GenerateAccessToken(int accountId, int employeeAccountMapId, int companyId, int role , IConfiguration configuration,out string jwtID)
        {
            jwtID = GenerateRefreshToken();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
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
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(configuration["Jwt:ExpiryInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
