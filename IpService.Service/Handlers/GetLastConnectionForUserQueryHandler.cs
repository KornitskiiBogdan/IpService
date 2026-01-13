using IpService.Contracts;
using IpService.Cqrs;
using IpService.Dal.Ef.Queries.Factories;
using IpService.Dal.Ef.QueryProvider;
using IpService.Dal.Queries.Filters;
using IpService.Dal.Queries.Materializers;
using IpService.Domain;
using IpService.Domain.Entities;
using IpService.Service.Queries;

namespace IpService.Service.Handlers
{
    public class GetLastConnectionForUserQueryHandler(IQueryProvider<UserIp> queryProviderUserIp) : IQueryHandler<GetLastConnectionForUserQuery, LastConnectionDto?>
    {
        public async Task<LastConnectionDto?> Handle(GetLastConnectionForUserQuery request, CancellationToken cancellationToken)
        {
            return await queryProviderUserIp.ProjectAsync(UserIpMaterializers.SelectFirstByFilter<LastConnectionDto>(
                UserIpFilters.ByUserId(request.UserId), 
                SorterFactory.Create<UserIp>(SortingDirection.Desc, nameof(UserIp.LastConnectedAt))), token: cancellationToken);
        }
    }
}
