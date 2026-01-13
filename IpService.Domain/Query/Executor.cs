using System.Diagnostics.CodeAnalysis;
using IpService.Domain.Query.Abstractions;

namespace IpService.Domain.Query;

public sealed class Executor<TModel, TOut, TQuery> : IExecutor<TModel, TOut, TQuery> where TModel : class
{
    private readonly Func<TQuery, CancellationToken, Task<TOut>> _expression;

    private Executor([NotNull] Func<TQuery, CancellationToken, Task<TOut>> expression)
    {
        _expression = expression ?? throw new ArgumentNullException(nameof(expression));
    }

    public static Executor<TModel, TOut, TQuery> Create([NotNull] Func<TQuery, CancellationToken, Task<TOut>> expression)
    {
        return new(expression);
    }

    public Task<TOut> ExecuteAsync(TQuery query, CancellationToken token = default)
    {
        return _expression.Invoke(query, token);
    }
}