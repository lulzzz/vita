using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NSwag.Annotations;
using Serilog;
using Serilog.Context;
using Vita.Contracts;
using Vita.Contracts.ChargeId;
using Vita.Domain.BankStatements;
using Vita.Domain.Services.Predictions;
using Vita.Domain.Services.TextClassifiers;

namespace Vita.Api.Controllers
{
  [Produces("application/json")]
  [Route("[controller]")]
  public class PredictionController : Controller
  {
    private readonly IPredictor _predictor;

    public PredictionController(IPredictor predictor)
    {
      _predictor = predictor;
    }

    [HttpPost("predict/")]
    [SwaggerResponse(HttpStatusCode.OK,typeof(string))]
    public async Task<IActionResult> Search(PredictionRequest request)
    {
      Guard.AgainstNull(request);
      var requestId = Guid.NewGuid();
      using (LogContext.PushProperty("request", request.ToJson()))
      using (LogContext.PushProperty("requestId", requestId))
      {
        try
        {
          var li = new BankStatementLineItem();
          li.AccountName = request.AccountName;
          li.AccountNumber = request.AccountNumber;
          li.Description = request.Description;
          li.Bank = request.Bank;
          li.Notes = request.Notes;
          li.Tags = request.Tags;
          li.AccountType = request.AccountType;
          li.Amount = request.Amount;
          li.TransactionUtcDate = request.TransactionUtcDate;
          
          var result = await _predictor.PredictAsync(li);
          return Ok(result);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          Log.Warning(e, "PredictionController error {request}", request.ToJson());
          return NoContent();
        }
      }
    }

  }
}