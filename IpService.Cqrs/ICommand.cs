using MediatR;

namespace IpService.Cqrs;

public interface ICommand<out T> : IRequest<T>
{
}

public interface ICommand : IRequest
{
}