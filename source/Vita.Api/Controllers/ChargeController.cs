using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;
using NSwag.Annotations;
using Serilog;
using Serilog.Context;
using Vita.Contracts;
using Vita.Contracts.ChargeId;
using Vita.Contracts.SubCategories;
using Vita.Domain.Services.TextClassifiers;

namespace Vita.Api.Controllers
{
  [Produces("application/json")]
  [Route("[controller]")]
  public class ChargeController : Controller
  {
    public ITextClassifier TextClassifier { get; }

    public ChargeController(ITextClassifier textClassifier)
    {
      TextClassifier = textClassifier;
    }

    [HttpGet("classify/{searchPhrase}")]
    [SwaggerResponse(HttpStatusCode.OK,typeof(IEnumerable<SearchResponse>))]
    public async Task<IActionResult> Search(string searchPhrase)
    {
      var requestId = Guid.NewGuid();
      using (LogContext.PushProperty("searchPhrase", searchPhrase))
      using (LogContext.PushProperty("requestId", requestId))
      {
        try
        {
         var result = await TextClassifier.MatchMany(searchPhrase);
          var list = new List<SearchResponse>();
          foreach (var r in result)
          {
            var response = new SearchResponse()
            {
              Id = requestId,
              Where = r.Locality,
              What = r.TransactionType,
              When = r.TransactionDate,
              PaymentMethodType = r.PaymentMethodType,
              Who = r.Company,
              Why = r.Classifier,
              CreatedUtcDate = DateTime.UtcNow,
              IsChargeId = false
            };

            list.Add(response);
          }

          return Ok(list);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          Log.Warning(e, "classify error {searchPhrase}", searchPhrase);
          return NoContent();
        }
      }
    }

    [HttpGet("{chargeId}")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(Charge))]
    public async Task<IActionResult> Get(Guid chargeId)
    {

      var charge = new Charge
      {
        Id = chargeId,
        Category = CategoryType.BankingFinance,
        SubCategory = Categories.BankingFinance.AtmWithdrawals,
        BankName = "ANZ",
        PlaceId = "Google Place Id here",
        TransactionType = TransactionType.Debit,
        PaymentMethod = PaymentMethodType.CashWithdrawl
      };
      return Ok(charge);
    }


    [HttpPost("verify/{searchResponseId}")]
    public async Task<IActionResult> Verify(Guid searchResponseId, [FromBody] string searchPhrase)
    {
      return Ok();
    }
  }
}