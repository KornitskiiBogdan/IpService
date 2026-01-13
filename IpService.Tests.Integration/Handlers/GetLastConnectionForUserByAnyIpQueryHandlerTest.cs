using IpService.Contracts;
using IpService.Domain.Entities;
using IpService.Domain.Store;
using IpService.Service.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace IpService.Tests.Integration.Handlers;

[TestFixture]
public class GetLastConnectionForUserByAnyIpQueryHandlerTest : CommandHandlerTestBase
{
    [Test]
    public async Task GetLastConnectionForUserByAnyIp_ShouldBeCorrectly()
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

        var query = new GetLastConnectionForUserByAnyIpQuery(127);

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
        resultGetting.Date.Millisecond.ShouldBe(entity2.LastConnectedAt.Millisecond);
    }
}