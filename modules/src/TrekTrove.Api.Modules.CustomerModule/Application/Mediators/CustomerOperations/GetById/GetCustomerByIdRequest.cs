using FluentValidator;
using FluentValidator.Validation;
using MediatR;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Dtos;

namespace TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.GetById
{
    public class GetCustomerByIdRequest : Notifiable, IRequest<DataResult<CustomerDto>>
    {
        public int Id { get; private set; }

        public GetCustomerByIdRequest(int id)
        {
            Id = id;

            AddNotifications(new ValidationContract()
                .IsNotNull(Id, nameof(Id), "Id cannot be null.")
                .IsGreaterOrEqualsThan(Id, 1, nameof(Id), "Id should be greater or equals than 1."));
        }

    }
}
