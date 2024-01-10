using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using TrekTrove.Api.Modules.SharedModule.Infrastructure.Bootstrapers;

namespace TrekTrove.Api.Modules.SharedModule.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class ModuleBootstrap
    {
        public static IServiceCollection ConfigureShared(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureHealthCheck(configuration);

            services.ConfigureClientsServices(configuration);

            services.ConfigureMediators();

            return services;
        }

        public static IApplicationBuilder ConfigureShared(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureHealthCheck();

            return app;
        }
    }
}
