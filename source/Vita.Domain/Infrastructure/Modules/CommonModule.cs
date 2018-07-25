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
      System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

      builder.RegisterType<GooglePlacesSearcher>().As<IGooglePlacesSearcher>();

      builder.RegisterGeneric(typeof(Repository<>))
        .As(typeof(IRepository<>)).SingleInstance();
      builder.RegisterConsumers(Assembly.GetAssembly(typeof(GoogleApiSearchHandler)));

    
    }
  }
}