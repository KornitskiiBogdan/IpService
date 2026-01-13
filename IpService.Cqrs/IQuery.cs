using MediatR;

namespace IpService.Cqrs;

public interface IQuery<out T> : IRequest<T>
{
}