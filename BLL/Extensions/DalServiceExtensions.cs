using Common_DAL.Interfaces;
using Common_DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common_BLL.Extensions
{
    public static class DalServiceExtensions
    {
        public static IServiceCollection AddDalServices(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");

            // 1. Đăng ký SqlHelper với chuỗi kết nối
            // Điều này giải quyết lỗi "cannot convert from string to SqlHelper"
            services.AddSingleton(new SqlHelper(connectionString!));

            // 2. Đăng ký các Repository
            // ASP.NET Core sẽ tự tìm thấy SqlHelper ở trên để truyền vào Constructor của Repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>(); // Giải quyết lỗi "Unable to resolve service"
            services.AddScoped<IPropertiesRepository, PropertiesRepository>();
            services.AddScoped<IListingsRepository, ListingsRepository>();


            return services;
        }
    }
}