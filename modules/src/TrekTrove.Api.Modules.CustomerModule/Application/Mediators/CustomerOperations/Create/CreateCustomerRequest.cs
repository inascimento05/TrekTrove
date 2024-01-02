using FluentValidator;
using FluentValidator.Validation;
using MediatR;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Dtos;

namespace TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Create
{
    public class CreateCustomerRequest : Notifiable, IRequest<DataResult<CustomerDto>>
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        public CreateCustomerRequest(string name, string description)
        {
            Name = name;
            Description = description;

            AddNotifications(new ValidationContract()
                .IsNotNullOrEmpty(Name, "Name", "Name is required.")
                .IsLowerOrEqualsThan(Name?.Length ?? 0, 100, "Name", "Name cannot be longer than 100 characters.")
                .IsNotNullOrEmpty(Description, "Description", "Description is required.")
                .IsLowerOrEqualsThan(Description?.Length ?? 0, 200, "Description", "Description cannot be longer than 200 characters."));
        }
    }
}
