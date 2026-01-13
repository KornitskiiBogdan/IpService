using Autofac;
using IpService.Dal.Ef;

namespace IpService.Dal
{
    public class DalModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterModule<DalModule<UserIpContext>>();
        }
    }
}
