using System.Linq.Expressions;
using IpService.Dal.Ef.Queries.Factories;
using IpService.Domain.Query;
using IpService.Domain.Query.Abstractions;

namespace IpService.Dal.Ef.Queries;

public sealed class Specification<TModel> : SpecificationBase<TModel, IQueryable<TModel>>
    where TModel : class
{
    private readonly Specification<TModel>? _inner;
    private readonly Specification<TModel>? _union;

    private Specification(
        IFilter<TModel, IQueryable<TModel>>? filter = null,
        ISorter<TModel, IQueryable<TModel>>? sorter = null,
        IPaginate<TModel, IQueryable<TModel>>? paginate = null,
        Specification<TModel>? inner = null,
        Specification<TModel>? union = null) : base(filter, sorter, paginate)
    {
        _inner = inner;
        _union = union;
    }

    public static Specification<TModel> Create(
        IFilter<TModel, IQueryable<TModel>>? filter = null,
        ISorter<TModel, IQueryable<TModel>>? sorter = null,
        IPaginate<TModel, IQueryable<TModel>>? paginate = null,
        Specification<TModel>? innerQuery = null,
        Specification<TModel>? unionQuery = null) =>
        new(filter, sorter, paginate, innerQuery, unionQuery);

    public static Specification<TModel> CreateExpr(
        Expression<Func<TModel, bool>>? filter = null,
        ISorter<TModel, IQueryable<TModel>>? sorter = null,
        IPaginate<TModel, IQueryable<TModel>>? paginate = null,
        Specification<TModel>? innerQuery = null,
        Specification<TModel>? unionQuery = null) =>
        new(FilterFactory.Create(filter), sorter, paginate, innerQuery, unionQuery);

    public override IQueryable<TModel> Apply(IQueryable<TModel> query)
    {
        var originalQuery = query;

        if (_inner != null)
        {
            query = _inner.Apply(query);
        }

        if (_union != null)
        {
            query = query.Union(_union.Apply(originalQuery));
        }

        if (Filter != null)
        {
            query = Filter.Apply(query);
        }

        if (Sorter != null)
        {
            query = Sorter.Apply(query);
        }

        if (Paginate != null)
        {
            query = Paginate.Apply(query);
        }

        return query;
    }

    public override ISpecification<TModel, IQueryable<TModel>> WithoutPaginate() =>
        new Specification<TModel>(Filter, Sorter, null, _inner, _union);
}