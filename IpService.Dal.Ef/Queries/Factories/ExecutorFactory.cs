using IpService.Domain.Query;
using IpService.Domain.Query.Abstractions;

namespace IpService.Dal.Ef.Queries.Factories;

public sealed class ExecutorFactory
{
    public static IExecutor<TModel, TOut, IQueryable<TModel>> Create<TModel, TOut>(
        Func<IQueryable<TModel>, CancellationToken, Task<TOut>> expression) where TModel : class
    {
        return Executor<TModel, TOut, IQueryable<TModel>>.Create(expression);
    }
}