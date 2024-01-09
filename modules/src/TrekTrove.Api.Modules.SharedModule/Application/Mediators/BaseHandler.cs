using FluentValidator;
using MediatR;
using Refit;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace TrekTrove.Api.Modules.SharedModule.Application.Mediators
{
    [ExcludeFromCodeCoverage]
    public class BaseHandler<T>
    {
        protected DataResult<T> ProcessException(DataResult<T> result, Exception ex)
        {
            result.AddNotification(new Notification("Response.Exception.Message", ex.Message));
            result.Error = ErrorCode.InternalServerError;
            return result;
        }

        protected DataResult<T> ProcessApiException<Y>(Y request, ApiException apiEx)
            where Y : Notifiable, IRequest<DataResult<T>>
        {
            switch (apiEx.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    request.AddNotification(new Notification("", $"Bad Request: {apiEx.Message}"));
                    return new DataResult<T>(request.Notifications) { Error = ErrorCode.BadRequest };
                case HttpStatusCode.Unauthorized:
                    request.AddNotification(new Notification($"", $"Unauthorized: {apiEx.Message}"));
                    return new DataResult<T>(request.Notifications) { Error = ErrorCode.Unauthorized };
                case HttpStatusCode.Forbidden:
                    request.AddNotification(new Notification($"", $"Forbidden: {apiEx.Message}"));
                    return new DataResult<T>(request.Notifications) { Error = ErrorCode.Forbidden };
                case HttpStatusCode.NotFound:
                    request.AddNotification(new Notification($"", $"Not found"));
                    return new DataResult<T>(request.Notifications) { Error = ErrorCode.NotFound };
                case HttpStatusCode.MethodNotAllowed:
                    request.AddNotification(new Notification($"", $"Method Not Allowed: {apiEx.Message}"));
                    return new DataResult<T>(request.Notifications) { Error = ErrorCode.MethodNotAllowed };
                case HttpStatusCode.UnprocessableEntity:
                    request.AddNotification(new Notification($"", $"Unprocessable Entity: {apiEx.Message}"));
                    return new DataResult<T>(request.Notifications) { Error = ErrorCode.UnprocessableEntity };
                case HttpStatusCode.InternalServerError:
                    request.AddNotification(new Notification($"", $"Internal Server Error: {apiEx.Message}"));
                    return new DataResult<T>(request.Notifications) { Error = ErrorCode.InternalServerError };
                case HttpStatusCode.BadGateway:
                    request.AddNotification(new Notification($"", $"Bad Gateway: {apiEx.Message}"));
                    return new DataResult<T>(request.Notifications) { Error = ErrorCode.BadGateway };
                case HttpStatusCode.ServiceUnavailable:
                    request.AddNotification(new Notification($"", $"Service Unavailable: {apiEx.Message}"));
                    return new DataResult<T>(request.Notifications) { Error = ErrorCode.ServiceUnavailable };
                default:
                    throw apiEx;
            }
        }
    }
}
