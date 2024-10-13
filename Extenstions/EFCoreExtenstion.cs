using AuthECapi.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthECapi.Extenstions
{
    public static class EFCoreExtenstion
    {
        public static IServiceCollection InjectDbContext(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DevConnection")));
            return services;
        }
    }
}
