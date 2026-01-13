namespace IpService.Domain.Query.Abstractions;

public interface ISpecification<TModel, TQuery> where TModel : class
{
    TQuery Apply(TQuery query);

    ISpecification<TModel, TQuery> WithoutPaginate();
}