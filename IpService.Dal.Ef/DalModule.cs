using Autofac;
using Autofac.Core;
using IpService.Dal.Ef.QueryProvider;
using IpService.Dal.Ef.Store;
using IpService.Dal.Ef.Transactions;

namespace IpService.Dal.Ef;

public class DalModule<TContext> : Module where TContext : DbContextBase
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder
            .RegisterGeneric(typeof(Store<>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .WithParameter(new ResolvedParameter((pi, _) => pi.ParameterType == typeof(DbContextBase),
                (_, ctx) => ctx.Resolve<TContext>()));

        builder
            .RegisterGeneric(typeof(QueryProvider<>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .WithParameter(new ResolvedParameter((pi, _) => pi.ParameterType == typeof(DbContextBase),
                (_, ctx) => ctx.Resolve<TContext>()));

        builder
            .RegisterType<TransactionManager>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .WithParameter(new ResolvedParameter((pi, _) => pi.ParameterType == typeof(DbContextBase),
                (_, ctx) => ctx.Resolve<TContext>()));
    }
}