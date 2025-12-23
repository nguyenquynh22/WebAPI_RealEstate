using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Common_Shared.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ApiKeyName = "X-API-KEY";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            var currentPath = context.Request.Path.Value?.ToLowerInvariant() ?? "";

            var excludedPaths = new[] {
                "/api/auth/login",
                "/api/auth/register",
                "/swagger",          
                "/index.html",       
                "/favicon.ico",
                "/v1/swagger.json"   
            };

            // Kiểm tra nếu đường dẫn hiện tại nằm trong danh sách loại trừ
            if (excludedPaths.Any(path => currentPath.Contains(path)))
            {
                await _next(context);
                return;
            }

            // Kiểm tra sự tồn tại của API Key trong Header
            if (!context.Request.Headers.TryGetValue(ApiKeyName, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync("Bạn không có quyền truy cập! (Thiếu API Key)");
                return;
            }

            // So sánh với Key trong appsettings.json
            var apiKey = configuration["AppSettings:MyApiKey"];
            if (string.IsNullOrEmpty(apiKey) || !apiKey.Equals(extractedApiKey.ToString()))
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync("Key của bạn không đúng!");
                return;
            }

            await _next(context);
        }
    }

    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}