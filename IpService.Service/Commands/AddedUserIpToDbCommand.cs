using IpService.Contracts;
using IpService.Cqrs;
using IpService.Domain.Entities;

namespace IpService.Service.Commands;

public record AddedUserIpToDbCommand(UserIpEventMessage Message) : ICommand<Result>
{
    public static AddedUserIpToDbCommand Create(UserIpEventMessage message) => new(message);
}