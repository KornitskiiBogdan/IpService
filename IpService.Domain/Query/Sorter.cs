using System.Diagnostics.CodeAnalysis;
using IpService.Domain.Query.Abstractions;

namespace IpService.Domain.Query;

public sealed class Sorter<TModel, TQuery> : ISorter<TModel, TQuery> where TModel : class
{
    private readonly Func<TQuery, TQuery> _expression;

    private Sorter([NotNull] Func<TQuery, TQuery> expression)
    {
        _expression = expression ?? throw new ArgumentNullException(nameof(expression));
    }

    public static Sorter<TModel, TQuery> Create(Func<TQuery, TQuery> expression)
    {
        return new(expression);
    }

    public TQuery Apply(TQuery provider)
    {
        return _expression.Invoke(provider);
    }
}