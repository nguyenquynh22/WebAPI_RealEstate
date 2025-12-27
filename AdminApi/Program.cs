using Common_BLL.Extensions;
using Common_Shared.Extensions;
using Microsoft.OpenApi.Models;

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

    // Thêm cấu hình này để Swagger của User có nút "Authorize"
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhập Token theo cú pháp: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            }, new string[] { }
        }
    });
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