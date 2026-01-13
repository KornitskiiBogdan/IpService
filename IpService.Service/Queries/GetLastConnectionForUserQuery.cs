using IpService.Contracts;
using IpService.Cqrs;

namespace IpService.Service.Queries
{
    public record GetLastConnectionForUserQuery(long UserId) : IQuery<LastConnectionDto>
    {
        public static GetLastConnectionForUserQuery Create(long userId) => new(userId);
    }
}
