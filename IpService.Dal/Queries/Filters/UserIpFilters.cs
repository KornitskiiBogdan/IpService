using IpService.Dal.Ef.Queries.Factories;
using IpService.Domain.Entities;
using IpService.Domain.Query;

namespace IpService.Dal.Queries.Filters
{
    public static class UserIpFilters
    {
        public static Filter<UserIp, IQueryable<UserIp>> ByUserId(long userIp) => FilterFactory.Create<UserIp>(f => f.UserId == userIp);

        public static Filter<UserIp, IQueryable<UserIp>> ByUserIp(string partialIp) => FilterFactory.Create<UserIp>(f => f.IpAddress.Contains(partialIp));
    }
}
