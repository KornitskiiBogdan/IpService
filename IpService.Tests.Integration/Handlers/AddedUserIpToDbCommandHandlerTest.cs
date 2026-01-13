using IpService.Contracts;
using IpService.Dal.Ef.QueryProvider;
using IpService.Dal.Queries.Filters;
using IpService.Dal.Queries.Materializers;
using IpService.Domain.Entities;
using IpService.Service.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace IpService.Tests.Integration.Handlers
{
    [TestFixture]
    public class AddedUserIpToDbCommandHandlerTest : CommandHandlerTestBase
    {
        [Test]
        public async Task AddedUserIpToDbCommandHandler_ShouldBeCorrectly()
        {
            var userIpEventMessage = new UserIpEventMessage()
            {
                IpAddress = "127.0.0.1",
                UserId = 127,
                LastConnectedAt = DateTimeOffset.Now
            };

            var request = AddedUserIpToDbCommand.Create(userIpEventMessage);

            await using var scope = ServiceProvider.CreateAsyncScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var queryProvider = scope.ServiceProvider.GetRequiredService<IQueryProvider<UserIp>>();

            await mediator.Send(request);

            var users = await queryProvider.ProjectAsync(UserIpMaterializers.SelectByFilter<UserIp>(UserIpFilters.ByUserId(userIpEventMessage.UserId)));
            var user = users.SingleOrDefault();

            ArgumentNullException.ThrowIfNull(user);
            user.IpAddress.ShouldBe("127.0.0.1");
            user.UserId.ShouldBe(127);
        }
    }
}
