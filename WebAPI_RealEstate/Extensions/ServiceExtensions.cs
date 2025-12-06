// Modules/AdminApi/Extensions/ServiceExtensions.cs (Cập nhật sau khi xóa gói mở rộng)

using AutoMapper; // Cần using này cho IMapperConfigurationExpression
using Common_BLL.Extensions;
using Common_BLL.Interfaces;
using Common_BLL.Profiles;
using Common_BLL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; // Cần có using này cho IServiceCollection
using System;
using Common_DAL.Repositories;
using Common_DAL.Interfaces;

namespace AdminApi.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDalServices(config);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(UserMappingProfile).Assembly);
            });

            return services;
        }
    }
}