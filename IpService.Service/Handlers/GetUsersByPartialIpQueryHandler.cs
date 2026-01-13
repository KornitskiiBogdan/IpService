using IpService.Contracts;
using IpService.Cqrs;
using IpService.Dal.Ef.QueryProvider;
using IpService.Dal.Queries.Filters;
using IpService.Dal.Queries.Materializers;
using IpService.Domain.Entities;
using IpService.Service.Queries;

namespace IpService.Service.Handlers
{
    public class GetUsersByPartialIpQueryHandler(IQueryProvider<UserIp> queryProviderUserIp) : IQueryHandler<GetUsersByPartialIpQuery, UserIdDto[]>
    {
        public async Task<UserIdDto[]> Handle(GetUsersByPartialIpQuery request, CancellationToken cancellationToken)
        {
            return await queryProviderUserIp.ProjectAsync(UserIpMaterializers.SelectByFilter<UserIdDto>(UserIpFilters.ByUserIp(request.PartialIp)), token: cancellationToken);
        }
    }
}
