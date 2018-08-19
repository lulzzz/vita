using System;
using System.Net;
using System.Reflection;
using Autofac;
using MassTransit;
using Vita.Contracts;
using Vita.Domain.Places;
using Vita.Domain.Services;
using Module = Autofac.Module;

namespace Vita.Domain.Infrastructure.Modules
{
    public class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => t.Name.Contains("Vita"))
                .AsImplementedInterfaces();

            builder.RegisterType<GooglePlacesSearcher>().As<IGooglePlacesSearcher>();

            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>)).SingleInstance();

            builder.RegisterConsumers(Assembly.GetAssembly(typeof(GoogleApiSearchHandler)));
        }
    }
}