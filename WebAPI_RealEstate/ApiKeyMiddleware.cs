namespace AdminApi
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
            var excludedPaths = new[]
            {
                "/api/auth/login", // Đã chuyển sang chữ thường
                "/api/auth/register" // Đã chuyển sang chữ thường
            };

            // Lấy đường dẫn hiện tại và chuyển sang chữ thường để so sánh
            var currentPath = context.Request.Path.Value?.ToLowerInvariant();

            // 2. Kiểm tra nếu đường dẫn nằm trong danh sách loại trừ
            // currentPath (ví dụ: "/api/auth/register") so sánh với excludedPaths (ví dụ: "/api/auth/register")
            if (currentPath != null && excludedPaths.Contains(currentPath))
            {
                await _next(context); // Bỏ qua kiểm tra API Key và chuyển sang bước tiếp theo
                return;
            }
            // --- LOGIC KIỂM TRA API KEY CHỈ CHẠY CHO CÁC PATH KHÁC ---

            if (!context.Request.Headers.TryGetValue(ApiKeyName, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Bạn không có quyền truy cập! (Thiếu API Key)");
                return;
            }

            var apiKey = configuration["AppSettings:MyApiKey"];
            if (!apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 403;
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
