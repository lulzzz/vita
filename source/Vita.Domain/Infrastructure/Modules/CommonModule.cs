using System.Collections.Generic;
using System.Reflection;
using Autofac;
using MassTransit;
using Vita.Domain.Places;
using Vita.Domain.Services;
using Vita.Domain.Services.Predictions;
using Vita.Domain.Services.TextClassifiers;
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

      builder.RegisterType<TextClassifier>()
        .As<ITextClassifier>()
        .SingleInstance()
        //.WithParameter((info, context) => info.Name == "companies", (info, context) => context.Resolve<IRepository<Company>>().GetAll())
        //.WithParameter((info, context) => info.Name == "localities", (info, context) => context.Resolve<IRepository<Locality>>().GetAll())
        //.WithParameter((info, context) => info.Name == "classifiers", (info, context) => context.Resolve<IRepository<Classifier>>().GetAll())
        ;

      builder.RegisterType<Predictor>().As<IPredictor>();
    }
  }
}