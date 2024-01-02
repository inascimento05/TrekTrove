using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Create;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Dtos;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.GetAll;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.GetById;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.RemoveById;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Update;

namespace TrekTrove.Api.Modules.CustomerModule.Infrastructure.Bootstrapers
{
    public static class MediatorBootstrap
    {
        public static IServiceCollection ConfigureMediators(this IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<CreateCustomerRequest, DataResult<CustomerDto>>, CreateCustomerHandler>();
            services.AddTransient<IRequestHandler<GetCustomerByIdRequest, DataResult<CustomerDto>>, GetCustomerByIdHandler>();
            services.AddTransient<IRequestHandler<GetAllCustomersRequest, DataResult<IEnumerable<CustomerDto>>>, GetAllCustomersHandler>();
            services.AddTransient<IRequestHandler<UpdateCustomerRequest, DataResult<CustomerDto>>, UpdateCustomerHandler>();
            services.AddTransient<IRequestHandler<RemoveCustomerByIdRequest, DataResult<bool>>, RemoveCustomerByIdHandler>();

            return services;
        }
    }
}
