using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NSwag.Annotations;
using Serilog;
using Serilog.Context;
using Vita.Contracts;

namespace Vita.Api.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class PredictionController : Controller
    {
        private readonly IPredict _predictor;

        public PredictionController(IPredict predictor)
        {
            _predictor = predictor;
        }

        [HttpPost("predict/")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(string))]
        public async Task<IActionResult> Search(PredictionRequest request)
        {
            Guard.AgainstNull(request);
            if (request.Id == Guid.Empty) request.Id = Guid.NewGuid();

            using (LogContext.PushProperty("request", request.ToJson()))
            using (LogContext.PushProperty("requestId", request.Id))
            {
                try
                {
                    var result = await _predictor.PredictAsync(request);
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