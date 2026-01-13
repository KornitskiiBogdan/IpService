using IpService.Domain.Entities;
using IpService.Domain.Store;
using IpService.Service.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace IpService.Tests.Integration.Handlers;

[TestFixture]
public class GetLastConnectionForUserQueryHandler : CommandHandlerTestBase
{
    [Test]
    public async Task GetLastConnectionForUser_ShouldBeCorrectly()
    {
        var entity1 = new UserIp()
        {
            IpAddress = "127.0.0.1",
            UserId = 127,
            LastConnectedAt = DateTimeOffset.Now.ToUniversalTime()
        };

        var entity2 = new UserIp()
        {
            IpAddress = "127.0.0.2",
            UserId = 127,
            LastConnectedAt = DateTimeOffset.Now.ToUniversalTime()
        };

        var query = new GetLastConnectionForUserQuery(127);

        await using var scope = ServiceProvider.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var store = scope.ServiceProvider.GetRequiredService<IStore<UserIp>>();

        var result = await store.AddAsync(entity1, CancellationToken.None);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        result = await store.AddAsync(entity2, CancellationToken.None);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        var resultGetting = await mediator.Send(query, CancellationToken.None);
        resultGetting.Ip.ShouldBe(entity2.IpAddress);
    }
}