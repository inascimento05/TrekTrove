using TrekTrove.Api.Modules.Shared.Application.Mediators;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.Dtos;
using TrekTrove.Api.Modules.CustomerModule.Domain.Interfaces;

namespace TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.GetById
{
    public class GetCustomerByIdHandler : BaseHandler<CustomerDto>, IBaseHandler<GetCustomerByIdRequest, DataResult<CustomerDto>>
    {
        private readonly ICustomerService _service;

        public GetCustomerByIdHandler(ICustomerService service)
        {
            _service = service;
        }

        public async Task<DataResult<CustomerDto>> Handle(GetCustomerByIdRequest request, CancellationToken cancellationToken)
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

            try
            {
                var entity = await _service.GetCustomerByIdAsync(request.Id);

                if (entity == null)
                {
                    result.AddNotification("Not found.");
                    result.Error = ErrorCode.NotFound;
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
