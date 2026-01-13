using IpService.Contracts;
using IpService.Cqrs;

namespace IpService.Service.Queries
{
    public record GetLastConnectionForUserByAnyIpQuery(long UserId) : IQuery<LastConnectionDateDto>
    {
        public static GetLastConnectionForUserByAnyIpQuery Create(long userId) => new(userId);
    }
}
