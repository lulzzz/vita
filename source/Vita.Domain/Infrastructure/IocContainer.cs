using System.Collections.Generic;
using System.Reflection;
using Autofac;
using EventFlow.Owin.Middlewares;
using MassTransit;
using Vita.Domain.Charges;
using Vita.Domain.Infrastructure.Modules;
using Module = Autofac.Module;

namespace Vita.Domain.Infrastructure
{
    public static class IocContainer
    {
        public static ContainerBuilder GetBuilder(Assembly ass = null, IList<Module> modules = null)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<LoggingModule>();
            builder.RegisterModule<CommonModule>();

            builder.RegisterType<CommandPublishMiddleware>().InstancePerRequest();

            if (modules != null)
                foreach (var m in modules)
                    builder.RegisterModule(m);

            builder.RegisterConsumers(Assembly.GetAssembly(typeof(ChargeAggregate)));
            if (ass != null) builder.RegisterConsumers(ass);

            return builder;
        }

        /// <summary>
        /// Eventflow wants to create the container
        /// </summary>
        /// <param name="builder"></param>
        public static void CreateContainer(ContainerBuilder builder)
        {
            var module = new EventFlowModule(builder);
            IocContainer.Container = module.Container;
        }

        public static IContainer Container { get; private set; }
    }
}