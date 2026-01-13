namespace IpService.Domain.Query.Abstractions;

public interface IPaginate<TModel, TQuery> : IApplicable<TModel, TQuery> where TModel : class
{
}