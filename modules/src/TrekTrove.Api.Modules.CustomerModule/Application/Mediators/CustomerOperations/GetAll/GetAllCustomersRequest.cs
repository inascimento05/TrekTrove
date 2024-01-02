using FluentValidator;
using FluentValidator.Validation;
using MediatR;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Dtos;

namespace TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.GetAll
{
    public class GetAllCustomersRequest : Notifiable, IRequest<DataResult<IEnumerable<CustomerDto>>>
    {
        public GetAllCustomersRequest(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            PageNumber = pageNumber;
            PageSize = pageSize;

            AddNotifications(new ValidationContract()
                .IsNotNull(PageNumber, nameof(PageNumber), "Id cannot be null.")
                .IsGreaterOrEqualsThan(PageNumber, 1, nameof(PageNumber), "Id should be greater or equals than 1.")
                .IsNotNull(pageSize, nameof(pageSize), "Id cannot be null.")
                .IsGreaterOrEqualsThan(pageSize, 1, nameof(pageSize), "Id should be greater or equals than 1."));
        }

        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
    }
}
