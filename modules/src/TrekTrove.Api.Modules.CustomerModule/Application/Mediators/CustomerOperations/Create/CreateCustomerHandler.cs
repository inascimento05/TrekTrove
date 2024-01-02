using TrekTrove.Api.Modules.Shared.Application.Mediators;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Dtos;
using TrekTrove.Api.Modules.CustomerModule.Domain.Entities;
using TrekTrove.Api.Modules.CustomerModule.Domain.Interfaces;

namespace TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Create
{
    public class CreateCustomerHandler : BaseHandler<CustomerDto>, IBaseHandler<CreateCustomerRequest, DataResult<CustomerDto>>
    {
        private readonly ICustomerService _service;

        public CreateCustomerHandler(ICustomerService service)
        {
            _service = service;
        }

        public async Task<DataResult<CustomerDto>> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            var result = new DataResult<CustomerDto>();
            if (request == null)
            {
                result.AddNotification("Request", "Request cannot be null.");
                result.Error = ErrorCode.BadRequest;
                return result;
            }

            result.AddNotifications(request.Notifications);
            if (result.Invalid)
            {
                result.Error = ErrorCode.BadRequest;
                return result;
            }

            var entity = new Customer
            {
                Name = request.Name,
                Description = request.Description
            };

            try
            {
                entity.Id = await _service.CreateCustomerAsync(entity);

                if (entity.Id <= 0)
                {
                    result.AddNotification("Failure to create new.");
                    result.Error = ErrorCode.UnprocessableEntity;
                    return result;
                }

                result.Data = (CustomerDto)entity;
            }
            catch (Exception ex)
            {
                return ProcessException(result, ex);
            }

            return result;
        }
    }
}
