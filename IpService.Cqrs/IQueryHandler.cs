using MediatR;

namespace IpService.Cqrs;

public interface IQueryHandler<in T, U> : IRequestHandler<T, U> where T : IQuery<U>
{
}