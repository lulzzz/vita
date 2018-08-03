using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ML;
using Newtonsoft.Json;
using Vita.Contracts;
using Vita.Domain.BankStatements;
using Vita.Predictor;

namespace Vita.Api.Prediction
{
  public static class Function1
  {
    [FunctionName("Prediction")]
    public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
      HttpRequest req, TraceWriter log)
    {
      var r = new StreamReader(req.Body);
      log.Info("Prediction trigger function started...");
      var content = await r.ReadToEndAsync();
      log.Info(content);

      //if (typeof(Microsoft.ML.Runtime.Data.LoadTransform) == null ||
      //    typeof(Microsoft.ML.Runtime.Learners.LinearClassificationTrainer) == null ||
      //    typeof(Microsoft.ML.Runtime.Internal.CpuMath.SseUtils) == null)
      //{
      //  log.Info("Assemblies are NOT loaded correctly");
      //  return new BadRequestObjectResult("ML model failed to load");
      //}

      var request = JsonConvert.DeserializeObject<PredictionRequest>(content);
      
        var model = await PredictionModel.ReadAsync<BankStatementLineItem, PredictedLabel>(PredictionModelWrapper.GetModel());
        var predicted = model.Predict(BankStatementLineItem.ToBankStatementLineItem(request));

        //return predicted != null
        //  ? (ActionResult) new OkObjectResult(predicted.SubCategory)
        //  : new BadRequestObjectResult("prediction failed");

      return new BadRequestObjectResult("no dice");
    }
  }
}