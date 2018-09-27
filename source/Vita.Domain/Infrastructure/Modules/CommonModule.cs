using System;
using System.Reflection;
using Autofac;
using MassTransit;
using Vita.Contracts;
using Vita.Domain.BankStatements;
using Vita.Domain.Companies.Commands;
using Vita.Domain.Companies.Events;
using Vita.Domain.Infrastructure.EventFlow;
using Vita.Domain.Places;
using Vita.Domain.Services;
using Module = Autofac.Module;

namespace Vita.Domain.Infrastructure.Modules
{
  public class CommonModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
        .Where(t => t.Name.Contains("Vita"))
        .AsImplementedInterfaces();

      builder.RegisterType<SerializedQueryProcessor>().As<ISerializedQueryProcessor>();

      builder.RegisterType<CustomSerializedCommandPublisher>().As<ICustomSerializedCommandPublisher>();

      builder.RegisterType<CreateCompanyCommandHandler>();
      builder.RegisterType<CreateCompanyCommand>();
      builder.RegisterType<CompanyCreatedEvent>();

         

      //bank
      builder.RegisterType<BankStatementService>()
        .As<IBankStatementService>()
        .WithParameter("bankStatementsConfiguration",
          new BankStatementsConfiguration(Mode.Test.ApiUrl, Mode.Test.ApiKey, Mode.Test.Prefix))
        ;
      //
      builder.RegisterGeneric(typeof(Repository<>))
        .As(typeof(IRepository<>)).SingleInstance();

      builder.RegisterConsumers(Assembly.GetAssembly(typeof(GoogleApiSearchHandler)));

      builder.RegisterType<GooglePlacesSearcher>().As<IGooglePlacesSearcher>();

      base.Load(builder);
    }
  }
}