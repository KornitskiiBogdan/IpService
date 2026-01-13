using Autofac;
using IpService.Service;

namespace IpService.Host
{
    public class HostModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterModule<ServiceModule>();
        }
    }
}
