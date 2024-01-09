using MediatR;

namespace TrekTrove.Api.Modules.SharedModule.Application.Interfaces
{
    public interface IMediator
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request);
    }
}
