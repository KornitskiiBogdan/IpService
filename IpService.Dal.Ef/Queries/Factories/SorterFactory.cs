using System.Diagnostics.CodeAnalysis;
using System.Linq.Dynamic.Core;
using IpService.Domain;
using IpService.Domain.Query;
using IpService.Domain.Query.Abstractions;

namespace IpService.Dal.Ef.Queries.Factories;

public sealed class SorterFactory
{
    public static ISorter<TModel, IQueryable<TModel>> Create<TModel>(
        [NotNull] Func<IQueryable<TModel>, IOrderedQueryable<TModel>> expression) where TModel : class
    {
        return Sorter<TModel, IQueryable<TModel>>.Create(expression);
    }

    public static ISorter<TModel, IQueryable<TModel>> Create<TModel>(
        SortingDirection? sortingDirection,
        string fieldName,
        Func<IOrderedQueryable<TModel>, IOrderedQueryable<TModel>>? thenByExpression = null) where TModel : class =>
        Create(sortingDirection, fieldName, null, thenByExpression);


    public static ISorter<TModel, IQueryable<TModel>> CreateWithNullCast<TModel>(
        SortingDirection? sortingDirection,
        string fieldName,
        Func<IOrderedQueryable<TModel>, IOrderedQueryable<TModel>>? thenByExpression = null) where TModel : class
    {
        var config = new ParsingConfig
        {
            RestrictOrderByToPropertyOrField = false
        };

        var dynamicNullCast = sortingDirection == SortingDirection.Desc
            ? $"{fieldName} == NULL"
            : $"{fieldName} != NULL";

        return Create(sortingDirection, fieldName, q => q.OrderBy(config, dynamicNullCast), thenByExpression);
    }

    private static ISorter<TModel, IQueryable<TModel>> Create<TModel>(
        SortingDirection? sortingDirection,
        string fieldName,
        Func<IQueryable<TModel>, IOrderedQueryable<TModel>>? orderByExpression = null,
        Func<IOrderedQueryable<TModel>, IOrderedQueryable<TModel>>? thenByExpression = null) where TModel : class
    {
        if (string.IsNullOrWhiteSpace(fieldName) || sortingDirection == null)
            return null;

        var dynamicSorting = sortingDirection == SortingDirection.Desc
            ? $"{fieldName} {SortingDirection.Desc.ToString().ToLowerInvariant()}"
            : fieldName;

        IOrderedQueryable<TModel> OrderBy(IQueryable<TModel> q) => orderByExpression == null
            ? q.OrderBy(dynamicSorting)
            : orderByExpression.Invoke(q).ThenBy(dynamicSorting);

        IOrderedQueryable<TModel> ThenBy(IQueryable<TModel> q) => thenByExpression == null
            ? OrderBy(q)
            : thenByExpression.Invoke(OrderBy(q));

        return Sorter<TModel, IQueryable<TModel>>.Create(ThenBy);
    }
}