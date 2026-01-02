using Common_BLL.Interfaces;
using Common_BLL.Services;
using Common_BLL.Profiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common_BLL.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Gọi đăng ký tầng DAL (đã bao gồm SqlHelper và Repositories)
            services.AddDalServices(config);

            // Đăng ký các nghiệp vụ Service
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IPropertiesService, PropertiesService>();
            services.AddScoped<IListingsService, ListingsService>();


            // Đăng ký AutoMapper phiên bản 12.0.1
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(UserMappingProfile).Assembly);
            });

            return services;
        }
    }
}