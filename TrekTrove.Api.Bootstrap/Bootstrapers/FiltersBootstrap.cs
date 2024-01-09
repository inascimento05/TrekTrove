using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using TrekTrove.Api.Bootstrap.Filters;

namespace TrekTrove.Api.Bootstrap.Bootstrapers
{
    [ExcludeFromCodeCoverage]
    public static class FiltersBootstrap
    {
        public static IServiceCollection ConfigureFilters(this IServiceCollection services)
        {
            services.AddScoped<DeserializeHeadersFilter>();

            return services;
        }
    }
}
