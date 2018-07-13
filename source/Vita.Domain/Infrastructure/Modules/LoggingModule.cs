using System;
using Autofac;
using Serilog;
using Serilog.Events;
using Vita.Contracts;
using Vita.Domain.Infrastructure.Logging;

namespace Vita.Domain.Infrastructure.Modules
{
  public class LoggingModule : ConfiguredModuleBase<LoggingModule, LoggingConfiguration>
  {
    protected override void Load(ContainerBuilder builder)
    {
      base.Load(builder);
      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .WriteTo.LogzIo(Constant.ApiKey.LogzIo,Environment.MachineName, new LogzioOptions
        {
          BatchPostingLimit = 2,
          UseHttps = true,
          RestrictedToMinimumLevel = LogEventLevel.Debug
        })
        .CreateLogger();
    }
  }

  public class LoggingConfiguration
  {
  }
}
