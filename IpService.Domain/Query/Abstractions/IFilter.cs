namespace IpService.Domain.Query.Abstractions;

public interface IFilter<TModel, TQuery> where TModel : class
{
    TQuery Apply(TQuery provider);
}