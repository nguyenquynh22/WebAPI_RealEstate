using Microsoft.OpenApi.Models;

namespace AdminApi.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Admin API", Version = "v1" });

                // --- 1. Thêm định nghĩa bảo mật JWT (Đã có) ---
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // --- 2. Thêm định nghĩa bảo mật API Key (MỚI) ---
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "API Key required for accessing secure endpoints (e.g., X-API-KEY)",
                    Name = "X-API-KEY", // Tên Header là X-API-KEY
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                // --- 3. Yêu cầu Security Global (Cập nhật) ---
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    // Yêu cầu JWT (Bearer)
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    },
                    // Yêu cầu API Key (ApiKey) MỚI
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey" // ID phải khớp với AddSecurityDefinition ở trên
                            },
                        },
                        new List<string>()
                    }
                });
            });

            return services;
        }
    }
}