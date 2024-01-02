using FluentValidator;
using FluentValidator.Validation;
using MediatR;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Dtos;

namespace TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Update
{
    public class UpdateCustomerRequest : Notifiable, IRequest<DataResult<CustomerDto>>
    {
        public UpdateCustomerRequest(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;

            AddNotifications(new ValidationContract()
                .IsNotNull(Id, nameof(Id), "Id cannot be null.")
                .IsGreaterOrEqualsThan(Id, 1, nameof(Id), "Id should be greater or equals than 1."));
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
    }
}
