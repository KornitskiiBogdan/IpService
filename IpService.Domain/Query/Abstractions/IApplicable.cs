namespace IpService.Domain.Query.Abstractions;

public interface IApplicable<TModel, TQuery> where TModel : class
{
    TQuery Apply(TQuery provider);
}