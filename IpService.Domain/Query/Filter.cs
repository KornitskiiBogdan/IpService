using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using IpService.Domain.Query.Abstractions;

namespace IpService.Domain.Query;

public sealed class Filter<TModel, TQuery> : IFilter<TModel, TQuery> where TModel : class
{
    private readonly Expression<Func<TModel, bool>> _expression;
    private readonly Func<TQuery, Expression<Func<TModel, bool>>, TQuery> _filter;

    private Filter([NotNull] Expression<Func<TModel, bool>> expression,
        Func<TQuery, Expression<Func<TModel, bool>>, TQuery> filter)
    {
        _expression = expression ?? throw new ArgumentNullException(nameof(expression));
        _filter = filter ?? throw new ArgumentNullException(nameof(filter));
    }

    public Expression<Func<TModel, bool>> Expr => _expression;

    public static bool operator false(Filter<TModel, TQuery> filter) => false;

    public static bool operator true(Filter<TModel, TQuery> filter) => false;

    public static Filter<TModel, TQuery> operator &(Filter<TModel, TQuery> filter1,
        Filter<TModel, TQuery> filter2) =>
        new(filter1._expression.And(filter2._expression), filter1._filter);

    public static Filter<TModel, TQuery> operator |(Filter<TModel, TQuery> filter1,
        Filter<TModel, TQuery> filter2) =>
        new(filter1._expression.Or(filter2._expression), filter1._filter);

    public static Filter<TModel, TQuery> operator !(Filter<TModel, TQuery> filter) =>
        new(filter._expression.Not(), filter._filter);

    public static Filter<TModel, TQuery> Create(
        Expression<Func<TModel, bool>> expression,
        Func<TQuery, Expression<Func<TModel, bool>>, TQuery> filter)
    {
        return new(expression, filter);
    }

    public TQuery Apply(TQuery provider)
    {
        return _filter.Invoke(provider, _expression);
    }
}