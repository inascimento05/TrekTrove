using Microsoft.Extensions.DependencyInjection;
using TrekTrove.Api.Modules.SharedModule.Data.Repositories;
using TrekTrove.Api.Modules.SharedModule.Domain.Interfaces;

namespace TrekTrove.Api.Modules.SharedModule.Infrastructure.Bootstrapers
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
            services.AddTransient<ISharedRepository, SharedRepository>();
        }
    }
}
