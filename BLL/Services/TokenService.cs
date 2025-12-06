// File: Common_BLL/Services/TokenService.cs

using Common_BLL.Interfaces;
using Common_DTOs.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Common_BLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _secret;
        private readonly int _expiryMinutes;

        public TokenService(IConfiguration configuration)
        {
            _secret = configuration["JwtSettings:Secret"]
                ?? throw new InvalidOperationException("JwtSettings:Secret not configured.");
            if (!int.TryParse(configuration["JwtSettings:ExpiryMinutes"], out _expiryMinutes))
            {
                _expiryMinutes = 60;
            }
        }

        public string CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);

            // Xây dựng các Claim cho Token (Thông tin người dùng)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role) // Thêm Role của User
                // Bạn có thể thêm các claim khác ở đây, ví dụ: user.Email
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_expiryMinutes),
                SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}