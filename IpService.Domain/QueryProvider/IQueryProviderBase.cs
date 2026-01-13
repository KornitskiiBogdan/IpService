using IpService.Domain.Query.Abstractions;

namespace IpService.Domain.QueryProvider;

public interface IQueryProviderBase<TModel, TQuery> : IProjectionProviderBase<TModel, TQuery> where TModel : class
{
    Task<TModel?> FirstOrDefaultBySpecAsync(ISpecification<TModel, TQuery>? spec = null, CancellationToken token = default);
    Task<TModel?> SingleOrDefaultBySpecAsync(ISpecification<TModel, TQuery>? spec = null, CancellationToken token = default);
    Task<IEnumerable<TModel>> EnumerableBySpecAsync(ISpecification<TModel, TQuery>? spec = null, CancellationToken token = default);
    Task<long> CountBySpecAsync(ISpecification<TModel, TQuery>? spec = null, CancellationToken token = default);
    Task<bool> AnyBySpecAsync(ISpecification<TModel, TQuery>? spec = null, CancellationToken token = default);
}