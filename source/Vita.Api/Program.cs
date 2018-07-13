using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Vita.Domain.Infrastructure;

namespace Vita.Api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      JsonConvert.DefaultSettings = () => VitaSerializerSettings.Settings;
      CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
      return WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>();
    }
  }
} 