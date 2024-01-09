using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace TrekTrove.Api.Bootstrap.Bootstrapers
{
    [ExcludeFromCodeCoverage]
    public static class ClientServiceBootstrap
    {
        public static IServiceCollection ConfigureClientsServices(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            ConfigureModuleClientsServices(services, configuration);

            return services;
        }

        private static void ConfigureModuleClientsServices(IServiceCollection services, IConfiguration configuration)
        {
            // Example of Refit implementation.
            //services.AddRefitClient<IBlingApiService>().ConfigureHttpClient(c =>
            //{
            //    c.BaseAddress = new Uri(configuration.GetSection("ApiServices:Bling:Url").Value);
            //    c.Timeout = TimeSpan.FromSeconds(15);
            //});
        }
    }
}
