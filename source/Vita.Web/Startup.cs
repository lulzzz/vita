using System.Globalization;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Vita.Domain.Infrastructure;

namespace Vita.Web
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-AU");
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // Add cors
      services.AddCors(options =>
      {
        options.AddPolicy("AllowAll",
          b => b
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowCredentials()
            .AllowAnyHeader());
      });
      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
        {
          HotModuleReplacement = true
        });
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

    //  app.UseCorsMiddleware();
      app.UseCors("AllowAll");

      app.UseStaticFiles();      
      

      app.UseMvc(routes =>
      {
        routes.MapRoute(
          "default",
          "{controller=Home}/{action=Index}/{id?}");

        routes.MapSpaFallbackRoute(
          "spa-fallback",
          new {controller = "Home", action = "Index"});
      });
    }
  }
}