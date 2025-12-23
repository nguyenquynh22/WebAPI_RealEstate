using Common_Shared.Extensions;
using Common_BLL.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// --- ĐĂNG KÝ DỊCH VỤ ---
builder.Services.AddControllers();

// Chỉ cần gọi dòng này là đủ cho cả SqlHelper, Repositories và Services
builder.Services.AddApplicationServices(config);

// Cấu hình bảo mật và tài liệu
builder.Services.AddJwtAuthentication(config);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "User API", Version = "v1" });
    c.CustomSchemaIds(type => type.FullName);
});
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// --- MIDDLEWARE ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API v1");
    });
}

app.UseHttpsRedirection();

// Thứ tự Middleware chuẩn của .NET
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();