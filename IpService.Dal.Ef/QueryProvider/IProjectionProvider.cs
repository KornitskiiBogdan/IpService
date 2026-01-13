using IpService.Domain.Query.Abstractions;
using IpService.Domain.QueryProvider;

namespace IpService.Dal.Ef.QueryProvider;

public interface IProjectionProvider<TModel> : IProjectionProviderBase<TModel, IQueryable<TModel>> where TModel : class
{
    IAsyncEnumerable<TProject> ProjectAsyncEnumerableBySpec<TProject>(
        ISpecification<TModel, IQueryable<TModel>>? spec = null,
        CancellationToken token = default);
}