using AdminApi.Extensions; // Cần thiết để gọi các Extension Methods
using Common_BLL.Interfaces;
using Common_BLL.Services;
// Thêm using Microsoft.AspNetCore.Builder; // Nếu chưa có

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// --- SERVICE REGISTRATION (Thứ tự không quan trọng) ---
builder.Services.AddControllers();
builder.Services.AddApplicationServices(config);
builder.Services.AddJwtAuthentication(config);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwt();


// --- MIDDLEWARE CONFIGURATION (Thứ tự quan trọng) ---
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 1. Xác thực (Đọc token, tạo principal)
app.UseAuthentication();

// 2. Ủy quyền (Kiểm tra role, policy)
app.UseAuthorization();

app.MapControllers();

app.Run();