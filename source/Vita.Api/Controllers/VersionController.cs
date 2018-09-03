using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Vita.Contracts;
using Vita.Predictor;

namespace Vita.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Produces("application/json")]
    [Route("[controller]")]
    public class VersionController : Controller
    {
        private readonly Predict _predict;
        private readonly ITextClassifier _textClassifier;

        public VersionController(Predict predict, ITextClassifier textClassifier)
        {
            _predict = predict;
            _textClassifier = textClassifier;
        }

        [HttpGet]
        public IActionResult Get()
        {
            Log.Debug("api version {time}", DateTime.UtcNow.ToShortTimeString());
            return Json(new
            {
                Version = AppVersion.Current,
                Timestamp = DateTime.UtcNow.ToShortTimeString()
            });
        }

        [HttpGet]
        public async Task<IActionResult> Warmup()
        {
            Log.Debug("Warmup {time}", DateTime.UtcNow.ToShortTimeString());
            var watch = Stopwatch.StartNew();
            await _predict.PredictAsync(new PredictionRequest());
            _textClassifier.UseCache = true;
            await _textClassifier.Match("coles");

            watch.Stop();

            string msg = ($"warmup took {watch.ElapsedMilliseconds} milliseconds");

            return Ok(msg);
        }
    }


    public class AppVersion
    {
        // private cache
        private static string _assemblyVersion;

        /// <summary>
        ///     Get the current app version
        /// </summary>
        public static string Current
        {
            get
            {
                if (string.IsNullOrEmpty(_assemblyVersion)) _assemblyVersion = GetVersionInformation();

                return _assemblyVersion;
            }

            set => _assemblyVersion = value;
        }

        //helper function that do reflection
        private static string GetVersionInformation()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return version.ToString(3);
        }
    }
}