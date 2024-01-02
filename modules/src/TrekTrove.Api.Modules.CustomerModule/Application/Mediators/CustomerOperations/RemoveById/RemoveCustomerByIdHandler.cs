using TrekTrove.Api.Modules.Shared.Application.Mediators;
using TrekTrove.Api.Modules.Shared.Application.Notifications;
using TrekTrove.Api.Modules.CustomerModule.Domain.Interfaces;

namespace TrekTrove.Api.Modules.CustomerModule.Application.Mediators.CustomerOperations.RemoveById
{
    public class RemoveCustomerByIdHandler : BaseHandler<bool>, IBaseHandler<RemoveCustomerByIdRequest, DataResult<bool>>
    {
        private readonly ICustomerService _service;

        public RemoveCustomerByIdHandler(ICustomerService service)
        {
            _service = service;
        }

        public async Task<DataResult<bool>> Handle(RemoveCustomerByIdRequest request, CancellationToken cancellationToken)
        {
            var result = new DataResult<bool>();
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
                var removed = await _service.RemoveCustomerByIdAsync(request.Id);

                if (removed == null)
                {
                    result.AddNotification("Not found.");
                    result.Error = ErrorCode.NotFound;
                    return result;
                }

                result.Data = (bool)removed;
            }
            catch (Exception ex)
            {
                return ProcessException(result, ex);
            }

            return result;
        }
    }
}
