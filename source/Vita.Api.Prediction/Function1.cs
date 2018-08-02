
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Vita.Contracts;
using Vita.Predictor;


namespace Vita.Api.Prediction
{
    public static class Function1
    {
        [FunctionName("Predict")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("Prediction request");

            StreamReader r = new StreamReader(req.Body);
            string content = await r.ReadToEndAsync();

            var request = JsonConvert.DeserializeObject<PredictionRequest>(content);
            var predict = new Vita.Predictor.Predict();
            var subcategory = await predict.PredictAsync(request);
            return new OkObjectResult($"{subcategory}");
        }
    }
}
