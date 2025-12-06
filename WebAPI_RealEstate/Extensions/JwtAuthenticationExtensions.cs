// AdminApi/Extensions/JwtAuthExtensions.cs

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AdminApi.Extensions
{
    public static class JwtAuthExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var jwtSettings = config.GetSection("JwtSettings");
            var secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret is not configured.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // 1. Kiểm tra khóa ký
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),

                    // 2. Kiểm tra Issuer (Nơi phát hành)
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],

                    // 3. Kiểm tra Audience (Nơi sử dụng)
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],

                    // 4. Kiểm tra thời gian hết hạn (Cho phép sai số thời gian)
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Thêm Authorization Policy (tùy chọn nhưng nên có)
            services.AddAuthorization();

            return services;
        }
    }
}