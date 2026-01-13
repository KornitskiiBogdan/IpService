using Autofac;
using IpService.Dal;
using IpService.Dal.Ef;
using IpService.Service.Consumers;
using MapsterMapper;
using MediatR;
using System.Reflection;
using IpService.Contracts;
using Module = Autofac.Module;

namespace IpService.Service
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .RegisterType<MigrationService<UserIpContext>>()
                .As<IMigrationService>()
                .SingleInstance();

            builder
                .RegisterType<ServiceMapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ServiceModule))!)
                .Where(t => t.IsClosedTypeOf(typeof(IRequestHandler<,>)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<MessageConsumer>()
                .As<IMessageConsumer<UserIpEventMessage>>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<KafkaConsumer<string, UserIpEventMessage>>()
                .As<IKafkaConsumer<string, UserIpEventMessage>>()
                .InstancePerLifetimeScope();

            builder
                .RegisterGeneric(typeof(MessageDeserializer<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterGeneric(typeof(MessageSerializer<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();


            builder.RegisterModule<DalModule>();

            MappingsConfigurationFactory.CreateGlobalConfig();
        }
    }
}
