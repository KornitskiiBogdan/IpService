using IpService.Domain.Query.Abstractions;

namespace IpService.Domain.QueryProvider;

public interface IProjectionProviderBase<TModel, TQuery> where TModel : class
{
    Task<TProject> ProjectAsync<TProject>(IExecutor<TModel, TProject, TQuery> exec, ISpecification<TModel, TQuery>? spec = null, CancellationToken token = default);
}