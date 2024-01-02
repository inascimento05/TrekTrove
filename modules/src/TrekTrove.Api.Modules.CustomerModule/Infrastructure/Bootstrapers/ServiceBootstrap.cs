using Microsoft.Extensions.DependencyInjection;
using TrekTrove.Api.Modules.CustomerModule.Domain.Interfaces;
using TrekTrove.Api.Modules.CustomerModule.Domain.Services;

namespace TrekTrove.Api.Modules.CustomerModule.Infrastructure.Bootstrapers
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
            services.AddTransient<ICustomerService, CustomerService>();
        }
    }
}
