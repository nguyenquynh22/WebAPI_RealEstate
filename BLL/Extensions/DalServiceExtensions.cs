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
            services.AddSingleton(new SqlHelper(connectionString!));

            // 2. Đăng ký các Repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>(); 
            services.AddScoped<IPropertiesRepository, PropertiesRepository>();
            services.AddScoped<IListingsRepository, ListingsRepository>();
<<<<<<< HEAD
            services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
            services.AddScoped<IAreaRepository, AreaRepository>();
=======
            services.AddScoped<INewsRepository, NewsRepository>();
>>>>>>> f8e813c28b6c7c2241a2917fe12f8ab75d77275f


            return services;
        }
    }
}