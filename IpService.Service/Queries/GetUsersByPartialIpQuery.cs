using IpService.Contracts;
using IpService.Cqrs;

namespace IpService.Service.Queries
{
    public record GetUsersByPartialIpQuery(string PartialIp) : IQuery<UserIdDto[]>
    {
        public static GetUsersByPartialIpQuery Create(string partialIp) => new(partialIp);
    }
}
