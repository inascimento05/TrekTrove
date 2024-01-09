using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TrekTrove.Api.Modules.SharedModule.Infrastructure.Bootstrapers
{
    public static class ClientServiceBootstrap
    {
        public static IServiceCollection ConfigureClientsServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            //services.AddRefitClient<ISampleExternalApiService>().ConfigureHttpClient(c =>
            //{
            //    c.BaseAddress = new Uri(configuration.GetSection("Shared:ApiServices:Sample:Url").Value);
            //    c.Timeout = TimeSpan.FromSeconds(15);
            //});

            return services;
        }
    }
}
