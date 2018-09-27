using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using EventFlow;
using EventFlow.Autofac.Extensions;
using EventFlow.Commands;
using EventFlow.Extensions;
using EventFlow.Jobs;
using EventFlow.Logs;
using EventFlow.MetadataProviders;
using EventFlow.MsSql;
using EventFlow.MsSql.Extensions;
using EventFlow.ReadStores;
using Vita.Contracts;
using Vita.Domain.BankStatements.ReadModels;
using Vita.Domain.Infrastructure.EventFlow;
using Module = Autofac.Module;

namespace Vita.Domain.Infrastructure.Modules
{
    public class EventFlowModule : Module
    {
        public static readonly Assembly DomainAssembly = typeof(EventFlowModule).Assembly;
        public IContainer Container { get; }

        public EventFlowModule(ContainerBuilder builder, Func<IEventFlowOptions, IEventFlowOptions> extraConfig = null)
        {
            Load(builder);
            Container = ConfigEventFlowAndBuildContainer(builder, extraConfig);
        }

        private IContainer ConfigEventFlowAndBuildContainer(ContainerBuilder builder,
            Func<IEventFlowOptions, IEventFlowOptions> extraConfig)
        {
            var eventFlowOptions = EventFlowOptions.New
                    .UseAutofacContainerBuilder(builder)
                    .AddDefaults(DomainAssembly)
                    .Configure(c => c.ThrowSubscriberExceptions = true)
                    .RegisterServices(sr =>
                    {
                        sr.Register<ILog, LogAdapter>();
                        sr.Decorate<ICommandBus>((r, o) => new LoggingCommandBus(o));
                        sr.RegisterType(typeof(BankStatementReadModelLocator));
                    })
                    .ConfigureMsSql(MsSqlConfiguration.New.SetConnectionString(Constant.ConnectionString))
                    .UseMssqlEventStore()
                    .UseReadModels()
                    // .UseMssqlReadModelFor<BankStatementAggregate, BankStatementId, BankStatementReadModel>()  - aggregate
                    .UseMssqlReadModel<BankStatementReadModel, BankStatementReadModelLocator>() // this is per prediction for the read model
                    .AddCommandHandlers()
                    .AddMetadataProviders()
                    .AddMetadataProvider<AddGuidMetadataProvider>()
                // .AddMetadataProvider<AddUriMetadataProvider>()
                //  .AddMetadataProvider<AddUserHostAddressMetadataProvider>()
                    ;


            var container = eventFlowOptions.CreateContainer(false);
            return container;
        }

        protected sealed override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }
    }


    public static class VitaEventFlowOptionsExtensions
    {
        public static IEventFlowOptions AddMetadataProviders(this IEventFlowOptions options)
        {
            options
                .AddMetadataProvider<AddGuidMetadataProvider>()
                .AddMetadataProvider<AddMachineNameMetadataProvider>()
                .AddMetadataProvider<AddEventTypeMetadataProvider>();
            return options;
        }

        public static IEventFlowOptions UseReadModels(this IEventFlowOptions options)
        {
            //options
            //	.UseMssqlReadModel<LoanApplicationReadModel>()
            //	.UseMssqlReadModel<LoanApplicationDecisionOutcomeReadModel>()
            //	.UseMssqlReadModel<LoanApplicationConditionalOfferReadModel>()
            //	.UseMssqlReadModel<LoanApplicationAppScreenReadModel>()
            //	.UseMssqlReadModel<VolumeTrendsReportReadModel>()
            //	.UseMssqlReadModel<DecisionMatrixReportReadModel>()
            //	.UseMssqlReadModel<AppFormReadModel>()
            //	;
            return options;
        }

        public static IEventFlowOptions UseMssqlReadModelWithLocator<TILocator, TLocator, TReadModel>(
            this IEventFlowOptions options)
            where TILocator : class, IReadModelLocator
            where TLocator : class, TILocator
            where TReadModel : class, IReadModel, new()
        {
            options
                .RegisterServices(r => r.Register<TILocator, TLocator>())
                .UseMssqlReadModel<TReadModel, TILocator>();

            return options;
        }

        public static IEventFlowOptions AddJobs(this IEventFlowOptions eventFlowOptions, Assembly fromAssembly,
            Predicate<Type> predicate = null)
        {
            predicate = predicate ?? (t => true);
            var jobTypes = fromAssembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i == typeof(IJob)))
                .Where(t => predicate(t));
            return eventFlowOptions.AddJobs(jobTypes);
        }
    }
}