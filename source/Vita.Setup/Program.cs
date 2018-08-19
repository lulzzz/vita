﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CommandLine;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.CodeAnalysis;
using Vita.Domain.Infrastructure;
using Vita.Setup.FlashDatabase;

namespace Vita.Setup
{
  internal class Program
  {
    private static IContainer _container;
    private static IMediator _mediator;

    /// <summary>
    ///   Debug args -- /d /b /u /t /c "Data Source=(local)\;Database=Fasti; Integrated Security=SSPI;" /w
    ///   /c = connection string		[required]
    ///   /b = Build				    Create database if not exist
    ///   /d = Delete					Delete database (database must exist, use with /b use not sure)
    ///   /u = Update					Apply sql scripts to database (database must exist, use with /b use not sure)
    ///   /n = NoPrompts              Uses this flag on build server to skip any prompt
    ///   /hangfire     Upgrade hangfire
    ///   /f = flash local muto
    /// </summary>
    /// <param name="args">/d /b /u  /n /w /c "Data Source=(local)\;Database=Fasti; Integrated Security=SSPI;"</param>
    public static void Main(string[] args)
    {
      ShowHeader();
      var builder = new ContainerBuilder();
      // this will add all your Request- and Notificationhandler
      // that are located in the same project as your program-class
      builder.AddMediatR(typeof(Program).Assembly);

      _container = builder.Build();

      _mediator = _container.Resolve<IMediator>();
      
      CommandLine.Parser.Default.ParseArguments<Options>(args)
        .WithParsed<Options>(async opts => await ParseOptions(opts))
        .WithNotParsed<Options>(ShowParseError);
    }

    private static async Task ParseOptions(Options opts)
    {
      if (opts.FlashDatabase)
      {
        var result = await _mediator.Send(new FlashCommand());
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
      sb.AppendLine("Database build/flash/purge");
      Consoler.ShowHeader("Vita Build Tools", sb.ToString());
    }

    public static void ShowHelpAndExit(bool noPrompt = false)
    {
      Consoler.Title("Usage:");
      Consoler.Write("/help\t\t\tShow help");
      Consoler.Write("Example:");
      Consoler.Write(@"Vita.Setup.exe /b /u /c CONNECTION STRING GOES HERE");
      Consoler.Write("");
      ShowPauseAndExit(noPrompt);
    }

    public static void ShowCompletedAndExit(bool noPrompt = false)
    {
      Consoler.Success(noPrompt);
      Environment.Exit(0);
    }

    public static void ShowErrorAndExit(Exception e, bool noPrompt = false)
    {
      Consoler.ShowError(e, noPrompt);
      Environment.Exit(1);
    }

    public static void ShowPauseAndExit(bool noPrompt = false)
    {
      Consoler.Pause(noPrompt);
      Environment.Exit(0);
    }
  }
}