using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using DbUp;
using EventFlow;
using EventFlow.Autofac.Extensions;
using EventFlow.Configuration;
using EventFlow.MsSql;
using EventFlow.MsSql.EventStores;
using EventFlow.MsSql.Extensions;
using MediatR;
using MongoDB.Driver.Core.Misc;
using Vita.Domain.Infrastructure;

namespace Vita.Setup.FlashDatabase
{
  public class FlashHandler : IRequestHandler<FlashCommand, bool>
  {
    public async Task<bool> Handle(FlashCommand request, CancellationToken cancellationToken)
    {
      Consoler.TitleStart("flash database start");

      var connection = 
        System.Configuration.ConfigurationManager.
          ConnectionStrings["Vita"].ConnectionString;

      // EventFlow Schema
      EventFlowEventStoresMsSql.MigrateDatabase(
        EventFlowOptions.New
          .UseAutofacContainerBuilder(new ContainerBuilder())
          .ConfigureMsSql(MsSqlConfiguration.New.SetConnectionString(connection))
          .CreateResolver()
          .Resolve<IMsSqlDatabaseMigrator>()
      );
      Consoler.Write("EventFlow Schema Created...");
      // Fasti Schema
      var result = DeployChanges.To
        .SqlDatabase(connection)
        .WithTransaction()
        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
        .LogToConsole()
        .Build()
        .PerformUpgrade(); 


      await Task.CompletedTask;

      Consoler.TitleEnd("flash database finished");
      return true;
    }
  }
}
