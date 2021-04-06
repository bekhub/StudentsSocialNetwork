using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Core;
using Core.Interfaces.Services;
using Infrastructure.Data.Repositories;
using Infrastructure.Services;
using MediatR;
using MediatR.Pipeline;
using Module = Autofac.Module;

namespace Infrastructure
{
    public class DefaultInfrastructureModule : Module
    {
        private readonly List<Assembly> _assemblies = new();

        public DefaultInfrastructureModule(Assembly callingAssembly = null)
        {
            var coreAssembly = Assembly.GetAssembly(typeof(CoreAssembly));
            var infrastructureAssembly = Assembly.GetAssembly(typeof(EfRepository<>));
            _assemblies.Add(coreAssembly);
            _assemblies.Add(infrastructureAssembly);
            if (callingAssembly != null)
            {
                _assemblies.Add(callingAssembly);
            }
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterCommonDependencies(builder);
        }

        private void RegisterCommonDependencies(ContainerBuilder builder)
        {
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                .RegisterAssemblyTypes(_assemblies.ToArray())
                .AsClosedTypesOf(mediatrOpenType)
                .AsImplementedInterfaces();
            }

            builder.RegisterType<EmailSender>().As<IEmailSender>()
                .InstancePerLifetimeScope();
        }
    }
}
