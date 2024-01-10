using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using TrekTrove.Api.Modules.SharedModule.Domain.Interfaces;
using TrekTrove.Api.Modules.SharedModule.Domain.Services;

namespace TrekTrove.Api.Modules.SharedModule.Infrastructure.Bootstrapers
{
    [ExcludeFromCodeCoverage]
    public static class ServiceBootstrap
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            ConfigureModuleServices(services);

            return services;
        }

        private static void ConfigureModuleServices(IServiceCollection services)
        {
        }
    }
}
