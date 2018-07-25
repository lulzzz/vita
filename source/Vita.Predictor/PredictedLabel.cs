using Microsoft.ML.Runtime.Api;

namespace Vita.Predictor
{
    public class PredictedLabel
    {
      [ColumnName("PredictedLabel")]
      public string SubCategory;
    }
}
