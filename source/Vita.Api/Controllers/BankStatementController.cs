using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Vita.Contracts;
using Vita.Contracts.ChargeId;
using Vita.Domain.BankStatements;
using Vita.Domain.Infrastructure;
using BankLogin = Vita.Domain.BankStatements.Login.BankLogin;

namespace Vita.Api.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class BankStatementController : Controller
    {
        public IBankStatementService BankStatementService { get; }
        public IPredict Predict { get; }
        public ITextClassifier TextClassifier { get; }

        public BankStatementController(IBankStatementService bankStatementService, IPredict predict,
            ITextClassifier textClassifier)
        {
            BankStatementService = bankStatementService;
            Predict = predict;
            TextClassifier = textClassifier;
        }

        [HttpPost("classify/{bankName}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<SearchResponse>))]
        public async Task<IActionResult> Classify(string bankName = "anz")
        {

            // command to extract

            // saga to predict

            // saga to text match


            var anz = new BankLogin("anz", "username", Environment.GetEnvironmentVariable("bankstatements-anz-test-username") , "password",Environment.GetEnvironmentVariable("bankstatements-anz-test-password"));
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