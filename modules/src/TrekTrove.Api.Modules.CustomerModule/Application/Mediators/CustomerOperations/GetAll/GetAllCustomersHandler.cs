using TrekTrove.Api.Modules.Shared.Application.Mediators;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Dtos;
using TrekTrove.Api.Modules.CustomerModule.Domain.Interfaces;

namespace TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.GetAll
{
    public class GetAllCustomersHandler : BaseHandler<IEnumerable<CustomerDto>>, IBaseHandler<GetAllCustomersRequest, DataResult<IEnumerable<CustomerDto>>>
    {
        
        private readonly ICustomerService _service;

        public GetAllCustomersHandler(ICustomerService service)
        {
            _service = service;
        }

        public async Task<DataResult<IEnumerable<CustomerDto>>> Handle(GetAllCustomersRequest request, CancellationToken cancellationToken)
        {
            var result = new DataResult<IEnumerable<CustomerDto>>();
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
            try
            {
                var entities = await _service.GetAllCustomersAsync(request.PageNumber, request.PageSize);

                if (entities == null)
                {
                    result.AddNotification("Not found.");
                    result.Error = ErrorCode.NotFound;
                    return result;
                }

                result.Data = entities.Select(entity => (CustomerDto)entity);
            }
            catch (Exception ex)
            {
                return ProcessException(result, ex);
            }

            return result;
        }
    }
}
