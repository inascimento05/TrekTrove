using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using TrekTrove.Api.Bootstrap.Filters;

namespace TrekTrove.Api.Bootstrap.Bootstrapers
{
    [ExcludeFromCodeCoverage]
    public static class MvcBootstrap
    {
        public static IServiceCollection ConfigureMVC(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.AddService<DeserializeHeadersFilter>();
                options.Filters.AddService<LogActionFilter>();
            });

            services.AddScoped<LogActionFilter>();

            return services;
        }
    }
}
