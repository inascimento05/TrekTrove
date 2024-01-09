using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace TrekTrove.Api.Bootstrap.Bootstrapers
{
    [ExcludeFromCodeCoverage]
    public static class ModulesBootstrap
    {
        public static IServiceCollection ConfigureModules(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            return services;
        }

        public static IApplicationBuilder ConfigureModules(
            this IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            return app;
        }
    }
}
