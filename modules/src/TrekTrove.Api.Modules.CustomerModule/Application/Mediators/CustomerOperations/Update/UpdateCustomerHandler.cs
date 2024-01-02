using TrekTrove.Api.Modules.Shared.Application.Mediators;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Dtos;
using TrekTrove.Api.Modules.CustomerModule.Domain.Entities;
using TrekTrove.Api.Modules.CustomerModule.Domain.Interfaces;

namespace TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Update
{
    public class UpdateCustomerHandler : BaseHandler<CustomerDto>, IBaseHandler<UpdateCustomerRequest, DataResult<CustomerDto>>
    {
        private readonly ICustomerService _service;

        public UpdateCustomerHandler(ICustomerService service)
        {
            _service = service;
        }

        public async Task<DataResult<CustomerDto>> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
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

            var entityToUpdate = new Customer
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description
            };

            try
            {
                var updatedCustomer = await _service.UpdateCustomerAsync(entityToUpdate);

                if(updatedCustomer == null)
                {
                    result.AddNotification("Not found.");
                    result.Error = ErrorCode.NotFound;
                    return result;
                }

                result.Data = (CustomerDto)updatedCustomer;
            }
            catch (Exception ex)
            {
                return ProcessException(result, ex);
            }

            return result;
        }
    }
}
