using Microsoft.Extensions.DependencyInjection;
using TrekTrove.Api.Modules.CustomerModule.Data.Repositories;
using TrekTrove.Api.Modules.CustomerModule.Domain.Interfaces;

namespace TrekTrove.Api.Modules.CustomerModule.Infrastructure.Bootstrapers
{
    public static class RepositoryBootstrap
    {
        public static IServiceCollection ConfigureRepositories(
            this IServiceCollection services)
        {
            ConfigureModuleRepositories(services);

            return services;
        }

        private static void ConfigureModuleRepositories(IServiceCollection services)
        {
            services.AddTransient<ICustomerRepository, CustomerRepository>();
        }
    }
}
