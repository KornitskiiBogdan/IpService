using IpService.Contracts;
using IpService.Cqrs;

namespace IpService.Service.Queries
{
    public record GetAllIpsForUserQuery(long UserId) : IQuery<IpAddressDto[]>
    {
        public static GetAllIpsForUserQuery Create(long userId) => new(userId);
    }
}
