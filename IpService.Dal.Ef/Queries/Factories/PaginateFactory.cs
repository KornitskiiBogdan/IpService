using IpService.Domain.Query;
using IpService.Domain.Query.Abstractions;

namespace IpService.Dal.Ef.Queries.Factories;

public sealed class PaginateFactory
{
    public static IPaginate<TModel, IQueryable<TModel>>? Create<TModel>(int? pageNumber, int? pageSize) where TModel : class
    {
        return Paginate<TModel, IQueryable<TModel>>.Create(
            pageNumber,
            pageSize,
            (provider, pn, ps) => provider.Skip((pn - 1) * ps).Take(ps));
    }
        
    public static IPaginate<TModel, IQueryable<TModel>>? CreateSkip<TModel>(int? skip) where TModel : class
    {
        if (!skip.HasValue || skip.GetValueOrDefault() == 0)
        {
            return null;
        }

        return Skip<TModel, IQueryable<TModel>>.Create(skip.GetValueOrDefault(), (provider, s) => provider.Skip(s));
    }
}