using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrekTrove.Api.Modules.SharedModule.Infrastructure.Bootstrapers;

namespace TrekTrove.Api.Modules.SharedModule.Infrastructure
{
    public static class ModuleBootstrap
    {
        public static IServiceCollection ConfigureSharedModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureHealthCheck(configuration);

            services.ConfigureContextDb(configuration);
            services.ConfigureClientsServices(configuration);

            services.ConfigureMediators();
            services.ConfigureRepositories();
            services.ConfigureServices();

            return services;
        }

        public static IApplicationBuilder ConfigureSharedModule(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureHealthCheck();

            //app.MigrateDatabaseOnStartup();

            return app;
        }
    }
}
