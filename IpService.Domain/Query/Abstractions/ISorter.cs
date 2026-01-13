namespace IpService.Domain.Query.Abstractions;

public interface ISorter<TModel, TQuery> : IApplicable<TModel, TQuery> where TModel : class
{
}