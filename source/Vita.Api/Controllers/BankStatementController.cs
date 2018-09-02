using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Core;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Vita.Contracts;
using Vita.Contracts.ChargeId;
using Vita.Domain.BankStatements;
using Vita.Domain.BankStatements.Commands;

namespace Vita.Api.Controllers
{
  [Produces("application/json")]
  [Route("[controller]")]
  public class BankStatementController : Controller
  {
    private readonly ICommandBus _bus;

    public BankStatementController(ICommandBus bus)
    {
      _bus = bus;
    }

    //[HttpPost("classify/{bankName}")]
      [HttpPost("classify/")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<SearchResponse>))]
    public async Task<IActionResult> Classify()
    {
        
      try
      {
        // command to extract
        var id = BankStatementId.NewComb();
        var cmd = new ExtractBankStatement1Command {AggregateId = id, SourceId = SourceId.New};
        // saga to predict, text match and populate read models / elastic search
        await _bus.PublishAsync(cmd, new CancellationToken(false));
      }
      catch (Exception ex)
      {
        Console.Write(ex.ToString());
      }

     
      //var predictions = await Predict.PredictManyAsync(request);
      return Ok("");
    }
  }
}