using Common_DAL.Interfaces;
using Common_DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_BLL.Extensions
{
    public static class DalServiceExtensions
    {
        public static IServiceCollection AddDalServices(this IServiceCollection services, IConfiguration config)
        {
            // 1. Lấy chuỗi kết nối
            var connectionString = config.GetConnectionString("DefaultConnection");

            // 2. Đăng ký SqlHelper và Repository (Tất cả thuộc DAL)
            services.AddSingleton(new SqlHelper(connectionString!));
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
