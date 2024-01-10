using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using TrekTrove.Api.Modules.SharedModule.Application.Mediators;

namespace TrekTrove.Api.Modules.SharedModule.Infrastructure.Bootstrapers
{
    [ExcludeFromCodeCoverage]
    public static class MediatorBootstrap
    {
        public static IServiceCollection ConfigureMediators(this IServiceCollection services)
        {
            services.AddMediatR(typeof(IBaseHandler<,>).Assembly);

            return services;
        }
    }
}
