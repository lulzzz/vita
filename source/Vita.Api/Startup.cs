using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSwag;
using NSwag.AspNetCore;
using Vita.Domain.Charges;
using Vita.Domain.Infrastructure;
using Vita.Domain.Infrastructure.Modules;
using Vita.Predictor;

namespace Vita.Api
{
  public class Startup
  {
    public IConfiguration Configuration { get; }
    public IContainer Container { get; set; }

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



      var builder = new ContainerBuilder();
      builder.RegisterModule<LoggingModule>();
      builder.RegisterModule<CommonModule>();
      builder.RegisterModule<PredictionModule>();
      builder.RegisterConsumers(Assembly.GetAssembly(typeof(ChargeClassifier)));
      builder.RegisterConsumers(Assembly.GetExecutingAssembly());


      builder.Populate(services);

      //Create Autofac ContainerBuilder 
      Container = builder.Build();
      //var container = ConfigureServicesSingleInstance(containerBuilder, services);
      //var container = ConfigureServicesInstancePerLifetimeScope(containerBuilder, services);
      //var container = ConfigureServicesInstancePerMatchingLifetimeScope
      //                (containerBuilder, services);
      //var container = ConfigureServicesInstancePerRequest(containerBuilder, services);
      //var container = ConfigureServicesInstancePerOwned(containerBuilder, services);
      //var container = ConfigureServicesThreadScope(containerBuilder, services);
      //Create Autofac Service Provider & assign Autofac container 
      var autofacServiceProvider = new AutofacServiceProvider(Container);
      //Finally return Autofac Service Provider.
      return autofacServiceProvider;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      if (env.IsDevelopment())
      {
        app.UseMiddleware<StackifyMiddleware.RequestTracerMiddleware>();
        app.UseDeveloperExceptionPage();
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
          document.Info.Contact = new SwaggerContact()
          {
            Name = "Info",
            Email = "info@chargeid.com",
            Url = "www.chargeid.com"
          };
          document.Info.Version = Assembly.GetExecutingAssembly().ImageRuntimeVersion;
        };
      });

      //app.UseHttpsRedirection();
      // app.UseCorsMiddleware();
      app.UseCors("AllowAll");
      app.UseMvc();
    }
  }
}