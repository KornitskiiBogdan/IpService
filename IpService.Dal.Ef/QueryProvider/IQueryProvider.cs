using IpService.Domain.Query.Abstractions;
using IpService.Domain.QueryProvider;

namespace IpService.Dal.Ef.QueryProvider;

public interface IQueryProvider<TModel> : IQueryProviderBase<TModel, IQueryable<TModel>> where TModel : class
{
    IAsyncEnumerable<TModel> AsyncEnumerableBySpec(ISpecification<TModel, IQueryable<TModel>>? spec = null);
}