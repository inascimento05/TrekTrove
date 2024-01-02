using FluentValidator;
using FluentValidator.Validation;
using MediatR;
using TrekTrove.Api.Modules.Shared.Application.Notifications;

namespace TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.RemoveById
{
    public class RemoveCustomerByIdRequest : Notifiable, IRequest<DataResult<bool>>
    {
        public int Id { get; private set; }

        public RemoveCustomerByIdRequest(int id)
        {
            Id = id;

            AddNotifications(new ValidationContract()
                .IsNotNull(Id, nameof(Id), "Id cannot be null.")
                .IsGreaterOrEqualsThan(Id, 1, nameof(Id), "Id should be greater or equals than 1."));
        }
    }
}
