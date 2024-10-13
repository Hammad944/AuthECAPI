using AuthECapi.Models;

namespace AuthECapi.Extenstions
{
    public static class AppConfigExtension
    {
        public static WebApplication ConfigureCORS(this WebApplication app, IConfiguration configuration)
        {
            app.UseCors(options =>
 options.WithOrigins("http://localhost:4200")
 .AllowAnyHeader()
 .AllowAnyMethod());
            return app;
        }
        public static IServiceCollection AddAppConfig(this IServiceCollection app, IConfiguration configuration)
        {
            app.Configure<AppSetting>(configuration.GetSection("AppSettings"));
            return app;
        }
    }
}
