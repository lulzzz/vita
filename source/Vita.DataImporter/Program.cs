using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Autofac;
using MassTransit;
using Newtonsoft.Json;
using Serilog;
using Vita.Contracts;
using Vita.Domain.Charges;
using Vita.Domain.Classifiers;
using Vita.Domain.Companies;
using Vita.Domain.Infrastructure;
using Vita.Domain.Infrastructure.Modules;
using Vita.Domain.Localities;
using Vita.Domain.Places;
using Vita.Domain.Services.TextClassifiers.SpreadSheets;

namespace Vita.DataImporter
{
  internal class Program
  {
    private static IBusControl _busControl;
    private static IContainer _container;

    public const string Letter = "c"; // lowercase

    private static void Main(string[] args)
    {
      Console.WriteLine("Vita.DataImporter started...");

      AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

      JsonConvert.DefaultSettings = () => VitaSerializerSettings.Settings;

      var builder = new ContainerBuilder();
      builder.RegisterModule<LoggingModule>();
      builder.RegisterModule<CommonModule>();
      builder.RegisterConsumers(Assembly.GetExecutingAssembly());
      builder.RegisterType<DataImporter>().SingleInstance();

      try
      {
        #region Mass Transit

        builder.Register(ConfigureBus)
          .SingleInstance()
          .As<IBusControl>()
          .As<IBus>();

        _container = builder.Build();

        _busControl = _container.Resolve<IBusControl>();

        _busControl.Start();
        CleanDatabase();
        EnsureIndexes();
         
        _container.Resolve<DataImporter>().Execute(companies:true);
        //ExtractLocalities();
        ExtractCompanies();
        //ExtractKeywordClassifiers();
        //ExtractPlaces();
       // ExtractCharges();

        long count = 1;
        while (true)
        {
          if (count > 100000)
          {
            Console.Clear();
            count = 1;
          }

          Util.WaitFor(1);
          //Console.WriteLine($"{DateTime.Now.ToLongTimeString()}");
          // if (count % 100 == 0) Console.Write($".");
          count++;
        }

        Console.ReadLine();
        _busControl.Stop();

        #endregion
      }
      catch (Exception e)
      {
        Log.Error(e, "Error startup {err}", e.Message);
        throw;
      }
    }

    private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      Trace.TraceError("Unhandled error {0}", e.ExceptionObject);
    }

    private static void CleanDatabase()
    {
      //var x = _container.Resolve<IRepository<Locality>>().Find(x=>string.IsNullOrWhiteSpace(x.))
    }

    private static void EnsureIndexes()
    {
      _container.Resolve<IRepository<Charge>>().EnsureIndex(x => x.Id, true);
      _container.Resolve<IRepository<Charge>>().EnsureIndex(x => x.SearchPhrase, true);

      _container.Resolve<IRepository<Locality>>().EnsureIndex(x => x.Id, true);
      _container.Resolve<IRepository<Locality>>().EnsureIndex(x => x.PlaceId);
      _container.Resolve<IRepository<Locality>>().EnsureIndex(x => x.Suburb);
      _container.Resolve<IRepository<Locality>>().EnsureIndex(x => x.Postcode);

      _container.Resolve<IRepository<Company>>().EnsureIndex(x => x.CompanyName);

      _container.Resolve<IRepository<Place>>().EnsureIndex(x => x.Id, true);
      _container.Resolve<IRepository<Place>>().EnsureIndex(x => x.PlaceId, false);
    }

    private static void ExtractCompanies()
    {
      foreach (var item in CompanySpreadsheet.Import()
        .Where(x => x.CompanyName.ToLowerInvariant().StartsWith(Letter))
        //.Skip(000)
        //.Take(1000)
      )
      {
        item.Id = Guid.NewGuid();
        _busControl.Publish(new CompanyRequest
        {
          Company = item
        });

        _busControl.Publish(new GoogleApiSearchRequest
        {
          SearchTerm = item.CompanyName
        });
      }
    }

    private static void ExtractLocalities()
    {
      foreach (var item in new LocalitiesSpreadsheet().LoadData()
        //.Where(x => x.CompanyName.ToLowerInvariant().StartsWith(Letter))
        //.Skip(000)
        //.Take(1000)
      )
      {
        item.Id = Guid.NewGuid();
        _busControl.Publish(item);
      }
    }

    private static void ExtractKeywordClassifiers()
    {
      foreach (var item in new KeywordsSpreadsheet().LoadData()
        //.Where(x => x.CompanyName.ToLowerInvariant().StartsWith(Letter))
        //.Skip(000)
        //.Take(1000)
      )
      {
        item.Id = Guid.NewGuid();
        _busControl.Publish(new ClassifierRequest
        {
          Identifier = item
        });
      }
    }

    private static void ExtractPlaces()
    {
      var data = _container.Resolve<IRepository<GoogleApiSearchResponse>>().GetAll();
      //  .Find(x => x.Request != null && x.Request.SearchTerm.ToLowerInvariant().StartsWith(Letter))
      // .Take(5000)
      ;

      foreach (var x in data.ToList())
      foreach (var r in x.PlacesTextSearchResponse.Results)
        _busControl.Publish(new PlaceRequest
        {
          Place = new Place
          {
            Id = Guid.NewGuid(),
            CreatedUtc = DateTime.UtcNow,
            Name = r.Name,
            PlaceId = r.PlaceId,
            Types = r.Types,
            FormattedAddress = r.FormattedAddress
          }
        });
    }

    private static void ExtractCharges()
    {
      var data = _container.Resolve<IRepository<Charge>>().GetAll().AsQueryable()
        .OrderByDescending(x => x.ModifiedUtc);

      foreach (var item in data)
        _busControl.Publish(new ChargeRefreshRequest
        {
          Id = item.Id
        });
    }


    private static IBusControl ConfigureBus(IComponentContext context)
    {
      return Bus.Factory.CreateUsingRabbitMq(cfg =>
      {
        // cfg.UseSerilog();

        var host = cfg.Host(new Uri("rabbitmq://localhost"), h =>
        {
          h.Username("guest");
          h.Password("guest");
          h.Heartbeat(60);
        });
        //cfg.ReceiveEndpoint(host, "seed_queue", e => { e.Consumer<SeedHandler>(context); });
        //  cfg.ReceiveEndpoint(host, "googleapisearch_queue", e => { e.Consumer<GoogleApiSearchHandler>(context); });
        cfg.ReceiveEndpoint(host, "companies_queue", e => { e.Consumer<CompanyHandler>(context); });
        cfg.ReceiveEndpoint(host, "localities_queue", e => { e.Consumer<KeywordIdentifierHandler>(context); });
        cfg.ReceiveEndpoint(host, "keywordclassifier_queue", e => { e.Consumer<ClassifierHandler>(context); });
        cfg.ReceiveEndpoint(host, "charge_queue", e => { e.Consumer<ChargeSeedHandler>(context); });
        cfg.ReceiveEndpoint(host, "chargerefresh_queue", e => { e.Consumer<ChargeRefreshHandler>(context); });
        cfg.ReceiveEndpoint(host, "places_queue", e => { e.Consumer<PlaceHandler>(context); });
      });
    }
  }
}