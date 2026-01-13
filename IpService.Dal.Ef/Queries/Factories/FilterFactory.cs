using System.Linq.Expressions;
using IpService.Domain.Query;

namespace IpService.Dal.Ef.Queries.Factories;

public static class FilterFactory
{
    public static Filter<TModel, IQueryable<TModel>> Create<TModel>(
        Expression<Func<TModel, bool>> expression) where TModel : class
    {
        return Filter<TModel, IQueryable<TModel>>.Create(expression,
            (prov, q) => prov.Where(q));
    }
}