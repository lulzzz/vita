using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using EventFlow;
using EventFlow.Autofac.Extensions;
using EventFlow.Extensions;
using EventFlow.Jobs;
using EventFlow.Logs;
using EventFlow.MetadataProviders;
using EventFlow.MsSql;
using EventFlow.MsSql.Extensions;
using EventFlow.ReadStores;
using Vita.Contracts;
using Vita.Domain.BankStatements;
using Vita.Domain.BankStatements.ReadModels;
using Vita.Domain.Infrastructure.EventFlow;
using Log = Serilog.Log;
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
                //.AddCommands(typeof(CreateCompanyCommand))
                //.AddCommandHandlers(typeof(CreateCompanyCommandHandler))
                //.AddEvents(typeof(CompanyCreatedEvent))
                .Configure(c => c.ThrowSubscriberExceptions = true)
                .RegisterServices(sr =>
                {
                    sr.Register<ILog, LogAdapter>();
                    sr.Decorate<ICommandBus>((r, o) => new LoggingCommandBus(o));
                    sr.RegisterType(typeof(BankStatementReadModelLocator));
                })
                
                ;

            eventFlowOptions = eventFlowOptions
                    .ConfigureMsSql(MsSqlConfiguration.New.SetConnectionString(Constant.ConnectionString))
                    .UseMssqlEventStore()
                    .UseReadModels()
                   // .UseMssqlReadModelFor<BankStatementAggregate, BankStatementId, BankStatementReadModel>()  - aggregate
                    .UseMssqlReadModel<BankStatementReadModel, BankStatementReadModelLocator>() // this is per prediction for the read model
                    .AddCommandHandlers()
                    .AddMetadataProviders()
                //.AddMetadataProvider<AddGuidMetadataProvider>()
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

    [DebuggerStepThrough]
    public class LogAdapter : ILog
    {
        public void Verbose(string format, params object[] args)
        {
            Log.Logger.Verbose(format, args);
        }

        public void Verbose(Exception exception, string format, params object[] args)
        {
            Log.Logger.Verbose(exception, format, args);
        }

        public void Verbose(Func<string> combersomeLogging)
        {
            Log.Logger.Verbose(combersomeLogging());
        }

        public void Verbose(Action<StringBuilder> combersomeLogging)
        {
            var sb = new StringBuilder();
            combersomeLogging(sb);
            Log.Logger.Verbose(sb.ToString());
        }

        public void Debug(string format, params object[] args)
        {
            Log.Logger.Debug(format, args);
        }

        public void Debug(Exception exception, string format, params object[] args)
        {
            Log.Logger.Debug(exception, format, args);
        }

        public void Debug(Func<string> combersomeLogging)
        {
            Log.Logger.Debug(combersomeLogging());
        }

        public void Debug(Action<StringBuilder> combersomeLogging)
        {
            var sb = new StringBuilder();
            combersomeLogging(sb);
            Log.Logger.Debug(sb.ToString());
        }

        public void Information(string format, params object[] args)
        {
            Log.Logger.Information(format, args);
        }

        public void Information(Exception exception, string format, params object[] args)
        {
            Log.Logger.Information(exception, format, args);
        }

        public void Information(Func<string> combersomeLogging)
        {
            Log.Logger.Information(combersomeLogging());
        }

        public void Information(Action<StringBuilder> combersomeLogging)
        {
            var sb = new StringBuilder();
            combersomeLogging.Invoke(sb);
            Log.Logger.Information(sb.ToString());
        }

        public void Warning(string format, params object[] args)
        {
            Log.Logger.Warning(format, args);
        }

        public void Warning(Exception exception, string format, params object[] args)
        {
            Log.Logger.Warning(exception, format, args);
        }

        public void Error(string format, params object[] args)
        {
            Log.Logger.Error(format, args);
        }

        public void Error(Exception exception, string format, params object[] args)
        {
            Log.Logger.Error(exception, format, args);
        }

        public void Fatal(string format, params object[] args)
        {
            Log.Logger.Fatal(format, args);
        }

        public void Fatal(Exception exception, string format, params object[] args)
        {
            Log.Logger.Fatal(exception, format, args);
        }

        public void Write(global::EventFlow.Logs.LogLevel logLevel, string format, params object[] args)
        {
            Write(logLevel, format, args);
        }

        public void Write(global::EventFlow.Logs.LogLevel logLevel, Exception exception, string format, params object[] args)
        {
            Write(logLevel, format, args);
        }

        public void Write(LogLevel logLevel, string format, params object[] args)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    Debug(string.Format(format, args));
                    break;
                case LogLevel.Information:
                    Information(string.Format(format, args));
                    break;
                case LogLevel.Warning:
                    Warning(string.Format(format, args));
                    break;
                case LogLevel.Error:
                    Error(string.Format(format, args));
                    break;
                case LogLevel.Verbose:
                    Verbose(string.Format(format, args));
                    break;
                case LogLevel.Fatal:
                    Fatal(string.Format(format, args));
                    break;
            }
        }

        public void Write(LogLevel logLevel, Exception exception, string format, params object[] args)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    Debug(string.Format(format, args));
                    break;
                case LogLevel.Information:
                    Information(string.Format(format, args));
                    break;
                case LogLevel.Warning:
                    Warning(exception, string.Format(format, args));
                    break;
                case LogLevel.Error:
                    Error(exception, string.Format(format, args));
                    break;
                case LogLevel.Verbose:
                    Verbose(string.Format(format, args));
                    break;
                case LogLevel.Fatal:
                    Fatal(exception, string.Format(format, args));
                    break;
            }
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