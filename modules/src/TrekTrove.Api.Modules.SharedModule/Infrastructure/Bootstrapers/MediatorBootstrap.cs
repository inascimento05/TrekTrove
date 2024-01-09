using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.SharedModule.Application.Mediators.SharedOperations.Create;
using TrekTrove.Api.Modules.SharedModule.Application.Mediators.SharedOperations.Dtos;
using TrekTrove.Api.Modules.SharedModule.Application.Mediators.SharedOperations.GetAll;
using TrekTrove.Api.Modules.SharedModule.Application.Mediators.SharedOperations.GetById;
using TrekTrove.Api.Modules.SharedModule.Application.Mediators.SharedOperations.RemoveById;
using TrekTrove.Api.Modules.SharedModule.Application.Mediators.SharedOperations.Update;

namespace TrekTrove.Api.Modules.SharedModule.Infrastructure.Bootstrapers
{
    public static class MediatorBootstrap
    {
        public static IServiceCollection ConfigureMediators(this IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<CreateSharedRequest, DataResult<SharedDto>>, CreateSharedHandler>();
            services.AddTransient<IRequestHandler<GetSharedByIdRequest, DataResult<SharedDto>>, GetSharedByIdHandler>();
            services.AddTransient<IRequestHandler<GetAllSharedsRequest, DataResult<IEnumerable<SharedDto>>>, GetAllSharedsHandler>();
            services.AddTransient<IRequestHandler<UpdateSharedRequest, DataResult<SharedDto>>, UpdateSharedHandler>();
            services.AddTransient<IRequestHandler<RemoveSharedByIdRequest, DataResult<bool>>, RemoveSharedByIdHandler>();

            return services;
        }
    }
}
