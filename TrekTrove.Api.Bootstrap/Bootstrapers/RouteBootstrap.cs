using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace TrekTrove.Api.Bootstrap.Bootstrapers
{
    [ExcludeFromCodeCoverage]
    public static class RouteBootstrap
    {
        public static IServiceCollection ConfigureRoutes(
           this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            return services;
        }
    }
}
