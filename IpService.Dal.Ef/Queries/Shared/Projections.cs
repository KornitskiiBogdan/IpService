using IpService.Dal.Ef.Queries.Factories;
using IpService.Domain.Query.Abstractions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace IpService.Dal.Ef.Queries.Shared;

public static class Projections<TModel, TProject> where TModel : class
{
    public static IExecutor<TModel, TProject, IQueryable<TModel>> FirstOrDefault { get; } =
        ExecutorFactory.Create<TModel, TProject>((q, t) => q.ProjectToType<TProject>().FirstOrDefaultAsync(t));

    public static IExecutor<TModel, TProject, IQueryable<TModel>> SingleOrDefault { get; } =
        ExecutorFactory.Create<TModel, TProject>((q, t) => q.ProjectToType<TProject>().SingleOrDefaultAsync(t));

    public static IExecutor<TModel, TProject[], IQueryable<TModel>> Enumerable { get; } =
        ExecutorFactory.Create<TModel, TProject[]>((q, t) => q.ProjectToType<TProject>().ToArrayAsync(t));
}