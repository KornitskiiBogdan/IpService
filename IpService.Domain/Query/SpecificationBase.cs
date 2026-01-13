using IpService.Domain.Query.Abstractions;

namespace IpService.Domain.Query;

public abstract class SpecificationBase<TModel, TQuery> : ISpecification<TModel, TQuery>
    where TModel : class
{
    protected readonly IFilter<TModel, TQuery>? Filter;
    protected readonly ISorter<TModel, TQuery>? Sorter;
    protected readonly IPaginate<TModel, TQuery>? Paginate;

    protected SpecificationBase(
        IFilter<TModel, TQuery>? filter = null,
        ISorter<TModel, TQuery>? sorter = null,
        IPaginate<TModel, TQuery>? paginate = null)
    {
        Filter = filter;
        Sorter = sorter;
        Paginate = paginate;
    }

    public abstract TQuery Apply(TQuery query);

    public abstract ISpecification<TModel, TQuery> WithoutPaginate();
}