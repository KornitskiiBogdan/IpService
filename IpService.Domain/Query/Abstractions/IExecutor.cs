namespace IpService.Domain.Query.Abstractions;

public interface IExecutor<TModel, TOut, in TQuery> where TModel : class
{
    Task<TOut> ExecuteAsync(TQuery provider, CancellationToken token = default);
}