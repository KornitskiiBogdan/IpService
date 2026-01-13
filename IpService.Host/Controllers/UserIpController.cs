using IpService.Contracts;
using IpService.Service.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IpService.Host.Controllers
{
    [Produces("application/json", new string[] { })]
    [ApiController]
    public class UserIpController(IMediator mediator) : ControllerBase
    {
        [HttpGet("ip/{userId}")]
        public async Task<IpAddressDto[]> GetAllIpsForUser([FromRoute] long userId, CancellationToken cancellationToken)
        {
            return await mediator.Send(GetAllIpsForUserQuery.Create(userId), cancellationToken);
        }

        [HttpGet("user/{partialIp}")]
        public async Task<UserIdDto[]> GetUsersByPartialIp([FromRoute] string partialIp, CancellationToken cancellationToken)
        {
            return await mediator.Send(GetUsersByPartialIpQuery.Create(partialIp), cancellationToken);
        }

        [HttpGet("ip/last")]
        public async Task<LastConnectionDto> GetLastConnectionForUser([FromQuery] long userId, CancellationToken cancellationToken)
        {
            return await mediator.Send(GetLastConnectionForUserQuery.Create(userId), cancellationToken);
        }

        [HttpGet("user/time/last")]
        public async Task<LastConnectionDateDto> GetLastConnectionForUserByAnyIp([FromQuery] long userId, CancellationToken cancellationToken)
        {
            return await mediator.Send(GetLastConnectionForUserByAnyIpQuery.Create(userId), cancellationToken);
        }
    }
}
