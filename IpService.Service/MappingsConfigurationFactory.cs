using IpService.Contracts;
using IpService.Domain.Entities;
using Mapster;

namespace IpService.Service
{
    public static class MappingsConfigurationFactory
    {
        public static TypeAdapterConfig CreateGlobalConfig()
        {
            TypeAdapterConfig.GlobalSettings.Default
                .EnumMappingStrategy(EnumMappingStrategy.ByName);

            DomainToDtoConfigure();
            DefinitionToDomainConfigure();

            return TypeAdapterConfig.GlobalSettings;
        }

        private static void DefinitionToDomainConfigure()
        {
            TypeAdapterConfig<UserIpEventMessage, UserIp>
                .NewConfig()
                .Map(d => d.UserId, s => s.UserId)
                .Map(d => d.IpAddress, s => s.IpAddress)
                .Map(d => d.LastConnectedAt, s => s.LastConnectedAt.ToUniversalTime());
        }

        private static void DomainToDtoConfigure()
        {
            TypeAdapterConfig<UserIp, UserIdDto>
                .NewConfig()
                .Map(d => d.Id, s=> s.UserId);

            TypeAdapterConfig<UserIp, IpAddressDto>
                .NewConfig()
                .Map(d => d.Address, s => s.IpAddress);

            TypeAdapterConfig<UserIp, LastConnectionDateDto>
                .NewConfig()
                .Map(d => d.Date, s => s.LastConnectedAt);

            TypeAdapterConfig<UserIp, LastConnectionDto>
                .NewConfig()
                .Map(d => d.LastConnectionDate, s => s.LastConnectedAt)
                .Map(d => d.Ip, s => s.IpAddress);
        }
    }
}
