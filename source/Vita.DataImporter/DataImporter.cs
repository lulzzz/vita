using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtensionMinder;
using MassTransit;
using Newtonsoft.Json;
using Serilog;
using Vita.Contracts;
using Vita.Domain.Charges;
using Vita.Domain.Classifiers;
using Vita.Domain.Companies;
using Vita.Domain.Infrastructure;
using Vita.Domain.Infrastructure.Importers;
using Vita.Domain.Localities;
using Vita.Domain.Services.TextClassifiers;
using Vita.Domain.Services.TextClassifiers.SpreadSheets;

namespace Vita.DataImporter
{
  public class DataImporter
  {
    private readonly IBusControl _busControl;
    private readonly ITextClassifier _textClassifier;
    private readonly IRepository<Classifier> _classifierRepository;
    private readonly IRepository<Company> _companiesRepository;
    private readonly IRepository<Locality> _localitiesRepository;
    private readonly IRepository<Charge> _chargesRepository;

    public DataImporter(IBusControl busControl, ITextClassifier textClassifier,
      IRepository<Classifier> classifierRepository, IRepository<Company> companiesRepository,
      IRepository<Locality> localitiesRepository, IRepository<Charge> chargesRepository)
    {
      _busControl = busControl;
      _textClassifier = textClassifier;
      _classifierRepository = classifierRepository;
      _companiesRepository = companiesRepository;
      _localitiesRepository = localitiesRepository;
      _chargesRepository = chargesRepository;
    }

    public void Execute(bool localities = false, bool companies = false, bool keywords = false, bool charges = false)
    {
      if (localities) ImportLocalities();
      if (companies) ImportCompanies();
      if (keywords) ImportKeywords();
      if (charges) AsyncUtil.RunSync(ImportCharges);
    }

    private async Task ImportCharges()
    {
      foreach (var item in PocketBookImporter.Import(@"C:/dev/vita/data/pocketbook-export-mckelt-20180422.csv"))
      {
        var found = _chargesRepository.Find(x => x.SearchPhrase == item.Description);
        var xxx = found.ToList();
        if (xxx.Any())
        {
          Log.Debug("charge skipped {desc}", item.Description);
          continue;
        }

        var result = await _textClassifier.Match(item.Description);

        Log.Debug("charge publish {desc}", item.Description);
        var dic = new Dictionary<string, string> {{item.GetType().FullName, JsonConvert.SerializeObject(item)}};
        Util.WaitFor(1);
        var charge = new Charge
        {
          Id = Guid.NewGuid(),
          AccountName = item.AccountName.ToLowerInvariant(),
          Category = result.Classifier.CategoryType,
          SubCategory = result.Classifier.SubCategory,
          SearchPhrase = result.SearchPhrase,
          Keywords = item.Tags,
          Notes = item.Notes,
          CreatedUtc = DateTime.UtcNow,
          BankName = item.Bank,
          JsonData = dic,
          TransactionUtcDate = Convert.ToDateTime(item.Date)
        };

        if (result.Locality != null) charge.LocalityId = result.Locality.Id;
        if (result.Company != null) charge.CompanyId = result.Company.Id;

        await _busControl.Publish(new ChargeSeedRequest
        {
          Charge = charge
        });
        _chargesRepository.Insert(charge);
      }
    }

    private void ImportKeywords()
    {
      var ks = new KeywordsSpreadsheet();

      foreach (var item in ks.LoadData())
      {
        var found = _classifierRepository.Find(x => x.SubCategory == item.SubCategory);
        var xxx = found.ToList();
        if (xxx.Any())
        {
          Log.Debug("KeywordClassifier skipped {desc}", item.SubCategory);
          continue;
        }

        Util.WaitFor();
        item.Id = Guid.NewGuid();
        Log.Information(item.ToString());
        _busControl.Publish(new ClassifierRequest
        {
          Identifier = item
        });

        _classifierRepository.Insert(item);
      }
    }

    /// <summary>
    /// DRGD - De-registered.
    //  EXAD - External administration(in receivership/liquidation).
    //  NOAC - Not active.
    //  NRGD - Not registered.
    //  PROV - Provisional (mentioned only under charges and refers
    //  to those which have not been fully registered).
    //  REGD – Registered.
    //  SOFF - Strike-off action in progress.
    //  DISS - Dissolved by Special Act of Parliament.
    //  DIV3 - Organisation Transferred Registration via DIV3.
    //  PEND - Pending - Schemes.
    /// </summary>
    private void ImportCompanies()
    {
      foreach (var item in CompanySpreadsheet.Import().Where(x=>x.Status ==""))
      {
        var found = _companiesRepository
          .Find(x => x.CompanyName == item.CompanyName && x.Status == "REGD");
        var xxx = found.ToList();
        if (xxx.Any())
        {
          Log.Debug("CompanyName skipped {desc}", item.CompanyName);
          continue;
        }

        Util.WaitFor(1);
        item.Id = Guid.NewGuid();
        Log.Information(item.ToString());
        _busControl.Publish(new CompanyRequest
        {
          Company = item.TrimAllStrings()
        });
        _companiesRepository.Insert(item);
      }
    }

    private void ImportLocalities()
    {
      var ss = new LocalitiesSpreadsheet();

      foreach (var item in ss.LoadData().Where(x => !string.IsNullOrEmpty(x.Suburb)))
      {
        var found = _localitiesRepository.Find(x =>
          !string.IsNullOrWhiteSpace(x.Suburb) && x.Suburb.Trim() == item.Suburb.Trim());
        var xxx = found.ToList();
        if (xxx.Any())
        {
          Log.Debug("Locality skipped {desc}", item.Suburb);
          continue;
        }

        item.Id = Guid.NewGuid();
        Log.Information(item.ToString());
        _busControl.Publish(new LocalityRequest
        {
          Locality = item.TrimAllStrings()
        });
        _localitiesRepository.Save(item);
      }
    }
  }
}