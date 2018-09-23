using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSwag;
using NSwag.AspNetCore;
using Serilog;
using StackifyLib;
using StackifyLib.Utils;
using StackifyMiddleware;
using Vita.Domain;
using Vita.Domain.Infrastructure;
using Vita.Predictor;
using Logger = StackifyLib.Logger;
using Module = Autofac.Module;

namespace Vita.Api
{
  public class Startup
  {
    public IConfiguration Configuration { get; }
    public IConfigurationRoot Root { get; set; }


    public Startup(IConfiguration configuration)
    {
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-AU");
      Configuration = configuration;
      JsonConvert.DefaultSettings = () => VitaSerializerSettings.Settings;
    }


    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
      // AddCors must be before AddMvc
      services.AddCors(options =>
      {
        options.AddPolicy("AllowAll",
          b => b
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowCredentials()
            .AllowAnyHeader());
      });

      //services.Configure<MvcOptions>(options => {
      //  options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAnyOrigin"));
      //});
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
        options.Authority = $"https://{Configuration["Auth0:Domain"]}/";
        options.Audience = Configuration["Auth0:ApiIdentifier"];
      });

      var builder = IocContainer.GetBuilder(Assembly.GetAssembly(typeof(CollectionBase)),
        new List<Module> {new PredictionModule()});
      builder.Populate(services);
      IocContainer.CreateContainer(builder);
      var autofacServiceProvider = new AutofacServiceProvider(IocContainer.Container);
      return autofacServiceProvider;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      if (env.IsDevelopment())
      {
        app.UseMiddleware<RequestTracerMiddleware>();
        app.UseDeveloperExceptionPage();
        //app.UseDatabaseErrorPage();                
      }
      else
      {
        app.UseHsts();
      }

      app.UseStaticFiles();
      app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, settings =>
      {
        settings.GeneratorSettings.DefaultUrlTemplate = "{controller}/{action}/{id?}";
        settings.PostProcess = document =>
        {
          document.Info.Title = "ChargeId";
          document.Info.Description = "Classifying the WWWWWH of your transactions";
          document.Info.Contact = new SwaggerContact
          {
            Name = "Info",
            Email = "info@chargeid.com",
            Url = "www.chargeid.com"
          };
          document.Info.Version = Assembly.GetExecutingAssembly().ImageRuntimeVersion;
        };
      });

      //app.UseHttpsRedirection();
      //app.UseCorsMiddleware();
      app.UseCors("AllowAll");

      var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
        .AddEnvironmentVariables();
      Root = builder.Build();

      Config.Environment = env.EnvironmentName; //optional

      app.ConfigureStackifyLogging(Root); //This is critical!!
      app.UseMvc();

      StackifyAPILogger.LogEnabled = true;
      StackifyAPILogger.OnLogMessage += StackifyAPILogger_OnLogMessage;
    }


    private static void StackifyAPILogger_OnLogMessage(string data)
    {
      Trace.WriteLine(data);
      Log.Logger.Debug(data);

    }
  }
}