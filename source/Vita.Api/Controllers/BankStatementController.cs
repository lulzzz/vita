﻿using System;
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
using BankLogin = Vita.Domain.BankStatements.Login.BankLogin;

namespace Vita.Api.Controllers
{
  [Produces("application/json")]
  [Route("[controller]")]
  public class BankStatementController : Controller
  {
    private readonly ICommandBus _bus;
    public IBankStatementService BankStatementService { get; }
    public IPredict Predict { get; }
    public ITextClassifier TextClassifier { get; }

    public BankStatementController(IBankStatementService bankStatementService,
      IPredict predict,
      ITextClassifier textClassifier,
      ICommandBus bus)
    {
      _bus = bus;
      BankStatementService = bankStatementService;
      Predict = predict;
      TextClassifier = textClassifier;
    }

    [HttpPost("classify/{bankName}")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<SearchResponse>))]
    public async Task<IActionResult> Classify(string bankName = "anz")
    {
      try
      {
        // command to extract
        var id = BankStatementId.NewComb();
        var cmd = new ExtractBankStatement1Command {AggregateId = id, SourceId = new SourceId(id.ToString())};
        // saga to predict, text match and populate read models / elastic search
        await _bus.PublishAsync(cmd, new CancellationToken(false));
      }
      catch (Exception ex)
      {
        Console.Write(ex.ToString());
      }

      var anz = new BankLogin("anz", "username", Environment.GetEnvironmentVariable("bankstatements-anz-test-username"),
        "password", Environment.GetEnvironmentVariable("bankstatements-anz-test-password"));
      var test = new BankLogin("bank_of_statements", "username", "12345678", "password", "TestMyMoney");
      BankLogin bank;

      switch (bankName)
      {
        case "anz":
          bank = anz;
          break;
        default:
          bank = test;
          break;
      }

      var result = await BankStatementService.LoginFetchAllAsync(bankName, bank);
      var request = result.ToPredictionRequests();
      var predictions = await Predict.PredictManyAsync(request);
      return Ok(predictions);
    }
  }
}