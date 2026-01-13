using MediatR;

namespace IpService.Cqrs;

public interface ICommandHandler<in T> : IRequestHandler<T> where T : ICommand
{
}

public interface ICommandHandler<in T, U> : IRequestHandler<T, U> where T : ICommand<U>
{
}