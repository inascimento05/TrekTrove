using Microsoft.Extensions.DependencyInjection;
using TrekTrove.Api.Modules.SharedModule.Domain.Interfaces;
using TrekTrove.Api.Modules.SharedModule.Domain.Services;

namespace TrekTrove.Api.Modules.SharedModule.Infrastructure.Bootstrapers
{
    public static class ServiceBootstrap
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            ConfigureModuleServices(services);

            return services;
        }

        private static void ConfigureModuleServices(IServiceCollection services)
        {
            services.AddTransient<ISharedService, SharedService>();
        }
    }
}
