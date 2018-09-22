using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CommandLine;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using Vita.Domain.Companies.Commands;
using Vita.Domain.Infrastructure;
using Vita.Setup.FlashDatabase;
using Vita.Setup.SeedDatabase;
using Module = Autofac.Module;

namespace Vita.Setup
{
    internal class Program
    {
        private static IMediator _mediator;

        /// <summary>
        ///     Debug arg
        ///     -f = flash local
        ///     -s = seed local 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            ShowHeader();
            var builder = IocContainer.GetBuilder(Assembly.GetAssembly(typeof(Domain.CollectionBase)));
            // this will add all your Request- and Notificationhandler
            // that are located in the same project as your program-class
            builder.AddMediatR(typeof(Program).Assembly);
            IocContainer.CreateContainer(builder);

            _mediator =  IocContainer.Container.Resolve<IMediator>();
            var found = IocContainer.Container.Resolve<CreateCompanyCommandHandler>();
            Debug.Assert(found!=null);

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(async opts => await ParseOptions(opts))
                .WithNotParsed(ShowParseError);
        }

        private static async Task ParseOptions(Options opts)
        {
            if (opts.FlashDatabase)
            {
                var result = await _mediator.Send(new FlashCommand());
                if (result) Consoler.Success(true);
            }

            if (opts.SeedDatabase)
            {
                var result = await _mediator.Send(new SeedCommand());
                if (result) Consoler.Success(true);
            }
        }

        private static void ShowParseError(IEnumerable<Error> errs)
        {
            Consoler.TitleStart("ARGS ERRORS");
            foreach (var error in errs)
            {
                Consoler.Warn(error.Tag.ToString());
                Consoler.Warn(error.ToString());
            }

            Consoler.TitleEnd("ARGS ERRORS");
        }

        public static void ShowHeader()
        {
            var sb = new StringBuilder();
            sb.AppendLine("This utility performs actions such as.");
            sb.AppendLine("Database flash/seed");
            Consoler.ShowHeader("Vita Build Tools", sb.ToString());
        }

        public static void ShowHelpAndExit(bool prompt = false)
        {
            Consoler.Title("Usage:");
            Consoler.Write("/help\t\t\tShow help");
            Consoler.Write("Examples:");
            Consoler.Write(@"see scripts/database folder");
            Consoler.Write("");
            ShowPauseAndExit(prompt);
        }

        public static void ShowCompletedAndExit(bool prompt = false)
        {
            Consoler.Success(prompt);
            Environment.Exit(0);
        }

        public static void ShowErrorAndExit(Exception e, bool prompt = false)
        {
            Consoler.ShowError(e, prompt);
            Environment.Exit(1);
        }

        public static void ShowPauseAndExit(bool prompt = false)
        {
            Consoler.Pause(prompt);
            Environment.Exit(0);
        }
    }
}