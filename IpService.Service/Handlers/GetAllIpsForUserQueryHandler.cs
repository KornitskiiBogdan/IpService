using IpService.Contracts;
using IpService.Cqrs;
using IpService.Dal.Ef.QueryProvider;
using IpService.Dal.Queries.Filters;
using IpService.Dal.Queries.Materializers;
using IpService.Domain.Entities;
using IpService.Service.Queries;

namespace IpService.Service.Handlers
{
    public class GetAllIpsForUserQueryHandler(IQueryProvider<UserIp> queryProviderUserIp) : IQueryHandler<GetAllIpsForUserQuery, IpAddressDto[]>
    {
        public async Task<IpAddressDto[]> Handle(GetAllIpsForUserQuery request, CancellationToken cancellationToken)
        {
            return await queryProviderUserIp.ProjectAsync(UserIpMaterializers.SelectByFilter<IpAddressDto>(UserIpFilters.ByUserId(request.UserId)), token: cancellationToken);
        }
    }
}
