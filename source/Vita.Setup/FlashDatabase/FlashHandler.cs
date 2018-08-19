﻿using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using DbUp;
using EventFlow;
using EventFlow.Autofac.Extensions;
using EventFlow.MsSql;
using EventFlow.MsSql.EventStores;
using EventFlow.MsSql.Extensions;
using MediatR;
using Vita.Domain.Infrastructure;

namespace Vita.Setup.FlashDatabase
{
    public class FlashHandler : IRequestHandler<FlashCommand, bool>
    {
        public async Task<bool> Handle(FlashCommand request, CancellationToken cancellationToken)
        {
            Consoler.TitleStart("flash database start");

            var connectionString =
                ConfigurationManager.ConnectionStrings["Vita"].ConnectionString;

            Consoler.TitleStart("Create Database if not exist ...");
            EnsureDatabase.For.SqlDatabase(connectionString);
            Consoler.TitleEnd("Completed");

            // EventFlow Schema
            EventFlowEventStoresMsSql.MigrateDatabase(
                EventFlowOptions.New
                    .UseAutofacContainerBuilder(IocContainer.GetBuilder(Assembly.GetExecutingAssembly()))
                    .ConfigureMsSql(MsSqlConfiguration.New.SetConnectionString(connectionString))
                    .CreateResolver()
                    .Resolve<IMsSqlDatabaseMigrator>()
            );
            Consoler.Write("EventFlow Schema Created...");

            // Schema
            var result = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithTransaction()
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build()
                .PerformUpgrade();

            foreach (var script in result.Scripts) Consoler.Write($"script: {script.Name}");

            if (result.Error != null) Consoler.ShowError(result.Error);

            await Task.CompletedTask;

            Consoler.TitleEnd("flash database finished");
            return result.Successful;
        }
    }
}