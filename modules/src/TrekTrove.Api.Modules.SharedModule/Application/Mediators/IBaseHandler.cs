using MediatR;

namespace TrekTrove.Api.Modules.SharedModule.Application.Mediators
{
    public interface IBaseHandler<T, K> : IRequestHandler<T, K> where T : IRequest<K> { }
}
