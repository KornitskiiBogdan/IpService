using IpService.Dal.Ef.Queries.Factories;
using IpService.Domain.Entities;
using IpService.Domain.Query;
using IpService.Domain.Query.Abstractions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace IpService.Dal.Queries.Materializers
{
    public static class UserIpMaterializers
    {
        public static IExecutor<UserIp, TDest[], IQueryable<UserIp>> SelectByFilter<TDest>(
            IFilter<UserIp, IQueryable<UserIp>>? filter = null, ISorter<UserIp, IQueryable<UserIp>>? sorter = null) where TDest : class
        {
            return ExecutorFactory.Create<UserIp, TDest[]>((u, t) =>
            {
                var query = filter?.Apply(u) ?? u;
                query = sorter?.Apply(query) ?? query;
                var projectedQuery = query.ProjectToType<TDest>();
                return projectedQuery.ToArrayAsync(t);
            });
        }

        public static IExecutor<UserIp, TDest?, IQueryable<UserIp>> SelectFirstByFilter<TDest>(
            IFilter<UserIp, IQueryable<UserIp>>? filter = null, ISorter<UserIp, IQueryable<UserIp>>? sorter = null) where TDest : class
        {
            return ExecutorFactory.Create<UserIp, TDest?>((u, t) =>
            {
                var query = filter?.Apply(u) ?? u;
                query = sorter?.Apply(query) ?? query;
                var projectedQuery = query.ProjectToType<TDest>();
                return projectedQuery.FirstOrDefaultAsync(t);
            });
        }

        public static IExecutor<UserIp, TDest[], IQueryable<UserIp>> SelectDistinctByFilter<TDest>(
            Filter<UserIp, IQueryable<UserIp>> filter) where TDest : class
        {
            return ExecutorFactory.Create<UserIp, TDest[]>((u, t) =>
            {
                var query = filter.Apply(u);
                var projectedQuery = query.ProjectToType<TDest>();
                projectedQuery = projectedQuery.Distinct();
                return projectedQuery.ToArrayAsync(t);
            });
        }
    }
}
