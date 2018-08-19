using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using DbUp;
using EventFlow;
using EventFlow.Autofac.Extensions;
using EventFlow.MsSql;
using EventFlow.MsSql.EventStores;
using EventFlow.MsSql.Extensions;
using MediatR;s
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

      EnsureDatabase.For.SqlDatabase(connection);

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

      foreach (var script in result.Scripts)
      {
        Consoler.Write($"script: {script.Name}");
      }

      if (result.Error != null)
      {
        Consoler.ShowError(result.Error);
      }

      await Task.CompletedTask;

      Consoler.TitleEnd("flash database finished");
      return  result.Successful;
    }
  }
}
