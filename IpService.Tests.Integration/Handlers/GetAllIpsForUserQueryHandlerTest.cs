using IpService.Contracts;
using IpService.Dal.Ef.QueryProvider;
using IpService.Dal.Queries.Filters;
using IpService.Dal.Queries.Materializers;
using IpService.Domain.Entities;
using IpService.Service.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Threading;
using IpService.Domain.Store;
using IpService.Service.Queries;

namespace IpService.Tests.Integration.Handlers;

[TestFixture]
public class GetAllIpsForUserQueryHandlerTest : CommandHandlerTestBase
{
    [Test]
    public async Task GetAllIpsForUser_ShouldBeCorrectly()
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

        var query = new GetAllIpsForUserQuery(127);

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

        var resultGetting =  await mediator.Send(query, CancellationToken.None);
        resultGetting.Length.ShouldBe(2);
        resultGetting.ShouldBeEquivalentTo(new IpAddressDto[]
        {
            new() { Address = "127.0.0.1" }, 
            new() { Address = "127.0.0.2" }
        });
    }

}