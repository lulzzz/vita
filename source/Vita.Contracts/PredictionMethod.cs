using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Vita.Contracts
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PredictionMethod
    {
      None,
      KeywordMatch,
      MultiClassClassifier,
      Regression
    }
}
